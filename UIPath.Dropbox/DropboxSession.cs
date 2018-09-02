using Dropbox.Api;
using Dropbox.Api.Files;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UIPath.Dropbox
{
    public sealed class DropboxSession : IDropboxSession
    {
        private readonly DropboxClient _dropboxClient;

        // TODO: this is very limiting but should suffice for version 1.0
        // TODO: maybe improve with something like:
        // DropboxConfiguration dc = ...;
        // _dropboxClient = DropboxClientFactory.NewClient(dc);
        public DropboxSession(string authToken)
        {
            ValidateStringIsNotNullOrEmpty(authToken);

            _dropboxClient = new DropboxClient(authToken);
        }

        public async Task<IEnumerable<DropboxFileMetadata>> ListFolderContentAsync(string path, bool recursive, CancellationToken cancellationToken)
        {
            ValidatePathAndCancellationToken(path, cancellationToken);
            List<DropboxFileMetadata> folderContent = new List<DropboxFileMetadata>();

            ListFolderResult list = await _dropboxClient.Files.ListFolderAsync(path);

            //await Task.Factory.FromAsync(_dropboxClient.Files.BeginListFolder(path, recursive), _dropboxClient.Files.EndListFolder);

            folderContent.AddRange(list.Entries.Where(f => f.IsFile).Select(f => f.ToFileMetadata()));

            //foreach (var item in list.Entries.Where(i => i.IsFile))
            //{
            //    Console.WriteLine("F{0,8} {1}", item.AsFile.Size, item.Name);
            //}

            if (recursive)
            {
                foreach (var folder in list.Entries.Where(i => i.IsFolder))
                {
                    //Console.WriteLine("D  {0}/", item.Name);
                    folderContent.AddRange(await ListFolderContentAsync(folder.PathLower, recursive, cancellationToken));
                }
            }

            return null;
        }

        public async Task CreateFolderAsync(string path, CancellationToken cancellationToken)
        {
            ValidatePathAndCancellationToken(path, cancellationToken);

            await _dropboxClient.Files.CreateFolderV2Async(path);
        }

        public async Task CreateFileWithContentAsync(string path, string content, CancellationToken cancellationToken)
        {
            ValidatePathAndCancellationToken(path, cancellationToken);
            ValidateStringIsNotNullOrEmpty(content);

            using (var mem = new MemoryStream(Encoding.UTF8.GetBytes(content)))
            {
                await _dropboxClient.Files.UploadAsync(
                    path,
                    WriteMode.Overwrite.Instance,
                    body: mem
                );
            }
        }

        public async Task CreateEmptyFileAsync(string path, CancellationToken cancellationToken)
        {
            ValidatePathAndCancellationToken(path, cancellationToken);

            await _dropboxClient.Files.UploadAsync(path, WriteMode.Overwrite.Instance);
        }

        public async Task CopyAsync(string fromPath, string toPath, CancellationToken cancellationToken)
        {
            ValidatePathAndCancellationToken(fromPath, cancellationToken);
            ValidateStringIsNotNullOrEmpty(toPath);
            
            await _dropboxClient.Files.CopyV2Async(fromPath, toPath);
        }

        public async Task MoveAsync(string fromPath, string toPath, CancellationToken cancellationToken)
        {
            ValidatePathAndCancellationToken(fromPath, cancellationToken);
            ValidateStringIsNotNullOrEmpty(toPath);

            var x = await _dropboxClient.Files.MoveV2Async(fromPath, toPath);
        }

        public async Task DeleteAsync(string path, CancellationToken cancellationToken)
        {
            ValidatePathAndCancellationToken(path, cancellationToken);

            await _dropboxClient.Files.DeleteV2Async(path);
        }

        public async Task UploadAsync(string filePath, string uploadFolder, CancellationToken cancellationToken)
        {
            ValidatePathAndCancellationToken(filePath, cancellationToken);
            ValidateStringIsNotNullOrEmpty(uploadFolder);
            // TODO: validate format of uploadFolder to start with "/" ?

            //var fileMetadata = await _dropboxClient.Files.UploadAsync(filePath);

            using (FileStream stream = File.Open(filePath, FileMode.Open))
            {
                await _dropboxClient.Files.UploadAsync(
                    //uploadFolder + "/" + Path.GetFileName(filePath),
                    uploadFolder,
                    WriteMode.Overwrite.Instance,
                    body: stream
                );
            }
        }

        public async Task DownloadFileAsync(string path, string downloadFolder, CancellationToken cancellationToken)
        {
            ValidatePathAndCancellationToken(path, cancellationToken);

            // TODO: validate path refers to file
            var response = await _dropboxClient.Files.DownloadAsync(path);
            
            //using (var fileStream = File.Create(downloadFolder + "\\" + response.Response.Name))
            //{
            //    var contentStream = await response.GetContentAsStreamAsync();
            //    contentStream.CopyTo(fileStream);
            //}

            ulong fileSize = response.Response.Size;
            const int bufferSize = 1024 * 1024;

            var buffer = new byte[bufferSize];

            using (var stream = await response.GetContentAsStreamAsync())
            {
                using (var file = new FileStream(downloadFolder + "\\" + response.Response.Name, FileMode.OpenOrCreate))
                {
                    var length = stream.Read(buffer, 0, bufferSize);

                    while (length > 0)
                    {
                        file.Write(buffer, 0, length);
                        //var percentage = 100 * (ulong)file.Length / fileSize;
                        // Update progress bar with the percentage.
                        // progressBar.Value = (int)percentage
                        //Console.WriteLine(percentage);

                        length = stream.Read(buffer, 0, bufferSize);
                    }
                }
            }
        }

        public async Task DownloadFolderAsZipAsync(string path, string downloadFolder, string zipName, CancellationToken cancellationToken)
        {
            ValidatePathAndCancellationToken(path, cancellationToken);

            // TODO: validate path refers to folder; see if folder metadata gets folder size (which must be bellow 1GB) and total files in folder (which need to be under 10k)
            var response = await _dropboxClient.Files.DownloadZipAsync(path);
            var zipFileName = response.Response.Metadata.Name;

            if (!string.IsNullOrEmpty(zipName))
            {
                zipFileName = zipName;
            }

            using (var fileStream = File.Create(downloadFolder + "\\" + zipFileName))
            {
                var contentStream = await response.GetContentAsStreamAsync();
                contentStream.CopyTo(fileStream);
            }
        }

        #region Validators

        private void ValidatePathAndCancellationToken(string path, CancellationToken cancellationToken)
        {
            ValidateStringIsNotNullOrEmpty(path);
            ValidateCancellationToken(cancellationToken);
        }

        private void ValidateCancellationToken(CancellationToken cancellationToken)
        {
            if (cancellationToken == null)
            {
                throw new ArgumentNullException(nameof(cancellationToken));
            }
        }

        private void ValidateStringIsNotNullOrEmpty(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException(nameof(path));
            }
        }

        #endregion

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    _dropboxClient.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~DropboxSession() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}

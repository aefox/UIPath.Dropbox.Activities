using Dropbox.Api;
using Dropbox.Api.Files;
using System;
using System.IO;
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
            if (string.IsNullOrEmpty(authToken))
            {
                throw new ArgumentNullException(nameof(authToken));
            }

            _dropboxClient = new DropboxClient(authToken);
        }

        public async Task ListFolderContentAsync(string path, CancellationToken cancellationToken)
        {
            ValidatePathAndCancellationToken(path, cancellationToken);

            await _dropboxClient.Files.ListFolderAsync(path);
        }

        public async Task CreateFolderAsync(string path, CancellationToken cancellationToken)
        {
            await _dropboxClient.Files.CreateFolderV2Async(path);
        }

        public async Task CreateFileAsync(string path, string content, CancellationToken cancellationToken)
        {
            using (var mem = new MemoryStream(Encoding.UTF8.GetBytes(content)))
            {
                var updated = await _dropboxClient.Files.UploadAsync(
                    path,
                    WriteMode.Overwrite.Instance,
                    body: mem
                );
            }
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

        public async Task UploadAsync(string path, CancellationToken cancellationToken)
        {
            ValidatePathAndCancellationToken(path, cancellationToken);

            var fileMetadata = await _dropboxClient.Files.UploadAsync(path);
        }

        public async Task DownloadAsync(string path, CancellationToken cancellationToken)
        {
            ValidatePathAndCancellationToken(path, cancellationToken);

            // TODO: validate path refers to file
            await _dropboxClient.Files.DownloadAsync(path);
        }

        public async Task DownloadZipAsync(string path, CancellationToken cancellationToken)
        {
            ValidatePathAndCancellationToken(path, cancellationToken);

            // TODO: validate path refers to folder; see if folder metadata gets folder size (which must be bellow 1GB) and total files in folder (which need to be under 10k)
            await _dropboxClient.Files.DownloadZipAsync(path);
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

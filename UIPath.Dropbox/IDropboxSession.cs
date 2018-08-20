using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace UIPath.Dropbox
{
    // TODO: add documentation
    public interface IDropboxSession : IDisposable
    {
        // TODO: this will end up being huge if we start covering more dropbox api's
        // TODO: maybe drop this "parent" interface and go for DropboxSession.Files.CreateAsync, DropboxSession.Files.CreateEmptyAsync, DropboxSession.Folders.CreateAsync, etc
        Task<IEnumerable<DropboxFileMetadata>> ListFolderContentAsync(string path, bool recursive, CancellationToken cancellationToken);
        Task CreateFolderAsync(string path, CancellationToken cancellationToken);
        Task CreateEmptyFileAsync(string path, CancellationToken cancellationToken);
        Task CreateFileWithContentAsync(string path, string content, CancellationToken cancellationToken);
        Task CopyAsync(string fromPath, string toPath, CancellationToken cancellationToken);
        Task MoveAsync(string fromPath, string toPath, CancellationToken cancellationToken);
        Task DeleteAsync(string path, CancellationToken cancellationToken);
        Task UploadAsync(string filePath, string uploadFolder, CancellationToken cancellationToken);
        Task DownloadFileAsync(string path, CancellationToken cancellationToken);
        Task DownloadFolderAsZipAsync(string path, CancellationToken cancellationToken);
    }
}
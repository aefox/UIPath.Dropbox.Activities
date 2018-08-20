using System;
using System.Threading;
using System.Threading.Tasks;

namespace UIPath.Dropbox
{
    // TODO: add documentation
    public interface IDropboxSession : IDisposable
    {
        Task ListFolderContentAsync(string path, CancellationToken cancellationToken);
        Task CreateFolderAsync(string path, CancellationToken cancellationToken);
        Task CreateEmptyFileAsync(string path, CancellationToken cancellationToken);
        Task CreateFileWithContentAsync(string path, string content, CancellationToken cancellationToken);
        Task CopyAsync(string fromPath, string toPath, CancellationToken cancellationToken);
        Task MoveAsync(string fromPath, string toPath, CancellationToken cancellationToken);
        Task DeleteAsync(string path, CancellationToken cancellationToken);
        Task UploadAsync(string path, CancellationToken cancellationToken);
        Task DownloadFileAsync(string path, CancellationToken cancellationToken);
        Task DownloadFolderAsZipAsync(string path, CancellationToken cancellationToken);
    }
}
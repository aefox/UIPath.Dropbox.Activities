using System;
using System.Threading;
using System.Threading.Tasks;

namespace UIPath.Dropbox
{
    public interface IDropboxSession : IDisposable
    {
        // files client
        Task ListFolderContentAsync(string path, CancellationToken cancellationToken);
        Task CreateFolderAsync(string path, CancellationToken cancellationToken);
        Task CreateFileAsync(string path, string content, CancellationToken cancellationToken);
        Task CopyAsync(string fromPath, string toPath, CancellationToken cancellationToken);
        Task MoveAsync(string fromPath, string toPath, CancellationToken cancellationToken);
        Task DeleteAsync(string path, CancellationToken cancellationToken);
        Task UploadAsync(string path, CancellationToken cancellationToken);
        Task DownloadAsync(string path, CancellationToken cancellationToken);
        Task DownloadZipAsync(string path, CancellationToken cancellationToken);
    }
}
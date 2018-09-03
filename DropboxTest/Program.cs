using System.Threading.Tasks;
using UIPath.Dropbox;

namespace DropboxTest
{
    /// <summary>
    /// This is a console program which can be used to test out the UIPath.Dropbox.DropboxSession app connector.
    /// To use DropboxSession you'll need a dropbox auth token. Checkout README.md for details on how to get one.
    /// </summary>
    class Program
    {

        static void Main(string[] args)
        {
            var instance = new Program();

            var task = Task.Run(instance.Run);

            task.Wait();
        }

        private async Task Run()
        {
            var cancellationToken = new System.Threading.CancellationToken();

            // The first step in using the DropboxSession is to always instantiate it with an auth token, which is mandatory
            using (DropboxSession dropboxSession = new DropboxSession("<YOUR DROPBOX AUTH TOKEN HERE>"))
            {
                // Once initialization is done you can use the dropboxSession object to interact with Dropbox
                await dropboxSession.CreateFolderAsync("/demo1", cancellationToken);
                await dropboxSession.DeleteAsync("/demo3", cancellationToken);
            }
        }
    }
}

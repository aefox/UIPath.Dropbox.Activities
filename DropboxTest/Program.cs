using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dropbox.Api;
using Dropbox.Api.Files;

namespace DropboxTest
{
    class Program
    {

        static int Main(string[] args)
        {
            var instance = new Program();

            var task = Task.Run((Func<Task<int>>)instance.Run);

            task.Wait();

            return task.Result;
        }

        private async Task<int> Run()
        {
            try
            {
                //DropboxClientConfig clientConfig = new DropboxClientConfig("UIPath.PowerUp.Automation");
                //DropboxAppClient appClient = new DropboxAppClient("f7jp1uvyoyjc3ie", "h9vxyb1g6tklnau");

                //DropboxClient client = new DropboxClient("iHRAJv9oK7AAAAAAAAALcTQzwWMNf_iJr-FQocCPZghcDSQ51fzE8Wee8TNXo1ra");
                //var list = await client.Files.ListFolderAsync("");

                //return list.Entries.Count;

                using (var dbx = new DropboxClient("iHRAJv9oK7AAAAAAAAALcTQzwWMNf_iJr-FQocCPZghcDSQ51fzE8Wee8TNXo1ra"))
                {
                    var full = await dbx.Users.GetCurrentAccountAsync();
                    Console.WriteLine("{0} - {1}", full.Name.DisplayName, full.Email);

                    //await ListRootFolder(dbx);

                    //await Upload(dbx, "/test-folder", "test-file.txt", "file test content");

                    //await ListRootFolder(dbx);

                    await ListRootFolder(dbx, "/test-folder");

                    var x = await dbx.Files.GetMetadataAsync("/test-folder");
                    return 1;
                }
            }
            catch (Exception ex)
            {
                var x = ex;
                return 0;
            }
        }

        async Task ListRootFolder(DropboxClient dbx, string path = "")
        {
            var list = await dbx.Files.ListFolderAsync(path);

            // show folders then files
            foreach (var item in list.Entries.Where(i => i.IsFolder))
            {
                Console.WriteLine("D  {0}/", item.Name);
            }

            foreach (var item in list.Entries.Where(i => i.IsFile))
            {
                Console.WriteLine("F{0,8} {1}", item.AsFile.Size, item.Name);
            }
        }

        async Task Upload(DropboxClient dbx, string folder, string file, string content)
        {
            using (var mem = new MemoryStream(Encoding.UTF8.GetBytes(content)))
            {
                var updated = await dbx.Files.UploadAsync(
                    folder + "/" + file,
                    WriteMode.Overwrite.Instance,
                    body: mem);
                Console.WriteLine("Saved {0}/{1} rev {2}", folder, file, updated.Rev);
            }
        }

        async Task Download(DropboxClient dbx, string folder, string file)
        {
            using (var response = await dbx.Files.DownloadAsync(folder + "/" + file))
            {
                Console.WriteLine(await response.GetContentAsStringAsync());
            }
        }
    }
}

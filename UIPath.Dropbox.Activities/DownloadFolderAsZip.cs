using System;
using System.Activities;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using UIPath.Dropbox.Activities.Properties;

namespace UIPath.Dropbox.Activities
{
    public sealed class DownloadFolderAsZip : ContinuableAsyncCodeActivity
    {
        [RequiredArgument]
        [LocalizedCategory(nameof(Resources.Input))]
        [LocalizedDisplayName(nameof(Resources.FolderPath))]
        public InArgument<string> FolderPath { get; set; }

        // TODO: do we need InArgument<string> DownloadLocation? For a better customization/user experience most probably yes

        protected override async Task<Action<AsyncCodeActivityContext>> ExecuteAsync(AsyncCodeActivityContext context, CancellationToken cancellationToken)
        {
            PropertyDescriptor dropboxSessionProperty = context.DataContext.GetProperties()[WithDropboxSession.DropboxSessionPropertyName];
            IDropboxSession dropboxSession = dropboxSessionProperty?.GetValue(context.DataContext) as IDropboxSession;

            if (dropboxSession == null)
            {
                throw new InvalidOperationException(Resources.DropboxSessionNotFoundException);
            }

            await dropboxSession.DownloadFolderAsZipAsync(FolderPath.Get(context), cancellationToken);

            return (asyncCodeActivityContext) => { };
        }
    }
}

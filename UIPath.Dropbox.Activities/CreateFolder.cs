using System;
using System.Activities;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using UIPath.Dropbox.Activities.Properties;
using UIPath.Dropbox.Shared.Activities;

namespace UIPath.Dropbox.Activities
{
    public sealed class CreateFolder : ContinuableAsyncCodeActivity
    {
        [RequiredArgument]
        [LocalizedCategory(nameof(Resources.Input))]
        [LocalizedDisplayName(nameof(Resources.FolderPath))]
        public InArgument<string> FolderPath { get; set; }

        protected override async Task<Action<AsyncCodeActivityContext>> ExecuteAsync(AsyncCodeActivityContext context, CancellationToken cancellationToken)
        {
            PropertyDescriptor dropboxSessionFactory = context.DataContext.GetProperties()[WithDropboxSession.DropboxSessionPropertyName];
            IDropboxSession dropboxSession = dropboxSessionFactory?.GetValue(context.DataContext) as IDropboxSession;

            if (dropboxSession == null)
            {
                throw new InvalidOperationException(Resources.DropboxSessionNotFoundException);
            }

            await dropboxSession.CreateFolderAsync(FolderPath.Get(context), cancellationToken);

            return (asyncCodeActivityContext) => { };
        }
    }
}

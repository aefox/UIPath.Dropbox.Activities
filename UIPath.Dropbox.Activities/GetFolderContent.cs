using System;
using System.Activities;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using UIPath.Dropbox.Activities.Properties;
using UIPath.Dropbox.Shared.Activities;

namespace UIPath.Dropbox.Activities
{
    public sealed class GetFolderContent : ContinuableAsyncCodeActivity
    {
        [RequiredArgument]
        [LocalizedCategory(nameof(Resources.Input))]
        [LocalizedDisplayName(nameof(Resources.FolderPath))]
        public InArgument<string> FolderPath { get; set; }

        [LocalizedCategory(nameof(Resources.Options))]
        [LocalizedDisplayName(nameof(Resources.Recursive))]
        public bool Recursive { get; set; }

        [LocalizedCategory(nameof(Resources.Output))]
        [LocalizedDisplayName(nameof(Resources.Files))]
        public OutArgument<IEnumerable<DropboxFileMetadata>> Files { get; set; }

        protected override async Task<Action<AsyncCodeActivityContext>> ExecuteAsync(AsyncCodeActivityContext context, CancellationToken cancellationToken)
        {
            PropertyDescriptor dropboxSessionProperty = context.DataContext.GetProperties()[WithDropboxSession.DropboxSessionPropertyName];
            IDropboxSession dropboxSession = dropboxSessionProperty?.GetValue(context.DataContext) as IDropboxSession;

            if (dropboxSession == null)
            {
                throw new InvalidOperationException(Resources.DropboxSessionNotFoundException);
            }

            IEnumerable<DropboxFileMetadata> files = await dropboxSession.ListFolderContentAsync(FolderPath.Get(context), Recursive, cancellationToken);

            return (asyncCodeActivityContext) =>
            {
                Files.Set(asyncCodeActivityContext, files);
            };
        }
    }
}

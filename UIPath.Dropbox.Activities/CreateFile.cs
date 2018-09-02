using System;
using System.Activities;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using UIPath.Dropbox.Activities.Properties;
using UIPath.Dropbox.Shared.Activities;

namespace UIPath.Dropbox.Activities
{
    public sealed class CreateFile : ContinuableAsyncCodeActivity
    {
        [RequiredArgument]
        [LocalizedCategory(nameof(Resources.Input))]
        [LocalizedDisplayName(nameof(Resources.FilePath))]
        public InArgument<string> FilePath { get; set; }

        [LocalizedCategory(nameof(Resources.Input))]
        [LocalizedDisplayName(nameof(Resources.FileContent))]
        public InArgument<string> FileContent { get; set; }

        protected override async Task<Action<AsyncCodeActivityContext>> ExecuteAsync(AsyncCodeActivityContext context, CancellationToken cancellationToken)
        {
            PropertyDescriptor dropboxSessionFactory = context.DataContext.GetProperties()[WithDropboxSession.DropboxSessionPropertyName];
            IDropboxSession dropboxSession = dropboxSessionFactory?.GetValue(context.DataContext) as IDropboxSession;

            if (dropboxSession == null)
            {
                throw new InvalidOperationException(Resources.DropboxSessionNotFoundException);
            }

            var fileContent = FileContent.Get(context);

            if (string.IsNullOrEmpty(fileContent))
            {
                await dropboxSession.CreateEmptyFileAsync(FilePath.Get(context), cancellationToken);
            }
            else
            {
                await dropboxSession.CreateFileWithContentAsync(FilePath.Get(context), fileContent, cancellationToken);
            }

            return (asyncCodeActivityContext) => { };
        }
    }
}

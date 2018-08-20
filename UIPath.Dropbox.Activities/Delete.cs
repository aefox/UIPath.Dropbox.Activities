using System;
using System.Activities;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using UIPath.Dropbox.Activities.Properties;

namespace UIPath.Dropbox.Activities
{
    public sealed class DropboxDelete : ContinuableAsyncCodeActivity
    {
        [RequiredArgument]
        [LocalizedCategory(nameof(Resources.Input))]
        [LocalizedDisplayName(nameof(Resources.Path))]
        public InArgument<string> Path { get; set; }

        protected override async Task<Action<AsyncCodeActivityContext>> ExecuteAsync(AsyncCodeActivityContext context, CancellationToken cancellationToken)
        {
            PropertyDescriptor ftpSessionProperty = context.DataContext.GetProperties()[WithDropboxSession.DropboxSessionPropertyName];
            IDropboxSession dropboxSession = ftpSessionProperty?.GetValue(context.DataContext) as IDropboxSession;

            if (dropboxSession == null)
            {
                throw new InvalidOperationException(Resources.DropboxSessionNotFoundException);
            }

            await dropboxSession.DeleteAsync(Path.Get(context), cancellationToken);
            //await new DropboxSession("iHRAJv9oK7AAAAAAAAALcTQzwWMNf_iJr-FQocCPZghcDSQ51fzE8Wee8TNXo1ra").DeleteAsync("/test-folder/test-file.txt", cancellationToken);

            return (asyncCodeActivityContext) => { };
        }
    }

    //public class DropboxDelete : AsyncCodeActivity
    //{
    //    [Category("Input")]
    //    [RequiredArgument]
    //    public InArgument<double> FirstNumber { get; set; }

    //    [Category("Input")]
    //    public InArgument<double> SecondNumber { get; set; }

    //    [Category("Output")]
    //    public OutArgument<double> ResultNumber { get; set; }

    //    protected override IAsyncResult BeginExecute(AsyncCodeActivityContext context, AsyncCallback callback, object state)
    //    {
    //        return Task.Run(() => new DropboxSession("iHRAJv9oK7AAAAAAAAALcTQzwWMNf_iJr-FQocCPZghcDSQ51fzE8Wee8TNXo1ra").DeleteAsync("/test-folder/test-file.txt"));
    //    }

    //    protected override void EndExecute(AsyncCodeActivityContext context, IAsyncResult result)
    //    {
            
    //    }
    //}
}

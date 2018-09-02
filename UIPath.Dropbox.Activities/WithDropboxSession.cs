using System;
using System.Activities;
using System.Activities.Statements;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using UIPath.Dropbox.Activities.Properties;
using UIPath.Dropbox.Shared.Activities;

namespace UIPath.Dropbox.Activities
{
    public class WithDropboxSession : ContinuableAsyncNativeActivity
    {
        private IDropboxSession _dropboxSession;

        public static readonly string DropboxSessionPropertyName = "DropboxSession";

        [Browsable(false)]
        public ActivityAction<IDropboxSession> Body { get; set; }

        [RequiredArgument]
        [LocalizedCategory(nameof(Resources.Session))]
        [LocalizedDisplayName(nameof(Resources.AuthToken))]
        public InArgument<string> AuthToken { get; set; }

        public WithDropboxSession()
        {
            Body = new ActivityAction<IDropboxSession>()
            {
                Argument = new DelegateInArgument<IDropboxSession>(DropboxSessionPropertyName),
                Handler = new Sequence() { DisplayName = Resources.DefaultBodyName }
            };
        }

        protected override async Task<Action<NativeActivityContext>> ExecuteAsync(NativeActivityContext context, CancellationToken cancellationToken)
        {
            IDropboxSession dropboxSession = new DropboxSession(AuthToken.Get(context));

            return (nativeActivityContext) =>
            {
                if (Body != null)
                {
                    _dropboxSession = dropboxSession;
                    nativeActivityContext.ScheduleAction(Body, dropboxSession, OnCompleted, OnFaulted);
                }
            };
        }

        private void OnCompleted(NativeActivityContext context, ActivityInstance completedInstance)
        {
            if (_dropboxSession == null)
            {
                throw new InvalidOperationException(Resources.DropboxSessionNotFoundException);
            }

            _dropboxSession.Dispose();
        }

        private void OnFaulted(NativeActivityFaultContext faultContext, Exception propagatedException, ActivityInstance propagatedFrom)
        {
            PropertyDescriptor dropboxSessionProperty = faultContext.DataContext.GetProperties()[DropboxSessionPropertyName];
            IDropboxSession dropboxSession = dropboxSessionProperty?.GetValue(faultContext.DataContext) as IDropboxSession;

            dropboxSession?.Dispose();
        }
    }
}

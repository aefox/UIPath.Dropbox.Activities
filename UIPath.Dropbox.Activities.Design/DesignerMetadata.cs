using System;
using System.Activities.Presentation.Metadata;
using System.Activities.Presentation.View.OutlineView;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIPath.Dropbox.Activities;
using UIPath.Dropbox.Activities.Design.Properties;

namespace UIPath.Dropbox.Activities.Design
{
    public class DesignerMetadata : IRegisterMetadata
    {
        public void Register()
        {
            CategoryAttribute category = new CategoryAttribute(Resources.DropboxActivitiesCategory);

            AttributeTableBuilder builder = new AttributeTableBuilder();
            ShowPropertyInOutlineViewAttribute hideFromOutlineAttribute = new ShowPropertyInOutlineViewAttribute() { CurrentPropertyVisible = false, DuplicatedChildNodesVisible = false };

            //builder.AddCustomAttributes(typeof(WithDropboxSession), new DesignerAttribute(typeof(WithFtpSessionDesigner)));
            builder.AddCustomAttributes(typeof(WithDropboxSession), nameof(WithDropboxSession.Body), hideFromOutlineAttribute);

            builder.AddCustomAttributes(typeof(DropboxDelete), category);
            //builder.AddCustomAttributes(typeof(DirectoryExists), new CategoryAttribute(Resources.FTPActivitiesCategory));
            //builder.AddCustomAttributes(typeof(DownloadFiles), new CategoryAttribute(Resources.FTPActivitiesCategory));
            //builder.AddCustomAttributes(typeof(EnumerateObjects), new CategoryAttribute(Resources.FTPActivitiesCategory));
            //builder.AddCustomAttributes(typeof(FileExists), new CategoryAttribute(Resources.FTPActivitiesCategory));
            //builder.AddCustomAttributes(typeof(UploadFiles), new CategoryAttribute(Resources.FTPActivitiesCategory));
            builder.AddCustomAttributes(typeof(WithDropboxSession), category);

            MetadataStore.AddAttributeTable(builder.CreateTable());
        }
    }
}

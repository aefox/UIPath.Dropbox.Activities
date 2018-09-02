using System.Activities.Presentation.Metadata;
using System.Activities.Presentation.View.OutlineView;
using System.ComponentModel;
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
            
            builder.AddCustomAttributes(typeof(WithDropboxSession), nameof(WithDropboxSession.Body), hideFromOutlineAttribute);

            builder.AddCustomAttributes(typeof(Copy), category);
            builder.AddCustomAttributes(typeof(CreateFile), category);
            builder.AddCustomAttributes(typeof(CreateFolder), category);
            builder.AddCustomAttributes(typeof(Delete), category);
            builder.AddCustomAttributes(typeof(DownloadFile), category);
            builder.AddCustomAttributes(typeof(DownloadFolderAsZip), category);
            builder.AddCustomAttributes(typeof(GetFolderContent), category);
            builder.AddCustomAttributes(typeof(Move), category);
            builder.AddCustomAttributes(typeof(UploadFile), category);
            builder.AddCustomAttributes(typeof(WithDropboxSession), category);

            MetadataStore.AddAttributeTable(builder.CreateTable());
        }
    }
}

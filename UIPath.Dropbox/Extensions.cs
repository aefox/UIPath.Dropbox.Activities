using Dropbox.Api.Files;

namespace UIPath.Dropbox
{
    internal static class Extensions
    {
        public static DropboxFileMetadata ToFileMetadata(this Metadata meta)
        {
            FileMetadata fileMetadata = meta as FileMetadata;
            DropboxFileMetadata metadata = new DropboxFileMetadata();

            metadata.Size = fileMetadata.Size;
            metadata.Name = fileMetadata.Name;
            metadata.PathDisplay = fileMetadata.PathDisplay;
            metadata.PathLower = fileMetadata.PathLower;
            metadata.Modified = fileMetadata.ServerModified;

            return metadata;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UIPath.Dropbox
{
    public class DropboxFileMetadata
    {
        public string Name { get; set; }
        public string PathDisplay { get; set; }
        public string PathLower { get; set; }
        public ulong Size { get; set; }
        public DateTime Modified { get; set; }
    }
}

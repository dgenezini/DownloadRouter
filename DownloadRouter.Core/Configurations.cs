using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownloadRouter.Core
{
    public class DestinationMapping
    {
        public string NameFilter { get; set; }
        public string PathFilter { get; set; }
        public List<string> DestinationDirectories { get; set; }
    }

    public class Configurations
    {
        public string TeraCopyPath { get; set; }
        public string[] FilesFilter { get; set; }
        public string[] WatchFolders { get; set; }
        public string[] SourcesDirectories { get; set; }
        public List<DestinationMapping> DestinationMappings { get; set; }
    }
}

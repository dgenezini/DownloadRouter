using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownloadRouter
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
        public List<string> FilesFilter { get; set; }
        public List<string> SourcesDirectories { get; set; }
        public List<DestinationMapping> DestinationMappings { get; set; }
    }
}

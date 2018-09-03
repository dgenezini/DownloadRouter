using CommandLine;
using CommandLine.Text;
using System;
using System.Collections.Generic;
using System.Text;

namespace DownloadRouter
{
    class CmdParams
    {
        [Option('p', "Path", HelpText = "Directory or file path", Required = true)]
        public string Path { get; set; }
    }

}

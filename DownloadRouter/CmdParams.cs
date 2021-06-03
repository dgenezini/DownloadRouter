using CommandLine;

namespace DownloadRouter
{
    class CmdParams
    {
        [Option('p', "Path", HelpText = "Directory or file path", Required = true)]
        public string Path { get; set; }
    }

}

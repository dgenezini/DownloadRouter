using CommandLine;
using DownloadRouter.Core;
using System;

namespace DownloadRouter
{
    class Program
    {
        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<CmdParams>(args)
                .WithParsed<CmdParams>(options => Execute(options))
                .WithNotParsed(error => { });
        }

        private static void Execute(CmdParams options)
        {
            var RouteResults = Router.Route(options.Path);

            bool WaitKey = false;

            foreach (var RouteResult in RouteResults)
            {
                if (!RouteResult.Color.HasValue)
                {
                    Console.ResetColor();
                }
                else
                {
                    WaitKey = true;

                    Console.ForegroundColor = RouteResult.Color.Value;
                }

                Console.WriteLine(RouteResult.Message);
            }

            if (WaitKey)
            {
                Console.ReadKey();
            }
        }
    }
}

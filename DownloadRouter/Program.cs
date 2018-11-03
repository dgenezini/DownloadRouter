using CommandLine;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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
            var Configurarions = JsonConvert.DeserializeObject<Configurations>(File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Configurations.json")));

            if ((!string.IsNullOrEmpty(Configurarions.TeraCopyPath)) &&
                (!File.Exists(Path.Combine(Configurarions.TeraCopyPath))))
            {
                Console.WriteLine($"TeraCopy not found: {Configurarions.TeraCopyPath}");
                Console.ReadKey();
                return;
            }

            string ParentFolder = new DirectoryInfo(options.Path).Parent.FullName.TrimEnd(Path.AltDirectorySeparatorChar);

            IEnumerable<string> SourcesDirectories = Configurarions.SourcesDirectories
                .Select(a => a.TrimEnd(Path.AltDirectorySeparatorChar));

            if (SourcesDirectories.Contains(ParentFolder))
            {
                if (Directory.Exists(options.Path))
                {
                    List<string> FileEntries = new List<string>();

                    foreach (var filter in Configurarions.FilesFilter)
                    {
                        FileEntries.AddRange(Directory.GetFiles(options.Path, filter, SearchOption.TopDirectoryOnly));
                    }

                    if (FileEntries.Count() > 0)
                    {
                        foreach (string SourcePath in FileEntries)
                        {
                            Console.ResetColor();
                            Console.WriteLine($"Source: {SourcePath}");

                            try
                            {
                                var DestinationMapping = Configurarions.DestinationMappings
                                    .SingleOrDefault(a => SourcePath.Contains(a.NameFilter));

                                if (DestinationMapping != null) //Found mapping
                                {
                                    string parameters;

                                    string Filename = Path.GetFileName(SourcePath);

                                    for (int I = 0; I <= DestinationMapping.DestinationDirectories.Count() - 1; I++)
                                    {
                                        string DestinationPath = Path.Combine(DestinationMapping.DestinationDirectories[I], Filename);

                                        Console.ForegroundColor = ConsoleColor.Blue;
                                        Console.WriteLine($"Destination: {DestinationPath}");

                                        if (!string.IsNullOrEmpty(Configurarions.TeraCopyPath))
                                        {
                                            parameters = string.Format("Copy \"{0}\" \"{1}\"", SourcePath, DestinationMapping.DestinationDirectories[I]);

                                            System.Diagnostics.Process.Start(Configurarions.TeraCopyPath, parameters);
                                        }
                                        else
                                        {
                                            File.Copy(SourcePath, DestinationPath);
                                        }

                                        //Wait for teracopy to open, so it includes the next file in the same window
                                        System.Threading.Thread.Sleep(5000);
                                    }
                                }
                                else
                                {
                                    Console.ForegroundColor = ConsoleColor.Yellow;
                                    Console.WriteLine("No mapping defined for the directory");
                                    Console.ReadLine();
                                }
                            }
                            catch
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("Error while moving files");
                                Console.ReadLine();
                            }
                        }
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine($"No files to copy: {options.Path}");
                        Console.WriteLine($"Filter: {string.Join(';', Configurarions.FilesFilter)}");
                        Console.ReadLine();
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Directory does not exists: {options.Path}");
                    Console.ReadLine();
                }
            }
            else
            {
                Console.ResetColor();
                Console.WriteLine($"Directory not mapped: {ParentFolder}");
            }
        }
    }
}

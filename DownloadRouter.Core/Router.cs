using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DownloadRouter.Core
{
    public static class Router
    {
        public static IEnumerable<RouteResultEventArgs> Route(string path)
        {
            var routeResults = new List<RouteResultEventArgs>();

            var Configurarions = JsonConvert.DeserializeObject<Configurations>(File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Configurations.json")));

            if ((!string.IsNullOrEmpty(Configurarions.TeraCopyPath)) &&
                (!File.Exists(Path.Combine(Configurarions.TeraCopyPath))))
            {
                routeResults.Add(new RouteResultEventArgs() { Color = ConsoleColor.Red, Message = $"TeraCopy not found: {Configurarions.TeraCopyPath}" });

                return routeResults;
            }

            bool IsFile = File.Exists(path);
            string ParentFolder = new DirectoryInfo(path).Parent.FullName.TrimEnd(Path.AltDirectorySeparatorChar);

            IEnumerable<string> SourcesDirectories = Configurarions.SourcesDirectories
                .Select(a => a.TrimEnd(Path.AltDirectorySeparatorChar));

            if (SourcesDirectories.Contains(ParentFolder))
            {
                if (IsFile || (Directory.Exists(path)))
                {
                    List<string> FileEntries = new List<string>();

                    if (IsFile)
                    {
                        if (Configurarions.FilesFilter.Any(a => a.Equals(Path.GetExtension(path), StringComparison.OrdinalIgnoreCase)))
                        {
                            FileEntries.Add(path);
                        }
                    }
                    else
                    {
                        foreach (var filter in Configurarions.FilesFilter)
                        {
                            FileEntries.AddRange(Directory.GetFiles(path, $"*.{filter}", SearchOption.TopDirectoryOnly));
                        }
                    }

                    if (FileEntries.Count > 0)
                    {
                        foreach (string SourcePath in FileEntries)
                        {
                            routeResults.Add(new RouteResultEventArgs() { Message = $"Source: {SourcePath}" });

                            try
                            {
                                DestinationMapping DestinationMapping = null;

                                if (IsFile)
                                {
                                    string FileName = Path.GetFileName(SourcePath);

                                    foreach (var mapping in Configurarions.DestinationMappings
                                        .Where(a => !string.IsNullOrEmpty(a.NameFilter)))
                                    {
                                        if (FileName.Contains(mapping.NameFilter))
                                        {
                                            DestinationMapping = mapping;

                                            break;
                                        }
                                    }
                                }
                                else
                                {
                                    DestinationMapping = Configurarions.DestinationMappings
                                        .Where(a => !string.IsNullOrEmpty(a.PathFilter))
                                        .SingleOrDefault(a => SourcePath.Contains(a.PathFilter));
                                }

                                if (DestinationMapping != null) //Found mapping
                                {
                                    string parameters;

                                    string Filename = Path.GetFileName(SourcePath);

                                    int destDirCountCache = DestinationMapping.DestinationDirectories.Count - 1;
                                    for (int I = 0; I <= destDirCountCache; I++)
                                    {
                                        string DestinationPath = Path.Combine(DestinationMapping.DestinationDirectories[I], Filename);

                                        routeResults.Add(new RouteResultEventArgs() { Color = ConsoleColor.Blue, Message = $"Destination: {DestinationPath}" });

                                        if (!string.IsNullOrEmpty(Configurarions.TeraCopyPath))
                                        {
                                            parameters = string.Format("Copy \"{0}\" \"{1}\"", SourcePath, DestinationMapping.DestinationDirectories[I]);

                                            System.Diagnostics.Process.Start(Configurarions.TeraCopyPath, parameters);

                                            //Wait for teracopy to open, so it includes the next file in the same window
                                            System.Threading.Thread.Sleep(5000);
                                        }
                                        else
                                        {
                                            File.Copy(SourcePath, DestinationPath);
                                        }
                                    }
                                }
                                else
                                {
                                    routeResults.Add(new RouteResultEventArgs() { Color = ConsoleColor.Yellow, Message = "No mapping defined for the directory" });
                                }
                            }
                            catch
                            {
                                routeResults.Add(new RouteResultEventArgs() { Color = ConsoleColor.Red, Message = "Error while moving files" });
                            }
                        }
                    }
                    else
                    {
                        routeResults.Add(new RouteResultEventArgs() { Color = ConsoleColor.Yellow, Message = $"No files to copy: {path}. Filter: {string.Join(";", Configurarions.FilesFilter)}" });
                    }
                }
                else
                {
                    routeResults.Add(new RouteResultEventArgs() { Color = ConsoleColor.Red, Message = $"Directory does not exists: {path}" });
                }
            }
            else
            {
                routeResults.Add(new RouteResultEventArgs() { Message = $"Directory not mapped: {ParentFolder}" });
            }

            return routeResults;
        }
    }
}

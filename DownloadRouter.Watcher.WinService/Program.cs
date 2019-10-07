using System;
using Topshelf;

namespace DownloadRouter.Watcher.WinService
{
    class Program
    {
        static void Main(string[] args)
        {
            var rc = HostFactory.Run(x =>                                   
            {
                x.Service<WatcherService>(s =>                              
                {
                    s.ConstructUsing(name => new WatcherService());         
                    s.WhenStarted(tc => tc.Start());                        
                    s.WhenStopped(tc => tc.Stop());                         
                });
                x.RunAsLocalSystem();                                       

                x.SetDescription("Download Router Service");                
                x.SetDisplayName("Download Router");                        
                x.SetServiceName("Download Router");                                  
            });                                                             

            var exitCode = (int)Convert.ChangeType(rc, rc.GetTypeCode()); 
            Environment.ExitCode = exitCode;
        }
    }
}

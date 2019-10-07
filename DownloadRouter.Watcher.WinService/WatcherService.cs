using DownloadRouter.Core;
using DownloadRouter.Watcher.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DownloadRouter.Watcher.WinService
{
    public class WatcherService
    {
        private DownloadFolderWatcher _DownloadFolderWatcher;

        public void Start()
        {
            var Configurarions = JsonConvert.DeserializeObject<Configurations>(
                File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, 
                        "Configurations.json")));

            _DownloadFolderWatcher = new DownloadFolderWatcher(Configurarions.WatchFolders);
        }

        public void Stop()
        {

        }
    }
}

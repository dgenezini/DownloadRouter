﻿using DownloadRouter.Core;
using DownloadRouter.Watcher.Core;
using System;
using System.IO;
using System.Text.Json;

namespace DownloadRouter.Watcher.WinService
{
    public class WatcherService
    {
        private DownloadFolderWatcher _DownloadFolderWatcher;

        public void Start()
        {
            var Configurarions = JsonSerializer.Deserialize<Configurations>(
                File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                        "Configurations.json")));

            _DownloadFolderWatcher = new DownloadFolderWatcher(Configurarions.WatchFolders);
        }

        public void Stop()
        {

        }
    }
}

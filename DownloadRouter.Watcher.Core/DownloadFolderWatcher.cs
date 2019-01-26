using DownloadRouter.Core;
using System;
using System.Collections.Generic;
using System.IO;

namespace DownloadRouter.Watcher.Core
{
    public class DownloadFolderWatcher
    {
        private readonly FileSystemWatcher[] _watchers;
        public event EventHandler<IEnumerable<RouteResultEventArgs>> OnRoute;

        public DownloadFolderWatcher(string[] path)
        {
            _watchers = new FileSystemWatcher[path.Length];

            for (int I = 0; I < path.Length; I++)
            {
                _watchers[I] = new FileSystemWatcher
                {
                    Path = path[I],
                    NotifyFilter = NotifyFilters.CreationTime | NotifyFilters.FileName,
                    Filter = "*.*",
                };

                _watchers[I].Changed += new FileSystemEventHandler(OnChanged);
                _watchers[I].Renamed += new RenamedEventHandler(OnChanged);

                _watchers[I].EnableRaisingEvents = true;
            }
        }

        private void OnChanged(object source, FileSystemEventArgs e) => OnRoute?.Invoke(this, Router.Route(e.FullPath));
    }
}

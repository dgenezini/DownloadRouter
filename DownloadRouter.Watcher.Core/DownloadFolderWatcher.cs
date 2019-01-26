using DownloadRouter.Core;
using System.Collections.Generic;
using System.IO;

namespace DownloadRouter.Watcher.Core
{
    public class DownloadFolderWatcher
    {
        private FileSystemWatcher[] _watchers;
        public delegate void OnRouteEvent(IEnumerable<RouteResult> routeResults);

        public OnRouteEvent OnRoute { get; set; }

        public DownloadFolderWatcher(string[] path)
        {    
            _watchers = new FileSystemWatcher[path.Length];

            for (int I = 0; I < path.Length; I++)
            {
                _watchers[I] = new FileSystemWatcher();
                _watchers[I].Path = path[I];

                _watchers[I].NotifyFilter = NotifyFilters.CreationTime | 
                    NotifyFilters.FileName;
                _watchers[I].Filter = "*.*";

                _watchers[I].Changed += new FileSystemEventHandler(OnChanged);
                _watchers[I].Renamed += new RenamedEventHandler(OnChanged);

                _watchers[I].EnableRaisingEvents = true;
            }
        }

        private void OnChanged(object source, FileSystemEventArgs e)
        {
            var RouteResults = Router.Route(e.FullPath);

            OnRoute(RouteResults);
        }
    }
}

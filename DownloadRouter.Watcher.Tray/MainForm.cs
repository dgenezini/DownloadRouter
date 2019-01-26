using DownloadRouter.Core;
using DownloadRouter.Watcher.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace DownloadRouter.Watcher.Tray
{
    public partial class MainForm : Form
    {
        private DownloadFolderWatcher _DownloadFolderWatcher;

        public MainForm()
        {
            InitializeComponent();

            var Configurarions = JsonConvert.DeserializeObject<Configs>(File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Configurations.json")));

            _DownloadFolderWatcher = new DownloadFolderWatcher(Configurarions.WatchFolders);
            _DownloadFolderWatcher.OnRoute += (sender, routeResultEventArgs) =>
            {
                foreach (var RouteResult in routeResultEventArgs)
                {
                    if (!RouteResult.Color.HasValue)
                    {
                        notifyIcon1.ShowBalloonTip(0, "Download Router", RouteResult.Message, ToolTipIcon.Info);
                    }
                }
            };
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}

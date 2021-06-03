using DownloadRouter.Core;
using DownloadRouter.Watcher.Core;
using System;
using System.IO;
using System.Text.Json;
using System.Windows.Forms;

namespace DownloadRouter.Watcher.Tray
{
    public partial class MainForm : Form
    {
        private DownloadFolderWatcher _DownloadFolderWatcher;

        public MainForm()
        {
            InitializeComponent();

            var Configurarions = JsonSerializer.Deserialize<Configurations>(File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Configurations.json")));

            _DownloadFolderWatcher = new DownloadFolderWatcher(Configurarions.WatchFolders);
            _DownloadFolderWatcher.OnRoute += (sender, routeResults) =>
            {
                foreach (var RouteResult in routeResults)
                {
                    if (!RouteResult.Color.HasValue)
                    {
                        notifyIcon1.ShowBalloonTip(0, "Download Router", RouteResult.Message, ToolTipIcon.Info);
                    }
                }
            };
        }

        protected override void SetVisibleCore(bool value)
        {
            base.SetVisibleCore(false);
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}

using System;

namespace DownloadRouter.Core
{
    public class RouteResultEventArgs : EventArgs
    {
        public ConsoleColor? Color { get; set; }
        public string Message { get; set; }
    }
}

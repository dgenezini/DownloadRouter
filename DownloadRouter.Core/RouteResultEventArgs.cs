using System;
using System.Collections.Generic;
using System.Text;

namespace DownloadRouter.Core
{
    public class RouteResultEventArgs: EventArgs
    {
        public ConsoleColor? Color { get; set; }
        public string Message { get; set; }
    }
}

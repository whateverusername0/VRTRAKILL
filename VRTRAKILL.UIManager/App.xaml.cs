using System;
using System.Diagnostics;
using System.Windows;
using VRTRAKILL.UIManager.Model.Logging;

namespace VRTRAKILL.UIManager
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static ILogger Logger { get; private set; }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            if (Debugger.IsAttached)
            {
                Logger = new VSDebuggerLogger();
                Logger.Debug("Hello!");
            }

            var MainW = new View.MainWindow();
            MainW.Show();
        }
    }
}

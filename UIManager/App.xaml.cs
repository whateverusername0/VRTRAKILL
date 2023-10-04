using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows;

namespace UIManager
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static readonly string APPLICATIONPATH = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        public static Model.Config Config => Model.Config.GetInstance().Instance;

        public static Dictionary<string, ResourceDictionary> Themes = new Dictionary<string, ResourceDictionary>()
        {
            { "Light", null },
            { "Dark", null },
            { "ULTRAKILL", null }
        };

        private void Application_Startup(object sender, StartupEventArgs e)
        {


            new View.Windows.MainWindow().Show();
            View.Windows.MainWindow.Instance.MainFrame.Navigate(new View.Pages.Startup());
        }
    }
}

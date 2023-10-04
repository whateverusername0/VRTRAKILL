using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace UIManager.View.Pages
{
    /// <summary>
    /// Interaction logic for Startup.xaml
    /// </summary>
    public partial class Startup : Page
    {
        public static Startup Instance { get; private set; }
        public static StartupVM VMInstance { get; private set; }

        public Startup()
        {
            if (Instance == null) Instance = this;
            if (VMInstance == null) VMInstance = new StartupVM();
            this.DataContext = VMInstance;
            InitializeComponent();
        }
    }
    public class StartupVM : Model.MVVM.ViewModelBase
    {
        public string ULTRAKILLPath
        { get => App.Config.ULTRAKILLPath; set { App.Config.ULTRAKILLPath = value; OnPropertyChanged(); } }
    }
}

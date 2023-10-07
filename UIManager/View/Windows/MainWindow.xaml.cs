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

namespace UIManager.View.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MainWindow Instance { get; private set; }
        public static MainWindowVM VMInstance { get; private set; }
        public MainWindow()
        {
            if (Instance == null) Instance = this;
            if (VMInstance == null) VMInstance = new MainWindowVM();
            this.DataContext = VMInstance;
            InitializeComponent();
        }
    }
    public class MainWindowVM : Model.MVVM.ViewModelBase
    {
        private string _CurrentPageSource; public string CurrentPageSource
        {
            get => _CurrentPageSource;
            set
            {
                SetProperty(ref _CurrentPageSource, ref value);
                //MainWindow.Instance.MainFrame.Navigate(new Uri(value, UriKind.RelativeOrAbsolute));
            }
        }

    }
}

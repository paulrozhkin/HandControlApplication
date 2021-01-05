using System.Windows;
using HandControl.ViewModel;

namespace HandControl
{
    /// <summary>
    ///     Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MainWindowViewModel dataModel = new MainWindowViewModel();

        public MainWindow()
        {
            InitializeComponent();
            DataContext = dataModel;
        }
    }
}
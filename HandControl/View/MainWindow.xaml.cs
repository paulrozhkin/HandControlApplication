using System;
using System.Windows;
using HandControl.ViewModel;

namespace HandControl.View
{
    /// <summary>
    ///     Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(MainWindowViewModel mainWindowViewModel)
        {
            DataContext = mainWindowViewModel ?? throw new ArgumentNullException(nameof(mainWindowViewModel));
            InitializeComponent();
        }
    }
}
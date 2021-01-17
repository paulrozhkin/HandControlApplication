using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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

        private void NumberPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                // Use SelectionStart property to find the caret position.
                // Insert the previewed text into the existing text in the textbox.
                var fullText = textBox.Text.Insert(textBox.SelectionStart, e.Text);

                // If parsing is successful, set Handled to false
                e.Handled = !int.TryParse(fullText,
                    NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign,
                    CultureInfo.InvariantCulture, out var val);
            }
        }

        private void DelayPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                // Use SelectionStart property to find the caret position.
                // Insert the previewed text into the existing text in the textbox.
                var fullText = textBox.Text.Insert(textBox.SelectionStart, e.Text);

                // If parsing is successful, set Handled to false
                e.Handled = !double.TryParse(fullText,
                    NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign,
                    CultureInfo.InvariantCulture, out var val);
            }
        }

        private void Number_OnLostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                if (string.IsNullOrEmpty(textBox.Text))
                {
                    textBox.Text = "0";
                }
            }
        }
    }
}
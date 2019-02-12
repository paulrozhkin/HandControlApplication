using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using HandControl.Services;

namespace HandControl.View
{
    /// <summary>
    /// Логика взаимодействия для ServoControlRaw.xaml
    /// </summary>
    public partial class ServoControlRaw : Window
    {
        public CommunicationManager Communication { get; set; }

        public ServoControlRaw()
        {
            InitializeComponent();
            Communication = CommunicationManager.GetInstance();
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void Tim4Ch1Txt_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Tim4Ch1Slider != null)
            {
                TextBox myTextBox = sender as TextBox;
                string valText = (myTextBox.Text != "") ? myTextBox.Text : "0";
                int val = int.Parse(valText);
                if (val > 20000)
                {
                    Tim4Ch1Slider.Value = 20000;
                    myTextBox.Text = Convert.ToString(20000);
                }
                else
                {
                    Tim4Ch1Slider.Value = val;
                }
                
            }
        }

        private void Tim4Ch1Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Slider slider = sender as Slider;
            Tim4Ch1Txt.Text = slider.Value.ToString();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            uint value = Convert.ToUInt32(Tim4Ch1Slider.Value);
            Communication.ExecuteTheRaw(value);
            // CommunicationManager.ExecuteTheRaw(value);

        }
    }
}

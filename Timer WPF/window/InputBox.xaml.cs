using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Shapes;

namespace Timer_WPF.window
{
    /// <summary>
    /// Логика взаимодействия для InputBox.xaml
    /// </summary>
    public partial class InputBox : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private string prompt;
        private string response;

        public InputBox(string prompt)
        {
            InitializeComponent();
            DataContext = this;
            Prompt = prompt;
        }
        public string Prompt
        {
            get { return prompt; }
            set
            {
                prompt = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Prompt"));
            }
        }

        public string Response
        {
            get { return response; }
            set
            {
                response = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Response"));
            }
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        public static string Show(string prompt)
        {
            var dlg = new InputBox(prompt);
            if (dlg.ShowDialog() == true)
                return dlg.Response;
            return null;
        }
    }
}

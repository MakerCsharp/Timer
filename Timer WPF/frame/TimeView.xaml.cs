using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Threading;
using Timer_WPF.Class;
using Timer_WPF.window;

namespace Timer_WPF.frame
{
    /// <summary>
    /// Логика взаимодействия для TimeView.xaml
    /// </summary>
    public partial class TimeView : Page
    {
        private DispatcherTimer _timer;
        private System.Diagnostics.Stopwatch _stopwat;
        private bool _isRunning;
      

        TodoItem todo;
        public TimeView(TodoItem item)
        {
            InitializeComponent();
            todo = item;
            TaskName.Content = todo.Title.ToString();
            _stopwat = new System.Diagnostics.Stopwatch();
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += Timer_Tick;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (_isRunning)
            {
                var elapsedTime = _stopwat.Elapsed;
                label_timer.Content = elapsedTime.ToString("hh':'mm':'ss");
            }
        }


        private void button_start_Click(object sender, RoutedEventArgs e)
        {
            todo.Start();
            _stopwat.Start();
            _isRunning = true;
            _timer.Start();
            button_start.IsEnabled = false;
        }
        private void button_stop_Click(object sender, RoutedEventArgs e)
        {
            todo.Stop();
            _isRunning = false;
            label_timer.Content = "00:00:00";
            button_start.IsEnabled = true;
            //text_name_timer.IsEnabled = true;
            //button_review.IsEnabled = true;
            //button_old_review.IsEnabled = true;
            //text_name_timer.Clear();
            //_stopwatch.InputRating(InputBox.Show("please enter rating"));
            // Сброс таймера
            _stopwat.Reset();
            NavigationService?.GoBack();
        }
        private void button_pause_Click(object sender, RoutedEventArgs e) 
        { 

        }





    }
}

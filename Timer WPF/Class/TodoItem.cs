using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Timer_WPF.Class
{
   
    public enum TaskStatus
    {
        ВЫПОЛНЯЕТСЯ,
        ЗАВЕРШЕН,
        ОТМЕНЕН,
        ПРИОСТАНОВЛЕН
    }
    public class TodoItem : INotifyPropertyChanged
    {
       
        

        private string _title;
        private string _status;
        private DateTime _startTime;
        private TimeSpan _timeSpent;
        private DateTime? _stopTime;
        private string _about;
        public string Title
        {
            get => _title;
            set
            {
                if (_title != value)
                {
                    _title = value;
                    OnPropertyChanged(nameof(Title));
                }
            }
        }
        public string Status
        {
            get => _status;
            set
            {
                if (_status != value)
                {
                    _status = value;
                    OnPropertyChanged(nameof(Status));
                    if ((_status == "ЗАВЕРШЕН") || (_status == "ОТМЕНЕН"))
                    {
                        IsDeleted = true;
                    }
                }
            }
        }
        public string About
        {
            get => _about;
            set
            {
                if (_about != value)
                {
                    _about = value;
                    OnPropertyChanged(nameof(About));
                }
            }
        }
        public DateTime StartTime
        {
            get => _startTime;
            set
            {
                if (_startTime != value)
                {
                    _startTime = value;
                    OnPropertyChanged(nameof(StartTime));
                }
            }
        }
        public TimeSpan TimeSpent
        {
            get => _timeSpent;
            set
            {
                if (_timeSpent != value)
                {
                    _timeSpent = value;
                    OnPropertyChanged(nameof(TimeSpent));
                }
            }
        }
        public DateTime? StopTime
        {
            get => _stopTime; 
            private set
            {
                if(_stopTime != value)
                {
                    _stopTime = value;
                    OnPropertyChanged(nameof(StopTime));
                }
            }
        }
        private bool _isDeleted;
        public bool IsDeleted
        {
            get => _isDeleted;
            set
            {
                if (_isDeleted != value)
                {
                    _isDeleted = value;
                    OnPropertyChanged(nameof(IsDeleted));
                }
            }
        }
        public void Stop()
        {
            StopTime = System.DateTime.Now;

            TimeSpent = StopTime.Value - StartTime;
        }

        public void Start()
        {
            StartTime = System.DateTime.Now;
        }

        public void InputAbout(string value)
        {
            bool c = true;
            while (c)
            {
                try
                {
                    //Console.Write("Please input mark about your task:");
                    About = value;
                    c = false;
                }
                catch
                {
                    c = true;
                }
            }
        }
        // Свойство для загрузки значений из enum в ComboBox
        public ObservableCollection<string> Statuses { get; } = new ObservableCollection<string>(Enum.GetNames(typeof(TaskStatus)));

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

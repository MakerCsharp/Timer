using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Timer_WPF.Class
{
    public  class Appdata
    {
        private static Appdata _instance;
        public ObservableCollection<TodoItem> TodoItems { get; } = new ObservableCollection<TodoItem>();

        private Appdata() { }

        public static Appdata Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Appdata();
                }
                return _instance;
            }
        }
    }
}

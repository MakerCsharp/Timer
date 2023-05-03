using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Windows.Threading;
using OfficeOpenXml;
using Timer_WPF.Class;
using System.Linq;
using System.Threading.Tasks;
using TaskStatus = Timer_WPF.Class.TaskStatus;
using LicenseContext = OfficeOpenXml.LicenseContext;

namespace Timer_WPF.frame
{
    /// <summary>
    /// Логика взаимодействия для Page1.xaml
    /// </summary>
    public partial class Page1 : Page
    {
        
        private TodoItem _selectedTodoItem;
        private DispatcherTimer _timer;
        private string FilePath = "/Data.txt";
        private ObservableCollection<TodoItem> _todoItems;
        public Page1()
        {
            InitializeComponent();
            DataContext = this;
            _timer = new DispatcherTimer();
            _todoItems = Appdata.Instance.TodoItems;


            Application.Current.MainWindow.Closing += MainWindowClosing;
        }
        public ObservableCollection<TodoItem> TodoItems
        {
            get { return _todoItems; }
            set
            {
                _todoItems = value;
                OnPropertyChanged("TodoItems");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
       
        private void ButtonAddTask_Click(object sender, RoutedEventArgs e)
        {
            string taskTitle = TextBoxNewTask.Text;
            if (!string.IsNullOrEmpty(taskTitle))
            {
                TimeSpan span = new TimeSpan(0,0,0);
                TodoItem newTask = new TodoItem { Title = taskTitle, Status = TaskStatus.ПРИОСТАНОВЛЕН.ToString(),TimeSpent = span };
                TodoItems.Add(newTask);
                TextBoxNewTask.Text = string.Empty;
            }
        }
        private void LoadTasksFromFile()
        {
            //if (File.Exists(FilePath))
            //{
            //    using (var reader = new StreamReader(FilePath))
            //    {






            //        while (!reader.EndOfStream)
            //        {
            //            var line = reader.ReadLine();
            //            var values = line.Split(',');
            //            if (values.Length == 4)
            //            {
            //                var task = new TodoItem
            //                {
            //                    Title = values[0],
            //                    Status = values[1],
            //                    TimeSpent = values[2],
            //                    IsDeleted = bool.Parse(values[3])
            //                };
            //                if (!task.IsDeleted)
            //                {
            //                    TodoItems.Add(task);
            //                }
            //            }
            //        }
            //    }
            //}
        }
        private void SaveTasksToFile()
        {
            //using (var writer = new StreamWriter(FilePath))
            //{
            //    foreach (var task in TodoItems)
            //    {
            //        // Сохраняем только неудаленные задачи
            //        if (!task.IsDeleted)
            //        {
            //            writer.WriteLine($"{task.Title},{task.Status},{task.TimeSpent},{task.IsDeleted}");
            //        }
            //    }
            //    writer.Dispose();
            //}
        }

        /// <summary>
        /// переход на новую страницу
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TimerStart(object sender, RoutedEventArgs e)
        {
            // Получение ComboBox, на котором произошло событие
            ComboBox comboBox = (ComboBox)sender;

            // Получение выбранного элемента в ComboBox
            TodoItem selectedTodoItem = (TodoItem)comboBox.DataContext;

            // Проверка выбранного значения и выполнение перехода на другую страницу, если условие выполняется
            if (selectedTodoItem.Status == TaskStatus.ВЫПОЛНЯЕТСЯ.ToString())
            {
                var item = ((FrameworkElement)sender).DataContext as TodoItem;
                // Выполнение перехода на другую страницу
                NavigationService.Navigate(new TimeView(item));
                item.Status = TaskStatus.ПРИОСТАНОВЛЕН.ToString();

            }
        }

       
        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedTodoItem = (TodoItem)ListBoxTodo.SelectedItem;
        }
    
        private void MainWindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            SaveTasksToFile();
        }

        private void Button_Click_Add_to_new_file(object sender, RoutedEventArgs e)
        {
            try
            {
                DateTime today = DateTime.Today;

                // Выбор места для сохранения файла
                var saveFileDialog = new Microsoft.Win32.SaveFileDialog();
                saveFileDialog.Filter = "Excel files (*.xlsx)|*.xlsx";
                saveFileDialog.FileName = $"report_{today:yyyy-MM-dd}.xlsx";
                bool appendToFile = false;



                if (saveFileDialog.ShowDialog() == true)
                {
                    FileInfo file = new FileInfo(saveFileDialog.FileName);
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                    using (ExcelPackage package = new ExcelPackage(file))
                    {
                        // Проверяем, существует ли уже лист с именем "Report"
                        ExcelWorksheet worksheet = package.Workbook.Worksheets.FirstOrDefault(x => x.Name == "Report");

                        // Если лист не существует, создаем его
                        if (worksheet == null)
                        {
                            worksheet = package.Workbook.Worksheets.Add("Report");

                            // Настройка заголовков столбцов
                            worksheet.Cells[1, 1].Value = "Name";
                            worksheet.Cells[1, 2].Value = "Start Time";
                            worksheet.Cells[1, 3].Value = "Elapsed Time";
                            worksheet.Cells[1, 4].Value = "Stop Time";
                            worksheet.Cells[1, 5].Value = "About";
                            worksheet.Cells[1, 6].Value = "Status";
                        }
                        else
                        {
                            // Если лист существует, спрашиваем пользователя, хочет ли он добавить данные в существующий файл
                            MessageBoxResult result = System.Windows.MessageBox.Show("Файл уже существует. Хотите добавить данные в файл?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
                            if (result == MessageBoxResult.Yes)
                            {
                                appendToFile = true;
                            }
                        }

                        // Заполнение ячеек данными
                        int startRow = appendToFile ? worksheet.Dimension.End.Row + 1 : 2;
                        foreach (TodoItem stopwatch in _todoItems)
                        {
                            if (stopwatch.StartTime.Date == today)
                            {
                                worksheet.Cells[startRow, 1].Value = stopwatch.Title;
                                worksheet.Cells[startRow, 2].Value = stopwatch.StartTime;
                                worksheet.Cells[startRow, 3].Value = stopwatch.TimeSpent.TotalMinutes;
                                worksheet.Cells[startRow, 4].Value = stopwatch.StopTime;
                                worksheet.Cells[startRow, 5].Value = stopwatch.About;
                                worksheet.Cells[startRow, 6].Value = stopwatch.Status;
                                startRow++;
                            }
                        }

                        // Сохранение файла
                        package.Save();
                    }

                   MessageBox.Show("Файл успешно сохранен!", "Информация");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка: {ex.Message}", "Ошибка");
            }

        }

        private void Button_Click_Add_to_old_file(object sender, RoutedEventArgs e)
        {
            DateTime today = DateTime.Today;
            //Выбор файла Excel
            var openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Filter = "Excel files (*.xlsx)|*.xlsx";
            openFileDialog.Title = "Select file to add data";

            if (openFileDialog.ShowDialog() == true)
            {
                FileInfo file = new FileInfo(openFileDialog.FileName);
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                using (ExcelPackage package = new ExcelPackage(file))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.FirstOrDefault();

                    // Если лист не существует, создаем его
                    if (worksheet == null)
                    {
                        worksheet = package.Workbook.Worksheets.Add("Report");
                        worksheet.Cells[1, 1].Value = "Name";
                        worksheet.Cells[1, 2].Value = "Start Time";
                        worksheet.Cells[1, 3].Value = "Elapsed Time";
                        worksheet.Cells[1, 4].Value = "Stop Time";
                        worksheet.Cells[1, 5].Value = "About";
                        worksheet.Cells[1, 6].Value = "Status";
                    }

                    // Добавление новых строк в конец листа
                    int row = worksheet.Dimension.End.Row + 1;
                    foreach (TodoItem stopwatch in _todoItems)
                    {
                        if (stopwatch.StartTime.Date == today)
                        {
                            worksheet.Cells[row, 1].Value = stopwatch.Title;
                            worksheet.Cells[row, 2].Value = stopwatch.StartTime;
                            worksheet.Cells[row, 3].Value = stopwatch.TimeSpent.TotalMinutes;
                            worksheet.Cells[row, 4].Value = stopwatch.StopTime;
                            worksheet.Cells[row, 5].Value = stopwatch.About;
                            worksheet.Cells[row, 6].Value = stopwatch.Status;
                            row++;
                        }
                    }

                    // Сохранение файла
                    package.Save();
                }

                MessageBox.Show("В файл добавлено значение ", "Информация");
            }
        }


    }
    
}



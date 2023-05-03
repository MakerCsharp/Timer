using System.Diagnostics;
using System.Collections.Generic;
using System.IO;
using OfficeOpenXml;
using System.Xml.Linq;
using OfficeOpenXml.Style;

namespace Timer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var stopwat = new System.Diagnostics.Stopwatch();
            List<Stopwatch> stopwatches= new List<Stopwatch>();

            string input = " ";

            //закрытие программы 
            while (input!="exit")
            {
                Console.WriteLine("Enter 'start' to start a new stopwatch or 'report' to get today's report.");
                input = Console.ReadLine();

                //Выбор варианта 
                switch (input)
                {
                    case "start":
                        Console.Write("Enter a name for the stopwatch: ");
                        string name = Console.ReadLine();
                        Stopwatch stopwatc = new Stopwatch(name);
                        stopwatches.Add(stopwatc);
                        stopwat.Start();
                        while (true)
                            {
                                Console.Clear();
                                var elapsedTime = stopwat.Elapsed;
                                Console.WriteLine($"{elapsedTime.Hours:00}:{elapsedTime.Minutes:00}:{elapsedTime.Seconds:00}");

                            if (Console.KeyAvailable)
                                {
                                    var key = Console.ReadKey(true);
                                    if (key.Key == ConsoleKey.Escape)
                                    {
                                        stopwatc.Stop();
                                        stopwatc.InputRating();
                                        stopwatc.DisplayElapsedTime();
                                        stopwat.Stop();
                                        break; 
                                    }
                                }
                            System.Threading.Thread.Sleep(10);
                        }

                        break;
                     
                    case "report":

                        DateTime today = DateTime.Today;
                        Console.WriteLine("Generating report...");

                    
                        // Создаем новый файл Excel
                        string fileName = $"report_{today:yyyy-MM-dd}.xlsx";
                        FileInfo file = new FileInfo(fileName);
                        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                        using (ExcelPackage package = new ExcelPackage(file))
                        {
                            ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Report");

                            // Настройка заголовков столбцов
                            worksheet.Cells[1, 1].Value = "Name";
                            worksheet.Cells[1, 2].Value = "Start Time";
                            worksheet.Cells[1, 3].Value = "Elapsed Time";
                            worksheet.Cells[1, 4].Value = "Stop Time";
                            worksheet.Cells[1, 5].Value = "Rating";

                            // Заполнение ячеек данными
                            int row = 2;
                            foreach (Stopwatch stopwatch in stopwatches)
                            {
                                if (stopwatch.StartTime.Date == today)
                                {
                                    worksheet.Cells[row, 1].Value = stopwatch.Name;
                                    worksheet.Cells[row, 2].Value = stopwatch.StartTime;
                                    worksheet.Cells[row, 3].Value = stopwatch.ElapsedTime.TotalSeconds;
                                    worksheet.Cells[row, 4].Value = stopwatch.StopTime;
                                    worksheet.Cells[row, 5].Value = stopwatch.Rating;
                                    row++;
                                }
                            }

                            // Сохранение файла
                            package.Save();
                        }

                        Console.WriteLine($"Report saved to {fileName}");
                        break;
                }

            }

        }

        static void OutTime(Stopwatch stopwatch)
        {
                Console.WriteLine($"Name: {stopwatch.Name}, Elapsed Time: {stopwatch.ElapsedTime}");
        }
    }

    class Stopwatch
    {
        public string Name { get; }
        public DateTime StartTime { get; }
        public TimeSpan ElapsedTime { get; private set; }
        public DateTime? StopTime { get; private set; }
        public int  Rating { get; private set; }

        public Stopwatch(string Name)
        {
            this.Name = Name;

            StartTime = DateTime.Now;
        }

        public void Stop()
        {
            StopTime = DateTime.Now;

            ElapsedTime = StopTime.Value - StartTime;
        }

        public void InputRating()
        {
            bool c=true;
            while (c)
            {
                try
                {
                    Console.Write("Please input mark about your task:");
                    Rating = Convert.ToInt32(Console.ReadLine());
                    if (Rating > 0 && Rating <= 5)
                        c = false;
                    else
                        c = true;
                }
                catch
                {
                    Console.Clear();
                    c = false;
                }
            }
        }

        public void DisplayElapsedTime()
        {
            Console.WriteLine($"Stopwatch '{Name}' elapsed time: {ElapsedTime}");
        }



    }

}
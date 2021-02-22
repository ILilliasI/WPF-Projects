using System;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using System.Globalization;

namespace Stopwatch
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool IsTimerEnabled { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            RotateSecond(rotateSecond);
            RotateMinute(rotateMinute, rotateSecond);

            timer.Interval = new TimeSpan(0, 0, 0, 0, 1);
            timer.Tick += ShowTime;
        }

        /// <summary>
        /// Движение секундной стрелки секундомера.
        /// </summary>
        /// <param name="rotateSecond"></param>
        static void RotateSecond(RotateTransform rotateSecond)
        {
            CompositionTarget.Rendering += (ss, ee) =>
            {
                rotateSecond.Angle = 6 * (stopwatch.ElapsedMilliseconds / 1000.0);
            };
        }

        /// <summary>
        /// Движение минутной стрелки секундомера.
        /// </summary>
        /// <param name="rotateMinute"></param>
        private void RotateMinute(RotateTransform rotateMinute, RotateTransform rotateSecond)
        {
            CompositionTarget.Rendering += (ss, ee) =>
            {
                rotateMinute.Angle = stopwatch.ElapsedMilliseconds / 10000.0;
            };
        }

        private static readonly DispatcherTimer timer = new DispatcherTimer();
        private static readonly System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();

        /// <summary>
        /// Отображение отсчёта времени.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShowTime(object sender, EventArgs e)
        {
            Time.Text = stopwatch.Elapsed.ToString(@"hh\:mm\:ss\.fff", CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Включение/выключение секундомера.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StartStopButton_Click(object sender, RoutedEventArgs e)
        {
            if (!IsTimerEnabled)
            {
                IsTimerEnabled = true;
                stopwatch.Start();
                timer.Start();
                StartStopButton.Content = "Стоп";
                LapClearButton.Content = "Круг";
            }
            else
            {
                IsTimerEnabled = false;
                stopwatch.Stop();
                timer.Stop();
                StartStopButton.Content = "Старт";
                LapClearButton.Content = "Сброс";
            }
        }

        /// <summary>
        /// Сброс времени или фиксирование круга.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LapClearButton_Click(object sender, RoutedEventArgs e)
        {
            if (!IsTimerEnabled)
            {
                if (Time.Text == "00:00:00.000") return;
                stopwatch.Restart();
                stopwatch.Stop();
                TimeIntervalsList.Items.Add(Time.Text);
                TimeIntervalsList.Items.Add("---------------");
                Time.Text = "00:00:00.000";
            }
            else
            {
                TimeIntervalsList.Items.Add(Time.Text);
            }
        }
        
        /// <summary>
        /// Метод, выводящий информацию о макс., мин. и среднем временных интервалах.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InfoButton_Click(object sender, RoutedEventArgs e)
        {
            if (TimeIntervalsList.Items.Count == 0)
            {
                MessageBox.Show($"Вы ещё не зафиксировали ни один временной интервал!");
                return;
            }
            int cntTime = 0;
            double maxTime = 0;
            double minTime = -1;
            double sumTime = 0;

            foreach (string row in TimeIntervalsList.Items)
            {
                if (row != "---------------")
                {
                    cntTime++;
                    string[] timeArr = row.Split(":");
                    double currentLineTime = 0;
                    // Степень для перевода времени в секунды.
                    int pow60 = 2;

                    foreach (string timeString in timeArr)
                    {
                        StringBuilder sb = new StringBuilder(timeString);
                        char separator = Convert.ToChar(NumberFormatInfo.CurrentInfo.CurrencyDecimalSeparator);
                        // Замена точки на системный разделитель для double, чтобы привести к double дробную часть секунд.
                        sb.Replace('.', separator);
                        double.TryParse(sb.ToString(), out double timeValue);

                        currentLineTime += timeValue * Math.Pow(60, pow60--);
                    }
                    sumTime += currentLineTime;
                    if ((minTime == -1) || (currentLineTime < minTime)) minTime = currentLineTime;
                    if (currentLineTime > maxTime) maxTime = currentLineTime;
                }
            }
            MessageBox.Show($"Максимальное время: {maxTime:F3}\nМинимальное время: {minTime:F3}\nСреднее время: {(sumTime / cntTime):F3}");
        }
    }
}

using System;
using System.Collections.Generic;
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

namespace Clock
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            RotateSecond(rotateSecond);
            RotateMinute(rotateMinute, rotateSecond);
            RotateHour(rotateHour, rotateMinute, rotateSecond);
        }

        /// <summary>
        /// Движение секундной стрелки.
        /// </summary>
        /// <param name="rotateSecond"></param>
        static void RotateSecond(RotateTransform rotateSecond)
        {
            CompositionTarget.Rendering += (ss, ee) =>
            {
                DateTime dt = DateTime.Now;
                rotateSecond.Angle = 6 * (dt.Second + dt.Millisecond / 1000.0);
            };
        }

        /// <summary>
        /// Движение минутной стрелки.
        /// </summary>
        /// <param name="rotateMinute"></param>
        static void RotateMinute(RotateTransform rotateMinute, RotateTransform rotateSecond)
        {
            CompositionTarget.Rendering += (ss, ee) => {
                DateTime dt = DateTime.Now;
                rotateSecond.Angle = 6 * (dt.Second + dt.Millisecond / 1000.0);
                rotateMinute.Angle = 6 * dt.Minute + rotateSecond.Angle / 60;
            };
        }

        /// <summary>
        /// Движение часовой стрелки.
        /// </summary>
        /// <param name="rotateHour"></param>
        /// <param name="rotateMinute"></param>
        /// <param name="rotateSecond"></param>
        static void RotateHour(RotateTransform rotateHour, RotateTransform rotateMinute, RotateTransform rotateSecond)
        {
            CompositionTarget.Rendering += (ss, ee) => {
                DateTime dt = DateTime.Now;
                rotateSecond.Angle = 6 * (dt.Second + dt.Millisecond / 1000.0);
                rotateMinute.Angle = 6 * dt.Minute + rotateSecond.Angle / 60;
                rotateHour.Angle = 30 * (dt.Hour % 12) + rotateMinute.Angle / 12;
            };
        }
    }
}

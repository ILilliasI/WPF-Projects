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

namespace ButtonAndKeys
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        int buttonPosition;

        List<VerticalAlignment> possibleVerticalPositions = new List<VerticalAlignment>
        { 0, 0, (VerticalAlignment)2, (VerticalAlignment)2};

        List<HorizontalAlignment> possibleHorizontalPositions = new List<HorizontalAlignment>
        { 0, (HorizontalAlignment)2, (HorizontalAlignment)2, 0 };

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Left:
                    buttonPosition = (4 + --buttonPosition) % 4;
                    break;
                case Key.Right:
                    buttonPosition = (buttonPosition + 1) % 4;
                    break;
            }

            DontTouchMe.VerticalAlignment = possibleVerticalPositions[buttonPosition];
            DontTouchMe.HorizontalAlignment = possibleHorizontalPositions[buttonPosition];

        }
    }
}

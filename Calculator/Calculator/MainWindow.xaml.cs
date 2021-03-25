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

namespace Calculator
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Операнды.
        double saveNumb, newNumb;
        // Знак операции.
        string operSign = "";

        public MainWindow()
        {
            InitializeComponent();

            Result.IsReadOnly = true;

            ButPoint.Click += (ss, ee) =>
            {
                if ((Result.Text == (6.0 / 0).ToString()) || (Result.Text == (-6.0 / 0).ToString())
                 || (Result.Text == double.NaN.ToString()))
                    ButAC_Click(this, null);
                if (!Result.Text.Contains(","))
                    Result.Text = $"{Result.Text},";
            };

            ButPlusMinus.Click += (ss, ee) =>
            {
                if (double.TryParse(Result.Text, out newNumb))
                {
                    newNumb = newNumb * (-1);
                    Result.Text = newNumb.ToString();
                }
            };

            ButInverse.Click += (ss, ee) =>
            {
                if (double.TryParse(Result.Text, out newNumb))
                {
                    if (newNumb == 0) Result.Text = $"{double.NegativeInfinity}";
                    else Result.Text = $"{1 / newNumb}";
                }
            };

            ButSqrt.Click += (ss, ee) =>
            {
                // Если от отрицательного числа взять корень, получается не число, которое, если его не учитывать, влияет на корректность
                // работы калькулятора.
                if (double.TryParse(Result.Text, out newNumb) )
                {
                    if (newNumb < 0) Result.Text = $"{double.NaN}";
                    else Result.Text = $"{Math.Sqrt(newNumb)}";
                }
            };
        }

        /// <summary>
        /// Нажатие на кнопку "равно".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Equal_Click(object sender, RoutedEventArgs e)
        {
            if (operSign == "") return;
            if (!double.TryParse(Result.Text, out newNumb))
                return;
            else
                switch (operSign)
                {
                    case "+": saveNumb += newNumb; break;
                    case "-": saveNumb -= newNumb; break;
                    case "*": saveNumb *= newNumb; break;
                    case "/": saveNumb /= newNumb; break;
                }
            Result.Text = $"{saveNumb}";
            operSign = "";
        }

        /// <summary>
        /// Нажатие на кнопку со знаком операции.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Operator_Click(object sender, RoutedEventArgs e)
        {
            if ((Result.Text == (6.0 / 0).ToString()) || (Result.Text == (-6.0 / 0).ToString())
                || (Result.Text == double.NaN.ToString()))
            {
                ButAC_Click(this, null);
            }

            if (double.TryParse(Result.Text, out newNumb))
            {
                saveNumb = newNumb;
                Result.Text = "0";
            }
            else return;

            Button but = (Button)sender;
            operSign = but.Content.ToString();
        }

        /// <summary>
        /// Очистка поля для ввода.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButAC_Click(object sender, RoutedEventArgs e)
        {
            Result.Text = "0";
            saveNumb = newNumb = 0;
        }

        /// <summary>
        /// Нажатие на кнопку с числом.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Number_Click(object sender, RoutedEventArgs e)
        {
            if ((Result.Text == (6.0 / 0).ToString()) || (Result.Text == (-6.0 / 0).ToString())
                 || (Result.Text == double.NaN.ToString()))
            {
                ButAC_Click(this, null);
            }

            // Очередная цифра.
            int newNumber = 0;
            Button but = (Button)sender;
            switch (but.Name)
            {
                case "But0": newNumber = 0; break;
                case "But1": newNumber = 1; break;
                case "But2": newNumber = 2; break;
                case "But3": newNumber = 3; break;
                case "But4": newNumber = 4; break;
                case "But5": newNumber = 5; break;
                case "But6": newNumber = 6; break;
                case "But7": newNumber = 7; break;
                case "But8": newNumber = 8; break;
                case "But9": newNumber = 9; break;
            }

            if (Result.Text == "0") Result.Text = $"{newNumber}";
            else Result.Text = $"{Result.Text}{newNumber}";
        }
    }
}

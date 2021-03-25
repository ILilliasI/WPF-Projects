using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace GaussFunction
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        double W = double.NaN; // Ширина холста.
        double H = double.NaN; // Высота холста.
        Point pixMin, pixMax; // Аппаратные координаты опорных точек.
        Point pointMin, pointMax; // Мировые координаты опорных точек.

        public MainWindow()
        {
            InitializeComponent();
            InitializeFields();

            DrawXCoordinates();
            DrawYCoordinates();
            DrawLegend();
            AddGraph();
            AddArea();
        }

        /// <summary>
        /// Иницилизация полей класса.
        /// </summary>
        private void InitializeFields()
        {
            W = canvas.Width;
            H = canvas.Height;
            pixMin = new Point(W / 20, H / 20 + 0.9 * H);
            pixMax = new Point(W / 20 + 0.9 * W, H / 20);
            pointMin = new Point(-5, 0);
            pointMax = new Point(5, 1);
        }

        /// <summary>
        /// Вычисление значений функции Гаусса.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="mu"></param>
        /// <param name="sigma"></param>
        /// <returns></returns>
        double Gauss(double x, double mu = 0, double sigma = 1)
        {
            double z = sigma * Math.Sqrt(2 * Math.PI);
            double term = Math.Exp(-(x - mu) * (x - mu) / (2 * sigma * sigma));
            return term / z;
        }

        /// <summary>
        /// Отрисовка легенды.
        /// </summary>
        void DrawLegend()
        {
            TextBox legend = new TextBox();
            legend.Width = 140;
            legend.Height = 70;
            legend.BorderBrush = Brushes.Cyan;
            legend.BorderThickness = new Thickness(1);
            legend.IsReadOnly = true;
            legend.IsHitTestVisible = false;
            string nl = Environment.NewLine;
            legend.Text = $"\u03BC=0,  \u03C3\u00B2=0.2{nl}\u03BC=0,  \u03C3\u00B2=1.0" +
                          $"{nl}\u03BC=0,  \u03C3\u00B2=5.0{nl}\u03BC=-2,  \u03C3\u00B2=0.5";
            canvas.Children.Add(legend);
            Canvas.SetTop(legend, H / 15);
            Canvas.SetRight(legend, W / 15);

            Line l02 = new Line();
            l02.Stroke = Brushes.Red;
            l02.StrokeThickness = 2;
            l02.X1 = W * 85/ 100;
            l02.Y1 = H / 11;
            l02.X2 = W * 9 / 10;
            l02.Y2 = H / 11;
            canvas.Children.Add(l02);

            Line l10 = new Line();
            l10.Stroke = Brushes.Green;
            l10.StrokeThickness = 2;
            l10.X1 = W * 85 / 100;
            l10.Y1 = H / 11 + 16;
            l10.X2 = W * 9 / 10;
            l10.Y2 = H / 11 + 16;
            canvas.Children.Add(l10);

            Line l50 = new Line();
            l50.Stroke = Brushes.Blue;
            l50.StrokeThickness = 2;
            l50.X1 = W * 85 / 100;
            l50.Y1 = H / 11 + 32;
            l50.X2 = W * 9 / 10;
            l50.Y2 = H / 11 + 32;
            canvas.Children.Add(l50);

            Line l05 = new Line();
            l05.Stroke = Brushes.Fuchsia;
            l05.StrokeThickness = 2;
            l05.X1 = W * 85 / 100;
            l05.Y1 = H / 11 + 48;
            l05.X2 = W * 9 / 10;
            l05.Y2 = H / 11 + 48;
           canvas.Children.Add(l05);
        }

        /// <summary>
        /// Перевод значения абсциссы в пиксели.
        /// </summary>
        /// <param name="xPixMin"></param>
        /// <param name="xPixMax"></param>
        /// <param name="x"></param>
        /// <param name="xMin"></param>
        /// <param name="xMax"></param>
        /// <returns></returns>
        public static double XDoubleToPix(double xPixMin, double xPixMax,
        double x, double xMin, double xMax)
        {
            double delPix = xPixMax - xPixMin;
            double delX = xMax - xMin;
            double xPix = xPixMin + (x - xMin) * delPix / delX;
            return xPix;
        }

        /// <summary>
        /// Перевод значения ординаты в пиксели.
        /// </summary>
        /// <param name="yPixMin"></param>
        /// <param name="yPixMax"></param>
        /// <param name="y"></param>
        /// <param name="yMin"></param>
        /// <param name="yMax"></param>
        /// <returns></returns>
        public static double YDoubleToPix(double yPixMin, double yPixMax,
        double y, double yMin, double yMax)
        {
            double delPix = yPixMin - yPixMax;
            double delY = yMin - yMax;
            double yPix = yPixMin + (y - yMin) * delPix / delY;
            return yPix;
        }

        /// <summary>
        /// Перевод координат точки в аппаратные координаты.
        /// </summary>
        /// <param name="ptPixMin"></param>
        /// <param name="ptPixMax"></param>
        /// <param name="pt"></param>
        /// <param name="ptMin"></param>
        /// <param name="ptMax"></param>
        /// <returns></returns>
        public static Point PointDoubleToPix(Point ptPixMin, Point ptPixMax,
        Point pt, Point ptMin, Point ptMax)
        {
            double xRes = XDoubleToPix(ptPixMin.X, ptPixMax.X, pt.X, ptMin.X, ptMax.X);
            double yRes = YDoubleToPix(ptPixMin.Y, ptPixMax.Y, pt.Y, ptMin.Y, ptMax.Y);
            return new Point(xRes, yRes);
        }

        /// <summary>
        /// Формирование области графиков.
        /// </summary>
        private void AddArea()
        {
            double W = canvas.Width;
            double H = canvas.Height;
            rect.Stroke = Brushes.Black;
            rect.Width = 0.9 * W;
            rect.Height = 0.9 * H;
            Canvas.SetTop(rect, H / 20);
            Canvas.SetLeft(rect, W / 20);
        }

        /// <summary>
        /// Формирование и визуализация кривых Гаусса.
        /// </summary>
        private void AddGraph()
        {
            Polyline pl02 = new Polyline();
            pl02.Stroke = Brushes.Red;
            Polyline pl10 = new Polyline();
            pl10.Stroke = Brushes.Green;
            Polyline pl50 = new Polyline();
            pl50.Stroke = Brushes.Blue;
            Polyline pl05 = new Polyline();
            pl05.Stroke = Brushes.Fuchsia;

            double delX = (pointMax.X - pointMin.X) / 100;
            for (double x = pointMin.X; x < pointMax.X; x += delX)
            {
                double y = Gauss(x);
                Point pt10 = PointDoubleToPix(pixMin, pixMax,
                new Point(x, y), pointMin, pointMax);
                pl10.Points.Add(pt10);

                y = Gauss(x, 0, Math.Sqrt(0.2));
                Point pt02 = PointDoubleToPix(pixMin, pixMax,
                new Point(x, y), pointMin, pointMax);
                pl02.Points.Add(pt02);

                y = Gauss(x, 0, Math.Sqrt(5.0));
                Point pt50 = PointDoubleToPix(pixMin, pixMax,
                new Point(x, y), pointMin, pointMax);
                pl50.Points.Add(pt50);


                y = Gauss(x, -2, Math.Sqrt(0.5));
                Point pt05 = PointDoubleToPix(pixMin, pixMax,
                new Point(x, y), pointMin, pointMax);
                pl05.Points.Add(pt05);
            }

            canvas.Children.Add(pl02);
            canvas.Children.Add(pl50);
            canvas.Children.Add(pl10);
            canvas.Children.Add(pl05);
        }

        /// <summary>
        /// Отрисовка координатной сетки ординат.
        /// </summary>
        void DrawYCoordinates()
        {
            double delY = (pointMax.Y - pointMin.Y) / 10;
            double xLpix = W * 0.05 - 5, yLpix;
            for (double dy = pointMin.Y; dy <= pointMax.Y + 0.1; dy += delY)
            {
                yLpix = YDoubleToPix(pixMin.Y, pixMax.Y, dy, pointMin.Y, pointMax.Y);
                Line lineY = new Line();
                lineY.Stroke = Brushes.Black;
                lineY.StrokeDashArray =
                new DoubleCollection(new double[2] { 1, 2 });
                lineY.X1 = xLpix;
                lineY.Y1 = yLpix;
                lineY.X2 = 0.95 * W;
                lineY.Y2 = yLpix;
                canvas.Children.Add(lineY);
                TextBlock tby = new TextBlock();
                tby.Text = $"{dy}";
                Canvas.SetLeft(tby, xLpix - 15);
                Canvas.SetTop(tby, yLpix - 10);
                canvas.Children.Add(tby);
            }
        }

        /// <summary>
        /// Отрисовка координатной сетки абсцисс.
        /// </summary>
        void DrawXCoordinates()
        {
            double delX = (pointMax.X - pointMin.X) / 10;
            double yPix = H * 0.95, xPix;
            for (double dx = pointMin.X; dx <= pointMax.X + 0.1; dx += delX)
            {
                xPix = XDoubleToPix(pixMin.X, pixMax.X, dx, pointMin.X, pointMax.X);
                Line llineX = new Line();
                llineX.Stroke = Brushes.Black;
                llineX.StrokeDashArray =
                new DoubleCollection(new double[2] { 1, 2 });
                llineX.X1 = xPix;
                llineX.Y1 = H / 20;
                llineX.X2 = xPix;
                llineX.Y2 = yPix + 5;
                canvas.Children.Add(llineX);

                TextBlock tbx = new TextBlock();
                tbx.Text = $"{dx}";
                Canvas.SetLeft(tbx, xPix - 5);
                Canvas.SetTop(tbx, yPix + 5);
                canvas.Children.Add(tbx);
            }
        }
    }
}

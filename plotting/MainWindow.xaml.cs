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

namespace plotting
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public List<Point> Points { get; set; } = RandomEvolution();

        static Random rng = new Random();

        /// <summary>
        ///maximum value change per tick
        ///this maximum is hit rarely since the random change distribution is gaussian
        ///eg. 0.01 means 1%
        /// </summary>
        static readonly double ValueChangeRate = 0.0105;
        static readonly double GrowthChangeRate = 0.00001;

        static double Grow = 0;
        static double Value = 100;


        /// random number between (-1,1), gaussian distribution
        static double RandGaussian() => (rng.NextDouble() - rng.NextDouble()) * rng.NextDouble();

        public static List<Point> RandomEvolution()
        {
            var points = new List<Point> { new Point(Grow,Value)};

            var rate= Grow;
            var val =Value;

            for (int i = 0; i < 1000; i++)
            {
                rate = Math.Tanh(rate + (RandGaussian() + rate) * GrowthChangeRate);
                val *= 1 + (RandGaussian() + rate) * ValueChangeRate;
                points.Add(new Point(i, val));
            }

            return points;
        }
        public MainWindow()
        {
            InitializeComponent();
            keepUpdating();
        }
        async Task keepUpdating()
        {
            while (true)
            { 
                Points = RandomEvolution();
                plott.ItemsSource = null;
                plott.ItemsSource = Points;
                await Task.Delay(5000);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FaceTrackingBasics
{
    /// <summary>
    /// Interaction logic for Aplikacje.xaml
    /// </summary>
    public partial class Aplikacje : Window
    {
        public Aplikacje()
        {
            InitializeComponent();
        }
        double opacity = 0;
        System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            if (opacity < 1)
            {
                this.Opacity = opacity;
                opacity = opacity + 0.01;
            }
        }
        private void dispatcherTimer2_Tick(object sender, EventArgs e)
        {
            if (opacity >= 0)
            {
                this.Opacity = opacity;
                opacity = opacity - 0.01;
            }
            else
                Close();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.Left = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Right - this.Width;
            this.Top = this.Width - 235;

            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 10);
            dispatcherTimer.Start();
        }

        private void image1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            opacity = 1;
            System.Windows.Threading.DispatcherTimer dispatcherTimer2 = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer2.Tick += new EventHandler(dispatcherTimer2_Tick);
            dispatcherTimer2.Interval = new TimeSpan(0, 0, 0, 0, 10);
            dispatcherTimer.Stop();
            dispatcherTimer2.Start();
        }

        private void image2_MouseDown(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Process.Start(System.AppDomain.CurrentDomain.BaseDirectory + "\\app\\klawiatura.exe");
        }

        private void image3_MouseDown(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Process.Start(System.AppDomain.CurrentDomain.BaseDirectory + "\\app\\kalkulator.exe");
        }

        private void image4_MouseDown(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Process.Start(System.AppDomain.CurrentDomain.BaseDirectory + "\\app\\zakupy.exe");
        }

        private void image1_MouseEnter(object sender, MouseEventArgs e)
        {
            image1.Source = new BitmapImage(new Uri(System.AppDomain.CurrentDomain.BaseDirectory + "\\images\\ukryj2.png"));
        }

        private void image1_MouseLeave(object sender, MouseEventArgs e)
        {
            image1.Source = new BitmapImage(new Uri(System.AppDomain.CurrentDomain.BaseDirectory + "\\images\\ukryj1.png"));
        }

        private void image2_MouseEnter(object sender, MouseEventArgs e)
        {
            image2.Source = new BitmapImage(new Uri(System.AppDomain.CurrentDomain.BaseDirectory + "\\images\\klawiatura2.png"));
        }

        private void image2_MouseLeave(object sender, MouseEventArgs e)
        {
            image2.Source = new BitmapImage(new Uri(System.AppDomain.CurrentDomain.BaseDirectory + "\\images\\klawiatura1.png"));
        }

        private void image3_MouseEnter(object sender, MouseEventArgs e)
        {
            image3.Source = new BitmapImage(new Uri(System.AppDomain.CurrentDomain.BaseDirectory + "\\images\\kalkulator2.png"));
        }

        private void image3_MouseLeave(object sender, MouseEventArgs e)
        {
            image3.Source = new BitmapImage(new Uri(System.AppDomain.CurrentDomain.BaseDirectory + "\\images\\kalkulator1.png"));
        }

        private void image4_MouseEnter(object sender, MouseEventArgs e)
        {
            image4.Source = new BitmapImage(new Uri(System.AppDomain.CurrentDomain.BaseDirectory + "\\images\\lista2.png"));
        }

        private void image4_MouseLeave(object sender, MouseEventArgs e)
        {
            image4.Source = new BitmapImage(new Uri(System.AppDomain.CurrentDomain.BaseDirectory + "\\images\\lista1.png"));
        }

        private void image5_MouseEnter(object sender, MouseEventArgs e)
        {
            image5.Source = new BitmapImage(new Uri(System.AppDomain.CurrentDomain.BaseDirectory + "\\images\\internet2.png"));
        }

        private void image5_MouseLeave(object sender, MouseEventArgs e)
        {
            image5.Source = new BitmapImage(new Uri(System.AppDomain.CurrentDomain.BaseDirectory + "\\images\\internet1.png"));
        }

        private void image5_MouseDown(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Process.Start(System.AppDomain.CurrentDomain.BaseDirectory + "\\app\\internet.exe");
        }
    }
}

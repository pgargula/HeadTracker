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
    /// Interaction logic for Kalibracja.xaml
    /// </summary>
    public partial class Kalibracja : Window
    {
        public Kalibracja()
        {
            InitializeComponent();
        }


        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            Close();
        }
        
        
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.Activate();
            System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 5);
            dispatcherTimer.Start();

          

            this.Left = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Right - this.Height - 300;
            this.Top = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Bottom - this.Width;
           // this.Height = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Height;
        }
    }
}

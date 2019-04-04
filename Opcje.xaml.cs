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
using System.Xml;

namespace FaceTrackingBasics
{
    /// <summary>
    /// Interaction logic for Opcje.xaml
    /// </summary>
    public partial class Opcje : Window
    {
        public Opcje()
        {
            InitializeComponent();
        }


        double opacity = 0;
        Label labelOpcje = new Label();
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
        private void dispatcherTimerCzulosc_Tick(object sender, EventArgs e)
        {
            labelCzulosc.Content = Convert.ToString(Convert.ToInt32(slider1.Value*16));
        }
        string[] gesty = new string[4];
        bool ready = false;
        System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.Left = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Right - this.Width;
            this.Top = this.Width - 235;

            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 10);
            dispatcherTimer.Start();

            System.Windows.Threading.DispatcherTimer dispatcherTimerCzulosc = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimerCzulosc.Tick += new EventHandler(dispatcherTimerCzulosc_Tick);
            dispatcherTimerCzulosc.Interval = new TimeSpan(0, 0, 0, 0, 10);
            dispatcherTimerCzulosc.Start();

            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 1;
            comboBox3.SelectedIndex = 2;
            comboBox4.SelectedIndex = 3;

            ready = true;
        }

        private void slider1_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            string newValue2 = Convert.ToString(Convert.ToInt32(slider1.Value*16));
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(System.AppDomain.CurrentDomain.BaseDirectory + "\\settings.xml");
            XmlNode node = xmlDoc.SelectSingleNode("options");
            node.Attributes[0].Value = newValue2;
            xmlDoc.Save(System.AppDomain.CurrentDomain.BaseDirectory + "\\settings.xml");
        }
        string bufor;
        int newValue = 0;
        private void comboBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboBox1.SelectedIndex == comboBox2.SelectedIndex && ready == true)
            {
                bufor = comboBox1.Text;
                comboBox1.Text = comboBox2.Text;
                comboBox2.Text = bufor;

                newValue = 1;
                     
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(System.AppDomain.CurrentDomain.BaseDirectory + "\\settings.xml");
                XmlNode node = xmlDoc.SelectSingleNode("options");
                node.Attributes[1].Value = Convert.ToString(newValue);
                xmlDoc.Save(System.AppDomain.CurrentDomain.BaseDirectory + "\\settings.xml");

                newValue = 0;

                XmlDocument xmlDoc2 = new XmlDocument();
                xmlDoc2.Load(System.AppDomain.CurrentDomain.BaseDirectory + "\\settings.xml");
                XmlNode node2 = xmlDoc2.SelectSingleNode("options");
                node2.Attributes[2].Value = Convert.ToString(newValue);
                xmlDoc2.Save(System.AppDomain.CurrentDomain.BaseDirectory + "\\settings.xml");

            }
            if (comboBox1.SelectedIndex == comboBox3.SelectedIndex && ready == true)
            {
                bufor = comboBox1.Text;
                comboBox1.Text = comboBox3.Text;
                comboBox3.Text = bufor;

                newValue = 2;

 
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(System.AppDomain.CurrentDomain.BaseDirectory + "\\settings.xml");
                XmlNode node = xmlDoc.SelectSingleNode("options");
                node.Attributes[1].Value = Convert.ToString(newValue);
                xmlDoc.Save(System.AppDomain.CurrentDomain.BaseDirectory + "\\settings.xml");

                newValue = 0;

                XmlDocument xmlDoc2 = new XmlDocument();
                xmlDoc2.Load(System.AppDomain.CurrentDomain.BaseDirectory + "\\settings.xml");
                XmlNode node2 = xmlDoc2.SelectSingleNode("options");
                node2.Attributes[3].Value = Convert.ToString(newValue);
                xmlDoc2.Save(System.AppDomain.CurrentDomain.BaseDirectory + "\\settings.xml");
            }
            if (comboBox1.SelectedIndex == comboBox4.SelectedIndex && ready == true)
            {
                bufor = comboBox1.Text;
                comboBox1.Text = comboBox4.Text;
                comboBox4.Text = bufor;

                newValue = 3;

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(System.AppDomain.CurrentDomain.BaseDirectory + "\\settings.xml");
                XmlNode node = xmlDoc.SelectSingleNode("options");
                node.Attributes[1].Value = Convert.ToString(newValue);
                xmlDoc.Save(System.AppDomain.CurrentDomain.BaseDirectory + "\\settings.xml");

                newValue = 0;

                XmlDocument xmlDoc2 = new XmlDocument();
                xmlDoc2.Load(System.AppDomain.CurrentDomain.BaseDirectory + "\\settings.xml");
                XmlNode node2 = xmlDoc2.SelectSingleNode("options");
                node2.Attributes[4].Value = Convert.ToString(newValue);
                xmlDoc2.Save(System.AppDomain.CurrentDomain.BaseDirectory + "\\settings.xml");
            }
            if (comboBox2.SelectedIndex == comboBox3.SelectedIndex && ready == true)
            {
                bufor = comboBox2.Text;
                comboBox2.Text = comboBox3.Text;
                comboBox3.Text = bufor;

                newValue = 2;


                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(System.AppDomain.CurrentDomain.BaseDirectory + "\\settings.xml");
                XmlNode node = xmlDoc.SelectSingleNode("options");
                node.Attributes[2].Value = Convert.ToString(newValue);
                xmlDoc.Save(System.AppDomain.CurrentDomain.BaseDirectory + "\\settings.xml");

                newValue = 1;

                XmlDocument xmlDoc2 = new XmlDocument();
                xmlDoc2.Load(System.AppDomain.CurrentDomain.BaseDirectory + "\\settings.xml");
                XmlNode node2 = xmlDoc2.SelectSingleNode("options");
                node2.Attributes[3].Value = Convert.ToString(newValue);
                xmlDoc2.Save(System.AppDomain.CurrentDomain.BaseDirectory + "\\settings.xml");
            }
            if (comboBox2.SelectedIndex == comboBox4.SelectedIndex && ready == true)
            {
                bufor = comboBox2.Text;
                comboBox2.Text = comboBox4.Text;
                comboBox4.Text = bufor;

                newValue = 3;

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(System.AppDomain.CurrentDomain.BaseDirectory + "\\settings.xml");
                XmlNode node = xmlDoc.SelectSingleNode("options");
                node.Attributes[2].Value = Convert.ToString(newValue);
                xmlDoc.Save(System.AppDomain.CurrentDomain.BaseDirectory + "\\settings.xml");

                newValue = 1;

                XmlDocument xmlDoc2 = new XmlDocument();
                xmlDoc2.Load(System.AppDomain.CurrentDomain.BaseDirectory + "\\settings.xml");
                XmlNode node2 = xmlDoc2.SelectSingleNode("options");
                node2.Attributes[4].Value = Convert.ToString(newValue);
                xmlDoc2.Save(System.AppDomain.CurrentDomain.BaseDirectory + "\\settings.xml");
            }
            if (comboBox3.SelectedIndex == comboBox4.SelectedIndex && ready == true)
            {
                bufor = comboBox3.Text;
                comboBox3.Text = comboBox4.Text;
                comboBox4.Text = bufor;

                newValue = 3;

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(System.AppDomain.CurrentDomain.BaseDirectory + "\\settings.xml");
                XmlNode node = xmlDoc.SelectSingleNode("options");
                node.Attributes[3].Value = Convert.ToString(newValue);
                xmlDoc.Save(System.AppDomain.CurrentDomain.BaseDirectory + "\\settings.xml");

                newValue = 2;

                XmlDocument xmlDoc2 = new XmlDocument();
                xmlDoc2.Load(System.AppDomain.CurrentDomain.BaseDirectory + "\\settings.xml");
                XmlNode node2 = xmlDoc2.SelectSingleNode("options");
                node2.Attributes[4].Value = Convert.ToString(newValue);
                xmlDoc2.Save(System.AppDomain.CurrentDomain.BaseDirectory + "\\settings.xml");
            }
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

        private void image1_MouseEnter(object sender, MouseEventArgs e)
        {
            image1.Source = new BitmapImage(new Uri(System.AppDomain.CurrentDomain.BaseDirectory + "\\images\\ukryj2.png"));
        }

        private void image1_MouseLeave(object sender, MouseEventArgs e)
        {
            image1.Source = new BitmapImage(new Uri(System.AppDomain.CurrentDomain.BaseDirectory + "\\images\\ukryj1.png"));
        }

    }
}

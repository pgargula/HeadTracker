
namespace FaceTrackingBasics
{
    using System;
    using System.Windows;
    using System.Windows.Data;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using Microsoft.Kinect;
    using Microsoft.Kinect.Toolkit;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Windows.Controls;
    using System.Xml;
    /// <summary>
    /// </summary>
    public partial class MainWindow : Window
    {
        private static readonly int Bgr32BytesPerPixel = (PixelFormats.Bgr32.BitsPerPixel + 7) / 8;
        private readonly KinectSensorChooser sensorChooser = new KinectSensorChooser();
        private WriteableBitmap colorImageWritableBitmap;
        private byte[] colorImageData;
        private ColorImageFormat currentColorImageFormat = ColorImageFormat.Undefined;
      
        public MainWindow()
        {
            InitializeComponent();

            var faceTrackingViewerBinding = new Binding("Kinect") { Source = sensorChooser };
            faceTrackingViewer.SetBinding(FaceTrackingViewer.KinectProperty, faceTrackingViewerBinding);

            sensorChooser.KinectChanged += SensorChooserOnKinectChanged;

            sensorChooser.Start();

        }

        private void SensorChooserOnKinectChanged(object sender, KinectChangedEventArgs kinectChangedEventArgs)
        {
            KinectSensor oldSensor = kinectChangedEventArgs.OldSensor;
            KinectSensor newSensor = kinectChangedEventArgs.NewSensor;

            if (oldSensor != null)
            {
                oldSensor.AllFramesReady -= KinectSensorOnAllFramesReady;
                oldSensor.ColorStream.Disable();
                oldSensor.DepthStream.Disable();
                oldSensor.DepthStream.Range = DepthRange.Default;
                oldSensor.SkeletonStream.Disable();
                oldSensor.SkeletonStream.EnableTrackingInNearRange = false;
                oldSensor.SkeletonStream.TrackingMode = SkeletonTrackingMode.Default;

            }

            if (newSensor != null)
            {
                try
                {
                    newSensor.ColorStream.Enable(ColorImageFormat.RgbResolution640x480Fps30);
                    newSensor.DepthStream.Enable(DepthImageFormat.Resolution320x240Fps30);
                    try
                    {
                  
                        newSensor.DepthStream.Range = DepthRange.Near;
                        newSensor.SkeletonStream.EnableTrackingInNearRange = true;
                    }
                    catch (InvalidOperationException)
                    {
                        newSensor.DepthStream.Range = DepthRange.Default;
                        newSensor.SkeletonStream.EnableTrackingInNearRange = false;
                    }

                    newSensor.SkeletonStream.TrackingMode = SkeletonTrackingMode.Seated;
                    newSensor.SkeletonStream.Enable();
                    newSensor.AllFramesReady += KinectSensorOnAllFramesReady;
                }
                catch (InvalidOperationException)
                {

                }
            }
        }

        private void WindowClosed(object sender, EventArgs e)
        {
            
            sensorChooser.Stop();
            faceTrackingViewer.Dispose();
        }
        
        private void KinectSensorOnAllFramesReady(object sender, AllFramesReadyEventArgs allFramesReadyEventArgs)
        {
          
            using (var colorImageFrame = allFramesReadyEventArgs.OpenColorImageFrame())
            {
                if (colorImageFrame == null)
                {
                    return;
                }

        
                var haveNewFormat = this.currentColorImageFormat != colorImageFrame.Format;
                if (haveNewFormat)
                {
                    this.currentColorImageFormat = colorImageFrame.Format;
                    this.colorImageData = new byte[colorImageFrame.PixelDataLength];
                    this.colorImageWritableBitmap = new WriteableBitmap(
                        colorImageFrame.Width, colorImageFrame.Height, 96, 96, PixelFormats.Bgr32, null);
                    
                    ColorImage.Source = this.colorImageWritableBitmap;
                    
                }

                colorImageFrame.CopyPixelDataTo(this.colorImageData);
                this.colorImageWritableBitmap.WritePixels(
                    new Int32Rect(0, 0, colorImageFrame.Width, colorImageFrame.Height),
                    this.colorImageData,
                    colorImageFrame.Width * Bgr32BytesPerPixel,
                    0);
                
            }
            
        }

       


        private void image1_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            string newValue = "0";

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(System.AppDomain.CurrentDomain.BaseDirectory + "\\settings.xml");
            XmlNode node = xmlDoc.SelectSingleNode("options");
            node.Attributes[5].Value = Convert.ToString(newValue);
            xmlDoc.Save(System.AppDomain.CurrentDomain.BaseDirectory + "\\settings.xml");

            Environment.Exit(1);
        }

        private void image1_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            image1.Source = new BitmapImage(new Uri(System.AppDomain.CurrentDomain.BaseDirectory + "\\images\\zakoncz_podswietlone.jpg"));
        }

        private void image1_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            image1.Source = new BitmapImage(new Uri(System.AppDomain.CurrentDomain.BaseDirectory + "\\images\\zakoncz.jpg"));
        }

        private void image2_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            image2.Source = new BitmapImage(new Uri(System.AppDomain.CurrentDomain.BaseDirectory + "\\images\\opcje_zaznaczone.jpg"));
        }

        private void image2_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            image2.Source = new BitmapImage(new Uri(System.AppDomain.CurrentDomain.BaseDirectory + "\\images\\opcje.jpg"));
        }

        private void form1_Loaded(object sender, RoutedEventArgs e)
        {
            this.Left = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Right - this.Width;
            this.Top = this.Width - 35;
        }

        private void image2_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Opcje okno = new Opcje();
            okno.Show();
        }

        private void image4_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            image4.Source = new BitmapImage(new Uri(System.AppDomain.CurrentDomain.BaseDirectory + "\\images\\kalibracja_zaznaczone.jpg"));
        }

        private void image4_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            image4.Source = new BitmapImage(new Uri(System.AppDomain.CurrentDomain.BaseDirectory + "\\images\\kalibracja.jpg"));
        }

        private void image3_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            image3.Source = new BitmapImage(new Uri(System.AppDomain.CurrentDomain.BaseDirectory + "\\images\\aplikacje_zaznaczone.jpg"));
        }

        private void image3_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            image3.Source = new BitmapImage(new Uri(System.AppDomain.CurrentDomain.BaseDirectory + "\\images\\aplikacje.jpg"));
        }

        private void image4_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            string newValue = "0";

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(System.AppDomain.CurrentDomain.BaseDirectory + "\\settings.xml");
            XmlNode node = xmlDoc.SelectSingleNode("options");
            node.Attributes[5].Value = Convert.ToString(newValue);
            xmlDoc.Save(System.AppDomain.CurrentDomain.BaseDirectory + "\\settings.xml");
        }

        private void form1_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            string newValue = "0";

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(System.AppDomain.CurrentDomain.BaseDirectory + "\\settings.xml");
            XmlNode node = xmlDoc.SelectSingleNode("options");
            node.Attributes[5].Value = Convert.ToString(newValue);
            xmlDoc.Save(System.AppDomain.CurrentDomain.BaseDirectory + "\\settings.xml");
        }

        private void image3_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Aplikacje okno = new Aplikacje();
            okno.Show();
        }
    }
}

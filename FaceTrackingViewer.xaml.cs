namespace FaceTrackingBasics
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;
    using Microsoft.Kinect;
    using Microsoft.Kinect.Toolkit.FaceTracking;
    //using System.Windows.Forms;
    using System.Runtime.InteropServices;
    using System.Xml;
    using Point = System.Windows.Point;
    /// <summary>

    /// </summary>
    /// 



    public partial class FaceTrackingViewer : UserControl, IDisposable
    {
        public static readonly DependencyProperty KinectProperty = DependencyProperty.Register(
            "Kinect", 
            typeof(KinectSensor), 
            typeof(FaceTrackingViewer), 
            new PropertyMetadata(
                null, (o, args) => ((FaceTrackingViewer)o).OnSensorChanged((KinectSensor)args.OldValue, (KinectSensor)args.NewValue)));

        private const uint MaxMissedFrames = 100;

        private readonly Dictionary<int, SkeletonFaceTracker> trackedSkeletons = new Dictionary<int, SkeletonFaceTracker>();

        private byte[] colorImage;

        private ColorImageFormat colorImageFormat = ColorImageFormat.Undefined;

        private short[] depthImage;

        private DepthImageFormat depthImageFormat = DepthImageFormat.Undefined;

        private bool disposed;

        private Skeleton[] skeletonData;

        

        public FaceTrackingViewer()
        {
            this.InitializeComponent();
        }
       
        ~FaceTrackingViewer()
        {
            this.Dispose(false);
        }

        public KinectSensor Kinect
        {
            get
            {
                return (KinectSensor)this.GetValue(KinectProperty);
            }

            set
            {
                this.SetValue(KinectProperty, value);
            }
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
        
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                this.ResetFaceTracking();

                this.disposed = true;
            }
        }
        
        protected override void OnRender(DrawingContext drawingContext)
        {
            
            base.OnRender(drawingContext);
            foreach (SkeletonFaceTracker faceInformation in this.trackedSkeletons.Values)
            {
                faceInformation.DrawFaceModel(drawingContext);
                
                
            }
           
        }

        public void OnAllFramesReady(object sender, AllFramesReadyEventArgs allFramesReadyEventArgs)
        {
            ColorImageFrame colorImageFrame = null;
            DepthImageFrame depthImageFrame = null;
            SkeletonFrame skeletonFrame = null;

            try
            {
                colorImageFrame = allFramesReadyEventArgs.OpenColorImageFrame();
                depthImageFrame = allFramesReadyEventArgs.OpenDepthImageFrame();
                skeletonFrame = allFramesReadyEventArgs.OpenSkeletonFrame();

                if (colorImageFrame == null || depthImageFrame == null || skeletonFrame == null)
                {
                    return;
                }


                if (this.depthImageFormat != depthImageFrame.Format)
                {
                    this.ResetFaceTracking();
                    this.depthImage = null;
                    this.depthImageFormat = depthImageFrame.Format;
                }

                if (this.colorImageFormat != colorImageFrame.Format)
                {
                    this.ResetFaceTracking();
                    this.colorImage = null;
                    this.colorImageFormat = colorImageFrame.Format;
                }

    
                if (this.depthImage == null)
                {
                    this.depthImage = new short[depthImageFrame.PixelDataLength];
                }

                if (this.colorImage == null)
                {
                    this.colorImage = new byte[colorImageFrame.PixelDataLength];
                }
                
          
                if (this.skeletonData == null || this.skeletonData.Length != skeletonFrame.SkeletonArrayLength)
                {
                    this.skeletonData = new Skeleton[skeletonFrame.SkeletonArrayLength];
                }
                
                colorImageFrame.CopyPixelDataTo(this.colorImage);
                depthImageFrame.CopyPixelDataTo(this.depthImage);
                skeletonFrame.CopySkeletonDataTo(this.skeletonData);

         
                foreach (Skeleton skeleton in this.skeletonData)
                {
                    if (skeleton.TrackingState == SkeletonTrackingState.Tracked
                        || skeleton.TrackingState == SkeletonTrackingState.PositionOnly)
                    {
                  
                        if (!this.trackedSkeletons.ContainsKey(skeleton.TrackingId))
                        {
                            this.trackedSkeletons.Add(skeleton.TrackingId, new SkeletonFaceTracker());
                        }

                    
                        SkeletonFaceTracker skeletonFaceTracker;
                        if (this.trackedSkeletons.TryGetValue(skeleton.TrackingId, out skeletonFaceTracker))
                        {
                            skeletonFaceTracker.OnFrameReady(this.Kinect, colorImageFormat, colorImage, depthImageFormat, depthImage, skeleton);
                            skeletonFaceTracker.LastTrackedFrame = skeletonFrame.FrameNumber;
                        }
                    }
                }

                this.RemoveOldTrackers(skeletonFrame.FrameNumber);

                this.InvalidateVisual();
            }
            finally
            {
                if (colorImageFrame != null)
                {
                    colorImageFrame.Dispose();
                }

                if (depthImageFrame != null)
                {
                    depthImageFrame.Dispose();
                }

                if (skeletonFrame != null)
                {
                    skeletonFrame.Dispose();
                }
            }
        }

        private void OnSensorChanged(KinectSensor oldSensor, KinectSensor newSensor)
        {
            if (oldSensor != null)
            {
                oldSensor.AllFramesReady -= this.OnAllFramesReady;
                this.ResetFaceTracking();
            }

            if (newSensor != null)
            {
                newSensor.AllFramesReady += this.OnAllFramesReady;
            }
        }


        private void RemoveOldTrackers(int currentFrameNumber)
        {
            var trackersToRemove = new List<int>();

            foreach (var tracker in this.trackedSkeletons)
            {
                uint missedFrames = (uint)currentFrameNumber - (uint)tracker.Value.LastTrackedFrame;
                if (missedFrames > MaxMissedFrames)
                {
                  
                    trackersToRemove.Add(tracker.Key);
                }
            }

            foreach (int trackingId in trackersToRemove)
            {
                this.RemoveTracker(trackingId);
            }
        }

        private void RemoveTracker(int trackingId)
        {
            this.trackedSkeletons[trackingId].Dispose();
            this.trackedSkeletons.Remove(trackingId);
        }

        private void ResetFaceTracking()
        {
            foreach (int trackingId in new List<int>(this.trackedSkeletons.Keys))
            {
                this.RemoveTracker(trackingId);
            }
        }

        public class SkeletonFaceTracker : IDisposable
        {
            [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
            public static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

            private const int MOUSEEVENTF_LEFTDOWN = 0x02;
            private const int MOUSEEVENTF_LEFTUP = 0x04;
            private const int MOUSEEVENTF_RIGHTDOWN = 0x08;
            private const int MOUSEEVENTF_RIGHTUP = 0x10;
            private const int MOUSEEVENTF_WHEEL = 0x0800;
            public int sensitive = 80;

            public void DoMouseClick(int choise)
            {
                if (choise == 0)
                {
                    int X = System.Windows.Forms.Cursor.Position.X;
                    int Y = System.Windows.Forms.Cursor.Position.Y;
                    mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, X, Y, 0, 0);
                }
                if (choise == 1)
                {
                    
                    int X = System.Windows.Forms.Cursor.Position.X;
                    int Y = System.Windows.Forms.Cursor.Position.Y;
                    mouse_event(MOUSEEVENTF_RIGHTDOWN | MOUSEEVENTF_RIGHTUP, X, Y, 0, 0);
                    
                }

            }
            public void DoMouseScroll(int choice)
            {
                if(choice == 0)
                    mouse_event(MOUSEEVENTF_WHEEL, 0, 0, 120, 0);
                if(choice == 1)
                    mouse_event(MOUSEEVENTF_WHEEL, 0, 0, -120, 0);
            }
            private static FaceTriangle[] faceTriangles;

            public EnumIndexableCollection<FeaturePoint, PointF> facePoints;

            private FaceTracker faceTracker;

            private bool lastFaceTrackSucceeded;

            private SkeletonTrackingState skeletonTrackingState;

            public int LastTrackedFrame { get; set; }
            
            public void Dispose()
            {
                if (this.faceTracker != null)
                {
                    this.faceTracker.Dispose();
                    this.faceTracker = null;
                }
            }

            public void DrawFaceModel(DrawingContext drawingContext)
            {
                if (!this.lastFaceTrackSucceeded || this.skeletonTrackingState != SkeletonTrackingState.Tracked)
                {
                    return;
                }

                var faceModelPts = new List<Point>();
                var faceModel = new List<FaceModelTriangle>();

                for (int i = 0; i < this.facePoints.Count; i++)
                {
                    faceModelPts.Add(new Point(this.facePoints[i].X + 0.5f, this.facePoints[i].Y + 0.5f));
                }

                foreach (var t in faceTriangles)
                {
                    var triangle = new FaceModelTriangle();
                    triangle.P1 = faceModelPts[t.First];
                    triangle.P2 = faceModelPts[t.Second];
                    triangle.P3 = faceModelPts[t.Third];
                    faceModel.Add(triangle);
                }

                var faceModelGroup = new GeometryGroup();
                for (int i = 0; i < faceModel.Count; i++)
                {
                    var faceTriangle = new GeometryGroup();
                    faceTriangle.Children.Add(new LineGeometry(faceModel[i].P1, faceModel[i].P2));
                    faceTriangle.Children.Add(new LineGeometry(faceModel[i].P2, faceModel[i].P3));
                    faceTriangle.Children.Add(new LineGeometry(faceModel[i].P3, faceModel[i].P1));
                    faceModelGroup.Children.Add(faceTriangle);
                }

                drawingContext.DrawGeometry(Brushes.LightYellow, new Pen(Brushes.LightYellow, 1.0), faceModelGroup);
            }

        
            /// 
            bool kalibracja = false;
            double oldX = 0, oldY = 0, newX, newY; //oldX,oldY to kalibracja(punkty początkowe)
            double aktualnyX, aktualnyY;
            double oldMouseX, oldMouseY;
            double stosunekX, stosunekY;
            int clickCounter = 0;
            bool ruchX = false, ruchY = false;
            int kalibracjaCounter = 0;
            int[] gest = new int[4];
            int[] tablicaGestow = new int[4];

            public void OnFrameReady(KinectSensor kinectSensor, ColorImageFormat colorImageFormat, byte[] colorImage, DepthImageFormat depthImageFormat, short[] depthImage, Skeleton skeletonOfInterest)
            {
                
                this.skeletonTrackingState = skeletonOfInterest.TrackingState;

                if (this.skeletonTrackingState != SkeletonTrackingState.Tracked)
                {
             
                    return;
                }

                if (this.faceTracker == null)
                {
                    try
                    {
                        this.faceTracker = new FaceTracker(kinectSensor);
                    }
                    catch (InvalidOperationException)
                    {
   
                        Debug.WriteLine("AllFramesReady - creating a new FaceTracker threw an InvalidOperationException");
                        this.faceTracker = null;
                    }
                }

                if (this.faceTracker != null)
                {
                    FaceTrackFrame frame = this.faceTracker.Track(
                        colorImageFormat, colorImage, depthImageFormat, depthImage, skeletonOfInterest);

                    this.lastFaceTrackSucceeded = frame.TrackSuccessful;
                    if (this.lastFaceTrackSucceeded)
                    {
                        if (faceTriangles == null)
                        {
                
                            faceTriangles = frame.GetTriangles();
                        }

                        this.facePoints = frame.GetProjected3DShape();

                        XmlDocument XmlDocKalibracja = new XmlDocument();
                        XmlDocKalibracja.Load(System.AppDomain.CurrentDomain.BaseDirectory + "\\settings.xml");
                        XmlNodeList elemListKalibracja = XmlDocKalibracja.GetElementsByTagName("options");
                        kalibracja = Convert.ToBoolean(Convert.ToInt32(elemListKalibracja[0].Attributes["kalibracja"].Value));

                        if (kalibracja == false)
                        {
                            kalibracjaCounter++;
                            if (kalibracjaCounter == 1)
                            {
                                Kalibracja okno = new Kalibracja();
                                okno.Show();
                            }

                            if (kalibracjaCounter > 150)
                            {
                                oldX = Convert.ToInt32(this.facePoints[23].X);
                                oldY = -Convert.ToInt32(this.facePoints[23].Y);

                                oldMouseX = System.Windows.Forms.Cursor.Position.X;
                                oldMouseY = System.Windows.Forms.Cursor.Position.Y;

                                aktualnyX = oldX;
                                aktualnyY = oldY;
                                kalibracja = true;

                                string newValue = "1";
                                kalibracjaCounter = 0;

                                XmlDocument xmlDoc = new XmlDocument();
                                xmlDoc.Load(System.AppDomain.CurrentDomain.BaseDirectory + "\\settings.xml");
                                XmlNode node = xmlDoc.SelectSingleNode("options");
                                node.Attributes[5].Value = Convert.ToString(newValue);
                                xmlDoc.Save(System.AppDomain.CurrentDomain.BaseDirectory + "\\settings.xml");
                            }
                        }
                        
                        if(kalibracja == true)
                        {
                            
                            try
                            {
                                //ustawienie gestów
                                XmlDocument XmlDoc = new XmlDocument();
                                XmlDoc.Load(System.AppDomain.CurrentDomain.BaseDirectory + "\\settings.xml");
                                XmlNodeList elemList = XmlDoc.GetElementsByTagName("options");
                                gest[0] = Convert.ToInt32(elemList[0].Attributes["lpm"].Value);

                                //ustawienie gestów
                                XmlDocument XmlDoc2 = new XmlDocument();
                                XmlDoc2.Load(System.AppDomain.CurrentDomain.BaseDirectory + "\\settings.xml");
                                XmlNodeList elemList2 = XmlDoc2.GetElementsByTagName("options");
                                gest[1] = Convert.ToInt32(elemList2[0].Attributes["ppm"].Value);

                                //ustawienie gestów
                                XmlDocument XmlDoc3 = new XmlDocument();
                                XmlDoc3.Load(System.AppDomain.CurrentDomain.BaseDirectory + "\\settings.xml");
                                XmlNodeList elemList3 = XmlDoc3.GetElementsByTagName("options");
                                gest[2] = Convert.ToInt32(elemList3[0].Attributes["scrollup"].Value);

                                //ustawienie gestów
                                XmlDocument XmlDoc4 = new XmlDocument();
                                XmlDoc4.Load(System.AppDomain.CurrentDomain.BaseDirectory + "\\settings.xml");
                                XmlNodeList elemList4 = XmlDoc4.GetElementsByTagName("options");
                                gest[3] = Convert.ToInt32(elemList4[0].Attributes["scrolldown"].Value);

                            }
                            catch
                            {
                                MessageBox.Show("Błąd przy odczycie pliku settings.xml");
                            }

                           

                            newX = Convert.ToInt32(this.facePoints[23].X);
                            newY = -Convert.ToInt32(this.facePoints[23].Y);
                            stosunekX = Math.Abs(newX / oldX);
                            stosunekY = Math.Abs(newY / oldY);
          
                            //odczyt czułości z pliku settings.xml
                            try
                            {
                                XmlDocument XmlDoc = new XmlDocument();
                                XmlDoc.Load(System.AppDomain.CurrentDomain.BaseDirectory + "\\settings.xml");
                                XmlNodeList elemList = XmlDoc.GetElementsByTagName("options");
                                sensitive = Convert.ToInt32(elemList[0].Attributes["sensitive"].Value);
                            }
                            catch
                            {
                                sensitive = 80;
                            }
                            
                            if (Math.Abs(this.facePoints[88].X - this.facePoints[89].X) < 28 && Math.Abs(oldX - newX) > 70 && Math.Abs(oldY - newY) > 70 && Convert.ToInt32(Math.Abs(oldMouseX + (sensitive * stosunekX - sensitive))) < 1980 || Convert.ToInt32(Math.Abs(oldMouseY + (sensitive * stosunekY - sensitive))) < 1200 && Convert.ToInt32(Math.Abs(oldMouseX + (sensitive * stosunekX))) >= 0 && Convert.ToInt32(Math.Abs(oldMouseY + (sensitive * stosunekY))) >= 0)
                            {

                                if (stosunekX > 1.03 && ruchY == false)
                                {
                                    System.Windows.Forms.Cursor.Position = new System.Drawing.Point(Convert.ToInt32(Math.Abs(oldMouseX + (sensitive * stosunekX - sensitive))), Convert.ToInt32(Math.Abs(oldMouseY)));
                                    oldMouseX = Convert.ToInt32(Math.Abs(oldMouseX + (sensitive * stosunekX - sensitive)));
                                    ruchX = true;
                                }
                                if (stosunekX < 0.97 && ruchY == false)
                                {
                                    System.Windows.Forms.Cursor.Position = new System.Drawing.Point(Convert.ToInt32(Math.Abs(oldMouseX - (sensitive - (sensitive * stosunekX)))), Convert.ToInt32(Math.Abs(oldMouseY)));
                                    oldMouseX = Convert.ToInt32(Math.Abs(oldMouseX - (sensitive - (sensitive * stosunekX))));
                                    ruchX = true;
                                }
                                if (stosunekY > 1.03 && ruchX == false)
                                {
                                    System.Windows.Forms.Cursor.Position = new System.Drawing.Point(Convert.ToInt32(Math.Abs(oldMouseX)), Convert.ToInt32(Math.Abs(oldMouseY + (sensitive * stosunekY - sensitive))));
                                    oldMouseY = Convert.ToInt32(Math.Abs(oldMouseY + (sensitive * stosunekY - sensitive)));
                                    ruchY = true;
                                }
                                if (stosunekY < 0.97 && ruchX == false)
                                {
                                    System.Windows.Forms.Cursor.Position = new System.Drawing.Point(Convert.ToInt32(Math.Abs(oldMouseX)), Convert.ToInt32(Math.Abs(oldMouseY - (sensitive - (sensitive * stosunekY)))));
                                    oldMouseY = Convert.ToInt32(Math.Abs(oldMouseY - (sensitive - (sensitive * stosunekY))));
                                    ruchY = true;
                                }
                            }


                            //stan spoczynku
                            if (Math.Abs(oldX - newX) < 70)
                                ruchX = false;
                            //stan spoczynku
                            if (Math.Abs(oldY - newY) < 70)
                                ruchY = false;

                           
                            //PIERWSZY GEST
                            if (gest[0] == 0)
                            {
                                if (this.facePoints[40].Y - this.facePoints[87].Y > 12)
                                {
                                    clickCounter++;
                                    if (clickCounter % 10 == 0)
                                        DoMouseClick(0);
                                    if (clickCounter == 10000)
                                        clickCounter = 0;
                                }
                            }
                            if (gest[0] == 1)
                            {
                                if (this.facePoints[57].Y - this.facePoints[51].Y > 15)
                                {
                                    clickCounter++;
                                    if (clickCounter % 10 == 0)
                                        DoMouseClick(1);
                                    if (clickCounter == 10000)
                                        clickCounter = 0;
                                }
                            }
                            if (gest[0] == 2)
                            {
                                if (Math.Abs(this.facePoints[88].X - this.facePoints[89].X) > 28 && stosunekY > 1.03)
                                {
                                    clickCounter++;
                                    if (clickCounter % 2 == 0)
                                        DoMouseScroll(1);
                                    if (clickCounter == 10000)
                                        clickCounter = 0;
                                }
                            }
                            
                            if (gest[0] == 3)
                            {
                                
                                if (Math.Abs(this.facePoints[88].X - this.facePoints[89].X) > 28 && stosunekY < 0.97)
                                {
                                    clickCounter++;
                                    if (clickCounter % 2 == 0)
                                        DoMouseScroll(0);
                                    if (clickCounter == 10000)
                                        clickCounter = 0;
                                }
                            }
                            //DRUGI GEST
                            if (gest[1] == 0)
                            {
                                if (this.facePoints[40].Y - this.facePoints[87].Y > 12)
                                {
                                    clickCounter++;
                                    if (clickCounter % 10 == 0)
                                        DoMouseClick(0);
                                    if (clickCounter == 10000)
                                        clickCounter = 0;
                                }
                            }
                            if (gest[1] == 1)
                            {
                                if (this.facePoints[57].Y - this.facePoints[51].Y > 14)
                                {
                                    clickCounter++;
                                    if (clickCounter % 10 == 0)
                                        DoMouseClick(1);
                                    if (clickCounter == 10000)
                                        clickCounter = 0;
                                }
                            }
                            if (gest[1] == 2)
                            {
                                if (Math.Abs(this.facePoints[88].X - this.facePoints[89].X) > 28 && stosunekY > 1.03)
                                {
                                    clickCounter++;
                                    if (clickCounter % 2 == 0)
                                        DoMouseScroll(1);
                                    if (clickCounter == 10000)
                                        clickCounter = 0;
                                }
                            }
                            if (gest[1] == 3)
                            {
                                if (Math.Abs(this.facePoints[88].X - this.facePoints[89].X) > 28 && stosunekY < 0.97)
                                {
                                    clickCounter++;
                                    if (clickCounter % 2 == 0)
                                        DoMouseScroll(0);
                                    if (clickCounter == 10000)
                                        clickCounter = 0;
                                }
                            }
                            //TRZECI GEST
                            if (gest[2] == 0)
                            {
                                if (this.facePoints[40].Y - this.facePoints[87].Y > 12)
                                {
                                    clickCounter++;
                                    if (clickCounter % 10 == 0)
                                        DoMouseClick(0);
                                    if (clickCounter == 10000)
                                        clickCounter = 0;
                                }
                            }
                            if (gest[2] == 1)
                            {
                                if (this.facePoints[57].Y - this.facePoints[51].Y > 14)
                                {
                                    clickCounter++;
                                    if (clickCounter % 10 == 0)
                                        DoMouseClick(1);
                                    if (clickCounter == 10000)
                                        clickCounter = 0;
                                }
                            }
                            if (gest[2] == 2)
                            {
                                if (Math.Abs(this.facePoints[88].X - this.facePoints[89].X) > 28 && stosunekY > 1.03)
                                {
                                    clickCounter++;
                                    if (clickCounter % 2 == 0)
                                        DoMouseScroll(1);
                                    if (clickCounter == 10000)
                                        clickCounter = 0;
                                }
                            }
                            if (gest[2] == 3)
                            {
                                if (Math.Abs(this.facePoints[88].X - this.facePoints[89].X) > 28 && stosunekY < 0.97)
                                {
                                    clickCounter++;
                                    if (clickCounter % 2 == 0)
                                        DoMouseScroll(0);
                                    if (clickCounter == 10000)
                                        clickCounter = 0;
                                }
                            }
                            //CZWARTY GEST
                            if (gest[3] == 0)
                            {
                                
                                if (this.facePoints[40].Y - this.facePoints[87].Y > 12)
                                {

                                    clickCounter++;
                                    if (clickCounter % 10 == 0)
                                        DoMouseClick(0);
                                    if (clickCounter == 10000)
                                        clickCounter = 0;
                                }
                            }
                            if (gest[3] == 1)
                            {
                                if (this.facePoints[57].Y - this.facePoints[51].Y > 14)
                                {
                                    clickCounter++;
                                    if (clickCounter % 10 == 0)
                                        DoMouseClick(1);
                                    if (clickCounter == 10000)
                                        clickCounter = 0;
                                }
                            }
                            if (gest[3] == 2)
                            {
                                if (Math.Abs(this.facePoints[88].X - this.facePoints[89].X) > 28 && stosunekY > 1.03)
                                {
                                    clickCounter++;
                                    if (clickCounter % 2 == 0)
                                        DoMouseScroll(1);
                                    if (clickCounter == 10000)
                                        clickCounter = 0;
                                }
                            }
                            if (gest[3] == 3)
                            {
                                if (Math.Abs(this.facePoints[88].X - this.facePoints[89].X) > 28 && stosunekY < 0.97)
                                {
                                    clickCounter++;
                                    if (clickCounter % 2 == 0)
                                        DoMouseScroll(0);
                                    if (clickCounter == 10000)
                                        clickCounter = 0;
                                }
                            }
                        }
                    }
                }
            }

            private struct FaceModelTriangle
            {
                public Point P1;
                public Point P2;
                public Point P3;
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Controls.Maps;
using Microsoft.Devices.Sensors;
using Microsoft.Xna.Framework;

namespace MapForCurs
{
    public partial class map_page : PhoneApplicationPage
    {
        private Accelerometer myAccel;
        private GeoCoordinateWatcher myGeoWatcher;
        private Vector3 currentValues;
        public map_page()
        {
            InitializeComponent();

            myAccel = new Accelerometer();
            myAccel.CurrentValueChanged += new EventHandler<SensorReadingEventArgs<AccelerometerReading>>(myAccel_CurrentValueChanged);
            myAccel.Start();
            myGeoWatcher = new GeoCoordinateWatcher();
            myGeoWatcher.MovementThreshold = 100.0f;

            // myGeoWatcher.StatusChanged += new EventHandler<GeoPositionStatusChangedEventArgs>(myGeoWatcher_StatusChanged);

            myGeoWatcher.PositionChanged += new EventHandler<GeoPositionChangedEventArgs<GeoCoordinate>>(myGeoWatcher_PositionChanged);
            myGeoWatcher.TryStart(false, TimeSpan.FromSeconds(60));
        }

        void myAccel_CurrentValueChanged(object sender, SensorReadingEventArgs<AccelerometerReading> e)
        {
            if (myAccel.IsDataValid)
            {
                float deltaZ = (currentValues - e.SensorReading.Acceleration).Z;
                float Z = e.SensorReading.Acceleration.Z;

                currentValues = e.SensorReading.Acceleration;

                if (Z < 0 && deltaZ > 0)
                {
                    //увеличиваем масштаб
                    Dispatcher.BeginInvoke(() => HandleZoomIn());
                }
                if (Z > 0 && deltaZ < 0)
                {
                    //уменьшаем масштаб
                    Dispatcher.BeginInvoke(() => HandleZoomOut());
                }
            }

        }

        private void HandleZoomIn()
        {
            MyMap.ZoomLevel += 1;
        }

        private void HandleZoomOut()
        {
            MyMap.ZoomLevel -= 1;
        }

        void myGeoWatcher_PositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            MyMap.Center = e.Position.Location;
        }

        private void ZoomIn_Click(object sender, EventArgs eventArgs)
        {
            MyMap.ZoomLevel += 1;
        }

        private void ZoomOut_Click(object sender, EventArgs eventArgs)
        {
            MyMap.ZoomLevel -= 1;
        }

        private void LayoutChange_Click(object sender, EventArgs eventArgs)
        {
            if (MyMap.Mode is RoadMode)
            {
                MyMap.Mode = new AerialMode(true);
            }
            else
            {
                MyMap.Mode = new RoadMode();
            }

        }

        private  void BackToMenu_Click(object sender, EventArgs eventArgs)
        {
            NavigationService.GoBack();
        }
    }
}
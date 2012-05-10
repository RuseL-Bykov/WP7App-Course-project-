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
using System.Threading;

namespace MapForCurs
{
    public partial class map_page : PhoneApplicationPage
    {
        private Accelerometer myAccel;
        private GeoCoordinateWatcher myGeoWatcher;
        private Vector3 currentValues;
        private Pushpin myPushpin;

        public map_page() //конструктор
        {

            InitializeComponent();


            myGeoWatcher = new GeoCoordinateWatcher(GeoPositionAccuracy.High);
            myGeoWatcher.MovementThreshold = 100.0f;

            myGeoWatcher.StatusChanged += myGeoWatcher_StatusChanged;

            myGeoWatcher.PositionChanged += myGeoWatcher_PositionChanged;
            myAccel = new Accelerometer();
            myAccel.CurrentValueChanged += myAccel_CurrentValueChanged;
            myAccel.Start();
            myPushpin = new Pushpin();
           myGeoWatcher.TryStart(false, TimeSpan.FromSeconds(60));
        }


        void myGeoWatcher_StatusChanged(object sender, GeoPositionStatusChangedEventArgs e) //обработчик сервися определения местоположений
        {
            switch (e.Status)
            {
                case GeoPositionStatus.Disabled:
                    if (myGeoWatcher.Permission == GeoPositionPermission.Denied)
                    {
                        GeoStatus.Text = "Сервис выключен";
                    }
                    else
                    {
                        GeoStatus.Text = "На этом устройстве сервис недоступен";
                    }
                    break;
                case GeoPositionStatus.Initializing:
                    GeoStatus.Text = "Сервис инициализируется";
                    break;
                case GeoPositionStatus.NoData:
                    GeoStatus.Text = "Данные о месположении недоступны";
                    break;
                case GeoPositionStatus.Ready:
                    {
                        GeoStatus.Text = "Данные о местоположении доступны";
                    }
                    break;
            }

        }

        void myAccel_CurrentValueChanged(object sender, SensorReadingEventArgs<AccelerometerReading> e)  //работа акселерометра
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

        private void HandleZoomIn() //акселерометр +
        {
            MyMap.ZoomLevel += 1;
        }

        private void HandleZoomOut() //акселерометр -
        {
            MyMap.ZoomLevel -= 1;
        }

        void myGeoWatcher_PositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)  //обработчик смены позиции
        {
            MyMap.Center = e.Position.Location;
            myPushpin.Location = e.Position.Location;
            if (!MyMap.Children.Contains(myPushpin)) MyMap.Children.Add(myPushpin);
            myPushpin.Content = "Ваши GPS координаты:\n" + myGeoWatcher.Position.Location.Latitude.ToString() + "\n" + myGeoWatcher.Position.Location.Longitude.ToString();
        }

        private void ZoomIn_Click(object sender, EventArgs eventArgs) // увеличиваем пасштаб
        {
            MyMap.ZoomLevel += 1;
        }

        private void ZoomOut_Click(object sender, EventArgs eventArgs) //уменшаем масшатб
        {
            MyMap.ZoomLevel -= 1;
        }

        private void LayoutChange_Click(object sender, EventArgs eventArgs) //изменение вида карты
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

        private  void BackToMenu_Click(object sender, EventArgs eventArgs) //кнопка назад в меню
        {
            NavigationService.GoBack();
        }
    }
}
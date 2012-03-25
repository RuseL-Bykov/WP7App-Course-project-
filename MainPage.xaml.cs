using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Controls.Maps; 

namespace MapForCurs
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Конструктор
        public MainPage()
        {
            InitializeComponent();
        }

        private void ZoomIn_Click(object sender, RoutedEventArgs e)
        {
            MyMap.ZoomLevel += 1;
        }

        private void ZoomOut_Click(object sender, RoutedEventArgs e)
        {
            MyMap.ZoomLevel -= 1;
        }

        private void LayoutChange_Click(object sender, RoutedEventArgs e)
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
    }
}
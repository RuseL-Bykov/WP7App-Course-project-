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
using Microsoft.Phone.Controls;
using System.Windows.Navigation;

namespace MapForCurs
{

    public partial class MainPage : PhoneApplicationPage
    {
        // Конструктор
        public MainPage()
        {
            InitializeComponent();
           
        }

        private  void GoToMap_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/map_page.xaml", UriKind.Relative));
        }

        private void For_User_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/PivotPageForUser.xaml", UriKind.Relative));
        }

               
    }
}
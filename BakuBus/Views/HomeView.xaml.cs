using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using BakuBus.Models;
using Microsoft.Maps.MapControl.WPF;

namespace BakuBus.Views
{
    public partial class HomeView : Window, INotifyPropertyChanged
    {
        private Bakubus myBuses;
        public Bakubus? MyBuses
        {
            get => myBuses;
            set
            {
                myBuses = value;
                INotifyPropertyChanged();
            }
        }

        public Attributes attributes { get; set; }

        private string selectedbus;

        public string SelectedBus
        {
            get { return selectedbus; }
            set { selectedbus = value; INotifyPropertyChanged(); }
        }
        public event PropertyChangedEventHandler? PropertyChanged;
        void INotifyPropertyChanged([CallerMemberName] string? propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        public HomeView()
        {
            InitializeComponent();
            var mapKey = "atdthVyqDCYydLA26DCL~SC41f6rxsdjIXsNz3GllbQ~AikySlhq1Q5Y4OqPJM-hUITVvNbnChXfFFaAVq8hxDQWCuLGhDorrDI7RO7RzCVz";
            DataContext = this;
            myMap.CredentialsProvider = new ApplicationIdCredentialsProvider(mapKey);
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var dir = new DirectoryInfo("../../../");
            var fileName = "bakubusApi.json";
            var filePath = dir.FullName + fileName;

            var jsonStr = File.ReadAllText(filePath);
            MyBuses = JsonSerializer.Deserialize<Bakubus>(jsonStr);



            List<string> Bus_Number = new List<string>();

            foreach (var item in MyBuses!.BUS)
            {
                string busNumber = item.attributes.DISPLAY_ROUTE_CODE;
                if (!Bus_Number.Contains(busNumber))
                {
                    Bus_Number.Add(busNumber);

                }
            }
            comboBox.ItemsSource = Bus_Number;

            Dictionary<string, SolidColorBrush> routeColorMappings = new Dictionary<string, SolidColorBrush>();

            foreach (var bus in MyBuses!.BUS)
            {
                Pushpin pushpin = new Pushpin();
                pushpin.Content = bus.attributes.DISPLAY_ROUTE_CODE.ToString();
                Location location = new Location(Convert.ToDouble(bus.attributes.LATITUDE), Convert.ToDouble(bus.attributes.LONGITUDE));
                pushpin.Location = location;
                myMap.Children.Add(pushpin);
                pushpin.Tag = bus.attributes;
                pushpin.ToolTip = new();
                pushpin.MouseEnter += PushPinEnter;
                string routeCode = bus.attributes.DISPLAY_ROUTE_CODE.ToString();

                if (routeColorMappings.ContainsKey(routeCode))
                {
                    pushpin.Background = routeColorMappings[routeCode];
                }
                else
                {
                    SolidColorBrush colorBrush = new SolidColorBrush(Color.FromRgb((byte)Random.Shared.Next(0, 255),
                        (byte)Random.Shared.Next(0, 255), (byte)Random.Shared.Next(0, 255)));
                    routeColorMappings.Add(routeCode, colorBrush);
                    pushpin.Background = colorBrush;
                }

            }

        }
        private void PushPinEnter(object sender, MouseEventArgs e)
        {
            Pushpin pushpin = (Pushpin)sender;
            attributes = pushpin.Tag as Attributes;
        }

        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            foreach (var buses in myMap.Children)
            {
                if (buses is Pushpin p)
                {
                    p.Visibility = Visibility.Visible;
                    comboBox.SelectedIndex =-1;
                }
            }
        }
        private void Search_Click(object sender, RoutedEventArgs e)
        {
            foreach (var buses in myMap.Children)
            {
                if (buses is Pushpin pushpin)
                {
                    if (pushpin.Content.ToString() == SelectedBus)
                    {
                        pushpin.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        pushpin.Visibility = Visibility.Collapsed;
                    }
                }
            }
        }
    }
}

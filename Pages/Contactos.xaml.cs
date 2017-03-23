using ADS.Classes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Windows.ApplicationModel.Email;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Services.Maps;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace ADS.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Contactos : Page
    {
        private string entity_name;
        private string entity_number;
        private Boolean entmode;
        private ObservableCollection<Classes.Entidade> EntList;
        private Geopoint current;

        public Contactos()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            MainPage.instance.change_title("Contactos Úteis");
            Translate();

            if (e.Parameter == null)
            {
                ZoomCurrentLoc();
                entmode = false;
                MyLocationButton.Visibility = Visibility.Visible;
            }
            else
            {
                Classes.Entidade se = e.Parameter as Classes.Entidade;
                MyLocationButton.Visibility = Visibility.Collapsed;
                entmode = true;
                ZoomToEnt(se);
            }
            List<BasicGeoposition> positions = new List<BasicGeoposition>();
            DatabaseHelperClass dbtemas = new DatabaseHelperClass();
            ObservableCollection<Classes.Entidade> EntList = dbtemas.ReadEntidade();
            this.EntList = EntList;
            Point p = new Point() { X = 0.32, Y = 0.78 };
            foreach (var E in EntList)
            {
                double lat = dbtemas.latitude(E.N_Vertice);
                double lon = dbtemas.longitude(E.N_Vertice);
                E.point_n = p;
                E.geo = new Geopoint(new BasicGeoposition() { Latitude = lat, Longitude = lon });
            }
            MapItems.ItemsSource = EntList;
        }

        private void map1_Loaded(object sender, RoutedEventArgs e)
        {
            //No longer used
            //ZoomCurrentLoc();
        }

        private async void ZoomToEnt(Classes.Entidade ent)
        {
            DatabaseHelperClass dbtemas = new DatabaseHelperClass();
            double lat = dbtemas.latitude(ent.N_Vertice);
            double lon = dbtemas.longitude(ent.N_Vertice);
            ent.geo = new Geopoint(new BasicGeoposition() { Latitude = lat, Longitude = lon });
            await map1.TrySetViewAsync(ent.geo, 16, 0, 0, MapAnimationKind.None);

            if (ent.dest)
            {
                Geoposition location = MainPage.instance.getPos();
                this.current = new Geopoint(new BasicGeoposition() { Latitude = location.Coordinate.Latitude, Longitude = location.Coordinate.Longitude });
                ent.dest = false;
                Plan_Route(ent);
            }
        }
     
       private async void ZoomCurrentLoc()
       {
           var gl = new Geolocator() { DesiredAccuracy = PositionAccuracy.High };
           try
           {
               // Carry out the operation  Geoposition pos =    
               Geoposition location = MainPage.instance.getPos();
               this.current = new Geopoint(new BasicGeoposition() { Latitude = location.Coordinate.Latitude, Longitude = location.Coordinate.Longitude });
               AddMapIcon(location.Coordinate.Point);
               await map1.TrySetViewAsync(location.Coordinate.Point, 14, 0, 0, MapAnimationKind.None);
           }
           catch
           {
               //Operation aborted Your App does not have permission to access location data.
               //Launcher.LaunchUriAsync(new Uri("ms-settings-location:"));
               Geopoint temp = new Geopoint(new BasicGeoposition() { Latitude = 37.2158895, Longitude = -8.0695234 });
               map1.TrySetViewAsync(temp, 8, 0, 0, MapAnimationKind.None);
           }
       }

       private void AddMapIcon(Geopoint dest)
       {
           MapIcon MapIcon1 = new MapIcon();
           MapIcon1.Image = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/map-pin-red-lo.png"));
           MapIcon1.Location = new Geopoint(new BasicGeoposition()
           {
               Latitude = dest.Position.Latitude,
               Longitude = dest.Position.Longitude
           });
           MapIcon1.NormalizedAnchorPoint = new Point(0.5, 1.0);
           MapIcon1.Title = "You are here!";
           map1.MapElements.Add(MapIcon1);
       }

       private void StackPanel_Tapped(object sender, TappedRoutedEventArgs e)
       {
           if(!entmode)
                Caixa.Visibility = Visibility.Visible;
           var selectedItem = (Entidade)(sender as StackPanel).DataContext;
           Nome.Text = selectedItem.Nome;
           Morada.Text = selectedItem.Morada;
           Telefone.Text = selectedItem.Telefone.ToString();
           Email.Text = selectedItem.Email;

           //Debug.WriteLine("objecto: " + selectedItem.Nome);
           this.entity_name = selectedItem.Nome;
           this.entity_number = Convert.ToString(selectedItem.Telefone);
       }

       private void map1_MapTapped(MapControl sender, MapInputEventArgs args)
       {
           Caixa.Visibility = Visibility.Collapsed;
       }

       

       private void phone_stack_Tapped(object sender, TappedRoutedEventArgs e)
       {
           Windows.ApplicationModel.Calls.PhoneCallManager.ShowPhoneCallUI(this.entity_number, this.entity_name);
       }

        private async void email_stack_Tapped(object sender, TappedRoutedEventArgs e)
       {
           EmailRecipient sendTo = new EmailRecipient()
           {
               Address = Email.Text
           };

           EmailMessage mail = new EmailMessage();

           mail.To.Add(sendTo);
           await EmailManager.ShowComposeNewEmailAsync(mail);
       }

       private void map1_ZoomLevelChanged(MapControl sender, object args)
       {
           //Debug.WriteLine("new zoomleve: " + map1.ZoomLevel);
           if(map1.ZoomLevel < 13) {
               MapItems.ItemsSource = null;
           }
           else
           {
               MapItems.ItemsSource = EntList;
           }
       }

       private void MyLocationButton_Tapped(object sender, TappedRoutedEventArgs e)
       {
           ZoomCurrentLoc();
       }


       private void Translate()
       {
           ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
           if (localSettings.Values["Idioma"].ToString() == "pt")
               return;

           DatabaseHelperClass dbhandle = new DatabaseHelperClass();
           MainPage.instance.change_title(dbhandle.Translate("Contactos Úteis"));
           teleBlc.Text = dbhandle.Translate(teleBlc.Text);

       }

       private async void MapItemsStack_Holding(object sender, HoldingRoutedEventArgs e)
       {
           var selectedItem = (Entidade)(sender as StackPanel).DataContext;
           Debug.WriteLine("new ROUTE TO: " + selectedItem.Nome);
           MapRouteFinderResult routeResult = await MapRouteFinder.GetDrivingRouteAsync(this.current, selectedItem.geo, MapRouteOptimization.Time, MapRouteRestrictions.None, 290);


           if (routeResult.Status == MapRouteFinderStatus.Success)
           {
               // Use the route to initialize a MapRouteView.
               try
               {
                   map1.Routes.RemoveAt(0);
               }
               catch
               {
                   //no route to remove
               }
               MapRouteView viewOfRoute = new MapRouteView(routeResult.Route);
               viewOfRoute.RouteColor = Colors.Blue;
               viewOfRoute.OutlineColor = Colors.Blue;
               // Add the new MapRouteView to the Routes collection
               // of the MapControl.
               map1.Routes.Add(viewOfRoute);
               // Fit the MapControl to the route.
               await map1.TrySetViewBoundsAsync(
                 routeResult.Route.BoundingBox,
                 null,
                 Windows.UI.Xaml.Controls.Maps.MapAnimationKind.Bow);
           }
       }

       private async void Plan_Route(Entidade e)
       {
           MapRouteFinderResult routeResult = await MapRouteFinder.GetDrivingRouteAsync(this.current, e.geo, MapRouteOptimization.Time, MapRouteRestrictions.None, 290);


           if (routeResult.Status == MapRouteFinderStatus.Success)
           {
               // Use the route to initialize a MapRouteView.
               try
               {
                   map1.Routes.RemoveAt(0);
               }
               catch
               {
                   //no route to remove
               }
               MapRouteView viewOfRoute = new MapRouteView(routeResult.Route);
               viewOfRoute.RouteColor = Colors.Blue;
               viewOfRoute.OutlineColor = Colors.Blue;
               // Add the new MapRouteView to the Routes collection
               // of the MapControl.
               map1.Routes.Add(viewOfRoute);
               // Fit the MapControl to the route.
               await map1.TrySetViewBoundsAsync(
                 routeResult.Route.BoundingBox,
                 null,
                 Windows.UI.Xaml.Controls.Maps.MapAnimationKind.Bow);
           }
       }

    }
}

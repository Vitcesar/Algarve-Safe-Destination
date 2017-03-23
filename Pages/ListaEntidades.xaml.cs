using ADS.Classes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Phone.UI.Input;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace ADS.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ListaEntidades : Page
    {

        private ObservableCollection<Classes.Entidade> EntList;

        public ListaEntidades()
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
            DatabaseHelperClass dbtemas = new DatabaseHelperClass();
            ObservableCollection<Classes.Entidade> EntList = dbtemas.ReadEntidade();
            ObservableCollection<Classes.Tipo_Entidade_Idioma> TypeEntList = dbtemas.ReadTipoEntidade();
            this.EntList = EntList;
            CalcDist();
            TypeEntList.Add(new Classes.Tipo_Entidade_Idioma("en", "All", "All"));

            listBoxobj.ItemsSource = EntList.OrderBy(i => i.dist).ToList();
            FilterBox.ItemsSource = TypeEntList.OrderBy(i => i.Nome_Idioma).ToList();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Pages.Contactos), null);
        }



        private void FilterBox_ItemsPicked(ListPickerFlyout sender, ItemsPickedEventArgs args)
        {
            Classes.Tipo_Entidade_Idioma t = new Classes.Tipo_Entidade_Idioma();
            t = FilterBox.SelectedItem as Classes.Tipo_Entidade_Idioma;

            if (t.Nome_Idioma == "All")
            {
                listBoxobj.ItemsSource = this.EntList.OrderBy(i => i.Nome).ToList();
            }
            else
            {
                ObservableCollection<Classes.Entidade> TempTypeEntList = new ObservableCollection<Entidade>();
                foreach (var ent in this.EntList)
                {
                    if (ent.Nome_Tipo_Entidade == t.Nome_Idioma)
                    {
                        TempTypeEntList.Add(ent);
                    }
                }
                listBoxobj.ItemsSource = TempTypeEntList;
            }
        }



        private async void CalcDist()
        {
            var geolocator = new Geolocator();
            Geoposition position;
            Position CurrentP = new Position();

            try
            {
                // Carry out the operation  Geoposition pos =    
                position = MainPage.instance.getPos();
                CurrentP.Latitude = position.Coordinate.Latitude;
                CurrentP.Longitude = position.Coordinate.Longitude;

            }
            catch
            {
                //Operation aborted Your App does not have permission to access location data.
                //Launcher.LaunchUriAsync(new Uri("ms-settings-location:"));
            }


            DatabaseHelperClass dbtemas = new DatabaseHelperClass();

            foreach (var E in EntList)
            {
                double lat = dbtemas.latitude(E.N_Vertice);
                double lon = dbtemas.longitude(E.N_Vertice);
                Position TargetP = new Position();
                TargetP.Latitude = lat;
                TargetP.Longitude = lon;

                E.dist = Distance(CurrentP, TargetP, DistanceType.Kilometers);
            }

        }

        public struct Position
        {
            public double Latitude;
            public double Longitude;
        }

        public enum DistanceType { Miles, Kilometers };

        public double Distance(Position pos1, Position pos2, DistanceType type)
        {
            return Math.Sqrt(Math.Pow(pos1.Latitude - pos2.Latitude, 2) + Math.Pow(pos1.Longitude - pos2.Longitude, 2));
        }

        private void SortDist_Click(object sender, RoutedEventArgs e)
        {
            listBoxobj.ItemsSource = EntList.OrderBy(i => i.dist).ToList();
        }

        private void SortName_Click(object sender, RoutedEventArgs e)
        {
            listBoxobj.ItemsSource = EntList.OrderBy(i => i.Nome).ToList();
        }

        private void SortType_Click(object sender, RoutedEventArgs e)
        {
            listBoxobj.ItemsSource = EntList.OrderBy(i => i.Nome_Tipo_Entidade).ToList();
        }

        private void listBoxobj_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Classes.Entidade m = new Classes.Entidade();
            m = listBoxobj.SelectedItem as Classes.Entidade;

            // If selected index is -1 (no selection) do nothing
            if (listBoxobj.SelectedIndex == -1)
                return;

            // Navigate to the new page
            Frame.Navigate(typeof(Pages.Detalhe_Entidade), m);

            // Reset selected index to -1 (no selection)
            listBoxobj.SelectedIndex = -1;
        }

        private void Translate()
        {
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
            if (localSettings.Values["Idioma"].ToString() == "pt")
                return;

            DatabaseHelperClass dbhandle = new DatabaseHelperClass();
            MainPage.instance.change_title(dbhandle.Translate("Contactos Úteis"));
            ContactBlock.Text = dbhandle.Translate(ContactBlock.Text);
            mapBtn.Content = dbhandle.Translate(mapBtn.Content.ToString());
            SortDist.Text = dbhandle.Translate(SortDist.Text);
            SortName.Text = dbhandle.Translate(SortName.Text);
            SortType.Text = dbhandle.Translate(SortType.Text);
            orderText.Text = dbhandle.Translate(orderText.Text);
        }
    }
}

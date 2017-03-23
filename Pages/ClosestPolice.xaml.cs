using ADS.Classes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Email;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace ADS.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ClosestPolice : Page
    {

        private ObservableCollection<Classes.Entidade> EntList;
        private Entidade closest;

        public ClosestPolice()
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
            MainPage.instance.change_title("Esquadra mais próxima");
            DatabaseHelperClass dbtemas = new DatabaseHelperClass();
            ObservableCollection<Classes.Entidade> EntList = dbtemas.ReadEntidade();
            this.EntList = EntList;
            CalcDist();
            closest_police();
            NomeEnt.Text = closest.Nome;
            MoradaEnt.Text = closest.Morada;
            CodPostalEnt.Text = closest.Codigo_Postal;

            TeleEnt.Text = closest.Telefone.ToString();
            EmailEnt.Text = closest.Email;

            if (closest.Fax.ToString() == "0")
            {
                FaxEnt.Text = "Não Disponível";
                FaxEnt.Foreground = new SolidColorBrush(Colors.Gray);
            }
            else
            {
                FaxEnt.Text = closest.Fax.ToString();
            }

            ImageEnt.Source = new BitmapImage(new Uri(ConfigData.Iconpath + closest.Anexo, UriKind.Absolute));
            MapFrame.Navigate(typeof(Pages.Contactos), closest);
            Translate();
        }

        private void Translate()
        {
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
            if (localSettings.Values["Idioma"].ToString() == "pt")
                return;

            DatabaseHelperClass dbhandle = new DatabaseHelperClass();
            MainPage.instance.change_title(dbhandle.Translate("Esquadra mais próxima"));
            TeleBlc.Text = dbhandle.Translate(TeleBlc.Text);
            TeleEnt.Margin = new Thickness(0, 20, 10, 0);
            Destinationbox.Text = dbhandle.Translate(Destinationbox.Text);
            if (FaxEnt.Text == "Não Disponível")
                FaxEnt.Text = dbhandle.Translate(FaxEnt.Text);
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


        private async void closest_police()
        {
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
            string polstring = "Policia";
            if (localSettings.Values["Idioma"].ToString() != "pt")
            {
                DatabaseHelperClass dbhandle = new DatabaseHelperClass();
                polstring = dbhandle.Translate("Policia");
            }

            ObservableCollection<Classes.Entidade> TempTypeEntList = new ObservableCollection<Entidade>();
            foreach (var ent in this.EntList)
            {
                if (ent.Nome_Tipo_Entidade == polstring)
                {
                    ent.closest_police = true;
                    TempTypeEntList.Add(ent);
                }
            }
            this.EntList = TempTypeEntList;
            this.closest = EntList.OrderBy(i => i.dist).ToList().ElementAt(0);
        }

        private void TeleEnt_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Windows.ApplicationModel.Calls.PhoneCallManager.ShowPhoneCallUI(this.closest.Telefone.ToString(), this.closest.Nome);
        }

        private async void EmailEnt_Tapped(object sender, TappedRoutedEventArgs e)
        {
            EmailRecipient sendTo = new EmailRecipient()
            {
                Address = EmailEnt.Text
            };

            EmailMessage mail = new EmailMessage();

            mail.To.Add(sendTo);
            await EmailManager.ShowComposeNewEmailAsync(mail);
        }

        private void Destinationbox_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.closest.dest = true;
            Frame.Navigate(typeof(Pages.Contactos), closest);
        }
    }


}

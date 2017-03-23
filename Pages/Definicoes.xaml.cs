using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using ADS.Classes;
using Windows.UI.Xaml.Media.Imaging;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using System.Diagnostics;
using Windows.Phone.UI.Input;
using System.Globalization;
using System.Collections.ObjectModel;
using Windows.Storage;
using Windows.System;
using Windows.Devices.Geolocation;
using Windows.UI;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace ADS.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Definicoes : Page
    {
        ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;

        public static Definicoes instance
        {
            get;
            private set;
        }

        public Definicoes()
        {
            this.InitializeComponent();
            instance = this;
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            MainPage.instance.change_title("Definições");
            Translate();
            DatabaseHelperClass dbidiomas = new DatabaseHelperClass();
            ObservableCollection<Classes.Idioma> myLang = dbidiomas.ReadIdiomas();//Get all DB contacts
            string pht = "Selected Language";

            foreach (var lang in myLang)
            {
                if(localSettings.Values["Idioma"].ToString() == lang.Codigo_Idioma)
                   pht = lang.Nome;
            }

            ComboBoxIdioma.PlaceholderText = pht;
            ComboBoxIdioma.ItemsSource = myLang.ToList();

            Geolocator locator = new Geolocator();
            if (locator.LocationStatus == PositionStatus.Disabled)
            {
                geoText.Visibility = Visibility.Visible;
            }
        }

        public void block_update()
        {
            syncBtn.BorderBrush = new SolidColorBrush(Colors.Gray);
            syncBtn.Foreground = new SolidColorBrush(Colors.Gray);
        }

        private void IdiomaComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Classes.Idioma i = new Classes.Idioma();
            i = ComboBoxIdioma.SelectedItem as Classes.Idioma;
            localSettings.Values["Idioma"] = i.Codigo_Idioma;
            Frame.Navigate(typeof(Pages.MainMenu));
        }

        private void ToggleSwitch_Toggled(object sender, RoutedEventArgs e)
        {

        }

        private void Translate()
        {
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
            if (localSettings.Values["Idioma"].ToString() == "pt")
                return;

            DatabaseHelperClass dbhandle = new DatabaseHelperClass();
            MainPage.instance.change_title(dbhandle.Translate("Definições"));
            PlaceholderTextBlock.Text = dbhandle.Translate(PlaceholderTextBlock.Text);
            toggleSwitch1.Header = dbhandle.Translate(toggleSwitch1.Header.ToString());
            toggleSwitch1.OnContent = dbhandle.Translate(toggleSwitch1.OnContent.ToString());
            toggleSwitch1.OffContent = dbhandle.Translate(toggleSwitch1.OffContent.ToString());
            toggleSwitch2.Header = dbhandle.Translate(toggleSwitch2.Header.ToString());
            toggleSwitch2.OnContent = dbhandle.Translate(toggleSwitch2.OnContent.ToString());
            toggleSwitch2.OffContent = dbhandle.Translate(toggleSwitch2.OffContent.ToString());
            syncBtn.Content = dbhandle.Translate(syncBtn.Content.ToString());
            geoText.Text = dbhandle.Translate(geoText.Text);
        }

        private void geoText_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Launcher.LaunchUriAsync(new Uri("ms-settings-location:"));
        }

        private void syncBtn_Click(object sender, RoutedEventArgs e)
        {
            Update up = new Update();
        }
    }
}

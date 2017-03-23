using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Phone.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Storage;
using System.Diagnostics;
using ADS.Classes;
using Windows.Devices.Geolocation;
using Windows.System;
using Windows.UI.Core;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace ADS
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>

    public sealed partial class MainPage : Page
    {
        private Geoposition pos;
        private Geolocator locator = null;
        private CoreDispatcher dispatcher; 

        public static MainPage instance
        {
            get;
            private set;
        }

        public MainPage()
        {
            this.InitializeComponent();
            dispatcher = Window.Current.CoreWindow.Dispatcher;  
            instance = this;
            this.NavigationCacheMode = NavigationCacheMode.Required;
            HardwareButtons.BackPressed += HardwareButtons_BackPressed;
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {

            //Better Geolocation Test
            if (locator == null)
            {
                locator = new Geolocator();
            }
            if (locator != null)
            {
                locator.MovementThreshold = 3;

                locator.PositionChanged +=
                    new TypedEventHandler<Geolocator,
                        PositionChangedEventArgs>(locator_PositionChanged);
            }    

            //StartGPS();
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
            Debug.WriteLine("FLAG= " + localSettings.Values["FIRST_RUN_FLAG"].Equals("false"));

            if (localSettings.Values["FIRST_RUN_FLAG"].Equals("false"))
            {
                appBar.Visibility = Visibility.Collapsed;
                searchpanel.Visibility = Visibility.Collapsed;
                Frame1.Navigate(typeof(Pages.FirstRun), this);
            }
            else
            {
                Frame1.Navigate(typeof(Pages.MainMenu), this);
            }

            Translate();

            // TODO: If your application contains multiple pages, ensure that you are
            // handling the hardware Back button by registering for the
            // Windows.Phone.UI.Input.HardwareButtons.BackPressed event.
            // If you are using the NavigationHelper provided by some templates,
            // this event is handled for you.
        }

        async private void locator_PositionChanged(Geolocator sender, PositionChangedEventArgs e)
        {
            await dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                Geoposition geoPosition = e.Position;
                this.pos = geoPosition;
            });
        }   


        //Not Used anymore
        async public void StartGPS()
        {
            var geolocator = new Geolocator();
            try
            {
                // Carry out the operation  Geoposition pos =    
                this.pos = await geolocator.GetGeopositionAsync(maximumAge: TimeSpan.FromSeconds(30),
            timeout: TimeSpan.FromSeconds(10));
            }
            catch
            {
                //Operation aborted Your App does not have permission to access location data.
                //Launcher.LaunchUriAsync(new Uri("ms-settings-location:"));
            }
        }

        public Geoposition getPos()
        { 
            return this.pos;
;
        }

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            Frame1.Navigate(typeof(Pages.MainMenu), this);
        }

        private void SOS_Button_Click(object sender, RoutedEventArgs e)
        {
            Windows.ApplicationModel.Calls.PhoneCallManager.ShowPhoneCallUI("112", "Emergency Services");

        }

        void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            if (Frame1 == null)
            {
                return;
            }

            if (Frame1.CanGoBack)
            {
                Frame1.GoBack();
                e.Handled = true;
            }
        }

        private void Settings_Button_Click(object sender, RoutedEventArgs e)
        {
            Frame1.Navigate(typeof(Pages.Definicoes), this);
        }

        public void toggle_visibility()
        {
            appBar.Visibility = Visibility.Visible;
            searchpanel.Visibility = Visibility.Visible;
        }

        private void search_button_Click(object sender, RoutedEventArgs e)
        {
            bool isValid = searchbox.Text.Length >= 3;
            
            if (isValid)
            {
                Frame1.Navigate(typeof(Pages.SearchResults), searchbox.Text);
            }
        }

        private void searchbox_KeyDown(object sender, KeyRoutedEventArgs e)
        {            
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                search_button_Click(sender, e);
            }
        }

        public void seach_clear()
        {
            searchbox.Text = "";
            LoseFocus(searchbox);

        }

        private void LoseFocus(object sender)
        {
            var control = sender as Control;
            var isTabStop = control.IsTabStop;
            control.IsTabStop = false;
            control.IsEnabled = false;
            control.IsEnabled = true;
            control.IsTabStop = isTabStop;
        }


        public void change_title(string s)
        {
            AppTitle.Text = s;
        }

        public void set_frame1(Frame a)
        {
            Frame1 = a;
        }

        private void Translate()
        {
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
            if (localSettings.Values["Idioma"].ToString() == "pt")
                return;

            DatabaseHelperClass dbhandle = new DatabaseHelperClass();
            Home_Button.Label = dbhandle.Translate(Home_Button.Label);
            Help_Button.Label = dbhandle.Translate(Help_Button.Label);
            Settings_Button.Label = dbhandle.Translate(Settings_Button.Label);
        }
    }
}

using ADS.Classes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
    public sealed partial class FirstRun : Page
    {
        public FirstRun()
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
            Translate();
            DatabaseHelperClass dbhandle = new DatabaseHelperClass();
            ObservableCollection<Classes.Idioma> myLang = dbhandle.ReadIdiomas();
            ObservableCollection<Classes.Pais> myCount = dbhandle.ReadPaises();
            ComboBoxLang.ItemsSource = myLang.OrderBy(i => i.Nome).ToList();
            ComboBoxCountry.ItemsSource = myCount.OrderBy(i => i.Nome).ToList();

            CultureInfo ci = new CultureInfo(Windows.System.UserProfile.GlobalizationPreferences.Languages[0]);
            string SOlang = ci.TwoLetterISOLanguageName;
            Boolean langfound = false; 

            foreach (var lang in myLang)
            {
                if (SOlang == lang.Codigo_Idioma)
                    langfound = true;
            }

            if (!langfound)
            {
                NoLangPanel.Visibility = Visibility.Visible;
                btnPanel.Margin = new Thickness(0, 0, 0, 0);
            }

        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Pages.MainMenu), this);
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
            localSettings.Values["Idioma"] = "en";
            localSettings.Values.Remove("Pais");
            Frame.Navigate(typeof(Pages.MainMenu), this);
        }

        private void ComboBoxCountry_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Pais p = ComboBoxCountry.SelectedItem as Classes.Pais;
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
            localSettings.Values["Pais"] = p.Nome;
            //Debug.WriteLine("Pais: " + localSettings.Values["Pais"]);
        }

        private void ComboBoxLang_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Classes.Idioma i = new Classes.Idioma();
            i = ComboBoxLang.SelectedItem as Classes.Idioma;
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
            localSettings.Values["Idioma"] = i.Codigo_Idioma;
        }

        private void Translate()
        {
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
            if (localSettings.Values["Idioma"].ToString() == "pt")
               return;

            DatabaseHelperClass dbhandle = new DatabaseHelperClass();
            welcometext.Text = dbhandle.Translate(welcometext.Text);
            CountryBlock.Text = dbhandle.Translate(CountryBlock.Text);
            ComboBoxCountry.PlaceholderText = dbhandle.Translate(ComboBoxCountry.PlaceholderText);
            sadtext.Text = dbhandle.Translate(sadtext.Text);
            LangBox.Text = dbhandle.Translate(LangBox.Text);
            Cancel.Content = dbhandle.Translate(Cancel.Content.ToString());
            Submit.Content = dbhandle.Translate(Submit.Content.ToString());
        }
    }
}

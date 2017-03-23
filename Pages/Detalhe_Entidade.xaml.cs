using ADS.Classes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Email;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Phone.UI.Input;
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
    public sealed partial class Detalhe_Entidade : Page
    {
        private Classes.Entidade ent;

        public Detalhe_Entidade()
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
            Classes.Entidade ent = e.Parameter as Classes.Entidade;
            this.ent = ent;
            NomeEnt.Text = ent.Nome;
            MoradaEnt.Text = ent.Morada;
            CodPostalEnt.Text = ent.Codigo_Postal;

            TeleEnt.Text = ent.Telefone.ToString();
            EmailEnt.Text = ent.Email;

            if (ent.Fax.ToString() == "0")
            {
                FaxEnt.Text = "Não Disponível";
                FaxEnt.Foreground = new SolidColorBrush(Colors.Gray);
            }
            else
            {
                FaxEnt.Text = ent.Fax.ToString();
            }

            ImageEnt.Source = new BitmapImage(new Uri(ConfigData.Iconpath + ent.Anexo, UriKind.Absolute));
            Translate();
            MapFrame.Navigate(typeof(Pages.Contactos), ent);
        }

        private void TeleEnt_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Windows.ApplicationModel.Calls.PhoneCallManager.ShowPhoneCallUI(this.ent.Telefone.ToString(), this.ent.Nome);
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

        private void Translate()
        {
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
            if (localSettings.Values["Idioma"].ToString() == "pt")
                return;

            DatabaseHelperClass dbhandle = new DatabaseHelperClass();
            TeleBlc.Text = dbhandle.Translate(TeleBlc.Text);
            TeleEnt.Margin = new Thickness(0, 20, 10, 0);
            Destinationbox.Text = dbhandle.Translate(Destinationbox.Text);
            if (FaxEnt.Text == "Não Disponível")
                FaxEnt.Text = dbhandle.Translate(FaxEnt.Text);
        }

        private void Destinationbox_Tapped(object sender, TappedRoutedEventArgs e)
        {
            ent.dest = true;
            Frame.Navigate(typeof(Pages.Contactos), ent);
        }

    }
}

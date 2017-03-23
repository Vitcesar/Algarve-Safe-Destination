using ADS.Classes;
using System;
using Windows.Devices.Geolocation;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace ADS.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainMenu : Page
    {
        public MainMenu()
        {
            this.InitializeComponent();
        }

        ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
        DatabaseHelperClass dbtemas = new DatabaseHelperClass();

        TextBlock Vitima_TextBlock = new TextBlock();
        TextBlock Conselhos_TextBlock = new TextBlock();
        TextBlock Problema_TextBlock = new TextBlock();
        TextBlock Contactos_TextBlock = new TextBlock();
        TextBlock Esquadra_TextBlock = new TextBlock();
        TextBlock Perguntas_TextBlock = new TextBlock();

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            MainPage.instance.toggle_visibility();
            MainPage.instance.change_title("Algarve Destino Seguro");

            //Translate();
            /*Binding binding = new Binding();
            Binding binding2 = new Binding();
            Binding binding3 = new Binding();*/

        }

        private void Contactos_Button_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Pages.ListaEntidades), this);
        }

        private void Vitima_Text_Loaded(object sender, RoutedEventArgs e)
        {
            Vitima_TextBlock = sender as TextBlock;
            Classes.Conteudo_Tema Vitima = dbtemas.ReadTemaTitle("Fui Vítima de Crime");
            /*binding.Source = Vitima;
            binding.Path = new PropertyPath("Titulo");
            BindingOperations.SetBinding(Vitima_TextBlock, TextBlock.TextProperty, binding);*/
            Vitima_TextBlock.Text = Vitima.Titulo as string;
        }

        private void Conselhos_Text_Loaded(object sender, RoutedEventArgs e)
        {
            Conselhos_TextBlock = sender as TextBlock;
            Classes.Conteudo_Tema Conselhos = dbtemas.ReadTemaTitle("Conselhos de Segurança");
            /*binding3.Source = Conselhos;
            binding3.Path = new PropertyPath("Titulo");
            BindingOperations.SetBinding(Conselhos_TextBlock, TextBlock.TextProperty, binding);*/
            Conselhos_TextBlock.Text = Conselhos.Titulo as string;
        }

        private void Problema_Text_Loaded(object sender, RoutedEventArgs e)
        {
            Problema_TextBlock = sender as TextBlock;
            Classes.Conteudo_Tema Problema = dbtemas.ReadTemaTitle("Tive um Problema");
            /*binding2.Source = Problema;
            binding2.Path = new PropertyPath("Titulo");
            BindingOperations.SetBinding(Problema_TextBlock, TextBlock.TextProperty, binding);*/
            Problema_TextBlock.Text = Problema.Titulo as string;
        }

        private void Vitima_Button_Click(object sender, RoutedEventArgs e)
        {
            // Navigate to the new page
            Frame.Navigate(typeof(Pages.Topicos), "Fui Vítima de Crime");
        }

        private void Conselhos_Button_Click(object sender, RoutedEventArgs e)
        {
            // Navigate to the new page
            Frame.Navigate(typeof(Pages.Topicos), "Conselhos de Segurança");
        }

        private void Problema_Button_Click(object sender, RoutedEventArgs e)
        {
            // Navigate to the new page
            Frame.Navigate(typeof(Pages.Topicos), "Tive um Problema");
        }

        private void Contactos_Text_Loaded(object sender, RoutedEventArgs e)
        {
            Contactos_TextBlock = sender as TextBlock;

            if (localSettings.Values["Idioma"].ToString() == "pt")
                return;
            string c = Contactos_TextBlock.Text.ToString();
            Contactos_TextBlock.Text = Translate(c);
        }

        private void Esquadra_Text_Loaded(object sender, RoutedEventArgs e)
        {
            Esquadra_TextBlock = sender as TextBlock;

            if (localSettings.Values["Idioma"].ToString() == "pt")
                return;
            string c = Esquadra_TextBlock.Text.ToString();
            Esquadra_TextBlock.Text = Translate(c);
        }

        private void Perguntas_Text_Loaded(object sender, RoutedEventArgs e)
        {
            Perguntas_TextBlock = sender as TextBlock;

            if (localSettings.Values["Idioma"].ToString() == "pt")
                return;
            string c = Perguntas_TextBlock.Text.ToString();
            Perguntas_TextBlock.Text = Translate(c);
        }

        private string Translate(string s)
        {
            //Debug.WriteLine("Sou o Translate e estou a correr!");

            DatabaseHelperClass dbhandle = new DatabaseHelperClass();
            s = dbhandle.Translate(s);
            return s;

            /*Contactos_TextBlock.Text = dbhandle.Translate(Contactos_TextBlock.Text);
            Esquadra_TextBlock.Text = dbhandle.Translate(Esquadra_TextBlock.Text);
            Perguntas_TextBlock.Text = dbhandle.Translate(Perguntas_TextBlock.Text);*/
        }

        private void ClosestPolice_Button_Click(object sender, RoutedEventArgs e)
        {
            Geolocator locator = new Geolocator();
            if (locator.LocationStatus != PositionStatus.Disabled)
            {
                Frame.Navigate(typeof(Pages.ClosestPolice));
            }
        }

        private void Image_Loaded(object sender, RoutedEventArgs e)
        {
            Geolocator locator = new Geolocator();
            if (locator.LocationStatus == PositionStatus.Disabled)
            {
                Image a = sender as Image;
                a.Source = new BitmapImage(new Uri("ms-appx:///Icons/comando_off.png", UriKind.Absolute));
            }

        }

        private void Faq_Button_Click(object sender, RoutedEventArgs e)
        {
            Topico t = new Topico(null, "FAQ", null, null, null);
            Frame.Navigate(typeof(Pages.Conteudo), t);
        }

        
    }
}

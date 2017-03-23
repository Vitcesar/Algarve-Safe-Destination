using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace ADS.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Temas : Page
    {
        public Temas()
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
            MainPage.instance.change_title("Temas");

            DatabaseHelperClass dbtemas = new DatabaseHelperClass();
            ObservableCollection<Classes.Conteudo_Tema> TemasList = dbtemas.ReadTemas();//Get all DB contacts
            foreach (var Temas in TemasList)
            {
                Temas.parent_tema.Icone = ConfigData.Iconpath + Temas.parent_tema.Icone;
            }
            listBoxobj.ItemsSource = TemasList.OrderByDescending(i => i.Titulo).ToList();//Latest contact ID can Display first
        }

        public async Task<BitmapImage> ConvertToBitmapImage(byte[] image)
        {
            BitmapImage bitmapimage = null;
            using (InMemoryRandomAccessStream ms = new InMemoryRandomAccessStream())
            {
                using (DataWriter writer = new DataWriter(ms.GetOutputStreamAt(0)))
                {
                    writer.WriteBytes((byte[])image);
                    await writer.StoreAsync();
                }
                bitmapimage = new BitmapImage();
                bitmapimage.SetSource(ms);

            }
            return bitmapimage;
        }

        private void listBoxobj_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Classes.Conteudo_Tema m = new Classes.Conteudo_Tema();
            m = listBoxobj.SelectedItem as Classes.Conteudo_Tema;
            
            // If selected index is -1 (no selection) do nothing
            if (listBoxobj.SelectedIndex == -1)
                return;

            // Navigate to the new page
            Frame.Navigate(typeof(Pages.Topicos), m.parent_tema.Titulo);

            // Reset selected index to -1 (no selection)
            listBoxobj.SelectedIndex = -1;
        }

    }
}

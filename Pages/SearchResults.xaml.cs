using ADS.Classes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public sealed partial class SearchResults : Page
    {
        public SearchResults()
        {
            this.InitializeComponent();
        }

        TextBlock TituloTxtBlock = new TextBlock();
        string NewTitle = "Resultados da Pesquisa";

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Translate();

            MainPage.instance.change_title(NewTitle);
            MainPage.instance.seach_clear();

            string Text = e.Parameter.ToString();
            DatabaseHelperClass dbtemas = new DatabaseHelperClass();
            ObservableCollection<Search_Result> ResultList = dbtemas.Search(Text);
            listBoxobj.ItemsSource = ResultList.ToList();
        }

        private void listBoxobj_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Classes.Search_Result m = new Classes.Search_Result();
            m = listBoxobj.SelectedItem as Classes.Search_Result;

            // If selected index is -1 (no selection) do nothing
            if (listBoxobj.SelectedIndex == -1)
                return;


            if (m.tipo == "tema")
                Frame.Navigate(typeof(Pages.Topicos), m.tema.ID_Tema);
            
            else if (m.tipo == "topico")
            {
                // Check if Subtopic Exists
                string s = Convert.ToString(m.topico.ID_Topico);
                DatabaseHelperClass dbtopicos = new DatabaseHelperClass();
                Boolean exists = dbtopicos.SubtopicExist(s);
                if (exists)
                {
                    // Navigate to the new page
                    Frame.Navigate(typeof(Pages.Subtopicos), m.topico.ID_Topico);
                }
                else
                {
                    // Navigate to the new page
                    Frame.Navigate(typeof(Pages.Conteudo), m.topico.ID_Topico);
                }
            }

            else if (m.tipo == "subtopico")
                Frame.Navigate(typeof(Pages.Conteudo), m.subtopico.ID_Subtopico);

            else if (m.tipo == "entidade")
                // Navigate to the new page
                Frame.Navigate(typeof(Pages.Detalhe_Entidade), m.entidade);


            // Reset selected index to -1 (no selection)
            listBoxobj.SelectedIndex = -1;
        }

        private void Translate()
        {
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
            if (localSettings.Values["Idioma"].ToString() == "pt")
                return;

            DatabaseHelperClass dbhandle = new DatabaseHelperClass();
            NewTitle = dbhandle.Translate(NewTitle);
        }
    }
}

using ADS.Classes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace ADS.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Topicos : Page
    {
        public Topicos()
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
            string idtema = e.Parameter.ToString();
            DatabaseHelperClass dbtopicos = new DatabaseHelperClass();
            Conteudo_Tema TemaTitle = dbtopicos.ReadTemaTitle(idtema);
            
            MainPage.instance.change_title(TemaTitle.Titulo.ToString());

            ObservableCollection<Classes.Conteudo_Topico> TopicsList = dbtopicos.ReadTopicos(idtema);//Get all DB contacts
            foreach (var Topico in TopicsList)
            {
                Topico.parent_topico.Icone = ConfigData.Iconpath + Topico.parent_topico.Icone;
            }
            listBoxobj.ItemsSource = TopicsList.OrderByDescending(i => i.Titulo).ToList();//Latest contact ID can Display first
        }

        private void listBoxobj_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Classes.Conteudo_Topico m = new Classes.Conteudo_Topico();
            m = listBoxobj.SelectedItem as Classes.Conteudo_Topico;

            // If selected index is -1 (no selection) do nothing
            if (listBoxobj.SelectedIndex == -1)
                return;

            // Check if Subtopic Exists
            string s = Convert.ToString(m.parent_topico.Titulo);
            DatabaseHelperClass dbtopicos = new DatabaseHelperClass();
            Boolean exists = dbtopicos.SubtopicExist(s);
            if (exists)
            {
                // Navigate to the new page
                Frame.Navigate(typeof(Pages.Subtopicos), m.parent_topico.Titulo);
            }
            else
            {
                // Navigate to the new page
                Frame.Navigate(typeof(Pages.Conteudo), m.parent_topico);
            }

            // Reset selected index to -1 (no selection)
            listBoxobj.SelectedIndex = -1;
        }
    }
}

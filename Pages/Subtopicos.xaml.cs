using ADS.Classes;
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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace ADS.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Subtopicos : Page
    {
        public Subtopicos()
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
            string idtopico = e.Parameter.ToString();
            DatabaseHelperClass dbtopicos = new DatabaseHelperClass();
            Conteudo_Topico TopicTitle = dbtopicos.ReadTopicTitle(idtopico);
            
            MainPage.instance.change_title(TopicTitle.Titulo.ToString());

            ObservableCollection<Classes.Conteudo_Subtopico> SubtopicsList = dbtopicos.ReadSubtopicos(idtopico);//Get all DB contacts
            foreach (var Subtopico in SubtopicsList)
            {
                Subtopico.parent_subtopico.Icone = ConfigData.Iconpath + Subtopico.parent_subtopico.Icone;
            }
            listBoxobj.ItemsSource = SubtopicsList.OrderByDescending(i => i.Titulo).ToList();//Latest contact ID can Display first
        }

        private void listBoxobj_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Classes.Conteudo_Subtopico m = new Classes.Conteudo_Subtopico();
            m = listBoxobj.SelectedItem as Classes.Conteudo_Subtopico;

            // If selected index is -1 (no selection) do nothing
            if (listBoxobj.SelectedIndex == -1)
                return;

            // Navigate to the new page
            Frame.Navigate(typeof(Pages.Conteudo), m.parent_subtopico);

            // Reset selected index to -1 (no selection)
            listBoxobj.SelectedIndex = -1;
        }
    }
}

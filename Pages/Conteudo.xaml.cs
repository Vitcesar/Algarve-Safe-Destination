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
    public sealed partial class Conteudo : Page
    {
        public Conteudo()
        {
            this.InitializeComponent();
        }

        ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            DatabaseHelperClass dbtopicos = new DatabaseHelperClass();
            if(e.Parameter is Subtopico)
            {
                Subtopico st = e.Parameter as Subtopico;
                Conteudo_Subtopico SubtopicTitle = dbtopicos.ReadSubtopicTitle(st.Titulo);
                MainPage.instance.change_title(SubtopicTitle.Titulo.ToString());

                Uri targetUri = new Uri(ConfigData.Htmlpath + SubtopicTitle.Documento);
                webviewCont.Navigate(targetUri); 
            }
            else if (e.Parameter is Topico)
            {
                Topico t = e.Parameter as Topico;

                if (t.Titulo == "FAQ")
                {
                    if (localSettings.Values["Idioma"].ToString() == "pt")
                        MainPage.instance.change_title("Perguntas Frequentes");
                    else
                    {
                        DatabaseHelperClass dbhandle = new DatabaseHelperClass();
                        string s = dbhandle.Translate("Perguntas Frequentes");
                        MainPage.instance.change_title(s);
                    }

                    Uri targetUri = new Uri(ConfigData.Htmlpath + "faq_" + localSettings.Values["Idioma"].ToString() + ".html");
                    webviewCont.Navigate(targetUri);
                }
                else
                {
                    Conteudo_Topico TopicTitle = dbtopicos.ReadTopicTitle(t.Titulo);
                    MainPage.instance.change_title(TopicTitle.Titulo.ToString());

                    Uri targetUri = new Uri(ConfigData.Htmlpath + TopicTitle.Documento);
                    webviewCont.Navigate(targetUri);
                }
            }             
        }

    }
}

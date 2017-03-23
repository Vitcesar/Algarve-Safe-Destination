using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace ADS.Classes
{
    public class ConfigData
    {
        static ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
        private static string iconpath = localSettings.Values["IconPath"].ToString() + "\\";
        private static string htmlpath = "ms-appx-web:///HTML/";

        public static string Iconpath
        {
            get { return iconpath; }
            set { iconpath = value; }
        }

        public static string Htmlpath
        {
            get { return htmlpath; }
            set { htmlpath = value; }
        }

    }
}

using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace MAL_UWP_Nightmare
{
    interface IPage
    {
        void SetContent(JObject json);
        void SetImages(BitmapImage[] images);
        void SetMainImage(BitmapImage image);
        void SetInfo(string[] info);
        bool SavePage();
        void LoadPage();
        bool IsLocal();
        void SetErrorContent(string errorMessage);
    }
}

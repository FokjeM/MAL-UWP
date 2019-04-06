using System;
using System.Collections.Generic;
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
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace MAL_UWP_Nightmare
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private bool bgflipped = false;

        public MainPage()
        {
            this.InitializeComponent();
            (Window.Current.Content as Frame).Background = new ImageBrush { Stretch = Stretch.Fill, ImageSource = new BitmapImage(new Uri("ms-appx:///Assets/StoreLogo.png")) };
            (Window.Current.Content as Frame).SizeChanged += TapHandle;
            JikanAPIState jas = new JikanAPIState();
            System.Diagnostics.Debug.Write(jas.requestAPI(jas.getRequestFromSearch("manga/mahou sensei negima!").Result).Result);
        }

        private void TapHandle(object sender, SizeChangedEventArgs e)
        {
            if (bgflipped)
            {
                (Window.Current.Content as Frame).Background = new ImageBrush { Stretch = Stretch.Fill, ImageSource = new BitmapImage(new Uri("ms-appx:///Assets/PokeBG.png")) };
            } else
            {
                (Window.Current.Content as Frame).Background = new ImageBrush { Stretch = Stretch.Fill, ImageSource = new BitmapImage(new Uri("ms-appx:///Assets/SplashScreen.scale-200.png")) };
            }
            bgflipped = !bgflipped;
        }
    }
}

using System;
using System.Collections;
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

namespace MAL_UWP_Nightmare
{
    public sealed partial class HomePage : Page
    {
        public Dictionary<string,string> seasonalAnime { get; set; } //To do: Replace Value type string with BitmapImage.

        public HomePage()
        {
            this.InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            seasonalAnime = LoadSeasonalViewData();
            DataContext = this;
        }

        private Dictionary<string,string> LoadSeasonalViewData() //To do: Add parameter List<IPage> and fill Dictionary according to the List.
        {
            Dictionary<string,string> seasonalList = new Dictionary<string, string>();
            seasonalList.Add("Tsuujou Kougeki ga Zentai Kougeki de Ni-kai Kougeki no Okaasan wa Suki Desu ka?", "https://cdn.myanimelist.net/images/anime/1857/94908.jpg?s=ff1349992ecce2dc5b5b1ab3d4bf6846"); //To do: Remove test data.

            return seasonalList;
        }

        private void seasonalView_ItemClick(object sender, ItemClickEventArgs e)
        {

            Frame.Navigate(typeof(AnimeInfoPage), null);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(SearchResultsPage), null);
        }
    }
}

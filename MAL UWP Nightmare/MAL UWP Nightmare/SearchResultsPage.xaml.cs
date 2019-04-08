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
using Windows.UI.Xaml.Navigation;

namespace MAL_UWP_Nightmare
{
    public sealed partial class SearchResultsPage : Page
    {
        public Dictionary<string, string> SearchedAnime { get; set; } //To do: Replace Value type string with BitmapImage.
        public SearchResultsPage(List<SearchResult> sr)
        {
            this.InitializeComponent();
            this.results = results;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = this;
        }
       

        private void searchResultsView_ItemClick(object sender, ItemClickEventArgs e)
        {
            Dictionary<string, string> searchedList = new Dictionary<string, string>();
            searchedList.Add("Tsuujou Kougeki ga Zentai Kougeki de Ni-kai Kougeki no Okaasan wa Suki Desu ka?", "https://cdn.myanimelist.net/images/anime/1857/94908.jpg?s=ff1349992ecce2dc5b5b1ab3d4bf6846"); //To do: Remove test data.
            foreach(SearchResult s in sr)
            {
                searchedList.Add(s.title, s.image);
            }
            return searchedList;
        }

        private void SearchResultsView_ItemClick(object sender, ItemClickEventArgs e)
        {
            Frame.Navigate(typeof(AnimeInfoPage), null);
        }
    }
}

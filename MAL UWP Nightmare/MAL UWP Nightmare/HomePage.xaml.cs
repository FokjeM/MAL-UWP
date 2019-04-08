using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
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
        public Dictionary<string,string> SeasonalAnime { get; set; }
        Main main = new Main();

        public HomePage()
        {
            this.InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            SeasonalAnime = LoadSeasonalViewData();
            DataContext = this;
        }

        private Dictionary<string,string> LoadSeasonalViewData() //To do: Add parameter List<IPage> and fill Dictionary according to the List.
        {
            Dictionary<string,string> seasonalList = new Dictionary<string, string>();
            seasonalList.Add("Tsuujou Kougeki ga Zentai Kougeki de Ni-kai Kougeki no Okaasan wa Suki Desu ka?", "https://cdn.myanimelist.net/images/anime/1857/94908.jpg?s=ff1349992ecce2dc5b5b1ab3d4bf6846"); //To do: Remove test data.

            return seasonalList;
        }

        private void SeasonalView_ItemClick(object sender, ItemClickEventArgs e)
        {
        }

        private async void Button_Click(object sender, RoutedEventArgs e) //Anime
        {
            string text = searchInput.Text;
            if(text.Length > 2)
            {
                JikanAPIState state = new JikanAPIState();
                SearchPage s = new SearchPage(main);
                List<SearchResult> results = state.SearchAPI("anime/" + text);
                Window.Current.Content = new SearchResultsPage(results);
            }
            else
            {
                var dialog = new MessageDialog("The search query has to be at least 3 characters", "Error");
                dialog.Commands.Add(new UICommand("Ok"));
                dialog.DefaultCommandIndex = 0;
                dialog.CancelCommandIndex = 1;
                await dialog.ShowAsync();
            }
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e) //Manga
        {
            string text = searchInput.Text;
            if (text.Length > 2)
            {
                JikanAPIState state = new JikanAPIState();
                SearchPage s = new SearchPage(main);
                List<SearchResult> results = state.SearchAPI("manga/" + text);
                Window.Current.Content = new SearchResultsPage(results);
            }
            else
            {
                var dialog = new MessageDialog("The search query has to be at least 3 characters", "Error");
                dialog.Commands.Add(new UICommand("Ok"));
                dialog.DefaultCommandIndex = 0;
                dialog.CancelCommandIndex = 1;
                await dialog.ShowAsync();
            }
        }
    }
}

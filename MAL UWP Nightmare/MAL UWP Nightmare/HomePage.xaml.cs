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
        public List<SearchResult> SeasonalAnime { get; set; }
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

        private List<SearchResult> LoadSeasonalViewData() //To do: Add parameter List<IPage> and fill Dictionary according to the List.
        {
            List<SearchResult> seasonalList = new List<SearchResult>();
            foreach(SearchResult s in main.getSeasonals())
            {
                seasonalList.Add(s);
            }

            return seasonalList;
        }

        private void SeasonalView_ItemClick(object sender, ItemClickEventArgs e)
        {
            SearchResult item = e.ClickedItem as SearchResult;
            IPage page = main.ProducePage("anime", item.id);
            Window.Current.Content = new AnimeInfoPage(page as AnimePage);
        }

        private async void Button_Click(object sender, RoutedEventArgs e) //Anime
        {
            string text = searchInput.Text;
            if(text.Length > 2)
            {
                Window.Current.Content = new SearchResultsPage((main.ProduceSearchPage("anime/" + text) as SearchPage).Results, main);
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
                Window.Current.Content = new SearchResultsPage((main.ProduceSearchPage("manga/" + text) as SearchPage).Results, main);
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

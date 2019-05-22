using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
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
        Main main;

        public HomePage()
        {
            main = new Main();
            this.InitializeComponent();
        }

        public HomePage(Main m)
        {
            main = m;
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

        private async void SeasonalView_ItemClick(object sender, ItemClickEventArgs e)
        {
            SearchResult item = e.ClickedItem as SearchResult;
            Task<IPage> t = new Task<IPage>(() => { return main.ProducePage(item.type, item.id); });
            t.Start();
            IPage page = await t;
            Window.Current.Content = new AnimeInfoPage(page as AnimePage, main);
        }

        private async void Button_Click(object sender, RoutedEventArgs e) //Anime
        {
            string text = searchInput.Text;
            if(text.Length > 2)
            {
                Task<SearchPage> t = new Task<SearchPage>(() => { return (SearchPage)main.ProduceSearchPage("anime/" + text); });
                t.Start();
                Window.Current.Content = new SearchResultsPage((await t).Results, main);
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
                Task<SearchPage> t = new Task<SearchPage>(() => { return (SearchPage)main.ProduceSearchPage("manga/" + text); });
                t.Start();
                Window.Current.Content = new SearchResultsPage((await t).Results, main);
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

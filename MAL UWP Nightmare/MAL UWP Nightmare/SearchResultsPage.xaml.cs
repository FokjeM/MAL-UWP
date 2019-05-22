using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
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
        public List<SearchResult> results { get; set; }
        private Main main;
        public SearchResultsPage(List<SearchResult> sr, Main m)
        {
            this.main = m;
            this.InitializeComponent();
            this.results = sr;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = this;
        }

        private async void SearchResultsView_ItemClick(object sender, ItemClickEventArgs e)
        {
            SearchResult item = e.ClickedItem as SearchResult;
            Task<IPage> t = new Task<IPage>(() => { return main.ProducePage(item.type, item.id); });
            t.Start();
            IPage page = await t;
            if(page.GetType().Name.Equals("MangaPage"))
            {
                Window.Current.Content = new MangaInfoPage(page as MangaPage, main);
            } else
            {
                Window.Current.Content = new AnimeInfoPage(page as AnimePage, main);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Window.Current.Content = new HomePage(main);
        }
    }
}

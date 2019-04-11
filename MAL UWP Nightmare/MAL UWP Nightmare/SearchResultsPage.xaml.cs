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
        public List<SearchResult> results { get; set; }
        public SearchResultsPage(List<SearchResult> sr)
        {
            this.InitializeComponent();
            this.results = sr;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = this;
        }

        private void SearchResultsView_ItemClick(object sender, ItemClickEventArgs e)
        {
            
        }
    }
}

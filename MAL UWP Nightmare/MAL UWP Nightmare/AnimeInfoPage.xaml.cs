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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace MAL_UWP_Nightmare
{
    public sealed partial class AnimeInfoPage : Page
    {
        private AnimePage anime;
        public string synopsis { get; set; }
        public string background { get; set; }
        public string altTitles { get; set; }
        public string jpTitle { get; set; }
        public string enTitle { get; set; }
        public string type { get; set; }
        public string status { get; set; }
        public string startDate { get; set; }
        public string endDate { get; set; }
        public object related { get; set; } //To do: Replace when API code is ready.
        public bool running { get; set; }
        public string genres { get; set; }
        public string producers { get; set; }
        public string licensors { get; set; }
        public string studios { get; set; }
        public string openings { get; set; }
        public string endings { get; set; }
        public string broadcast { get; set; }
        public string premiered { get; set; }
        public string image { get; set; }
        public string title { get; set; }

        public AnimeInfoPage(AnimePage anime)
        {
            this.anime = anime;
            this.InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            setData(anime);
            DataContext = this;
        }

        private string ConvertListToString(List<string> list, bool sameLine)
        {
            string s = "";

            if (sameLine)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    s += list[i];
                    if (i != list.Count - 1)
                    {
                        s += ", ";
                    }
                }
            }
            else
            {
                for (int i = 0; i < list.Count; i++)
                {
                    s += list[i];
                    if (i != list.Count - 1)
                    {
                        s += "\r\n";
                    }
                }
            }

            return s;
        }

        private void setData(AnimePage anime)
        {
            title = anime.Title != null ? anime.Title : "Title Unavailable";
            synopsis = anime.Synopsis != null ? anime.Synopsis : "No synopsis information has been added to this title.";
            background = anime.Background != null ? anime.Background : "No background information has been added to this title.";
            altTitles = anime.AltTitles != null ? ConvertListToString(anime.AltTitles, false) : "Unavailable";
            jpTitle = anime.JapTitle != null ? anime.JapTitle : "Unavailable";
            enTitle = anime.EngTitle != null ? anime.EngTitle : "Unavailable";
            type = anime.Type != null ? anime.Type : "Unavailable";
            status = anime.Status != null ? anime.Status : "Unavailable";
            startDate = anime.StartDate != null ? anime.StartDate.ToString("MMMM dd, yyyy") : "Unavailable";
            endDate = anime.EndDate != null ? anime.EndDate.ToString("MMMM dd, yyyy") : "Unavailable";
            running = anime.Running;
            genres = anime.Genres != null ? ConvertListToString(anime.Genres, true) : "Unavailable";
            producers = anime.Producers != null ? ConvertListToString(anime.Producers, false) : "Unavailable";
            licensors = anime.Licensors != null ? ConvertListToString(anime.Licensors, false) : "Unavailable";
            studios = anime.Studios != null ? ConvertListToString(anime.Studios, false) : "Unavailable";
            openings= anime.Openings != null ? ConvertListToString(anime.Openings, false) : "Unavailable";
            endings = anime.Endings != null ? ConvertListToString(anime.Endings, false) : "Unavailable";
            broadcast = anime.Broadcast != null ? anime.Broadcast : "Unavailable";
            premiered = anime.PremiereSeason != null ? anime.PremiereSeason : "Unavailable";
            image = anime.MainImage;
        }
    }
}

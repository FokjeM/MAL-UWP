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
            title = anime.title != null ? anime.title : "Title Unavailable";
            synopsis = anime.synopsis != null ? anime.synopsis : "No synopsis information has been added to this title.";
            background = anime.background != null ? anime.background : "No background information has been added to this title.";
            altTitles = anime.altTitles != null ? ConvertListToString(anime.altTitles, false) : "Unavailable";
            jpTitle = anime.japTitle != null ? anime.japTitle : "Unavailable";
            enTitle = anime.engTitle != null ? anime.engTitle : "Unavailable";
            type = anime.type != null ? anime.type : "Unavailable";
            status = anime.status != null ? anime.status : "Unavailable";
            startDate = anime.startDate != null ? anime.startDate.ToString("MMMM dd, yyyy") : "Unavailable";
            endDate = anime.endDate != null ? anime.endDate.ToString("MMMM dd, yyyy") : "Unavailable";
            running = anime.running;
            genres = anime.genres != null ? ConvertListToString(anime.genres, true) : "Unavailable";
            producers = anime.producers != null ? ConvertListToString(anime.producers, false) : "Unavailable";
            licensors = anime.licensors != null ? ConvertListToString(anime.licensors, false) : "Unavailable";
            studios = anime.studios != null ? ConvertListToString(anime.studios, false) : "Unavailable";
            openings= anime.openings != null ? ConvertListToString(anime.openings, false) : "Unavailable";
            endings = anime.endings != null ? ConvertListToString(anime.endings, false) : "Unavailable";
            broadcast = anime.broadcast != null ? anime.broadcast : "Unavailable";
            premiered = anime.premiereSeason != null ? anime.premiereSeason : "Unavailable";
            image = anime.mainImage;
        }
    }
}

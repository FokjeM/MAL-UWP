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

        public AnimeInfoPage(AnimePage anime)
        {
            this.anime = anime;
            this.InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            setData(anime); //To do: Remove or edit when connected to API.
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
            synopsis = anime.synopsis;
            background = anime.background;
            altTitles = ConvertListToString(anime.altTitles, false);
            jpTitle = anime.japTitle;
            enTitle = anime.engTitle;
            //Type
            //Status
            startDate = anime.startDate.ToString("MMMM dd, yyyy");
            endDate = anime.endDate.ToString("MMMM dd, yyyy");
            running = anime.running;
            genres = ConvertListToString(anime.genres, true);
            producers = ConvertListToString(anime.producers, false);
            licensors = ConvertListToString(anime.licensors, false);
            studios = ConvertListToString(anime.studios, false);
            openings= ConvertListToString(anime.openings, false);
            endings = ConvertListToString(anime.endings, false);
            broadcast = anime.broadcast;
            premiered = anime.premiereSeason;
        }

        private void setTestData() //To do: Remove or edit when connected to API.
        {
            synopsis = "Just when Masato thought that a random survey conducted in school was over, he got involved in a secret Government scheme. " +
                "As he was carried along with the flow, he ended up in a Game world! As if that wasn't enough, shockingly, his mother was there as well! " +
                "It was a little different from a typical transported to another world setting, but after some bickering, Mom wants go on an adventure " +
                "together with Maa-kun. Can mom become Maa-kun's companion? With that, Masato and Mamako began their adventure as a mother and son pair. " +
                "They met Porta, a cute traveling merchant, and Wise, a regrettable philosopher. Along with their new party members, Masato and co. start on their " +
                "journey.";
            background = "No background information has been added to this title.";

            List<string> altTitlesList = new List<string>();
            altTitlesList.Add("Do You Like Your Mom? Her Normal Attack is Two Attacks at Full Power");
            altTitlesList.Add("Okaasan online");
            altTitles = ConvertListToString(altTitlesList, false);

            jpTitle = "通常攻撃が全体攻撃で二回攻撃のお母さんは好きですか？";
            enTitle = "Unknown";
            type = "TV";
            status = "Not yet aired";
            startDate = "Unknown";
            endDate = "Unknown";
            running = false;

            List<string> genresList = new List<string>();
            genresList.Add("Adventure");
            genresList.Add("Comedy");
            genresList.Add("Fantasy");
            genres = ConvertListToString(genresList, true);

            List<string> producersList = new List<string>();
            producersList.Add("Bandai Visual");
            producersList.Add("Bandai Visual");
            producers = ConvertListToString(producersList, false);

            List<string> licensorsList = new List<string>();
            licensorsList.Add("Funimation");
            licensorsList.Add("Bandai Entertainment");
            licensors = ConvertListToString(licensorsList, false);

            List<string> studiosList = new List<string>();
            studiosList.Add("Sunrise");
            studios = ConvertListToString(studiosList, false);

            List<string> openingsList = new List<string>();
            openingsList.Add("\"Tank!\" by The Seatbelts (eps 1-25)");
            openings = ConvertListToString(openingsList, false);

            List<string> endingsList = new List<string>();
            endingsList.Add("\"The Real Folk Blues\" by The Seatbelts feat. Mai Yamane (eps 1-12, 14-25)");
            endingsList.Add("\"Space Lion\" by The Seatbelts (ep 13)");
            endings = ConvertListToString(endingsList, false);

            broadcast = "Unknown";
            premiered = "Summer 2019";
        }
    }
}

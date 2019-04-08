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
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MangaInfoPage : Page
    {
        public MangaPage manga;
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
        public string authors { get; set; }
        public string image { get; set; }
        public string title { get; set; }

        public MangaInfoPage(MangaPage manga)
        {
            this.InitializeComponent();
            this.manga = manga;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            setData(); //To do: Remove or edit when connected to API.
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

        private void setData()
        {
            title = manga.Title != null ? manga.Title : "Title Unavailable";
            synopsis = manga.Synopsis != null ? manga.Synopsis : "No synopsis information has been added to this title.";
            background = manga.Background != null ? manga.Background : "No background information has been added to this title.";
            altTitles = manga.AltTitles != null ? ConvertListToString(manga.AltTitles, false) : "Unavailable";
            jpTitle = manga.JapTitle != null ? manga.JapTitle : "Unavailable";
            enTitle = manga.EngTitle != null ? manga.EngTitle : "Unavailable";
            type = manga.Type != null ? manga.Type : "Unavailable";
            status = manga.Status != null ? manga.Status : "Unavailable";
            startDate = manga.StartDate != null ? manga.StartDate.ToString("MMMM dd, yyyy") : "Unavailable";
            endDate = manga.EndDate != null ? manga.EndDate.ToString("MMMM dd, yyyy") : "Unavailable";
            running = manga.Running;
            genres = manga.Genres != null ? ConvertListToString(manga.Genres, true) : "Unavailable";
            authors = manga.Authors != null ? ConvertListToString(manga.Authors, false) : "Unavailable";
            image = manga.MainImage;
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
            type = "Manga";
            status = "Not yet aired";
            startDate = "Unknown";
            endDate = "Unknown";
            running = false;

            List<string> genresList = new List<string>();
            genresList.Add("Adventure");
            genresList.Add("Comedy");
            genresList.Add("Fantasy");
            genres = ConvertListToString(genresList, true);

            List<string> authorsList = new List<string>();
            authorsList.Add("Jyura");
            authors = ConvertListToString(authorsList, false);
        }
    }
}

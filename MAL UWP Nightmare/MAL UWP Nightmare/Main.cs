using System.Collections.Generic;

namespace MAL_UWP_Nightmare
{
    public class Main : IObserver
    {
        private IThreadingStrategy CurrentStrategy;
        private MultiThreadingStrategy MultiThreaded;
        private SingleThreadingStrategy SingleThreaded;
        private SplashScreen splash;
        private HomePageBackend home;
        private IPage currentPage;
        private PageFactory pages;
        private SearchPage search;

        public Main()
        {
            PageFactory p = new PageFactory();
            MultiThreaded = new MultiThreadingStrategy(p);
            SingleThreaded = new SingleThreadingStrategy(p);
            pages = p;
            CurrentStrategy = MultiThreaded;
            splash = new SplashScreen();
            currentPage = home;
            home = (HomePageBackend)pages.Home(splash);
            search = (SearchPage)pages.Search(this);
        }

        public void SwitchThreadingStrategy()
        {
            if (CurrentStrategy.Equals(MultiThreaded))
            {
                CurrentStrategy = SingleThreaded;
            } else
            {
                CurrentStrategy = MultiThreaded;
            }
        }

        public IPage NotifyMe(SearchResult res)
        {
            return producePage(res.type, res.id);
        }

        public IPage producePage(string req, long id)
        {
            switch (req.ToLower())
            {
                case "manga":
                case "novel":
                case "oneshot":
                case "doujin":
                case "manhwa":
                case "manhua":
                    return CurrentStrategy.ProduceContentPage("manga/", id);
                case "anime":
                case "tv":
                case "ova":
                case "movie":
                case "special":
                case "ona":
                    return CurrentStrategy.ProduceContentPage("anime/", id);
                case "person":
                    return CurrentStrategy.ProducePersonPage("person/", id);
                case "character":
                    return CurrentStrategy.ProduceCharacterPage("character/", id);
                default:
                    return CurrentStrategy.ProduceSearchPage(req + "/" + id.ToString(), this);
            }
        }

        public IPage ProduceSearchPage(string query)
        {
            return CurrentStrategy.ProduceSearchPage(query, this);
        }

        public List<SearchResult> getSeasonals()
        {
            return CurrentStrategy.getSeasonals();
        }

        public void NotifyMe(IPage p)
        {
            
        }
    }
}
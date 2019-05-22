using System.Collections.Generic;
using System.Threading.Tasks;

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
        private IPage lastPage;
        private PageFactory pages;
        public SearchPage search;

        public Main()
        {
            PageFactory p = new PageFactory();
            MultiThreaded = new MultiThreadingStrategy(p);
            SingleThreaded = new SingleThreadingStrategy(p);
            pages = p;
            CurrentStrategy = MultiThreaded;
            splash = new SplashScreen();
            currentPage = home;
            lastPage = home;
            home = (HomePageBackend)pages.Home(splash);
            search = (SearchPage)pages.Search(this);
        }

        public IPage Previous()
        {
            IPage newLast = currentPage;
            currentPage = lastPage;
            lastPage = newLast;
            return currentPage;
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
            return ProducePage(res.type, res.id);
        }

        public IPage ProducePage(string req, long id)
        {
            lastPage = search;
            Previous();
            switch (req.ToLower())
            {
                case "manga":
                case "novel":
                case "oneshot":
                case "doujin":
                case "manhwa":
                case "manhua":
                    Task<IPage> mangaPage = CurrentStrategy.ProduceContentPage("manga/", id);
                    return mangaPage.Result;
                case "anime":
                case "tv":
                case "ova":
                case "movie":
                case "special":
                case "ona":
                    Task<IPage> animePage = CurrentStrategy.ProduceContentPage("anime/", id);
                    return animePage.Result;
                case "person":
                    //This is sync anyways
                    return CurrentStrategy.ProducePersonPage("person/", id);
                case "character":
                    //This is sync anyways
                    return CurrentStrategy.ProduceCharacterPage("character/", id);
                default:
                    Task<IPage> searchPage = CurrentStrategy.ProduceSearchPage(req + "/" + id.ToString(), this);
                    return searchPage.Result;
            }
        }

        public IPage ProduceSearchPage(string query)
        {
            lastPage = home;
            Task<IPage> searcher = CurrentStrategy.ProduceSearchPage(query, this);
            search = (SearchPage)searcher.Result;
            return search;
        }

        public List<SearchResult> getSeasonals()
        {
            return CurrentStrategy.GetSeasonals(home);
        }

        public void NotifyMe(IPage p)
        {
            
        }
    }
}
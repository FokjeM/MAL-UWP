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
            home = (HomePageBackend)pages.Home(splash);
            search = (SearchPage)pages.Search(this);
            currentPage = search;
            lastPage = home;
        }

        public IPage Previous()
        {
            IPage newLast = currentPage;
            currentPage = lastPage;
            lastPage = newLast;
            return currentPage;
        }

        public void SeasonalFlip()
        {
            if (!currentPage.Equals(home))
            {
                currentPage = home;
                lastPage = search;
            }
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
            //We have to flip the current and last pages unless a searchpage appears
            switch (req.ToLower())
            {
                case "manga":
                case "novel":
                case "oneshot":
                case "one-shot":
                case "doujin":
                case "manhwa":
                case "manhua":
                    Task<IPage> mangaPage = CurrentStrategy.ProduceContentPage("manga/", id);
                    Previous();
                    return mangaPage.Result;
                case "anime":
                case "tv":
                case "ova":
                case "movie":
                case "special":
                case "ona":
                    Task<IPage> animePage = CurrentStrategy.ProduceContentPage("anime/", id);
                    Previous();
                    return animePage.Result;
                case "person":
                    Previous();
                    //This is sync anyways
                    return CurrentStrategy.ProducePersonPage("person/", id);
                case "character":
                    Previous();
                    //This is sync anyways
                    return CurrentStrategy.ProduceCharacterPage("character/", id);
                default:
                    Task<IPage> searchPage = CurrentStrategy.ProduceSearchPage(req + "/" + id.ToString(), this);
                    return searchPage.Result;
            }
        }

        public IPage ProduceSearchPage(string query)
        {
            Task<IPage> searcher = CurrentStrategy.ProduceSearchPage(query, this);
            if (currentPage.Equals(search))
            {
                currentPage = searcher.Result;
            } else
            {
                lastPage = searcher.Result;
            }
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
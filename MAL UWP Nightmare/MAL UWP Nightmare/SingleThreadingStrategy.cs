using System.Collections.Generic;
using System.Threading.Tasks;

namespace MAL_UWP_Nightmare
{
    public class SingleThreadingStrategy : IThreadingStrategy
    {
        private PageFactory pages;

        public SingleThreadingStrategy(PageFactory p)
        {
            pages = p;
        }

        public List<SearchResult> GetSeasonals(HomePageBackend home)
        {
            return home.Seasonals;
        }

        public IPage ProduceCharacterPage(string request, long id)
        {
            return pages.Character();
        }

        public Task<IPage> ProduceContentPage(string type, long id)
        {
            return new Task<IPage>(() => pages.Content(type, id));
        }

        public IPage ProduceHomePage(IObserver observer)
        {
            return pages.Home(observer);
        }

        public IPage ProducePersonPage(string request, long id)
        {
            return pages.Person();
        }

        public Task<IPage> ProduceSearchPage(string query, IObserver observer)
        {
            return new Task<IPage>(() => pages.Search(observer, query));
        }
    }
}
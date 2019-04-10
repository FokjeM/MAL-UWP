using System.Threading.Tasks;
using System.Collections.Generic;

namespace MAL_UWP_Nightmare
{
    public class MultiThreadingStrategy : IThreadingStrategy
    {
        private PageFactory pages;

        public MultiThreadingStrategy(PageFactory p)
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

        public async Task<IPage> ProduceContentPage(string type, long id)
        {
            return await pages.ContentAsync(type, id);
        }

        public IPage ProduceHomePage(IObserver observer)
        {
            return pages.Home(observer);
        }

        public IPage ProducePersonPage(string request, long id)
        {
            return pages.Person();
        }

        public async Task<IPage> ProduceSearchPage(string query, IObserver observer)
        {
            return await pages.SearchAsync(observer, query);
        }
    }
}
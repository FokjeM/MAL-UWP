using System.Threading.Tasks;
using System.Threading;
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

        public List<SearchResult> getSeasonals()
        {
            return ((HomePageBackend)pages.Home(new SplashScreen())).seasonals;
        }

        public CharacterPage ProduceCharacterPage(string request, long id)
        {
            return (CharacterPage)pages.Character();
        }

        public ContentPage ProduceContentPage(string type, long id)
        {
            return ProduceContentPageAsync(type, id).Result;
        }

        public async Task<ContentPage> ProduceContentPageAsync(string type, long id)
        {
            return (ContentPage)await pages.ContentAsync(type, id);
        }

        public HomePageBackend ProduceHomePage(IObserver observer)
        {
            return (HomePageBackend)pages.Home(observer);
        }

        public PersonPage ProducePersonPage(string request, long id)
        {
            return (PersonPage)pages.Person();
        }

        public SearchPage ProduceSearchPage(string query, IObserver observer)
        {
            SearchPage search = (SearchPage)pages.Search(observer, query);
            /*new Task(() =>
            {
                foreach (SearchResult s in search.Results)
                {
                    s.addPage(search.NotifyObserver(s));
                }
            }).Start();*/
            return search;
        }
    }
}
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace MAL_UWP_Nightmare
{
    public class PageFactory
    {
        private readonly APIMachine source;

        public PageFactory()
        {
            source = new APIMachine();
        }
        

        public IPage Home(IObserver o)
        {
            HomePageBackend home = new HomePageBackend(o);
            home.SetContent(source.GetSeasonals());
            return home;
        }

        public IPage Content(string type, long id)
        {
            ContentPage page;
            if(type.ToLower().Equals("anime"))
            {
                page = new AnimePage();
            } else if(type.ToLower().Equals("manga"))
            {
                page = new MangaPage();
            } else
            {
                return null;
            }
            page.SetContent(source.RequestAPI(type + id.ToString()));
            return page;
        }

        public async Task<IPage> ContentAsync(string type, long id)
        {
            Task<JObject> res = source.RequestAPIAsync(type + id.ToString());
            //Start the fetch while preparing the actual page
            IPage page;
            if (type.ToLower().Equals("anime"))
            {
                page = new AnimePage();
            }
            else if (type.ToLower().Equals("manga"))
            {
                page = new MangaPage();
            }
            else
            {
                return null;
            }
            page.SetContent(await res);
            return page;
        }

        public IPage Search(IObserver o)
        {
            SearchPage s = new SearchPage(o);
            return s;
        }

        public IPage Search(IObserver o, string query)
        {
            SearchPage s = new SearchPage(o);
            s.SetResults(source.SearchAPI(query));
            return s;
        }

        public async Task<IPage> SearchAsync(IObserver o, string query)
        {
            SearchPage s = new SearchPage(o);
            s.SetResults(await source.SearchAPIAsync(query));
            return s;
        }

        /// <summary>
        /// Because of time constraints, there is no handling for Character requests yet.
        /// If this is used, external data will need to be set for a CharacterPage.
        /// </summary>
        /// <returns>an empty CharacterPage</returns>
        public IPage Character()
        {
            return new CharacterPage();
        }

        /// <summary>
        /// DON'T USE THIS, ALL METHODS THROW NOT IMPLEMENTED EXCEPTIONS!
        /// </summary>
        /// <returns>A new PersonPage. These are dangerous</returns>
        public IPage Person()
        {
            return new PersonPage();
        }
    }
}

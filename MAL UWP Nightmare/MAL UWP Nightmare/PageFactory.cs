using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAL_UWP_Nightmare
{
    class PageFactory
    {
        private readonly APIMachine source;

        public PageFactory()
        {
            source = new APIMachine();
        }
        

        public IPage Home()
        {
            throw new NotImplementedException();
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
            page.SetContent(source.requestAPI(type + id.ToString()));
            return page;
        }

        public IPage Search(string query)
        {
            SearchPage s = new SearchPage();

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

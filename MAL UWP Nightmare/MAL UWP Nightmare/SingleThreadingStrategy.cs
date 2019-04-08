namespace MAL_UWP_Nightmare
{
    public class SingleThreadingStrategy : IThreadingStrategy
    {
        private PageFactory pages;

        public SingleThreadingStrategy(PageFactory p)
        {
            pages = p;
        }

        public CharacterPage ProduceCharacterPage(string request, long id)
        {
            return (CharacterPage)pages.Character();
        }

        public ContentPage ProduceContentPage(string type, long id)
        {
            return (ContentPage)pages.Content(type, id);
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
            return (SearchPage)pages.Search(observer, query);
        }
    }
}
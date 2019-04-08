using System.Collections.Generic;

namespace MAL_UWP_Nightmare
{
    public interface IThreadingStrategy
    {
        ContentPage ProduceContentPage(string request, long id);
        CharacterPage ProduceCharacterPage(string request, long id);
        PersonPage ProducePersonPage(string request, long id);
        HomePageBackend ProduceHomePage(IObserver observer);
        SearchPage ProduceSearchPage(string query, IObserver observer);
        List<SearchResult> getSeasonals();
    }
}
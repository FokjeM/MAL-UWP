using System.Collections.Generic;
using System.Threading.Tasks;

namespace MAL_UWP_Nightmare
{
    public interface IThreadingStrategy
    {
        Task<IPage> ProduceContentPage(string request, long id);
        IPage ProduceCharacterPage(string request, long id);
        IPage ProducePersonPage(string request, long id);
        IPage ProduceHomePage(IObserver observer);
        Task<IPage> ProduceSearchPage(string query, IObserver observer);
        List<SearchResult> GetSeasonals(HomePageBackend home);
    }
}
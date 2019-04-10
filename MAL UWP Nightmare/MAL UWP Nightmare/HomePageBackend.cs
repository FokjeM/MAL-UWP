using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace MAL_UWP_Nightmare
{
    /// <summary>
    /// Doesn't really use much of the IPage interface anymore as front-end
    /// and backend now live in seperate subsystems. Using the Facade pattern,
    /// we managed to decouple it and pass along an IPage instead of using a
    /// lot of overly complicated code.
    /// 
    /// Keeping the IPage for simplicity though.
    /// </summary>
    public class HomePageBackend : IPage
    {
        public List<SearchResult> seasonals;
        private IObserver observer;
        public string ErrorText { get; private set; }

        public List<SearchResult> GetSeasonals()
        {
            return seasonals;
        }

        public HomePageBackend(IObserver o)
        {
            observer = o;
            observer.NotifyMe(this);
        }

        public bool IsLocal()
        {
            return false;
        }

        public bool SavePage()
        {
            return false;
        }

        public void SetContent(JObject json)
        {
            List<SearchResult> resultList = new List<SearchResult>();
            foreach (JToken jt in json.GetValue("anime"))
            {
                string title = jt.Value<string>("title");
                string image = jt.Value<string>("image_url");
                string type = jt.Value<string>("type");
                long id = jt.Value<long>("mal_id");
                SearchResult res = new SearchResult(type, title, image, id);
                resultList.Add(res);
            }
            seasonals = resultList;
        }

        public void SetErrorContent(string errorMessage)
        {
            ErrorText = errorMessage;
        }

        private void NotifyObserver()
        {
            observer.NotifyMe(this);
        }

        public async Task<bool> SavePageAsync()
        {
            return false;
        }
    }
}

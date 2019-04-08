using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace MAL_UWP_Nightmare
{
    public class HomePageBackend : IPage
    {
        public List<SearchResult> seasonals;
        private IObserver observer;


        public HomePageBackend(IObserver o)
        {
            observer = o;
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
            List<SearchResult> resultList = new List<SearchResult>(25);
            foreach (JToken jt in json.GetValue("anime").Values())
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
            throw new NotImplementedException();
        }

        private void NotifyObserver()
        {
            observer.NotifyMe(this);
        }
    }
}

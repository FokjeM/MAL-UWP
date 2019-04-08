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
        private List<SearchResult> seasonals;
        private IObserver observer;

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
            foreach (JToken jt in json.GetValue("results"))
            {
                string title = jt.Value<string>("title");
                string image = jt.Value<string>("image_url");
                string type = jt.Value<string>("type");
                long id = jt.Value<long>("mal_id");
                SearchResult res = new SearchResult(type, title, image, id);
                resultList.Add(res);
            }
            throw new NotImplementedException();
        }

        public void SetErrorContent(string errorMessage)
        {
            throw new NotImplementedException();
        }
    }
}

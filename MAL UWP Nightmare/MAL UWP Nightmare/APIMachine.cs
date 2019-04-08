using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Web.Http;

namespace MAL_UWP_Nightmare
{
    /// <summary>
    /// Class that supplies the API currently in use.
    /// Has methods to test both the Jikan and Kitsu APIs and will set both availlable states.
    /// Has a method to test access to local storafe; something it should always get in the
    /// ApplicationData folder and file objects because of how UWP is set up.
    /// Regular checks should be done if either or both are offline.
    /// </summary>
    class APIMachine
    {
        private readonly JikanAPIState jikan = new JikanAPIState();
        private readonly OfflineAPIState offline = new OfflineAPIState();

        public APIMachine()
        {

        }

        public JObject RequestAPI(string request)
        {
            string offlineSearchResult = offline.GetRequestFromSearch(request);
            if (string.IsNullOrEmpty(offlineSearchResult))
            {
                return jikan.RequestAPI(jikan.GetRequestFromSearch(request));
            } else
            {
                return offline.RequestAPI(offlineSearchResult);
            }
        }

        public async Task<JObject> RequestAPIAsync(string request)
        {
            return await jikan.RequestAPIAsync(await jikan.GetRequestFromSearchAsync(request));
        }

        public bool[] CheckAPIs()
        {
            bool[] res = new bool[2];
            res[0] = jikan.TestAPI();
            res[1] = offline.TestAPI();
            return res;
        }

        public JObject GetSeasonals()
        {
            if (jikan.TestAPI())
            {
                return jikan.GetSeasonals();
            }
            return offline.GetSeasonals();
        }

        public List<SearchResult> SearchAPI(string query)
        {
            List<SearchResult> search = new List<SearchResult>(50);
            foreach (SearchResult s in offline.SearchAPI(query)) { search.Add(s); }
            foreach (SearchResult s in jikan.SearchAPI(query)) { if(!search.Contains(s))search.Add(s); }
            return search;
        }
    }
}

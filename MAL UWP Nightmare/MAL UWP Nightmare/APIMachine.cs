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

        public JObject requestAPI(string request)
        {
            string offlineSearchResult = offline.getRequestFromSearch(request).Result;
            if (string.IsNullOrEmpty(offlineSearchResult))
            {
                return jikan.requestAPI(jikan.getRequestFromSearch(request).Result).Result;
            } else
            {
                return offline.requestAPI(offlineSearchResult).Result;
            }
        }

        public bool[] checkAPIs()
        {
            bool[] res = new bool[2];
            res[0] = jikan.testAPI();
            res[1] = offline.testAPI();
            return res;
        }

        public List<SearchResult> searchAPI(string query)
        {
            List<SearchResult> search = new List<SearchResult>(50);
            foreach (SearchResult s in offline.searchAPI(query)) { search.Add(s); }
            foreach (SearchResult s in jikan.searchAPI(query)) { search.Add(s); }
            return search;
        }
    }
}

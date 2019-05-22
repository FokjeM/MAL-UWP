using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

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
            if (jikan.TestAPI())
            {
                string jikanSearchResult = jikan.GetRequestFromSearch(request);
                if (!string.IsNullOrEmpty(jikanSearchResult))
                {
                    return jikan.RequestAPI(jikanSearchResult);
                }
            }
            //offline check if Jikan is unavaillable
            if(offline.TestAPI())
            {
                string offlineSearchResult = offline.GetRequestFromSearch(request);
                if (!string.IsNullOrEmpty(offlineSearchResult))
                {
                    return offline.RequestAPI(offlineSearchResult);
                }
            }
            return null;
        }

        /// <summary>
        /// Prefers offline resources due to request constraints
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<JObject> RequestAPIAsync(string request)
        {
            if (jikan.TestAPI())
            {
                string jikanSearchResult = await jikan.GetRequestFromSearchAsync(request);
                if (!string.IsNullOrEmpty(jikanSearchResult))
                {
                    return await jikan.RequestAPIAsync(jikanSearchResult);
                }
            }
            //offline check if Jikan is unavaillable
            if (offline.TestAPI())
            {
                string offlineSearchResult = await offline.GetRequestFromSearchAsync(request);
                if (!string.IsNullOrEmpty(offlineSearchResult))
                {
                    return await offline.RequestAPIAsync(offlineSearchResult);
                }
            }
            
            return null;
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
            if (offline.TestAPI())
            {
                foreach (SearchResult s in offline.SearchAPI(query))
                {
                    search.Add(s);
                }
            }
            if (jikan.TestAPI())
            {
                foreach (SearchResult s in jikan.SearchAPI(query)) {
                    //Try to prevent duplicate results, this breaks if instances of JObject are never or always equal.
                    if (!search.Contains(s))
                    {
                        search.Add(s);
                    }
                }
            }
            return search;
        }

        /// <summary>
        /// Searches both sources, grabs all local results while waiting for Jikan to respond and be parsed
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<List<SearchResult>> SearchAPIAsync(string query)
        {
            bool jikanAvaillable = jikan.TestAPI();
            //Prevent errors and problems for when Jikan turns out to be useless. Quick and gorgeous
            List<SearchResult> jikanSearch = await jikan.SearchAPIAsync(query);
            List<SearchResult> search = new List<SearchResult>(50);
            if (offline.TestAPI())
            {
                List<SearchResult> offlineRes = await offline.SearchAPIAsync(query);
                if (offlineRes != null)
                {
                    foreach (SearchResult s in offlineRes)
                    {
                        search.Add(s);
                    }
                }
            }
            if (jikanAvaillable)
            {
                if (jikanSearch != null)
                {
                    foreach (SearchResult s in jikanSearch)
                    {
                        //Try to prevent duplicate results, this breaks if instances of JObject are never or always equal.
                        if (!search.Contains(s))
                        {
                            search.Add(s);
                        }
                    }
                }
            }
            return search;
        }
    }
}

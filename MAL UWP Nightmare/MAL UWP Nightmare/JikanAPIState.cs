using System;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Collections.Generic;

namespace MAL_UWP_Nightmare
{
    class JikanAPIState : APIState
    {
        public JikanAPIState() : base("https://api.jikan.moe/v3/")
        {
            availlable = false;
            lastChecked = DateTime.UtcNow;
        }

        public override async Task<string> getRequestFromSearch(string query)
        {
            string[] reqParts = query.Split('/');
            long id = CheckKnownIDs(query);
            if (id > 0L)
            {
                return reqParts[0] + "/" + id.ToString();
            }
            string searchReq = "search/" + reqParts[0] + "?q=";
            for (int i = 1; i < reqParts.Length; i++)
            {
                if(i > 1)
                {
                    searchReq += Uri.EscapeDataString("/");
                }
                searchReq += Uri.EscapeDataString(reqParts[i]);
            }
            searchReq += "&limit=1";
            JObject result = requestAPI(searchReq).Result;
            return reqParts[0] + "/" + result.GetValue("results").First.First.ToObject("".GetType());
        }

        public override async Task<JObject> requestAPI(string request)
        {
            HttpClient req = new HttpClient();
            req.DefaultRequestHeaders.Add("User-Agent", userAgent);
            Uri api = new Uri(getURL() + request);
            System.Diagnostics.Debug.WriteLine(api.ToString());
            HttpResponseMessage response = req.GetAsync(api).Result;
            JObject result = JObject.Parse(response.Content.ReadAsStringAsync().Result);
            if (result.ContainsKey("title"))
            {
                JObject ret = new JObject();
                ret.Add("title", result.GetValue("title"));
                ret.Add("id", result.GetValue("id"));
                ret.Add("url", JToken.FromObject(api.ToString()));
                ret.Add("title_japanese", result.GetValue("title_japanese"));
                if (request.Contains("manga"))
                {
                    //add manga-specific parts
                } else if (request.Contains("anime"))
                {
                    //add anime-specific parts
                }
            }
#pragma warning disable CS4014 // Is niet relevant voor deze method.
            var added = AddToKnownIDs(request.Split('/')[0], result.GetValue("title").ToString(), long.Parse(request.Split('/')[1]), 0L).Result;
#pragma warning restore CS4014 // Bij de volgende call is het al klaar.
            return result;
        }

        public override List<SearchResult> searchAPI(string query)
        {
            string[] reqParts = query.Split('/');
            string searchReq = "search/" + reqParts[0] + "?q=";
            for (int i = 1; i < reqParts.Length; i++)
            {
                if (i > 1)
                {
                    searchReq += Uri.EscapeDataString("/");
                }
                searchReq += Uri.EscapeDataString(reqParts[i]);
            }
            searchReq += "&limit=25";
            JObject result = requestAPI(searchReq).Result;
            List<SearchResult> resultList = new List<SearchResult>(25);
            foreach(JToken jt in result.GetValue("results").Values())
            {
                string title = jt.Value<string>("title");
                string image = jt.Value<string>("image_url");
                long id = jt.Value<long>("mal_id");
                SearchResult res = new SearchResult(reqParts[0], title, image, id);
                resultList.Add(res);
            }
            return resultList;
        }

        /// <summary>
        /// Tests the Jikan API if it can be reached.
        /// </summary>
        /// <returns>True if it doesn't get an error or success was achieved
        /// within the last 5 minutes of calling this function.</returns>
        public override bool testAPI()
        {
            if (lastChecked.AddMinutes(5).CompareTo(DateTime.UtcNow) < 0 || !availlable)
            {
                JObject response = requestAPI("anime/5081").Result;
                if (!response.ContainsKey("error"))
                {
                    availlable = true;
                }
                else
                {
                    availlable = false;
                }
                lastChecked = DateTime.UtcNow;
            }
            return availlable;
        }

        protected override long CheckKnownIDs(string query)
        {
            string[] q = query.Split('/');
            try
            {
                string tempRes = knownIDs.GetValue(string.Format("{0}/{1}", q[0], q[1]).ToLower()).ToObject("".GetType()).ToString().Split((new string[] { " : " }), StringSplitOptions.RemoveEmptyEntries)[0];
                if (tempRes == null)
                {
                    return 0L;
                }
                return long.Parse(tempRes);
            } catch
            {
                return 0L;
            }
        }
    }
}

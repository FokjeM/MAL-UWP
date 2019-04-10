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

        public override string GetRequestFromSearch(string query)
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
                if (i > 1)
                {
                    searchReq += Uri.EscapeDataString("/");
                }
                searchReq += Uri.EscapeDataString(reqParts[i]);
            }
            searchReq += "&limit=1";
            JObject result = RequestAPI(searchReq);
            return reqParts[0] + "/" + result.GetValue("results").First.First.ToObject("".GetType());
        }

        public override async Task<string> GetRequestFromSearchAsync(string query)
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
            JObject result = await RequestAPIAsync(searchReq);
            return reqParts[0] + "/" + result.GetValue("results").First.First.ToObject("".GetType());
        }

        /// <summary>
        /// Season determination made possible by the wonderful source
        /// http://imaginekitty.com/599/finding-the-current-season-using-c/
        /// Slightly edited because we don't need a CSS file.
        /// </summary>
        /// <returns></returns>
        public override JObject GetSeasonals()
        {
            int doy = DateTime.Now.DayOfYear - Convert.ToInt32((DateTime.IsLeapYear(DateTime.Now.Year)) && DateTime.Now.DayOfYear > 59);
            string currentSeason = string.Format("season/{0}/{1}", DateTime.Now.Year.ToString(), ((doy < 80 || doy >= 355) ? "winter" : ((doy >= 80 && doy < 172) ? "spring" : ((doy >= 172 && doy < 266) ? "summer" : "fall"))));
            return RequestAPI(currentSeason);
        }

        public override JObject RequestAPI(string request)
        {
            HttpClient req = new HttpClient();
            req.DefaultRequestHeaders.Add("User-Agent", userAgent);
            Uri api = new Uri(GetURL() + request);
            HttpResponseMessage response = req.GetAsync(api).Result;
            //We did not achieve success. Abort mission!
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }
            JObject result = JObject.Parse(response.Content.ReadAsStringAsync().Result);
            AddToKnownIDs(request.Split('/')[0], result.GetValue("title").ToString(), long.Parse(request.Split('/')[1]), 0L);
            return ParseResponse(result, api, request.Split('/')[0]);
        }

        public override async Task<JObject> RequestAPIAsync(string request)
        {
            HttpClient req = new HttpClient();
            req.DefaultRequestHeaders.Add("User-Agent", userAgent);
            Uri api = new Uri(GetURL() + request);
            HttpResponseMessage response = await req.GetAsync(api);
            //We did not achieve success. Abort mission!
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }
            JObject result = JObject.Parse(response.Content.ReadAsStringAsync().Result);
            AddToKnownIDs(request.Split('/')[0], result.GetValue("title").ToString(), long.Parse(request.Split('/')[1]), 0L);
            return ParseResponse(result, api, request.Split('/')[0]);
        }

        private JObject ParseResponse(JObject response, Uri api, string type)
        {
            if (response.ContainsKey("title"))
            {
                JObject ret = new JObject
                {
                    { "title", response.GetValue("title") },
                    { "id", response.GetValue("mal_id") },
                    { "url", JToken.FromObject(api.ToString()) },
                    { "title_japanese", response.GetValue("title_japanese") },
                    { "title_english", response.GetValue("title_english") },
                    { "synopsis", response.GetValue("synopsis") },
                    { "background", response.GetValue("background") },
                    { "image", response.GetValue("image_url") },
                    { "title_synonyms", response.GetValue("title_synonyms") },
                    { "status", response.GetValue("status") },
                    { "type", response.GetValue("type") }
                };
                JToken gens = response.GetValue("genres");
                List<string> genres = new List<string>();
                foreach (JToken jt in gens.Children())
                {
                    genres.Add(jt["name"].Value<string>());
                }
                ret.Add("genres", JToken.FromObject(genres));
                //The API differentiates between airing and publishing for anime and mange. We don't.
                if (type.Equals("anime"))
                {
                    //add anime-specific parts
                    ret.Add("running", response.GetValue("airing"));
                    ret.Add("run_from", response.GetValue("aired")["from"]);
                    ret.Add("run_to", response.GetValue("aired")["to"]);
                    ret.Add("premiered", response.GetValue("premiered"));
                    ret.Add("broadcast", response.GetValue("broadcast"));
                    ret.Add("producers", response.GetValue("producers"));
                    ret.Add("licensors", response.GetValue("licensors"));
                    ret.Add("studios", response.GetValue("studios"));
                    ret.Add("opening_themes", response.GetValue("opening_themes"));
                    ret.Add("ending_themes", response.GetValue("ending_themes"));
                }
                else if (type.Equals("manga"))
                {
                    //add manga-specific parts
                    ret.Add("running", response.GetValue("publishing"));
                    ret.Add("run_from", response.GetValue("published")["from"]);
                    ret.Add("run_to", response.GetValue("published")["to"]);
                    ret.Add("authors", response.GetValue("authors"));
                }
                return ret;
            } else
            {
                return response;
            }
        }

        public override List<SearchResult> SearchAPI(string query)
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
            JObject result = RequestAPI(searchReq);
            List<SearchResult> resultList = new List<SearchResult>(25);
            foreach(JToken jt in result.GetValue("results"))
            {
                string title = jt.Value<string>("title");
                string image = jt.Value<string>("image_url");
                string type = jt.Value<string>("type");
                long id = jt.Value<long>("mal_id");
                SearchResult res = new SearchResult(type, title, image, id);
                resultList.Add(res);
            }
            return resultList;
        }

        public async override Task<List<SearchResult>> SearchAPIAsync(string query)
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
            Task<JObject> result = RequestAPIAsync(searchReq);
            List<SearchResult> resultList = new List<SearchResult>(25);
            foreach (JToken jt in (await result).GetValue("results"))
            {
                string title = jt.Value<string>("title");
                string image = jt.Value<string>("image_url");
                string type = jt.Value<string>("type");
                long id = jt.Value<long>("mal_id");
                SearchResult res = new SearchResult(type, title, image, id);
                resultList.Add(res);
            }
            return resultList;
        }

        /// <summary>
        /// Tests the Jikan API if it can be reached.
        /// </summary>
        /// <returns>True if it doesn't get an error or success was achieved
        /// within the last 5 minutes of calling this function.</returns>
        public override bool TestAPI()
        {
            if (lastChecked.AddMinutes(5).CompareTo(DateTime.UtcNow) < 0)
            {
                JObject response = RequestAPI("anime/5081");
                if (response != null && !response.ContainsKey("error"))
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

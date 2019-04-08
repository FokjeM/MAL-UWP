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
        /// Made possible by the wonderful source
        /// http://imaginekitty.com/599/finding-the-current-season-using-c/
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
            JObject result = JObject.Parse(response.Content.ReadAsStringAsync().Result);
            if (result.ContainsKey("title"))
            {
                JObject ret = new JObject
                {
                    { "title", result.GetValue("title") },
                    { "id", result.GetValue("mal_id") },
                    { "url", JToken.FromObject(api.ToString()) },
                    { "title_japanese", result.GetValue("title_japanese") },
                    { "title_english", result.GetValue("title_english") },
                    { "synopsis", result.GetValue("synopsis") },
                    { "background", result.GetValue("background") },
                    { "image", result.GetValue("image_url") },
                    { "title_synonyms", result.GetValue("title_synonyms") },
                    { "status", result.GetValue("status") },
                    { "type", result.GetValue("type") }
                };
                JToken gens = result.GetValue("genres");
                List<string> genres = new List<string>();
                foreach (JToken jt in gens.Children())
                {
                    genres.Add(jt["name"].Value<string>());
                }
                ret.Add("genres", JToken.FromObject(genres));
                //The API differentiates between airing and publishing for anime and mange. We don't.
                if (request.Contains("anime"))
                {
                    //add anime-specific parts
                    ret.Add("running", result.GetValue("airing"));
                    ret.Add("run_from", result.GetValue("aired")["from"]);
                    ret.Add("run_to", result.GetValue("aired")["to"]);
                    ret.Add("premiered", result.GetValue("premiered"));
                    ret.Add("broadcast", result.GetValue("broadcast"));
                    ret.Add("producers", result.GetValue("producers"));
                    ret.Add("licensors", result.GetValue("licensors"));
                    ret.Add("studios", result.GetValue("studios"));
                    ret.Add("opening_themes", result.GetValue("opening_themes"));
                    ret.Add("ending_themes", result.GetValue("ending_themes"));
                }
                else if (request.Contains("manga"))
                {
                    //add manga-specific parts
                    ret.Add("running", result.GetValue("publishing"));
                    ret.Add("run_from", result.GetValue("published")["from"]);
                    ret.Add("run_to", result.GetValue("published")["to"]);
                    ret.Add("authors", result.GetValue("authors"));
                }
                result = ret;
                var added = AddToKnownIDs(request.Split('/')[0], result.GetValue("title").ToString(), long.Parse(request.Split('/')[1]), 0L);
            }
            return result;
        }

        public override async Task<JObject> RequestAPIAsync(string request)
        {
            HttpClient req = new HttpClient();
            req.DefaultRequestHeaders.Add("User-Agent", userAgent);
            Uri api = new Uri(GetURL() + request);
            HttpResponseMessage response = await req.GetAsync(api);
            JObject result = JObject.Parse(response.Content.ReadAsStringAsync().Result);
            if (result.ContainsKey("title"))
            {
                JObject ret = new JObject
                {
                    { "title", result.GetValue("title") },
                    { "id", result.GetValue("mal_id") },
                    { "url", JToken.FromObject(api.ToString()) },
                    { "title_japanese", result.GetValue("title_japanese") },
                    { "title_english", result.GetValue("title_english") },
                    { "synopsis", result.GetValue("synopsis") },
                    { "background", result.GetValue("background") },
                    { "image", result.GetValue("image_url") },
                    { "title_synonyms", result.GetValue("title_synonyms") },
                    { "status", result.GetValue("status") },
                    { "type", result.GetValue("type") }
                };
                JToken gens = result.GetValue("genres");
                List<string> genres = new List<string>();
                foreach (JToken jt in gens.Children())
                {
                    genres.Add(jt["name"].Value<string>());
                }
                ret.Add("genres", JToken.FromObject(genres));
                //The API differentiates between airing and publishing for anime and mange. We don't.
                if (request.Contains("anime"))
                {
                    //add anime-specific parts
                    ret.Add("running", result.GetValue("airing"));
                    ret.Add("run_from", result.GetValue("aired")["from"]);
                    ret.Add("run_to", result.GetValue("aired")["to"]);
                    ret.Add("premiered", result.GetValue("premiered"));
                    ret.Add("broadcast", result.GetValue("broadcast"));
                    ret.Add("producers", result.GetValue("producers"));
                    ret.Add("licensors", result.GetValue("licensors"));
                    ret.Add("studios", result.GetValue("studios"));
                    ret.Add("opening_themes", result.GetValue("opening_themes"));
                    ret.Add("ending_themes", result.GetValue("ending_themes"));
                } else if (request.Contains("manga"))
                {
                    //add manga-specific parts
                    ret.Add("running", result.GetValue("publishing"));
                    ret.Add("run_from", result.GetValue("published")["from"]);
                    ret.Add("run_to", result.GetValue("published")["to"]);
                    ret.Add("authors", result.GetValue("authors"));
                }
                result = ret;
            await AddToKnownIDsAsync(request.Split('/')[0], result.GetValue("title").ToString(), long.Parse(request.Split('/')[1]), 0L);
            }
            return result;
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

        /// <summary>
        /// Tests the Jikan API if it can be reached.
        /// </summary>
        /// <returns>True if it doesn't get an error or success was achieved
        /// within the last 5 minutes of calling this function.</returns>
        public override bool TestAPI()
        {
            if (lastChecked.AddMinutes(5).CompareTo(DateTime.UtcNow) < 0 || !availlable)
            {
                JObject response = RequestAPI("anime/5081");
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

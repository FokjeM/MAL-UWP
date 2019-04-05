using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Net.Http;

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
            string searchReq = "search/" + reqParts[0] + "?q=";
            for (int i = 1; i < reqParts.Length; i++)
            {
                searchReq += reqParts[i];
            }
            searchReq += "&limit=1";
            JObject result = await requestAPI(searchReq);
            return reqParts[0] + "/" + result.GetValue("results").First.First.ToObject("".GetType());
        }

        public override async Task<JObject> requestAPI(string request)
        {
            HttpClient req = new HttpClient();
            Uri api = new Uri(getURL() + request);
            HttpResponseMessage response = await req.GetAsync(api);
            JObject result = JObject.Parse(response.Content.ToString());
            return result;
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
    }
}

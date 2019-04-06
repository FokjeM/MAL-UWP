using System;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using Windows.Storage;

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
                searchReq += reqParts[i];
            }
            searchReq += "&limit=1";
            System.Diagnostics.Debug.WriteLine(searchReq);
            JObject result = await requestAPI(searchReq);
            return reqParts[0] + "/" + result.GetValue("results").First.First.ToObject("".GetType());
        }

        public override async Task<JObject> requestAPI(string request)
        {
            HttpClient req = new HttpClient();
            req.DefaultRequestHeaders.Add("User-Agent", userAgent);
            Uri api = new Uri(getURL() + request);
            System.Diagnostics.Debug.WriteLine(api.ToString());
            HttpResponseMessage response = req.GetAsync(api).Result;
            JObject result = JObject.Parse(await response.Content.ReadAsStringAsync());
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

        protected override long CheckKnownIDs(string query)
        {
            string[] q = query.Split('/');
            try
            {
                string tempRes = knownIDs.GetValue(string.Format("{0}/{1}", q[0], q[1])).ToObject("str".GetType()).ToString().Split((new string[] { " : " }), StringSplitOptions.RemoveEmptyEntries)[0];
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace MAL_UWP_Nightmare
{
    /// <summary>
    /// This is the KitsuAPIState class. Created as part of the application,
    /// but not yet implemented due to a heavy lack in features. Don't use this!
    /// </summary>
    class KitsuAPIState : APIState
    {
        public KitsuAPIState() : base("https://kitsu.io/api/edge/")
        {
            availlable = false;
            lastChecked = DateTime.UtcNow;
        }

        public override string GetRequestFromSearch(string query)
        {
            throw new NotImplementedException();
        }

        public override Task<string> GetRequestFromSearchAsync(string query)
        {
            throw new NotImplementedException();
        }

        public override JObject GetSeasonals()
        {
            throw new NotImplementedException();
        }

        public override JObject RequestAPI(string request)
        {
            throw new NotImplementedException();
        }

        public override Task<JObject> RequestAPIAsync(string request)
        {
            throw new NotImplementedException();
        }

        public override List<SearchResult> SearchAPI(string query)
        {
            throw new NotImplementedException();
        }

        public override Task<List<SearchResult>> SearchAPIAsync(string query)
        {
            throw new NotImplementedException();
        }

        public override bool TestAPI()
        {
            throw new NotImplementedException();
        }

        protected override long CheckKnownIDs(string query)
        {
            throw new NotImplementedException();
        }
    }
}

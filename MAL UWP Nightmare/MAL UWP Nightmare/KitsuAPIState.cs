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
    /// but not yet implemented. Don't use this!
    /// </summary>
    class KitsuAPIState : APIState
    {
        public KitsuAPIState() : base("https://kitsu.io/api/edge/")
        {
            availlable = false;
            lastChecked = DateTime.UtcNow;
        }

        public override Task<string> getRequestFromSearch(string query)
        {
            throw new NotImplementedException();
        }

        public override Task<JObject> requestAPI(string request)
        {
            throw new NotImplementedException();
        }

        public override bool testAPI()
        {
            throw new NotImplementedException();
        }

        protected override long CheckKnownIDs(string query)
        {
            throw new NotImplementedException();
        }
    }
}

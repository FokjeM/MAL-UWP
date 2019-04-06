using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Windows.Storage;


namespace MAL_UWP_Nightmare
{
    class OfflineAPIState : APIState
    {
        public OfflineAPIState() : base("LocalOnly")
        {
            availlable = false;
            lastChecked = DateTime.UtcNow;
        }

        /// <summary>
        /// Checks the local storage for a resource matching what was being searched.
        /// </summary>
        /// <param name="query">The query to search for. Same as with the APIs, "{type}/{search}"</param>
        /// <returns>The path to the file if it's locally availlable</returns>
        public override async Task<string> getRequestFromSearch(string query)
        {
            string[] path = query.Split('/');
            try
            {
                StorageFolder folder = await localPages.GetFolderAsync(path[0]);
                if(folder.TryGetItemAsync(path[1] + ".json").AsTask().Result != null)
                {
                    return string.Concat(query, ".json");
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        public override async Task<JObject> requestAPI(string request)
        {
            string[] path = request.Split('/');
            try
            {
                StorageFolder folder = await localPages.GetFolderAsync(path[0]);
                return JObject.Parse(await FileIO.ReadTextAsync(await folder.GetFileAsync(path[1] + ".json")));
            }
            catch
            {
                return null;
            }
        }

        public override bool testAPI()
        {
           JObject response = requestAPI("anime/Bakemonogatari").Result;
            if (response != null)
            {
                availlable = true;
            }
            else
            {
                availlable = false;
            }
            lastChecked = DateTime.UtcNow;
            return availlable;
        }

        /// <summary>
        /// Implemented out of necessity, but IDs are not stored for offline use.
        /// </summary>
        /// <param name="query">A string suitable to build a search query</param>
        /// <returns>0. Always.</returns>
        protected override long CheckKnownIDs(string query)
        {
            return 0L;
        }
    }
}

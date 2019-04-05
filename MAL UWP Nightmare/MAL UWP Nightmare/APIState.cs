using System;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace MAL_UWP_Nightmare
{
    abstract class APIState
    {
        /// <summary>
        /// The URL for the implemented API
        /// </summary>
        public readonly string API_URL;
        /// <summary>
        /// Wether or not the API was availlable the last time it was polled.
        /// </summary>
        protected bool availlable;
        /// <summary>
        /// The last time this API was checked for online/ready state.
        /// </summary>
        protected DateTime lastChecked;

        public APIState(string url)
        {
            API_URL = url;
        }

        /// <summary>
        /// Test wether or not the API can be reached.
        /// This method <b>should not</b> call the API if
        /// it was successful within the last 5 minutes.
        /// This is to prevent too many requests.
        /// </summary>
        /// <returns></returns>
        public abstract bool testAPI();

        /// <summary>
        /// Poll the API for a response. Use this for API calls.
        /// This should return a <see cref="Newtonsoft.Json.Linq.JObject"/>
        /// </summary>
        /// <param name="request">The request to add to the API URL</param>
        /// <returns>A JSON object from the APIs response</returns>
        public abstract Task<JObject> requestAPI(string request);

        /// <summary>
        /// Poll an API for a search in order to find the ID, this should
        /// assume an exact query is used and only return a single result.
        /// </summary>
        /// <param name="query">The search query in the form of "<code>type/name</code>"</param>
        /// <returns>The resulting request based on the first search result</returns>
        public abstract Task<string> getRequestFromSearch(string query);

        /// <summary>
        /// A get function for <see cref="API_URL"/>
        /// </summary>
        /// <returns>The base URL for the implemented API</returns>
        public string getURL()
        {
            return this.API_URL;
        }
    }
}

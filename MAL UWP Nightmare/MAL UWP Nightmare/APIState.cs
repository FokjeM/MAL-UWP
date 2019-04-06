using System;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using Windows.Storage;

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
        /// <summary>
        /// A list of known IDs for requests. The format is a Key/Value pair,
        /// built as {type}/{name}: "{MAL ID} : {Kitsu ID}"
        /// </summary>
        public static JObject knownIDs;
        /// <summary>
        /// Folder for application storage. Will also contain the locally saved files
        /// </summary>
        public readonly StorageFolder localPages = ApplicationData.Current.LocalFolder;
        /// <summary>
        /// Custom UA Header for web requests
        /// </summary>
        protected readonly string userAgent = "MAL_Nightmares/0.1 (" + new Windows.Security.ExchangeActiveSyncProvisioning.EasClientDeviceInformation().OperatingSystem + "; I; en-us) UWP/10";

        public APIState(string url)
        {
            API_URL = url;
            if (knownIDs == null)
            {
                StorageFile idList = ApplicationData.Current.LocalFolder.CreateFileAsync("known_pages.json", CreationCollisionOption.OpenIfExists).AsTask().Result;
                string jsonFileContents = FileIO.ReadTextAsync(idList).AsTask().Result;
                if (jsonFileContents.Length > 10)
                {
                    knownIDs = JObject.Parse(jsonFileContents);
                }
                else
                {
                    knownIDs = new JObject();
                }
            }
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

        protected abstract long CheckKnownIDs(string query);

        /// <summary>
        /// Add a new entry to the knownIDs for this information, to make looking it up easier.
        /// This function checks for the existence of the token and only updates changed fields.
        /// Starts an asynchronous task to write to a local file.
        /// </summary>
        /// <param name="type">Type of data; aninme, manga, character, etc.</param>
        /// <param name="name">Name of the data; Medaka Box, Akamatsu Ken, etc.</param>
        /// <param name="idMAL">The MAL id provided by Jikan</param>
        /// <param name="idKitsu">the Kitsu ID provided by Kitsu</param>
        /// <returns>True if the info was added or updated, false if the info is already known.</returns>
        private bool addToKnownIDs(string type, string name, long idMAL, long idKitsu)
        {
            string token = string.Format("{0}/{1}", type, name);
            JToken value;
            if (knownIDs.ContainsKey(token))
            {
                string[] container = ((string)knownIDs.GetValue(token).ToObject("".GetType())).Split(new string[] { " : " }, StringSplitOptions.RemoveEmptyEntries);
                if (container[0].Equals(idMAL.ToString()) && container[1].Equals(idKitsu.ToString()))
                {
                    return false;
                }
                if (idMAL > 0L && !container[0].Equals(idMAL.ToString()))
                {
                    container[0] = idMAL.ToString();
                }
                else
                {
                    container[0] = 0L.ToString();
                }
                if (idKitsu > 0L && !container[1].Equals(idKitsu.ToString()))
                {
                    container[1] = idKitsu.ToString();
                }
                else
                {
                    container[1] = 0L.ToString();
                }
                string val = string.Concat(container[0], " : ", container[1]);
                value = JToken.FromObject(val);
            }
            else
            {
                value = JToken.FromObject(string.Format(idMAL.ToString(), " : ", idKitsu.ToString()));
            }
            knownIDs.Add(token, value);
            FileIO.WriteTextAsync(localPages.GetFileAsync("known_pages.json").AsTask().Result, knownIDs.ToString()).AsTask().Start();
            return true;
        }
    }
}

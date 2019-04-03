using System;
using Windows.Storage;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using Windows.Web.Http;

namespace MAL_Nightmare_viewer
{
    /// <summary>
    /// An adapter class so there needn't be duplicate calls and methods
    /// in the PageFactory. This handles the check and the call to the right API.
    /// Prefers local storage if availlable.
    /// 
    /// Saves the known combinations of Name / Type / MAL ID / Kitsu ID to a JSON file.
    /// Saves saved pages as JSON files. Uses the same format for the knownIDs
    /// </summary>
    class APIAdapter
    {
        APIState apiState = new APIState();
        StorageFolder localPageDir = ApplicationData.Current.LocalFolder;
        JObject knownIDs;

        public APIAdapter()
        {
            StorageFile idList = ApplicationData.Current.LocalFolder.CreateFileAsync("known_pages.json", CreationCollisionOption.OpenIfExists).GetResults();
            knownIDs = JObject.Parse(FileIO.ReadTextAsync(idList).GetResults());
        }
        
        /// <summary>
        /// Probes the locally saved files first, then checks APIs for info.
        /// Sends a request to the API supplied by the APIState and returns the JSON response.
        /// This method assumes the API sends JSON.
        /// This method knows that the API endpoint that gets called can be reached from the
        /// call to APIState.getCurrentURL().
        /// </summary>
        /// <param name="request">The request part of the API call. This should be supplied
        /// when calling the function, like "/anime/1". It's the section after the API endpoint</param>
        /// <returns>The API response as a JObject</returns>
        public async Task<JObject> requestAPI(string request)
        {
            JObject local = await checkLocalPages(request);
            if (!local.Equals(null))
            {
                return local;
            }
            string src = await apiState.getCurrentURL();
            if (!src.Equals("LocalOnly"))
            {
                HttpClient req = new HttpClient();
                Uri api = new Uri(src + request);
                HttpResponseMessage response = await req.GetAsync(api);
                JObject result = JObject.Parse(response.Content.ToString());

                return result;
            }
            return null;
        }

        /// <summary>
        /// Checks the local storage for a resource matching what was being searched.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private async Task<JObject> checkLocalPages(string request)
        {
            string[] path = request.Split('/');
            StorageFolder folder = await localPageDir.GetFolderAsync(path[0]);
            try
            {
                return JObject.Parse(await FileIO.ReadTextAsync(await folder.GetFileAsync(path[1] + ".json")));
            } catch
            {
                return null;
            }
        }

        private long[] checkKnownIDs(string type, string name)
        {
            return (long[])knownIDs.GetValue(string.Format("{0}{1}", type, name)).ToObject((new long[0]).GetType());
        } 

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
            string token = string.Format("{0}{1}", type, name);
            JToken value;
            if (knownIDs.ContainsKey(token))
            {
                long[] container = (long[])knownIDs.GetValue(token).ToObject(new long[0].GetType());
                if(container[0].Equals(idMAL) && container[1].Equals(idKitsu))
                {
                    return false;
                }
                if(idMAL > 0L && !container[0].Equals(idMAL))
                {
                    container[0] = idMAL;
                }
                if (idKitsu > 0L && !container[1].Equals(idMAL))
                {
                    container[1] = idKitsu;
                }
                value = JToken.FromObject(container);
            }else
            {
                value = JToken.FromObject(new long[] { idMAL, idKitsu });
            }
            knownIDs.Add(token, value);
            FileIO.WriteTextAsync(localPageDir.GetFileAsync("known_pages.json").GetResults(), knownIDs.ToString()).AsTask().Start();
            return true;
        }


    }
}

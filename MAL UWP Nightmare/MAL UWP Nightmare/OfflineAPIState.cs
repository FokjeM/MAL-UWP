using System;
using System.Collections.Generic;
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
            Task<StorageFolder> f = localPages.CreateFolderAsync("test", CreationCollisionOption.OpenIfExists).AsTask();
            JObject test = new JObject();
            test.Add("test", new JValue("test"));
            FileIO.WriteTextAsync(f.Result.CreateFileAsync("file.json", CreationCollisionOption.OpenIfExists).AsTask().Result, test.ToString()).AsTask().Wait();
            
        }

        /// <summary>
        /// Checks the local storage for a resource matching what was being searched.
        /// </summary>
        /// <param name="query">The query to search for. Same as with the APIs, "{type}/{search}"</param>
        /// <returns>The path to the file if it's locally availlable</returns>
        public override  string GetRequestFromSearch(string query)
        {
            string[] path = query.ToLower().Split('/');
            try
            {
                StorageFolder folder = localPages.GetFolderAsync(path[0]).AsTask().Result;
                var fileList = folder.GetFilesAsync().AsTask().Result;
                foreach(StorageFile s in fileList)
                {
                    if(s.Name.ToLower().Equals(path[1] + ".json"))
                    {
                        return string.Concat(query.ToLower(), ".json");
                    }
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        public async override Task<string> GetRequestFromSearchAsync(string query)
        {
            string[] path = query.ToLower().Split('/');
            try
            {
                StorageFolder folder = await localPages.GetFolderAsync(path[0]);
                var fileList = await folder.GetFilesAsync();
                foreach (StorageFile s in fileList)
                {
                    if (s.Name.ToLower().Equals(path[1] + ".json"))
                    {
                        return string.Concat(query.ToLower(), ".json");
                    }
                }
                if (folder.TryGetItemAsync(path[1] + ".json").AsTask().Result != null)
                {
                    return query;
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        public override JObject GetSeasonals()
        {
            return new JObject();
        }

        public override JObject RequestAPI(string request)
        {
            string[] path = request.Split('/');
            try
            {
                StorageFolder folder = localPages.GetFolderAsync(path[0]).AsTask().Result;
                return JObject.Parse(FileIO.ReadTextAsync(folder.GetFileAsync(path[1] + ".json").AsTask().Result).AsTask().Result);
            }
            catch
            {
                return null;
            }
        }

        public async override Task<JObject> RequestAPIAsync(string request)
        {
            string[] path = request.Split('/');
            try
            {
                StorageFolder folder = await localPages.GetFolderAsync(path[0]);
                return JObject.Parse(FileIO.ReadTextAsync(folder.GetFileAsync(path[1] + ".json").AsTask().Result).AsTask().Result);
            }
            catch
            {
                return null;
            }
        }

        public override List<SearchResult> SearchAPI(string query)
        {
            string[] path = query.ToLower().Split('/');
            List<SearchResult> resultList = new List<SearchResult>(25);
            try
            {
                StorageFolder folder = localPages.GetFolderAsync(path[0]).AsTask().Result;
                var fileList = folder.GetFilesAsync().AsTask().Result;
                foreach (StorageFile s in fileList)
                {
                    if (s.Name.ToLower().Contains(path[1]))
                    {
                        JObject file = JObject.Parse(FileIO.ReadTextAsync(s).AsTask().Result);
                        long id = long.Parse((string)file.GetValue("mal_id").ToObject("".GetType()));
                        string title = (string)file.GetValue("title").ToObject("".GetType());
                        string image = (string)file.GetValue("image").ToObject("".GetType());
                        SearchResult res = new SearchResult(path[0], title, image, id);
                        resultList.Add(res);
                    }
                }
            } catch
            {
                return null;
            }
            return resultList;
        }

        public async override Task<List<SearchResult>> SearchAPIAsync(string query)
        {
            string[] path = query.ToLower().Split('/');
            List<SearchResult> resultList = new List<SearchResult>(25);
            try
            {
                StorageFolder folder = await localPages.GetFolderAsync(path[0]);
                var fileList = await folder.GetFilesAsync();
                foreach (StorageFile s in fileList)
                {
                    if (s.Name.ToLower().Contains(path[1]))
                    {
                        JObject file = JObject.Parse(FileIO.ReadTextAsync(s).AsTask().Result);
                        long id = long.Parse((string)file.GetValue("mal_id").ToObject("".GetType()));
                        string title = (string)file.GetValue("title").ToObject("".GetType());
                        string image = (string)file.GetValue("image").ToObject("".GetType());
                        SearchResult res = new SearchResult(path[0], title, image, id);
                        resultList.Add(res);
                    }
                }
            }
            catch
            {
                return null;
            }
            return resultList;
        }

        public override bool TestAPI()
        {
           JObject response = RequestAPI("test/file");
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
        /// <returns>0. Always. IDs are irrelevant for locally saved pages</returns>
        protected override long CheckKnownIDs(string query)
        {
            return 0L;
        }
    }
}

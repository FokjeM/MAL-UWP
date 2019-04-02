using Windows.Storage;
using Windows.Data.Json;

namespace MAL_Nightmare_viewer
{
    class APIAdapter
    {
        APIState apiState = new APIState();
        StorageFolder localPageDir = ApplicationData.Current.LocalFolder;
        JsonObject knownIDs = new JsonObject();
        
        public JsonObject requestMAL(string request)
        {
            return null;
        }
        //await ApplicationData.Current.LocalFolder.CreateFileAsync("known_pages.json", CreationCollisionOption.OpenIfExists)
    }
}

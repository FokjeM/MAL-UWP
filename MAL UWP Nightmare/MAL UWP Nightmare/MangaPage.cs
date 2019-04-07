using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using Windows.Storage;

namespace MAL_UWP_Nightmare
{
    class MangaPage : ContentPage
    {
        private List<string> authors;

        public override bool SavePage()
        {
            StorageFolder folder = ApplicationData.Current.LocalFolder.CreateFolderAsync("manga", CreationCollisionOption.OpenIfExists).AsTask().Result;
            StorageFile file = folder.CreateFileAsync(_title.ToString() + ".json", CreationCollisionOption.OpenIfExists).AsTask().Result;
            try
            {
                FileIO.WriteTextAsync(file, origin.ToString()).AsTask().Wait();
                _url = file.Path;
            }
            catch
            {
                return false;
            }
            return true;
        }

        public override void SetContent(JObject json)
        {
            if (json.First.Value<string>().ToLower().Contains("error"))
            {
                SetErrorContent(json.First.Value<string>());
                return;
            }
            base.SetContent(json);
            authors = new List<string>();
            JToken auths = json.GetValue("authors");
            foreach (JToken jt in auths.Children())
            {
                authors.Add(jt.Children()["name"].Value<string>());
            }
        }
    }
}
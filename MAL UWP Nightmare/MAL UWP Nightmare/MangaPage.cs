using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using Windows.Storage;

namespace MAL_UWP_Nightmare
{
    public class MangaPage : ContentPage
    {
        private List<string> _authors;
        public List<string> authors
        {
            get
            {
                return _authors;
            }
        }

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
            if (json.First.ToObject<string>().ToLower().Contains("error"))
            {
                SetErrorContent(json.First.Value<string>());
                return;
            }
            base.SetContent(json);
            _authors = new List<string>();
            JToken auths = json.GetValue("authors");
            foreach (JToken jt in auths.Children())
            {
                authors.Add(jt["name"].Value<string>());
            }
        }
    }
}
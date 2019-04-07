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
            StorageFile file = folder.CreateFileAsync(title.ToString() + ".json", CreationCollisionOption.OpenIfExists).AsTask().Result;
            try
            {
                FileIO.WriteTextAsync(file, origin.ToString()).AsTask().Wait();
                url = file.Path;
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
            running = (bool)json.GetValue("publishing").ToObject(new bool().GetType());
            JToken published = json.GetValue("published");
            if (published["from"].Value<object>() == null)
            {
                startDate = DateTime.MaxValue;
            }
            else
            {
                startDate = published["from"].Value<DateTime>();
            }
            if (published["to"].Value<object>() == null)
            {
                endDate = DateTime.MaxValue;
            }
            else
            {
                endDate = published["from"].Value<DateTime>();
            }
            authors = new List<string>();
            JToken auths = json.GetValue("authors");
            foreach (JToken jt in auths.Children())
            {
                authors.Add(jt.Children()["name"].Value<string>());
            }
        }
    }
}
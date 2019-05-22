using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage;

namespace MAL_UWP_Nightmare
{
    public class MangaPage : ContentPage
    {
        private List<string> _authors;
        public List<string> Authors
        {
            get
            {
                return _authors;
            }
        }

        public override bool SavePage()
        {
            StorageFolder folder;
            StorageFile file;
            Task<StorageFolder> folderTask = ApplicationData.Current.LocalFolder.CreateFolderAsync("manga", CreationCollisionOption.OpenIfExists).AsTask();
            folderTask.RunSynchronously();
            folder = folderTask.Result;
            try
            {
                Task<StorageFile> fileTask = folder.CreateFileAsync(_title.ToString() + ".json", CreationCollisionOption.FailIfExists).AsTask();
                fileTask.RunSynchronously();
                file = fileTask.Result;
            }
            catch
            {
                Task<StorageFile> fileTask = folder.GetFileAsync(_title.ToString() + ".json").AsTask();
                fileTask.RunSynchronously();
                file = fileTask.Result;
            }
            try
            {
                FileIO.WriteTextAsync(file, origin.ToString()).AsTask().RunSynchronously();
                _url = file.Path;
            }
            catch
            {
                return false;
            }
            return true;
        }

        public async override Task<bool> SavePageAsync()
        {
            StorageFolder folder;
            StorageFile file;
            folder = await ApplicationData.Current.LocalFolder.CreateFolderAsync("manga", CreationCollisionOption.OpenIfExists);
            try
            {
                file = await folder.CreateFileAsync(_title.ToString() + ".json", CreationCollisionOption.FailIfExists);
            }
            catch
            {
                file = await folder.GetFileAsync(_title.ToString() + ".json");
            }
            try
            {
                await FileIO.WriteTextAsync(file, origin.ToString());
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
                Authors.Add(jt["name"].Value<string>());
            }
        }
    }
}
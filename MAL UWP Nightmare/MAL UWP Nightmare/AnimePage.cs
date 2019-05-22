using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml.Media.Imaging;

namespace MAL_UWP_Nightmare
{
    public class AnimePage : ContentPage
    {
        private string _premiereSeason;
        public string PremiereSeason
        {
            get
            {
                return _premiereSeason;
            }
        }
        private string _broadcast;
        public string Broadcast
        {
            get
            {
                return _broadcast;
            }
        }
        private List<string> _producers;
        public List<string> Producers
        {
            get
            {
                return _producers;
            }
        }
        private List<string> _licensors;
        public List<string> Licensors
        {
            get
            {
                return _licensors;
            }
        }
        private List<string> _studios;
        public List<string> Studios
        {
            get
            {
                return _studios;
            }
        }
        private List<string> _openings;
        public List<string> Openings
        {
            get
            {
                return _openings;
            }
        }
        private List<string> _endings;
        public List<string> Endings
        {
            get
            {
                return _endings;
            }
        }

        public override bool SavePage()
        {
            StorageFolder folder;
            StorageFile file;
            Task<StorageFolder> folderTask = ApplicationData.Current.LocalFolder.CreateFolderAsync("anime", CreationCollisionOption.OpenIfExists).AsTask();
            folderTask.RunSynchronously();
            folder = folderTask.Result;
            try
            {
                Task<StorageFile> fileTask = folder.CreateFileAsync(_title.ToString() + ".json", CreationCollisionOption.FailIfExists).AsTask();
                fileTask.RunSynchronously();
                file = fileTask.Result;
            } catch
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
            folder = await ApplicationData.Current.LocalFolder.CreateFolderAsync("anime", CreationCollisionOption.OpenIfExists);
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
                SetErrorContent(json.First.ToObject<string>());
                return;
            }
            base.SetContent(json);
            _premiereSeason = (string)json.GetValue("premiered").ToObject("".GetType());
            _broadcast = (string)json.GetValue("broadcast").ToObject("".GetType());
            JToken prods = json.GetValue("producers");
            _producers = new List<string>();
            foreach(JToken jt in prods.Children())
            {
                _producers.Add(jt["name"].Value<string>());
            }
            JToken lics = json.GetValue("licensors");
            _licensors = new List<string>();
            foreach (JToken jt in lics.Children())
            {
                _licensors.Add(jt["name"].Value<string>());
            }
            JToken studs = json.GetValue("studios");
            _studios = new List<string>();
            foreach (JToken jt in studs.Children())
            {
                _studios.Add(jt["name"].Value<string>());
            }
            _openings = new List<string>((string[])json.GetValue("opening_themes").ToObject(new string[] { }.GetType()));
            _endings = new List<string>((string[])json.GetValue("ending_themes").ToObject(new string[] { }.GetType()));
        }
    }
}
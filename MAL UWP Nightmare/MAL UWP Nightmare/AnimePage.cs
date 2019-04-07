using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using Windows.Storage;
using Windows.UI.Xaml.Media.Imaging;

namespace MAL_UWP_Nightmare
{
    public class AnimePage : ContentPage
    {
        private string _premiereSeason;
        public string premiereSeason
        {
            get
            {
                return _premiereSeason;
            }
        }
        private string _broadcast;
        public string broadcast
        {
            get
            {
                return _broadcast;
            }
        }
        private List<string> _producers;
        public List<string> producers
        {
            get
            {
                return _producers;
            }
        }
        private List<string> _licensors;
        public List<string> licensors
        {
            get
            {
                return _licensors;
            }
        }
        private List<string> _studios;
        public List<string> studios
        {
            get
            {
                return _studios;
            }
        }
        private List<string> _openings;
        public List<string> openings
        {
            get
            {
                return _openings;
            }
        }
        private List<string> _endings;
        public List<string> endings
        {
            get
            {
                return _endings;
            }
        }

        public override bool SavePage()
        {
            StorageFolder folder = ApplicationData.Current.LocalFolder.CreateFolderAsync("anime", CreationCollisionOption.OpenIfExists).AsTask().Result;
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
            _premiereSeason = (string)json.GetValue("premiered").ToObject("".GetType());
            _broadcast = (string)json.GetValue("broadcast").ToObject("".GetType());
            JToken prods = json.GetValue("producers");
            _producers = new List<string>();
            foreach(JToken jt in prods.Children())
            {
                _producers.Add(jt.Children()["name"].Value<string>());
            }
            JToken lics = json.GetValue("licensors");
            _licensors = new List<string>();
            foreach (JToken jt in lics.Children())
            {
                _licensors.Add(jt.Children()["name"].Value<string>());
            }
            JToken studs = json.GetValue("studios");
            _studios = new List<string>((string[])json.GetValue("").ToObject(new string[] { }.GetType()));
            foreach (JToken jt in studs.Children())
            {
                _studios.Add(jt.Children()["name"].Value<string>());
            }
            _openings = new List<string>((string[])json.GetValue("opening_themes").ToObject(new string[] { }.GetType()));
            _endings = new List<string>((string[])json.GetValue("ending_themes").ToObject(new string[] { }.GetType()));
        }
    }
}
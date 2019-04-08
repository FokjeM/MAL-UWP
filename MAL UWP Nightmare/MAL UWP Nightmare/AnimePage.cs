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
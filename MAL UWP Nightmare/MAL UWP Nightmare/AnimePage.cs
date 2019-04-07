using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using Windows.Storage;
using Windows.UI.Xaml.Media.Imaging;

namespace MAL_UWP_Nightmare
{
    class AnimePage : ContentPage
    {
        private string premiereSeason;
        private string broadcast;
        private List<string> producers;
        private List<string> licensors;
        private List<string> studios;
        private List<string> openings;
        private List<string> endings;

        public override bool SavePage()
        {
            StorageFolder folder = ApplicationData.Current.LocalFolder.CreateFolderAsync("anime", CreationCollisionOption.OpenIfExists).AsTask().Result;
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
            running = (bool)json.GetValue("airing").ToObject(new bool().GetType());
            JToken airedFrom = json.GetValue("aired_from");
            if(airedFrom.Value<object>() == null)
            {
                startDate = DateTime.MaxValue;
            } else
            {
                startDate = airedFrom.Value<DateTime>();
            }
            JToken airedTo = json.GetValue("aired_to");
            if (airedTo.Value<object>() == null)
            {
                endDate = DateTime.MaxValue;
            }
            else
            {
                endDate = airedTo.Value<DateTime>();
            }
            premiereSeason = (string)json.GetValue("premiered").ToObject("".GetType());
            broadcast = (string)json.GetValue("broadcast").ToObject("".GetType());
            JToken prods = json.GetValue("producers");
            producers = new List<string>();
            foreach(JToken jt in prods.Children())
            {
                producers.Add(jt.Children()["name"].Value<string>());
            }
            JToken lics = json.GetValue("licensors");
            licensors = new List<string>();
            foreach (JToken jt in lics.Children())
            {
                licensors.Add(jt.Children()["name"].Value<string>());
            }
            JToken studs = json.GetValue("studios");
            studios = new List<string>((string[])json.GetValue("").ToObject(new string[] { }.GetType()));
            foreach (JToken jt in studs.Children())
            {
                studios.Add(jt.Children()["name"].Value<string>());
            }
            openings = new List<string>((string[])json.GetValue("opening_themes").ToObject(new string[] { }.GetType()));
            endings = new List<string>((string[])json.GetValue("ending_themes").ToObject(new string[] { }.GetType()));
        }
    }
}
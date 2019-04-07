using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Windows.Storage;
using Windows.UI.Xaml.Media.Imaging;

namespace MAL_UWP_Nightmare
{
    class CharacterPage : IPage
    {
        private long id;
        private string url;
        private string name;
        private string kanjiName;
        private List<string> nicknames;
        private string about;
        private string mainImage;
        private List<AnimePage> anime;
        private List<MangaPage> manga;
        private Dictionary<PersonPage, string> voiceActors;
        private JObject origin;

        public bool IsLocal()
        {
            return !url.StartsWith("https://", StringComparison.OrdinalIgnoreCase);
        }

        public bool SavePage()
        {
            StorageFolder folder = ApplicationData.Current.LocalFolder.CreateFolderAsync("character", CreationCollisionOption.OpenIfExists).AsTask().Result;
            StorageFile file = folder.CreateFileAsync(name.ToString() + ".json", CreationCollisionOption.OpenIfExists).AsTask().Result;
            try
            {
                FileIO.WriteTextAsync(file, origin.ToString()).AsTask().Wait();
            } catch
            {
                return false;
            }
            return true;
        }

        public void SetContent(JObject json)
        {
            if(json.First.Value<string>().ToLower().Contains("error"))
            {
                SetErrorContent(json.First.Value<string>());
                return;
            }
            id = long.Parse((string)json.GetValue("mal_id").ToObject("".GetType()));
            url = (string)json.GetValue("url").ToObject("".GetType());
            name = (string)json.GetValue("name").ToObject("".GetType());
            kanjiName = (string)json.GetValue("name_kanji").ToObject("".GetType());
            nicknames = new List<string>((string[])json.GetValue("nicknames").ToObject(new string[] {  }.GetType()));
            about = (string)json.GetValue("about").ToObject("".GetType());
            mainImage = (string)json.GetValue("image").ToObject("".GetType());
            anime = new List<AnimePage>((AnimePage[])json.GetValue("anime").ToObject(new AnimePage[] { }.GetType()));
            manga = new List<MangaPage>((MangaPage[])json.GetValue("manga").ToObject(new MangaPage[] { }.GetType()));
            voiceActors = (Dictionary<PersonPage, string>)json.GetValue("voice_actors").ToObject(new Dictionary<PersonPage, string>().GetType());
            origin = json;
        }

        public void SetErrorContent(string errorMessage)
        {
            name = errorMessage;
        }
    }
}
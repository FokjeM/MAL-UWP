using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Windows.Storage;

namespace MAL_UWP_Nightmare
{
    /// <summary>
    /// This has been dropped, beause we cannot implement it without exceeding the APIs
    /// Request limit. We are only allowed 2 calls a second for a maximum of 30 a minute.
    /// That makes it impossible to add character info, which is an extra API call.
    /// 
    /// Not to mention that Kitsu still hasn't implemented this information.
    /// </summary>
    public class CharacterPage : IPage
    {
        private long _id;
        public long Id
        {
            get
            {
                return _id;
            }
        }
        private string _url;
        public string Url
        {
            get
            {
                return _url;
            }
        }
        private string _name;
        public string Name
        {
            get
            {
                return _name;
            }
        }
        private string _kanjiName;
        public string KanjiName
        {
            get
            {
                return _kanjiName;
            }
        }
        private List<string> _nicknames;
        public List<string> Nicknames
        {
            get
            {
                return _nicknames;
            }
        }
        private string _about;
        public string About
        { 
            get
            {
                return _about;
            }
        }
        private string _mainImage;
        public string MainImage
        {
            get
            {
                return _mainImage;
            }
        }
        private List<AnimePage> _anime;
        public List<AnimePage> Anime
        {
            get
            {
                return _anime;
            }
        }
        private List<MangaPage> _manga;
        public List<MangaPage> Manga
        {
            get
            {
                return _manga;
            }
        }
        private JObject origin;

        public bool IsLocal()
        {
            return !_url.StartsWith("https://", StringComparison.OrdinalIgnoreCase);
        }

        public bool SavePage()
        {
            StorageFolder folder;
            StorageFile file;
            Task<StorageFolder> folderTask = ApplicationData.Current.LocalFolder.CreateFolderAsync("character", CreationCollisionOption.OpenIfExists).AsTask();
            folderTask.RunSynchronously();
            folder = folderTask.Result;
            try
            {
                Task<StorageFile> fileTask = folder.CreateFileAsync(_name.ToString() + ".json", CreationCollisionOption.FailIfExists).AsTask();
                fileTask.RunSynchronously();
                file = fileTask.Result;
            }
            catch
            {
                Task<StorageFile> fileTask = folder.GetFileAsync(_name.ToString() + ".json").AsTask();
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

        public async Task<bool> SavePageAsync()
        {
            StorageFolder folder;
            StorageFile file;
            folder = await ApplicationData.Current.LocalFolder.CreateFolderAsync("characters", CreationCollisionOption.OpenIfExists);
            try
            {
                file = await folder.CreateFileAsync(_name.ToString() + ".json", CreationCollisionOption.FailIfExists);
            }
            catch
            {
                file = await folder.GetFileAsync(_name.ToString() + ".json");
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

        public void SetContent(JObject json)
        {
            if(json.First.Value<string>().ToLower().Contains("error"))
            {
                SetErrorContent(json.First.Value<string>());
                return;
            }
            _id = long.Parse((string)json.GetValue("mal_id").ToObject("".GetType()));
            _url = (string)json.GetValue("url").ToObject("".GetType());
            _name = (string)json.GetValue("name").ToObject("".GetType());
            _kanjiName = (string)json.GetValue("name_kanji").ToObject("".GetType());
            _nicknames = new List<string>((string[])json.GetValue("nicknames").ToObject(new string[] {  }.GetType()));
            _about = (string)json.GetValue("about").ToObject("".GetType());
            _mainImage = (string)json.GetValue("image").ToObject("".GetType());
            _anime = new List<AnimePage>((AnimePage[])json.GetValue("anime").ToObject(new AnimePage[] { }.GetType()));
            _manga = new List<MangaPage>((MangaPage[])json.GetValue("manga").ToObject(new MangaPage[] { }.GetType()));
            origin = json;
        }

        public void SetErrorContent(string errorMessage)
        {
            _about = errorMessage;
        }
    }
}
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace MAL_UWP_Nightmare
{
    public abstract class ContentPage : IPage
    {
        /// <summary>
        /// Dictionary to hold related items. Has to be set by the factory.
        /// </summary>
        private Dictionary<Related, string> _related;
        public Dictionary<Related,string> related
        {
            get
            {
                return _related;
            }
        }
        /// <summary>
        /// List to link all of the CharacterPages. Has to be set by the factory.
        /// </summary>
        private Dictionary<CharacterPage, string> characters;
        /// <summary>
        /// A List to hold all of the alternative titles. Has to be set by the factory.
        /// </summary>
        protected List<string> _altTitles;
        public List<string> altTitles
        {
            get
            {
                return _altTitles;
            }
        }
        /// <summary>
        /// A list to hold all of the Genres. Has to be set by the factory.
        /// </summary>
        protected List<string> _genres;
        public List<string> genres
        {
            get
            {
                return _genres;
            }
        }
        /// <summary>
        /// This is really only gonna see use in the SavePage function.
        /// </summary>
        protected long _id;
        public long id
        {
            get
            {
                return _id;
            }
        }
        protected string _synopsis;
        public string synopsis
        {
            get
            {
                return _synopsis;
            }
        }
        protected string _background;
        public string background
        {
            get
            {
                return _background;
            }
        }
        protected string _url;
        public string url
        {
            get
            {
                return _url;
            }
        }
        protected string _title;
        public string title
        {
            get
            {
                return _title;
            }
        }
        protected string _japTitle;
        public string japTitle
        {
            get
            {
                return _japTitle;
            }
        }
        protected string _engTitle;
        public string engTitle
        {
            get
            {
                return _engTitle;
            }
        }
        protected bool _running;
        public bool running
        {
            get
            {
                return _running;
            }
        }
        protected DateTime _startDate;
        public DateTime startDate
        {
            get
            {
                return _startDate;
            }
        }
        protected DateTime _endDate;
        public DateTime endDate
        {
            get
            {
                return _endDate;
            }
        }
        protected string _mainImage;
        public string mainImage
        {
            get
            {
                return _mainImage;
            }
        }
        protected string _status;
        public string status
        {
            get
            {
                return _status;
            }
        }
        protected string _type;
        public string type
        {
            get
            {
                return _type;
            }
        }
        
        protected JObject origin;

        /// <summary>
        /// Supplies info on wether or not this content is still being produced.
        /// </summary>
        /// <returns>true if in production, false if not.</returns>
        public bool IsRunning()
        {
            return _running;
        }

        /// <summary>
        /// Check to see if this content has certain genres.
        /// Useful if someone decides to modify the application to block R-Rated content.
        /// Or if someone decides to implement genres for anything.
        /// </summary>
        /// <param name="genre">the genre to check for.</param>
        /// <returns></returns>
        public bool HasGenre(string genre)
        {
            return genres.Contains(genre.ToLower());
        }

        public bool IsLocal()
        {
            return !_url.StartsWith("https://", StringComparison.OrdinalIgnoreCase);
        }

        public abstract bool SavePage();

        public virtual void SetContent(JObject json)
        {
            _id = long.Parse((string)json.GetValue("id").ToObject("".GetType()));
            _url = (string)json.GetValue("url").ToObject("".GetType());
            _title = (string)json.GetValue("title").ToObject("".GetType());
            _japTitle = (string)json.GetValue("title_japanese").ToObject("".GetType());
            _engTitle = (string)json.GetValue("title_english").ToObject("".GetType());
            _running = (bool)json.GetValue("running").ToObject(true.GetType());
            JToken runFrom = json.GetValue("run_from");
            if (runFrom.Value<object>() == null)
            {
                _startDate = DateTime.MaxValue;
            }
            else
            {
                _startDate = runFrom.Value<DateTime>();
            }
            JToken runTo = json.GetValue("run_to");
            if (runTo.Value<object>() == null)
            {
                _endDate = DateTime.MaxValue;
            }
            else
            {
                _endDate = runTo.Value<DateTime>();
            }
            _synopsis = (string)json.GetValue("synopsis").ToObject("".GetType());
            _background = (string)json.GetValue("background").ToObject("".GetType());
            _mainImage = (string)json.GetValue("image").ToObject("".GetType());
            _altTitles = new List<string>((string[])json.GetValue("title_synonyms").ToObject(new string[] { }.GetType()));
            _status = (string)json.GetValue("status").ToObject("".GetType());
            _type = (string)json.GetValue("type").ToObject("".GetType());
            _genres = new List<string>((string[])json.GetValue("genres").ToObject(new string[] { }.GetType()));
            origin = json;
        }
        
        public void SetErrorContent(string errorMessage)
        {
            _title = errorMessage;
        }

        public void setRelated(Dictionary<Related, string> related)
        {
            _related = related;
        }
    }
}

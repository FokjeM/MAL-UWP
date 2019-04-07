﻿using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace MAL_UWP_Nightmare
{
    abstract class ContentPage : IPage
    {
        /// <summary>
        /// Dictionary to hold related items. Has to be set by the factory.
        /// </summary>
        protected Dictionary<ContentPage, string> related;
        /// <summary>
        /// List to link all of the CharacterPages. Has to be set by the factory.
        /// </summary>
        protected List<CharacterPage> characters;
        /// <summary>
        /// A List to hold all of the alternative titles. Has to be set by the factory.
        /// </summary>
        protected List<string> altTitles;
        /// <summary>
        /// A list to hold all of the Genres. Has to be set by the factory.
        /// </summary>
        protected List<string> genres;
        /// <summary>
        /// This is really only gonna see use in the SavePage function.
        /// </summary>
        protected long id;
        protected string synopsis;
        protected string background;
        protected string url;
        protected string title;
        protected string japTitle;
        protected string engTitle;
        protected bool running;
        protected DateTime startDate;
        protected DateTime endDate;
        protected string mainImage;
        protected JObject origin;

        /// <summary>
        /// Supplies info on wether or not this content is still being produced.
        /// </summary>
        /// <returns>true if in production, false if not.</returns>
        public bool IsRunning()
        {
            return running;
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
            return !url.StartsWith("https://", StringComparison.OrdinalIgnoreCase);
        }

        public abstract bool SavePage();

        public virtual void SetContent(JObject json)
        {
            id = long.Parse((string)json.GetValue("mal_id").ToObject("".GetType()));
            url = (string)json.GetValue("url").ToObject("".GetType());
            title = (string)json.GetValue("title").ToObject("".GetType());
            japTitle = (string)json.GetValue("title_japanese").ToObject("".GetType());
            engTitle = (string)json.GetValue("title_english").ToObject("".GetType());
            //Set properties about running and the dates in the extending classes
            //For anime, this is the "airing" and "aired" property
            //For manga, it's "publishing" and "published"
            synopsis = (string)json.GetValue("synopsis").ToObject("".GetType());
            background = (string)json.GetValue("background").ToObject("".GetType());
            mainImage = (string)json.GetValue("image").ToObject("".GetType());
            related = (Dictionary<ContentPage, string>)json.GetValue("related").ToObject(new Dictionary<ContentPage, string>().GetType());
            characters = new List<CharacterPage>((CharacterPage[])json.GetValue("characters").ToObject(new CharacterPage[] { }.GetType()));
            altTitles = new List<string>((string[])json.GetValue("title_synonyms").ToObject(new string[] { }.GetType()));
            JToken gens = json.GetValue("genres");
            genres = new List<string>();
            foreach (JToken jt in gens.Children())
            {
                genres.Add(jt.Children()["name"].Value<string>());
            }
            origin = json;
        }
        
        public void SetErrorContent(string errorMessage)
        {
            title = errorMessage;
        }
    }
}

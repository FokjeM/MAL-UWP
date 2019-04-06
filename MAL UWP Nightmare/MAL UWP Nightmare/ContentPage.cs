using Newtonsoft.Json.Linq;
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
        /// A list to hold all of the Images once loaded. Has to be set by the factory.
        /// </summary>
        protected List<BitmapImage> images;
        /// <summary>
        /// This is really only gonna see use in the SavePage function.
        /// </summary>
        private long id;
        private string synopsis;
        private string background;
        private string url;
        private string title;
        private string japTitle;
        private string engTitle;
        private bool running;
        private DateTime startDate;
        private DateTime endDate;
        private BitmapImage mainImage;

        public bool isRunning()
        {
            return running;
        }

        public bool hasGenre(string genre)
        {
            return genres.Contains(genre);
        }

        public void setSynopsis(string synopsis)
        {
            this.synopsis = synopsis;
        }

        public void setBackground(string background)
        {
            this.background = background;
        }

        public void setRelated(Dictionary<ContentPage, string> related)
        {
            this.related = related;
        }

        public void setCharacters(List<CharacterPage> characters)
        {
            this.characters = characters;
        }

        public bool IsLocal()
        {
            return !url.StartsWith("https://", StringComparison.OrdinalIgnoreCase);
        }

        public void LoadPage()
        {
            //Load the page anew from the source! Might be better to figure out a way to replace this with another ContentPage
            throw new NotImplementedException();
        }

        public bool SavePage()
        {
            //Save the page to a JSON file. Save the file in LocalFolder/{type}/{title}
            throw new NotImplementedException();
        }

        public void SetContent(JObject json)
        {
            //Pull off a magic trick to get the right info out of the JObject
            setSynopsis(json.GetValue("synopsis").ToString());
            setBackground(json.GetValue("background").ToString());
            setRelated(json.GetValue("related"));
        }

        public void SetImages(BitmapImage[] images)
        {
            throw new NotImplementedException();
        }

        public void SetMainImage(BitmapImage image)
        {
            throw new NotImplementedException();
        }

        public void SetInfo(string[] info)
        {
            throw new NotImplementedException();
        }

        public void SetErrorContent(string errorMessage)
        {
            throw new NotImplementedException();
        }
    }
}

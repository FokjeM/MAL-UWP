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

        /// <summary>
        /// Set the suppplied synopsis for this content.
        /// </summary>
        /// <param name="synopsis">the supplied synopsis, as a string.</param>
        public void SetSynopsis(string synopsis)
        {
            this.synopsis = synopsis;
        }

        /// <summary>
        /// Set the supplied background information for this content.
        /// </summary>
        /// <param name="background">the supplied background info, as a string</param>
        public void SetBackground(string background)
        {
            this.background = background;
        }

        /// <summary>
        /// Set the related media for this object and pretend you know how it matches up.
        /// </summary>
        /// <param name="related">the supplied related media, as a Dictionary</param>
        public void SetRelated(Dictionary<ContentPage, string> related)
        {
            this.related = related;
        }

        /// <summary>
        /// Set the characters for this media. Don't even think you know all of them.
        /// </summary>
        /// <param name="characters">The supplied list of characters</param>
        public void SetCharacters(List<CharacterPage> characters)
        {
            this.characters = characters;
        }
        
        public bool IsLocal()
        {
            return !url.StartsWith("https://", StringComparison.OrdinalIgnoreCase);
        }

        public abstract bool SavePage();

        public abstract void SetContent(JObject json);
        
        public void SetErrorContent(string errorMessage)
        {
            title = errorMessage;
        }
    }
}

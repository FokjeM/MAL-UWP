using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace MAL_UWP_Nightmare
{
    interface IPage
    {
        /// <summary>
        /// Set all of the page-specific content.
        /// </summary>
        /// <param name="json">A customized JSON response for the IPage implementation.
        /// This is also what gets exported for <see cref="SavePage()"/></param>
        void SetContent(JObject json);
        /// <summary>
        /// Save this page locally, in the same format that it gets loaded in.
        /// That allows for REALLY easy parsing.
        /// Set the url to the local filepath when calling this.
        /// </summary>
        /// <returns>true if it succesfully writes to a file. False if everything breaks</returns>
        bool SavePage();
        /// <summary>
        /// Check if this page is saved locally.
        /// Might need a reload if <see cref="SavePage()"/> was implemented incorrectly.
        /// </summary>
        /// <returns>true if this page is saved locally, false if it isn't.</returns>
        bool IsLocal();
        /// <summary>
        /// I'd rather not describe this as calling this function means something is broken.
        /// </summary>
        /// <param name="errorMessage">The error message to display.</param>
        void SetErrorContent(string errorMessage);
    }
}

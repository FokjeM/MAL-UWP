using System;
using System.Threading.Tasks;
using Windows.Web.Http;

namespace MAL_Nightmare_viewer
{
    /// <summary>
    /// Class that supplies the API currently in use.
    /// Has methods to test both the Jikan and Kitsu APIs and will set both availlable states.
    /// Has a method to test access to local storafe; something it should always get in the
    /// ApplicationData folder and file objects because of how UWP is set up.
    /// Regular checks should be done if either or both are offline.
    /// </summary>
    class APIMachine
    {
        public APIMachine()
        {

        }
    }
}

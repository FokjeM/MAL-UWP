using System;
using System.Threading.Tasks;
using Windows.Web.Http;

namespace MAL_Nightmare_viewer
{
    /// <summary>
    /// Class that supplies the API currently in use.
    /// Has methods to test both the Jikan and Kitsu APIs and will set both availlable states.
    /// Regular checks should be done if either or both are offline.
    /// 
    /// Instantiating it can take a while as it emmediately tests the resources.
    /// This'll take a while as this goes at IE speeds...
    /// </summary>
    class APIMachine
    {
        public APIMachine()
        {

        }
    }
}

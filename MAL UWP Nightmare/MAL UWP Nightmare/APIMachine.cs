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
        public readonly static string MAL_URL = "";
        public readonly static string KITSU_URL = "https://kitsu.io/api/edge/";
        private readonly string userAgent = "MAL_Nightmares/0.1 (" + new Windows.Security.ExchangeActiveSyncProvisioning.EasClientDeviceInformation().OperatingSystem + "; I; en-us) UWP/8.1";

        private bool jikanAvaillable;
        private bool kitsuAvaillable;
        public bool done = false;

        public APIMachine()
        {
            //testJikan();
            //testKitsu();
        }

        /// <summary>
        /// Tests the Jikan API to poll MAL.
        /// Added the anime/1 endpoint to ensure both MAL and the API are up
        /// </summary>
        private async Task<bool> testJikan()
        {
            HttpClient request = new HttpClient();
            Uri api = new Uri(MAL_URL + "anime/1");
            HttpResponseMessage response = await request.GetAsync(api);
            jikanAvaillable = response.IsSuccessStatusCode;
            return response.IsSuccessStatusCode;
        }

        /// <summary>
        /// Tests the Kitsu API to poll Kitsu
        /// Added the anime/1 endpoint because the clean API URL is a 404
        /// Kitsu does not adhere to the expectation of API/Author info.
        /// </summary>
        private async Task<bool> testKitsu()
        {
            HttpClient request = new HttpClient();
            Uri api = new Uri(KITSU_URL + "anime/1");
            HttpResponseMessage response = await request.GetAsync(api);
            kitsuAvaillable = response.IsSuccessStatusCode;
            return response.IsSuccessStatusCode;
        }

        /// <summary>
        /// A method to get whatever URL is known to be availlable.
        /// This method only tests the availlability of Kitsu if Jikan/MAL is down.
        /// </summary>
        /// <returns>The correct endpoint for an API call as URI</returns>
        public async Task<string> getCurrentURL()
        {
            if (await testJikan())
            {
                return MAL_URL;
            }
            else
            {

                if (await testKitsu())
                {
                    return KITSU_URL;
                }
                else
                {
                    return "LocalOnly";
                }
            }
        }
    }
}

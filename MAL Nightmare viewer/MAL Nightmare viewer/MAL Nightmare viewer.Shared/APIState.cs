using System;
using Windows.Web.Http;

namespace MAL_Nightmare_viewer
{
    /// <summary>
    /// Class that supplies the API currently in use.
    /// Has methods to test both the Jikan and Kitsu APIs and will set both availlable states.
    /// Regular checks should be done if either or both are offline.
    /// </summary>
    class APIState
    {
        public readonly static string MAL_URL = "https://api.jikan.moe/v3/";
        public readonly static string KITSU_URL = "https://kitsu.io/api/edge/";
        private readonly string userAgent = "MAL_Nightmares/0.1 (" + new Windows.Security.ExchangeActiveSyncProvisioning.EasClientDeviceInformation().OperatingSystem + "; I; en-us) UWP/8.1";

        private bool jikanAvaillable;
        private bool kitsuAvaillable;

        private async void testJikan()
        {
            HttpClient request = new HttpClient();
            Uri api = new Uri(MAL_URL);
            if(request.DefaultRequestHeaders.UserAgent.TryParseAdd(userAgent))
            {
                HttpResponseMessage response = await request.GetAsync(api);
                jikanAvaillable = response.IsSuccessStatusCode;
            }
        }

        private async void testKitsu()
        {
            HttpClient request = new HttpClient();
            Uri api = new Uri(KITSU_URL);
            if (request.DefaultRequestHeaders.UserAgent.TryParseAdd(userAgent))
            {
                HttpResponseMessage response = await request.GetAsync(api);
                kitsuAvaillable = response.IsSuccessStatusCode;
            }
        }

        public Uri getCurrentURL()
        {
            testJikan();
            if (jikanAvaillable)
            {
                return MAL_URL;
            }
            else
            {
                testKitsu();
                if(kitsuAvaillable)
                {
                    return kitsuAvaillable;
                }
                else
                {
                    return null;
                }
            }
    }
}

using System;

namespace MAL_UWP_Nightmare
{
    /// <summary>
    /// Dropped in the new design; Facade pattern obsoletes this IObserver.
    /// </summary>
    class SplashScreen : IObserver
    {
        HomePageBackend home;

        public IPage NotifyMe(SearchResult res)
        {
            throw new NotImplementedException();
        }

        public void NotifyMe(IPage p)
        {
            home = (HomePageBackend)p;
        }
    }
}

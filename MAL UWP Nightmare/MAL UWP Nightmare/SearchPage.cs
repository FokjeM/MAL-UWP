using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace MAL_UWP_Nightmare
{
    public class SearchPage : IPage
    {
        private List<SearchResult> _results;
        public List<SearchResult> Results
        {
            get
            {
                return _results;
            }
        }

        private IObserver observer;

        public SearchPage(IObserver o)
        {
            observer = o;
        }

        /// <summary>
        /// Kindly ask main to load the requested page.
        /// Just not in a kind manner.
        /// </summary>
        /// <param name="res"></param>
        public IPage NotifyObserver(SearchResult res)
        {
            return observer.NotifyMe(res);
        }

        public void SelectResult(SearchResult res)
        {
            NotifyObserver(res);
        }

        /// <summary>
        /// Be warned! Results from the OfflineAPIState will ALWAYS be manga or anime
        /// </summary>
        /// <param name="results"></param>
        public void SetResults(List<SearchResult> results)
        {
            _results = results;
        }

        /// <summary>
        /// Result is given as a more useful object for this class.
        /// </summary>
        /// <param name="json"></param>
        public void SetContent(JObject json)
        {
            return;
        }

        /// <summary>
        /// Savepages cannot be saved.
        /// </summary>
        /// <returns>False, always.</returns>
        public bool SavePage()
        {
            return false;
        }

        /// <summary>
        /// <see cref="SavePage()"/>
        /// </summary>
        /// <returns></returns>
        public async Task<bool> SavePageAsync()
        {
            return false;
        }

        /// <summary>
        /// <see cref="SavePage()"/>
        /// </summary>
        /// <returns></returns>
        public bool IsLocal()
        {
            return false;
        }

        /// <summary>
        /// Doesn't do anything; 
        /// </summary>
        /// <param name="errorMessage"></param>
        public void SetErrorContent(string errorMessage)
        {
            return;
        }
    }
}

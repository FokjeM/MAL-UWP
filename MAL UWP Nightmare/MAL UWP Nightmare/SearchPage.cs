using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public bool IsLocal()
        {
            return false;
        }

        public bool SavePage()
        {
            return false;
        }

        public void SetContent(JObject json)
        {
            throw new NotImplementedException();
        }

        public void SetErrorContent(string errorMessage)
        {
            throw new NotImplementedException();
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

        public void selectResult(SearchResult res)
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
    }
}

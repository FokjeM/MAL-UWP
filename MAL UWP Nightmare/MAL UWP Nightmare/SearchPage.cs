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
        public List<SearchResult> results
        {
            get
            {
                return _results;
            }
        }
        private object _observer; //To do: Change object to type Main.
        public object observer
        {
            get
            {
                return _observer;
            }
        }
        public bool IsLocal()
        {
            throw new NotImplementedException();
        }

        public bool SavePage()
        {
            throw new NotImplementedException();
        }

        public void SetContent(JObject json)
        {
            throw new NotImplementedException();
        }

        public void SetErrorContent(string errorMessage)
        {
            throw new NotImplementedException();
        }
        private void notifyObserver()
        {

        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace MAL_UWP_Nightmare
{
    public class HomePageBackend : IPage
    {
        private List<SearchResult> seasonals;
        private IObserver observer;

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
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAL_UWP_Nightmare
{
    class SearchResult
    {
        private string title;
        private string image;
        private long id;
        private IPage page;

        public SearchResult(string title, string image, long id)
        {
            this.title = title;
            this.image = image;
            this.id = id;
        }

        public void addPage(IPage page)
        {
            this.page = page;
        }
    }
}

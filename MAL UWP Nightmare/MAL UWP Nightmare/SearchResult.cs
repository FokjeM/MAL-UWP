using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAL_UWP_Nightmare
{
    public class SearchResult
    {
        public readonly string type;
        public readonly string title;
        public readonly string image;
        public readonly long id;
        private IPage page;

        public SearchResult(string type, string title, string image, long id)
        {
            this.type = type;
            this.title = title;
            this.image = image;
            this.id = id;
        }

        public void addPage(IPage page)
        {
            this.page = page;
        }

        public IPage GetPage()
        {
            return page;
        }
    }
}

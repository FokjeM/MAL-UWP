﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAL_UWP_Nightmare
{
    public class SearchResult
    {
        public string type { get; set; }
        public string title { get; set; }
        public string image { get; set; }
        public long id { get; set; }
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

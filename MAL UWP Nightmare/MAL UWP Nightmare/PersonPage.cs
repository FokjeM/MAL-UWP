﻿using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace MAL_UWP_Nightmare
{
    public class PersonPage : IPage
    {
        public bool IsLocal()
        {
            throw new System.NotImplementedException();
        }

        public bool SavePage()
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> SavePageAsync()
        {
            throw new System.NotImplementedException();
        }

        public void SetContent(JObject json)
        {
            throw new System.NotImplementedException();
        }

        public void SetErrorContent(string errorMessage)
        {
            throw new System.NotImplementedException();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.ContentManagement;
using Lombiq.RssReader.Models;

namespace Lombiq.RssReader.Services
{
    public class TextFieldSavingProvider : IRssFeedDataSavingProvider
    {
        public string ProviderType
        {
            get
            {
                return "TextField";
            }
        }

        public void Save(IRssFeedDataSavingProviderContext context)
        {

        }
    }
}
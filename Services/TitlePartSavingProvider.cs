using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.ContentManagement;
using Lombiq.RssReader.Models;

namespace Lombiq.RssReader.Services
{
    public class TitlePartSavingProvider : IRssFeedDataSavingProvider
    {
        public string ProviderType
        {
            get
            {
                return "TitlePart";
            }
        }

        public void Save(IRssFeedDataSavingProviderContext context)
        {

        }
    }
}
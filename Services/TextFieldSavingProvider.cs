using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.ContentManagement;
using Lombiq.FeedAggregator.Models;

namespace Lombiq.FeedAggregator.Services
{
    public class TextFieldSavingProvider : IFeedDataSavingProvider
    {
        public string ProviderType
        {
            get
            {
                return "TextField";
            }
        }

        public void Save(IFeedDataSavingProviderContext context)
        {

        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.ContentManagement;

namespace Lombiq.FeedAggregator.Models
{
    public class FeedDataSavingProviderContext : IFeedDataSavingProviderContext
    {
        public IContent Content { get; set; }

        public string Data { get; set; }

        public string ProviderTypeName { get; set; }
    }
}
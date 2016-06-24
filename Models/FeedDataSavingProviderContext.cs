using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.ContentManagement;
using Lombiq.FeedAggregator.Models.NonPersistent;

namespace Lombiq.FeedAggregator.Models
{
    public class FeedDataSavingProviderContext : IFeedDataSavingProviderContext
    {
        public FeedSyncProfilePart FeedSyncProfilePart { get; set; }

        public string FeedContent { get; set; }

        public Mapping Mapping { get; set; }

        public IContent Content { get; set; }
    }
}
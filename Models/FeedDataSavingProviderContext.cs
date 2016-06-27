using Lombiq.FeedAggregator.Models.NonPersistent;
using Orchard.ContentManagement;
using System.Collections.Generic;

namespace Lombiq.FeedAggregator.Models
{
    public class FeedDataSavingProviderContext : IFeedDataSavingProviderContext
    {
        public FeedSyncProfilePart FeedSyncProfilePart { get; set; }

        public List<string> FeedContent { get; set; }

        public Mapping Mapping { get; set; }

        public IContent Content { get; set; }
    }
}
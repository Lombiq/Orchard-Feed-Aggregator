using Lombiq.FeedAggregator.Models;
using Orchard.ContentManagement.Handlers;
using Orchard.Data;

namespace Lombiq.FeedAggregator.Handlers
{
    public class FeedSyncProfileItemPartHandler : ContentHandler
    {
        public FeedSyncProfileItemPartHandler(IRepository<FeedSyncProfileItemPartRecord> repository)
        {
            Filters.Add(StorageFilter.For(repository));
        }
    }
}
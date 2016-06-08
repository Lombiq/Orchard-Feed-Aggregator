using Lombiq.FeedAggregator.Models;
using Orchard.ContentManagement.Handlers;
using Orchard.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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
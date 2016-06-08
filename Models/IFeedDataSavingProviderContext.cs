using Lombiq.FeedAggregator.Models.NonPersistent;
using Orchard.ContentManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lombiq.FeedAggregator.Models
{
    public interface IFeedDataSavingProviderContext
    {
        FeedSyncProfilePart FeedSyncProfilePart { get; }

        string Data { get; }

        Mapping Mapping { get; }

        IContent Content { get; }
    }
}
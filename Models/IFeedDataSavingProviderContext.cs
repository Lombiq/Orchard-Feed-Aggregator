using Orchard.ContentManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lombiq.FeedAggregator.Models
{
    public interface IFeedDataSavingProviderContext
    {
        IContent Content { get; }

        string Data { get; }

        string ProviderTypeName { get; }
    }
}
using Lombiq.FeedAggregator.Models;
using Orchard;
using Orchard.ContentManagement;
using Orchard.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lombiq.FeedAggregator.Services
{
    public interface IFeedDataSavingProvider : IDependency
    {
        string ProviderType { get; }
        bool Save(IFeedDataSavingProviderContext feedDataSavingProviderContext);
    }
}
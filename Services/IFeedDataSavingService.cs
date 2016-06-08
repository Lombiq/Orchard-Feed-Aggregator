using Orchard;
using Orchard.ContentManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lombiq.FeedAggregator.Services
{
    public interface IFeedDataSavingService : IDependency
    {
        IList<string> GetAccessibleContentItemStorageNames(string contentType);
    }
}
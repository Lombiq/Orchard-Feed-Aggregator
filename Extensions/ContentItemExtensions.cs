using Lombiq.FeedAggregator.Constants;
using Orchard.ContentManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lombiq.FeedAggregator.Extensions
{
    internal static class ContentItemExtensions
    {
        public static string GetFeedSyncProfileUpdaterTaskName(this ContentItem contentItem)
        {
            return TaskTypes.FeedSyncProfileUpdaterPrefix + contentItem.Id.ToString();
        }
    }
}
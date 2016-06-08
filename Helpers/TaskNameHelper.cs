using Lombiq.FeedAggregator.Constants;
using Orchard.ContentManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lombiq.FeedAggregator.Helpers
{
    public static class TaskNameHelper
    {
        public static string GetFeedSyncProfileUpdaterTaskName(ContentItem contentItem)
        {
            return TaskTypes.FeedSyncProfileUpdaterBase + contentItem.Id.ToString();
        }
    }
}
using Lombiq.FeedAggregator.Constants;
using Orchard.ContentManagement;

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
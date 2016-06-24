using Lombiq.FeedAggregator.Models;
using Orchard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lombiq.FeedAggregator.Services
{
    /// <summary>
    /// Service for the feed related operations.
    /// </summary>
    public interface IFeedManager : IDependency
    {
        /// <summary>
        /// Get feed type from a valid feed.
        /// If the feed is invalid then it counts as unsuccesful retrieval.
        /// It also sets the FeedItemIdType once and for all.
        /// </summary>
        /// <param name="feedSyncProfilePart">The FeedSyncProfilePart</param>
        /// <returns>The feed type, or null if the feed is invalid or unsupported.</returns>
        string GetValidFeedType(FeedSyncProfilePart feedSyncProfilePart);

        /// <summary>
        /// Gets the available content item storage names on the type.
        /// </summary>
        /// <param name="contentType">The content type.</param>
        /// <returns>The storage names.</returns>
        IList<string> GetCompatibleContentItemStorageNames(string contentType);
    }
}
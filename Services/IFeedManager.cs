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
        /// Returns true if feed type getting was successful.
        /// If the feed is invalid then it counts as unsuccesful getting.
        /// It also sets the FeedItemIdType once and for all.
        /// </summary>
        /// <param name="feedSyncProfilePart">The FeedSyncProfilePart</param>
        /// <param name="feedType">The type of the feed.</param>
        bool TryGetValidFeedType(FeedSyncProfilePart feedSyncProfilePart, out string feedType);

        /// <summary>
        /// Returns the available content item storage names on the type.
        /// </summary>
        /// <param name="contentType">The content type.</param>
        IList<string> GetAccessibleContentItemStorageNames(string contentType);
    }
}
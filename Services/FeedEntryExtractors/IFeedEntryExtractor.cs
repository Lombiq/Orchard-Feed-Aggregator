using Lombiq.FeedAggregator.Models;
using Orchard;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Lombiq.FeedAggregator.Services.FeedEntryExtractors
{
    /// <summary>
    /// Provider for extracting feed entries from feeds.
    /// </summary>
    public interface IFeedEntryExtractor : IDependency
    {
        /// <summary>
        /// The type of the feed.
        /// </summary>
        string FeedType { get; }

        /// <summary>
        /// The given feedType must be equal to the ProviderType. The provider should run only when they are equal.
        /// It will return only the valid entries.
        /// </summary>
        /// <param name="feedSyncProfilePart">The FeedSyncProfilePart.</param>
        /// <param name="feedType">The type of the feed.</param>
        /// <returns>A list of XmlElements.
        /// In case of type mismatch or any error it should return an empty list.
        /// </returns>
        IList<XElement> GetNewValidEntries(FeedSyncProfilePart feedSyncProfilePart, string feedType);
    }
}
using Lombiq.FeedAggregator.Models;
using Orchard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.Xml.Linq;

namespace Lombiq.FeedAggregator.Services
{
    public interface IFeedEntryExtractorProvider : IDependency
    {
        /// <summary>
        /// The type of the provider.
        /// </summary>
        string ProviderType { get; }

        /// <summary>
        /// The given feedType must be equal with the ProviderType. The provider should run only when they are equal.
        /// It will return only the valid entries.
        /// The list of the entries will begin with the latest not yet created entry.
        /// </summary>
        /// <param name="feedSyncProfilePart">The FeedSyncProfilePart.</param>
        /// <param name="feedType">The type of the feed.</param>
        /// <returns>A list of XmlElements. 
        /// In case of type mismatch or any error it should return null.
        /// </returns>
        IList<XElement> GetNewValidEntries(FeedSyncProfilePart feedSyncProfilePart, string feedType);
    }
}
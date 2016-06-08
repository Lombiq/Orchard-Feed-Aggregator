using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using Lombiq.FeedAggregator.Models;
using System.Xml.Linq;

namespace Lombiq.FeedAggregator.Services
{
    public class AtomEntryExtractorProvider : IFeedEntryExtractorProvider
    {
        public string ProviderType { get { return "Atom"; } }

        public IList<XElement> GetNewValidEntries(FeedSyncProfilePart feedSyncProfilePart, string feedType)
        {
            if (ProviderType != feedType || string.IsNullOrEmpty(feedSyncProfilePart.FeedItemIdType)) return null;

            return new List<XElement>();
        }
    }
}
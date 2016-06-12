using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lombiq.FeedAggregator.Models;
using System.Xml;
using System.Xml.Linq;
using System.IO;
using Orchard.Logging;

namespace Lombiq.FeedAggregator.Services
{
    public class RssItemExtractorProvider : IFeedEntryExtractorProvider
    {
        public string ProviderType { get { return "Rss"; } }
        public ILogger Logger { get; set; }


        public RssItemExtractorProvider()
        {
            Logger = NullLogger.Instance;
        }


        public IList<XElement> GetNewValidEntries(FeedSyncProfilePart feedSyncProfilePart, string feedType)
        {
            if (ProviderType != feedType || string.IsNullOrEmpty(feedSyncProfilePart.FeedItemIdType)) return null;

            var newEntries = new List<XElement>();

            try
            {
                var feedXml = XDocument.Load(feedSyncProfilePart.FeedUrl);
                var channelElement = feedXml.Root.Element("channel");
                if (channelElement == null) return null;
                var feedItems = channelElement.Descendants("item");
                if (feedItems == null) return null;

                foreach (var feedItem in feedItems)
                {
                    // If this is the init and the init count is more than the set one.
                    if (feedSyncProfilePart.LatestCreatedItemDate == default(DateTime) &&
                        newEntries.Count() >= feedSyncProfilePart.NumberOfItemsToSyncDuringInit)
                    {
                        break;
                    }

                    var pubDateElement = feedItem.Element("pubDate");
                    var idElement = feedItem.Element(feedSyncProfilePart.FeedItemIdType);
                    var modificationDate = new DateTime();
                    if (pubDateElement == null ||
                        idElement == null ||
                        !DateTime.TryParse(pubDateElement.Value, out modificationDate))
                    {
                        continue;
                    }

                    if (modificationDate.ToUniversalTime() <= feedSyncProfilePart.LatestCreatedItemDate)
                    {
                        break;
                    }

                    newEntries.Add(feedItem);
                }

                return newEntries;
            }
            catch (Exception ex) when (ex is FileNotFoundException || ex is XmlException || ex is NotSupportedException)
            {
                Logger.Error(ex, "Cannot find or parse the feed with the given url.");
                throw;
            }
        }
    }
}
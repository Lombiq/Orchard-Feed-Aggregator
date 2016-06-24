using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lombiq.FeedAggregator.Models;
using System.Xml;
using System.Xml.Linq;
using System.IO;
using Orchard.Logging;
using Lombiq.FeedAggregator.Helpers;

namespace Lombiq.FeedAggregator.Services.ExtractorProviders
{
    public class RssExtractorProvider : IExtractorProvider
    {
        public string FeedType { get { return "Rss"; } }
        public ILogger Logger { get; set; }


        public RssExtractorProvider()
        {
            Logger = NullLogger.Instance;
        }


        public IList<XElement> GetNewValidEntries(FeedSyncProfilePart feedSyncProfilePart, string feedType)
        {
            var newEntries = new List<XElement>();

            if (FeedType != feedType || string.IsNullOrEmpty(feedSyncProfilePart.FeedItemIdType)) return newEntries;

            try
            {
                var feedXml = XDocument.Load(feedSyncProfilePart.FeedUrl);
                var channelElement = feedXml.Root.GetDescendantNodeByName("channel");
                if (channelElement == null) return newEntries;
                var feedItems = channelElement.Descendants("item");
                if (feedItems == null) return newEntries;

                foreach (var feedItem in feedItems)
                {
                    // If this is the init and the init count is more than the set one.
                    if (!feedSyncProfilePart.SuccesfulInit &&
                        newEntries.Count() >= feedSyncProfilePart.NumberOfItemsToSyncDuringInit)
                    {
                        break;
                    }

                    var pubDateElement = feedItem.GetDescendantNodeByName("pubDate");
                    var idElement = feedItem.GetDescendantNodeByName(feedSyncProfilePart.FeedItemIdType);
                    var modificationDate = new DateTime();
                    if (pubDateElement == null ||
                        idElement == null ||
                        !DateTime.TryParse(pubDateElement.Value, out modificationDate))
                    {
                        continue;
                    }

                    if (modificationDate.ToUniversalTime() <= feedSyncProfilePart.LatestCreatedItemModificationDate)
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
using Lombiq.FeedAggregator.Helpers;
using Lombiq.FeedAggregator.Models;
using Orchard.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Net;

namespace Lombiq.FeedAggregator.Services.FeedEntryExtractors
{
    public class RssFeedEntryExtractor : IFeedEntryExtractor
    {
        public string FeedType { get { return "Rss"; } }
        public ILogger Logger { get; set; }


        public RssFeedEntryExtractor()
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
                        !DateTimeHelper.TryParseDateTime(pubDateElement.Value, out modificationDate))
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
            catch (Exception ex)
            when (ex is FileNotFoundException || ex is XmlException || ex is NotSupportedException || ex is WebException)
            {
                Logger.Error(ex, "Cannot find or parse the feed with the url " + feedSyncProfilePart.FeedUrl + ".");
                throw;
            }
        }
    }
}
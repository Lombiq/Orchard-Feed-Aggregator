using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using Lombiq.FeedAggregator.Models;
using System.Xml.Linq;
using Orchard.Logging;
using System.IO;
using Lombiq.FeedAggregator.Helpers;

namespace Lombiq.FeedAggregator.Services
{
    public class AtomEntryExtractorProvider : IFeedEntryExtractorProvider
    {
        public string ProviderType { get { return "Atom"; } }
        public ILogger Logger { get; set; }


        public AtomEntryExtractorProvider()
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
                var feedEntries = XDocumentHelper.GetDescendantNodesByName(feedXml.Root, "entry");
                var i = 0;
                while (feedEntries != null && i < feedEntries.Count())
                {
                    // If this is the init and the init count is more than the set one.
                    if (!feedSyncProfilePart.SuccesfulInit &&
                        newEntries.Count() >= feedSyncProfilePart.NumberOfItemsToSyncDuringInit)
                    {
                        break;
                    }

                    var updatedElement = XDocumentHelper.GetDescendantNodeByName(feedEntries.ElementAt(i), "updated");
                    var idElement = XDocumentHelper.GetDescendantNodeByName(
                        feedEntries.ElementAt(i),
                        feedSyncProfilePart.FeedItemIdType);
                    var modificationDate = new DateTime();
                    if (updatedElement == null ||
                        idElement == null ||
                        !DateTime.TryParse(updatedElement.Value, out modificationDate))
                    {
                        i++;
                        continue;
                    }

                    if (modificationDate.ToUniversalTime() <= feedSyncProfilePart.LatestCreatedItemDate)
                    {
                        break;
                    }

                    newEntries.Add(feedEntries.ElementAt(i));

                    i++;

                    // If this entry is the last one in this batch, then getting the next batch.
                    if (feedEntries.Count() == i)
                    {
                        // Getting the "next" element, it contains a link to the next feed page.
                        var nextAtomElement = feedXml
                            .Root
                            .Elements()
                            .FirstOrDefault(element =>
                            {
                                if (element.Name.LocalName != "link") return false;
                                var relAttribute = element.Attribute("rel");
                                if (relAttribute == null) return false;
                                return relAttribute.Value == "next";
                            });

                        if (nextAtomElement == null) break;
                        feedXml = XDocument.Load(nextAtomElement.Value);
                        feedEntries = XDocumentHelper.GetDescendantNodesByName(feedXml.Root, "entry");
                        i = 0;
                    }
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
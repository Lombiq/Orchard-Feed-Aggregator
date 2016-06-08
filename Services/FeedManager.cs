using Lombiq.FeedAggregator.Constants;
using Lombiq.FeedAggregator.Models;
using Orchard.ContentManagement.MetaData;
using Orchard.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml;
using System.Xml.Linq;

namespace Lombiq.FeedAggregator.Services
{
    public class FeedManager : IFeedManager
    {
        private readonly IEnumerable<IFeedDataSavingProvider> _providers;
        private readonly IContentDefinitionManager _contentDefinitionManager;

        public ILogger Logger { get; set; }


        public FeedManager(
            IEnumerable<IFeedDataSavingProvider> providers,
            IContentDefinitionManager contentDefinitionManager)
        {
            _providers = providers;
            _contentDefinitionManager = contentDefinitionManager;

            Logger = NullLogger.Instance;
        }


        public bool TryGetValidFeedType(FeedSyncProfilePart feedSyncProfilePart, out string feedType)
        {
            feedType = "";

            try
            {
                var feedXml = XDocument.Load(feedSyncProfilePart.FeedUrl);

                // Checking if the feed is a valid RSS.
                if (feedXml.Root.Name.LocalName == "rss")
                {
                    var channelElement = feedXml.Root.Element("channel");
                    if (channelElement == null)
                    {
                        Logger.Error("Feed is invalid. No channel element.");
                        return false;
                    }

                    var firstItemElement = channelElement.Element("item");
                    if (firstItemElement == null)
                    {
                        Logger.Error("Feed is invalid. No item element.");
                        return false;
                    }

                    var pubDateElement = firstItemElement.Element("pubDate");
                    if (pubDateElement == null)
                    {
                        Logger.Error("Feed is invalid. No pubDate element.");
                        return false;
                    }

                    if (!firstItemElement
                        .Elements()
                        .Any(element =>
                            element.Name.LocalName == "guid" ||
                            element.Name.LocalName == "title" ||
                            element.Name.LocalName == "description"))
                    {
                        Logger.Error("Feed is invalid. No guid, title or description element.");
                        return false;
                    }
                    // Since the feed is valid, set the id type.
                    else if (string.IsNullOrEmpty(feedSyncProfilePart.FeedItemIdType))
                    {
                        if (ElementContainsNodeWithName(firstItemElement, "guid"))
                            feedSyncProfilePart.FeedItemIdType = "guid";
                        else if (ElementContainsNodeWithName(firstItemElement, "title"))
                            feedSyncProfilePart.FeedItemIdType = "title";
                        else if (ElementContainsNodeWithName(firstItemElement, "description"))
                            feedSyncProfilePart.FeedItemIdType = "description";
                    }

                    feedType = "Rss";
                    if (string.IsNullOrEmpty(feedSyncProfilePart.FeedItemModificationDateType)) feedSyncProfilePart.FeedItemModificationDateType = "pubDate";
                    return true;
                }

                // Checking if the feed is a valid Atom.
                if (feedXml.Root.Name.LocalName == "feed")
                {
                    var xmlnsAttribute = feedXml.Root.Attribute("xmlns");
                    if (xmlnsAttribute != null && xmlnsAttribute.Value == "http://www.w3.org/2005/Atom")
                    {
                        var firstEntryElement = feedXml
                            .Root
                            .Elements()
                            .FirstOrDefault(element => element.Name.LocalName == "entry");
                        if (firstEntryElement == null)
                        {
                            Logger.Error("Feed is invalid. No entry element.");
                            return false;
                        }

                        var updatedElement = firstEntryElement.Element("updated");
                        if (firstEntryElement == null)
                        {
                            Logger.Error("Feed is invalid. No updated element.");
                            return false;
                        }

                        var idElement = firstEntryElement.Element("id");
                        if (firstEntryElement == null)
                        {
                            Logger.Error("Feed is invalid. No id element.");
                            return false;
                        }
                        // Since the feed is valid, set the id type.
                        else if (string.IsNullOrEmpty(feedSyncProfilePart.FeedItemIdType))
                        {
                            feedSyncProfilePart.FeedItemIdType = "id";
                        }

                        feedType = "Atom";
                        if (string.IsNullOrEmpty(feedSyncProfilePart.FeedItemModificationDateType)) feedSyncProfilePart.FeedItemModificationDateType = "updated";
                        return true;
                    }
                }

            }
            catch (Exception ex) when (ex is FileNotFoundException || ex is XmlException || ex is NotSupportedException)
            {
                Logger.Error(ex, "Cannot find or parse the feed with the given url.");
                throw;
            }

            Logger.Error("Cannot get the feed type, so it's unsupported.");
            return false;
        }

        public IList<string> GetAccessibleContentItemStorageNames(string contentType)
        {
            var accessibleDataStorageNames = new List<string>();
            var typeDefinition = _contentDefinitionManager.GetTypeDefinition(contentType);

            if (typeDefinition == null) return accessibleDataStorageNames;

            foreach (var provider in _providers)
            {
                var partOnContentItem = typeDefinition
                    .Parts
                    .FirstOrDefault(part => part.PartDefinition.Name == provider.ProviderType);

                if (partOnContentItem != null)
                {
                    accessibleDataStorageNames.Add(provider.ProviderType);
                }
                else
                {
                    foreach (var part in typeDefinition.Parts)
                    {
                        foreach (var field in part.PartDefinition.Fields)
                        {
                            if (field.FieldDefinition.Name == provider.ProviderType)
                            {
                                accessibleDataStorageNames.Add(part.PartDefinition.Name + "." + field.Name);
                            }
                        }
                    }
                }
            }

            return accessibleDataStorageNames;
        }


        private bool ElementContainsNodeWithName(XElement xElement, string name)
        {
            return xElement.Elements().Any(element => element.Name.LocalName == name);
        }
    }
}
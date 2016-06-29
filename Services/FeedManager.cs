using Lombiq.FeedAggregator.Models;
using Lombiq.FeedAggregator.Services.FeedDataSavingProviders;
using Orchard.ContentManagement.MetaData;
using Orchard.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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


        public string GetValidFeedType(FeedSyncProfilePart feedSyncProfilePart)
        {
            try
            {
                var feedXml = XDocument.Load(feedSyncProfilePart.FeedUrl);

                // Checking if the feed is valid RSS.
                if (feedXml.Root.Name.LocalName == "rss")
                {
                    var channelElement = feedXml.Root.GetDescendantNodeByName("channel");
                    if (channelElement == null)
                    {
                        Logger.Error("Feed is invalid. No channel element.");
                        return null;
                    }

                    var firstItemElement = channelElement.GetDescendantNodeByName("item");
                    if (firstItemElement == null)
                    {
                        Logger.Error("Feed is invalid. No item element.");
                        return null;
                    }

                    var pubDateElement = firstItemElement.GetDescendantNodeByName("pubDate");
                    if (pubDateElement == null)
                    {
                        Logger.Error("Feed is invalid. No pubDate element.");
                        return null;
                    }

                    if (!firstItemElement
                        .Elements()
                        .Any(element =>
                            element.Name.LocalName == "guid" ||
                            element.Name.LocalName == "title" ||
                            element.Name.LocalName == "description"))
                    {
                        Logger.Error("Feed is invalid. No guid, title or description element.");
                        return null;
                    }
                    // Since the feed is valid, set the id type.
                    else if (string.IsNullOrEmpty(feedSyncProfilePart.FeedItemIdType))
                    {
                        if (firstItemElement.ElementContainsNodeWithName("guid"))
                            feedSyncProfilePart.FeedItemIdType = "guid";
                        else if (firstItemElement.ElementContainsNodeWithName("title"))
                            feedSyncProfilePart.FeedItemIdType = "title";
                        else if (firstItemElement.ElementContainsNodeWithName("description"))
                            feedSyncProfilePart.FeedItemIdType = "description";
                    }

                    if (string.IsNullOrEmpty(feedSyncProfilePart.FeedItemModificationDateType))
                    {
                        feedSyncProfilePart.FeedItemModificationDateType = "pubDate";
                    }

                    return "Rss";
                }

                // Checking if the feed is valid Atom.
                if (feedXml.Root.Name.LocalName == "feed")
                {
                    var xmlnsAttribute = feedXml.Root.Attribute("xmlns");
                    if (xmlnsAttribute != null && xmlnsAttribute.Value == "http://www.w3.org/2005/Atom")
                    {
                        var firstEntryElement = feedXml.Root.GetDescendantNodeByName("entry");
                        if (firstEntryElement == null)
                        {
                            Logger.Error("Feed is invalid. No entry element.");
                            return null;
                        }

                        var updatedElement = firstEntryElement.GetDescendantNodeByName("updated");
                        if (updatedElement == null)
                        {
                            Logger.Error("Feed is invalid. No updated element.");
                            return null;
                        }

                        var idElement = firstEntryElement.GetDescendantNodeByName("id");
                        if (idElement == null)
                        {
                            Logger.Error("Feed is invalid. No id element.");
                            return null;
                        }
                        // Since the feed is valid, set the id type.
                        else if (string.IsNullOrEmpty(feedSyncProfilePart.FeedItemIdType))
                        {
                            feedSyncProfilePart.FeedItemIdType = "id";
                        }

                        if (string.IsNullOrEmpty(feedSyncProfilePart.FeedItemModificationDateType))
                        {
                            feedSyncProfilePart.FeedItemModificationDateType = "updated";
                        }

                        return "Atom";
                    }
                }
            }
            catch (Exception ex) when (ex is FileNotFoundException || ex is XmlException || ex is NotSupportedException)
            {
                Logger.Error(ex, "Cannot find or parse the feed with the given url.");
                return null;
            }

            Logger.Error("Cannot get the feed type, so it's unsupported.");
            return null;
        }

        public IList<string> GetCompatibleContentItemStorageNames(string contentType)
        {
            var compatibleDataStorageNames = new List<string>();
            var typeDefinition = _contentDefinitionManager.GetTypeDefinition(contentType);

            if (typeDefinition == null) return compatibleDataStorageNames;

            foreach (var provider in _providers)
            {
                var splitProviderType = provider.ProviderType.Split('.');
                var partName = splitProviderType.Length == 1 ? provider.ProviderType : splitProviderType[0];
                var partOnContentItem = typeDefinition
                    .Parts
                    .FirstOrDefault(part => part.PartDefinition.Name == partName);

                // If such a part exists on the type then it's a part provider.
                if (partOnContentItem != null)
                {
                    compatibleDataStorageNames.Add(provider.ProviderType);
                }
                // If no such part exists on the type then it's a field provider.
                else
                {
                    foreach (var part in typeDefinition.Parts)
                    {
                        foreach (var field in part.PartDefinition.Fields)
                        {
                            if (field.FieldDefinition.Name == provider.ProviderType)
                            {
                                compatibleDataStorageNames.Add(part.PartDefinition.Name + "." + field.Name);
                            }
                        }
                    }
                }
            }

            return compatibleDataStorageNames;
        }
    }
}
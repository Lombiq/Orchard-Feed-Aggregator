using Lombiq.RssReader.Constants;
using Lombiq.RssReader.Models;
using Orchard.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml;
using System.Xml.Linq;

namespace Lombiq.RssReader.Services
{
    public class FeedManager : IFeedManager
    {
        public ILogger Logger { get; set; }


        public FeedManager()
        {
            Logger = NullLogger.Instance;
        }


        public bool TryGetValidFeedType(RssSyncProfilePart rssSyncProfilePart, out string feedType)
        {
            feedType = "";

            try
            {
                var feedXml = XDocument.Load(rssSyncProfilePart.RssFeedUrl);

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

                    feedType = "Rss";
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

                        feedType = "Atom";
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
    }
}
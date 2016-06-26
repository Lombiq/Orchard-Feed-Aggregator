using Lombiq.FeedAggregator.Constants;
using Lombiq.FeedAggregator.Extensions;
using Lombiq.FeedAggregator.Models;
using Lombiq.FeedAggregator.Services.FeedDataSavingProviders;
using Lombiq.FeedAggregator.Services.FeedEntryExtractors;
using Orchard.ContentManagement;
using Orchard.Core.Common.Models;
using Orchard.Core.Title.Models;
using Orchard.Environment;
using Orchard.Logging;
using Orchard.Services;
using Orchard.Tasks.Scheduling;
using Piedone.HelpfulLibraries.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Lombiq.FeedAggregator.Services
{
    public class FeedSyncProfileUpdaterScheduledTask : IScheduledTaskHandler, IOrchardShellEvents
    {
        private readonly IScheduledTaskManager _scheduledTaskManager;
        private readonly IClock _clock;
        private readonly IContentManager _contentManager;
        private readonly IFeedManager _feedManager;
        private readonly IEnumerable<IFeedEntryExtractor> _feedEntryExtractors;
        private readonly IEnumerable<IFeedDataSavingProvider> _feedDataSavingProviders;

        public ILogger Logger { get; set; }


        public FeedSyncProfileUpdaterScheduledTask(
            IScheduledTaskManager scheduledTaskManager,
            IClock clock,
            IContentManager contentManager,
            IFeedManager feedManager,
            IEnumerable<IFeedEntryExtractor> feedEntryExtractors,
            IEnumerable<IFeedDataSavingProvider> feedDataSavingProviders)
        {
            _scheduledTaskManager = scheduledTaskManager;
            _clock = clock;
            _contentManager = contentManager;
            _feedManager = feedManager;
            _feedEntryExtractors = feedEntryExtractors;
            _feedDataSavingProviders = feedDataSavingProviders;

            Logger = NullLogger.Instance;
        }


        public void Process(ScheduledTaskContext context)
        {
            if (!context.Task.TaskType.StartsWith(TaskTypes.FeedSyncProfileUpdaterPrefix)) return;

            Renew(true, context.Task.ContentItem);

            var feedSyncProfileContentItem = context.Task.ContentItem;
            if (feedSyncProfileContentItem == null ||
                feedSyncProfileContentItem.ContentType != ContentTypes.FeedSyncProfile) return;

            var feedSyncProfilePart = feedSyncProfileContentItem.As<FeedSyncProfilePart>();

            var feedType = _feedManager.GetValidFeedType(feedSyncProfilePart);
            if (string.IsNullOrEmpty(feedType)) return;

            var newEntries = new List<XElement>();
            foreach (var feedEntryExtractorProvider in _feedEntryExtractors)
            {
                var extractedEntries = feedEntryExtractorProvider
                    .GetNewValidEntries(feedSyncProfilePart, feedType);

                newEntries.AddRange(extractedEntries);
            }

            foreach (var newEntry in newEntries)
            {
                // Persisting must be happen only if at least one successful mapping saving happened.
                var contentItemShouldBePersisted = false;

                var feedItemIdNode = newEntry.GetDescendantNodeByName(feedSyncProfilePart.FeedItemIdType);
                if (feedItemIdNode == null) continue;
                var feedItemId = feedItemIdNode.Value;
                var feedItemModificationDateNode = newEntry.GetDescendantNodeByName(feedSyncProfilePart.FeedItemModificationDateType);
                var feedItemModificationDate = new DateTime();
                if (feedItemModificationDateNode == null ||
                    !DateTime.TryParse(feedItemModificationDateNode.Value, out feedItemModificationDate)) continue;

                var feedSyncProfileItem = _contentManager
                        .Query(feedSyncProfilePart.ContentType)
                        .Where<FeedSyncProfileItemPartRecord>(record => record.FeedItemId == feedItemId)
                        .Slice(1)
                        .FirstOrDefault();
                if (feedSyncProfileItem == null)
                {
                    feedSyncProfileItem = _contentManager.New(feedSyncProfilePart.ContentType);
                    _contentManager.Create(feedSyncProfileItem, VersionOptions.Draft);
                }

                foreach (var mapping in feedSyncProfilePart.Mappings)
                {
                    var feedEntryData = "";
                    var splitFeedMapping = mapping.FeedMapping.Split('.');
                    XElement selectedNodeInFeedEntry;
                    if (!newEntry.TryGetNodeByPath(splitFeedMapping[0], out selectedNodeInFeedEntry))
                        continue;
                    // If it is an attribute mapping.
                    if (splitFeedMapping.Length > 1)
                    {
                        var feedEntryAttribute = selectedNodeInFeedEntry.Attribute(splitFeedMapping[1]);
                        if (feedEntryAttribute == null) continue;

                        feedEntryData = feedEntryAttribute.Value;
                    }
                    // If it is node mapping,
                    else
                    {
                        // This is necessary, because sometimes the node contains another node as the actual content.
                        // E.g. img element.
                        feedEntryData = string.IsNullOrEmpty(selectedNodeInFeedEntry.Value)
                            ? string.Join("", selectedNodeInFeedEntry.Nodes().Select(x => x.ToString()).ToArray())
                            : selectedNodeInFeedEntry.Value;
                    }

                    var successfulMappingSaving = false;
                    foreach (var feedDataSavingProvider in _feedDataSavingProviders)
                    {
                        successfulMappingSaving = feedDataSavingProvider.Save(new FeedDataSavingProviderContext
                        {
                            FeedSyncProfilePart = feedSyncProfilePart,
                            FeedContent = feedEntryData,
                            Mapping = mapping,
                            Content = feedSyncProfileItem
                        });

                        // In case of a provider saved the data continue with the next mapping.
                        if (successfulMappingSaving)
                        {
                            contentItemShouldBePersisted = true;
                            break;
                        }
                    }

                    if (!successfulMappingSaving)
                    {
                        var feedSyncProfileTitlePart = feedSyncProfileContentItem.As<TitlePart>();
                        Logger.Error(string.Format(
                            "No suitable provider for the selected mapping ({0} to {1}). This can be due to a content type definion change. Please edit and save the corresponding (Title: {2}) FeedSyncProfile.",
                            mapping.FeedMapping,
                            mapping.ContentItemStorageMapping,
                            feedSyncProfileTitlePart == null ? "unknown" : feedSyncProfileTitlePart.Title));
                    }
                }

                if (contentItemShouldBePersisted)
                {
                    // We publish it only if at least one mapping saving was successful.
                    // Also this is the time when we want to store the last creation date
                    // on the FeedSyncProfilePart and set the FeedItemId if it not set yet.
                    var feedSyncProfileItemPart = feedSyncProfileItem.As<FeedSyncProfileItemPart>();
                    if (string.IsNullOrEmpty(feedSyncProfileItemPart.FeedItemId)) feedSyncProfileItemPart.FeedItemId = feedItemId;
                    feedSyncProfilePart.LatestCreatedItemModificationDate = feedItemModificationDate;
                    // Setting the content item's container.
                    var container = feedSyncProfilePart.Container.ContentItems.Any()
                        ? feedSyncProfilePart.Container.ContentItems.First()
                        : null;
                    if (container != null)
                    {
                        var commonPart = feedSyncProfileItem.As<CommonPart>();
                        if (commonPart != null)
                        {
                            commonPart.Container = container;
                        }
                    }

                    _contentManager.Publish(feedSyncProfileItem);
                }
                else
                {
                    _contentManager.Remove(feedSyncProfileItem);
                }
            }

            feedSyncProfilePart.SuccesfulInit = true;
            feedSyncProfilePart.LatestCreatedItemModificationDate = _clock.UtcNow;
        }

        public void Activated()
        {
            // Renewing all tasks.
            foreach (var contentItem in _contentManager.Query(ContentTypes.FeedSyncProfile).List())
            {
                Renew(false, contentItem);
            }
        }

        public void Terminating()
        {
        }


        private void Renew(bool calledFromTaskProcess, ContentItem contentItem)
        {
            _scheduledTaskManager
                .CreateTaskIfNew(
                    contentItem.GetFeedSyncProfileUpdaterTaskName(),
                    _clock.UtcNow.AddMinutes(Convert.ToDouble(contentItem.As<FeedSyncProfilePart>().MinutesBetweenSyncs)),
                    contentItem,
                    calledFromTaskProcess);
        }
    }
}
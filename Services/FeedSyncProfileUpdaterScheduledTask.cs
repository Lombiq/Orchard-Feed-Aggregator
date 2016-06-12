using Orchard.Environment;
using Orchard.Services;
using Orchard.Tasks.Scheduling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Piedone.HelpfulLibraries.Tasks;
using Orchard.ContentManagement;
using Lombiq.FeedAggregator.Constants;
using Lombiq.FeedAggregator.Models;
using Lombiq.FeedAggregator.Helpers;
using System.Xml;
using System.Xml.Linq;
using Orchard.Logging;
using Orchard.Core.Title.Models;
using Orchard.Core.Common.Models;

namespace Lombiq.FeedAggregator.Services
{
    public class FeedSyncProfileUpdaterScheduledTask : IScheduledTaskHandler, IOrchardShellEvents
    {
        private readonly IScheduledTaskManager _scheduledTaskManager;
        private readonly IClock _clock;
        private readonly IContentManager _contentManager;
        private readonly IFeedManager _feedManager;
        private readonly IEnumerable<IFeedEntryExtractorProvider> _feedEntryExtractorProviders;
        private readonly IEnumerable<IFeedDataSavingProvider> _feedDataSavingProviders;

        public ILogger Logger { get; set; }


        public FeedSyncProfileUpdaterScheduledTask(
            IScheduledTaskManager scheduledTaskManager,
            IClock clock,
            IContentManager contentManager,
            IFeedManager feedManager,
            IEnumerable<IFeedEntryExtractorProvider> feedEntryExtractorProviders,
            IEnumerable<IFeedDataSavingProvider> feedDataSavingProviders)
        {
            _scheduledTaskManager = scheduledTaskManager;
            _clock = clock;
            _contentManager = contentManager;
            _feedManager = feedManager;
            _feedEntryExtractorProviders = feedEntryExtractorProviders;
            _feedDataSavingProviders = feedDataSavingProviders;

            Logger = NullLogger.Instance;
        }


        public void Process(ScheduledTaskContext context)
        {
            if (!context.Task.TaskType.StartsWith(TaskTypes.FeedSyncProfileUpdaterBase)) return;

            Renew(true, context.Task.ContentItem);

            var feedSyncProfileContentItem = context.Task.ContentItem;
            if (feedSyncProfileContentItem == null ||
                feedSyncProfileContentItem.ContentType != ContentTypes.FeedSyncProfile) return;

            var feedSyncProfilePart = feedSyncProfileContentItem.As<FeedSyncProfilePart>();

            var feedType = "";
            if (!_feedManager.TryGetValidFeedType(feedSyncProfilePart, out feedType)) return;

            var newEntries = new List<XElement>();
            foreach (var feedEntryExtractorProvider in _feedEntryExtractorProviders)
            {
                var extractedEntries = feedEntryExtractorProvider
                    .GetNewValidEntries(feedSyncProfilePart, feedType);
                if (extractedEntries != null) newEntries.AddRange(extractedEntries);
            }

            // The latest is at the beginning of the list, so the list must be reversed.
            newEntries.Reverse();
            foreach (var newEntry in newEntries)
            {
                // Persisting must be happen only if at least one successful mapping saving happened.
                var contentItemShouldBePersisted = false;

                var feedItemIdNode = XDocumentHelper.GetDescendantNodeByName(newEntry, feedSyncProfilePart.FeedItemIdType);
                if (feedItemIdNode == null) continue;
                var feedItemId = feedItemIdNode.Value;
                var feedItemModificationDateNode = XDocumentHelper.GetDescendantNodeByName(newEntry, feedSyncProfilePart.FeedItemModificationDateType);
                var feedItemModificationDate = new DateTime();
                if (feedItemModificationDateNode == null ||
                    !DateTime.TryParse(feedItemModificationDateNode.Value, out feedItemModificationDate)) continue;

                var feedSyncProfileItem = _contentManager
                        .Query(feedSyncProfilePart.ContentType)
                        .Where<FeedSyncProfileItemPartRecord>(record => record.FeedItemId == feedItemId)
                        .List()
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
                    // If it is an attribute mapping.
                    if (splitFeedMapping.Length > 1)
                    {
                        var feedEntryNode = XDocumentHelper.GetDescendantNodeByName(newEntry, splitFeedMapping[0]);
                        if (feedEntryNode == null) continue;

                        var feedEntryAttribute = feedEntryNode.Attribute(splitFeedMapping[1]);
                        if (feedEntryAttribute == null) continue;

                        feedEntryData = feedEntryAttribute.Value;
                    }
                    // If it is node mapping,
                    else
                    {
                        var feedEntryNode = XDocumentHelper.GetDescendantNodeByName(newEntry, mapping.FeedMapping);
                        if (feedEntryNode == null) continue;

                        feedEntryData = feedEntryNode.Value;
                    }

                    var successfulMappingSaving = false;
                    foreach (var feedDataSavingProvider in _feedDataSavingProviders)
                    {
                        successfulMappingSaving = feedDataSavingProvider.Save(new FeedDataSavingProviderContext
                        {
                            FeedSyncProfilePart = feedSyncProfilePart,
                            Data = feedEntryData,
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
                    feedSyncProfilePart.LatestCreatedItemDate = feedItemModificationDate;
                    // Setting the content item's container.
                    var container = feedSyncProfilePart.Container.ContentItems.ElementAt(0);
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
            feedSyncProfilePart.LatestCreatedItemDate = _clock.UtcNow;
        }

        public void Activated()
        {
            // Renewing all tasks.
            foreach (var contentItem in _contentManager.Query(ContentTypes.FeedSyncProfile).List())
            {
                Renew(false, contentItem);
            }
        }

        public void Terminating() { }


        private void Renew(bool calledFromTaskProcess, ContentItem contentItem)
        {
            _scheduledTaskManager
                .CreateTaskIfNew(
                    TaskNameHelper.GetFeedSyncProfileUpdaterTaskName(contentItem),
                    _clock.UtcNow.AddMinutes(Convert.ToDouble(contentItem.As<FeedSyncProfilePart>().MinutesBetweenSyncs)),
                    contentItem,
                    calledFromTaskProcess);
        }
    }
}
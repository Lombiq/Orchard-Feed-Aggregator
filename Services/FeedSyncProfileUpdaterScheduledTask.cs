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

namespace Lombiq.FeedAggregator.Services
{
    public class FeedSyncProfileUpdaterScheduledTask : IScheduledTaskHandler, IOrchardShellEvents
    {
        private readonly IScheduledTaskManager _scheduledTaskManager;
        private readonly IClock _clock;
        private readonly IContentManager _contentManager;
        private readonly IFeedManager _feedManager;


        public FeedSyncProfileUpdaterScheduledTask(
            IScheduledTaskManager scheduledTaskManager,
            IClock clock,
            IContentManager contentManager,
            IFeedManager feedManager)
        {
            _scheduledTaskManager = scheduledTaskManager;
            _clock = clock;
            _contentManager = contentManager;
            _feedManager = feedManager;
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

            // If the init happened, then check for the new entires.
            if (feedSyncProfilePart.SuccesfulInit)
            {

            }
            // If the init didn't happened yet, then do the init.
            else
            {

                //feedSyncProfilePart.SuccesfulInit = true;
            }
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
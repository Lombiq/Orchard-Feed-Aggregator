using Orchard.Environment;
using Orchard.Services;
using Orchard.Tasks.Scheduling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Piedone.HelpfulLibraries.Tasks;
using Orchard.ContentManagement;
using Lombiq.RssReader.Constants;
using Lombiq.RssReader.Models;
using Lombiq.RssReader.Helpers;

namespace Lombiq.RssReader.Services
{
    public class RssSyncProfileUpdaterScheduledTask : IScheduledTaskHandler, IOrchardShellEvents
    {
        private readonly IScheduledTaskManager _scheduledTaskManager;
        private readonly IClock _clock;
        private readonly IContentManager _contentManager;


        public RssSyncProfileUpdaterScheduledTask(
            IScheduledTaskManager scheduledTaskManager,
            IClock clock,
            IContentManager contentManager)
        {
            _scheduledTaskManager = scheduledTaskManager;
            _clock = clock;
            _contentManager = contentManager;
        }


        public void Process(ScheduledTaskContext context)
        {
            if (!context.Task.TaskType.StartsWith(TaskTypes.RssSyncProfileUpdaterBase)) return;

            Renew(true, context.Task.ContentItem);

            var rssSyncProfileContentItem = context.Task.ContentItem;
            if (rssSyncProfileContentItem == null ||
                rssSyncProfileContentItem.ContentType != ContentTypes.RssSyncProfile) return;

            var rssSyncProfilePart = rssSyncProfileContentItem.As<RssSyncProfilePart>();

            // If the init happened, then check for the new entires.
            if (rssSyncProfilePart.SuccesfulInit)
            {

            }
            // If the init didn't happened yet, then do the init.
            else
            {

            }
        }

        public void Activated()
        {
            // Renewing all tasks.
            foreach (var contentItem in _contentManager.Query(ContentTypes.RssSyncProfile).List())
            {
                Renew(false, contentItem);
            }
        }

        public void Terminating() { }


        private void Renew(bool calledFromTaskProcess, ContentItem contentItem)
        {
            _scheduledTaskManager
                .CreateTaskIfNew(
                    TaskNameHelper.GetRssSyncProfileUpdaterTaskName(contentItem),
                    _clock.UtcNow.AddMinutes(Convert.ToDouble(contentItem.As<RssSyncProfilePart>().MinutesBetweenSyncs)),
                    contentItem,
                    calledFromTaskProcess);
        }
    }
}
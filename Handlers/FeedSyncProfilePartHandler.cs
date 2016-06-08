using Lombiq.FeedAggregator.Helpers;
using Lombiq.FeedAggregator.Models;
using Lombiq.FeedAggregator.Models.NonPersistent;
using Orchard.ContentManagement.Handlers;
using Orchard.Services;
using Orchard.Tasks.Scheduling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lombiq.FeedAggregator.Handlers
{
    public class FeedSyncProfilePartHandler : ContentHandler
    {
        public FeedSyncProfilePartHandler(
            IJsonConverter jsonConverter,
            IScheduledTaskManager scheduledTaskManager,
            IClock clock
            )
        {
            OnActivated<FeedSyncProfilePart>((context, part) =>
            {
                part.MappingsField.Loader(() =>
                {
                    return string.IsNullOrEmpty(part.MappingsSerialized)
                        ? new List<Mapping>()
                        : jsonConverter.Deserialize<List<Mapping>>(part.MappingsSerialized);
                });
            });

            OnRemoved<FeedSyncProfilePart>((context, part) =>
            {
                scheduledTaskManager.DeleteTasks(part.ContentItem);
            });

            OnPublished<FeedSyncProfilePart>((context, part) =>
            {
                if (context.PreviousItemVersionRecord != null)
                {
                    scheduledTaskManager.DeleteTasks(part.ContentItem);

                    // Because of the ContentType-first editor, we want to create the task only after
                    // the second successful saving.
                    scheduledTaskManager
                        .CreateTask(
                            TaskNameHelper.GetFeedSyncProfileUpdaterTaskName(part.ContentItem),
                            clock.UtcNow.AddMinutes(1),
                            part.ContentItem);
                }
            });
        }
    }
}
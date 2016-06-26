using Lombiq.FeedAggregator.Extensions;
using Lombiq.FeedAggregator.Models;
using Lombiq.FeedAggregator.Models.NonPersistent;
using Orchard.ContentManagement.Handlers;
using Orchard.Services;
using Orchard.Tasks.Scheduling;
using System.Collections.Generic;

namespace Lombiq.FeedAggregator.Handlers
{
    public class FeedSyncProfilePartHandler : ContentHandler
    {
        public FeedSyncProfilePartHandler(
            IJsonConverter jsonConverter,
            IScheduledTaskManager scheduledTaskManager,
            IClock clock)
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
                part.PublishedCount++;

                if (context.PreviousItemVersionRecord != null)
                {
                    scheduledTaskManager.DeleteTasks(part.ContentItem);

                    // Because of the ContentType-first editor, we want to create the task only after
                    // the second successful saving.
                    scheduledTaskManager
                        .CreateTask(
                            part.ContentItem.GetFeedSyncProfileUpdaterTaskName(),
                            clock.UtcNow.AddMinutes(1),
                            part.ContentItem);
                }
            });
        }
    }
}
using Lombiq.RssReader.Helpers;
using Lombiq.RssReader.Models;
using Lombiq.RssReader.Models.NonPersistent;
using Orchard.ContentManagement.Handlers;
using Orchard.Services;
using Orchard.Tasks.Scheduling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lombiq.RssReader.Handlers
{
    public class RssSyncProfilePartHandler : ContentHandler
    {
        public RssSyncProfilePartHandler(
            IJsonConverter jsonConverter,
            IScheduledTaskManager scheduledTaskManager,
            IClock clock
            )
        {
            OnActivated<RssSyncProfilePart>((context, part) =>
            {
                part.MappingsField.Loader(() =>
                {
                    return string.IsNullOrEmpty(part.MappingsSerialized)
                        ? new List<Mapping>()
                        : jsonConverter.Deserialize<List<Mapping>>(part.MappingsSerialized);
                });
            });

            OnRemoved<RssSyncProfilePart>((context, part) =>
            {
                scheduledTaskManager.DeleteTasks(part.ContentItem);
            });

            OnPublished<RssSyncProfilePart>((context, part) =>
            {
                if (context.PreviousItemVersionRecord != null)
                {
                    scheduledTaskManager.DeleteTasks(part.ContentItem);

                    // Because of the ContentType-first editor, we want to create the task only after
                    // the second successful saving.
                    scheduledTaskManager
                        .CreateTask(
                            TaskNameHelper.GetRssSyncProfileUpdaterTaskName(part.ContentItem),
                            clock.UtcNow.AddMinutes(1),
                            part.ContentItem);
                }
            });
        }
    }
}
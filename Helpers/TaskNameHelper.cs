using Lombiq.RssReader.Constants;
using Orchard.ContentManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lombiq.RssReader.Helpers
{
    public static class TaskNameHelper
    {
        public static string GetRssSyncProfileUpdaterTaskName(ContentItem contentItem)
        {
            return TaskTypes.RssSyncProfileUpdaterBase + contentItem.Id.ToString();
        }
    }
}
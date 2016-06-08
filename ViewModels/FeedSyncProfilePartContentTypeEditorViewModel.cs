using Lombiq.FeedAggregator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Lombiq.FeedAggregator.ViewModels
{
    public class FeedSyncProfilePartContentTypeEditorViewModel
    {
        public FeedSyncProfilePart FeedSyncProfilePart { get; set; }
        public IEnumerable<SelectListItem> AccessibleContentTypes { get; set; }
    }
}
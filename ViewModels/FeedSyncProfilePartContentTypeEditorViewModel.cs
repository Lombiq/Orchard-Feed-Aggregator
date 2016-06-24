using Lombiq.FeedAggregator.Models;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Lombiq.FeedAggregator.ViewModels
{
    public class FeedSyncProfilePartContentTypeEditorViewModel
    {
        public FeedSyncProfilePart FeedSyncProfilePart { get; set; }
        public IEnumerable<SelectListItem> CompatibleContentTypes { get; set; }
    }
}
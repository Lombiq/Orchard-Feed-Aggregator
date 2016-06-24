using System.Collections.Generic;
using System.Web.Mvc;

namespace Lombiq.FeedAggregator.ViewModels
{
    public class MappingViewModel
    {
        public IEnumerable<SelectListItem> ContentItemStorageMappingSelectList { get; set; }
        public string FeedMapping { get; set; }
    }
}
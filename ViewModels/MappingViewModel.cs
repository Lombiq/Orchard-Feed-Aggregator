using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Lombiq.FeedAggregator.ViewModels
{
    public class MappingViewModel
    {
        public IEnumerable<SelectListItem> ContentItemStorageMappingSelectList { get; set; }
        public string FeedMapping { get; set; }
    }
}
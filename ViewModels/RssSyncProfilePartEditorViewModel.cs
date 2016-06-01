using Lombiq.RssReader.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Lombiq.RssReader.ViewModels
{
    public class RssSyncProfilePartEditorViewModel
    {
        public RssSyncProfilePart RssSyncProfilePart { get; set; }
        public IEnumerable<SelectListItem> AccessibleContentTypes { get; set; }
    }
}
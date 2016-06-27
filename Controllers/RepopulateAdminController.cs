using Lombiq.FeedAggregator.Constants;
using Lombiq.FeedAggregator.Models;
using Orchard.ContentManagement;
using Orchard.Localization;
using Orchard.UI.Admin;
using Orchard.UI.Notify;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Lombiq.FeedAggregator.Controllers
{
    [Admin]
    public class RepopulateAdminController : Controller
    {
        private readonly INotifier _notifier;
        private readonly IContentManager _contentManager;

        public Localizer T { get; set; }


        public RepopulateAdminController(INotifier notifier, IContentManager contentManager)
        {
            _notifier = notifier;
            _contentManager = contentManager;

            T = NullLocalizer.Instance;
        }
        public ActionResult RepopulateFeedSyncProfileItems(int id, string returnUrl)
        {
            var feedSyncProfile = _contentManager.Get(id);

            if (feedSyncProfile == null || feedSyncProfile.ContentType != ContentTypes.FeedSyncProfile)
            {
                _notifier.Error(T("Scheduling of the repopulation failed."));
            }
            else
            {
                var feedSyncProfilePart = feedSyncProfile.As<FeedSyncProfilePart>();
                feedSyncProfilePart.SuccesfulInit = false;
                feedSyncProfilePart.LatestCreatedItemModificationDate = default(DateTime);

                _notifier.Information(T("Repopulation was successfully scheduled."));
            }

            return Redirect(returnUrl);
        }
    }
}
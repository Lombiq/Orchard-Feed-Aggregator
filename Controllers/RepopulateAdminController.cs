using Lombiq.FeedAggregator.Constants;
using Lombiq.FeedAggregator.Models;
using Orchard.ContentManagement;
using Orchard.Localization;
using Orchard.Mvc.Extensions;
using Orchard.Security;
using Orchard.UI.Admin;
using Orchard.UI.Notify;
using System;
using System.Web.Mvc;

namespace Lombiq.FeedAggregator.Controllers
{
    [Admin]
    public class RepopulateAdminController : Controller
    {
        private readonly INotifier _notifier;
        private readonly IContentManager _contentManager;
        private readonly IAuthorizer _authorizer;

        public Localizer T { get; set; }


        public RepopulateAdminController(INotifier notifier, IContentManager contentManager, IAuthorizer authorizer)
        {
            _notifier = notifier;
            _contentManager = contentManager;
            _authorizer = authorizer;

            T = NullLocalizer.Instance;
        }


        [HttpPost]
        public ActionResult RepopulateFeedSyncProfileItems(int id, string returnUrl)
        {
            if (!_authorizer.Authorize(Permissions.RepopulateFeedSyncProfilePermission))
                return new HttpUnauthorizedResult(T("You are not allowed to repopulate feed sync profiles.").Text);

            var feedSyncProfile = _contentManager.Get(id);

            if (feedSyncProfile == null || feedSyncProfile.ContentType != ContentTypes.FeedSyncProfile)
            {
                _notifier.Error(T("Scheduling the repopulation failed."));
            }
            else
            {
                var feedSyncProfilePart = feedSyncProfile.As<FeedSyncProfilePart>();
                feedSyncProfilePart.SuccesfulInit = false;
                feedSyncProfilePart.LatestCreatedItemModificationDate = default(DateTime);

                _notifier.Information(T("Repopulation was successfully scheduled."));
            }

            if (string.IsNullOrEmpty(returnUrl)) returnUrl = "~/";

            return this.RedirectLocal(returnUrl);
        }
    }
}
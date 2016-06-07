using Orchard.ContentManagement;
using Orchard.ContentManagement.Records;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lombiq.RssReader.Models
{
    public class RssSyncProfileItemPart : ContentPart<RssSyncProfileItemPartRecord>
    {
        /// <summary>
        /// The id of the correspondig RssSyncProfile.
        /// </summary>
        public int RssSyncProfileId
        {
            get { return Retrieve(x => x.RssSyncProfileId); }
            set { Store(x => x.RssSyncProfileId, value); }
        }

        /// <summary>
        /// The feed specific id in the feed entry. This is necessary for identifying the content item from
        /// the feed's perspective.
        /// In case of RSS feed it could be: guid, title or description.
        /// In case of Atom feed the id is required so it will be.
        /// </summary>
        public string FeedItemId
        {
            get { return Retrieve(x => x.FeedItemId); }
            set { Store(x => x.FeedItemId, value); }
        }
    }

    public class RssSyncProfileItemPartRecord : ContentPartRecord
    {
        public virtual int RssSyncProfileId { get; set; }
        public virtual string FeedItemId { get; set; }
    }
}
using Orchard.ContentManagement;
using Orchard.ContentManagement.Records;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lombiq.FeedAggregator.Models
{
    public class FeedSyncProfileItemPart : ContentPart<FeedSyncProfileItemPartRecord>
    {
        /// <summary>
        /// The ID of the correspondig FeedSyncProfile.
        /// </summary>
        public int FeedSyncProfileId
        {
            get { return Retrieve(x => x.FeedSyncProfileId); }
            set { Store(x => x.FeedSyncProfileId, value); }
        }

        /// <summary>
        /// The feed specific ID in the feed entry. This is necessary for identifying the content item.
        /// In case of an RSS feed it could be: guid, title or description.
        /// In case of an Atom feed the ID is required so it will be.
        /// </summary>
        public string FeedItemId
        {
            get { return Retrieve(x => x.FeedItemId); }
            set { Store(x => x.FeedItemId, value); }
        }
    }


    public class FeedSyncProfileItemPartRecord : ContentPartRecord
    {
        public virtual int FeedSyncProfileId { get; set; }
        public virtual string FeedItemId { get; set; }
    }
}
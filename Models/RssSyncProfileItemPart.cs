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
        public int RssSyncProfileId
        {
            get { return Retrieve(x => x.RssSyncProfileId); }
            set { Store(x => x.RssSyncProfileId, value); }
        }
    }

    public class RssSyncProfileItemPartRecord : ContentPartRecord
    {
        public virtual int RssSyncProfileId { get; set; }
    }
}
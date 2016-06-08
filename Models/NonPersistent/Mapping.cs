using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lombiq.FeedAggregator.Models.NonPersistent
{
    public class Mapping
    {
        /// <summary>
        /// Format: NodeName or NodeName.AttributeName.
        /// </summary>
        public string FeedMapping { get; set; }

        /// <summary>
        /// Format: PartName, PartName.FieldName or ContentType.FieldName.
        /// </summary>
        public string ContentItemStorageMapping { get; set; }
    }
}
using Lombiq.FeedAggregator.Constants;
using Lombiq.FeedAggregator.Models.NonPersistent;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Piedone.HelpfulLibraries.Contents;
using Orchard.Fields.Fields;
using Orchard.Core.Common.Fields;

namespace Lombiq.FeedAggregator.Models
{
    public class FeedSyncProfilePart : ContentPart
    {
        public string ContentType
        {
            get { return this.Retrieve(x => x.ContentType); }
            set { this.Store(x => x.ContentType, value); }
        }

        public string MappingsSerialized
        {
            get { return this.Retrieve(x => x.MappingsSerialized); }
            set { this.Store(x => x.MappingsSerialized, value); }
        }

        public string FeedItemIdType
        {
            get { return this.Retrieve(x => x.FeedItemIdType); }
            set { this.Store(x => x.FeedItemIdType, value); }
        }

        public string FeedItemModificationDateType
        {
            get { return this.Retrieve(x => x.FeedItemModificationDateType); }
            set { this.Store(x => x.FeedItemModificationDateType, value); }
        }

        /// <summary>
        /// The modification date of the entry in the feed.
        /// </summary>
        public DateTime LatestCreatedItemDate
        {
            get { return this.Retrieve(x => x.LatestCreatedItemDate); }
            set { this.Store(x => x.LatestCreatedItemDate, value); }
        }

        private readonly LazyField<List<Mapping>> _mappings = new LazyField<List<Mapping>>();
        internal LazyField<List<Mapping>> MappingsField { get { return _mappings; } }
        public List<Mapping> Mappings { get { return _mappings.Value; } }

        public decimal? MinutesBetweenSyncs
        {
            get
            {
                return this.AsField<NumericField>(
                    typeof(FeedSyncProfilePart).Name,
                    FieldNames.MinutesBetweenSyncs).Value;
            }
        }

        public string FeedUrl
        {
            get
            {
                return this.AsField<TextField>(
                    typeof(FeedSyncProfilePart).Name,
                    FieldNames.FeedUrl).Value;
            }
        }

        public decimal? NumberOfItemsToSyncDuringInit
        {
            get
            {
                return this.AsField<NumericField>(
                    typeof(FeedSyncProfilePart).Name,
                    FieldNames.NumberOfItemsToSyncDuringInit).Value;
            }
        }
    }
}
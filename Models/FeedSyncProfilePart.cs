using Lombiq.FeedAggregator.Constants;
using Lombiq.FeedAggregator.Models.NonPersistent;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Utilities;
using Orchard.ContentPicker.Fields;
using Orchard.Core.Common.Fields;
using Orchard.Fields.Fields;
using Piedone.HelpfulLibraries.Contents;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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

        public bool SuccesfulInit
        {
            get { return this.Retrieve(x => x.SuccesfulInit); }
            set { this.Store(x => x.SuccesfulInit, value); }
        }

        public int PublishedCount
        {
            get { return this.Retrieve(x => x.PublishedCount); }
            set { this.Store(x => x.PublishedCount, value); }
        }

        public string FeedType
        {
            get { return this.Retrieve(x => x.FeedType); }
            set { this.Store(x => x.FeedType, value); }
        }

        [Required]
        public string FeedUrl
        {
            get { return this.Retrieve(x => x.FeedUrl); }
            set { this.Store(x => x.FeedUrl, value); }
        }

        [Required]
        public int NumberOfItemsToSyncDuringInit
        {
            get { return this.Retrieve(x => x.NumberOfItemsToSyncDuringInit, 10); }
            set { this.Store(x => x.NumberOfItemsToSyncDuringInit, value); }
        }

        /// <summary>
        /// The modification date of the latest created entry.
        /// </summary>
        public DateTime LatestCreatedItemModificationDate
        {
            get { return this.Retrieve(x => x.LatestCreatedItemModificationDate); }
            set { this.Store(x => x.LatestCreatedItemModificationDate, value); }
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

        public ContentPickerField Container
        {
            get
            {
                return this.AsField<ContentPickerField>(
                    typeof(FeedSyncProfilePart).Name,
                    FieldNames.Container);
            }
        }
    }
}
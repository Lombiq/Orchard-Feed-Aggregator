using Lombiq.FeedAggregator.Models.NonPersistent;
using Orchard.ContentManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lombiq.FeedAggregator.Models
{
    public interface IFeedDataSavingProviderContext
    {
        /// <summary>
        /// The corresponding FeedSyncProfilePart.
        /// </summary>
        FeedSyncProfilePart FeedSyncProfilePart { get; }

        /// <summary>
        /// The feed content (xml node/attribute value) what will be saved.
        /// </summary>
        string FeedContent { get; }

        /// <summary>
        /// The corresponding mapping.
        /// </summary>
        Mapping Mapping { get; }

        /// <summary>
        /// The content in which the data will be saved.
        /// </summary>
        IContent Content { get; }
    }
}
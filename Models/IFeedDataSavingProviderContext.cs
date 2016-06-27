using Lombiq.FeedAggregator.Models.NonPersistent;
using Orchard.ContentManagement;
using System.Collections.Generic;

namespace Lombiq.FeedAggregator.Models
{
    public interface IFeedDataSavingProviderContext
    {
        /// <summary>
        /// The corresponding FeedSyncProfilePart.
        /// </summary>
        FeedSyncProfilePart FeedSyncProfilePart { get; }

        /// <summary>
        /// The feed content (xml node/attribute value) that will be saved.
        /// </summary>
        List<string> FeedContent { get; }

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
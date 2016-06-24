using Lombiq.FeedAggregator.Models;
using Orchard;
using Orchard.ContentManagement;
using Orchard.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lombiq.FeedAggregator.Services.FeedDataSavingProviders
{
    /// <summary>
    /// Provider for saving of an xml node/attribute value in the content item.
    /// </summary>
    public interface IFeedDataSavingProvider : IDependency
    {
        /// <summary>
        /// The type of the provider. 
        /// Format: PartType, PartType.PartPropertyName, FieldType.
        /// </summary>
        string ProviderType { get; }

        /// <summary>
        /// Save the data on the content item.
        /// </summary>
        /// <param name="feedDataSavingProviderContext">The feed data saving provider context.</param>
        /// <returns>True if the saving was successful.</returns>
        bool Save(IFeedDataSavingProviderContext feedDataSavingProviderContext);
    }
}
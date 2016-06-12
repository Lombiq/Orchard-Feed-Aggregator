using Lombiq.FeedAggregator.Models;
using Lombiq.FeedAggregator.Models.NonPersistent;
using Orchard.ContentManagement;
using Orchard.ContentManagement.MetaData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Piedone.HelpfulLibraries.Contents;

namespace Lombiq.FeedAggregator.Services
{
    public class FeedDataSavingProviderBase
    {
        private readonly IContentDefinitionManager _contentDefinitionManager;
        private readonly IContentManager _contentManager;


        public FeedDataSavingProviderBase(IContentDefinitionManager contentDefinitionManager, IContentManager contentManager)
        {
            _contentDefinitionManager = contentDefinitionManager;
            _contentManager = contentManager;
        }


        public bool ProviderIsSuitable(Mapping mapping, string providerType, string feedSyncProfileItemContentType)
        {
            var splitMapping = mapping.ContentItemStorageMapping.Split('.');
            // If it is a field mapping.
            if (splitMapping.Length > 1)
            {
                return _contentManager.New(feedSyncProfileItemContentType).HasField(splitMapping[0], splitMapping[1]);
            }
            // If it is a part mapping.
            else
            {
                return mapping.ContentItemStorageMapping == providerType;
            }
        }
    }
}
using Lombiq.FeedAggregator.Models;
using Lombiq.FeedAggregator.Models.NonPersistent;
using Orchard.ContentManagement;
using Orchard.ContentManagement.MetaData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lombiq.FeedAggregator.Services
{
    public class FeedDataSavingProviderBase
    {
        private readonly IContentDefinitionManager _contentDefinitionManager;


        public FeedDataSavingProviderBase(IContentDefinitionManager contentDefinitionManager)
        {
            _contentDefinitionManager = contentDefinitionManager;
        }


        public bool ProviderIsSuitable(Mapping mapping, string providerType, string feedSyncProfileItemContentType)
        {
            // It can be a simple part or a complex part.property mapping.
            if (mapping.ContentItemStorageMapping == providerType) return true;

            // If it isn't a part mapping, then it can be a field mapping.
            var typeDefinition = _contentDefinitionManager.GetTypeDefinition(feedSyncProfileItemContentType);
            if (typeDefinition == null) return false;

            // Checking for the part in the content type. If no such part, then it isn't suitable.
            var splitMapping = mapping.ContentItemStorageMapping.Split('.');
            var contentTypePartDefinition = typeDefinition
                .Parts
                .FirstOrDefault(part => part.PartDefinition.Name == splitMapping[0]);
            if (contentTypePartDefinition == null) return false;

            // Checking for a field with the name.
            var contentPartFieldDefinition = contentTypePartDefinition
                .PartDefinition
                .Fields
                .FirstOrDefault(field => field.DisplayName == splitMapping[1] && field.FieldDefinition.Name == providerType);

            return contentPartFieldDefinition != null;
        }
    }
}
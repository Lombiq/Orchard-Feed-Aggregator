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
            var splitMapping = mapping.ContentItemStorageMapping.Split('.');
            // If it is a field mapping.
            if (splitMapping.Length > 1)
            {
                var typeDefinition = _contentDefinitionManager.GetTypeDefinition(feedSyncProfileItemContentType);
                if (typeDefinition == null) return false;

                // Checking for the field in the content type.
                var contentTypePartDefinition = typeDefinition
                    .Parts
                    .FirstOrDefault(part => part.PartDefinition.Name == splitMapping[0]);

                if (contentTypePartDefinition == null) return false;

                var contentPartFieldDefinition = contentTypePartDefinition
                    .PartDefinition
                    .Fields
                    .FirstOrDefault(field => field.DisplayName == splitMapping[1] && field.FieldDefinition.Name == providerType);

                return contentPartFieldDefinition != null;
            }
            // If it is a part mapping.
            else
            {
                return mapping.ContentItemStorageMapping == providerType;
            }
        }
    }
}
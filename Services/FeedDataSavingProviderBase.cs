using Lombiq.FeedAggregator.Models;
using Lombiq.FeedAggregator.Models.NonPersistent;
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

                // Cecking for the field in the content type.
                foreach (var part in typeDefinition.Parts)
                {
                    if (part.PartDefinition.Name != splitMapping[0]) continue;

                    foreach (var field in part.PartDefinition.Fields)
                    {
                        if (field.DisplayName == splitMapping[1] && field.FieldDefinition.Name == providerType)
                        {
                            return true;
                        }
                    }
                }

                return false;
            }
            // If it is a part mapping.
            else
            {
                return mapping.ContentItemStorageMapping == providerType;
            }
        }
    }
}
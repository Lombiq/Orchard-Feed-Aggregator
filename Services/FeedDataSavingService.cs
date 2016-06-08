using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.ContentManagement;
using Orchard.ContentManagement.MetaData;

namespace Lombiq.FeedAggregator.Services
{
    public class FeedDataSavingService : IFeedDataSavingService
    {
        private readonly IEnumerable<IFeedDataSavingProvider> _providers;
        private readonly IContentDefinitionManager _contentDefinitionManager;


        public FeedDataSavingService(
            IEnumerable<IFeedDataSavingProvider> providers,
            IContentDefinitionManager contentDefinitionManager)
        {
            _providers = providers;
            _contentDefinitionManager = contentDefinitionManager;
        }


        public IList<string> GetAccessibleContentItemStorageNames(string contentType)
        {
            var accessibleDataStorageNames = new List<string>();
            var typeDefinition = _contentDefinitionManager.GetTypeDefinition(contentType);

            if (typeDefinition == null) return accessibleDataStorageNames;

            foreach (var provider in _providers)
            {
                var partOnContentItem = typeDefinition
                    .Parts
                    .FirstOrDefault(part => part.PartDefinition.Name == provider.ProviderType);

                if (partOnContentItem != null)
                {
                    accessibleDataStorageNames.Add(provider.ProviderType);
                }
                else
                {
                    foreach (var part in typeDefinition.Parts)
                    {
                        foreach (var field in part.PartDefinition.Fields)
                        {
                            if (field.FieldDefinition.Name == provider.ProviderType)
                            {
                                accessibleDataStorageNames.Add(part.PartDefinition.Name + "." + field.Name);
                            }
                        }
                    }
                }
            }

            return accessibleDataStorageNames;
        }
    }
}
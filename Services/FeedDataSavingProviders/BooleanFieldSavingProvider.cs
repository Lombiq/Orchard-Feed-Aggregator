using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Piedone.HelpfulLibraries.Contents;
using Orchard.ContentManagement.MetaData;
using Orchard.ContentManagement;
using Lombiq.FeedAggregator.Models;
using Orchard.Fields.Fields;

namespace Lombiq.FeedAggregator.Services.FeedDataSavingProviders
{
    public class BooleanFieldSavingProvider : FeedDataSavingProviderBase, IFeedDataSavingProvider
    {
        public BooleanFieldSavingProvider(
            IContentDefinitionManager contentDefinitionManager) :
            base(contentDefinitionManager)
        {
            ProviderType = "BooleanField";
        }


        public bool Save(IFeedDataSavingProviderContext context)
        {
            if (!ProviderIsSuitable(context.Mapping, ProviderType, context.FeedSyncProfilePart.ContentType))
                return false;

            var splitMapping = context.Mapping.ContentItemStorageMapping.Split('.');

            var booleanField = context.Content.AsField<BooleanField>(splitMapping[0], splitMapping[1]);
            if (booleanField == null) return false;

            var booleanValue = default(bool);
            if (!bool.TryParse(context.FeedContent, out booleanValue))
                return false;
            booleanField.Value = booleanValue;

            return true;
        }
    }
}
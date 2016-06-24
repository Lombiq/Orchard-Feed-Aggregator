using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.ContentManagement;
using Lombiq.FeedAggregator.Models;
using Orchard.ContentManagement.MetaData;
using Piedone.HelpfulLibraries.Contents;
using Orchard.Core.Common.Fields;
using Orchard.Fields.Fields;

namespace Lombiq.FeedAggregator.Services.FeedDataSavingProviders
{
    public class LinkFieldSavingProvider : FeedDataSavingProviderBase, IFeedDataSavingProvider
    {
        public LinkFieldSavingProvider(
            IContentDefinitionManager contentDefinitionManager) :
            base(contentDefinitionManager)
        {
            ProviderType = "LinkField";
        }


        public bool Save(IFeedDataSavingProviderContext context)
        {
            if (!ProviderIsSuitable(context.Mapping, ProviderType, context.FeedSyncProfilePart.ContentType)) return false;

            var splitMapping = context.Mapping.ContentItemStorageMapping.Split('.');

            var linkField = context.Content.AsField<LinkField>(splitMapping[0], splitMapping[1]);
            if (linkField == null) return false;

            linkField.Value = context.FeedContent;

            return true;
        }
    }
}
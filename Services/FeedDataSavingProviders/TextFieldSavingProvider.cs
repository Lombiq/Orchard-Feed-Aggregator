using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.ContentManagement;
using Lombiq.FeedAggregator.Models;
using Orchard.ContentManagement.MetaData;
using Piedone.HelpfulLibraries.Contents;
using Orchard.Core.Common.Fields;

namespace Lombiq.FeedAggregator.Services.FeedDataSavingProviders
{
    public class TextFieldSavingProvider : FeedDataSavingProviderBase, IFeedDataSavingProvider
    {
        public TextFieldSavingProvider(
            IContentDefinitionManager contentDefinitionManager) :
            base(contentDefinitionManager)
        {
            ProviderType = "TextField";
        }


        public bool Save(IFeedDataSavingProviderContext context)
        {
            if (!ProviderIsSuitable(context.Mapping, ProviderType, context.FeedSyncProfilePart.ContentType)) return false;

            var splitMapping = context.Mapping.ContentItemStorageMapping.Split('.');

            var textField = context.Content.AsField<TextField>(splitMapping[0], splitMapping[1]);
            if (textField == null) return false;

            textField.Value = context.FeedContent;

            return true;
        }
    }
}
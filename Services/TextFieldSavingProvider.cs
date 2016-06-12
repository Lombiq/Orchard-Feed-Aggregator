using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.ContentManagement;
using Lombiq.FeedAggregator.Models;
using Orchard.ContentManagement.MetaData;
using Piedone.HelpfulLibraries.Contents;
using Orchard.Core.Common.Fields;

namespace Lombiq.FeedAggregator.Services
{
    public class TextFieldSavingProvider : FeedDataSavingProviderBase, IFeedDataSavingProvider
    {
        public string ProviderType { get { return "TextField"; } }


        public TextFieldSavingProvider(
            IContentDefinitionManager contentDefinitionManager, IContentManager contentManager) :
            base(contentDefinitionManager, contentManager)
        {
        }


        public bool Save(IFeedDataSavingProviderContext context)
        {
            if (!ProviderIsSuitable(context.Mapping, ProviderType, context.FeedSyncProfilePart.ContentType)) return false;

            var splitMapping = context.Mapping.ContentItemStorageMapping.Split('.');

            var textField = context.Content.AsField<TextField>(splitMapping[0], splitMapping[1]);
            if (textField == null) return false;

            textField.Value = context.Data;

            return true;
        }
    }
}
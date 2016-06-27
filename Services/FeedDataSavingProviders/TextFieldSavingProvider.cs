using Lombiq.FeedAggregator.Models;
using Orchard.ContentManagement.MetaData;
using Orchard.Core.Common.Fields;
using Piedone.HelpfulLibraries.Contents;
using System.Linq;

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
            if (!ProviderIsSuitable(context.Mapping, context.FeedSyncProfilePart.ContentType)) return false;

            var splitMapping = context.Mapping.ContentItemStorageMapping.Split('.');

            var textField = context.Content.AsField<TextField>(splitMapping[0], splitMapping[1]);
            if (textField == null) return false;

            textField.Value = context.FeedContent.First();

            return true;
        }
    }
}
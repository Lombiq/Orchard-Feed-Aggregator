using Lombiq.FeedAggregator.Models;
using Orchard.ContentManagement.MetaData;
using Orchard.Fields.Fields;
using Piedone.HelpfulLibraries.Contents;

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
            if (!ProviderIsSuitable(context.Mapping, context.FeedSyncProfilePart.ContentType))
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
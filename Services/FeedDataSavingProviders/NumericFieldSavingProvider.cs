using Lombiq.FeedAggregator.Models;
using Orchard.ContentManagement.MetaData;
using Orchard.Fields.Fields;
using Piedone.HelpfulLibraries.Contents;
using System.Globalization;
using System.Linq;

namespace Lombiq.FeedAggregator.Services.FeedDataSavingProviders
{
    public class NumericFieldSavingProvider : FeedDataSavingProviderBase, IFeedDataSavingProvider
    {
        public NumericFieldSavingProvider(
            IContentDefinitionManager contentDefinitionManager) :
            base(contentDefinitionManager)
        {
            ProviderType = "NumericField";
        }


        public bool Save(IFeedDataSavingProviderContext context)
        {
            if (!ProviderIsSuitable(context.Mapping, context.FeedSyncProfilePart.ContentType))
                return false;

            var splitMapping = context.Mapping.ContentItemStorageMapping.Split('.');

            var numericField = context.Content.AsField<NumericField>(splitMapping[0], splitMapping[1]);
            if (numericField == null) return false;

            var decimalValue = default(decimal);
            if (!decimal.TryParse(context.FeedContent.First(), NumberStyles.Number, CultureInfo.InvariantCulture, out decimalValue))
                return false;
            numericField.Value = decimalValue;

            return true;
        }
    }
}
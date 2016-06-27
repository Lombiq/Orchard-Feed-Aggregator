using Lombiq.FeedAggregator.Models;
using Orchard.ContentManagement;
using Orchard.ContentManagement.MetaData;
using Orchard.Core.Common.Models;
using System;
using System.Linq;

namespace Lombiq.FeedAggregator.Services.FeedDataSavingProviders
{
    public class CommonPartModifiedUtcSavingProvider : FeedDataSavingProviderBase, IFeedDataSavingProvider
    {
        public CommonPartModifiedUtcSavingProvider(
            IContentDefinitionManager contentDefinitionManager)
            : base(contentDefinitionManager)
        {
            ProviderType = "CommonPart.ModifiedUtc";
        }


        public bool Save(IFeedDataSavingProviderContext context)
        {
            if (!ProviderIsSuitable(context.Mapping, context.FeedSyncProfilePart.ContentType))
                return false;

            var commonPart = context.Content.As<CommonPart>();
            if (commonPart == null) return false;

            var dateValue = default(DateTime);
            if (!DateTime.TryParse(context.FeedContent.First(), out dateValue))
                return false;

            commonPart.ModifiedUtc = dateValue;

            return true;
        }
    }
}
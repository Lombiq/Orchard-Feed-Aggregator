using Lombiq.FeedAggregator.Helpers;
using Lombiq.FeedAggregator.Models;
using Orchard.ContentManagement;
using Orchard.ContentManagement.MetaData;
using Orchard.Core.Common.Models;
using System;
using System.Linq;

namespace Lombiq.FeedAggregator.Services.FeedDataSavingProviders
{
    public class CommonPartCreatedUtcSavingProvider : FeedDataSavingProviderBase, IFeedDataSavingProvider
    {
        public CommonPartCreatedUtcSavingProvider(
            IContentDefinitionManager contentDefinitionManager)
            : base(contentDefinitionManager)
        {
            ProviderType = "CommonPart.CreatedUtc";
        }


        public bool Save(IFeedDataSavingProviderContext context)
        {
            if (!ProviderIsSuitable(context.Mapping, context.FeedSyncProfilePart.ContentType))
                return false;

            var commonPart = context.Content.As<CommonPart>();
            if (commonPart == null) return false;

            var dateValue = default(DateTime);
            if (!DateTimeHelper.TryParseDateTime(context.FeedContent.First(), out dateValue))
                return false;

            commonPart.CreatedUtc = dateValue;

            return true;
        }
    }
}
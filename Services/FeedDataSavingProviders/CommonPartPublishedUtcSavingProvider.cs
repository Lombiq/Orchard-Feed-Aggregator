using Lombiq.FeedAggregator.Models;
using Orchard.ContentManagement;
using Orchard.ContentManagement.MetaData;
using Orchard.Core.Common.Models;
using System;

namespace Lombiq.FeedAggregator.Services.FeedDataSavingProviders
{
    public class CommonPartPublishedUtcSavingProvider : FeedDataSavingProviderBase, IFeedDataSavingProvider
    {
        public CommonPartPublishedUtcSavingProvider(
            IContentDefinitionManager contentDefinitionManager)
            : base(contentDefinitionManager)
        {
            ProviderType = "CommonPart.PublishedUtc";
        }


        public bool Save(IFeedDataSavingProviderContext context)
        {
            if (!ProviderIsSuitable(context.Mapping, context.FeedSyncProfilePart.ContentType))
                return false;

            var commonPart = context.Content.As<CommonPart>();
            if (commonPart == null) return false;

            var dateValue = default(DateTime);
            if (!DateTime.TryParse(context.FeedContent, out dateValue))
                return false;

            commonPart.PublishedUtc = dateValue;

            return true;
        }
    }
}
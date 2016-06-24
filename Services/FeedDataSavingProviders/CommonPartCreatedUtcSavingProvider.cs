using Lombiq.FeedAggregator.Models;
using Orchard.ContentManagement;
using Orchard.ContentManagement.MetaData;
using Orchard.Core.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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
            if (!ProviderIsSuitable(context.Mapping, ProviderType, context.FeedSyncProfilePart.ContentType))
                return false;

            var commonPart = context.Content.As<CommonPart>();
            if (commonPart == null) return false;

            var dateValue = default(DateTime);
            if (!DateTime.TryParse(context.FeedContent, out dateValue))
                return false;

            commonPart.CreatedUtc = dateValue;

            return true;
        }
    }
}
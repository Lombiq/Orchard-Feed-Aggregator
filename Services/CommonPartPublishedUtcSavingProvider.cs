using Lombiq.FeedAggregator.Models;
using Orchard.ContentManagement;
using Orchard.ContentManagement.MetaData;
using Orchard.Core.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lombiq.FeedAggregator.Services
{
    public class CommonPartPublishedUtcSavingProvider : FeedDataSavingProviderBase, IFeedDataSavingProvider
    {
        public string ProviderType { get { return "CommonPart.PublishedUtc"; } }


        public CommonPartPublishedUtcSavingProvider(
            IContentDefinitionManager contentDefinitionManager)
            : base(contentDefinitionManager)
        {
        }


        public bool Save(IFeedDataSavingProviderContext context)
        {
            if (!ProviderIsSuitable(context.Mapping, ProviderType, context.FeedSyncProfilePart.ContentType))
                return false;

            var commonPart = context.Content.As<CommonPart>();
            if (commonPart == null) return false;

            var dateValue = default(DateTime);
            if (!DateTime.TryParse(context.Data, out dateValue))
                return false;

            commonPart.PublishedUtc = dateValue;

            return true;
        }
    }
}
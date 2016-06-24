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
    public class BodyPartSavingProvider : FeedDataSavingProviderBase, IFeedDataSavingProvider
    {
        public BodyPartSavingProvider(
            IContentDefinitionManager contentDefinitionManager)
            : base(contentDefinitionManager)
        {
            ProviderType = "BodyPart";
        }


        public bool Save(IFeedDataSavingProviderContext context)
        {
            if (!ProviderIsSuitable(context.Mapping, ProviderType, context.FeedSyncProfilePart.ContentType)) return false;

            var bodyPart = context.Content.As<BodyPart>();
            if (bodyPart == null) return false;

            bodyPart.Text = context.FeedContent;

            return true;
        }
    }
}
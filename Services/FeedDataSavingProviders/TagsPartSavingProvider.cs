using Lombiq.FeedAggregator.Models;
using Orchard.ContentManagement;
using Orchard.ContentManagement.MetaData;
using Orchard.Tags.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lombiq.FeedAggregator.Services.FeedDataSavingProviders
{
    public class TagsPartSavingProvider : FeedDataSavingProviderBase, IFeedDataSavingProvider
    {
        public TagsPartSavingProvider(
            IContentDefinitionManager contentDefinitionManager)
            : base(contentDefinitionManager)
        {
            ProviderType = "TagsPart";
        }


        public bool Save(IFeedDataSavingProviderContext context)
        {
            if (!ProviderIsSuitable(context.Mapping, ProviderType, context.FeedSyncProfilePart.ContentType)) return false;

            var tagsPart = context.Content.As<TagsPart>();
            if (tagsPart == null) return false;

            tagsPart.CurrentTags = context.FeedContent.Split(',');

            return true;
        }
    }
}
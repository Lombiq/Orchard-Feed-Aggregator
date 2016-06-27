using Lombiq.FeedAggregator.Models;
using Orchard.ContentManagement;
using Orchard.ContentManagement.MetaData;
using Orchard.Tags.Models;

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
            if (!ProviderIsSuitable(context.Mapping, context.FeedSyncProfilePart.ContentType)) return false;

            var tagsPart = context.Content.As<TagsPart>();
            if (tagsPart == null) return false;

            tagsPart.CurrentTags = context.FeedContent;

            return true;
        }
    }
}
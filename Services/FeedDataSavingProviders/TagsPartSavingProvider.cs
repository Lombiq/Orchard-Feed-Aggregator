using Lombiq.FeedAggregator.Models;
using Orchard.ContentManagement;
using Orchard.ContentManagement.MetaData;
using Orchard.Tags.Models;
using Orchard.Tags.Services;

namespace Lombiq.FeedAggregator.Services.FeedDataSavingProviders
{
    public class TagsPartSavingProvider : FeedDataSavingProviderBase, IFeedDataSavingProvider
    {
        private readonly ITagService _tagService;


        public TagsPartSavingProvider(
            IContentDefinitionManager contentDefinitionManager,
            ITagService tagService)
            : base(contentDefinitionManager)
        {
            _tagService = tagService;
            ProviderType = "TagsPart";
        }


        public bool Save(IFeedDataSavingProviderContext context)
        {
            if (!ProviderIsSuitable(context.Mapping, context.FeedSyncProfilePart.ContentType)) return false;

            var tagsPart = context.Content.As<TagsPart>();
            if (tagsPart == null) return false;

            foreach (var feedContent in context.FeedContent)
            {
                _tagService.CreateTag(feedContent);
            }

            _tagService.UpdateTagsForContentItem(context.Content.ContentItem, context.FeedContent);

            return true;
        }
    }
}
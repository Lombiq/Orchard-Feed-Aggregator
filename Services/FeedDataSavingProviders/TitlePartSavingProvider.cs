using Lombiq.FeedAggregator.Models;
using Orchard.ContentManagement;
using Orchard.ContentManagement.MetaData;
using Orchard.Core.Title.Models;

namespace Lombiq.FeedAggregator.Services.FeedDataSavingProviders
{
    public class TitlePartSavingProvider : FeedDataSavingProviderBase, IFeedDataSavingProvider
    {
        public TitlePartSavingProvider(
            IContentDefinitionManager contentDefinitionManager)
            : base(contentDefinitionManager)
        {
            ProviderType = "TitlePart";
        }


        public bool Save(IFeedDataSavingProviderContext context)
        {
            if (!ProviderIsSuitable(context.Mapping, context.FeedSyncProfilePart.ContentType))
                return false;

            var titlePart = context.Content.As<TitlePart>();
            if (titlePart == null) return false;

            titlePart.Title = context.FeedContent;

            return true;
        }
    }
}
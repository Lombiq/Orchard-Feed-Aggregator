using Lombiq.FeedAggregator.Models;
using Orchard.ContentManagement;
using Orchard.ContentManagement.MetaData;
using Orchard.Core.Common.Models;

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
            if (!ProviderIsSuitable(context.Mapping, context.FeedSyncProfilePart.ContentType)) return false;

            var bodyPart = context.Content.As<BodyPart>();
            if (bodyPart == null) return false;

            bodyPart.Text = context.FeedContent;

            return true;
        }
    }
}
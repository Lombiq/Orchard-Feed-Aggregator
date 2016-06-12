using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.ContentManagement;
using Lombiq.FeedAggregator.Models;
using Orchard.ContentManagement.MetaData;

namespace Lombiq.FeedAggregator.Services
{
    public class TitlePartSavingProvider : FeedDataSavingProviderBase, IFeedDataSavingProvider
    {
        public string ProviderType { get { return "TitlePart"; } }


        public TitlePartSavingProvider(
            IContentDefinitionManager contentDefinitionManager, IContentManager contentManager)
            : base(contentDefinitionManager, contentManager)
        {
        }


        public bool Save(IFeedDataSavingProviderContext context)
        {
            if (!ProviderIsSuitable(context.Mapping, ProviderType, context.FeedSyncProfilePart.ContentType)) return false;

            return true;
        }
    }
}
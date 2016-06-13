using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.ContentManagement;
using Lombiq.FeedAggregator.Models;
using Orchard.ContentManagement.MetaData;
using Orchard.Core.Title.Models;

namespace Lombiq.FeedAggregator.Services
{
    public class TitlePartSavingProvider : FeedDataSavingProviderBase, IFeedDataSavingProvider
    {
        public string ProviderType { get { return "TitlePart"; } }


        public TitlePartSavingProvider(
            IContentDefinitionManager contentDefinitionManager)
            : base(contentDefinitionManager)
        {
        }


        public bool Save(IFeedDataSavingProviderContext context)
        {
            if (!ProviderIsSuitable(context.Mapping, ProviderType, context.FeedSyncProfilePart.ContentType))
                return false;

            var titlePart = context.Content.As<TitlePart>();
            if (titlePart == null) return false;

            titlePart.Title = context.Data;

            return true;
        }
    }
}
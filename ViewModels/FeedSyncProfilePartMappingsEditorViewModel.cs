using Lombiq.FeedAggregator.Models;
using System.Collections.Generic;

namespace Lombiq.FeedAggregator.ViewModels
{
    public class FeedSyncProfilePartMappingsEditorViewModel
    {
        public FeedSyncProfilePart FeedSyncProfilePart { get; set; }
        public IEnumerable<MappingViewModel> MappingViewModel { get; set; }
    }
}
using Lombiq.RssReader.Models;
using System.Collections.Generic;

namespace Lombiq.RssReader.ViewModels
{
    public class RssSyncProfilePartMappingsEditorViewModel
    {
        public RssSyncProfilePart RssSyncProfilePart { get; set; }
        public IEnumerable<MappingViewModel> MappingViewModel { get; set; }
    }
}
using Lombiq.RssReader.Models;
using Lombiq.RssReader.Models.NonPersistent;
using Orchard.ContentManagement.Handlers;
using Orchard.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lombiq.RssReader.Handlers
{
    public class RssSyncProfilePartHandler : ContentHandler
    {
        public RssSyncProfilePartHandler(IJsonConverter jsonConverter)
        {
            OnActivated<RssSyncProfilePart>((context, part) =>
            {
                part.MappingsField.Loader(() =>
                {
                    return string.IsNullOrEmpty(part.MappingsSerialized)
                        ? new List<Mapping>()
                        : jsonConverter.Deserialize<List<Mapping>>(part.MappingsSerialized);
                });
            });
        }
    }
}
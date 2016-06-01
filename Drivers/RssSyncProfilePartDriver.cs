using Lombiq.RssReader.Models;
using Lombiq.RssReader.ViewModels;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.ContentManagement.MetaData;
using Orchard.Core.Contents.Settings;
using Orchard.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Lombiq.RssReader.Drivers
{
    public class RssSyncProfilePartDriver : ContentPartDriver<RssSyncProfilePart>
    {
        private readonly IContentDefinitionManager _contentDefinitionManager;


        public Localizer T { get; set; }


        public RssSyncProfilePartDriver(IContentDefinitionManager contentDefinitionManager)
        {
            _contentDefinitionManager = contentDefinitionManager;

            T = NullLocalizer.Instance;
        }

        protected override DriverResult Editor(RssSyncProfilePart part, dynamic shapeHelper)
        {
            return ContentShape(
                "Parts_RssSyncProfile_Edit",
                () =>
                {
                    var accessibleContentTypes =
                        GetTypesWithRssSyncProfileItemPart()
                        .Select(item =>
                            new SelectListItem { Text = item, Value = item, Selected = item == part.ContentType });

                    return shapeHelper.EditorTemplate(
                        TemplateName: "Parts.RssSyncProfile",
                        Model: new RssSyncProfilePartEditorViewModel
                        {
                            RssSyncProfilePart = part,
                            AccessibleContentTypes = accessibleContentTypes
                        },
                        Prefix: Prefix);
                });
        }

        protected override DriverResult Editor(RssSyncProfilePart part, IUpdateModel updater, dynamic shapeHelper)
        {
            var oldContentTypeValue = part.ContentType;
            if (updater.TryUpdateModel(part, Prefix, null, null))
            {
                // This cannot be changed, because the mappings will be generated according to this type.
                if (!string.IsNullOrEmpty(oldContentTypeValue))
                {
                    part.ContentType = oldContentTypeValue;
                }

                if (!GetTypesWithRssSyncProfileItemPart().Contains(part.ContentType))
                {
                    updater.AddModelError("InvalidContentTye", T("Please select a content type with RssSyncProfileItemPart."));
                }
            }

            return Editor(part, shapeHelper);
        }


        private IEnumerable<string> GetTypesWithRssSyncProfileItemPart()
        {
            return _contentDefinitionManager.ListTypeDefinitions()
                .Where(ctd =>
                    ctd.Parts.Select(p => p.PartDefinition.Name).Contains(typeof(RssSyncProfileItemPart).Name) &&
                    ctd.Settings.GetModel<ContentTypeSettings>().Draftable)
                .Select(ctd => ctd.Name);
        }
    }
}
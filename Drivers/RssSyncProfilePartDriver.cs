using Lombiq.RssReader.Models;
using Lombiq.RssReader.Models.NonPersistent;
using Lombiq.RssReader.Services;
using Lombiq.RssReader.ViewModels;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.ContentManagement.MetaData;
using Orchard.Core.Contents.Settings;
using Orchard.Localization;
using Orchard.Services;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Lombiq.RssReader.Drivers
{
    public class RssSyncProfilePartDriver : ContentPartDriver<RssSyncProfilePart>
    {
        private readonly IContentDefinitionManager _contentDefinitionManager;
        private readonly IRssFeedDataSavingService _rssFeedDataSavingService;
        private readonly IJsonConverter _jsonConverter;


        public Localizer T { get; set; }


        public RssSyncProfilePartDriver(
            IContentDefinitionManager contentDefinitionManager,
            IRssFeedDataSavingService rssFeedDataSavingService,
            IJsonConverter jsonConverter)
        {
            _contentDefinitionManager = contentDefinitionManager;
            _rssFeedDataSavingService = rssFeedDataSavingService;
            _jsonConverter = jsonConverter;

            T = NullLocalizer.Instance;
        }


        protected override DriverResult Editor(RssSyncProfilePart part, dynamic shapeHelper)
        {
            return Combined(
                ContentShape(
                    "Parts_RssSyncProfile_ContentType_Edit",
                    () =>
                    {
                        var accessibleContentTypes =
                            GetTypesWithRssSyncProfileItemPart()
                            .Select(item =>
                                new SelectListItem { Text = item, Value = item, Selected = item == part.ContentType });

                        return shapeHelper.EditorTemplate(
                            TemplateName: "Parts.RssSyncProfile.ContentType",
                            Model: new RssSyncProfilePartContentTypeEditorViewModel
                            {
                                RssSyncProfilePart = part,
                                AccessibleContentTypes = accessibleContentTypes
                            },
                            Prefix: Prefix);
                    }),
                ContentShape(
                    "Parts_RssSyncProfile_Mappings_Edit",
                    () =>
                    {
                        var accessibleContentItemStorageNames = _rssFeedDataSavingService
                            .GetAccessibleContentItemStorageNames(part.ContentType);

                        // If a mapping data storage no longer available or the RSS node field is empty,
                        // then delete it from the mappings.
                        part
                            .Mappings
                            .RemoveAll(mapping =>
                                !accessibleContentItemStorageNames.Contains(mapping.ContentItemStorageMapping) ||
                                string.IsNullOrEmpty(mapping.RssMapping));

                        var mappingViewModel = new List<MappingViewModel>();
                        var contentItemStorageMappings = part
                            .Mappings
                            .Select(mapping => mapping.ContentItemStorageMapping);

                        // Adding as many empty fields as many can be created.
                        foreach (var accessibleContentItemStorageName in accessibleContentItemStorageNames)
                        {
                            if (!contentItemStorageMappings.Contains(accessibleContentItemStorageName))
                            {
                                part
                                    .Mappings
                                    .Add(
                                        new Mapping
                                        {
                                            ContentItemStorageMapping = "",
                                            RssMapping = ""
                                        });
                            }
                        }

                        // Constructing data sources for the dropdowns.
                        foreach (var mapping in part.Mappings)
                        {
                            var selectList = new List<SelectListItem>();
                            selectList.Add(
                                new SelectListItem
                                {
                                    Text = "Select an Option Below",
                                    Value = "",
                                    Selected = string.IsNullOrEmpty(mapping.ContentItemStorageMapping)
                                });

                            selectList
                                .AddRange(
                                    accessibleContentItemStorageNames
                                    .Select(item =>
                                        new SelectListItem
                                        {
                                            Text = item,
                                            Value = item,
                                            Selected = item == mapping.ContentItemStorageMapping
                                        }));

                            mappingViewModel.Add(
                                new MappingViewModel
                                {
                                    RssMapping = mapping.RssMapping,
                                    ContentItemStorageMappingSelectList = selectList
                                });
                        }

                        return shapeHelper.EditorTemplate(
                            TemplateName: "Parts.RssSyncProfile.Mappings",
                            Model: new RssSyncProfilePartMappingsEditorViewModel
                            {
                                RssSyncProfilePart = part,
                                MappingViewModel = mappingViewModel
                            },
                            Prefix: Prefix);
                    }));
        }

        protected override DriverResult Editor(RssSyncProfilePart part, IUpdateModel updater, dynamic shapeHelper)
        {
            var oldContentTypeValue = part.ContentType;
            if (updater.TryUpdateModel(part, Prefix, null, null))
            {
                // This property cannot be changed, because the mappings will be generated according to this type.
                if (!string.IsNullOrEmpty(oldContentTypeValue))
                {
                    part.ContentType = oldContentTypeValue;
                }

                if (!GetTypesWithRssSyncProfileItemPart().Contains(part.ContentType))
                {
                    updater.AddModelError("InvalidContentTye", T("Please select a content type with RssSyncProfileItemPart."));
                }

                // Clearing the empty mappings so only the filled ones will be saved.
                part
                    .Mappings
                    .RemoveAll(mapping =>
                        string.IsNullOrEmpty(mapping.RssMapping) ||
                        string.IsNullOrEmpty(mapping.ContentItemStorageMapping));

                part.MappingsSerialized = _jsonConverter.Serialize(part.Mappings);
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
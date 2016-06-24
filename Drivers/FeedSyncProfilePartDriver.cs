using Lombiq.FeedAggregator.Models;
using Lombiq.FeedAggregator.Models.NonPersistent;
using Lombiq.FeedAggregator.Services;
using Lombiq.FeedAggregator.ViewModels;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.ContentManagement.MetaData;
using Orchard.Core.Contents.Settings;
using Orchard.Localization;
using Orchard.Services;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace Lombiq.FeedAggregator.Drivers
{
    public class FeedSyncProfilePartDriver : ContentPartDriver<FeedSyncProfilePart>
    {
        private readonly IContentDefinitionManager _contentDefinitionManager;
        private readonly IFeedManager _feedManager;
        private readonly IJsonConverter _jsonConverter;


        public Localizer T { get; set; }


        public FeedSyncProfilePartDriver(
            IContentDefinitionManager contentDefinitionManager,
            IFeedManager feedManager,
            IJsonConverter jsonConverter)
        {
            _contentDefinitionManager = contentDefinitionManager;
            _feedManager = feedManager;
            _jsonConverter = jsonConverter;

            T = NullLocalizer.Instance;
        }


        protected override DriverResult Editor(FeedSyncProfilePart part, dynamic shapeHelper)
        {
            return Combined(
                ContentShape(
                    "Parts_FeedSyncProfile_ContentType_Edit",
                    () =>
                    {
                        var compatibleContentTypes =
                            GetTypesWithFeedSyncProfileItemPart()
                            .Select(item =>
                                new SelectListItem { Text = item, Value = item, Selected = item == part.ContentType });

                        return shapeHelper.EditorTemplate(
                            TemplateName: "Parts.FeedSyncProfile.ContentType",
                            Model: new FeedSyncProfilePartContentTypeEditorViewModel
                            {
                                FeedSyncProfilePart = part,
                                CompatibleContentTypes = compatibleContentTypes
                            },
                            Prefix: Prefix);
                    }),
                ContentShape(
                    "Parts_FeedSyncProfile_Mappings_Edit",
                    () =>
                    {
                        var compatibleContentItemStorageNames = _feedManager
                            .GetCompatibleContentItemStorageNames(part.ContentType);

                        // Setting the smart defaults.
                        if (!part.Mappings.Any())
                        {
                            if (compatibleContentItemStorageNames.Contains("TitlePart"))
                            {
                                part.Mappings.Add(new Mapping { FeedMapping = "title", ContentItemStorageMapping = "TitlePart" });
                            }

                            if (compatibleContentItemStorageNames.Contains("BodyPart"))
                            {
                                part.Mappings.Add(new Mapping { FeedMapping = "description", ContentItemStorageMapping = "BodyPart" });
                            }
                        }

                        // If a mapping data storage is no longer available or the feed node field is empty,
                        // then delete it from the mappings.
                        part
                            .Mappings
                            .RemoveAll(mapping =>
                                !compatibleContentItemStorageNames.Contains(mapping.ContentItemStorageMapping) ||
                                string.IsNullOrEmpty(mapping.FeedMapping));

                        var mappingViewModel = new List<MappingViewModel>();
                        var contentItemStorageMappings = part
                            .Mappings
                            .Select(mapping => mapping.ContentItemStorageMapping);

                        // Adding as many empty fields as many can be created.
                        foreach (var compatibleContentItemStorageName in compatibleContentItemStorageNames)
                        {
                            if (!contentItemStorageMappings.Contains(compatibleContentItemStorageName))
                            {
                                part
                                    .Mappings
                                    .Add(
                                        new Mapping
                                        {
                                            ContentItemStorageMapping = "",
                                            FeedMapping = ""
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
                                    Text = T("Select an Option Below").Text,
                                    Value = "",
                                    Selected = string.IsNullOrEmpty(mapping.ContentItemStorageMapping)
                                });

                            selectList
                                .AddRange(
                                    compatibleContentItemStorageNames
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
                                    FeedMapping = mapping.FeedMapping,
                                    ContentItemStorageMappingSelectList = selectList
                                });
                        }

                        return shapeHelper.EditorTemplate(
                            TemplateName: "Parts.FeedSyncProfile.Mappings",
                            Model: new FeedSyncProfilePartMappingsEditorViewModel
                            {
                                FeedSyncProfilePart = part,
                                MappingViewModel = mappingViewModel
                            },
                            Prefix: Prefix);
                    }));
        }

        protected override DriverResult Editor(FeedSyncProfilePart part, IUpdateModel updater, dynamic shapeHelper)
        {
            var oldContentTypeValue = part.ContentType;
            var oldNumberOfItemsToSyncDuringInitValue = part.NumberOfItemsToSyncDuringInit;
            if (updater.TryUpdateModel(part, Prefix, null, null))
            {
                // This property cannot be changed, because the mappings will be generated according to this type.
                if (!string.IsNullOrEmpty(oldContentTypeValue))
                {
                    part.ContentType = oldContentTypeValue;
                }

                if (oldNumberOfItemsToSyncDuringInitValue != null)
                {
                    part.NumberOfItemsToSyncDuringInit = oldNumberOfItemsToSyncDuringInitValue;
                }

                if (!GetTypesWithFeedSyncProfileItemPart().Contains(part.ContentType))
                {
                    updater.AddModelError("InvalidContentType", T("Please select a content type with FeedSyncProfileItemPart."));
                }

                // Clearing the empty mappings so only the filled ones will be saved.
                part
                    .Mappings
                    .RemoveAll(mapping =>
                        string.IsNullOrEmpty(mapping.FeedMapping) ||
                        string.IsNullOrEmpty(mapping.ContentItemStorageMapping));

                // Removing all whitespace characters from the mappings.
                part.MappingsSerialized = Regex.Replace(_jsonConverter.Serialize(part.Mappings), @"\s+", "");
            }

            return Editor(part, shapeHelper);
        }


        private IEnumerable<string> GetTypesWithFeedSyncProfileItemPart()
        {
            return _contentDefinitionManager.ListTypeDefinitions()
                .Where(ctd =>
                    ctd.Parts.Select(p => p.PartDefinition.Name).Contains(typeof(FeedSyncProfileItemPart).Name) &&
                    ctd.Settings.GetModel<ContentTypeSettings>().Draftable)
                .Select(ctd => ctd.Name);
        }
    }
}
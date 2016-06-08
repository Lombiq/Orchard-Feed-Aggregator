using Lombiq.FeedAggregator.Constants;
using Lombiq.FeedAggregator.Models;
using Orchard.ContentManagement.MetaData;
using Orchard.Core.Contents.Extensions;
using Orchard.Data.Migration;

namespace Lombiq.FeedAggregator.Migrations
{
    public class FeedSyncProfilePartMigrations : DataMigrationImpl
    {
        public int Create()
        {
            ContentDefinitionManager.AlterPartDefinition(
                typeof(FeedSyncProfilePart).Name,
                part => part
                    .Attachable(false)
                    .WithField(FieldNames.FeedUrl, field => field
                        .OfType("TextField")
                        .WithDisplayName("Feed Url")
                        .WithSetting("TextFieldSettings.Required", "True")
                        .WithSetting("TextFieldSettings.Hint", "The Url of the feed to sync."))
                    .WithField(FieldNames.NumberOfItemsToSyncDuringInit, field => field
                        .OfType("NumericField")
                        .WithDisplayName("Number Of Items To Sync During Init")
                        .WithSetting("NumericFieldSettings.Required", "True")
                        .WithSetting("NumericFieldSettings.Hint", "This many items will be creadted during the init. The init happens only once and if the set number is 0, then won't be created any items during init. If the feed isn't pageable, only as many items will be created as many items are available in the first page.")
                        .WithSetting("NumericFieldSettings.Minimum", "0"))
                    .WithField(FieldNames.MinutesBetweenSyncs, field => field
                        .OfType("NumericField")
                        .WithDisplayName("Minutes Between Syncs")
                        .WithSetting("NumericFieldSettings.Required", "True")
                        .WithSetting("NumericFieldSettings.Hint", "A background task will check the feed for new entries.")
                        .WithSetting("NumericFieldSettings.Minimum", "1"))
                    .WithField(FieldNames.Container, field => field
                        .OfType("ContentPickerField")
                        .WithDisplayName("Container")
                        .WithSetting("ContentPickerFieldSettings.Required", "False")
                        .WithSetting("ContentPickerFieldSettings.Hint", "Select a container for the items. E.g. if you want to sync into BlogPosts, then you have to select a container Blog for them.")
                        .WithSetting("ContentPickerFieldSettings.Multiple", "False")));

            ContentDefinitionManager.AlterTypeDefinition(
                ContentTypes.FeedSyncProfile,
                cfg => cfg
                    .WithPart(typeof(FeedSyncProfilePart).Name)
                    .Creatable()
                    .Listable()
                    .Securable()
                    .DisplayedAs("Feed Sync Profile")
                    .WithPart("TitlePart")
                    .WithPart("CommonPart",
                        part => part
                            .WithSetting("OwnerEditorSettings.ShowOwnerEditor", "False")
                            .WithSetting("DateEditorSettings.ShowDateEditor", "False")));

            return 1;
        }
    }
}
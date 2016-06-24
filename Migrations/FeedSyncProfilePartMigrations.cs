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
                nameof(FeedSyncProfilePart),
                part => part
                    .Attachable(false)
                    .WithField(FieldNames.FeedUrl, field => field
                        .OfType("TextField")
                        .WithDisplayName("Feed URL")
                        .WithSetting("TextFieldSettings.Required", "True")
                        .WithSetting("TextFieldSettings.Hint", "The URL of the feed to sync.")
                        .WithSetting("TextFieldSettings.Flavor", "Large"))
                    .WithField(FieldNames.MinutesBetweenSyncs, field => field
                        .OfType("NumericField")
                        .WithDisplayName("Minutes between syncs")
                        .WithSetting("NumericFieldSettings.Required", "True")
                        .WithSetting("NumericFieldSettings.Hint", "A background task will check the feed for new entries.")
                        .WithSetting("NumericFieldSettings.Minimum", "1")
                        .WithSetting("NumericFieldSettings.DefaultValue", "15"))
                    .WithField(FieldNames.Container, field => field
                        .OfType("ContentPickerField")
                        .WithDisplayName("Container")
                        .WithSetting("ContentPickerFieldSettings.Required", "False")
                        .WithSetting("ContentPickerFieldSettings.Hint", "Select a container for the items. E.g. if you want to sync into BlogPosts, then you have to select a container Blog for them.")
                        .WithSetting("ContentPickerFieldSettings.Multiple", "False")));

            ContentDefinitionManager.AlterTypeDefinition(
                ContentTypes.FeedSyncProfile,
                cfg => cfg
                    .WithPart(nameof(FeedSyncProfilePart))
                    .Creatable()
                    .Listable()
                    .Securable()
                    .DisplayedAs("Feed sync profile")
                    .WithPart("TitlePart")
                    .WithPart("CommonPart",
                        part => part
                            .WithSetting("OwnerEditorSettings.ShowOwnerEditor", "False")
                            .WithSetting("DateEditorSettings.ShowDateEditor", "False")));

            return 1;
        }
    }
}
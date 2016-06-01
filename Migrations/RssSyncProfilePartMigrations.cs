﻿using Lombiq.RssReader.Constants;
using Lombiq.RssReader.Models;
using Orchard.ContentManagement.MetaData;
using Orchard.Core.Contents.Extensions;
using Orchard.Data.Migration;

namespace Lombiq.RssReader.Migrations
{
    public class RssSyncProfilePartMigrations : DataMigrationImpl
    {
        public int Create()
        {
            ContentDefinitionManager.AlterPartDefinition(
                typeof(RssSyncProfilePart).Name,
                part => part
                    .Attachable(false)
                    .WithField(FieldNames.RssFeedUrl, field => field
                        .OfType("TextField")
                        .WithDisplayName("Rss Feed Url")
                        .WithSetting("TextFieldSettings.Required", "True")
                        .WithSetting("TextFieldSettings.Hint", "The Url of the RSS feed to sync."))
                    .WithField(FieldNames.RssFeedItemTagName, field => field
                        .OfType("TextField")
                        .WithDisplayName("Rss Feed Item Tag Name")
                        .WithSetting("TextFieldSettings.Required", "True")
                        .WithSetting("TextFieldSettings.Hint", "The xml tag name of the item/entry in the RSS feed."))
                    .WithField(FieldNames.NumberOfItemsToSyncDuringInit, field => field
                        .OfType("NumericField")
                        .WithDisplayName("Number Of Items To Sync During Init")
                        .WithSetting("NumericFieldSettings.Required", "True")
                        .WithSetting("NumericFieldSettings.Hint", "This many items will be creadted during the init. The init happens only once and if the set number is 0, then won't be created any items during init. If the RSS feed isn't pageable, only as many items will be created as many items are available.")
                        .WithSetting("NumericFieldSettings.Minimum", "0"))
                    .WithField(FieldNames.MinutesBetweenSyncs, field => field
                        .OfType("NumericField")
                        .WithDisplayName("Minutes Between Syncs")
                        .WithSetting("NumericFieldSettings.Required", "True")
                        .WithSetting("NumericFieldSettings.Hint", "A background task will check the RSS feed for new entries.")
                        .WithSetting("NumericFieldSettings.Minimum", "1")));

            ContentDefinitionManager.AlterTypeDefinition(
                ContentTypes.RssSyncProfile,
                cfg => cfg
                    .WithPart(typeof(RssSyncProfilePart).Name)
                    .Creatable()
                    .Listable()
                    .Draftable()
                    .Securable()
                    .DisplayedAs("Rss Sync Profile")
                    .WithPart("TitlePart")
                    .WithPart("CommonPart",
                        part => part
                            .WithSetting("OwnerEditorSettings.ShowOwnerEditor", "False")
                            .WithSetting("DateEditorSettings.ShowDateEditor", "False")));

            return 1;
        }
    }
}
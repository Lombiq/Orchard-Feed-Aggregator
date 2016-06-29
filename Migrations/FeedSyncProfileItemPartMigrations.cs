using Lombiq.FeedAggregator.Models;
using Orchard.ContentManagement.MetaData;
using Orchard.Core.Contents.Extensions;
using Orchard.Data.Migration;

namespace Lombiq.FeedAggregator.Migrations
{
    public class FeedSyncProfileItemPartMigrations : DataMigrationImpl
    {
        public int Create()
        {
            SchemaBuilder
                .CreateTable(nameof(FeedSyncProfileItemPartRecord),
                   table => table
                       .ContentPartRecord()
                       .Column<int>("FeedSyncProfileId")
                       .Column<string>("FeedItemId")
                   )
                .AlterTable(nameof(FeedSyncProfileItemPartRecord),
                   table =>
                   {
                       table.CreateIndex("FeedSyncProfileId", "FeedSyncProfileId");
                       table.CreateIndex("FeedItemId", "FeedItemId");
                   });

            ContentDefinitionManager.AlterPartDefinition(nameof(FeedSyncProfileItemPart),
                part => part
                    .Attachable(true)
                    .WithDescription("If you attach this to a type that type will be selectable during FeedSyncProfile creation."));

            return 1;
        }
    }
}
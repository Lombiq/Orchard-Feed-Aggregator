using Lombiq.RssReader.Models;
using Orchard.ContentManagement.MetaData;
using Orchard.Core.Contents.Extensions;
using Orchard.Data.Migration;

namespace Lombiq.RssReader.Migrations
{
    public class RssSyncProfileItemPartMigrations : DataMigrationImpl
    {
        public int Create()
        {
            SchemaBuilder
                .CreateTable(typeof(RssSyncProfileItemPartRecord).Name,
                   table => table
                       .ContentPartRecord()
                       .Column<string>("RssSyncProfileId")
                   )
                .AlterTable(typeof(RssSyncProfileItemPartRecord).Name,
                   table =>
                   {
                       table.CreateIndex("RssSyncProfileId", "RssSyncProfileId");
                   });

            ContentDefinitionManager.AlterPartDefinition(typeof(RssSyncProfileItemPart).Name,
                part => part
                    .Attachable(true)
                    .WithDescription("If you attach this to a type that type will be selectable during RssSyncProfile creation."));

            return 1;
        }
    }
}
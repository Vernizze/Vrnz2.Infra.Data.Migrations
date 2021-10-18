using FluentMigrator;

namespace Vrnz2.Infra.Data.Migrations.Example.migrations
{
    [Migration(202110180001)]
    public class CreateDatabase
        : Migration
    {
        public override void Up()
        {
            //Seed
            Create
                .Table("Seed")
                .WithColumn("Id").AsString().PrimaryKey()
                .WithColumn("CreationDateTime").AsDateTime().NotNullable()
                .WithColumn("Number").AsString().NotNullable().Indexed();
        }

        public override void Down()
        {
            Delete.Table("Seed");
        }
    }
}

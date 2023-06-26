namespace LibrariesBookInventory.Persistence.Migrations
{
    using LibrariesBookInventory.Persistence.Migrations.Scripting;
    using System.Data.Entity.Migrations;
    
    public partial class AddSpGetBooks : DbMigration
    {
        public override void Up()
        {
            Sql(MigrationExtensions.ScriptFromFile("AddSpGetBooks"));
        }
        
        public override void Down()
        {
        }
    }
}

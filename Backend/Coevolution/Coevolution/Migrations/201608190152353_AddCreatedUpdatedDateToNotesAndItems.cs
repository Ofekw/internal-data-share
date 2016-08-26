namespace Coevolution.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCreatedUpdatedDateToNotesAndItems : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Items", "CreatedOn", c => c.DateTime(nullable: false));
            AddColumn("dbo.Items", "UpdatedOn", c => c.DateTime(nullable: false));
            AddColumn("dbo.Notes", "CreatedOn", c => c.DateTime(nullable: false));
            AddColumn("dbo.Notes", "UpdatedOn", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Notes", "UpdatedOn");
            DropColumn("dbo.Notes", "CreatedOn");
            DropColumn("dbo.Items", "UpdatedOn");
            DropColumn("dbo.Items", "CreatedOn");
        }
    }
}

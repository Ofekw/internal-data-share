namespace Coevolution.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Stale : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Items", "Stale", c => c.Boolean(defaultValue: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Items", "Stale");
        }
    }
}

namespace Coevolution.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class unicode : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Items", "Stale", c => c.Boolean());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Items", "Stale");
        }
    }
}

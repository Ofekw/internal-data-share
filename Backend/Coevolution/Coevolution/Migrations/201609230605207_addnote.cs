namespace Coevolution.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addnote : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Items", "Note", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Items", "Note");
        }
    }
}

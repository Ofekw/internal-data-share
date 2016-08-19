namespace Coevolution.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class labelchange : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Labels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Content = c.String(),
                        Item_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Items", t => t.Item_Id)
                .Index(t => t.Item_Id);
            
            CreateTable(
                "dbo.Notes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Content = c.String(),
                        Item_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Items", t => t.Item_Id)
                .Index(t => t.Item_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Notes", "Item_Id", "dbo.Items");
            DropForeignKey("dbo.Labels", "Item_Id", "dbo.Items");
            DropIndex("dbo.Notes", new[] { "Item_Id" });
            DropIndex("dbo.Labels", new[] { "Item_Id" });
            DropTable("dbo.Notes");
            DropTable("dbo.Labels");
        }
    }
}

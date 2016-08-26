namespace Coevolution.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migration2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Items",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Key = c.String(),
                        Date = c.Long(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        UpdatedOn = c.DateTime(nullable: false),
                        Value = c.String(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                        Parent_Id = c.Int(),
                        Node_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Items", t => t.Parent_Id)
                .ForeignKey("dbo.Items", t => t.Node_Id)
                .Index(t => t.Parent_Id)
                .Index(t => t.Node_Id);
            
            CreateTable(
                "dbo.Labels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Content = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Notes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Content = c.String(),
                        CreatedOn = c.DateTime(nullable: false),
                        UpdatedOn = c.DateTime(nullable: false),
                        Item_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Items", t => t.Item_Id)
                .Index(t => t.Item_Id);
            
            CreateTable(
                "dbo.LabelItems",
                c => new
                    {
                        Label_Id = c.Int(nullable: false),
                        Item_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Label_Id, t.Item_Id })
                .ForeignKey("dbo.Labels", t => t.Label_Id, cascadeDelete: true)
                .ForeignKey("dbo.Items", t => t.Item_Id, cascadeDelete: true)
                .Index(t => t.Label_Id)
                .Index(t => t.Item_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Items", "Node_Id", "dbo.Items");
            DropForeignKey("dbo.Items", "Parent_Id", "dbo.Items");
            DropForeignKey("dbo.Notes", "Item_Id", "dbo.Items");
            DropForeignKey("dbo.LabelItems", "Item_Id", "dbo.Items");
            DropForeignKey("dbo.LabelItems", "Label_Id", "dbo.Labels");
            DropIndex("dbo.LabelItems", new[] { "Item_Id" });
            DropIndex("dbo.LabelItems", new[] { "Label_Id" });
            DropIndex("dbo.Notes", new[] { "Item_Id" });
            DropIndex("dbo.Items", new[] { "Node_Id" });
            DropIndex("dbo.Items", new[] { "Parent_Id" });
            DropTable("dbo.LabelItems");
            DropTable("dbo.Notes");
            DropTable("dbo.Labels");
            DropTable("dbo.Items");
        }
    }
}

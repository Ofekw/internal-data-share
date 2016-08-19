namespace Coevolution.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class manytomany : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Labels", "Item_Id", "dbo.Items");
            DropIndex("dbo.Labels", new[] { "Item_Id" });
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
            
            DropColumn("dbo.Labels", "Item_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Labels", "Item_Id", c => c.Int());
            DropForeignKey("dbo.LabelItems", "Item_Id", "dbo.Items");
            DropForeignKey("dbo.LabelItems", "Label_Id", "dbo.Labels");
            DropIndex("dbo.LabelItems", new[] { "Item_Id" });
            DropIndex("dbo.LabelItems", new[] { "Label_Id" });
            DropTable("dbo.LabelItems");
            CreateIndex("dbo.Labels", "Item_Id");
            AddForeignKey("dbo.Labels", "Item_Id", "dbo.Items", "Id");
        }
    }
}

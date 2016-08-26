namespace Coevolution.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class itemnotes : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Notes", "Item_Id", "dbo.Items");
            DropIndex("dbo.Notes", new[] { "Item_Id" });
            CreateTable(
                "dbo.NoteItems",
                c => new
                    {
                        Note_Id = c.Int(nullable: false),
                        Item_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Note_Id, t.Item_Id })
                .ForeignKey("dbo.Notes", t => t.Note_Id, cascadeDelete: true)
                .ForeignKey("dbo.Items", t => t.Item_Id, cascadeDelete: true)
                .Index(t => t.Note_Id)
                .Index(t => t.Item_Id);
            
            DropColumn("dbo.Notes", "Item_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Notes", "Item_Id", c => c.Int());
            DropForeignKey("dbo.NoteItems", "Item_Id", "dbo.Items");
            DropForeignKey("dbo.NoteItems", "Note_Id", "dbo.Notes");
            DropIndex("dbo.NoteItems", new[] { "Item_Id" });
            DropIndex("dbo.NoteItems", new[] { "Note_Id" });
            DropTable("dbo.NoteItems");
            CreateIndex("dbo.Notes", "Item_Id");
            AddForeignKey("dbo.Notes", "Item_Id", "dbo.Items", "Id");
        }
    }
}

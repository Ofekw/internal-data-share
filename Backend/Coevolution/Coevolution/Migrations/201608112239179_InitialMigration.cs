namespace Coevolution.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialMigration : DbMigration
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
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Items", "Node_Id", "dbo.Items");
            DropForeignKey("dbo.Items", "Parent_Id", "dbo.Items");
            DropIndex("dbo.Items", new[] { "Node_Id" });
            DropIndex("dbo.Items", new[] { "Parent_Id" });
            DropTable("dbo.Items");
        }
    }
}

namespace CosStay.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Interests : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Interests",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Category_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.InterestCategories", t => t.Category_Id)
                .Index(t => t.Category_Id);
            
            CreateTable(
                "dbo.InterestCategories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Category = c.String(),
                        Icon = c.String(),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.InterestUsers",
                c => new
                    {
                        Interest_Id = c.Int(nullable: false),
                        User_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.Interest_Id, t.User_Id })
                .ForeignKey("dbo.Interests", t => t.Interest_Id, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id, cascadeDelete: true)
                .Index(t => t.Interest_Id)
                .Index(t => t.User_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.InterestUsers", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.InterestUsers", "Interest_Id", "dbo.Interests");
            DropForeignKey("dbo.Interests", "Category_Id", "dbo.InterestCategories");
            DropIndex("dbo.InterestUsers", new[] { "User_Id" });
            DropIndex("dbo.InterestUsers", new[] { "Interest_Id" });
            DropIndex("dbo.Interests", new[] { "Category_Id" });
            DropTable("dbo.InterestUsers");
            DropTable("dbo.InterestCategories");
            DropTable("dbo.Interests");
        }
    }
}

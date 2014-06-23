namespace CosStay.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ExpandingUserInterests : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.InterestUsers", "Interest_Id", "dbo.Interests");
            DropForeignKey("dbo.InterestUsers", "User_Id", "dbo.AspNetUsers");
            DropIndex("dbo.InterestUsers", new[] { "Interest_Id" });
            DropIndex("dbo.InterestUsers", new[] { "User_Id" });
            CreateTable(
                "dbo.UserInterests",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                        Interest_Id = c.Int(),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Interests", t => t.Interest_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .Index(t => t.Interest_Id)
                .Index(t => t.User_Id);
            
            AddColumn("dbo.Interests", "AllowDescription", c => c.Boolean(nullable: false));
            AddColumn("dbo.Interests", "RequireDescription", c => c.Boolean(nullable: false));
            DropTable("dbo.InterestUsers");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.InterestUsers",
                c => new
                    {
                        Interest_Id = c.Int(nullable: false),
                        User_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.Interest_Id, t.User_Id });
            
            DropForeignKey("dbo.UserInterests", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.UserInterests", "Interest_Id", "dbo.Interests");
            DropIndex("dbo.UserInterests", new[] { "User_Id" });
            DropIndex("dbo.UserInterests", new[] { "Interest_Id" });
            DropColumn("dbo.Interests", "RequireDescription");
            DropColumn("dbo.Interests", "AllowDescription");
            DropTable("dbo.UserInterests");
            CreateIndex("dbo.InterestUsers", "User_Id");
            CreateIndex("dbo.InterestUsers", "Interest_Id");
            AddForeignKey("dbo.InterestUsers", "User_Id", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.InterestUsers", "Interest_Id", "dbo.Interests", "Id", cascadeDelete: true);
        }
    }
}

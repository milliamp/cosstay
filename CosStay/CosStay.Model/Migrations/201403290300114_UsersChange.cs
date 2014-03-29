namespace CosStay.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UsersChange : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.RolePermissions", "Role_RoleId", "dbo.Roles");
            DropForeignKey("dbo.AspNetUsers", "Role_RoleId", "dbo.Roles");
            DropIndex("dbo.AspNetUsers", new[] { "Role_RoleId" });
            DropIndex("dbo.RolePermissions", new[] { "Role_RoleId" });
            AlterColumn("dbo.AspNetUsers", "JoinDate", c => c.DateTimeOffset(nullable: false, precision: 7));
            AlterColumn("dbo.AspNetUsers", "LastSeen", c => c.DateTimeOffset(nullable: false, precision: 7));
            AlterColumn("dbo.AspNetUsers", "DetailsUpdatedDate", c => c.DateTimeOffset(nullable: false, precision: 7));
            DropColumn("dbo.AspNetUsers", "Discriminator");
            DropColumn("dbo.AspNetUsers", "Role_RoleId");
            DropColumn("dbo.RolePermissions", "Role_RoleId");
            DropTable("dbo.Roles");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        RoleId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.RoleId);
            
            AddColumn("dbo.RolePermissions", "Role_RoleId", c => c.Int());
            AddColumn("dbo.AspNetUsers", "Role_RoleId", c => c.Int());
            AddColumn("dbo.AspNetUsers", "Discriminator", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.AspNetUsers", "DetailsUpdatedDate", c => c.DateTimeOffset(precision: 7));
            AlterColumn("dbo.AspNetUsers", "LastSeen", c => c.DateTimeOffset(precision: 7));
            AlterColumn("dbo.AspNetUsers", "JoinDate", c => c.DateTimeOffset(precision: 7));
            CreateIndex("dbo.RolePermissions", "Role_RoleId");
            CreateIndex("dbo.AspNetUsers", "Role_RoleId");
            AddForeignKey("dbo.AspNetUsers", "Role_RoleId", "dbo.Roles", "RoleId");
            AddForeignKey("dbo.RolePermissions", "Role_RoleId", "dbo.Roles", "RoleId");
        }
    }
}

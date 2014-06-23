namespace CosStay.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddResidents : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ResidentInterests",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                        Interest_Id = c.Int(),
                        Resident_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Interests", t => t.Interest_Id)
                .ForeignKey("dbo.Residents", t => t.Resident_Id)
                .Index(t => t.Interest_Id)
                .Index(t => t.Resident_Id);
            
            CreateTable(
                "dbo.Residents",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ResidentImage = c.Int(nullable: false),
                        Order = c.Int(nullable: false),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.AspNetUsers", "Resident_Id", c => c.Int());
            AddColumn("dbo.AccomodationVenues", "AddressVerified", c => c.Boolean(nullable: false));
            AddColumn("dbo.AccomodationVenues", "CoverImage_PhotoId", c => c.Guid());
            CreateIndex("dbo.AspNetUsers", "Resident_Id");
            CreateIndex("dbo.AccomodationVenues", "CoverImage_PhotoId");
            AddForeignKey("dbo.AspNetUsers", "Resident_Id", "dbo.Residents", "Id");
            AddForeignKey("dbo.AccomodationVenues", "CoverImage_PhotoId", "dbo.Photos", "PhotoId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AccomodationVenues", "CoverImage_PhotoId", "dbo.Photos");
            DropForeignKey("dbo.AspNetUsers", "Resident_Id", "dbo.Residents");
            DropForeignKey("dbo.ResidentInterests", "Resident_Id", "dbo.Residents");
            DropForeignKey("dbo.ResidentInterests", "Interest_Id", "dbo.Interests");
            DropIndex("dbo.AccomodationVenues", new[] { "CoverImage_PhotoId" });
            DropIndex("dbo.ResidentInterests", new[] { "Resident_Id" });
            DropIndex("dbo.ResidentInterests", new[] { "Interest_Id" });
            DropIndex("dbo.AspNetUsers", new[] { "Resident_Id" });
            DropColumn("dbo.AccomodationVenues", "CoverImage_PhotoId");
            DropColumn("dbo.AccomodationVenues", "AddressVerified");
            DropColumn("dbo.AspNetUsers", "Resident_Id");
            DropTable("dbo.Residents");
            DropTable("dbo.ResidentInterests");
        }
    }
}

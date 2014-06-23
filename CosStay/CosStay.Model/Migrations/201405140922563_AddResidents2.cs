namespace CosStay.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddResidents2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Residents", "AccomodationVenue_Id", c => c.Int());
            CreateIndex("dbo.Residents", "AccomodationVenue_Id");
            AddForeignKey("dbo.Residents", "AccomodationVenue_Id", "dbo.AccomodationVenues", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Residents", "AccomodationVenue_Id", "dbo.AccomodationVenues");
            DropIndex("dbo.Residents", new[] { "AccomodationVenue_Id" });
            DropColumn("dbo.Residents", "AccomodationVenue_Id");
        }
    }
}

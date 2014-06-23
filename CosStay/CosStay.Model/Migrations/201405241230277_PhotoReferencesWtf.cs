namespace CosStay.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PhotoReferencesWtf : DbMigration
    {
        public override void Up()
        {
            //CreateIndex("dbo.Photos", "AccomodationVenueId");
            //AddForeignKey("dbo.Photos", "AccomodationVenueId", "dbo.AccomodationVenues", "Id");
        }
        
        public override void Down()
        {
            //DropForeignKey("dbo.Photos", "AccomodationVenueId", "dbo.AccomodationVenues");
            //DropIndex("dbo.Photos", new[] { "AccomodationVenueId" });
        }
    }
}

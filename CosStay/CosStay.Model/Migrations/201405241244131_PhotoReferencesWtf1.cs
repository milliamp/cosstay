namespace CosStay.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PhotoReferencesWtf1 : DbMigration
    {
        public override void Up()
        {
            //DropIndex("dbo.Photos", new[] { "AccomodationVenue_Id" });
            //DropColumn("dbo.Photos", "AccomodationVenueId");
            //RenameColumn(table: "dbo.Photos", name: "AccomodationVenue_Id", newName: "AccomodationVenueId");
        }
        
        public override void Down()
        {
            //RenameColumn(table: "dbo.Photos", name: "AccomodationVenueId", newName: "AccomodationVenue_Id");
            //AddColumn("dbo.Photos", "AccomodationVenueId", c => c.Int());
            //CreateIndex("dbo.Photos", "AccomodationVenue_Id");
        }
    }
}

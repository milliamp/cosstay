namespace CosStay.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DoubleLatLng : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Locations", "LatLng_Lat", c => c.Double());
            AlterColumn("dbo.Locations", "LatLng_Lng", c => c.Double());
            AlterColumn("dbo.Venues", "LatLng_Lat", c => c.Double());
            AlterColumn("dbo.Venues", "LatLng_Lng", c => c.Double());
            AlterColumn("dbo.AccomodationVenues", "LatLng_Lat", c => c.Double());
            AlterColumn("dbo.AccomodationVenues", "LatLng_Lng", c => c.Double());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AccomodationVenues", "LatLng_Lng", c => c.Single());
            AlterColumn("dbo.AccomodationVenues", "LatLng_Lat", c => c.Single());
            AlterColumn("dbo.Venues", "LatLng_Lng", c => c.Single());
            AlterColumn("dbo.Venues", "LatLng_Lat", c => c.Single());
            AlterColumn("dbo.Locations", "LatLng_Lng", c => c.Single());
            AlterColumn("dbo.Locations", "LatLng_Lat", c => c.Single());
        }
    }
}

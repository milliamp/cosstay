namespace CosStay.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Deletables : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AccomodationBedAvailabilityNights", "IsDeleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.Beds", "IsDeleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.Photos", "IsDeleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.Locations", "IsDeleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.Venues", "IsDeleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.AccomodationRoomFeatures", "IsDeleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.AccomodationRooms", "IsDeleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.AccomodationVenueFeatures", "IsDeleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.AccomodationVenues", "IsDeleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.BookingRequests", "IsDeleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.EventInstances", "IsDeleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.Events", "IsDeleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.UserEventAttendances", "IsDeleted", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserEventAttendances", "IsDeleted");
            DropColumn("dbo.Events", "IsDeleted");
            DropColumn("dbo.EventInstances", "IsDeleted");
            DropColumn("dbo.BookingRequests", "IsDeleted");
            DropColumn("dbo.AccomodationVenues", "IsDeleted");
            DropColumn("dbo.AccomodationVenueFeatures", "IsDeleted");
            DropColumn("dbo.AccomodationRooms", "IsDeleted");
            DropColumn("dbo.AccomodationRoomFeatures", "IsDeleted");
            DropColumn("dbo.Venues", "IsDeleted");
            DropColumn("dbo.Locations", "IsDeleted");
            DropColumn("dbo.Photos", "IsDeleted");
            DropColumn("dbo.Beds", "IsDeleted");
            DropColumn("dbo.AccomodationBedAvailabilityNights", "IsDeleted");
        }
    }
}

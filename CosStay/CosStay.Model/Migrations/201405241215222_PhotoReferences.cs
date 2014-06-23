namespace CosStay.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PhotoReferences : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Photos", name: "Bed_Id", newName: "BedId");
            RenameColumn(table: "dbo.Photos", name: "Location_Id", newName: "LocationId");
            RenameColumn(table: "dbo.Photos", name: "Venue_Id", newName: "VenueId");
            RenameColumn(table: "dbo.Photos", name: "EventInstance_Id", newName: "EventInstanceId");
            RenameColumn(table: "dbo.Photos", name: "Event_Id", newName: "EventId");
            RenameColumn(table: "dbo.Photos", name: "AccomodationRoom_Id", newName: "AccomodationRoomId");
            RenameColumn(table: "dbo.Photos", name: "AccomodationVenue_Id", newName: "AccomodationVenueId");
            RenameIndex(table: "dbo.Photos", name: "IX_Bed_Id", newName: "IX_BedId");
            RenameIndex(table: "dbo.Photos", name: "IX_Location_Id", newName: "IX_LocationId");
            RenameIndex(table: "dbo.Photos", name: "IX_Event_Id", newName: "IX_EventId");
            RenameIndex(table: "dbo.Photos", name: "IX_EventInstance_Id", newName: "IX_EventInstanceId");
            RenameIndex(table: "dbo.Photos", name: "IX_AccomodationRoom_Id", newName: "IX_AccomodationRoomId");
            RenameIndex(table: "dbo.Photos", name: "IX_Venue_Id", newName: "IX_VenueId");
            RenameIndex(table: "dbo.Photos", name: "IX_AccomodationVenue_Id", newName: "IX_AccomodationVenueId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Photos", name: "IX_AccomodationVenueId", newName: "IX_AccomodationVenue_Id");
            RenameIndex(table: "dbo.Photos", name: "IX_VenueId", newName: "IX_Venue_Id");
            RenameIndex(table: "dbo.Photos", name: "IX_AccomodationRoomId", newName: "IX_AccomodationRoom_Id");
            RenameIndex(table: "dbo.Photos", name: "IX_EventInstanceId", newName: "IX_EventInstance_Id");
            RenameIndex(table: "dbo.Photos", name: "IX_EventId", newName: "IX_Event_Id");
            RenameIndex(table: "dbo.Photos", name: "IX_LocationId", newName: "IX_Location_Id");
            RenameIndex(table: "dbo.Photos", name: "IX_BedId", newName: "IX_Bed_Id");
            RenameColumn(table: "dbo.Photos", name: "AccomodationRoomId", newName: "AccomodationRoom_Id");
            RenameColumn(table: "dbo.Photos", name: "AccomodationVenueId", newName: "AccomodationVenue_Id");
            RenameColumn(table: "dbo.Photos", name: "EventId", newName: "Event_Id");
            RenameColumn(table: "dbo.Photos", name: "EventInstanceId", newName: "EventInstance_Id");
            RenameColumn(table: "dbo.Photos", name: "VenueId", newName: "Venue_Id");
            RenameColumn(table: "dbo.Photos", name: "LocationId", newName: "Location_Id");
            RenameColumn(table: "dbo.Photos", name: "BedId", newName: "Bed_Id");
        }
    }
}

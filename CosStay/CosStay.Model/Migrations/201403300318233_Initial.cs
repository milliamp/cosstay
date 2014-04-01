namespace CosStay.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AccomodationBedAvailabilityNights",
                c => new
                    {
                        AccomodationBedAvailabilityNightId = c.Int(nullable: false, identity: true),
                        Night = c.DateTimeOffset(nullable: false, precision: 7),
                        BedStatus = c.Int(nullable: false),
                        Bed_BedId = c.Int(),
                    })
                .PrimaryKey(t => t.AccomodationBedAvailabilityNightId)
                .ForeignKey("dbo.Beds", t => t.Bed_BedId)
                .Index(t => t.Bed_BedId);
            
            CreateTable(
                "dbo.Beds",
                c => new
                    {
                        BedId = c.Int(nullable: false, identity: true),
                        Size = c.Int(nullable: false),
                        DateAdded = c.DateTimeOffset(nullable: false, precision: 7),
                        Type_BedTypeId = c.Int(),
                        AccomodationRoom_AccomodationRoomId = c.Int(),
                    })
                .PrimaryKey(t => t.BedId)
                .ForeignKey("dbo.BedTypes", t => t.Type_BedTypeId)
                .ForeignKey("dbo.AccomodationRooms", t => t.AccomodationRoom_AccomodationRoomId)
                .Index(t => t.Type_BedTypeId)
                .Index(t => t.AccomodationRoom_AccomodationRoomId);
            
            CreateTable(
                "dbo.Photos",
                c => new
                    {
                        PhotoId = c.Guid(nullable: false),
                        Caption = c.String(),
                        DateAdded = c.DateTimeOffset(nullable: false, precision: 7),
                        Location_LocationId = c.Int(),
                        Venue_VenueId = c.Int(),
                        Owner_Id = c.String(maxLength: 128),
                        Bed_BedId = c.Int(),
                        AccomodationVenue_AccomodationVenueId = c.Int(),
                        AccomodationRoom_AccomodationRoomId = c.Int(),
                        Event_EventId = c.Int(),
                        EventInstance_EventInstanceId = c.Int(),
                    })
                .PrimaryKey(t => t.PhotoId)
                .ForeignKey("dbo.Locations", t => t.Location_LocationId)
                .ForeignKey("dbo.Venues", t => t.Venue_VenueId)
                .ForeignKey("dbo.AspNetUsers", t => t.Owner_Id)
                .ForeignKey("dbo.Beds", t => t.Bed_BedId)
                .ForeignKey("dbo.AccomodationVenues", t => t.AccomodationVenue_AccomodationVenueId)
                .ForeignKey("dbo.AccomodationRooms", t => t.AccomodationRoom_AccomodationRoomId)
                .ForeignKey("dbo.Events", t => t.Event_EventId)
                .ForeignKey("dbo.EventInstances", t => t.EventInstance_EventInstanceId)
                .Index(t => t.Location_LocationId)
                .Index(t => t.Venue_VenueId)
                .Index(t => t.Owner_Id)
                .Index(t => t.Bed_BedId)
                .Index(t => t.AccomodationVenue_AccomodationVenueId)
                .Index(t => t.AccomodationRoom_AccomodationRoomId)
                .Index(t => t.Event_EventId)
                .Index(t => t.EventInstance_EventInstanceId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                        JoinDate = c.DateTimeOffset(nullable: false, precision: 7),
                        LastSeen = c.DateTimeOffset(nullable: false, precision: 7),
                        DetailsUpdatedDate = c.DateTimeOffset(nullable: false, precision: 7),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                        Location_LocationId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Locations", t => t.Location_LocationId)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex")
                .Index(t => t.Location_LocationId);
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.ContactMethods",
                c => new
                    {
                        ContactMethodId = c.Int(nullable: false, identity: true),
                        Value = c.String(),
                        DateAdded = c.DateTimeOffset(nullable: false, precision: 7),
                        Order = c.Int(nullable: false),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ContactMethodId)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.Locations",
                c => new
                    {
                        LocationId = c.Int(nullable: false, identity: true),
                        Lat = c.Single(),
                        Lng = c.Single(),
                        Name = c.String(),
                        Country_CountryId = c.Int(),
                    })
                .PrimaryKey(t => t.LocationId)
                .ForeignKey("dbo.Countries", t => t.Country_CountryId)
                .Index(t => t.Country_CountryId);
            
            CreateTable(
                "dbo.Venues",
                c => new
                    {
                        VenueId = c.Int(nullable: false, identity: true),
                        Lat = c.Single(),
                        Lng = c.Single(),
                        Address = c.String(),
                        Name = c.String(),
                        Location_LocationId = c.Int(),
                    })
                .PrimaryKey(t => t.VenueId)
                .ForeignKey("dbo.Locations", t => t.Location_LocationId)
                .Index(t => t.Location_LocationId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.BedTypes",
                c => new
                    {
                        BedTypeId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.BedTypeId);
            
            CreateTable(
                "dbo.AccomodationRoomFeatures",
                c => new
                    {
                        AccomodationRoomFeatureId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        AccomodationRoom_AccomodationRoomId = c.Int(),
                    })
                .PrimaryKey(t => t.AccomodationRoomFeatureId)
                .ForeignKey("dbo.AccomodationRooms", t => t.AccomodationRoom_AccomodationRoomId)
                .Index(t => t.AccomodationRoom_AccomodationRoomId);
            
            CreateTable(
                "dbo.AccomodationRooms",
                c => new
                    {
                        AccomodationRoomId = c.Int(nullable: false, identity: true),
                        DateAdded = c.DateTimeOffset(nullable: false, precision: 7),
                        AccomodationVenue_AccomodationVenueId = c.Int(),
                    })
                .PrimaryKey(t => t.AccomodationRoomId)
                .ForeignKey("dbo.AccomodationVenues", t => t.AccomodationVenue_AccomodationVenueId)
                .Index(t => t.AccomodationVenue_AccomodationVenueId);
            
            CreateTable(
                "dbo.AccomodationVenues",
                c => new
                    {
                        AccomodationVenueId = c.Int(nullable: false, identity: true),
                        Lat = c.Single(),
                        Lng = c.Single(),
                        Address = c.String(),
                        DateAdded = c.DateTimeOffset(nullable: false, precision: 7),
                        AllowsBedSharing = c.Boolean(nullable: false),
                        AllowsMixedRooms = c.Boolean(nullable: false),
                        Name = c.String(),
                        Location_LocationId = c.Int(),
                        Owner_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.AccomodationVenueId)
                .ForeignKey("dbo.Locations", t => t.Location_LocationId)
                .ForeignKey("dbo.AspNetUsers", t => t.Owner_Id)
                .Index(t => t.Location_LocationId)
                .Index(t => t.Owner_Id);
            
            CreateTable(
                "dbo.AccomodationVenueFeatures",
                c => new
                    {
                        AccomodationVenueFeatureId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        AccomodationVenue_AccomodationVenueId = c.Int(),
                    })
                .PrimaryKey(t => t.AccomodationVenueFeatureId)
                .ForeignKey("dbo.AccomodationVenues", t => t.AccomodationVenue_AccomodationVenueId)
                .Index(t => t.AccomodationVenue_AccomodationVenueId);
            
            CreateTable(
                "dbo.AccomodationVenuePermissions",
                c => new
                    {
                        AccomodationVenuePermissionId = c.Int(nullable: false, identity: true),
                        Role = c.Int(nullable: false),
                        Venue_AccomodationVenueId = c.Int(),
                    })
                .PrimaryKey(t => t.AccomodationVenuePermissionId)
                .ForeignKey("dbo.AccomodationVenues", t => t.Venue_AccomodationVenueId)
                .Index(t => t.Venue_AccomodationVenueId);
            
            CreateTable(
                "dbo.AdministrationPermissions",
                c => new
                    {
                        AdministrationPermissionId = c.Int(nullable: false, identity: true),
                        SecurableEntity = c.String(),
                    })
                .PrimaryKey(t => t.AdministrationPermissionId);
            
            CreateTable(
                "dbo.Audits",
                c => new
                    {
                        AuditId = c.Int(nullable: false, identity: true),
                        IP = c.String(),
                        UserAgent = c.String(),
                        EventDate = c.DateTimeOffset(nullable: false, precision: 7),
                        AuditEvent = c.String(),
                        Data = c.String(),
                        InitiatingUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.AuditId)
                .ForeignKey("dbo.AspNetUsers", t => t.InitiatingUser_Id)
                .Index(t => t.InitiatingUser_Id);
            
            CreateTable(
                "dbo.BookingRequests",
                c => new
                    {
                        BookingRequestId = c.Int(nullable: false, identity: true),
                        Guests = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                        Bed_BedId = c.Int(),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.BookingRequestId)
                .ForeignKey("dbo.Beds", t => t.Bed_BedId)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .Index(t => t.Bed_BedId)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.ContentPages",
                c => new
                    {
                        ContentPageId = c.Int(nullable: false, identity: true),
                        Uri = c.String(),
                    })
                .PrimaryKey(t => t.ContentPageId);
            
            CreateTable(
                "dbo.ContentPageVersions",
                c => new
                    {
                        ContentPageVersionId = c.Int(nullable: false, identity: true),
                        Version = c.Int(nullable: false),
                        Title = c.String(),
                        MarkdownContent = c.String(),
                        Status = c.Int(nullable: false),
                        CreatedDate = c.DateTimeOffset(nullable: false, precision: 7),
                        PublishDate = c.DateTimeOffset(nullable: false, precision: 7),
                        Page_ContentPageId = c.Int(),
                    })
                .PrimaryKey(t => t.ContentPageVersionId)
                .ForeignKey("dbo.ContentPages", t => t.Page_ContentPageId)
                .Index(t => t.Page_ContentPageId);
            
            CreateTable(
                "dbo.Countries",
                c => new
                    {
                        CountryId = c.Int(nullable: false, identity: true),
                        ShortName = c.String(),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.CountryId);
            
            CreateTable(
                "dbo.EventInstances",
                c => new
                    {
                        EventInstanceId = c.Int(nullable: false, identity: true),
                        Url = c.String(),
                        FacebookEventId = c.String(),
                        Description = c.String(),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        DateUpdated = c.DateTimeOffset(nullable: false, precision: 7),
                        Name = c.String(),
                        Event_EventId = c.Int(),
                        Venue_VenueId = c.Int(),
                    })
                .PrimaryKey(t => t.EventInstanceId)
                .ForeignKey("dbo.Events", t => t.Event_EventId)
                .ForeignKey("dbo.Venues", t => t.Venue_VenueId)
                .Index(t => t.Event_EventId)
                .Index(t => t.Venue_VenueId);
            
            CreateTable(
                "dbo.Events",
                c => new
                    {
                        EventId = c.Int(nullable: false, identity: true),
                        Url = c.String(),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.EventId);
            
            CreateTable(
                "dbo.RolePermissions",
                c => new
                    {
                        RolePermissionId = c.Int(nullable: false, identity: true),
                        PermissionLevel = c.Int(nullable: false),
                        AdministrationPermission_AdministrationPermissionId = c.Int(),
                    })
                .PrimaryKey(t => t.RolePermissionId)
                .ForeignKey("dbo.AdministrationPermissions", t => t.AdministrationPermission_AdministrationPermissionId)
                .Index(t => t.AdministrationPermission_AdministrationPermissionId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.SeedDatas",
                c => new
                    {
                        SeedDataId = c.Int(nullable: false, identity: true),
                        Category = c.String(),
                        Version = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.SeedDataId);
            
            CreateTable(
                "dbo.UserEventAttendances",
                c => new
                    {
                        UserEventAttendanceId = c.Int(nullable: false, identity: true),
                        AutomaticallyAdded = c.Boolean(nullable: false),
                        DateAdded = c.DateTimeOffset(nullable: false, precision: 7),
                        EventInstance_EventInstanceId = c.Int(),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.UserEventAttendanceId)
                .ForeignKey("dbo.EventInstances", t => t.EventInstance_EventInstanceId)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .Index(t => t.EventInstance_EventInstanceId)
                .Index(t => t.User_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserEventAttendances", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.UserEventAttendances", "EventInstance_EventInstanceId", "dbo.EventInstances");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.RolePermissions", "AdministrationPermission_AdministrationPermissionId", "dbo.AdministrationPermissions");
            DropForeignKey("dbo.EventInstances", "Venue_VenueId", "dbo.Venues");
            DropForeignKey("dbo.Photos", "EventInstance_EventInstanceId", "dbo.EventInstances");
            DropForeignKey("dbo.Photos", "Event_EventId", "dbo.Events");
            DropForeignKey("dbo.EventInstances", "Event_EventId", "dbo.Events");
            DropForeignKey("dbo.Locations", "Country_CountryId", "dbo.Countries");
            DropForeignKey("dbo.ContentPageVersions", "Page_ContentPageId", "dbo.ContentPages");
            DropForeignKey("dbo.BookingRequests", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.BookingRequests", "Bed_BedId", "dbo.Beds");
            DropForeignKey("dbo.Audits", "InitiatingUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AccomodationVenuePermissions", "Venue_AccomodationVenueId", "dbo.AccomodationVenues");
            DropForeignKey("dbo.Photos", "AccomodationRoom_AccomodationRoomId", "dbo.AccomodationRooms");
            DropForeignKey("dbo.AccomodationRoomFeatures", "AccomodationRoom_AccomodationRoomId", "dbo.AccomodationRooms");
            DropForeignKey("dbo.Beds", "AccomodationRoom_AccomodationRoomId", "dbo.AccomodationRooms");
            DropForeignKey("dbo.AccomodationRooms", "AccomodationVenue_AccomodationVenueId", "dbo.AccomodationVenues");
            DropForeignKey("dbo.Photos", "AccomodationVenue_AccomodationVenueId", "dbo.AccomodationVenues");
            DropForeignKey("dbo.AccomodationVenues", "Owner_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AccomodationVenues", "Location_LocationId", "dbo.Locations");
            DropForeignKey("dbo.AccomodationVenueFeatures", "AccomodationVenue_AccomodationVenueId", "dbo.AccomodationVenues");
            DropForeignKey("dbo.AccomodationBedAvailabilityNights", "Bed_BedId", "dbo.Beds");
            DropForeignKey("dbo.Beds", "Type_BedTypeId", "dbo.BedTypes");
            DropForeignKey("dbo.Photos", "Bed_BedId", "dbo.Beds");
            DropForeignKey("dbo.Photos", "Owner_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "Location_LocationId", "dbo.Locations");
            DropForeignKey("dbo.Photos", "Venue_VenueId", "dbo.Venues");
            DropForeignKey("dbo.Venues", "Location_LocationId", "dbo.Locations");
            DropForeignKey("dbo.Photos", "Location_LocationId", "dbo.Locations");
            DropForeignKey("dbo.ContactMethods", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.UserEventAttendances", new[] { "User_Id" });
            DropIndex("dbo.UserEventAttendances", new[] { "EventInstance_EventInstanceId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.RolePermissions", new[] { "AdministrationPermission_AdministrationPermissionId" });
            DropIndex("dbo.EventInstances", new[] { "Venue_VenueId" });
            DropIndex("dbo.EventInstances", new[] { "Event_EventId" });
            DropIndex("dbo.ContentPageVersions", new[] { "Page_ContentPageId" });
            DropIndex("dbo.BookingRequests", new[] { "User_Id" });
            DropIndex("dbo.BookingRequests", new[] { "Bed_BedId" });
            DropIndex("dbo.Audits", new[] { "InitiatingUser_Id" });
            DropIndex("dbo.AccomodationVenuePermissions", new[] { "Venue_AccomodationVenueId" });
            DropIndex("dbo.AccomodationVenueFeatures", new[] { "AccomodationVenue_AccomodationVenueId" });
            DropIndex("dbo.AccomodationVenues", new[] { "Owner_Id" });
            DropIndex("dbo.AccomodationVenues", new[] { "Location_LocationId" });
            DropIndex("dbo.AccomodationRooms", new[] { "AccomodationVenue_AccomodationVenueId" });
            DropIndex("dbo.AccomodationRoomFeatures", new[] { "AccomodationRoom_AccomodationRoomId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.Venues", new[] { "Location_LocationId" });
            DropIndex("dbo.Locations", new[] { "Country_CountryId" });
            DropIndex("dbo.ContactMethods", new[] { "User_Id" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", new[] { "Location_LocationId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.Photos", new[] { "EventInstance_EventInstanceId" });
            DropIndex("dbo.Photos", new[] { "Event_EventId" });
            DropIndex("dbo.Photos", new[] { "AccomodationRoom_AccomodationRoomId" });
            DropIndex("dbo.Photos", new[] { "AccomodationVenue_AccomodationVenueId" });
            DropIndex("dbo.Photos", new[] { "Bed_BedId" });
            DropIndex("dbo.Photos", new[] { "Owner_Id" });
            DropIndex("dbo.Photos", new[] { "Venue_VenueId" });
            DropIndex("dbo.Photos", new[] { "Location_LocationId" });
            DropIndex("dbo.Beds", new[] { "AccomodationRoom_AccomodationRoomId" });
            DropIndex("dbo.Beds", new[] { "Type_BedTypeId" });
            DropIndex("dbo.AccomodationBedAvailabilityNights", new[] { "Bed_BedId" });
            DropTable("dbo.UserEventAttendances");
            DropTable("dbo.SeedDatas");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.RolePermissions");
            DropTable("dbo.Events");
            DropTable("dbo.EventInstances");
            DropTable("dbo.Countries");
            DropTable("dbo.ContentPageVersions");
            DropTable("dbo.ContentPages");
            DropTable("dbo.BookingRequests");
            DropTable("dbo.Audits");
            DropTable("dbo.AdministrationPermissions");
            DropTable("dbo.AccomodationVenuePermissions");
            DropTable("dbo.AccomodationVenueFeatures");
            DropTable("dbo.AccomodationVenues");
            DropTable("dbo.AccomodationRooms");
            DropTable("dbo.AccomodationRoomFeatures");
            DropTable("dbo.BedTypes");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.Venues");
            DropTable("dbo.Locations");
            DropTable("dbo.ContactMethods");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Photos");
            DropTable("dbo.Beds");
            DropTable("dbo.AccomodationBedAvailabilityNights");
        }
    }
}

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
                        Id = c.Int(nullable: false, identity: true),
                        Night = c.DateTimeOffset(nullable: false, precision: 7),
                        BedStatus = c.Int(nullable: false),
                        Bed_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Beds", t => t.Bed_Id)
                .Index(t => t.Bed_Id);
            
            CreateTable(
                "dbo.Beds",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DateAdded = c.DateTimeOffset(nullable: false, precision: 7),
                        BedSize_Id = c.Int(),
                        BedType_Id = c.Int(),
                        AccomodationRoom_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BedSizes", t => t.BedSize_Id)
                .ForeignKey("dbo.BedTypes", t => t.BedType_Id)
                .ForeignKey("dbo.AccomodationRooms", t => t.AccomodationRoom_Id)
                .Index(t => t.BedSize_Id)
                .Index(t => t.BedType_Id)
                .Index(t => t.AccomodationRoom_Id);
            
            CreateTable(
                "dbo.BedSizes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.BedTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Photos",
                c => new
                    {
                        PhotoId = c.Guid(nullable: false),
                        Caption = c.String(),
                        DateAdded = c.DateTimeOffset(nullable: false, precision: 7),
                        Location_Id = c.Int(),
                        Venue_Id = c.Int(),
                        Owner_Id = c.String(maxLength: 128),
                        Bed_Id = c.Int(),
                        AccomodationRoom_Id = c.Int(),
                        AccomodationVenue_Id = c.Int(),
                        Event_Id = c.Int(),
                        EventInstance_Id = c.Int(),
                    })
                .PrimaryKey(t => t.PhotoId)
                .ForeignKey("dbo.Locations", t => t.Location_Id)
                .ForeignKey("dbo.Venues", t => t.Venue_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Owner_Id)
                .ForeignKey("dbo.Beds", t => t.Bed_Id)
                .ForeignKey("dbo.AccomodationRooms", t => t.AccomodationRoom_Id)
                .ForeignKey("dbo.AccomodationVenues", t => t.AccomodationVenue_Id)
                .ForeignKey("dbo.Events", t => t.Event_Id)
                .ForeignKey("dbo.EventInstances", t => t.EventInstance_Id)
                .Index(t => t.Location_Id)
                .Index(t => t.Venue_Id)
                .Index(t => t.Owner_Id)
                .Index(t => t.Bed_Id)
                .Index(t => t.AccomodationRoom_Id)
                .Index(t => t.AccomodationVenue_Id)
                .Index(t => t.Event_Id)
                .Index(t => t.EventInstance_Id);
            
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
                        Location_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Locations", t => t.Location_Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex")
                .Index(t => t.Location_Id);
            
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
                        Id = c.Int(nullable: false, identity: true),
                        Value = c.String(),
                        DateAdded = c.DateTimeOffset(nullable: false, precision: 7),
                        Order = c.Int(nullable: false),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.Locations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LatLng_Lat = c.Double(),
                        LatLng_Lng = c.Double(),
                        Name = c.String(),
                        Country_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Countries", t => t.Country_Id)
                .Index(t => t.Country_Id);
            
            CreateTable(
                "dbo.Venues",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LatLng_Lat = c.Double(),
                        LatLng_Lng = c.Double(),
                        Address = c.String(),
                        Name = c.String(),
                        Location_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Locations", t => t.Location_Id)
                .Index(t => t.Location_Id);
            
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
                "dbo.AccomodationRoomFeatures",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        AccomodationRoom_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AccomodationRooms", t => t.AccomodationRoom_Id)
                .Index(t => t.AccomodationRoom_Id);
            
            CreateTable(
                "dbo.AccomodationRooms",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DateAdded = c.DateTimeOffset(nullable: false, precision: 7),
                        AccomodationVenue_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AccomodationVenues", t => t.AccomodationVenue_Id)
                .Index(t => t.AccomodationVenue_Id);
            
            CreateTable(
                "dbo.AccomodationVenueFeatures",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        AccomodationVenue_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AccomodationVenues", t => t.AccomodationVenue_Id)
                .Index(t => t.AccomodationVenue_Id);
            
            CreateTable(
                "dbo.AccomodationVenuePermissions",
                c => new
                    {
                        AccomodationVenuePermissionId = c.Int(nullable: false, identity: true),
                        Role = c.Int(nullable: false),
                        Venue_Id = c.Int(),
                    })
                .PrimaryKey(t => t.AccomodationVenuePermissionId)
                .ForeignKey("dbo.AccomodationVenues", t => t.Venue_Id)
                .Index(t => t.Venue_Id);
            
            CreateTable(
                "dbo.AccomodationVenues",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LatLng_Lat = c.Double(),
                        LatLng_Lng = c.Double(),
                        Address = c.String(),
                        DateAdded = c.DateTimeOffset(nullable: false, precision: 7),
                        AllowsBedSharing = c.Boolean(nullable: false),
                        AllowsMixedRooms = c.Boolean(nullable: false),
                        Name = c.String(),
                        Location_Id = c.Int(),
                        Owner_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Locations", t => t.Location_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Owner_Id)
                .Index(t => t.Location_Id)
                .Index(t => t.Owner_Id);
            
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
                        Id = c.Int(nullable: false, identity: true),
                        IP = c.String(),
                        UserAgent = c.String(),
                        EventDate = c.DateTimeOffset(nullable: false, precision: 7),
                        AuditEvent = c.String(),
                        Data = c.String(),
                        InitiatingUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.InitiatingUser_Id)
                .Index(t => t.InitiatingUser_Id);
            
            CreateTable(
                "dbo.BookingRequests",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Guests = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                        Bed_Id = c.Int(),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Beds", t => t.Bed_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .Index(t => t.Bed_Id)
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
                        Id = c.Int(nullable: false, identity: true),
                        ShortName = c.String(),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.EventInstances",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Url = c.String(),
                        FacebookEventId = c.String(),
                        Description = c.String(),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        DateUpdated = c.DateTimeOffset(nullable: false, precision: 7),
                        Name = c.String(),
                        Event_Id = c.Int(),
                        Venue_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Events", t => t.Event_Id)
                .ForeignKey("dbo.Venues", t => t.Venue_Id)
                .Index(t => t.Event_Id)
                .Index(t => t.Venue_Id);
            
            CreateTable(
                "dbo.Events",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Url = c.String(),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
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
                        Id = c.Int(nullable: false, identity: true),
                        AutomaticallyAdded = c.Boolean(nullable: false),
                        DateAdded = c.DateTimeOffset(nullable: false, precision: 7),
                        EventInstance_Id = c.Int(),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.EventInstances", t => t.EventInstance_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .Index(t => t.EventInstance_Id)
                .Index(t => t.User_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserEventAttendances", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.UserEventAttendances", "EventInstance_Id", "dbo.EventInstances");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.RolePermissions", "AdministrationPermission_AdministrationPermissionId", "dbo.AdministrationPermissions");
            DropForeignKey("dbo.EventInstances", "Venue_Id", "dbo.Venues");
            DropForeignKey("dbo.Photos", "EventInstance_Id", "dbo.EventInstances");
            DropForeignKey("dbo.Photos", "Event_Id", "dbo.Events");
            DropForeignKey("dbo.EventInstances", "Event_Id", "dbo.Events");
            DropForeignKey("dbo.Locations", "Country_Id", "dbo.Countries");
            DropForeignKey("dbo.ContentPageVersions", "Page_ContentPageId", "dbo.ContentPages");
            DropForeignKey("dbo.BookingRequests", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.BookingRequests", "Bed_Id", "dbo.Beds");
            DropForeignKey("dbo.Audits", "InitiatingUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AccomodationVenuePermissions", "Venue_Id", "dbo.AccomodationVenues");
            DropForeignKey("dbo.AccomodationRooms", "AccomodationVenue_Id", "dbo.AccomodationVenues");
            DropForeignKey("dbo.Photos", "AccomodationVenue_Id", "dbo.AccomodationVenues");
            DropForeignKey("dbo.AccomodationVenues", "Owner_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AccomodationVenues", "Location_Id", "dbo.Locations");
            DropForeignKey("dbo.AccomodationVenueFeatures", "AccomodationVenue_Id", "dbo.AccomodationVenues");
            DropForeignKey("dbo.Photos", "AccomodationRoom_Id", "dbo.AccomodationRooms");
            DropForeignKey("dbo.AccomodationRoomFeatures", "AccomodationRoom_Id", "dbo.AccomodationRooms");
            DropForeignKey("dbo.Beds", "AccomodationRoom_Id", "dbo.AccomodationRooms");
            DropForeignKey("dbo.AccomodationBedAvailabilityNights", "Bed_Id", "dbo.Beds");
            DropForeignKey("dbo.Photos", "Bed_Id", "dbo.Beds");
            DropForeignKey("dbo.Photos", "Owner_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "Location_Id", "dbo.Locations");
            DropForeignKey("dbo.Photos", "Venue_Id", "dbo.Venues");
            DropForeignKey("dbo.Venues", "Location_Id", "dbo.Locations");
            DropForeignKey("dbo.Photos", "Location_Id", "dbo.Locations");
            DropForeignKey("dbo.ContactMethods", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Beds", "BedType_Id", "dbo.BedTypes");
            DropForeignKey("dbo.Beds", "BedSize_Id", "dbo.BedSizes");
            DropIndex("dbo.UserEventAttendances", new[] { "User_Id" });
            DropIndex("dbo.UserEventAttendances", new[] { "EventInstance_Id" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.RolePermissions", new[] { "AdministrationPermission_AdministrationPermissionId" });
            DropIndex("dbo.EventInstances", new[] { "Venue_Id" });
            DropIndex("dbo.EventInstances", new[] { "Event_Id" });
            DropIndex("dbo.ContentPageVersions", new[] { "Page_ContentPageId" });
            DropIndex("dbo.BookingRequests", new[] { "User_Id" });
            DropIndex("dbo.BookingRequests", new[] { "Bed_Id" });
            DropIndex("dbo.Audits", new[] { "InitiatingUser_Id" });
            DropIndex("dbo.AccomodationVenues", new[] { "Owner_Id" });
            DropIndex("dbo.AccomodationVenues", new[] { "Location_Id" });
            DropIndex("dbo.AccomodationVenuePermissions", new[] { "Venue_Id" });
            DropIndex("dbo.AccomodationVenueFeatures", new[] { "AccomodationVenue_Id" });
            DropIndex("dbo.AccomodationRooms", new[] { "AccomodationVenue_Id" });
            DropIndex("dbo.AccomodationRoomFeatures", new[] { "AccomodationRoom_Id" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.Venues", new[] { "Location_Id" });
            DropIndex("dbo.Locations", new[] { "Country_Id" });
            DropIndex("dbo.ContactMethods", new[] { "User_Id" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", new[] { "Location_Id" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.Photos", new[] { "EventInstance_Id" });
            DropIndex("dbo.Photos", new[] { "Event_Id" });
            DropIndex("dbo.Photos", new[] { "AccomodationVenue_Id" });
            DropIndex("dbo.Photos", new[] { "AccomodationRoom_Id" });
            DropIndex("dbo.Photos", new[] { "Bed_Id" });
            DropIndex("dbo.Photos", new[] { "Owner_Id" });
            DropIndex("dbo.Photos", new[] { "Venue_Id" });
            DropIndex("dbo.Photos", new[] { "Location_Id" });
            DropIndex("dbo.Beds", new[] { "AccomodationRoom_Id" });
            DropIndex("dbo.Beds", new[] { "BedType_Id" });
            DropIndex("dbo.Beds", new[] { "BedSize_Id" });
            DropIndex("dbo.AccomodationBedAvailabilityNights", new[] { "Bed_Id" });
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
            DropTable("dbo.AccomodationVenues");
            DropTable("dbo.AccomodationVenuePermissions");
            DropTable("dbo.AccomodationVenueFeatures");
            DropTable("dbo.AccomodationRooms");
            DropTable("dbo.AccomodationRoomFeatures");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.Venues");
            DropTable("dbo.Locations");
            DropTable("dbo.ContactMethods");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Photos");
            DropTable("dbo.BedTypes");
            DropTable("dbo.BedSizes");
            DropTable("dbo.Beds");
            DropTable("dbo.AccomodationBedAvailabilityNights");
        }
    }
}

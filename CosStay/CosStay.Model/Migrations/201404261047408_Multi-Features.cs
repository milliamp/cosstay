namespace CosStay.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MultiFeatures : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AccomodationRoomFeatures", "AccomodationRoom_Id", "dbo.AccomodationRooms");
            DropForeignKey("dbo.AccomodationVenueFeatures", "AccomodationVenue_Id", "dbo.AccomodationVenues");
            DropIndex("dbo.AccomodationRoomFeatures", new[] { "AccomodationRoom_Id" });
            DropIndex("dbo.AccomodationVenueFeatures", new[] { "AccomodationVenue_Id" });
            CreateTable(
                "dbo.AccomodationRoomFeatureAccomodationRooms",
                c => new
                    {
                        AccomodationRoomFeature_Id = c.Int(nullable: false),
                        AccomodationRoom_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.AccomodationRoomFeature_Id, t.AccomodationRoom_Id })
                .ForeignKey("dbo.AccomodationRoomFeatures", t => t.AccomodationRoomFeature_Id, cascadeDelete: true)
                .ForeignKey("dbo.AccomodationRooms", t => t.AccomodationRoom_Id, cascadeDelete: true)
                .Index(t => t.AccomodationRoomFeature_Id)
                .Index(t => t.AccomodationRoom_Id);
            
            CreateTable(
                "dbo.AccomodationVenueFeatureAccomodationVenues",
                c => new
                    {
                        AccomodationVenueFeature_Id = c.Int(nullable: false),
                        AccomodationVenue_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.AccomodationVenueFeature_Id, t.AccomodationVenue_Id })
                .ForeignKey("dbo.AccomodationVenueFeatures", t => t.AccomodationVenueFeature_Id, cascadeDelete: true)
                .ForeignKey("dbo.AccomodationVenues", t => t.AccomodationVenue_Id, cascadeDelete: true)
                .Index(t => t.AccomodationVenueFeature_Id)
                .Index(t => t.AccomodationVenue_Id);
            
            DropColumn("dbo.AccomodationRoomFeatures", "AccomodationRoom_Id");
            DropColumn("dbo.AccomodationVenueFeatures", "AccomodationVenue_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AccomodationVenueFeatures", "AccomodationVenue_Id", c => c.Int());
            AddColumn("dbo.AccomodationRoomFeatures", "AccomodationRoom_Id", c => c.Int());
            DropForeignKey("dbo.AccomodationVenueFeatureAccomodationVenues", "AccomodationVenue_Id", "dbo.AccomodationVenues");
            DropForeignKey("dbo.AccomodationVenueFeatureAccomodationVenues", "AccomodationVenueFeature_Id", "dbo.AccomodationVenueFeatures");
            DropForeignKey("dbo.AccomodationRoomFeatureAccomodationRooms", "AccomodationRoom_Id", "dbo.AccomodationRooms");
            DropForeignKey("dbo.AccomodationRoomFeatureAccomodationRooms", "AccomodationRoomFeature_Id", "dbo.AccomodationRoomFeatures");
            DropIndex("dbo.AccomodationVenueFeatureAccomodationVenues", new[] { "AccomodationVenue_Id" });
            DropIndex("dbo.AccomodationVenueFeatureAccomodationVenues", new[] { "AccomodationVenueFeature_Id" });
            DropIndex("dbo.AccomodationRoomFeatureAccomodationRooms", new[] { "AccomodationRoom_Id" });
            DropIndex("dbo.AccomodationRoomFeatureAccomodationRooms", new[] { "AccomodationRoomFeature_Id" });
            DropTable("dbo.AccomodationVenueFeatureAccomodationVenues");
            DropTable("dbo.AccomodationRoomFeatureAccomodationRooms");
            CreateIndex("dbo.AccomodationVenueFeatures", "AccomodationVenue_Id");
            CreateIndex("dbo.AccomodationRoomFeatures", "AccomodationRoom_Id");
            AddForeignKey("dbo.AccomodationVenueFeatures", "AccomodationVenue_Id", "dbo.AccomodationVenues", "Id");
            AddForeignKey("dbo.AccomodationRoomFeatures", "AccomodationRoom_Id", "dbo.AccomodationRooms", "Id");
        }
    }
}

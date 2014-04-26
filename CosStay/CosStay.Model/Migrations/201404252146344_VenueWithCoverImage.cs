namespace CosStay.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class VenueWithCoverImage : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EventInstances", "MainImageUrl", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.EventInstances", "MainImageUrl");
        }
    }
}

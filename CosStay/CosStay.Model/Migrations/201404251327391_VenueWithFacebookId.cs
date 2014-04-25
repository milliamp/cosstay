namespace CosStay.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class VenueWithFacebookId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Venues", "FacebookId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Venues", "FacebookId");
        }
    }
}

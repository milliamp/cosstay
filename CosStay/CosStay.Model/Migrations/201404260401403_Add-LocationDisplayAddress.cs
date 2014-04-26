namespace CosStay.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddLocationDisplayAddress : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AccomodationVenues", "PublicAddress", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AccomodationVenues", "PublicAddress");
        }
    }
}

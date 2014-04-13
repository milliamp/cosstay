namespace CosStay.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NamedRooms : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AccomodationRooms", "Name", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AccomodationRooms", "Name");
        }
    }
}

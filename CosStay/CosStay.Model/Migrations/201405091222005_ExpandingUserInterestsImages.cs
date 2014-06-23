namespace CosStay.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ExpandingUserInterestsImages : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Interests", "ImageProviderService", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Interests", "ImageProviderService");
        }
    }
}

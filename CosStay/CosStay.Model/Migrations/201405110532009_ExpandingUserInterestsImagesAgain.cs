namespace CosStay.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ExpandingUserInterestsImagesAgain : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Interests", "Approved", c => c.Boolean(nullable: false));
            AddColumn("dbo.InterestCategories", "ImageProviderService", c => c.Int(nullable: false));
            DropColumn("dbo.Interests", "ImageProviderService");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Interests", "ImageProviderService", c => c.Int(nullable: false));
            DropColumn("dbo.InterestCategories", "ImageProviderService");
            DropColumn("dbo.Interests", "Approved");
        }
    }
}

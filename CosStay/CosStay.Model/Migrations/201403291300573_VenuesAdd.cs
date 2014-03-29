namespace CosStay.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class VenuesAdd : DbMigration
    {
        public override void Up()
        {
            DropTable("dbo.FacebookAccessTokens");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.FacebookAccessTokens",
                c => new
                    {
                        FacebookAccessTokenId = c.Int(nullable: false, identity: true),
                        AccessToken = c.String(),
                        Expiry = c.DateTimeOffset(nullable: false, precision: 7),
                        Obtained = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.FacebookAccessTokenId);
            
        }
    }
}

namespace CosStay.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Interests1 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.InterestCategories", "Category");
        }
        
        public override void Down()
        {
            AddColumn("dbo.InterestCategories", "Category", c => c.String());
        }
    }
}

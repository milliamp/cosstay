namespace CosStay.Model.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<CosStay.Model.CosStayContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(CosStay.Model.CosStayContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            context.BedSizes.AddOrUpdate(
                bs => bs.Name,
                new BedSize {  Name = "Single" },
                new BedSize {  Name = "King Single" },
                new BedSize {  Name = "Double" },
                new BedSize {  Name = "Queen" },
                new BedSize {  Name = "King" },
                new BedSize {  Name = "Other" }
            );

            context.BedTypes.AddOrUpdate(
                bs => bs.Name,
                new BedType { Name = "Normal" },
                new BedType { Name = "Bunk" },
                new BedType { Name = "Air Bed" },
                new BedType { Name = "Sofa Bed" },
                new BedType { Name = "Water Bed" },
                new BedType { Name = "Foam Matress" },
                new BedType { Name = "Other" }
            );
        }
    }
}

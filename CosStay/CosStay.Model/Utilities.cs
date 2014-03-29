using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosStay.Model
{
    public static class Utilities
    {
        private const string SECURITY_CATEGORY = "SECURITY";
        private const string APPLICATION_CATEGORY = "APPLICATION";
        public static bool SeedData(CosStayContext context)
        {
            if (NeedsUpdate(context, SECURITY_CATEGORY, new DateTimeOffset(new DateTime(2014,03,23,17,46,0))))
            {
                var admin = new User {
                    Name = "Adminstrator",
                    UserName = "administrator",
                    Email = "alex@milliamp.org",
                    JoinDate = DateTime.UtcNow
                };
                var adminRole = new IdentityRole
                {
                    Name = "Administrator",
                };
                context.Users.Add(admin);
                

                var seedData = new SeedData
                {
                    Category = SECURITY_CATEGORY,
                    Version = new DateTimeOffset(new DateTime(2014, 03, 23, 17, 46, 0))
                };
                context.SeedData.Add(seedData);
                context.SaveChanges();
            }

            if (NeedsUpdate(context, APPLICATION_CATEGORY, new DateTimeOffset(new DateTime(2014, 03, 29, 18, 01, 0))))
            {
                var hamilton = new Location
                {
                    Name = "Hamilton",
                    LatLng = new LatLng
                    {
                        Lat = -37.7802969f,
                        Lng = 175.2586997f
                    },
                };
                context.Locations.Add(hamilton);

                var page1 = new ContentPage
                {
                    Uri = "test-page"
                };

                var ver1 = new ContentPageVersion
                {
                    CreatedDate = DateTime.Now,
                    MarkdownContent = "HALOO1",
                    Page = page1,
                    PublishDate = DateTime.Now,
                    Status = ContentPageVersionStatus.Published,
                    Title = "TITEL1!",
                    Version = 1
                };

                var ver2 = new ContentPageVersion
                {
                    CreatedDate = DateTime.Now,
                    MarkdownContent = "HALOO",
                    Page = page1,
                    PublishDate = DateTime.Now.AddDays(2),
                    Status = ContentPageVersionStatus.Draft,
                    Title = "TITEL!",
                    Version = 2
                };

                context.ContentPages.Add(page1);
                context.ContentPageVersions.Add(ver1);
                context.ContentPageVersions.Add(ver2);

                var seedData = new SeedData
                {
                    Category = APPLICATION_CATEGORY,
                    Version = new DateTimeOffset(new DateTime(2014, 03, 23, 18, 01, 0))
                };
                context.SeedData.Add(seedData);
                context.SaveChanges();

            }


            if (NeedsUpdate(context, APPLICATION_CATEGORY, new DateTimeOffset(new DateTime(2014, 03, 29, 18, 30, 0))))
            {

                var page1 = new ContentPage
                {
                    Uri = "404error"
                };

                var ver1 = new ContentPageVersion
                {
                    CreatedDate = DateTime.Now,
                    MarkdownContent = "ERROR 404 NOT FND",
                    Page = page1,
                    PublishDate = DateTime.Now,
                    Status = ContentPageVersionStatus.Published,
                    Title = "NT FND!",
                    Version = 1
                };

                context.ContentPages.Add(page1);
                context.ContentPageVersions.Add(ver1);

                var seedData = new SeedData
                {
                    Category = APPLICATION_CATEGORY,
                    Version = new DateTimeOffset(new DateTime(2014, 03, 23, 18, 30, 0))
                };
                context.SeedData.Add(seedData);
                context.SaveChanges();
            }

            return true;
        }

        private static bool NeedsUpdate(CosStayContext context, string category, DateTimeOffset date)
        {
            var latest = context.SeedData.OrderByDescending(sd => sd.Version).FirstOrDefault(sd =>sd.Category == category);
            if (latest == null)
                return true;
            return latest.Version < date;
        }
    }
}

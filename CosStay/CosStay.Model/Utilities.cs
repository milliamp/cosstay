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
                var adminRole = new Role
                {
                    Name = "Administrator",
                    Users = new List<User>() {
                        admin
                    }
                };
                context.Users.Add(admin);
                context.Roles.Add(adminRole);

                var seedData = new SeedData
                {
                    Category = SECURITY_CATEGORY,
                    Version = new DateTimeOffset(new DateTime(2014, 03, 23, 17, 46, 0))
                };
                context.SeedData.Add(seedData);
                context.SaveChanges();
            }

            if (NeedsUpdate(context, APPLICATION_CATEGORY, new DateTimeOffset(new DateTime(2014, 03, 23, 17, 46, 0))))
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

                var seedData = new SeedData
                {
                    Category = APPLICATION_CATEGORY,
                    Version = new DateTimeOffset(new DateTime(2014, 03, 23, 17, 46, 0))
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

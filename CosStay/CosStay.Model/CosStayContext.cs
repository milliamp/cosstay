//using CosStay.Model.Migrations;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosStay.Model
{
    public class CosStayContext : IdentityDbContext<User>
    {
        public CosStayContext()
            : base("DefaultConnection")
        {

            //Database.SetInitializer(new MigrateDatabaseToLatestVersion<CosStayContext, Configuration>()); 
            //Database.SetInitializer(new DropCreateDatabaseAlways<CosStayContext>());
        }

        public DbSet<Event> Events { get; set; }
        public DbSet<EventInstance> EventInstances { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Venue> Venues { get; set; }
        public DbSet<Country> Countries { get; set; }
        //public DbSet<User> Users { get; set; }
        public DbSet<Audit> Audits { get; set; }
        public DbSet<ContactMethod> ContactMethods { get; set; }
        public DbSet<AccomodationVenue> AccomodationVenues { get; set; }
        public DbSet<AccomodationRoom> AccomodationRooms { get; set; }
        public DbSet<BookingRequest> BookingRequests { get; set; }
        public DbSet<AccomodationBedAvailabilityNight> AccomodationBedAvailabilityNights { get; set; }
        public DbSet<UserEventAttendance> UserEventAttendances { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<Bed> Beds { get; set; }
        public DbSet<BedType> BedTypes { get; set; }
        public DbSet<BedSize> BedSizes { get; set; }
        public DbSet<AccomodationVenueFeature> AccomodationVenueFeatures { get; set; }
        public DbSet<AccomodationRoomFeature> AccomodationRoomFeatures { get; set; }

        public DbSet<AccomodationVenuePermission> AccomodationVenuePermissions { get; set; }
        public DbSet<AdministrationPermission> AdministrationPermissions { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        //public DbSet<Role> Roles { get; set; }

        public DbSet<ContentPage> ContentPages { get; set; }
        public DbSet<ContentPageVersion> ContentPageVersions { get; set; }

        public DbSet<SeedData> SeedData { get; set; }

    }
}

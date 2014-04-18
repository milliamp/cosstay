//using CosStay.Model.Migrations;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Data.Entity.Validation;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosStay.Model
{
    public class CosStayContext : DbContext
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


        public virtual IDbSet<User> Users
        {
            get;
            set;
        }
        public virtual IDbSet<IdentityRole> Roles
        {
            get;
            set;
        }
        public bool RequireUniqueEmail
        {
            get;
            set;
        }

        /// <summary>
        ///     Maps table names, and sets up relationships between the various user entities
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            if (modelBuilder == null)
            {
                throw new ArgumentNullException("modelBuilder");
            }
            EntityTypeConfiguration<User> entityTypeConfiguration = modelBuilder.Entity<User>().ToTable("AspNetUsers");
            entityTypeConfiguration.HasMany<IdentityUserRole>((User u) => u.Roles).WithRequired().HasForeignKey<string>((IdentityUserRole ur) => ur.UserId);
            entityTypeConfiguration.HasMany<IdentityUserClaim>((User u) => u.Claims).WithRequired().HasForeignKey<string>((IdentityUserClaim uc) => uc.UserId);
            entityTypeConfiguration.HasMany<IdentityUserLogin>((User u) => u.Logins).WithRequired().HasForeignKey<string>((IdentityUserLogin ul) => ul.UserId);
            StringPropertyConfiguration arg_273_0 = entityTypeConfiguration.Property((User u) => u.UserName).IsRequired().HasMaxLength(new int?(256));
            string arg_273_1 = "Index";
            IndexAttribute indexAttribute = new IndexAttribute("UserNameIndex");
            indexAttribute.IsUnique = true;
            arg_273_0.HasColumnAnnotation(arg_273_1, new IndexAnnotation(indexAttribute));
            entityTypeConfiguration.Property((User u) => u.Email).HasMaxLength(new int?(256));
            modelBuilder.Entity<IdentityUserRole>().HasKey((IdentityUserRole r) => new
            {
                r.UserId,
                r.RoleId
            }).ToTable("AspNetUserRoles");
            modelBuilder.Entity<IdentityUserLogin>().HasKey((IdentityUserLogin l) => new
            {
                l.LoginProvider,
                l.ProviderKey,
                l.UserId
            }).ToTable("AspNetUserLogins");
            modelBuilder.Entity<IdentityUserClaim>().ToTable("AspNetUserClaims");
            EntityTypeConfiguration<IdentityRole> entityTypeConfiguration2 = modelBuilder.Entity<IdentityRole>().ToTable("AspNetRoles");
            StringPropertyConfiguration arg_563_0 = entityTypeConfiguration2.Property((IdentityRole r) => r.Name).IsRequired().HasMaxLength(new int?(256));
            string arg_563_1 = "Index";
            IndexAttribute indexAttribute2 = new IndexAttribute("RoleNameIndex");
            indexAttribute2.IsUnique =true;
            arg_563_0.HasColumnAnnotation(arg_563_1, new IndexAnnotation(indexAttribute2));
            entityTypeConfiguration2.HasMany<IdentityUserRole>((IdentityRole r) => r.Users).WithRequired().HasForeignKey<string>((IdentityUserRole ur) => ur.RoleId);
        }
        /// <summary>
        ///     Validates that UserNames are unique and case insenstive
        /// </summary>
        /// <param name="entityEntry"></param>
        /// <param name="items"></param>
        /// <returns></returns>
        protected override DbEntityValidationResult ValidateEntity(DbEntityEntry entityEntry, IDictionary<object, object> items)
        {
            if (entityEntry != null && entityEntry.State == EntityState.Added)
            {
                List<DbValidationError> list = new List<DbValidationError>();
                User user = entityEntry.Entity as User;
                if (user != null)
                {
                    if (this.Users.Any((User u) => string.Equals(u.UserName, user.UserName)))
                    {
                        list.Add(new DbValidationError("User", string.Format(CultureInfo.CurrentCulture, "Duplicate user name", new object[]
						{
							user.UserName
						})));
                    }
                    if (this.RequireUniqueEmail && this.Users.Any((User u) => string.Equals(u.Email, user.Email)))
                    {
                        list.Add(new DbValidationError("User", string.Format(CultureInfo.CurrentCulture, "Duplicate email address", new object[]
						{
							user.Email
						})));
                    }
                }
                else
                {
                    IdentityRole role = entityEntry.Entity as IdentityRole;
                    if (role != null && this.Roles.Any((IdentityRole r) => string.Equals(r.Name, role.Name)))
                    {
                        list.Add(new DbValidationError("Role", string.Format(CultureInfo.CurrentCulture, "Role already exists", new object[]
						{
							role.Name
						})));
                    }
                }
                if (list.Any<DbValidationError>())
                {
                    return new DbEntityValidationResult(entityEntry, list);
                }
            }
            return base.ValidateEntity(entityEntry, items);
        }
    }
}

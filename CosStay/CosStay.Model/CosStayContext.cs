//using CosStay.Model.Migrations;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Data.Entity.Validation;
using System.Globalization;
using System.Linq;
using System.Reflection;
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


            var objectContext = ((IObjectContextAdapter)this).ObjectContext;
            var mdw = objectContext.MetadataWorkspace;
            var items = mdw.GetItems<EntityType>(DataSpace.CSpace);

            foreach (var entity in items)
            {
                //var entity = items.First(e => e.FullName == entityType.FullName);

                var entitySetName = objectContext.DefaultContainerName + "." + entity.Name;
                var keyNames = entity.KeyMembers.Select(k => k.Name);

                var prop = Type.GetType(entity.FullName); // entity.GetProperty(keyName);

                _entityKeyProperties.Add(prop, new EntityKeyInfo() { EntitySetName = entitySetName, PropertyInfo = prop.GetProperties().Where(p => keyNames.Contains(p.Name)).ToArray() });
            }
        }

        private Dictionary<Type, EntityKeyInfo> _entityKeyProperties = new Dictionary<Type, EntityKeyInfo>();

        private class EntityKeyInfo
        {
            public string EntitySetName { get; set; }
            public PropertyInfo[] PropertyInfo { get; set; }
        }

        public override int SaveChanges()
        {
            return SaveChanges(new Request()
            {
                Date = DateTimeOffset.Now
            });
            //throw new NotImplementedException("Must provide a 'request' object");
        }

        public override async Task<int> SaveChangesAsync()
        {
            return await base.SaveChangesAsync();
        }

        public override async Task<int> SaveChangesAsync(System.Threading.CancellationToken cancellationToken)
        {
            return await base.SaveChangesAsync(cancellationToken);
        }

        public PropertyInfo KeyForEntity(Type entityType)
        {
            return _entityKeyProperties[entityType].PropertyInfo.First();
        }

        public int SaveChanges(Request request)
        {
            return SaveChangesAsync(request).Result;
        }

        public async Task<int> SaveChangesAsync(Request request)
        {
            var oca = this as IObjectContextAdapter;
            var oc = oca.ObjectContext;
            oc.DetectChanges();
            var entries = oc.ObjectStateManager.GetObjectStateEntries(EntityState.Added | EntityState.Modified | EntityState.Unchanged).Where(e => !e.IsRelationship);

            // Save these for after we get PK
            var addedEntities = entries.Where(a => a.State == EntityState.Added).ToArray();
            var modifiedEntities = entries.Where(a => a.State == EntityState.Modified).ToList();
            var deletedEntites = entries.Where(a => a.State == EntityState.Deleted).ToList();

            var ose = oc.ObjectStateManager.GetObjectStateEntries(EntityState.Added).Where(e => e.IsRelationship);

            var relations = new Dictionary<ObjectStateEntry, List<ObjectStateEntry>>();

            foreach (var entry in ose)
            {
                var leftKey = entry.CurrentValues.GetValue(0) as EntityKey;
                var rightKey = entry.CurrentValues.GetValue(1) as EntityKey;

                //var entityType = entry.Entity.GetType();
                //var keyInfo = _entityKeyProperties[entityType];

                var matchingEntry = entries.FirstOrDefault(e => e.EntityKey.Equals(leftKey));
                var rightHandEntry = entries.FirstOrDefault(e => e.EntityKey.Equals(rightKey));

                if (matchingEntry == null)
                {
                    //explode
                    throw new InvalidOperationException("Can't find entry");
                }
                if (!relations.Keys.Contains(matchingEntry))
                    relations.Add(matchingEntry, new List<ObjectStateEntry>(1));
                var relationList = relations[matchingEntry];
                relationList.Add(rightHandEntry);

                if (!leftKey.IsTemporary && !modifiedEntities.Contains(entry))
                    modifiedEntities.Add(matchingEntry);

            }

            foreach (var entry in modifiedEntities)
            {
                var audit = CreateAudit(request, entry, EntityState.Modified, relations.ContainsKey(entry) ? relations[entry] : new List<ObjectStateEntry>());

                Audits.Add(audit);
            }

            var firstSave = await base.SaveChangesAsync();

            foreach (var entry in addedEntities)
            {
                var audit = CreateAudit(request, entry, EntityState.Added, relations.ContainsKey(entry) ? relations[entry] : new List<ObjectStateEntry>());
                Audits.Add(audit);

            }

            var secondSave = await base.SaveChangesAsync();
            return firstSave + secondSave;
        }

        private Audit CreateAudit(Request request, ObjectStateEntry entry, EntityState state, IEnumerable<ObjectStateEntry> relatedEntities)
        {
            string objectTypeName = "";
            string data = "";
            try
            {
                DbDataRecord original = null;
                if (state != EntityState.Added)
                    original = entry.OriginalValues;
                var properties = ChangedProperties(state, entry.OriginalValues, entry.CurrentValues, relatedEntities);

                foreach (var relatedEntity in relatedEntities)
                {
                    var propertyList = entry.Entity.GetType().GetProperties();
                    // Random Guess based on type
                    var namedProperty = propertyList.FirstOrDefault(pi => pi.PropertyType == relatedEntity.Entity.GetType() || pi.PropertyType.GenericTypeArguments.Any(gta => gta == relatedEntity.Entity.GetType()));
                    properties.Add(namedProperty.Name, relatedEntity.EntityKey.EntityKeyValues != null ? relatedEntity.EntityKey.EntityKeyValues[0].Value.ToString() : "New Entry");
                }

                var objectType = entry.Entity.GetType();
                while (!objectType.Namespace.Contains("CosStay.Model") && objectType.BaseType != null)
                    objectType = objectType.BaseType;

                objectTypeName = objectType.Name;

                data = Newtonsoft.Json.JsonConvert.SerializeObject(properties);
            }
            catch (Exception e)
            {
                if (string.IsNullOrWhiteSpace(data))
                    data = "Failed to create Audit - " + e.Message;
            }

            var audit = new Audit()
            {
                AuditEvent = Enum.GetName(typeof(EntityState), state),
                EventDate = request.Date,
                InitiatingUser = Users.SingleOrDefault(u => u.Id == request.UserId),
                IP = request.IP,
                UserAgent = request.UserAgent,
                ObjectType = objectTypeName,
                Data = data
            };

            return audit;
        }

        private Dictionary<string, object> ChangedProperties(EntityState state, DbDataRecord original, DbDataRecord current, IEnumerable<ObjectStateEntry> relatedEntities)
        {

            var properties = new Dictionary<string, object>();
            //foreach (var p in current.PropertyNames)
            for (var i = 0; i < original.FieldCount; i++)
            {
                var oldValue = original.GetValue(i);
                var newValue = current.GetValue(i);
                //ObjectStateEntryDbDataRecord
                if (oldValue is DbDataRecord && newValue is DbDataRecord)
                {
                    var complexTypeChangedValues = ChangedProperties(state, (DbDataRecord)oldValue, (DbDataRecord)newValue, new List<ObjectStateEntry>());
                    if (complexTypeChangedValues.Count > 0)
                        properties.Add(original.GetName(i), complexTypeChangedValues);
                    continue;
                }
                if (ValuesHaveChanged(oldValue, newValue) || state == EntityState.Added || original.GetName(i) == "Id")
                    properties.Add(original.GetName(i), newValue);
            }


            return properties;
        }

        private bool ValuesHaveChanged(object oldValue, object newValue)
        {
            if (oldValue == null && newValue == null)
                return false;
            if (oldValue == null && newValue != null)
                return true;
            if (oldValue != null && newValue == null)
                return true;

            return !oldValue.Equals(newValue);
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
            indexAttribute2.IsUnique = true;
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

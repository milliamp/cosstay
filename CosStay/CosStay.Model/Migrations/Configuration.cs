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

            #region Beds
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
                new BedType { Name = "Couch" },
                new BedType { Name = "Air Bed" },
                new BedType { Name = "Sofa Bed" },
                new BedType { Name = "Water Bed" },
                new BedType { Name = "Foam Matress" },
                new BedType { Name = "Other" }
            );
            #endregion
            #region Countries/Locations
            context.Countries.AddOrUpdate(
                c => c.Name,
                new Country()
                {
                    Name = "New Zealand",
                    ShortName = "NZ",
                },
                new Country()
                {
                    Name = "Australia",
                    ShortName = "AU"
                }
            );

            context.Locations.AddOrUpdate(
                l => l.Name,
                new Location() {
                    Name = "Auckland",
                    Country = context.Countries.Single(c => c.ShortName == "NZ"),
                    LatLng = new LatLng()
                },
                new Location() {
                    Name = "Hamilton",
                    Country = context.Countries.Single(c => c.ShortName == "NZ"),
                    LatLng = new LatLng()
                },
                new Location() {
                    Name = "Wellington",
                    Country = context.Countries.Single(c => c.ShortName == "NZ"),
                    LatLng = new LatLng()
                },
                new Location() {
                    Name = "Christchurch",
                    Country = context.Countries.Single(c => c.ShortName == "NZ"),
                    LatLng = new LatLng()
                },
                new Location() {
                    Name = "Dunedin",
                    Country = context.Countries.Single(c => c.ShortName == "NZ"),
                    LatLng = new LatLng()
                },
                new Location() {
                    Name = "Sydney",
                    Country = context.Countries.Single(c => c.ShortName == "AU"),
                    LatLng = new LatLng()
                },
                new Location() {
                    Name = "Melbourne",
                    Country = context.Countries.Single(c => c.ShortName == "AU"),
                    LatLng = new LatLng()
                },
                new Location() {
                    Name = "Brisbane",
                    Country = context.Countries.Single(c => c.ShortName == "AU"),
                    LatLng = new LatLng()
                },
                new Location() {
                    Name = "Adelaide",
                    Country = context.Countries.Single(c => c.ShortName == "AU"),
                    LatLng = new LatLng()
                },
                new Location() {
                    Name = "Perth",
                    Country = context.Countries.Single(c => c.ShortName == "AU"),
                    LatLng = new LatLng()
                }
            );
            #endregion
            #region Users
            context.Users.AddOrUpdate(
                u => u.UserName,
                new User()
                {
                    Name = "Bilbo Baggins",
                    Email = "bilbo@bagend.org",
                    EmailConfirmed = false,
                    UserName = "bilbo",
                    Id = Guid.NewGuid().ToString()
                },
                new User()
                {
                    Name = "Gollum",
                    Email = "gollum@nofixedabode.org",
                    EmailConfirmed = false,
                    UserName = "precious",
                    Id = Guid.NewGuid().ToString()
                }
            );
            #endregion
            #region Accomodation
            context.AccomodationVenues.AddOrUpdate(
                av => av.Name,
                new AccomodationVenue()
                {
                    Name = "The Shire",
                    Address = "The Shire, Bagend",
                    PublicAddress = "Bagend",
                    AllowsBedSharing = false,
                    AllowsMixedRooms = false,
                    DateAdded = DateTimeOffset.Now,
                    Description = "Just for the hobbitses",
                    LatLng = new LatLng(),
                    Location = context.Locations.Single(c => c.Name == "Brisbane"),
                    Owner = context.Users.First(),
                    Rooms = new System.Collections.Generic.List<AccomodationRoom>()  {
                        new AccomodationRoom() {
                            Beds = new System.Collections.Generic.List<Bed>() {
                                new Bed() {
                                    BedSize = context.BedSizes.Single(bs => bs.Name == "Single"),
                                    BedType = context.BedTypes.Single(bt => bt.Name == "Bunk"),
                                    DateAdded = DateTimeOffset.Now
                                },
                                new Bed() {
                                    BedSize = context.BedSizes.Single(bs => bs.Name == "Single"),
                                    BedType = context.BedTypes.Single(bt => bt.Name == "Bunk"),
                                    DateAdded = DateTimeOffset.Now
                                },
                                new Bed() {
                                    BedSize = context.BedSizes.Single(bs => bs.Name == "Single"),
                                    BedType = context.BedTypes.Single(bt => bt.Name == "Bunk"),
                                    DateAdded = DateTimeOffset.Now
                                },
                                new Bed() {
                                    BedSize = context.BedSizes.Single(bs => bs.Name == "Single"),
                                    BedType = context.BedTypes.Single(bt => bt.Name == "Bunk"),
                                    DateAdded = DateTimeOffset.Now
                                },
                                new Bed() {
                                    BedSize = context.BedSizes.Single(bs => bs.Name == "Single"),
                                    BedType = context.BedTypes.Single(bt => bt.Name == "Bunk"),
                                    DateAdded = DateTimeOffset.Now
                                },
                                new Bed() {
                                    BedSize = context.BedSizes.Single(bs => bs.Name == "Single"),
                                    BedType = context.BedTypes.Single(bt => bt.Name == "Bunk"),
                                    DateAdded = DateTimeOffset.Now
                                }
                            },
                            DateAdded = DateTimeOffset.Now,
                            Name = "Room 1"
                        },
                        new AccomodationRoom() {
                            Beds = new System.Collections.Generic.List<Bed>() {
                                new Bed() {
                                    BedSize = context.BedSizes.Single(bs => bs.Name == "Single"),
                                    BedType = context.BedTypes.Single(bt => bt.Name == "Bunk"),
                                    DateAdded = DateTimeOffset.Now
                                },
                                new Bed() {
                                    BedSize = context.BedSizes.Single(bs => bs.Name == "Single"),
                                    BedType = context.BedTypes.Single(bt => bt.Name == "Bunk"),
                                    DateAdded = DateTimeOffset.Now
                                },
                                new Bed() {
                                    BedSize = context.BedSizes.Single(bs => bs.Name == "Single"),
                                    BedType = context.BedTypes.Single(bt => bt.Name == "Bunk"),
                                    DateAdded = DateTimeOffset.Now
                                },
                                new Bed() {
                                    BedSize = context.BedSizes.Single(bs => bs.Name == "Single"),
                                    BedType = context.BedTypes.Single(bt => bt.Name == "Bunk"),
                                    DateAdded = DateTimeOffset.Now
                                },
                                new Bed() {
                                    BedSize = context.BedSizes.Single(bs => bs.Name == "Single"),
                                    BedType = context.BedTypes.Single(bt => bt.Name == "Bunk"),
                                    DateAdded = DateTimeOffset.Now
                                },
                                new Bed() {
                                    BedSize = context.BedSizes.Single(bs => bs.Name == "Single"),
                                    BedType = context.BedTypes.Single(bt => bt.Name == "Bunk"),
                                    DateAdded = DateTimeOffset.Now
                                }
                            },
                            DateAdded = DateTimeOffset.Now,
                            Name = "Room 2"
                        }
                    }
                }
            );
            #endregion
            #region Interests
            context.InterestCategories.AddOrUpdate(
                c => c.Name,
                new InterestCategory()
                {
                    Name = "Anime"
                },
                new InterestCategory()
                {
                    Name = "Manga"
                },
                new InterestCategory()
                {
                    Name = "Comics"
                },
                new InterestCategory()
                {
                    Name = "Films"
                },
                new InterestCategory()
                {
                    Name = "TV Shows"
                },
                new InterestCategory()
                {
                    Name = "Books"
                },
                new InterestCategory()
                {
                    Name = "Cosplay"
                },
                new InterestCategory()
                {
                    Name = "Other interests"
                }

            );

            context.Interests.AddOrUpdate(
                i => i.Name,
                new Interest()
                {
                    Name = "Anime",
                    Category = context.InterestCategories.Single(a => a.Name == "Anime")
                },

                new Interest()
                {
                    Name = "Manga",
                    Category = context.InterestCategories.Single(a => a.Name == "Manga")
                },
                new Interest()
                {
                    Name = "Comics",
                    Category = context.InterestCategories.Single(a => a.Name == "Comics")
                },

                new Interest()
                {
                    Name = "Films",
                    Category = context.InterestCategories.Single(a => a.Name == "Films")
                },
                new Interest()
                {
                    Name = "Cosplay",
                    Category = context.InterestCategories.Single(a => a.Name == "Cosplay")
                },
                new Interest()
                {
                    Name = "TV Shows",
                    Category = context.InterestCategories.Single(a => a.Name == "TV Shows")
                },

                new Interest()
                {
                    Name = "Books",
                    Category = context.InterestCategories.Single(a => a.Name == "Books")
                },
                new Interest()
                {
                    Name = "Art",
                    Category = context.InterestCategories.Single(a => a.Name == "Other interests")
                },
                new Interest()
                {
                    Name = "Music",
                    Category = context.InterestCategories.Single(a => a.Name == "Other interests")
                },
                new Interest()
                {
                    Name = "My Little Pony",
                    Category = context.InterestCategories.Single(a => a.Name == "TV Shows")
                }
            );

            #endregion
        }
    }
}

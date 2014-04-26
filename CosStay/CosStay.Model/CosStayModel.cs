using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosStay.Model
{
    public class NamedEntity : IEntity
    {
        public string Name { get; set; }
        public int Id { get; set; }
    }

    public class Event : NamedEntity, IAddable, IDeletable, IAuditable
    {
        public virtual List<EventInstance> Instances { get; set; }
        [Display(Name="URL")]
        public string Url { get; set; }

        public virtual List<Photo> Photos { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class EventInstance : NamedEntity, IAddable, IDeletable, IAuditable
    {
        public virtual Event Event { get; set; }
        [Display(Name = "URL")]
        public string Url { get; set; }
        public string FacebookEventId { get; set; }
        public string Description { get; set; }
        public virtual Venue Venue { get; set; }
        [Display(Name = "Start Date", ShortName="Start")]
        public DateTime StartDate { get; set; }
        [Display(Name = "End Date", ShortName = "End")]
        public DateTime EndDate { get; set; }

        [Display(Name = "Date Updated", ShortName = "Updated")]
        public DateTimeOffset DateUpdated { get; set; }

        public string MainImageUrl { get; set; }

        public virtual List<Photo> Photos { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class Location : NamedEntity, IAddable, IDeletable, IAuditable
    {
        public virtual LatLng LatLng { get; set; }
        public virtual List<Venue> Venues { get; set; }

        public virtual List<Photo> Photos { get; set; }
        public bool IsDeleted { get; set; }
        public virtual Country Country { get; set; }
    }

    public class TravelInfo
    {
        public virtual LatLng From { get; set; }
        public virtual LatLng To { get; set; }
        public virtual List<TravelCost> TravelCosts { get; set; }
    }
    public class TravelCost
    {
        public virtual TravelMethod Method { get; set; }
        public decimal? Distance { get; set; }
        public decimal? Price { get; set; }
    }
    public enum TravelMethod
    {
        Direct,
        Walking,
        Cycling,
        Driving,
        PublicTransport,
        Taxi
    };

    public class Venue : NamedEntity, IAddable, IDeletable, IAuditable
    {
        public virtual LatLng LatLng { get; set; }
        public string Address { get; set; }
        public virtual Location Location { get; set; }

        public virtual List<Photo> Photos { get; set; }
        public bool IsDeleted { get; set; }

        public string FacebookId { get; set; }

        public virtual List<EventInstance> EventInstances { get; set; }

    }

    [ComplexType]
    public class LatLng
    {
        public LatLng()
        {
        }
        public LatLng(double lat, double lng)
        {
            Lat = lat;
            Lng = lng;
        }
        public double? Lat { get; private set; }
        public double? Lng { get; private set; }

        public bool HasValue { get { return Lat.HasValue && Lng.HasValue; } }
    }

    public class Country : NamedEntity, IAuditable
    {
        public virtual List<Location> Locations { get; set; }
        public string ShortName { get; set; }
    }

    public class User : IUser<string>, IAddable, IDeletable, IAuditable
    {
        public string Name { get; set; }
        public virtual Location Location { get; set; }

        [Display(Name = "Joined")]
        public DateTimeOffset JoinDate { get; set; }

        [Display(Name = "Last Seen", ShortName = "Seen")]
        public DateTimeOffset LastSeen { get; set; }
        [Display(Name = "Last Updated", ShortName = "Updated")]
        public DateTimeOffset DetailsUpdatedDate { get; set; }

        //public virtual List<Role> Roles { get; set; }
        public virtual List<ContactMethod> ContactMethods { get; set; }

        // From AspNet.Identity.EntityFramework
        // Summary:
        //     Used to record failures for the purposes of lockout
        public virtual int AccessFailedCount { get; set; }
        //
        // Summary:
        //     Navigation property for user claims
        public virtual ICollection<IdentityUserClaim> Claims { get; set; }
        //
        // Summary:
        //     Email
        public virtual string Email { get; set; }
        //
        // Summary:
        //     True if the email is confirmed, default is false
        public virtual bool EmailConfirmed { get; set; }
        //
        // Summary:
        //     User ID (Primary Key)
        public virtual string Id { get; set; }
        //
        // Summary:
        //     Is lockout enabled for this user
        public virtual bool LockoutEnabled { get; set; }
        //
        // Summary:
        //     DateTime in UTC when lockout ends, any time in the past is considered not
        //     locked out.
        public virtual DateTime? LockoutEndDateUtc { get; set; }
        //
        // Summary:
        //     Navigation property for user logins
        public virtual ICollection<IdentityUserLogin> Logins { get; set; }
        //
        // Summary:
        //     The salted/hashed form of the user password
        public virtual string PasswordHash { get; set; }
        //
        // Summary:
        //     PhoneNumber for the user
        public virtual string PhoneNumber { get; set; }
        //
        // Summary:
        //     True if the phone number is confirmed, default is false
        public virtual bool PhoneNumberConfirmed { get; set; }
        //
        // Summary:
        //     Navigation property for user roles
        public virtual ICollection<IdentityUserRole> Roles { get; private set; }
        //
        // Summary:
        //     A random value that should change whenever a users credentials have changed
        //     (password changed, login removed)
        public virtual string SecurityStamp { get; set; }
        //
        // Summary:
        //     Is two factor enabled for the user
        public virtual bool TwoFactorEnabled { get; set; }
        //
        // Summary:
        //     User name
        public virtual string UserName { get; set; }

        public bool IsDeleted
        {
            get;
            set;
        }
    }

    // Summary:
    //     Entity type for a user's login (i.e. facebook, google)
    //
    // Type parameters:
    //   TKey:
    public class IdentityUserLogin: IAuditable
    {
        public IdentityUserLogin()
        {

        }

        // Summary:
        //     The login provider for the login (i.e. facebook, google)
        public virtual string LoginProvider { get; set; }
        //
        // Summary:
        //     Key representing the login for the provider
        public virtual string ProviderKey { get; set; }
        //
        // Summary:
        //     User Id for the user who owns this login
        public virtual string UserId { get; set; }
    }

    // Summary:
    //     EntityType that represents a user belonging to a role
    //
    // Type parameters:
    //   TKey:
    public class IdentityUserRole:IAuditable
    {
        public IdentityUserRole()
        {

        }

        // Summary:
        //     RoleId for the role
        public virtual string RoleId { get; set; }
        //
        // Summary:
        //     UserId for the user that is in the role
        public virtual string UserId { get; set; }
    }

    // Summary:
    //     EntityType that represents one specific user claim
    //
    // Type parameters:
    //   TKey:
    public class IdentityUserClaim:IDestoyable
    {
        public IdentityUserClaim()
        {

        }

        // Summary:
        //     Claim type
        public virtual string ClaimType { get; set; }
        //
        // Summary:
        //     Claim value
        public virtual string ClaimValue { get; set; }
        //
        // Summary:
        //     Primary key
        public virtual int Id { get; set; }
        //
        // Summary:
        //     User Id for the user who owns this login
        public virtual string UserId { get; set; }
    }

	/// <summary>
	///     Represents a Role entity
	/// </summary>
	/// <typeparam name="TKey"></typeparam>
	/// <typeparam name="TUserRole"></typeparam>
	public class IdentityRole : IRole<string>,IDestoyable
	{
		/// <summary>
		///     Navigation property for users in the role
		/// </summary>
		public virtual ICollection<IdentityUserRole> Users
		{
			get;
			private set;
		}
		/// <summary>
		///     Role id
		/// </summary>
		public string Id
		{
			get;
			set;
		}
		/// <summary>
		///     Role name
		/// </summary>
		public string Name
		{
			get;
			set;
		}
		/// <summary>
		///     Constructor
		/// </summary>
		public IdentityRole()
		{
			this.Users = new List<IdentityUserRole>();
		}
	}


    public class Audit:IEntity,IAddable
    {
        public int Id { get; set; }
        public virtual User InitiatingUser { get; set; }
        public string IP { get; set; }
        public string UserAgent { get; set; }
        public DateTimeOffset EventDate { get; set; }
        public string AuditEvent { get; set; }
        public string ObjectType { get; set; }
        public string Data { get; set; }
    }

    public class ContactMethod : IEntity, IAddable, IDestoyable, IAuditable
    {
        public int Id { get; set; }
        public string Value { get; set; }
        [Display(Name = "Date Added", ShortName = "Added")]
        public DateTimeOffset DateAdded { get; set; }
        public int Order { get; set; }
        public virtual User User { get; set; }
    }

    public class AccomodationVenue : NamedEntity, IAddable, IDeletable, IOwnable, IAuditable
    {
        public string Description { get; set; }
        public virtual LatLng LatLng { get; set; }
        public string Address { get; set; }
        public string PublicAddress { get; set; }
        public virtual Location Location { get; set; }

        public virtual User Owner { get; set; }
        [Display(Name = "Date Added", ShortName = "Added")]
        public DateTimeOffset DateAdded { get; set; }

        public virtual List<AccomodationVenueFeature> Features { get; set; }
        [Display(Name = "Allows Bed Sharing")]
        public bool AllowsBedSharing { get; set; }
        [Display(Name = "Allows Mixed Rooms", ShortName = "Mixed Rooms")]
        public bool AllowsMixedRooms { get; set; }

        public virtual List<Photo> Photos { get; set; }
        public virtual List<AccomodationRoom> Rooms { get; set; }
        public bool IsDeleted { get; set; }

        public string OwnerId { get { return Owner != null ? Owner.Id : null; } }
    }


    public class AccomodationRoom : NamedEntity, IAddable, IDeletable, IAuditable
    {
        public string Description { get; set; }
        public virtual AccomodationVenue Venue { get; set; }
        public virtual List<Bed> Beds { get; set; }

        public virtual List<AccomodationRoomFeature> Features { get; set; }

        [Display(Name = "Date Added", ShortName = "Added")]
        public DateTimeOffset DateAdded { get; set; }

        public virtual List<Photo> Photos { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class BookingRequest : IEntity, IAddable, IDeletable, IAuditable
    {
        public string Notes { get; set; }
        public int Id { get; set; }
        public virtual Bed Bed { get; set; }
        public virtual User User { get; set; }
        public int Guests { get; set; }
        
        public virtual BookingStatus Status { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class AccomodationBedAvailabilityNight : IEntity, IAddable, IDeletable, IAuditable
    {
        public int Id { get; set; }
        public virtual Bed Bed { get; set; }
        public DateTimeOffset Night { get; set; }

        [Display(Name = "Status")]
        public BedStatus BedStatus { get; set; }
        public bool IsDeleted { get; set; }
    }

    public enum BookingStatus
    {
        Draft,
        Deleted,
        Submitted,
        Pending,
        Approved,
        Rejected,
        Tentative,
        Cancelled
    }

    public enum BedStatus
    {
        NotAvailable,
        Available,
        Tentative,
        Booked
    }

    public class UserEventAttendance : IEntity, IAddable, IDeletable, IAuditable
    {
        public int Id { get; set; }
        public virtual User User { get; set; }
        public virtual EventInstance EventInstance { get; set; }
        public bool AutomaticallyAdded { get; set; }
        [Display(Name = "Date Added", ShortName = "Added")]
        public DateTimeOffset DateAdded { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class Photo : IDeletable, IOwnable, IAuditable
    {
        public Guid PhotoId { get; set; }
        public string Caption { get; set; }
        public virtual User Owner { get; set; }

        [Display(Name = "Date Added", ShortName = "Added")]
        public DateTimeOffset DateAdded { get; set; }
        public bool IsDeleted { get; set; }
        public string OwnerId { get { return Owner != null ? Owner.Id : null; } }
    }

    public class Bed : IEntity, IAddable, IDeletable, IAuditable
    {
        public int Id { get; set; }
        public virtual BedSize BedSize { get; set; }
        public virtual BedType BedType { get; set; }
        public virtual AccomodationRoom Room { get; set; }

        [Display(Name = "Date Added", ShortName = "Added")]
        public DateTimeOffset DateAdded { get; set; }

        public virtual List<Photo> Photos { get; set; }
        public bool IsDeleted { get; set; }
    }

    /*public enum BedSize
    {
        Single,
        KingSingle,
        Double,
        Queen,
        King,
        SuperKing,
        Other
    }*/

    public class BedType : NamedEntity
    {
    }

    public class BedSize : NamedEntity
    {
    }

    public class AccomodationVenueFeature : NamedEntity, IAddable, IDeletable, IAuditable
    {
        public bool IsDeleted { get; set; }
        public virtual List<AccomodationVenue> Rooms { get; set; }
    }

    public class AccomodationRoomFeature : NamedEntity, IAddable, IDeletable, IAuditable
    {
        public bool IsDeleted { get; set; }
        public virtual List<AccomodationRoom> Rooms { get; set; }
    }

}

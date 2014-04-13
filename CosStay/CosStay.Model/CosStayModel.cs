using Microsoft.AspNet.Identity.EntityFramework;
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

    public class Event : NamedEntity,IAddable,IDeletable
    {
        public virtual List<EventInstance> Instances { get; set; }
        [Display(Name="URL")]
        public string Url { get; set; }

        public virtual List<Photo> Photos { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class EventInstance : NamedEntity, IAddable, IDeletable
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

        public virtual List<Photo> Photos { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class Location : NamedEntity, IAddable, IDeletable
    {
        public virtual LatLng LatLng { get; set; }
        public virtual List<Venue> Venues { get; set; }

        public virtual List<Photo> Photos { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class Venue : NamedEntity, IAddable, IDeletable
    {
        public virtual LatLng LatLng { get; set; }
        public string Address { get; set; }
        public virtual Location Location { get; set; }

        public virtual List<Photo> Photos { get; set; }
        public bool IsDeleted { get; set; }

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
    }

    public class Country : NamedEntity
    {
        public virtual List<Location> Locations { get; set; }
        public string ShortName { get; set; }
    }

    public class User:IdentityUser
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
    }

    public class Audit:IEntity,IAddable
    {
        public int Id { get; set; }
        public virtual User InitiatingUser { get; set; }
        public string IP { get; set; }
        public string UserAgent { get; set; }
        public DateTimeOffset EventDate { get; set; }
        public string AuditEvent { get; set; }
        public string Data { get; set; }
    }

    public class ContactMethod : IEntity, IAddable, IDestoyable
    {
        public int Id { get; set; }
        public string Value { get; set; }
        [Display(Name = "Date Added", ShortName = "Added")]
        public DateTimeOffset DateAdded { get; set; }
        public int Order { get; set; }
    }

    public class AccomodationVenue : NamedEntity, IAddable, IDeletable
    {
        public virtual LatLng LatLng { get; set; }
        public string Address { get; set; }
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
    }


    public class AccomodationRoom : NamedEntity, IAddable, IDeletable
    {
        public virtual List<Bed> Beds { get; set; }

        public virtual List<AccomodationRoomFeature> Features { get; set; }

        [Display(Name = "Date Added", ShortName = "Added")]
        public DateTimeOffset DateAdded { get; set; }

        public virtual List<Photo> Photos { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class BookingRequest : IEntity, IAddable, IDeletable
    {
        public int Id { get; set; }
        public virtual Bed Bed { get; set; }
        public virtual User User { get; set; }
        public int Guests { get; set; }
        

        public virtual BookingStatus Status { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class AccomodationBedAvailabilityNight : IEntity, IAddable, IDeletable
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

    public class UserEventAttendance : IEntity, IAddable, IDeletable
    {
        public int Id { get; set; }
        public virtual User User { get; set; }
        public virtual EventInstance EventInstance { get; set; }
        public bool AutomaticallyAdded { get; set; }
        [Display(Name = "Date Added", ShortName = "Added")]
        public DateTimeOffset DateAdded { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class Photo:IDeletable
    {
        public Guid PhotoId { get; set; }
        public string Caption { get; set; }
        public virtual User Owner { get; set; }

        [Display(Name = "Date Added", ShortName = "Added")]
        public DateTimeOffset DateAdded { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class Bed:IEntity,IAddable,IDeletable
    {
        public int Id { get; set; }
        public virtual BedSize BedSize { get; set; }
        public virtual BedType BedType { get; set; }

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

    public class AccomodationVenueFeature : NamedEntity,IAddable,IDeletable
    {
        public bool IsDeleted { get; set; }
    }

    public class AccomodationRoomFeature : NamedEntity,IAddable,IDeletable
    {
        public bool IsDeleted { get; set; }
    }

}

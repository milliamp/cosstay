using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosStay.Model
{
    public class NamedEntity
    {
        public string Name { get; set; }
    }
    public class Event : NamedEntity
    {
        public int EventId { get; set; }
        public virtual List<EventInstance> Instances { get; set; }
        public string Url { get; set; }

        public virtual List<Photo> Photos { get; set; }
    }

    public class EventInstance : NamedEntity
    {
        public int EventInstanceId { get; set; }
        public virtual Event Event { get; set; }
        public string Url { get; set; }
        public string FacebookEventId { get; set; }
        public string Description { get; set; }
        public virtual Venue Venue { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        
        public DateTimeOffset DateUpdated { get; set; }

        public virtual List<Photo> Photos { get; set; }
    }

    public class Location : NamedEntity
    {
        public int LocationId { get; set; }
        public LatLng LatLng { get; set; }
        public virtual List<Venue> Venues { get; set; }

        public virtual List<Photo> Photos { get; set; }
    }

    public class Venue : NamedEntity
    {
        public int VenueId { get; set; }
        public virtual LatLng? LatLng { get; set; }
        public string Address { get; set; }
        public virtual Location Location { get; set; }

        public virtual List<Photo> Photos { get; set; }
    }

    public struct LatLng
    {
        public float Lat { get; set; }
        public float Lng { get; set; }
    }

    public class Country : NamedEntity
    {
        public int CountryId { get; set; }
        public virtual List<Location> Locations { get; set; }
        public string ShortName { get; set; }
    }

    public class User:IdentityUser
    {
        public string Name { get; set; }
        public virtual Location Location { get; set; }

        public DateTimeOffset JoinDate { get; set; }
        public DateTimeOffset LastSeen { get; set; }
        public DateTimeOffset DetailsUpdatedDate { get; set; }

        //public virtual List<Role> Roles { get; set; }
        public virtual List<ContactMethod> ContactMethods { get; set; }
    }

    public class Audit
    {
        public int AuditId { get; set; }
        public virtual User InitiatingUser { get; set; }
        public string IP { get; set; }
        public string UserAgent { get; set; }
        public DateTimeOffset EventDate { get; set; }
        public string AuditEvent { get; set; }
        public string Data { get; set; }
    }

    public class ContactMethod
    {
        public int ContactMethodId { get; set; }
        public string Value { get; set; }
        public DateTimeOffset DateAdded { get; set; }
        public int Order { get; set; } 
    }

    public class AccomodationVenue : NamedEntity
    {
        public int AccomodationVenueId { get; set; }
        public virtual LatLng? LatLng { get; set; }
        public string Address { get; set; }
        public virtual Location Location { get; set; }

        public virtual User Owner { get; set; }

        public DateTimeOffset DateAdded { get; set; }

        public virtual List<AccomodationVenueFeature> Features { get; set; }
        public bool AllowsBedSharing { get; set; }
        public bool AllowsMixedRooms { get; set; }

        public virtual List<Photo> Photos { get; set; }
    }


    public class AccomodationRoom
    {
        public int AccomodationRoomId { get; set; }
        public virtual AccomodationVenue AccomodationVenue { get; set; }
        public virtual List<Bed> Beds { get; set; }

        public virtual List<AccomodationRoomFeature> Features { get; set; }
        public DateTimeOffset DateAdded { get; set; }

        public virtual List<Photo> Photos { get; set; }
    }

    public class BookingRequest
    {
        public int BookingRequestId { get; set; }
        public virtual Bed Bed { get; set; }
        public virtual User User { get; set; }
        public int Guests { get; set; }
        

        public virtual BookingStatus Status { get; set; }
    }

    public class AccomodationBedAvailabilityNight
    {
        public int AccomodationBedAvailabilityNightId { get; set; }
        public virtual Bed Bed { get; set; }
        public DateTimeOffset Night { get; set; }
        public BedStatus BedStatus { get; set; }
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

    public class UserEventAttendance
    {
        public int UserEventAttendanceId { get; set; }
        public virtual User User { get; set; }
        public virtual EventInstance EventInstance { get; set; }
        public bool AutomaticallyAdded { get; set; }
        public DateTimeOffset DateAdded { get; set; }
    }

    public class Photo
    {
        public Guid PhotoId { get; set; }
        public string Caption { get; set; }
        public virtual User Owner { get; set; }

        public DateTimeOffset DateAdded { get; set; }
    }

    public class Bed
    {
        public int BedId { get; set; }
        public virtual BedSize Size { get; set; }
        public virtual BedType Type { get; set; }

        public DateTimeOffset DateAdded { get; set; }

        public virtual List<Photo> Photos { get; set; }
    }

    public enum BedSize
    {
        Single,
        KingSingle,
        Double,
        Queen,
        King,
        SuperKing,
        Other
    }

    public class BedType : NamedEntity
    {
        public int BedTypeId { get; set; }
    }

    public class AccomodationVenueFeature : NamedEntity
    {
        public int AccomodationVenueFeatureId { get; set; }
    }


    public class AccomodationRoomFeature : NamedEntity
    {
        public int AccomodationRoomFeatureId { get; set; }
    }

}

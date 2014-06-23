using AutoMapper;
using CosStay.Model;
using CosStay.Site.Models;
using System.Linq;
using System.Web.Mvc;

namespace CosStay.Site
{
    public static class AutoMapperConfig
    {
        public static void RegisterMaps()
        {
            Mapper.CreateMap<AccomodationRoom, AccomodationRoomViewModel>()
                .ReverseMap()
                .ForMember(r => r.Beds, c => c.UseDestinationValue());
            Mapper.CreateMap<AccomodationVenue, AccomodationVenueViewModel>()
                .ForMember("TotalBeds", c=> c.MapFrom(v => v.Rooms.Where(r => !r.IsDeleted).Select(r => r.Beds.Count).Sum()))
                .ReverseMap()
                .ForMember(r => r.Rooms, c => c.UseDestinationValue());

            Mapper.CreateMap<Bed, BedViewModel>()
                .ForMember(b => b.BedSizeId, c => c.MapFrom(b => b.BedSize.Id))
                .ForMember(b => b.BedTypeId, c => c.MapFrom(b => b.BedType.Id))
                .ForMember(b => b.Description, c=> c.MapFrom(b => string.Format("{0} - {1}", b.BedSize.Name, b.BedType.Name)))
                .ForMember(b => b.RoomName, c => c.MapFrom(b => b.Room.Name));
            Mapper.CreateMap<BedViewModel, Bed>()
                .ForMember(b => b.BedSize, c => c.MapFrom(b => new BedSize() { Id = b.BedSizeId }))
                .ForMember(b => b.BedType, c => c.MapFrom(b => new BedType() { Id = b.BedTypeId }));
            Mapper.CreateMap<User, UserViewModel>().ReverseMap();
            Mapper.CreateMap<Location, LocationViewModel>().ReverseMap();
            Mapper.CreateMap<Bed, BedAvailabilityViewModel>()
                .ForMember(b => b.BedSizeId, c => c.MapFrom(b => b.BedSize.Id))
                .ForMember(b => b.BedTypeId, c => c.MapFrom(b => b.BedType.Id))
                .ForMember(b => b.Description, c=> c.MapFrom(b => string.Format("{0} - {1}", b.BedSize.Name, b.BedType.Name)))
                .ForMember(b => b.RoomName, c => c.MapFrom(b => b.Room.Name));
            Mapper.CreateMap<Photo, PhotoViewModel>()
                .ForMember(p => p.Id, c => c.MapFrom(p => p.PhotoId))
                .ForMember(p => p.Url, c => c.MapFrom(p => new UrlHelper().Photo(p.PhotoId, null, null)));
            //Mapper.AssertConfigurationIsValid();
        }
    }
}
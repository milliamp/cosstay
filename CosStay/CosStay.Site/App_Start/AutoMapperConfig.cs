using AutoMapper;
using CosStay.Model;
using CosStay.Site.Models;

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
                .ReverseMap()
                .ForMember(r => r.Rooms, c => c.UseDestinationValue());

            Mapper.CreateMap<Bed, BedViewModel>()
                .ForMember(b => b.BedSizeId, c => c.MapFrom(b => b.BedSize.Id))
                .ForMember(b => b.BedTypeId, c => c.MapFrom(b => b.BedType.Id));
            Mapper.CreateMap<BedViewModel, Bed>()
                .ForMember(b => b.BedSize, c => c.MapFrom(b => new BedSize() { Id = b.BedSizeId }))
                .ForMember(b => b.BedType, c => c.MapFrom(b => new BedType() { Id = b.BedTypeId }));
            Mapper.CreateMap<User, UserViewModel>().ReverseMap();
            //Mapper.AssertConfigurationIsValid();
        }
    }
}
using AutoMapper;
using CityProject.Entities;
using CityProject.Models;

namespace CityProject.MappingProfiles
{
    public class HotelMapperProfile : Profile
    {
        public HotelMapperProfile()
        {
            CreateMap<Hotel, HotelDto>().ReverseMap();
        }
    }
}
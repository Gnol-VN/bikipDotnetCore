using AutoMapper;
using CityProject.Entities;
using CityProject.Models;

namespace CityProject.MappingProfiles
{
    public class CityMapperProfile : Profile
    {
        public CityMapperProfile()
        {
            CreateMap<City, CityDto>().ReverseMap();
            CreateMap<City, CityDToWithoutHotel>().ReverseMap();
        }
    }
}
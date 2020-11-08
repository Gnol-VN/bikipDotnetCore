using AutoMapper;
using CityProject.Entities;
using CityProject.Models;

namespace CityProject.MappingProfiles
{
    public class CityMapper : Profile
    {
        public CityMapper()
        {
            CreateMap<City, CityDto>();
            CreateMap<City, CityDToWithoutHotel>();
        }
    }
}
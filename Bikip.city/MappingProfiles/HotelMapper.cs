using AutoMapper;
using CityProject.Entities;
using CityProject.Models;

namespace CityProject.MappingProfiles
{
    public class HotelMapper : Profile
    {
        public HotelMapper()
        {
            CreateMap<Hotel, HotelDto>();
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using CityProject.Entities;

namespace CityProject.Repositories
{
    public interface ICityRepository
    {
        IEnumerable<City> GetCities();

        City GetCity(long cityId, bool includeHotels);

        IEnumerable<Hotel> GetHotelsInOneCity(long cityId);

        Hotel GetHotel(long cityId, long hotelId);

        bool IsCityExists(long cityId);

        bool Save();
    }
}
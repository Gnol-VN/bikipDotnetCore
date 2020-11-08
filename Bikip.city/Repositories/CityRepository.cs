using System;
using System.Collections.Generic;
using System.Linq;
using CityProject.Contexts;
using CityProject.Entities;
using Microsoft.EntityFrameworkCore;

namespace CityProject.Repositories
{
    public class CityRepository : ICityRepository
    {
        private readonly CityDbContext _cityDbContext; // Concrete type

        public CityRepository(CityDbContext dbContext)
        {
            _cityDbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public IEnumerable<City> GetCities()
        {
            return _cityDbContext.Cities.ToList();
        }

        public City GetCity(long cityId, bool includeHotels)
        {
            if (includeHotels)
            {
                return _cityDbContext.Cities
                    .Include(city => city.HotelList)
                    .FirstOrDefault(city => city.Id == cityId);
            }

            return _cityDbContext.Cities.FirstOrDefault(city => city.Id == cityId);
        }

        public IEnumerable<Hotel> GetHotelsInOneCity(long cityId)
        {
            return _cityDbContext.Hotels.Where(hotel => hotel.CityId == cityId).ToList();
        }

        public Hotel GetHotel(long cityId, long hotelId)
        {
            return _cityDbContext.Hotels.FirstOrDefault(hotel => hotel.CityId == cityId && hotel.Id == hotelId);
        }
    }
}
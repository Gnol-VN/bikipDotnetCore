using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityProject.Model
{
    public class City
    {
        public long id { get; set; }
        public string CityName { get; set; }

        public List<Hotel> HotelList { get; set; }

        public static List<City> CityList { get; set; } = new List<City>()
        {
            new City(1, "Hanoi"),
            new City(2, "Dublin"),
        };

        public City(long id, string cityName)
        {
            this.id = id;
            CityName = cityName;
            HotelList = new List<Hotel>();
        }

        public City()
        {
        }
    }
}

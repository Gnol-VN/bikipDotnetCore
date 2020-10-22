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

        public City(long id, string cityName)
        {
            this.id = id;
            CityName = cityName;
        }

        public City()
        {
        }
    }
}

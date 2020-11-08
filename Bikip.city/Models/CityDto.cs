using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityProject.Models
{
    public class CityDto
    {
        public long Id { get; set; }
        public string CityName { get; set; }

        public List<HotelDto> HotelList { get; set; }

        public static List<CityDto> CityList { get; set; } 

        public CityDto(long id, string cityName)
        {
            this.Id = id;
            CityName = cityName;
            HotelList = new List<HotelDto>();
        }

        public CityDto()
        {
        }
    }
}

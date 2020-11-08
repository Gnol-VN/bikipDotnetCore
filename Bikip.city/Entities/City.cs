using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CityProject.Entities
{
    public class City
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        public string CityName { get; set; }

        public List<Hotel> HotelList { get; set; }

        public City()
        {
            HotelList = new List<Hotel>();
        }
    }
}
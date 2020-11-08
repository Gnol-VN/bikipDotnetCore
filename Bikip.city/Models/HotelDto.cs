using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CityProject.Models
{
    public class HotelDto
    {
        [Required(ErrorMessage = "Custom message: hotel name is required")]
        public string HotelName { get; set; }

        [Required(ErrorMessage = "Custom message: email is required")]
        [EmailAddress(ErrorMessage = "Custom message: email is not in correct format")]
        public string Email { get; set; }
    }
}

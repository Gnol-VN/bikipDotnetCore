using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CityProject.Entities
{
    public class Hotel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required(ErrorMessage = "Custom message: hotel name is required")]
        public string HotelName { get; set; }

        [Required(ErrorMessage = "Custom message: email is required")]
        [EmailAddress(ErrorMessage = "Custom message: email is not in correct format")]
        public string Email { get; set; }

        [ForeignKey("CityId")] //Match to property CityId
        public City City { get; set; }

        public long CityId { get; set; }
    }
}
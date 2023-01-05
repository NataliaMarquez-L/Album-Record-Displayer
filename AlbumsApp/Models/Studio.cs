using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AlbumsApp.Models
{
    public class Studio
    {
        public int StudioId { get; set; }


        [Required(ErrorMessage = "Studio name is required.")]
        [MaxLength(64, ErrorMessage ="The name must contain less that 64 characters")]
        public string Name { get; set; }

        [Required]
        [Url(ErrorMessage ="Must be a valid URL")]
        public string Url { get; set; }


        [Required(ErrorMessage ="Adress is required")]
        public string Address { get; set; }


        [Required(ErrorMessage = "City is required")]
        public string City { get; set; }

        [Required]
        [RegularExpression(@"^\d{5}$", ErrorMessage ="the zipCode must be 5 numbers long.")]
        public string ZipCode { get; set; }
    }
}

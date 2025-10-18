using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCareBLL.ViewModels.AddressViewModel
{
    public class CreateAddressViewModel
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        [Required(ErrorMessage = "BuildingNumber is Required")]
        [Range(1, int.MaxValue, ErrorMessage = "BuildingNumber must be a positive integer")]
        public int BuildingNumber { get; set; }


        [Required(ErrorMessage = "Street is Required")]
        public string Street { get; set; } = null!;


        [Required(ErrorMessage = "City is Required")]
        public string City { get; set; } = null!;


        [Required(ErrorMessage = "ZipCode is Required")]
        public int ZipCode { get; set; }

    }
}

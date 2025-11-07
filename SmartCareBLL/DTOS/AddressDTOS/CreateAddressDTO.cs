using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCareBLL.DTOS.AddressDTOS
{
    public class CreateAddressDTO
    {
        public int UserId { get; set; }
        public int BuildingNumber { get; set; }
        public string Street { get; set; } = null!;
        public string City { get; set; } = null!;
        public int ZipCode { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCareBLL.ViewModels.AddressViewModel
{
    public class AddressViewModel
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public int BuildingNumber { get; set; }
        public string Street { get; set; } = null!;
        public string City { get; set; } = null!;
        public int ZipCode { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCareDAL.Models
{
    public class Address : BaseEntity
    {
        public int UserId { get; set; }

        public int BuildingNumber { get; set; }
        public string Street { get; set; } = null!;
        public string City { get; set; } = null!;
        public int ZipCode { get; set; }

        // Navigation property
        public User? User { get; set; }

    }
}

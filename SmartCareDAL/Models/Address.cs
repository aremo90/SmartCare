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

        public string BuildingNumber { get; set; } = null!;
        public string Street { get; set; } = null!;
        public string City { get; set; } = null!;

        // Navigation property
        public User? User { get; set; }

    }
}

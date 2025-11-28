using LinkO.Domin.Models.IdentityModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkO.Domin.Models
{
    public class GpsLocation : BaseEntity
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public string UserId { get; set; }
        public ApplicationUser User { get; set; } = null!;
    }
}

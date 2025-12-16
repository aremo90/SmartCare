using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkO.Shared.DTOS.ProductDTOS
{
    public class AddProductDTO
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Price { get; set; }
        public string ImageUrl { get; set; } = null!;
        public string ImageAlt { get; set; } = null!;
        public int TypeId { get; set; }
    }
}

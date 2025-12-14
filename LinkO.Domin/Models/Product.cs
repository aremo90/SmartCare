using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkO.Domin.Models
{
    public class Product : BaseEntity<int>
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Price { get; set; }
        public string ImageUrl { get; set; } = null!;
        public string ImageAlt { get; set; } = null!;


        #region RS

        public int TypeId { get; set; }
        public ProductType ProductType { get; set; }

        #endregion
    }
}

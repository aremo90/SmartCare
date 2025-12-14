using LinkO.Shared.CommonResult;
using LinkO.Shared.DTOS.ProductDTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkO.ServiceAbstraction
{
    public interface IProductService
    {
        Task<Result<IEnumerable<ProductDTO>>> GetAllProductAsync();
        Task<Result<ProductDTO>> GetProductByIdAsync(int id);
        Task<Result<IEnumerable<TypeDTO>>> GetAllTypesAsync();
    }
}

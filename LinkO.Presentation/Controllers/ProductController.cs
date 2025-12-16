using LinkO.ServiceAbstraction;
using LinkO.Shared.DTOS.MedicineReminderDTOS;
using LinkO.Shared.DTOS.ProductDTOS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkO.Presentation.Controllers
{
    public class ProductController : ApiBaseController
    {
        private readonly IProductService productService;

        public ProductController(IProductService productService)
        {
            this.productService = productService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetAllProductsAsync()
        {
            var result = await productService.GetAllProductAsync();
            return HandleResult<IEnumerable<ProductDTO>>(result);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDTO>> GetProductByIdAsync(int id)
        {
            var result = await productService.GetProductByIdAsync(id);
            return HandleResult<ProductDTO>(result);
        }
        [HttpGet("types")]
        public async Task<ActionResult<IEnumerable<TypeDTO>>> GetAllTypesAsync()
        {
            var result = await productService.GetAllTypesAsync();
            return HandleResult<IEnumerable<TypeDTO>>(result);
        }
        [Authorize]
        [HttpPut]
        public async Task<ActionResult<ProductDTO>> UpdateProduct([FromBody] UpdateProductDTO updateProductDTO, [FromQuery] int id)
        {
            var result = await productService.UpdateProduct(id, GetUserEmail(), updateProductDTO);
            return HandleResult<ProductDTO>(result);
        }
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<ProductDTO>> AddProductAsync([FromBody] AddProductDTO addProductDTO)
        {
            var result = await productService.AddProductAsync(GetUserEmail(), addProductDTO);
            return HandleResult<ProductDTO>(result);
        }
        [Authorize]
        [HttpDelete]
        public async Task<ActionResult<bool>> DeleteProductAsync([FromQuery]int id)
        {
            var result = await productService.DeleteProductAsync(id, GetUserEmail());
            return HandleResult<bool>(result);
        }
    }
}

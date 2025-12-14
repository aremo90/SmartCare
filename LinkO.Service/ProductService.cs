using AutoMapper;
using LinkO.Domin.Contract;
using LinkO.Domin.Models;
using LinkO.Service.Specification.ProductSpec;
using LinkO.ServiceAbstraction;
using LinkO.Shared.CommonResult;
using LinkO.Shared.DTOS.AddressDTOS;
using LinkO.Shared.DTOS.MedicineReminderDTOS;
using LinkO.Shared.DTOS.ProductDTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkO.Service
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public ProductService(IUnitOfWork unitOfWork , IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<Result<IEnumerable<ProductDTO>>> GetAllProductAsync()
        {
            var spec = new ProductWithTypeSpecification();
            var products = await unitOfWork.GetRepository<Product , int>().GetAllAsync(spec);
            if (products is null || !products.Any())
                return Error.NotFound();
            var ProductsDto = mapper.Map<IEnumerable<ProductDTO>>(products);
            return Result<IEnumerable<ProductDTO>>.Ok(ProductsDto);
        }

        public async Task<Result<IEnumerable<TypeDTO>>> GetAllTypesAsync()
        {
            var types = await unitOfWork.GetRepository<ProductType , int>().GetAllAsync();
            if (types is null || !types.Any())
                return Error.NotFound();
            var TypesDto = mapper.Map<IEnumerable<TypeDTO>>(types);
            return Result<IEnumerable<TypeDTO>>.Ok(TypesDto);
        }

        public async Task<Result<ProductDTO>> GetProductByIdAsync(int id)
        {
            var spec = new ProductWithTypeSpecification(id);
            var product = await unitOfWork.GetRepository<Product , int>().GetByIdAsync(spec);
            if (product is null)
                return Error.NotFound();
            return mapper.Map<ProductDTO>(product);
        }
    }
}

using AutoMapper;
using LinkO.Domin.Contract;
using LinkO.Domin.Models;
using LinkO.Domin.Models.IdentityModule;
using LinkO.Service.Specification.ProductSpec;
using LinkO.ServiceAbstraction;
using LinkO.Shared.CommonResult;
using LinkO.Shared.DTOS.AddressDTOS;
using LinkO.Shared.DTOS.MedicineReminderDTOS;
using LinkO.Shared.DTOS.ProductDTOS;
using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<ApplicationUser> _userManager;

        public ProductService(IUnitOfWork unitOfWork , IMapper mapper , UserManager<ApplicationUser> userManager)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            _userManager = userManager;
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

        public async Task<Result<ProductDTO>> UpdateProduct(int id, string email, UpdateProductDTO updateProductDTO)
        {
            // Check User Is Admin
            var User = await _userManager.FindByEmailAsync(email);
            if (User == null || !await _userManager.IsInRoleAsync(User, "Admin"))
                return Error.Unauthorized("You are not authorized to perform this action.");
            var spec = new ProductWithTypeSpecification(id);
            var product = await unitOfWork.GetRepository<Product , int>().GetByIdAsync(spec);
            if (product is null)
                return Error.NotFound();
            // Update Product Logic Here (Example: updating the name)
            product.Name = updateProductDTO.Name ?? product.Name;
            product.Description = updateProductDTO.Description ?? product.Description;
            product.Price = updateProductDTO.Price ?? product.Price;
            product.ImageUrl = updateProductDTO.ImageUrl ?? product.ImageUrl;
            product.ImageAlt = updateProductDTO.ImageAlt ?? product.ImageAlt;
            unitOfWork.GetRepository<Product , int>().Update(product);
            await unitOfWork.SaveChangesAsync();
            return mapper.Map<ProductDTO>(product);
        }
        public async Task<Result<ProductDTO>> AddProductAsync(string email, AddProductDTO addProductDTO)
        {
            var User = await _userManager.FindByEmailAsync(email);
            if (User == null || !await _userManager.IsInRoleAsync(User, "Admin"))
                return Error.Unauthorized("You are not authorized to perform this action.");

            var product = mapper.Map<Product>(addProductDTO);
            await unitOfWork.GetRepository<Product , int>().AddAsync(product);
            await unitOfWork.SaveChangesAsync();
            return mapper.Map<ProductDTO>(product);
        }

        public async Task<Result<bool>> DeleteProductAsync(int id, string email)
        {
            var User = await _userManager.FindByEmailAsync(email);
            if (User == null || !await _userManager.IsInRoleAsync(User, "Admin"))
                return Error.Unauthorized("You are not authorized to perform this action.");
            var spec = new ProductWithTypeSpecification(id);
            var product = await unitOfWork.GetRepository<Product , int>().GetByIdAsync(spec);
            if (product is null)
                return Error.NotFound();

            unitOfWork.GetRepository<Product , int>().Delete(product);
            await unitOfWork.SaveChangesAsync();
            return Result<bool>.Ok(true);

        }
    }
}

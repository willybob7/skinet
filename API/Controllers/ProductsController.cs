using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using skinet.Core.Interfaces;
using skinet.Core.Specifications;
using skinet.API.DTOs;
using AutoMapper;
using skinet.API.Controllers;
using skinet.API.Errors;
using Microsoft.AspNetCore.Http;
using skinet.API.Helpers;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : BaseApiController
    {
        private readonly IGenericRepository<Product> _productsRepo;
        private readonly IGenericRepository<ProductBrand> _productBrandRepo;
        private readonly IGenericRepository<ProductType> _productTypeRepo;
        private readonly IMapper _mapper;

        // private readonly StoreContext _context;
        // private readonly IProductRepository _repo;
        public ProductsController(IGenericRepository<Product> productsRepo,
        IGenericRepository<ProductBrand> productBrandRepo,
        IGenericRepository<ProductType> productTypeRepo, IMapper mapper
            // IProductRepository repo
        // IProductRepository context
        )
        {
            // _repo = repo;
            _productsRepo = productsRepo;
            _productBrandRepo = productBrandRepo;
            _productTypeRepo = productTypeRepo;
            _mapper = mapper;
            // _context = context;

        }

        [HttpGet]
        public async Task<ActionResult<Pagination<ProductToReturnDTO>>> GetProducts(
            [FromQuery]ProductSpecParams productSpecParams)
        {
            // var products = await _repo.GetProductsAsync();
            // var products = await _productsRepo.ListAllAsync();
            var spec = new ProductsWithTypesAndBrandsSpecification(productSpecParams);

            var countSpec = new ProductsWithFiltersForCountSpecification(productSpecParams);

            var totalItems = await _productsRepo.CountAsync(countSpec);

            var products = await _productsRepo.ListAsync(spec);

            var data = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDTO>>(products);
            // return products.Select(product => new ProductToReturnDTO(){
            //     Id = product.Id,
            //     Name = product.Name,
            //     Description = product.Description,
            //     PictureUrl = product.PictureUrl,
            //     Price = product.Price,
            //     ProductBrand = product.ProductBrand.Name,
            //     ProductType = product.ProductType.Name
            // }).ToList();

            var result = new Pagination<ProductToReturnDTO>(){
                PageIndex = productSpecParams.PageIndex,
                PageSize = productSpecParams.PageSize,
                Count = totalItems,
                Data = data
            };

            return Ok(result);
        }

        [HttpGet("{id}")]
        //these tell swagger what kind of response it's getting.
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductToReturnDTO>> GetProduct(int id)
        {
            var spec = new ProductsWithTypesAndBrandsSpecification(id);

            // return await _repo.GetProductByIdAsync(id); 
            // return await _productsRepo.GetByIdAsync(id);
            //returns a json object
            // return await _productsRepo.GetEntityWithSpec(spec);

            var product = await _productsRepo.GetEntityWithSpec(spec);

            if (product is null) return NotFound(new ApiResponse(404));

            return _mapper.Map<Product, ProductToReturnDTO>(product);
        }

        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductBrands(){
            
            // return Ok(await _repo.GetProductBrandsAsync());
            return Ok(await _productBrandRepo.ListAllAsync());
        }

        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetProductTypes()
        {
            // return Ok(await _repo.GetProductTypesAsync());
            return Ok(await _productTypeRepo.ListAllAsync());
        }
    }
}
using API.DTO;
using API.Errors;
using API.Helpers;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class ProductsController : BaseApiController
    {
        private readonly IGenericRepository<Product> productsRepository;
        private readonly IGenericRepository<ProductBrand> productBrandsRepository;
        private readonly IGenericRepository<ProductType> productTypesRepository;
        private readonly IMapper mapper;

        public ProductsController(
            IGenericRepository<Product> productsRepository,
            IGenericRepository<ProductBrand> productBrandsRepository,
            IGenericRepository<ProductType> productTypesRepository,
            IMapper mapper)
        {
            this.productsRepository = productsRepository;
            this.productBrandsRepository = productBrandsRepository;
            this.productTypesRepository = productTypesRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<Pagination<ProductToReturnDto>>> GetProducts(
            [FromQuery]ProductSpecParams productParams)
        {
            var specification = new ProductsWithTypesAndBrandsSpecification(productParams);
            var countSpecification = new ProductWithFiltersForCountSpecification(productParams);
            var totalItems = await productsRepository.CountAsync(countSpecification);
            var products = await productsRepository.ListAsync(specification);
            var data = mapper.
                Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(products);

            return Ok(new Pagination<ProductToReturnDto>(
                productParams.PageIndex,
                productParams.PageSize,
                totalItems,
                data
            ));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
        {
            var specification = new ProductsWithTypesAndBrandsSpecification(id);
            var product = await productsRepository.GetEntityWithSpecification(specification);

            if (product == null) return NotFound(new ApiResponse(404));

            return mapper.Map<Product, ProductToReturnDto>(product);
        }

        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductBrands()
        {
            return Ok(await productBrandsRepository.ListAllAsync());
        }

        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetProductTypes()
        {
            return Ok(await productTypesRepository.ListAllAsync());
        }
    }
}
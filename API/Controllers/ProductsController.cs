using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Models;
using Microsoft.AspNetCore.Mvc;
using Core.Interfaces;
using Core.Specifications;
using API.Dtos;
using AutoMapper;
using API.Errors;
using Microsoft.AspNetCore.Http;
using API.Helpers;

namespace API.Controllers
{
    public class ProductsController : BaseApiController
    {
        private readonly IGenericRepository<ProductBrand> _productBrandsRepo;
        private readonly IMapper _mapper;
        private readonly IGenericRepository<Product> _productsRepo;
        private readonly IGenericRepository<ProductType> _productTypesRepo;

        public ProductsController(IGenericRepository<Product> productsRepo,
        IGenericRepository<ProductType> productTypesRepo,
        IGenericRepository<ProductBrand> productBrandsRepo,
        IMapper mapper)
        {
            this._productsRepo = productsRepo;
            this._productTypesRepo = productTypesRepo;
            this._productBrandsRepo = productBrandsRepo;
            this._mapper = mapper;
        }

        [HttpGet]
        /// <summary>
        /// Assynchronously sets the Specifications and brings a list of products according to it.
        /// </summary>
        /// <param name="productParam">A class with properties,as sort and filters , to configure our query</param>
        /// <returns>List of products</returns>
        public async Task<ActionResult<Pagination<ProductToReturnDto>>> GetProducts(
            //FromQuery is necessary to inform the API that the parameters will be passed by Query String.
            //It would work properly without it if instead pass a class in GetProducts arguments, we pass the argmuments individually 
            [FromQuery] ProductSpecParams productParams
            )
        {
            var spec = new ProductsWithTypesAndBrandsSpecification(productParams);

            var countSpec = new ProducWithFiltersForCountSpecification(productParams);

            var totalItems = await _productsRepo.CountAsync(countSpec);

            var products = await _productsRepo.ListAsync(spec);

            var data = this._mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(products);

            return Ok(new Pagination<ProductToReturnDto>(productParams.PageIndex, productParams.PageSize, totalItems, data));
        }

        [HttpGet("{id}")]
        //These attributes are useful to indicate what kind of return each method has.
        //It also improves Swagger documentation.
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
        {
            var spec = new ProductsWithTypesAndBrandsSpecification(id);

            //GetEntityWithSpec is a Query
            Product product = await this._productsRepo.GetEntityWithSpec(spec);

            if (product == null) return NotFound(new ApiResponse(404));
            //makes the automapper between the two entities.
            return this._mapper.Map<Product, ProductToReturnDto>(product);
        }

        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductBrands()
        {
            IReadOnlyList<ProductBrand> brands = await this._productBrandsRepo.ListAllAsync();
            return Ok(brands);
        }

        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetProductTypes()
        {
            IReadOnlyList<ProductType> types = await this._productTypesRepo.ListAllAsync();
            return Ok(types);
        }
    }
}
using AutoMapper;
using DoAn3API.Authorize.CustomAuthorize;
using DoAn3API.Dtos.Products;
using DoAn3API.Services.Products;
using DoAn3API.Constants;
using Domain.Common.Paging;
using Domain.Common.Wrappers;
using Domain.Entities.Catalog;
using Infastructure.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DoAn3API.Controllers.Catalog
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IProductService _productService;
        private readonly IGenericRepository<Product> _productRepository;

        public ProductsController(
            IMapper mapper,
            IProductService productService,
            IGenericRepository<Product> productRepository)
        {
            _mapper = mapper;
            _productService = productService;
            _productRepository = productRepository;
        }

        // GET: api/<ProductsController>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [AllowAnonymous]
        public IActionResult GetAll([FromQuery] ProductPagedRequestDto pagedRequestDto)
        {
            var listProduct =  _productService.GetAllProductPaging(pagedRequestDto);
            //listProduct ?? Enumerable.Empty<ProductDto>()
            var metadata = new
            {
                listProduct.TotalCount,
                listProduct.PageSize,
                listProduct.CurrentPage,
                listProduct.TotalPages,
                listProduct.HasNext,
                listProduct.HasPrevious
            };
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

            return Ok(new PagedReponse<List<ProductDto>>(listProduct) { 
                TotalRecord = listProduct.TotalCount,
                PageSize = listProduct.PageSize,
                TotalPages = listProduct.TotalPages,
                CurrentPage = listProduct.CurrentPage,
                HasNext = listProduct.HasNext,
                HasPrevious = listProduct.HasPrevious,
                PageNumber = pagedRequestDto.PageNumber

            });
        }

        // GET api/<ProductsController>/5
        [AllowAnonymous]
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Get(int id)
        {
            if(id < 0)
            {
                return BadRequest();
            }

            var product = await _productService.GetProductById(id);

            if(product == null)
            {
                return BadRequest("Cannot found entity");
            }

            return Ok(_mapper.Map<ProductDto>(product));
        }

        [CustomAuthorize(NamePermissions.Product.View)]
        [HttpGet("admin/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAdminProduct(int id)
        {
            if (id < 0)
            {
                return BadRequest();
            }

            var product = await _productService.GetProductById(id);

            if (product == null)
            {
                return BadRequest("Cannot found entity");
            }

            return Ok(_mapper.Map<ProductDto>(product));
        }

        // POST api/<ProductsController>
        [HttpPost]
        [CustomAuthorize(NamePermissions.Product.Create)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post([FromForm] CreateProductDto createProductDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var productId = await _productService.CreateProduct(createProductDto);
            var productDto = await _productService.GetProductById(productId);

            return Ok(productDto);

        }

        // PUT api/<ProductsController>/5
        [HttpPut("{id}")]
        [CustomAuthorize(NamePermissions.Product.Edit)]
        public async Task<IActionResult> Put(int id, [FromForm] UpdateProductDto productDto)
        {
            if (id < 0)
                return BadRequest();

            var product = await _productRepository.GetById(id);

            if (product == null)
            {
                return BadRequest("Cannot found entity");
            }

            var productUpdated = await _productService.UpdateProduct(productDto);

            return Ok(productUpdated);
        }

        // DELETE api/<ProductsController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id < 0)
                return BadRequest();

            var product = await _productService.GetProductById(id);

            if (product == null)
            {
                return BadRequest("Cannot found entity");
            }

            await _productService.DeleteProduct(id);

            return Ok();
        }
        
        [HttpGet("updateImage")]
        public async Task<IActionResult> UpdateRandomImage()
        {
            var products = await _productService.GetAllProduct();

            foreach (var item in products)
            {
                var productUpdateDto = _mapper.Map<UpdateProductDto>(item);
                await _productService.UpdateDefaultImage(productUpdateDto);
            }


            return Ok();
        }

        [HttpGet("GetProductByCategory")]
        public IActionResult GetProductByCategory([FromQuery] ProductPagedRequestDto requestDto, int categoryId)
        {
            try
            {
                var listProduct =  _productService.GetProductByCategoryId(requestDto, categoryId);

                var metadata = new
                {
                    listProduct.TotalCount,
                    listProduct.PageSize,
                    listProduct.CurrentPage,
                    listProduct.TotalPages,
                    listProduct.HasNext,
                    listProduct.HasPrevious
                };
                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

                return Ok(new PagedReponse<List<ProductDto>>(listProduct)
                {
                    TotalRecord = listProduct.TotalCount,
                    PageSize = listProduct.PageSize,
                    TotalPages = listProduct.TotalPages,
                    CurrentPage = listProduct.CurrentPage,
                    HasNext = listProduct.HasNext,
                    HasPrevious = listProduct.HasPrevious,
                    PageNumber = requestDto.PageNumber

                });
            }
            catch (Exception e)
            {
                return BadRequest(new ResponseResult<object>(e.Message));
            }
        }

        [HttpGet("GetProductByName")]
        public IActionResult GetProductByName([FromQuery] ProductPagedRequestDto requestDto)
        {
            try
            {
                var listProduct = _productService.GetAllProductPaging(requestDto);
                if (!string.IsNullOrEmpty(requestDto.Search))
                {
                    listProduct = _productService.GetProductByName(requestDto);
                }

                return Ok(new PagedReponse<List<ProductDto>>(listProduct)
                {
                    TotalRecord = listProduct.TotalCount,
                    PageSize = listProduct.PageSize,
                    TotalPages = listProduct.TotalPages,
                    CurrentPage = listProduct.CurrentPage,
                    HasNext = listProduct.HasNext,
                    HasPrevious = listProduct.HasPrevious,
                    PageNumber = requestDto.PageNumber

                });
            }
            catch (Exception e)
            {
                return BadRequest(new ResponseResult<object>(e.Message));
            }
        }

        [HttpGet("updateSEO")]
        public async Task<IActionResult> updateSEO()
        {
            var products = await _productService.GetAllProduct();

            foreach (var item in products)
            {
                var productUpdateDto = _mapper.Map<UpdateProductDto>(item);
                await _productService.UpdateSEOTitle(productUpdateDto);
            }


            return Ok();
        }
    }
}

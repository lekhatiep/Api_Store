using AutoMapper;
using DoAn3API.Authorize.CustomAuthorize;
using DoAn3API.Constants;
using DoAn3API.Dtos.Categories;
using DoAn3API.Services.Categories;
using Domain.Common.Paging;
using Domain.Common.Wrappers;
using Domain.Entities.Catalog;
using Infastructure.Repositories.Catalogs.CategoryRepo;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DoAn3API.Controllers.Catalog
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoriesController(
            ICategoryService categoryService,
            ICategoryRepository categoryRepository,
            IMapper mapper
            )
        {
            _categoryService = categoryService;
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        // GET: api/<CategoriesController>
        [HttpGet]
        public IActionResult GetAll ([FromQuery] PagedCategoryRequestDto categoryRequestDto)
        {
            var listCategories = _categoryService.GetCategoryPaging(categoryRequestDto);

            try
            {
                return Ok(new PagedReponse<PagedList<CategoryDto>>(listCategories) { 
                    
                });
            }
            catch (Exception e)
            {
                return BadRequest(new ResponseResult<object>(e.Message)) ;
            }
            
        }

        // GET api/<CategoriesController>/5
        [CustomAuthorize(NamePermissions.Category.View)]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var category = await _categoryRepository.GetById(id);
                return Ok(new ResponseResult<Category>(category));
            }
            catch (Exception e)
            {
                return BadRequest(new ResponseResult<object>(e.Message));
            }
        }

        // POST api/<CategoriesController>
        [CustomAuthorize(NamePermissions.Category.Create)]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateCategoryDto categoryDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _categoryService.AddNewCategory(categoryDto);

                return Ok();
            }
            catch (Exception e)
            {

                 return BadRequest(new ResponseResult<object>(e.Message));
            }
        }

        // PUT api/<CategoriesController>/5
        [CustomAuthorize(NamePermissions.Category.Edit)]
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] UpdateCategoryDto updateCategoryDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var category = await _categoryRepository.GetById(id);

                category.ModifyTime = DateTime.Now;

                var categoryUpdate = _mapper.Map(updateCategoryDto, category);
                var categoryUpdated = await _categoryRepository.Update(category, updateCategoryDto.Id);

                return Ok(new ResponseResult<Category>(categoryUpdated));
            }
            catch (Exception e)
            {

                return BadRequest(new ResponseResult<object>(e.Message));
            }
        }

        // DELETE api/<CategoriesController>/5
        [CustomAuthorize(NamePermissions.Category.Delete)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _categoryService.DeleteCategory(id);

                return Ok();
            }
            catch (Exception e)
            {

                return BadRequest(new ResponseResult<object>(e.Message));
            }
        }

        [CustomAuthorize(NamePermissions.Category.Edit)]
        [HttpPut("UpdateCategoryOfProduct")]
        public async Task<IActionResult> UpdateCategoryOfProduct(int productID, [FromBody] List<int> CategoryIds)
        {

            try
            {
                await _categoryService.UpdateCategoryOfProduct(productID, CategoryIds);

                return Ok();
            }
            catch (Exception e)
            {

                return BadRequest(new ResponseResult<object>(e.Message));
            }
        }
    }
}

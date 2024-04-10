using AutoMapper;
using Dapper;
using DoAn3API.DataContext;
using DoAn3API.Dtos.Categories;
using DoAn3API.Dtos.Products;
using DoAn3API.Services.Categories;
using Domain.Common.Paging;
using Domain.Entities.Catalog;
using Infastructure.Repositories.Catalogs.CategoryRepo;
using Infastructure.Repositories.Catalogs.ProductCategoryRepo;
using Infastructure.Repositories.ProductRepo;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DoAn3API.Services.Categories
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IProductRepository _productRepository;
        private readonly IProductCategoryRepository _productCategoryRepository;
        private readonly DapperContext _dapperContext;
        private readonly IMapper _mapper;

        public CategoryService(
            IMapper mapper,
            IProductRepository productRepository,
            ICategoryRepository categoryRepository, 
            IProductCategoryRepository productCategoryRepository,
            DapperContext dapperContext
            )
        {
            _mapper = mapper;
            _categoryRepository = categoryRepository;
            _productCategoryRepository = productCategoryRepository;
            _productRepository = productRepository;
            _dapperContext = dapperContext;
        }
 

        public PagedList<CategoryDto> GetCategoryPaging(PagedCategoryRequestDto pagedCategoryRequest)
        {
            //Query
            var queryCategory = _categoryRepository.List();

            //List category

            var listCategory = queryCategory
                .Where(x => x.IsDelete == false);
            var data = PagedList<Category>.ToPagedList(ref listCategory, pagedCategoryRequest.PageNumber, pagedCategoryRequest.PageSize);

            var dataResult = _mapper.Map<PagedList<CategoryDto>>(data);

            return dataResult;
        }

        public IQueryable<Product> GetAllProductByCategoryId(ProductPagedRequestDto pagedRequestDto, int categoryId)
        {
            var queryProduct = from c in _categoryRepository.List()
                     join cp in _productCategoryRepository.List() on c.Id equals cp.CategoryId into cpt
                     from cp in cpt.DefaultIfEmpty()
                     join p in _productRepository.List()
                              .Include(x=>x.ProductImages.Where(x=>x.IsDefault == true && x.IsDelete == false)) on cp.ProductId equals p.Id
                     where c.Id == categoryId &&
                     c.IsDelete == false && p.IsDelete == false 
                     select p                                                   
                    ;

            return queryProduct;
        }

        public async Task AddRandomCategoryToProduct()
        {
            var queryProduct = _productRepository.List().Where(x => x.IsDelete == false);
            var queryCat = _categoryRepository.List().Where(x => x.IsDelete == false);

            var listCateId = await queryCat.Select(x => x.Id).ToListAsync();
            var listProduct = await queryProduct.ToListAsync();

            Random rnd = new Random();
            
            foreach (var item in listProduct)
            {
                var index = rnd.Next(1, listCateId.Count);
                var newProductCat = new ProductCategory
                {
                    CategoryId = listCateId.ElementAt(index),
                    ProductId = item.Id,
                };

                await _productCategoryRepository.Insert(newProductCat);
                
            }
            await _productCategoryRepository.Save();
        }

        public async Task UpdateCategoryOfProduct(int productID, List<int> CatIDs)
        {
            var product = await _productRepository.List().Where(x => x.Id == productID && x.IsDelete == false).FirstOrDefaultAsync();

            if (product == null)
            {
                return;
            }

            if (CatIDs.Count > 0)
            {

                using(var con = _dapperContext.CreateConnection())
                {
                    var sql = @"DELETE ProductCategories WHERE ProductId = @ProductId
                                INSERT INTO ProductCategories (ProductId, CategoryId) 
                                SELECT @ProductId, Id FROM Categories WHERE Id IN @CategoryIds";

                    await con.ExecuteAsync(sql, new { ProductId = productID, CategoryIds = CatIDs });
                }
            }

        }

        public async Task AddNewCategory(CreateCategoryDto createCategoryDto)
        {
            try
            {
                var newCat = _mapper.Map<Category>(createCategoryDto);
                await _categoryRepository.Insert(newCat);
                await _categoryRepository.Save();
            }
            catch (Exception )
            {

                throw;
            }
 
        }

        public async Task DeleteCategory(int catID)
        {
            try
            {
                using (var connection = _dapperContext.CreateConnection())
                {
                    await connection.ExecuteAsync("UPDATE Categories SET IsDelete=1 WHERE id = @catID", new { catID = catID });
                    await connection.ExecuteAsync("DELETE FROM ProductCategories WHERE CategoryId = @catID", new { catID = catID });
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<int> IsCatAssignProduct(int catId)
        {
            var result = 0;
            try
            {
                using (var connection = _dapperContext.CreateConnection())
                {
                    var sql = "SELECT COUNT(1) FROM ProductCategories WHERE CategoryId = @catID";
                    var data = await connection.QueryAsync<int>(sql, new { catID = catId });


                    return data.FirstOrDefault() > 0 ? 1 : 0;
                    
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}

using DoAn3API.Dtos.Categories;
using DoAn3API.Dtos.Products;
using Domain.Common.Paging;
using Domain.Entities.Catalog;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DoAn3API.Services.Categories
{
    public interface ICategoryService
    {
        PagedList<CategoryDto> GetCategoryPaging(PagedCategoryRequestDto pagedCategoryRequest);

        IQueryable<Product> GetAllProductByCategoryId(ProductPagedRequestDto pagedRequestDto, int categoryId);

        Task AddRandomCategoryToProduct();
        Task AddNewCategory(CreateCategoryDto createCategoryDto);
        Task DeleteCategory(int catID);
        Task UpdateCategoryOfProduct(int productID, List<int> CatIDs);
        Task<int> IsCatAssignProduct(int catId);
    }
}

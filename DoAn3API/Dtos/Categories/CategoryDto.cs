using Domain.Base;

namespace DoAn3API.Dtos.Categories
{
    public class CategoryDto : BaseCategoryDto, IEntity<int>
    {
        public int Id { get; set ; }
    }
}

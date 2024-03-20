using AutoMapper;
using Domain.Entities.Catalog;

namespace DoAn3API.Dtos.Categories
{
    [AutoMap(typeof(Category))]
    public class BaseCategoryDto
    {
        public string Name { get; set; }

    }
}

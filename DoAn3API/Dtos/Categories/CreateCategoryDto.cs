using Domain.Interfaces.Audit;
using System;

namespace DoAn3API.Dtos.Categories
{
    public class CreateCategoryDto : BaseCategoryDto
    {
        public CreateCategoryDto()
        {
            CreateTime = DateTime.Now;
        }
        public DateTime CreateTime { get ; set ; }

    }
}

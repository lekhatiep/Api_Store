using AutoMapper;
using Domain.Entities.Catalog;
using System;

namespace DoAn3API.Dtos.Categories
{
    [AutoMap(typeof(Category))]
    public class BaseCategoryDto
    {
        public string Name { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime ModifyTime { get; set; }

        public bool IsDelete { get; set; }

    }
}

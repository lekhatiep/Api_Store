using AutoMapper;
using Domain.Entities.Catalog;

namespace DoAn3API.Dtos.ProductImages
{
    [AutoMap(typeof(ProductImage))]
    public class ProductImageDto 
    {
        public long Id { get; set; }

        public string ImagePath { get; set; }

        public string Caption { get; set; }

        public bool IsDefault { get; set; }

        public int SortOrder { get; set; }
    }
}

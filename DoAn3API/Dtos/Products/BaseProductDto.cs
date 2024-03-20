using AutoMapper;
using DoAn3API.Dtos.ProductImages;
using Domain.Entities.Catalog;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace DoAn3API.Dtos.Products
{
    [AutoMap(typeof(Product))]
    public class BaseProductDto
    {
        private IList<ProductImageDto> _productImages;
        public string Code { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public int ViewCount { get; set; }

        public bool IsFavourite { get; set; }

        public bool IsFeatured { get; set; }

        public IFormFile ThumbnailImage { get; set; }

        public ProductCategory  ProductCategory { get; set; }

        public IList<ProductImageDto> ProductImages
        {
            get => _productImages ??= new List<ProductImageDto>();
            set => _productImages = value;
        }
    }
}

using Domain.Base;
using System;
using System.Collections.Generic;

namespace Domain.Entities.Catalog
{
    public class Category : Entity<int>
    {
        private ICollection<ProductCategory> _productCategories;

        public Category()
        {
            CreateTime = DateTime.Now;
        }

        public string Name { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime ModifyTime { get; set; }

        public bool IsDelete { get; set; }

        public ICollection<ProductCategory> ProductCategories
        {
            get => _productCategories ??= new List<ProductCategory>();
            set => _productCategories = value;
        }
    }
}

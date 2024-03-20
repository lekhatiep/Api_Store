using Domain.Base;
using Domain.Interfaces.Audit;
using System;
using System.Collections.Generic;

namespace Domain.Entities.Catalog
{
    public class Product : Entity<int>, IAudit
    {
        private ICollection<ProductCategory> _productCategories;
        private ICollection<ProductImage> _productImages;
        //private ICollection<Promotion> _promotions;
        //private ICollection<CartItem> _cartItems;
        //private ICollection<OrderItem> _orderItems;

        public string Code { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public int ViewCount { get; set; }

        public bool IsFavourite { get; set; }

        public bool IsFeatured { get; set; }

        public DateTime CreateTime { get ; set ; }

        public DateTime ModifyTime { get; set ; }

        public bool IsDelete { get; set; }

        public string SeoTitle { get; set; }

        public string SeoDescription { get; set; }

        public ICollection<ProductCategory> ProductCategories
        {
            get => _productCategories ??= new List<ProductCategory>();
            set => _productCategories = value;
        }

        public ICollection<ProductImage> ProductImages
        {
            get => _productImages ??= new List<ProductImage>();
            set => _productImages = value;
        }

        //public ICollection<Promotion> Promotions
        //{
        //    get => _promotions ??= new List<Promotion>();
        //    set => _promotions = value;
        //}

        //public ICollection<CartItem> CartItems
        //{
        //    get => _cartItems ??= new List<CartItem>();
        //    set => _cartItems = value;
        //}

        //public ICollection<OrderItem> OrderItems
        //{
        //    get => _orderItems ??= new List<OrderItem>();
        //    set => _orderItems = value;
        //}
    }
}

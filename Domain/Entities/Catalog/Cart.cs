using Domain.Base;
using Domain.Entities.Identity;
using Domain.Interfaces.Audit;
using System;
using System.Collections.Generic;

namespace Domain.Entities.Catalog
{
    public class Cart : Entity<int> , IAudit
    {
        private ICollection<CartItem> _cartItems;

        public int UserId { get; set; }

        public string SessionId { get; set; }

        public string Token { get; set; }

        public string Status { get; set; }

        public DateTime CreateTime { get ; set ; }

        public DateTime ModifyTime { get ; set ; }

        public bool IsDelete { get ; set ; }

        public virtual User User { get; set; }

        public ICollection<CartItem> CartItems
        {
            get => _cartItems ??= new List<CartItem>();
            set => _cartItems = value;
        }
    }
}

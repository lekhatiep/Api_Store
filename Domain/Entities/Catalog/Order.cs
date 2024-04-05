using Domain.Base;
using Domain.Entities.Identity;
using Domain.Interfaces.Audit;
using System;
using System.Collections.Generic;

namespace Domain.Entities.Catalog
{
    public class Order : Entity<int>, IAudit
    {
        private ICollection<OrderItem> _orderItems;

        public int UserId { get; set; }

        public string SessionId { get; set; }

        public string Token { get; set; }

        public int Status { get; set; }

        public double SubTotal { get; set; }

        public double ItemDisCount { get; set; }

        public double Tax { get; set; }

        public double Shipping { get; set; }

        public double Total { get; set; }

        public double Promo { get; set; }

        public double Discount { get; set; }

        public double GrandTotal { get; set; }

        public DateTime CreateTime { get ; set ; }

        public DateTime ModifyTime { get ; set ; }

        public bool IsDelete { get ; set ; }

        public virtual User User { get; set; }

        public ICollection<OrderItem> OrderItems
        {
            get => _orderItems ??= new List<OrderItem>();
            set => _orderItems = value;
        }
    }
}

using Domain.Base;
using System;

namespace Domain.Entities.Catalog
{
    public class CartItem : Entity<Guid>
    {
        public int ProductId { get; set; }

        public int CartId { get; set; }

        public double Price { get; set; }

        public double Discount { get; set; }

        public int Quantity { get; set; }


        public bool Active { get; set; }

        public bool IsChecked { get; set; }

        public bool IsOrder { get; set; }

        public virtual Product Product { get; set; }

        public virtual Cart Cart { get; set; }


    }
}

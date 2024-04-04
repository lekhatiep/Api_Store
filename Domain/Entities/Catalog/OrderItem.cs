using Domain.Base;
using Domain.Interfaces.Audit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Catalog
{
    public class OrderItem : Entity<int>, IAudit
    {
        public int ProductId { get; set; }

        public int OrderId { get; set; }

        public double Price { get; set; }

        public double Discount { get; set; }

        public double Quantity { get; set; }

        public DateTime CreateTime { get ; set ; }

        public DateTime ModifyTime { get ; set ; }

        public bool IsDelete { get ; set ; }

        public virtual Order Order { get; set; }

        public virtual Product Product { get; set; }
    }
}

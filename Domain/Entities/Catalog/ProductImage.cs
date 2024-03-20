using Domain.Base;
using Domain.Interfaces.Audit;
using System;

namespace Domain.Entities.Catalog
{
    public class ProductImage : Entity<long>, IAudit
    {

        public int ProductId { get; set; }

        public string ImagePath { get; set; }

        public string Caption { get; set; }

        public bool IsDefault { get; set; }

        public int SortOrder { get; set; }

        public long FileSize { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime ModifyTime { get; set; }

        public bool IsDelete { get; set; }

        public virtual Product Product { get; set; }
    }
}

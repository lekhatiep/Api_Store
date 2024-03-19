using Domain.Base;
using System.Collections.Generic;

namespace Domain.Entities.Catalog
{
    public class ProductCategory : Entity<int>
    {
        //private ICollection<Promotion> _promotions;
        public int ProductId { get; set; }

        public int CategoryId { get; set; }

        public virtual Product Product { get; set; }

        public virtual Category Category { get; set; }

        //public ICollection<Promotion> Promotions
        //{
        //    get => _promotions ??= new List<Promotion>();
        //    set => _promotions = value;
        //}

    }
}

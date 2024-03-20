using Domain.Base;

namespace DoAn3API.Dtos.Products
{
    public class ProductDto : BaseProductDto, IEntity<int>
    {
        public int Id { get ; set ; }

        public string ImagePath { get; set; }

        public int CategoryId { get; set; }

    }
}

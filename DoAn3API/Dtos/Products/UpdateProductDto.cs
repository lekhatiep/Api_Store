using Domain.Base;

namespace DoAn3API.Dtos.Products
{
    public class UpdateProductDto : BaseProductDto, IEntity<int>
    {
        public int Id { get; set; }
    }
}

using Domain.Common.Paging;

namespace DoAn3API.Dtos.Categories
{
    public class PagedProductCategoryRequestDto : PagedRequestBase
    {
        public int CategoryId { get; set; }
    }
}

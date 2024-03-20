using Domain.Common.Paging;

namespace DoAn3API.Dtos.Products
{
    public class ProductPagedRequestDto : PagedRequestBase
    {
        //Sorting

        public string SortBy { get; set; }

        public string Newest { get; set; }

        public string Featured { get; set; }

        public string BestSale { get; set; }

        //Filter

        public string Search { get; set; }
    }
}

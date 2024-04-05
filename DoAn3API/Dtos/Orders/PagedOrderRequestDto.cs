using Domain.Common.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DoAn3API.Dtos.Orders
{
    public class PagedOrderRequestDto : PagedRequestBase
    {
        public int SearchOrderID { get; set; }
        public string SortValue { get; set; }
        public string SortBy { get; set; }
    }
}

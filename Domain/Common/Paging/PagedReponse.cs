using Domain.Common.Wrappers;

namespace Domain.Common.Paging
{
    public class PagedReponse<T> : ResponseResult<T>
    {
        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public int TotalPages { get; set; }

        public int TotalRecord { get; set; }

        public int CurrentPage { get; set; }

        public bool HasNext { get; set; }

        public bool HasPrevious { get; set; }

        public PagedReponse(T data) : base(data)
        {
           
        }
    }
}

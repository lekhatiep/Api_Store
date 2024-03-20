using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Common.Paging
{
    public class PagedList<T>: List<T>

    {
        public int CurrentPage { get; private set; }

        public int PageSize { get; private set; }

        public int TotalCount { get; set; }

        public bool HasPrevious => CurrentPage > 1;

        public bool HasNext => CurrentPage < TotalPages;


        public int TotalPages { get; set; }
        //public int TotalPages
        //{
        //    get
        //    {
        //        var pageCount = (double)TotalCount / PageSize;
        //        return (int)Math.Ceiling(pageCount);
        //    }
        //}

        public PagedList(List<T> items, int count, int pageNumber, int pageSize)
        {
            TotalPages = (int)Math.Ceiling((double)count / pageSize);
            TotalCount = count;
            CurrentPage = pageNumber;
            PageSize = pageSize;
            AddRange(items);
        }


        // public List<T> Items { get; set; }


        public IQueryable<T> Data { get; set; }

        public static PagedList<T> ToPagedList(ref IQueryable<T> source, int pageNumber, int pageSize)
        {
            var count = source.Count();
            var items = source
                        .Skip((pageNumber - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();

            return new PagedList<T>(items, count, pageNumber, pageSize);
        }


    }
}

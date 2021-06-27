using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace API.Helpers
{
    // TODO: try to convert to record which implements IEnumerable
    public class PagedList<T> : List<T>
    {
        public int PageNumber { get; }

        public int PageSize { get; }

        public int TotalCount { get; }

        public int TotalPages { get; }

        private PagedList(IEnumerable<T> items, int totalCount, int pageNumber, int pageSize)
        {
            AddRange(items);

            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalCount = totalCount;

            TotalPages = (int) Math.Ceiling(totalCount / (double) pageSize);
        }

        public static async Task<PagedList<T>> CreateAsync(IQueryable<T> allItems, int pageNumber, int pageSize)
        {
            var totalCount = await allItems.CountAsync();
            var items = await allItems
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedList<T>(items, totalCount, pageNumber, pageSize);
        }
    }
}
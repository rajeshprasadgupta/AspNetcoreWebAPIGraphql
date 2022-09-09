using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace API.Helpers
{
    //Generic List that supports pagination
    public class PagedList<T>: List<T>
    {
        //range of items in the list
        //pageNumber - current pages
        public PagedList(IEnumerable<T> items, int totalCount, int pageNumber, int pageSize)
        {
            CurrentPage = pageNumber;
            //take the higher value
            TotalPages = (int)Math.Ceiling(totalCount/(double)pageSize);
            PageSize = pageSize;
            TotalCount = totalCount;
            AddRange(items);
        }

        public int CurrentPage { get; set; }

        //Total number of pages
        public int TotalPages { get; set; } 

        //Size of the page
        public int PageSize { get; set; }  

        //Number of items in the query
        public int TotalCount { get; set; }   

        // source - queryable collection to apply pagination to
        public static async Task<PagedList<T>> CreateAsync(IQueryable<T> source, int pageNumber, int pageSize)
        { 
            //number of items in the query
            var count = await source.CountAsync();
            //skip the paged items and pick the rest pageSize number of items 
            var items = await source.Skip((pageNumber -1) * pageSize).Take(pageSize).ToListAsync();
            return new PagedList<T>(items,count, pageNumber, pageSize);
        }

    }
}
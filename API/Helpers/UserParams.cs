using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Helpers
{
    //Class to hold the Page infomration parameters
    public class UserParams
    {
        //Max page size so that if client sends a higher pagesize, restict it to MaxPageSize
        private const int MaxPageSize = 50;

        // First pageNumber will be 1 by default
        public int PageNumber { get; set; } = 1;

        //default page size is 5
        private int _pageSize = 5;

        public int PageSize
        {
            get { return _pageSize; }
            set { _pageSize = value > MaxPageSize ? MaxPageSize : value; }
        }
        
    }
}
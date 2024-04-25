using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleAPI.Helpers
{
    public class QueryObject
    {
        public string? Symbol { get; set; } = null;

        public string? CompanyName { get; set; } = null;

        public String? SortBy { get; set; } = null;

        public bool IsDescending { get; set; } = false;

        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 20;
    }
}
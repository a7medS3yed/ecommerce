using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Shared
{
    public class ProductQueryParam
    {
        public int? brandId { get; set; }
        public int? typeId { get; set; }
        public string? search { get; set; }
        public ProductSortOption? sort { get; set; }

        private const int maxPageSize = 10;
        private const int defaultPageSize = 5;
        private int _pageSize = defaultPageSize;
        private int _pageIndex { get; set; } = 1;
        public int pageCount { get; set; }
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > maxPageSize) ? maxPageSize : value;
        }

        public int PageIndex
        {
            get => _pageIndex;
            set => _pageIndex = (value <= 0) ? 1 : value;
        }


    }
}

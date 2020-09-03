using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LiteCommerce.Admin.Models
{
    public abstract class PaginationResult
    {
        /// <summary>
        /// trang
        /// </summary>
        public int Page { get; set; }
        /// <summary>
        /// Số dòng trên 1 trang
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// Số lượng dòng
        /// </summary>
        public int RowCount { get; set; }
        public string SearchValue { get; set; }
        /// <summary>
        /// Tổng số trang
        /// </summary>
        public int PageCount
        {
            get
            {
                int p = RowCount / PageSize;
                if (RowCount % PageSize > 0)
                    p += 1;
                return p;
            }
        }
    }
}
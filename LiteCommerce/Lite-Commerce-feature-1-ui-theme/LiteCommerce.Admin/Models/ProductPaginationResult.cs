using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LiteCommerce.Admin.Models
{
    public class ProductPaginationResult :PaginationResult
    {
        /// <summary>
        /// 
        /// </summary>
        public string Category { get; set; }
        public string Supplier { get; set; }
        public List<Product> Data { get; set; }
        public string Countries { get; internal set; }
    }
}
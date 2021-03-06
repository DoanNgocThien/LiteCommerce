﻿using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LiteCommerce.Admin.Models
{
    public class OrderPaginationResult : PaginationResult
    {
        public string Country { get; set; }
        public string Category { get; set; }
        public string Employee { get; set; }
        public string Shipper { get; set; }

        public List<Order> Data { get; set; }

    }
}
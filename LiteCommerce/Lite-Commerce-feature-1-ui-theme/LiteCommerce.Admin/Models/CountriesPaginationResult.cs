﻿using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LiteCommerce.Admin.Models
{
    public class CountriesPaginationResult : PaginationResult
    {
        public List<Country> Data { get; set; }
    }
}
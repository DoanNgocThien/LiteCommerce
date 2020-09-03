using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LiteCommerce.Admin.Models.Password
{
    public class Password<T>
    {
        public T Id { get; set; }
        public string password { get; set; }
        public string nPassword { get; set; }
        public string aPassword { get; set; }
    }
}
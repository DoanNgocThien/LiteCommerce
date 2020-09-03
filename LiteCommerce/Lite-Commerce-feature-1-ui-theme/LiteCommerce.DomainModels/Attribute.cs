using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteCommerce.DomainModels
{
    public class Attribute
    {
        public int AttributeID { get; set; }
        public int CategoryID { get; set; }
        public string AttributeName { get; set; }
    }
}

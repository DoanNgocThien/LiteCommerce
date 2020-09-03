using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteCommerce.DomainModels
{
    /// <summary>
    /// Chi tiết đơn hàng
    /// </summary>
    public class OrderDetail : Order
    {
        public double UnitPrice { get; set; }
        public int Quantity { get; set; }
        public double Discount { get; set; }
        virtual public Product Product { get; set; }
    }
}

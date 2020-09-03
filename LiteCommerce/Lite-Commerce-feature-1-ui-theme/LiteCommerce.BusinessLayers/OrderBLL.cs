using LiteCommerce.DataLayers;
using LiteCommerce.DataLayers.SqlServer;
using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteCommerce.BusinessLayers
{
    public static class OrderBLL
    {
        public static void Initialize(string connectionString)
        {
            OrderDB = new OrderDAL(connectionString);
        }

        #region Khai báo các thuộc tính giao tiếp với DAL
        private static IOrderDAL OrderDB { get; set; }
        #endregion

        public static List<Order> ListOfOrders(int page, int pageSize, string country, string category, string employee, string shipper, out int rowCount)
        {
            if (page < 1)
                page = 1;
            if (pageSize <= 0)
                pageSize = 3;
            rowCount = OrderDB.Count(country,category,employee,shipper);
            return OrderDB.List(page, pageSize, country, category, employee, shipper);
        }

        public static int AddOrder(Order order)
        {
            return OrderDB.Add(order);
        }

        public static int DeleteOrder(int[] orderIDs)
        {
            return OrderDB.Delete(orderIDs);
        }

        public static Order Get(int orderID)
        {
            return OrderDB.Get(orderID);
        }

        //public static bool Update(Order order)
        //{
        //    return OrderDB.Update(order);
        //}
    }
}
//    public static class OrderBLL
//    {
//        /// <summary>
//        ///  Hàm phải được gọi để khởi tạo chức năng tác nghiệp
//        /// </summary>
//        /// <param name="connectionString"></param>
//        public static void Initialize(string connectionString)
//        {
//            OrderDB = new OrderDAL(connectionString);
//        }
//        #region Khai báo các thuộc tính giao tiếp với DAL

//        private static IOrderDAL OrderDB { get; set; }

//        #endregion

//        #region Khai báo các chức năng xử lý nghiệp vụ
//        /// <summary>
//        /// Hiển thị danh sách của orders
//        /// </summary>
//        /// <param name="page"></param>
//        /// <param name="pageSize"></param>
//        /// <param name="searchValue"></param>
//        /// <param name="rowCount"></param>
//        /// <returns></returns>
//        public static int Add(Order data)
//        {
//            return OrderDB.Add(data);
//        }
//        public static List<Order> ListOfOrders(int page, int pageSize, string searchValue, out int rowCount)
//        {
//            if (page < 1)
//                page = 1;
//            if (pageSize < 0)
//                pageSize = 20;
//            rowCount = OrderDB.Count(searchValue);
//            return OrderDB.List(page, pageSize, searchValue);
//        }
//        #endregion
//    }
//}

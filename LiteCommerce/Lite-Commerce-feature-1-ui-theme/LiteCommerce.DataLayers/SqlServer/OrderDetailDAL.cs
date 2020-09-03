using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiteCommerce.DomainModels;
using System.Data.SqlClient;
using System.Data;

namespace LiteCommerce.DataLayers.SqlServer
{
    public class OrderDetailDAL : IOrderDetailDAL
    {
        private string connectionString;
        public OrderDetailDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }
        public OrderDetail Get(int OrderID)
        {
            //List<OrderDetail> data = new OrderDetail();
            OrderDetail data = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                // Tạo lệnh thực thi truy vấn dữ liệu
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"SELECT (                                        
                                        Products.ProductID,
                                        Products.ProductName,
                                        OrderDetails.UnitPrice,
                                        Quantity,
                                        Discount
                                            )
                                    from Orders
                                        JOIN OrderDetails
                                            ON Orders.OrderID = OrderDetails.OrderID
                                        JOIN Products
                                            ON OrderDetails.ProductID = Products.ProductID
                                    where ((@OrderID  = N'') or (Orders.OrderID = @OrderID ))";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@OrderID", OrderID);

                //thực thi câu lệnh
                using (SqlDataReader dbReader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (dbReader.Read())
                    {
                       data = new OrderDetail()
                        {
                            UnitPrice = Convert.ToDouble(dbReader["UnitPrice"]),
                            Quantity = Convert.ToInt32(dbReader["Quantity"]),
                            Discount = Convert.ToDouble(dbReader["Discount"]),
                            Product = new Product()
                            {
                                ProductID = Convert.ToInt32(dbReader["ProductID"]),
                                ProductName = Convert.ToString(dbReader["ProductName"]),
                            }
                        };
         
                    }
                }

                connection.Close();
            }
            return data;
        }

        public List<OrderDetail> List(int OrderID)
        {
            List<OrderDetail> data = new List<OrderDetail>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                // Tạo lệnh thực thi truy vấn dữ liệu
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"SELECT                                         
                                        Products.ProductID,
                                        Products.ProductName,
                                        OrderDetails.UnitPrice,
                                        Quantity,
                                        Discount
                                            
                                    from Orders
                                        JOIN OrderDetails
                                            ON Orders.OrderID = OrderDetails.OrderID
                                        JOIN Products
                                            ON OrderDetails.ProductID = Products.ProductID
                                    where ((@OrderID  = N'') or (Orders.OrderID = @OrderID ))";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@OrderID", OrderID);

                //thực thi câu lệnh
                using (SqlDataReader dbReader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (dbReader.Read())
                    {
                        OrderDetail order = new OrderDetail()
                        {
                            UnitPrice = Convert.ToDouble(dbReader["UnitPrice"]),
                            Quantity = Convert.ToInt32(dbReader["Quantity"]),
                            Discount = Convert.ToDouble(dbReader["Discount"]),
                            Product = new Product()
                            {
                                ProductID = Convert.ToInt32(dbReader["ProductID"]),
                                ProductName = Convert.ToString(dbReader["ProductName"]),
                            }
                        };
                        data.Add(order);
                    }
                }

                connection.Close();
            }
            return data;
        }
    }
}
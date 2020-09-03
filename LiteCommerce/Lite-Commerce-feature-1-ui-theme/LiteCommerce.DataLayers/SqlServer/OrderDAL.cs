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
    public class OrderDAL : IOrderDAL
    {
        private string connectionString;
        public OrderDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }
        public int Add(Order order)
        {
            //TODO: implement add order
            int orderID = 0;
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"INSERT INTO Orders
                                          (
                                        OrderDate,
                                        RequiredDate,
                                        ShippedDate,
                                        Freight,
                                        ShipAddress,
                                        ShipCity,
                                        ShipCountry,
                                        ShipperID,
                                        CustomerID,
                                        EmployeeID
                        
                                          )
                                          VALUES (
                                                 @OrderDate,
                                                @RequiredDate,
                                                @ShippedDate,
                                                @Freight,
                                                @ShipAddress,
                                                @ShipCity,
                                                @ShipCountry,
                                                @ShipperID,
                                                @CustomerID,
                                                @EmployeeID
                                                );
                                          SELECT @@IDENTITY";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@OrderDate", order.OrderDate);
                cmd.Parameters.AddWithValue("@RequiredDate", order.RequiredDate);
                cmd.Parameters.AddWithValue("@ShippedDate", order.ShippedDate);
                cmd.Parameters.AddWithValue("@Freight", order.Freight);
                cmd.Parameters.AddWithValue("@ShipAddress", order.ShipAddress);
                cmd.Parameters.AddWithValue("@ShipCity", order.ShipCity);
                cmd.Parameters.AddWithValue("@ShipCountry", order.ShipCountry);
                cmd.Parameters.AddWithValue("@ShipperID", order.Shipper.ShipperID);
                cmd.Parameters.AddWithValue("@CustomerID", order.Customer.CustomerID);
                cmd.Parameters.AddWithValue("@EmployeeID", order.Employee.EmployeeID);

                orderID = Convert.ToInt32(cmd.ExecuteNonQuery());
                connection.Close();
            }
            return orderID;
        }

        public int Count(string countryID, string categoryID, string employeeID, string shipperID)
        {
            int count = 0;
            if (categoryID == "0")
                categoryID = "";
            if (employeeID == "0")
                employeeID = "";
            if (shipperID == "0")
                shipperID = "";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"select COUNT(DISTINCT Orders.OrderID)
                                        from Orders
                                            JOIN Shippers
                                                ON Orders.ShipperID = Shippers.ShipperID
                                            JOIN Customers
                                                ON Orders.CustomerID = Customers.CustomerID
                                            JOIN Employees
                                                ON Orders.EmployeeID = Employees.EmployeeID
                                            JOIN OrderDetails
                                                ON Orders.OrderID = OrderDetails.OrderID
                                            JOIN Products
                                                ON OrderDetails.ProductID = Products.ProductID
                                            JOIN Countries
                                                ON Countries.CountryName = Orders.ShipCountry
                                            JOIN Categories
                                                ON Products.CategoryID = Categories.CategoryID
                                        where ((@countryID = N'') or (Countries.CountryID = @countryID))
                                            AND ((@categoryID = N'') or (Categories.CategoryID = @categoryID))
                                            AND ((@employeeID = N'') or (Employees.EmployeeID = @employeeID))
                                            AND ((@shipperID = N'') or (Shippers.ShipperID = @shipperID))";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@countryID", countryID);
                cmd.Parameters.AddWithValue("@categoryID", categoryID);
                cmd.Parameters.AddWithValue("@employeeID", employeeID);
                cmd.Parameters.AddWithValue("@shipperID", shipperID);

                count = Convert.ToInt32(cmd.ExecuteScalar());

                connection.Close();
            }
            return count;
        }

        public int Delete(int[] OrderIDs)
        {
            int countDeleted = 0;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"DELETE FROM Orders
                                            WHERE(OrderID = @OrderID)";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.Add("@OrderID", SqlDbType.Int);
                foreach (int orderID in OrderIDs)
                {
                    cmd.Parameters["@OrderID"].Value = orderID;
                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                        countDeleted += 1;
                }

                connection.Close();
            }
            return countDeleted;
        }

        public Order Get(int orderID)
        {
            Order data = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"SELECT * FROM Orders WHERE OrderID = @OrderID";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@OrderID", orderID);

                using (SqlDataReader dbReader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    if (dbReader.Read())
                    {
                        data = new Order()
                        {
                           
                            OrderDate = Convert.IsDBNull(dbReader["OrderDate"]) ? new DateTime(2000, 1, 1) : Convert.ToDateTime(dbReader["OrderDate"]),
                            OrderID = Convert.ToInt32(dbReader["OrderID"]),
                            RequiredDate = Convert.IsDBNull(dbReader["RequiredDate"]) ? new DateTime(2000, 1, 1) : Convert.ToDateTime(dbReader["RequiredDate"]),
                            ShipAddress = Convert.ToString(dbReader["ShipAddress"]),
                            ShipCity = Convert.ToString(dbReader["ShipCity"]),
                            ShipCountry = Convert.ToString(dbReader["ShipCountry"]),
                            ShippedDate = Convert.IsDBNull(dbReader["ShippedDate"]) ? new DateTime(2000, 1, 1) : Convert.ToDateTime(dbReader["ShippedDate"]),
                     
                        };
                    }
                }

                connection.Close();
            }
            return data;
        }

        public List<Order> List(int page, int pageSize, string countryID, string categoryID, string employeeID, string shipperID)
        {
            List<Order> data = new List<Order>();
            if (categoryID == "0")
                categoryID = "";
            if (employeeID == "0")
                employeeID = "";
            if (shipperID == "0")
                shipperID = "";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                // Tạo lệnh thực thi truy vấn dữ liệu
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"select * from (
                                        select DISTINCT Orders.OrderID,
                                            DENSE_RANK() over(order by Orders.OrderID DESC) as RowNumber, 
                                            Orders.OrderDate,
                                            Orders.RequiredDate,
                                            Orders.ShippedDate,
                                            Orders.Freight,
                                            Orders.ShipAddress,
                                            Orders.ShipCity,
                                            Orders.ShipCountry,
                                            Shippers.CompanyName as ShipperCompanyName,
                                            Customers.CompanyName as CustomerCompanyName,
                                            Employees.FirstName,
                                            Employees.LastName
                                        from Orders
                                            JOIN Shippers
                                                ON Orders.ShipperID = Shippers.ShipperID
                                            JOIN Customers
                                                ON Orders.CustomerID = Customers.CustomerID
                                            JOIN Employees
                                                ON Orders.EmployeeID = Employees.EmployeeID
                                            JOIN OrderDetails
                                                ON Orders.OrderID = OrderDetails.OrderID
                                            JOIN Products
                                                ON OrderDetails.ProductID = Products.ProductID
                                            JOIN Countries
                                                ON Countries.CountryName = Orders.ShipCountry
                                            JOIN Categories
                                                ON Products.CategoryID = Categories.CategoryID
                                        where ((@countryID = N'') or (Countries.CountryID = @countryID))
                                            AND ((@categoryID = N'') or (Categories.CategoryID = @categoryID))
                                            AND ((@employeeID = N'') or (Employees.EmployeeID = @employeeID))
                                            AND ((@shipperID = N'') or (Shippers.ShipperID = @shipperID))
                                    ) as t
                                    where
                                        (@pageSize = -1)
                                        or (t.RowNumber between @pageSize * (@page -  1) + 1 and @page * @pageSize)
                                    order by t.RowNumber";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@page", page);
                cmd.Parameters.AddWithValue("@pageSize", pageSize);
                cmd.Parameters.AddWithValue("@countryID", countryID);
                cmd.Parameters.AddWithValue("@categoryID", categoryID);
                cmd.Parameters.AddWithValue("@employeeID", employeeID);
                cmd.Parameters.AddWithValue("@shipperID", shipperID);

                using (SqlDataReader dbReader = cmd.ExecuteReader())
                {
                    while (dbReader.Read())
                    {
                        Order order = new Order()
                        {
                            OrderID = Convert.ToInt32(dbReader["OrderID"]),
                            OrderDate = Convert.ToDateTime(dbReader["OrderDate"]),
                            RequiredDate = Convert.IsDBNull(dbReader["RequiredDate"]) ? new DateTime(2000, 1, 1) : Convert.ToDateTime(dbReader["RequiredDate"]),
                            ShippedDate = Convert.IsDBNull(dbReader["ShippedDate"]) ? new DateTime(2000, 1, 1) : Convert.ToDateTime(dbReader["ShippedDate"]),
                            Freight = Convert.ToDouble(dbReader["Freight"]),
                            ShipAddress = Convert.ToString(dbReader["ShipAddress"]),
                            ShipCity = Convert.ToString(dbReader["ShipCity"]),
                            ShipCountry = Convert.ToString(dbReader["ShipCountry"]),

                            Shipper = new Shipper()
                            {
                                CompanyName = Convert.ToString(dbReader["ShipperCompanyName"]),
                            },
                            Customer = new Customer()
                            {
                                CompanyName = Convert.ToString(dbReader["CustomerCompanyName"]),
                            },
                            Employee = new Employee()
                            {
                                FirstName = Convert.ToString(dbReader["FirstName"]),
                                LastName = Convert.ToString(dbReader["LastName"])
                            },
                        };
                            data.Add(order);
                    }
                }

                connection.Close();
            }
            return data;
        }

        //public bool Update(Order data)
        //{
        //    int rowsAffected = 0;
        //    using (SqlConnection connection = new SqlConnection(this.connectionString))
        //    {
        //        connection.Open();

        //        SqlCommand cmd = new SqlCommand();
        //        cmd.CommandText = @"UPDATE Orders
        //                                   SET [CustomerID] = @CustomerID 
        //                                      ,[EmployeeID] = @EmployeeID
        //                                      ,[OrderDate] = @OrderDate
        //                                      ,[RequiredDate] = @RequiredDate
        //                                      ,[ShippedDate] = @ShippedDate
        //                                      ,[ShipperID] = @ShipperID
        //                                      ,[Freight] = @Freight
        //                                      ,[ShipAddress] = @ShipAddress
        //                                      ,[ShipCity] = @ShipCity
        //                                      ,[ShipCountry] = @ShipCountry
        //                                  WHERE OrderID = @OrderID";
        //        cmd.CommandType = CommandType.Text;
        //        cmd.Connection = connection;
        //        cmd.Parameters.AddWithValue("@CustomerID", data.CustomerID);
        //        cmd.Parameters.AddWithValue("@EmployeeID", data.EmployeeID);
        //        cmd.Parameters.AddWithValue("@OrderDate", data.OrderDate);
        //        cmd.Parameters.AddWithValue("@RequiredDate", data.RequiredDate);
        //        cmd.Parameters.AddWithValue("@ShippedDate", data.ShippedDate);
        //        cmd.Parameters.AddWithValue("@ShipperID", data.ShipperID);
        //        cmd.Parameters.AddWithValue("@Freight", data.Freight);
        //        cmd.Parameters.AddWithValue("@ShipAddress", data.ShipAddress);
        //        cmd.Parameters.AddWithValue("@ShipCity", data.ShipCity);
        //        cmd.Parameters.AddWithValue("@ShipCountry", data.ShipCountry);
        //        cmd.Parameters.AddWithValue("@OrderID", data.OrderID);

        //        rowsAffected = Convert.ToInt32(cmd.ExecuteNonQuery());

        //        connection.Close();
        //    }

        //    return rowsAffected > 0;
        //}
    }
}
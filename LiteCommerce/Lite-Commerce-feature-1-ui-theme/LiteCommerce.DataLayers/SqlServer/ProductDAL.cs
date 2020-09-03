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
    public class ProductDAL : IProductDAL
    {
        private string connectionString;
        public ProductDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public int Add(Product data)
        {
            int productId = 0;
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"INSERT INTO Products
                                          (
	                                            ProductName,
	                                            SupplierID,
                                                CategoryID,
                                                QuantityPerUnit,
                                                UnitPrice,
                                                Descriptions,
                                                PhotoPath
                                          )
                                          VALUES
                                          (
	                                            @ProductName,
	                                            @SupplierID,
                                                @CategoryID,
                                                @QuantityPerUnit,
                                                @UnitPrice,
                                                @Descriptions,
                                                @PhotoPath                                          
                                          );
                                          SELECT @@IDENTITY;";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@ProductName", data.ProductName);
                cmd.Parameters.AddWithValue("@SupplierID", data.SupplierID);
                cmd.Parameters.AddWithValue("@CategoryID", data.CategoryID);
                cmd.Parameters.AddWithValue("@QuantityPerUnit", data.QuantityPerUnit);
                cmd.Parameters.AddWithValue("@UnitPrice", data.UnitPrice);
                cmd.Parameters.AddWithValue("@Descriptions", data.Descriptions);
                cmd.Parameters.AddWithValue("@PhotoPath", data.PhotoPath);
                productId = Convert.ToInt32(cmd.ExecuteScalar());

                connection.Close();
            }

            return productId;
        }

        public int Count(string searchValue,string Category,string Supplier)
        {
            int count = 0;
            if (!string.IsNullOrEmpty(searchValue))
                searchValue = "%" + searchValue + "%";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"select count(*) as count from Products 
                    where ((@searchValue =N'') or (ProductName like @searchValue)) and (((@Category =N'') or (CategoryID like @Category)) and ((@Supplier =N'') or (SupplierID like @Supplier)))";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@searchValue", searchValue);
                cmd.Parameters.AddWithValue("@Category", Category);
                cmd.Parameters.AddWithValue("@Supplier", Supplier);
                count = Convert.ToInt32(cmd.ExecuteScalar());
                connection.Close();
            }
            return count;
        }

        public int Delete(int[] productIDs)
        {
            int countDeleted = 0;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"DELETE FROM Products    
                                            WHERE(ProductID = @productID)
                                        AND(ProductID NOT IN(SELECT ProductID FROM OrderDetails))"
                                             ;
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.Add("@productID", SqlDbType.Int);
                foreach (int productId in productIDs)
                {
                    cmd.Parameters["@productID"].Value = productId;
                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                        countDeleted += 1;
                }

                connection.Close();
            }
            return countDeleted;
        }

        public Product Get(int productID)
        {
            Product data = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"SELECT * FROM Products WHERE ProductID = @ProductID";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@ProductID", productID);

                using (SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    if (reader.Read())
                    {
                        data = new Product()
                        {
                            ProductID = Convert.ToInt32(reader["ProductID"]),
                            ProductName = Convert.ToString(reader["ProductName"]),
                            SupplierID = Convert.ToInt32(reader["SupplierID"]),
                            CategoryID = Convert.ToInt32(reader["CategoryID"]),
                            QuantityPerUnit = Convert.ToString(reader["QuantityPerUnit"]),
                            UnitPrice = Convert.ToDecimal(reader["UnitPrice"]),
                            PhotoPath = Convert.ToString(reader["PhotoPath"]),
                            Descriptions = Convert.ToString(reader["Descriptions"])

                        };
                    }
                }

                connection.Close();
            }
            return data;
        }

        public List<Product> List(int page, int pageSize, string searchValue,string Category,string Supplier)
        {
            List<Product> data = new List<Product>();
            if (!string.IsNullOrEmpty(searchValue))
                searchValue = "%" + searchValue + "%";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"SELECT * 
                        from 
                        (
	                        select row_number() over(order by ProductName) as RowNumber,
			                        Products.*
	                    from Products 
	                        where  ((@searchValue =N'') or (ProductName like @searchValue)) and (((@Category =N'') or (CategoryID like @Category)) and ((@Supplier =N'') or (SupplierID like @Supplier)))
                        ) as t
                        where   t.RowNumber between  (@page-1)*@pageSize + 1 and @page*@pageSize
                        order by t.RowNumber";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@page", page);
                cmd.Parameters.AddWithValue("@pageSize", pageSize);
                cmd.Parameters.AddWithValue("@searchValue", searchValue);
                cmd.Parameters.AddWithValue("@Category", Category);
                cmd.Parameters.AddWithValue("@Supplier", Supplier);
                using (SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (reader.Read())
                    {
                        data.Add(new Product()
                        {
                            ProductID = Convert.ToInt32(reader["ProductID"]),
                            ProductName = Convert.ToString(reader["ProductName"]),
                            SupplierID = Convert.ToInt32(reader["SupplierID"]),
                            CategoryID = Convert.ToInt32(reader["CategoryID"]),
                            QuantityPerUnit = Convert.ToString(reader["QuantityPerUnit"]),
                            UnitPrice = Convert.ToDecimal(reader["UnitPrice"]),
                            PhotoPath = Convert.ToString(reader["PhotoPath"]),
                            Descriptions = Convert.ToString(reader["Descriptions"])
                        });
                    }
                }

                connection.Close();
            }

            return data;
        }

        public bool Update(Product data)
        {
            int rowsAffected = 0;
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"UPDATE Products
                                           SET   ProductName = @ProductName
                                                ,SupplierID=@SupplierID
                                                ,CategoryID=@CategoryID
                                                ,QuantityPerUnit=@QuantityPerUnit
                                                ,UnitPrice=@UnitPrice
                                                ,Descriptions=@Descriptions
                                                ,PhotoPath = @PhotoPath                                          
                                          WHERE ProductID = @ProductID";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@ProductName", data.ProductName);
                cmd.Parameters.AddWithValue("@SupplierID", data.SupplierID);
                cmd.Parameters.AddWithValue("@CategoryID", data.CategoryID);
                cmd.Parameters.AddWithValue("@QuantityPerUnit", data.QuantityPerUnit);
                cmd.Parameters.AddWithValue("@UnitPrice", data.UnitPrice);
                cmd.Parameters.AddWithValue("@Descriptions", data.Descriptions);
                cmd.Parameters.AddWithValue("@PhotoPath", data.PhotoPath);
                cmd.Parameters.AddWithValue("@ProductID", data.ProductID);


                rowsAffected = Convert.ToInt32(cmd.ExecuteNonQuery());

                connection.Close();
            }

            return rowsAffected > 0;
        }
    }
}

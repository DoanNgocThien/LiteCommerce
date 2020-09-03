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
    public class ProductAttributeDAL : IProductAttributesDAL
    {
        private string connectionString;
        public ProductAttributeDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }
        public int Add(List<ProductAttributes> productAttributes)
        {
            int countAdd = 0;
            using(SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"Insert into 
                                    ProductAttributes(
                                            ProductID,
                                            AttributeName,
                                            AttributeValues,
                                            DisplayOrder
                                                )
                                    Values(
                                            @ProductID,
                                            @AttributeName,
                                            @AttributeValues,
                                            @DisplayOrder    
                                           )";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.Add("@ProductID", SqlDbType.Int);
                cmd.Parameters.Add("@AttributeName", SqlDbType.NVarChar);
                cmd.Parameters.Add("@AttributeValues", SqlDbType.NVarChar);
                cmd.Parameters.Add("@DisplayOrder", SqlDbType.Int);
                foreach (var productAttribute in productAttributes)
                {
                    cmd.Parameters["@ProductID"].Value = productAttribute.ProductID;
                    cmd.Parameters["@AttributeName"].Value = productAttribute.AttributeName;
                    cmd.Parameters["@AttributeValues"].Value = productAttribute.AttributeValues;
                    cmd.Parameters["@DisplayOrder"].Value = productAttribute.DisplayOrder;
                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                        countAdd += 1;
                }
                connection.Close();
            }
                return countAdd;
        }

        public bool CheckProductInAttibute(int ProductID)
        {
            Product data = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"SELECT * FROM ProductAttributes WHERE ProductID = @ProductID";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@ProductID", ProductID);

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
            if(data != null)
            {
                return true;    
            }
            else
            {
                return false;
            }
            
        }

        public int Delete(int[] ProductIDs)
        {
            int countDelete = 0;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"DELETE FROM ProductAttributes
                                            WHERE(ProductID = @ProductID)";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.Add("@ProductID", SqlDbType.Int);

                foreach(var product in ProductIDs)
                {
                    cmd.Parameters["@ProductID"].Value = product;
                    countDelete = cmd.ExecuteNonQuery();
                }
                
                connection.Close();
            }
            return countDelete;
        }

        public List<ProductAttributes> getAll(int productID)
        {
            List<ProductAttributes> data = new List<ProductAttributes>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"select * from ProductAttributes where ProductID =  @productID ";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@productID", productID);
                using(SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (reader.Read())
                    {
                        data.Add(new ProductAttributes()
                        {
                            AttributeID = Convert.ToInt32(reader["AttributeID"]),
                            ProductID = Convert.ToInt32(reader["ProductID"]),
                            AttributeName = Convert.ToString(reader["AttributeName"]),
                            AttributeValues = Convert.ToString(reader["AttributeValues"]),
                            DisplayOrder = Convert.ToInt32(reader["DisplayOrder"])
                        });
                    }
                }
                connection.Close();
            }
            return data;
        }

        public int Update(List<ProductAttributes> ProductAttributes)
        {
            int countUpdate = 0;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"Update ProductAttributes
                                        Set 
                                            ProductID = @ProductID,
                                            AttributeName = @AttributeName,
                                            AttributeValues = @AttributeValues,
                                            DisplayOrder = @DisplayOrder 
                                        Where AttributeID =@AttributeID   
                                           ";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.Add("@ProductID", SqlDbType.Int);
                cmd.Parameters.Add("@AttributeName", SqlDbType.NVarChar);
                cmd.Parameters.Add("@AttributeValues", SqlDbType.NVarChar);
                cmd.Parameters.Add("@DisplayOrder", SqlDbType.Int);
                cmd.Parameters.Add("@AttributeID", SqlDbType.Int);
                foreach (var productAttribute in ProductAttributes)
                {
                    cmd.Parameters["@AttributeID"].Value = productAttribute.AttributeID;
                    cmd.Parameters["@ProductID"].Value = productAttribute.ProductID;
                    cmd.Parameters["@AttributeName"].Value = productAttribute.AttributeName;
                    cmd.Parameters["@AttributeValues"].Value = productAttribute.AttributeValues;
                    cmd.Parameters["@DisplayOrder"].Value = productAttribute.DisplayOrder;
                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                        countUpdate += 1;
                }
                connection.Close();
            }
            return countUpdate;
        }
    }
}

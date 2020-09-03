using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiteCommerce.DomainModels;
namespace LiteCommerce.DataLayers.SqlServer
{
    public class AttributeDAL : IAttributeDAL
    {
        private string connectionString;
        public AttributeDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }

        List<DomainModels.Attribute> IAttributeDAL.getAll(int CategoryID)
        {
            List<DomainModels.Attribute> data = new List<DomainModels.Attribute>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"select * from Attributes where CategoryID = @categoryID";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@categoryID", CategoryID);
                using (SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (reader.Read())
                    {
                        data.Add(new DomainModels.Attribute()
                        {
                            AttributeID = Convert.ToInt32(reader["AttributeID"]),
                            CategoryID = Convert.ToInt32(reader["CategoryID"]),
                            AttributeName = Convert.ToString(reader["AttributeName"])

                        });
                    }
                }
                connection.Close();

            }
            return data;
        }

        
    }
}

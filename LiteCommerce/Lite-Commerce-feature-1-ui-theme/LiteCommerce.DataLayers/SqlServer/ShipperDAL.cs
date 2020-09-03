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
    public class ShipperDAL : IShipperDAL
    {
        private string connectionString;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="connectionString"></param>
        public ShipperDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public int Add(Shipper data)
        {
            int categoryId = 0;
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"INSERT INTO Shippers
                                          (
	                                          CompanyName,
	                                          Phone
                                          )
                                          VALUES
                                          (
	                                          @Name,
	                                          @Phone
	                                          
                                          );
                                          SELECT @@IDENTITY;";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@Name", data.CompanyName);
                cmd.Parameters.AddWithValue("@Phone", data.Phone);


                categoryId = Convert.ToInt32(cmd.ExecuteScalar());

                connection.Close();
            }

            return categoryId;
        }

        public int Count(string searchValue)
        {
            int count = 0;
            if (!string.IsNullOrEmpty(searchValue))
                searchValue = "%" + searchValue + "%";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"select count(*) as count from Shippers 
                                where @searchValue = N'' or CompanyName like @searchValue";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@searchValue", searchValue);
                count = Convert.ToInt32(cmd.ExecuteScalar());
                connection.Close();
            }
            return count;
        }

        public int Delete(int[] shipperIDs)
        {
            int countDeleted = 0;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"DELETE FROM Shippers
                                            WHERE(ShipperID = @shipperId)
                                        AND(ShipperID NOT IN(SELECT ShipperID FROM Orders))"
                                             ;
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.Add("@shipperId", SqlDbType.Int);
                foreach (int shipperID in shipperIDs)
                { 
                    cmd.Parameters["@shipperId"].Value = shipperID;
                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                        countDeleted += 1;
                }

                connection.Close();
            }
            return countDeleted;
        }

        public Shipper Get(int shipperID)
        {
            Shipper data = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"SELECT * FROM Shippers WHERE ShipperID = @shipperID";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@shipperID", shipperID);

                using (SqlDataReader dbReader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    if (dbReader.Read())
                    {
                        data = new Shipper()
                        {
                            ShipperID = Convert.ToInt32(dbReader["ShipperID"]),
                            CompanyName = Convert.ToString(dbReader["CompanyName"]),
                            Phone = Convert.ToString(dbReader["Phone"])

                        };
                    }
                }

                connection.Close();
            }
            return data;
        }
        public List<Shipper> GetAll()
        {
            List<Shipper> data = new List<Shipper>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "select * from Shippers";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                using (SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (reader.Read())
                    {
                        data.Add(new Shipper()
                        {
                            ShipperID = Convert.ToInt32(reader["ShipperID"]),
                            CompanyName = Convert.ToString(reader["CompanyName"]),
                            Phone = Convert.ToString(reader["Phone"])
                        });
                    }
                }
                connection.Close();
            }
            return data;
        }
        public List<Shipper> List(int page, int pagesize, string searchValue)
        {
            List<Shipper> data = new List<Shipper>();
            if (!string.IsNullOrEmpty(searchValue))
                searchValue = "%" + searchValue + "%";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"SELECT * 
                            from
                            (
	                            select row_number() over(order by CompanyName) as RowNumber,
			                            Shippers.*
	                            from Shippers
	                            where (@searchValue =N'') or (CompanyName like @searchValue)
                            ) as t
                            where t.RowNumber between  (@page-1)*@pageSize + 1 and @page*@pageSize
                            order by t.RowNumber";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@page", page);
                cmd.Parameters.AddWithValue("@pageSize", pagesize);
                cmd.Parameters.AddWithValue("@searchValue", searchValue);
                using (SqlDataReader dbReader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (dbReader.Read())
                    {
                        data.Add(new Shipper()
                        {
                            ShipperID = Convert.ToInt32(dbReader["ShipperID"]),
                            CompanyName = Convert.ToString(dbReader["CompanyName"]),
                            Phone = Convert.ToString(dbReader["Phone"])
                        });
                    }
                }
                connection.Close();
            }

            return data;
        }

        public bool Update(Shipper data)
        {
            int rowsAffected = 0;
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"UPDATE Shippers
                                           SET CompanyName = @CompanyName 
                                              ,Phone = @Phone                                          
                                          WHERE ShipperID = @ShipperID";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@ShipperID", data.ShipperID);
                cmd.Parameters.AddWithValue("@CompanyName", data.CompanyName);
                cmd.Parameters.AddWithValue("@Phone", data.Phone);


                rowsAffected = Convert.ToInt32(cmd.ExecuteNonQuery());

                connection.Close();
            }

            return rowsAffected > 0;
        }
    }
}

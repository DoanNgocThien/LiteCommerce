using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteCommerce.DataLayers.SqlServer
{
    public class CountryDAL : ICountryDAL
    {
        private string connectionString;
        public CountryDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }
        public int Add(Country data)
        {
            int countryId = 0;
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"INSERT INTO Countries
                                          (
	                                          CountryName,
	                                          Abbreviation                                        
                                          )
                                          VALUES
                                          (
	                                          @CountryName,
	                                          @Abbreviation
                                          );
                                          SELECT @@IDENTITY;";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@CountryName", data.CountryName);
                cmd.Parameters.AddWithValue("@Abbreviation", data.Abbreviation);
                countryId = Convert.ToInt32(cmd.ExecuteScalar());
                connection.Close();
            }
            return countryId;
        }
        public int Count(string searchValue)
        {
            int count = 0;
            if (!string.IsNullOrEmpty(searchValue))
                searchValue = "%" + searchValue + "%";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                ///1.thiet lap ket noi
                ///2.tạo câu lệnh
                ///3.thực thi câu lệnh
                ///4.trả về kq
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"select count(*) from Countries where @searchValue =N''or CountryName like @searchValue";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@searchValue", searchValue);
                count = Convert.ToInt32(cmd.ExecuteScalar());
                connection.Close();
            }
            return count;
        }

        public int Delete(string[] countryIDs)
        {
            int countDeleted = 0;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"DELETE FROM Countries
                                            WHERE(CountryID = @countryId)";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.Add("@countryId", SqlDbType.Int);
                foreach (var countryID in countryIDs)
                {
                    cmd.Parameters["@countryId"].Value = countryID;
                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                        countDeleted += 1;
                }
                connection.Close();
            }
            return countDeleted;
        }

       
        public Country Get(string CountryID)
        {
            Country data = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"SELECT * FROM Countries WHERE CountryID = @countryId";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@countryId", CountryID);
                using (SqlDataReader dbReader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    if (dbReader.Read())
                    {
                        data = new Country()
                        {
                            CountryID = Convert.ToInt32(dbReader["CountryID"]),
                            CountryName = Convert.ToString(dbReader["CountryName"]),
                            Abbreviation = Convert.ToString(dbReader["Abbreviation"]),
                        };
                    }
                }
                connection.Close();
            }
            return data;
        }
        public List<Country> GetAll()
        {
            List<Country> data = new List<Country>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"Select * from Countries";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                using (SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (reader.Read())
                    {
                        data.Add(new Country
                        {
                            CountryID = Convert.ToInt32(reader["CountryID"]),
                            CountryName = Convert.ToString(reader["CountryName"]),
                            Abbreviation = Convert.ToString(reader["Abbreviation"])
                        });
                    }
                }
                connection.Close();
            }
            return data;
        }
        public List<Country> List(int page, int pageSize, string searchValue)
        {
            List<Country> data = new List<Country>();
            if (!string.IsNullOrEmpty(searchValue))
                searchValue = "%" + searchValue + "%";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                //tạo lệnh thực thi truy vấn dữ liệu
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"SELECT *
                                    FROM
                                    (
	                                    SELECT ROW_NUMBER() OVER(ORDER BY CountryName) as RowNumber,Countries.*
	                                    from Countries
	                                    where (@searchValue = N'') or (CountryName like @searchValue)
                                    ) as t
                                    where t.RowNumber between (@page -1)*@pageSize +1 and (@page*@pageSize) order by t.RowNumber";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@page", page);
                cmd.Parameters.AddWithValue("@pageSize", pageSize);
                cmd.Parameters.AddWithValue("@searchValue", searchValue);
                using (SqlDataReader dbReader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (dbReader.Read())
                    {
                        data.Add(new Country()
                        {
                            CountryID = Convert.ToInt32(dbReader["CountryID"]),
                            CountryName = Convert.ToString(dbReader["CountryName"]),
                            Abbreviation = Convert.ToString(dbReader["Abbreviation"]),
                        });
                    }
                }
                connection.Close();
            }
            return data;
        }
        public bool Update(Country data)
        {
            int rowsAffected = 0;
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"UPDATE Countries
                                           SET CountryName = @CountryName ,Abbreviation = @Abbreviation
                                          WHERE CountryID = @CountryID";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@CountryID", data.CountryID);
                cmd.Parameters.AddWithValue("@CountryName", data.CountryName);
                cmd.Parameters.AddWithValue("@Abbreviation", data.Abbreviation);
                rowsAffected = Convert.ToInt32(cmd.ExecuteNonQuery());
                connection.Close();
            }
            return rowsAffected > 0;
        }
    }
}
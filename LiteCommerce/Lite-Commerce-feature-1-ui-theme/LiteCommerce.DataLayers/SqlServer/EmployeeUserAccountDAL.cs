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
    public class EmployeeUserAccountDAL : IUserAccountDAL
    {
        private string connectionString;
        public EmployeeUserAccountDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }
        public UserAccount Authorize(string username, string password)
        {
            UserAccount data = null;
            // TODO :Kiểm tra thông tin tài khoản từ bảng Employees
            using(SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"select Email, LastName, FirstName, Title, PhotoPath,Roles  from Employees where Email = @username and Password = @password";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@password", password);
                using(SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    if (reader.Read())
                    {
                        data = new UserAccount()
                        {
                            UserID = Convert.ToString(reader["Email"]),
                            FullName = Convert.ToString(reader["LastName"])+" " + (Convert.ToString(reader["FirstName"])),
                            Photo = Convert.ToString(reader["PhotoPath"]),
                            Title = Convert.ToString(reader["Title"]),
                            Roles = Convert.ToString(reader["Roles"])
                        };
                    }
                }
                connection.Close();
                   
            }
            return data;
        }
    }
}

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
    public class EmloyeeDAL : IEmployeeDAL
    {
        private string connectionString;
        public EmloyeeDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public int Add(Employee data)
        {
            int employeeId = 0;
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"INSERT INTO Employees
                                          (
	                                          LastName,
	                                          FirstName,
	                                          Title,
                                              BirthDate,
                                              HireDate,
                                              Email,
	                                          Address,
	                                          City,
	                                          Country,
	                                          HomePhone,
	                                          Notes,
	                                          PhotoPath,
                                              Password
                                          )
                                          VALUES
                                          (
	                                          @LastName,
	                                          @FirstName,
	                                          @Title,
                                              @BirthDate,
                                              @HireDate,
                                              @Email,
	                                          @Address,
	                                          @City,
	                                          @Country,
	                                          @HomePhone,
	                                          @Notes,
	                                          @PhotoPath,
                                              @Password
                                          );
                                          SELECT @@IDENTITY;";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@LastName", data.LastName);
                cmd.Parameters.AddWithValue("@FirstName", data.FirstName);
                cmd.Parameters.AddWithValue("@Title", data.Title);
                cmd.Parameters.AddWithValue("@BirthDate", data.BirthDate);
                cmd.Parameters.AddWithValue("@HireDate", data.HireDate);
                cmd.Parameters.AddWithValue("@Email", data.Email);
                cmd.Parameters.AddWithValue("@Address", data.Address);
                cmd.Parameters.AddWithValue("@City", data.City);
                cmd.Parameters.AddWithValue("@Country", data.Country);
                cmd.Parameters.AddWithValue("@HomePhone", data.HomePhone);
                cmd.Parameters.AddWithValue("@Notes", data.Notes);
                cmd.Parameters.AddWithValue("@PhotoPath", data.PhotoPath);
                cmd.Parameters.AddWithValue("@Password", data.Password);

                employeeId = Convert.ToInt32(cmd.ExecuteScalar());

                connection.Close();
            }

            return employeeId;
        }

        public bool ChangePassword(int id, string password, string nPassword, string aPassword)
        {
            int row = 0;
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"update Employees 
	                        set Password = @newPassword
	                        where EmployeeID = @id and Password = @password and @newPassword = @APassword";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@password", password);
                cmd.Parameters.AddWithValue("@newPassword", nPassword);
                cmd.Parameters.AddWithValue("@APassword", aPassword);

                row = Convert.ToInt32(cmd.ExecuteNonQuery());
                connection.Close();
            }
            return row > 0;
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
                cmd.CommandText = @"select count(*) as count from Employees 
                        where @searchValue = N'' or FirstName like @searchValue or LastName like @searchValue";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@searchValue", searchValue);
                count = Convert.ToInt32(cmd.ExecuteScalar());
                connection.Close();
            }
            return count;
        }

        public int Delete(int[] employeeIDs)
        {
            int countDeleted = 0;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"DELETE FROM Employees
                                            WHERE(EmployeeID = @EmployeeID)
                                    AND(EmployeeID NOT IN(SELECT EmployeeID FROM Orders))";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.Add("@EmployeeID", SqlDbType.Int);
                foreach (int employeeID in employeeIDs)
                {
                    cmd.Parameters["@EmployeeID"].Value = employeeID;
                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                        countDeleted += 1;
                }

                connection.Close();
            }
            return countDeleted;
        }

        public Employee Get(int employeeID)
        {
            Employee data = new Employee();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"SELECT * FROM Employees WHERE EmployeeID = @employeeID";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@employeeID", employeeID);

                using (SqlDataReader dbReader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    if (dbReader.Read())
                    {
                        data = new Employee()
                        {
                            EmployeeID = Convert.ToInt32(dbReader["EmployeeID"]),
                            FirstName = Convert.ToString(dbReader["FirstName"]),
                            LastName = Convert.ToString(dbReader["LastName"]),
                            Title = Convert.ToString(dbReader["Title"]),
                            Address = Convert.ToString(dbReader["Address"]),
                            City = Convert.ToString(dbReader["City"]),
                            Country = Convert.ToString(dbReader["Country"]),
                            Email = Convert.ToString(dbReader["Email"]),
                            Password = Convert.ToString(dbReader["Password"]),
                            PhotoPath = Convert.ToString(dbReader["PhotoPath"]),
                            HomePhone = Convert.ToString(dbReader["HomePhone"]),
                            Notes = Convert.ToString(dbReader["Notes"]),
                            BirthDate = Convert.ToDateTime(dbReader["BirthDate"]),
                            HireDate = Convert.ToDateTime(dbReader["HireDate"])

                        };
                    }
                }

                connection.Close();
            }
            return data;
        }

        public Employee Get(string email)
        {
            Employee data = new Employee();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"SELECT * FROM Employees WHERE Email = @email";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@email", email);

                using (SqlDataReader dbReader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    if (dbReader.Read())
                    {
                        data = new Employee()
                        {
                            EmployeeID = Convert.ToInt32(dbReader["EmployeeID"]),
                            FirstName = Convert.ToString(dbReader["FirstName"]),
                            LastName = Convert.ToString(dbReader["LastName"]),
                            Title = Convert.ToString(dbReader["Title"]),
                            Address = Convert.ToString(dbReader["Address"]),
                            City = Convert.ToString(dbReader["City"]),
                            Country = Convert.ToString(dbReader["Country"]),
                            Email = Convert.ToString(dbReader["Email"]),
                            Password = Convert.ToString(dbReader["Password"]),
                            PhotoPath = Convert.ToString(dbReader["PhotoPath"]),
                            HomePhone = Convert.ToString(dbReader["HomePhone"]),
                            Notes = Convert.ToString(dbReader["Notes"]),
                            BirthDate = Convert.ToDateTime(dbReader["BirthDate"]),
                            HireDate = Convert.ToDateTime(dbReader["HireDate"])

                        };
                    }
                }

                connection.Close();
            }
            return data;
        }

        public List<Employee> GetAll()
        {
            List<Employee> data = new List<Employee>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"SELECT * FROM Employees ";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                using (SqlDataReader dbReader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (dbReader.Read())
                    {
                        data.Add(new Employee()
                        {
                            EmployeeID = Convert.ToInt32(dbReader["EmployeeID"]),
                            FirstName = Convert.ToString(dbReader["FirstName"]),
                            LastName = Convert.ToString(dbReader["LastName"]),
                            Title = Convert.ToString(dbReader["Title"]),
                            Address = Convert.ToString(dbReader["Address"]),
                            City = Convert.ToString(dbReader["City"]),
                            Country = Convert.ToString(dbReader["Country"]),
                            Email = Convert.ToString(dbReader["Email"]),
                            Password = Convert.ToString(dbReader["Password"]),
                            PhotoPath = Convert.ToString(dbReader["PhotoPath"]),
                            HomePhone = Convert.ToString(dbReader["HomePhone"]),
                            Notes = Convert.ToString(dbReader["Notes"]),
                            BirthDate = Convert.ToDateTime(dbReader["BirthDate"]),
                            HireDate = Convert.ToDateTime(dbReader["HireDate"])

                        });
                    }
                }

                connection.Close();
            }
            return data;
        }

        public List<Employee> List(int page, int pageSize, string searchValue)
        {
            List<Employee> data = new List<Employee>();
            if (!string.IsNullOrEmpty(searchValue))
                searchValue = "%" + searchValue + "%";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"SELECT * 
                                from
                                (
                                    select row_number() over(order by FirstName) as RowNumber,
                                            Employees.*
                                    from Employees

                                    where (@searchValue = N'') or(LastName like @searchValue) or(FirstName like @searchValue)
                                ) as t
                                where t.RowNumber between  (@page - 1) * @pageSize + 1 and @page * @pageSize
                                order by t.RowNumber";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@page", page);
                cmd.Parameters.AddWithValue("@pageSize", pageSize);
                cmd.Parameters.AddWithValue("@searchValue", searchValue);
                using (SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (reader.Read())
                    {
                        data.Add(new Employee()
                        {
                            EmployeeID = Convert.ToInt32(reader["EmployeeID"]),
                            LastName = Convert.ToString(reader["LastName"]),
                            FirstName = Convert.ToString(reader["FirstName"]),
                            Title = Convert.ToString(reader["Title"]),
                            BirthDate = Convert.ToDateTime(reader["BirthDate"]),
                            HireDate = Convert.ToDateTime(reader["HireDate"]),
                            Email = Convert.ToString(reader["Email"]),
                            Address = Convert.ToString(reader["Address"]),
                            City = Convert.ToString(reader["City"]),
                            Country = Convert.ToString(reader["Country"]),
                            HomePhone = Convert.ToString(reader["HomePhone"]),
                            PhotoPath = Convert.ToString(reader["PhotoPath"]),
                            Notes = Convert.ToString(reader["Notes"]),
                            Password = Convert.ToString(reader["Password"])
                        });
                    }
                }

                connection.Close();
            }

            return data;
        }

        public bool Update(Employee data)
        {
            int rowsAffected = 0;
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"UPDATE Employees
                                           SET LastName = @LastName 
                                              ,FirstName = @FirstName
                                              ,Title = @Title
                                              ,BirthDate = @BirthDate
                                              ,HireDate = @HireDate
                                              ,Email = @Email
                                              ,Address = @Address
                                              ,City = @City
                                              ,Country = @Country
                                              ,HomePhone = @HomePhone
                                              ,Notes = @Notes
                                              ,PhotoPath = @PhotoPath
                                              ,Password = @Password
                                          WHERE EmployeeID = @EmployeeID";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@EmployeeID", data.EmployeeID);
                cmd.Parameters.AddWithValue("@LastName", data.LastName);
                cmd.Parameters.AddWithValue("@FirstName", data.FirstName);
                cmd.Parameters.AddWithValue("@Title", data.Title);
                cmd.Parameters.AddWithValue("@BirthDate", data.BirthDate);
                cmd.Parameters.AddWithValue("@HireDate", data.HireDate);
                cmd.Parameters.AddWithValue("@Email", data.Email);
                cmd.Parameters.AddWithValue("@Address", data.Address);
                cmd.Parameters.AddWithValue("@City", data.City);
                cmd.Parameters.AddWithValue("@Country", data.Country);
                cmd.Parameters.AddWithValue("@HomePhone", data.HomePhone);
                cmd.Parameters.AddWithValue("@Notes", data.Notes);
                cmd.Parameters.AddWithValue("@PhotoPath", data.PhotoPath);
                cmd.Parameters.AddWithValue("@Password", data.Password);

                rowsAffected = Convert.ToInt32(cmd.ExecuteNonQuery());

                connection.Close();
            }

            return rowsAffected > 0;
        }
    }
}

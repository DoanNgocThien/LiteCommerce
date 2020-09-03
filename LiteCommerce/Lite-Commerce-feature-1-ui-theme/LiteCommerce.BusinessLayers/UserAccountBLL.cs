using LiteCommerce.DataLayers;
using LiteCommerce.DataLayers.SqlServer;
using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace LiteCommerce.BusinessLayers
{
    /// <summary>
    /// Cung cấp các chức năng tác nghiệp liên quan đến tài khoản
    /// </summary>
    public static class UserAccountBLL
    {
        private static string _connectionString;
        public static void Initialize(string connectionString)
        {
            _connectionString = connectionString;
        }
        public static UserAccount Authorize(string username,string password,UserAccountTypes userType)
        {
            IUserAccountDAL userAccountDB;
            switch (userType)
            {
                case UserAccountTypes.Employee:
                    userAccountDB = new EmployeeUserAccountDAL(_connectionString);
                    break;
                case UserAccountTypes.Customer:
                    userAccountDB = new CustomerUserAccountDAL(_connectionString);
                    break;
                default:
                    return null;
            }
            return userAccountDB.Authorize(username,password);
        }
        /// <summary>
        /// Mã hóa MD5 cho chuỗi text
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
    }
}

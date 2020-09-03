using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiteCommerce.DomainModels;

namespace LiteCommerce.DataLayers.SqlServer
{
    public class CustomerUserAccountDAL : IUserAccountDAL
    {
        private string connectionString;
        public CustomerUserAccountDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }
        public UserAccount Authorize(string username, string password)
        {
            // TODO :Kiểm tra thông tin tài khoản từ bảng Customers
            return new UserAccount()
            {
                UserID = username,
                FullName = "Yua Mikami",
                Photo = "mikami.jpg"
            };
        }
    }
}

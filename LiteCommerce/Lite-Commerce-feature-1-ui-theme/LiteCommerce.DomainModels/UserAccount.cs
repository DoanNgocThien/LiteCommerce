using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteCommerce.DomainModels
{
    /// <summary>
    /// lƯU thông tin liên quan đến tài khoản đăng nhập hệ thống
    /// </summary>
    public class UserAccount
    {
        public int ValueID { get; set; }
        /// <summary>
        /// Tài khoản đăng nhập vào hệ thống
        /// </summary>
        public string UserID { get; set; }
        /// <summary>
        /// Tên gọi đầy đủ của User :First Name and Last Name
        /// </summary>
        public string FullName { get; set; }
        /// <summary>
        /// Tên file ảnh của User
        /// </summary>
        public string Photo { get; set; }
        public string Title { get; set; }
        public string Roles { get; set; }
    }
}

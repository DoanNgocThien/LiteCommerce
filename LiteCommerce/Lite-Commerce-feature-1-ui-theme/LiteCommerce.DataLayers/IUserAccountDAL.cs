using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteCommerce.DataLayers
{
    /// <summary>
    /// Các chức năng cần thiết của các đối tượng sử dụng
    /// </summary>
    public interface IUserAccountDAL
    {
        //int ChangePassword(string passWord, string newPassWord);
        /// <summary>
        /// Kiểm tra UserName và Password có hợp lệ không?
        /// Nếu hợp lệ trả về thông tin của User,
        /// ngược lại trả về giá trị Null
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        UserAccount Authorize(string username, string password);
    }
}

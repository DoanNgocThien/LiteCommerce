using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteCommerce.DataLayers
{
    public interface ICustomerDAL
    {
        /// <summary>
        /// Hiển thị danh sách Customer ,phân trang và tìm kiếm
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pagesize"></param>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        List<Customer> List(int page, int pagesize, string searchValue, string Countries);
        /// <summary>
        /// Đếm số lượng Customer
        /// </summary>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        int Count(string searchValue, string Countries);
        /// <summary>
        /// Lấy Customer
        /// </summary>
        /// <returns></returns>
        Customer Get(string customerID);
        /// <summary>
        /// Thêm Customer.Hàm trả về ID Customer được bổ sung.
        /// Nếu lỗi ,hàm trả về giá trj nhỏ hơn hoặc bằng 0.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        int Add(Customer data);
        /// <summary>
        /// Sữa đỗi 1 Customer.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        bool Update(Customer data);
        /// <summary>
        /// Xóa nhiều Customer.
        /// Trả về số lượng được xóa.
        /// </summary>
        /// <param name="customerIDs"></param>
        /// <returns></returns>
        int Delete(string[] customerIDs);
        List<Customer> GetAll();
    }
}

using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteCommerce.DataLayers
{
    public interface IProductDAL
    {
        /// <summary>
        /// Hiển thị danh sách Product ,phân trang và tìm kiếm
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pagesize"></param>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        List<Product> List(int page, int pagesize, string searchValue,string Category,string Supplier);
        /// <summary>
        /// Đếm số lượng Product
        /// </summary>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        int Count(string searchValue,string Category,string Supplier);
        /// <summary>
        /// Lấy Product
        /// </summary>
        /// <returns></returns>
        Product Get(int productID);
        /// <summary>
        /// Thêm Product.Hàm trả về ID Product được bổ sung.
        /// Nếu lỗi ,hàm trả về giá trj nhỏ hơn hoặc bằng 0.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        int Add(Product data);
        /// <summary>
        /// Sữa đỗi 1 Product.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        bool Update(Product data);
        /// <summary>
        /// Xóa nhiều Product.
        /// Trả về số lượng được xóa.
        /// </summary>
        /// <param name="productIDs"></param>
        /// <returns></returns>
        int Delete(int[] productIDs);
    }
}

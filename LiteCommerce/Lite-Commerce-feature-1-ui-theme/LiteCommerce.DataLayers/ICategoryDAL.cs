using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteCommerce.DataLayers
{
    public interface ICategoryDAL
    {
        /// <summary>
        /// Hiển thị danh sách Category ,phân trang và tìm kiếm
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pagesize"></param>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        List<Category> List(int page, int pagesize, string searchValue);
        /// <summary>
        /// Đếm số lượng Category
        /// </summary>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        int Count(string searchValue);
        /// <summary>
        /// Lấy Category
        /// </summary>
        /// <returns></returns>
        Category Get(int categoryID);
        /// <summary>
        /// Thêm Category.Hàm trả về ID Category được bổ sung.
        /// Nếu lỗi ,hàm trả về giá trj nhỏ hơn hoặc bằng 0.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        int Add(Category data);
        /// <summary>
        /// Sữa đỗi 1 Category.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        bool Update(Category data);
        /// <summary>
        /// Xóa nhiều Category.
        /// Trả về số lượng được xóa.
        /// </summary>
        /// <param name="categoryIDs"></param>
        /// <returns></returns>
        int Delete(int[] categoryIDs);
        /// <summary>
        /// Lay tat ca categories
        /// </summary>
        /// <returns></returns>
        List<Category> GetAll();
    }
}

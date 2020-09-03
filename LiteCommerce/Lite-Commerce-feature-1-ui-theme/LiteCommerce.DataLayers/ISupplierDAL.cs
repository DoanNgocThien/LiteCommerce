using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteCommerce.DataLayers
{
    /// <summary>
    /// 
    /// </summary>
    public interface ISupplierDAL
    {
        /// <summary>
        /// Hiển thị danh sách supplier ,phân trang và tìm kiếm
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pagesize"></param>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        List<Supplier> List(int page, int pagesize, string searchValue);
        /// <summary>
        /// Đếm số lượng supplier
        /// </summary>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        int Count(string searchValue);
        /// <summary>
        /// Lấy supplier
        /// </summary>
        /// <returns></returns>
        Supplier Get(int supplierID);
        /// <summary>
        /// Thêm supplier.Hàm trả về ID supplier được bổ sung.
        /// Nếu lỗi ,hàm trả về giá trj nhỏ hơn hoặc bằng 0.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        int Add(Supplier data);
        /// <summary>
        /// Sữa đỗi 1 supplier.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        bool Update(Supplier data);
        /// <summary>
        /// Xóa nhiều suppliers.
        /// Trả về số lượng được xóa.
        /// </summary>
        /// <param name="supplierIDs"></param>
        /// <returns></returns>
        int Delete(int[] supplierIDs);
        List<Supplier> GetAll();
    }
}

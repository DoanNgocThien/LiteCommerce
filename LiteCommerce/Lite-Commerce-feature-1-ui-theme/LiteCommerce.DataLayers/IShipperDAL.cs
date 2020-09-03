using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteCommerce.DataLayers
{
    public interface IShipperDAL
    {
        /// <summary>
        /// Hiển thị danh sách Shipper ,phân trang và tìm kiếm
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pagesize"></param>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        List<Shipper> List(int page, int pagesize, string searchValue);
        /// <summary>
        /// Đếm số lượng Shipper
        /// </summary>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        int Count(string searchValue);
        /// <summary>
        /// Lấy Shipper
        /// </summary>
        /// <returns></returns>
        Shipper Get(int shipperID);
        /// <summary>
        /// Thêm Shipper.Hàm trả về ID supplier được bổ sung.
        /// Nếu lỗi ,hàm trả về giá trj nhỏ hơn hoặc bằng 0.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        int Add(Shipper data);
        /// <summary>
        /// Sữa đỗi 1 Shipper.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        bool Update(Shipper data);
        /// <summary>
        /// Xóa nhiều Shipper.
        /// Trả về số lượng được xóa.
        /// </summary>
        /// <param name="shipperIDs"></param>
        /// <returns></returns>
        int Delete(int[] shipperIDs);
        List<Shipper> GetAll();
    }
}

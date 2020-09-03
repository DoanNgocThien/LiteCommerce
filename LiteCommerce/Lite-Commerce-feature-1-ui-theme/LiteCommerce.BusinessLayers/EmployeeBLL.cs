using LiteCommerce.DataLayers;
using LiteCommerce.DataLayers.SqlServer;
using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteCommerce.BusinessLayers
{
    public static class EmloyeeBLL
    {
        /// <summary>
        ///  Hàm phải được gọi để khởi tạo chức năng tác nghiệp
        /// </summary>
        /// <param name="connectionString"></param>
        public static void Initialize(string connectionString)
        {
            EmployeeDB = new EmloyeeDAL(connectionString);
        }
        #region Khai báo các thuộc tính giao tiếp với DAL

        private static IEmployeeDAL EmployeeDB { get; set; }

        #endregion

        #region Khai báo các chức năng xử lý nghiệp vụ
        /// <summary>
        /// Hiển thị danh sách của emloyee
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchValue"></param>
        /// <param name="rowCount"></param>
        /// <returns></returns>
        public static List<Employee> ListOfEmployees(int page, int pageSize, string searchValue, out int rowCount)
        {
            if (page < 1)
                page = 1;
            if (pageSize < 0)
                pageSize = 20;
            rowCount = EmployeeDB.Count(searchValue);
            return EmployeeDB.List(page, pageSize, searchValue);
        }

        /// <summary>
        /// Lấy 1 Employee 
        /// </summary>
        /// <param name="EmployeeID"></param>
        /// <returns></returns>
        public static Employee GetEmployee(int EmployeeID)
        {
            return EmployeeDB.Get(EmployeeID);
        }
        /// <summary>
        /// thêm 1 Employee
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static int AddEmployee(Employee data)
        {
            return EmployeeDB.Add(data);
        }
        /// <summary>
        /// xóa 1 danh sách Employee
        /// </summary>
        /// <param name="EmployeeIDs"></param>
        /// <returns></returns>
        public static int DeleteEmployees(int[] EmployeeIDs)
        {
            return EmployeeDB.Delete(EmployeeIDs);
        }
        /// <summary>
        /// update 1 Employee
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool UpdateEmployee(Employee data)
        {
            return EmployeeDB.Update(data);
        }
        #endregion
    }
}

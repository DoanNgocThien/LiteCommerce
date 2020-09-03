using LiteCommerce.DataLayers;
using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace LiteCommerce.BusinessLayers
{
    /// <summary>
    /// Các chức năng xử lý nghiệp vụ liên quan đến quản lý dữ liệu chung của hệ thống
    /// như : nhà cung cấp.khách hàng ,mặt bằng...
    /// </summary>
    public static class CatalogBLL
    {
        /// <summary>
        ///  Hàm phải được gọi để khởi tạo chức năng tác nghiệp
        /// </summary>
        /// <param name="connectionString"></param>
        public static void Initialize(string connectionString)
        {
            SupplierDB = new DataLayers.SqlServer.SupplierDAL(connectionString);
            CustomerDB = new DataLayers.SqlServer.CustomerDAL(connectionString);
            ShipperDB  = new DataLayers.SqlServer.ShipperDAL(connectionString);
            CategoryDB = new DataLayers.SqlServer.CategoryDAL(connectionString);
            ProductDB  = new DataLayers.SqlServer.ProductDAL(connectionString);
            CountriesDB = new DataLayers.SqlServer.CountryDAL(connectionString);
            OrderDetailDB = new DataLayers.SqlServer.OrderDetailDAL(connectionString);
        }
        #region Khai báo các thuộc tính giao tiếp với DAL
        private static IEmployeeDAL EmployeeDB { get; set; }
        private static ISupplierDAL SupplierDB { get; set; }
        private static ICustomerDAL CustomerDB { get; set; }
        private static IShipperDAL ShipperDB { get; set; }
        private static ICategoryDAL CategoryDB { get; set; }
        private static IProductDAL ProductDB { get; set; }
        private static ICountryDAL CountriesDB { get; set; }
        private static IOrderDetailDAL OrderDetailDB { get; set; }
        #endregion

        #region Khai báo các chức năng xử lý nghiệp vụ
        /// <summary>
        /// Hiển thị danh sách của suppliers
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchValue"></param>
        /// <param name="rowCount"></param>
        /// <returns></returns>
        /// 
        public static List<Supplier> ListOfSuppliers(int page,int pageSize,string searchValue,out int rowCount)
        {
            if (page < 1)
                page = 1;
            if (pageSize < 0)
                pageSize = 20;
            rowCount = SupplierDB.Count(searchValue);
            return SupplierDB.List(page, pageSize, searchValue);
        }
        /// <summary>
        ///  Hiển thị danh sách của Customers
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchValue"></param>
        /// <param name="rowCount"></param>
        /// <returns></returns>
        //khai báo các chức năng xử lý nghiệp vụ
        public static List<Employee> ListOfEmployees(int page, int pageSize, string searchValue, out int rowCount)
        {
            if (page < 1)
                page = 1;
            if (pageSize < 0)
                pageSize = 20;
            rowCount = EmployeeDB.Count(searchValue);
            return EmployeeDB.List(page, pageSize, searchValue);
        }

        public static Employee GetEmployee(int employeeID)
        {
            return EmployeeDB.Get(employeeID);
        }

        public static int AddEmployee(Employee data)
        {
            return EmployeeDB.Add(data);
        }

        public static int DeleteEmployees(int[] employeeIDs)
        {
            return EmployeeDB.Delete(employeeIDs);
        }

        public static bool UpdateEmployee(Employee data)
        {
            return EmployeeDB.Update(data);
        }

        public static List<Customer> ListOfCustomers(int page, int pageSize, string searchValue,string Countries, out int rowCount)
        {
            if (page < 1)
                page = 1;
            if (pageSize < 0)
                pageSize = 20;
            if (Countries.Equals("0"))
            {
                Countries = "";
            }
            rowCount = CustomerDB.Count(searchValue,Countries);
            return CustomerDB.List(page, pageSize, searchValue,Countries);
        }

        public static List<Shipper> ListOfShippers(int page, int pageSize, string searchValue, out int rowCount)
        {
            if (page < 1)
                page = 1;
            if (pageSize < 0)
                pageSize = 20;
            rowCount = ShipperDB.Count(searchValue);
            return ShipperDB.List(page, pageSize, searchValue);
        }
        public static List<Category> ListOfCategories(int page, int pageSize, string searchValue, out int rowCount)
        {
            if (page < 1)
                page = 1;
            if (pageSize < 0)
                pageSize = 20;
            rowCount = CategoryDB.Count(searchValue);
            return CategoryDB.List(page, pageSize, searchValue);
        }
        public static List<Category> GetAllCategories()
        {
            return CategoryDB.GetAll();
        }
        public static List<Supplier> GetAllSuppliers()
        {
            return SupplierDB.GetAll();
        }
        public static List<Product> ListOfProducts(int page, int pageSize, string searchValue,string Category,string Supplier, out int rowCount)
        {
            if (page < 1)
                page = 1;
            if (pageSize < 0)
                pageSize = 20;
            if (Category.Equals("0"))
            {
                Category = "";
            }
            if (Supplier.Equals("0"))
                Supplier = "";
            rowCount = ProductDB.Count(searchValue,Category,Supplier);
            return ProductDB.List(page, pageSize, searchValue,Category,Supplier);
        }
        /// <summary>
        /// Lấy 1 supplier 
        /// </summary>
        /// <param name="supplierID"></param>
        /// <returns></returns>
        public static Supplier GetSupplier(int supplierID)
        {
            return SupplierDB.Get(supplierID);
        }
        /// <summary>
        /// thêm 1 supplier
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static int AddSupplier(Supplier data)
        {
            return SupplierDB.Add(data);
        }
        /// <summary>
        /// xóa 1 danh sách suppliers
        /// </summary>
        /// <param name="supplierIDs"></param>
        /// <returns></returns>
        public static int DeleteSuppliers(int[] supplierIDs)
        {
            return SupplierDB.Delete(supplierIDs);
        }
        /// <summary>
        /// update 1 supplier
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool UpdateSupplier(Supplier data)
        {
            return SupplierDB.Update(data);
        }
        /// <summary>
        /// Lấy 1 Category 
        /// </summary>
        /// <param name="categoryID"></param>
        /// <returns></returns>
        public static Category GetCategory(int categoryID)
        {
            return CategoryDB.Get(categoryID);
        }
        /// <summary>
        /// thêm 1 Category
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static int AddCategory(Category data)
        {
            return CategoryDB.Add(data);
        }
        /// <summary>
        /// xóa 1 danh sách Category
        /// </summary>
        /// <param name="CatrgoriesID"></param>
        /// <returns></returns>
        public static int DeleteCategories(int[] CatrgoriesID)
        {
            return CategoryDB.Delete(CatrgoriesID);
        }
        /// <summary>
        /// update 1 category
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool UpdateCategory(Category data)
        {
            return CategoryDB.Update(data);
        }

        /// <summary>
        /// Lấy 1 Shipper 
        /// </summary>
        /// <param name="shipperID"></param>
        /// <returns></returns>
        public static Shipper GetShipper(int shipperID)
        {
            return ShipperDB.Get(shipperID);
        }
        public static List<Shipper> GetAllShipper()
        {
            return ShipperDB.GetAll();
        }
        /// <summary>
        /// thêm 1 Shipper
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static int AddShipper(Shipper data)
        {
            return ShipperDB.Add(data);
        }
        /// <summary>
        /// xóa 1 danh sách Shipper
        /// </summary>
        /// <param name="ShipperIDs"></param>
        /// <returns></returns>
        public static int DeleteShippers(int[] ShipperIDs)
        {
            return ShipperDB.Delete(ShipperIDs);
        }
        /// <summary>
        /// update 1 Shipper
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool UpdateShipper(Shipper data)
        {
            return ShipperDB.Update(data);
        }

        /// <summary>
        /// Lấy 1 Customer 
        /// </summary>
        /// <param name="customerID"></param>
        /// <returns></returns>
        public static Customer GetCustomer(string customerID)
        {
            return CustomerDB.Get(customerID);
        }
        public static List<Customer> GetAllCustomer()
        {
            return CustomerDB.GetAll();
        }
        /// <summary>
        /// thêm 1 Customer
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static int AddCustomerr(Customer data)
        {
            return CustomerDB.Add(data);
        }
        /// <summary>
        /// xóa 1 danh sách Customer
        /// </summary>
        /// <param name="CustomerIDs"></param>
        /// <returns></returns>
        public static int DeleteCustomers(string[] CustomerIDs)
        {
            return CustomerDB.Delete(CustomerIDs);
        }
        /// <summary>
        /// update 1 customer
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool UpdateCustomer(Customer data)
        {
            return CustomerDB.Update(data);
        }


        /// <summary>
        /// Lấy 1 Product 
        /// </summary>
        /// <param name="customerID"></param>
        /// <returns></returns>
        public static Product GetProduct(int productID)
        {
            return ProductDB.Get(productID);
        }
        /// <summary>
        /// thêm 1 Product
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static int AddProduct(Product data)
        {
            return ProductDB.Add(data);
        }
        /// <summary>
        /// xóa 1 danh sách Product
        /// </summary>
        /// <param name="ProductIDs"></param>
        /// <returns></returns>
        public static int DeleteProducts(int[] ProductIDs)
        {
            return ProductDB.Delete(ProductIDs);
        }
        /// <summary>
        /// update 1 Product
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool UpdateProduct(Product data)
        {
            return ProductDB.Update(data);
        }
        public static List<Country> GetCountries(int page, int pageSize, string searchValue, out int rowCount)
        {
            rowCount = CountriesDB.Count(searchValue);
            return CountriesDB.List(page, pageSize, searchValue);
        }

        public static Country GetCountry(string countryID)
        {
            return CountriesDB.Get(countryID);
        }

        public static int AddCountry(Country data)
        {
            return CountriesDB.Add(data);
        }

        public static bool UpdateCountry(Country country)
        {
            return CountriesDB.Update(country);
        }

        public static int DeleteCountry(string[] countryIDs)
        {
            return CountriesDB.Delete(countryIDs);
        }
        #endregion
        public static List<OrderDetail> ListOfOrderDetail(int OrderID)
        {
            return OrderDetailDB.List(OrderID);
        }
    }

}

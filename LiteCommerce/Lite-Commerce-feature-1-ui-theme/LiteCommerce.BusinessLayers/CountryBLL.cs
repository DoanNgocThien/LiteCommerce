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
    public static class CountryBLL
    {
        private static ICountryDAL CountryDB;

        public static void Initialize(string connectionString)
        {
            CountryDB = new CountryDAL(connectionString);
            CountriesDB = new DataLayers.SqlServer.CountryDAL(connectionString);
        }
        private static ICountryDAL CountriesDB { get; set; }
        public static List<Country> ListOfCountries(int page, int pageSize, string searchValue, out int rowCount)
        {
            if (page < 1)
                page = 1;
            if (pageSize < 0)
                pageSize = 20;
            rowCount =CountriesDB.Count(searchValue);
            return CountriesDB.List(page, pageSize, searchValue);
        }

        /// <summary>
        /// Lấy 1 Employee 
        /// </summary>
        /// <param name="EmployeeID"></param>
        /// <returns></returns>
        public static Country GetCountries(string CountryID)
        {
            return CountriesDB.Get(CountryID);
        }
        /// <summary>
        /// thêm 1 Employee
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static int AddCountries(Country data)
        {
            return CountryDB.Add(data);
        }
        /// <summary>
        /// xóa 1 danh sách Employee
        /// </summary>
        /// <param name="EmployeeIDs"></param>
        /// <returns></returns>
        public static int DeleteCountries(string[] countryIDs)
        {
            return CountriesDB.Delete(countryIDs);
        }
        /// <summary>
        /// update 1 Employee
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool UpdateCountries(Country data)
        {
            return CountriesDB.Update(data);
        }
        public static List<Country> GetAll()
        {
            return CountriesDB.GetAll();
        }
    }
}

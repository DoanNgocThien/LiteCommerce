using LiteCommerce.BusinessLayers;
using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LiteCommerce.Admin
{
    public class SelectListHelper
    {
        /// <summary>
        /// Các quốc gia 
        /// </summary>
        /// <returns></returns>
        public static List<SelectListItem> Countries(bool allSelectAll = true)
        {
            List<Country> getList = new List<Country>();
            getList = CountryBLL.GetAll();
            List<SelectListItem> List = new List<SelectListItem>();
            if (allSelectAll)
            {
                List.Add(new SelectListItem() { Value = "", Text = "-- All Countries --" });

            }
            foreach (var country in getList)
            {
                List.Add(new SelectListItem { Value = country.CountryID.ToString(), Text = country.CountryName });
            }
            return List;
        }
        public static List<SelectListItem> Categories(bool allSelectAll = true)
        {
            List<Category> getAll = new List<Category>();
            getAll = CatalogBLL.GetAllCategories();
            List<SelectListItem> List = new List<SelectListItem>();
            if (allSelectAll)
            {
                List.Add(new SelectListItem() { Value = "", Text = "-- All Categories --" });               
                
            }
            foreach (var category in getAll)
            {
                List.Add(new SelectListItem() { Value = category.CategoryID.ToString(), Text = category.CategoryName });
            }
            return List;
            
        }
        public static List<SelectListItem> Suppliers(bool allSelectAll = true)
        {
            List<Supplier> getAll = new List<Supplier>();
            getAll = CatalogBLL.GetAllSuppliers();
            List<SelectListItem> List = new List<SelectListItem>();
            if (allSelectAll)
            {
                List.Add(new SelectListItem() { Value = "", Text = "-- All Suppliers --" });
                
            }
            foreach (var supplier in getAll)
            {
                List.Add(new SelectListItem() { Value = supplier.SupplierID.ToString(), Text = supplier.CompanyName });
            }
            return List;

        }

        public static List<SelectListItem> Shippers(bool allSelectAll = true)
        {
            List<Shipper> getAll = new List<Shipper>();
            getAll = CatalogBLL.GetAllShipper();
            List<SelectListItem> List = new List<SelectListItem>();
            if (allSelectAll)
            {
                List.Add(new SelectListItem() { Value = "", Text = "-- All Shippers --" });

            }
            foreach (var shipper in getAll)
            {
                List.Add(new SelectListItem() { Value = shipper.ShipperID.ToString(), Text = shipper.CompanyName });
            }
            return List;

        }
        public static List<SelectListItem> Customers(bool allSelectAll = true)
        {
            List<Customer> getAll = new List<Customer>();
            getAll = CatalogBLL.GetAllCustomer();
            List<SelectListItem> List = new List<SelectListItem>();
            if (allSelectAll)
            {
                List.Add(new SelectListItem() { Value = "", Text = "-- All Customers --" });

            }
            foreach (var customer in getAll)
            {
                List.Add(new SelectListItem() { Value = customer.CustomerID, Text = customer.CompanyName });
            }
            return List;

        }
        public static List<SelectListItem> Employees(bool allSelectAll = true)
        {
            List<Employee> getAll = new List<Employee>();
            getAll = EmloyeeBLL.GetAllEmployee();
            List<SelectListItem> List = new List<SelectListItem>();
            if (allSelectAll)
            {
                List.Add(new SelectListItem() { Value = "", Text = "-- All Employees --" });

            }
            foreach (var employee in getAll)
            {
                List.Add(new SelectListItem() { Value = employee.EmployeeID.ToString(), Text = employee.FirstName + " " + employee.LastName });
            }
            return List;

        }
    }
}
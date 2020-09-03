using LiteCommerce.Admin.Models;
using LiteCommerce.BusinessLayers;
using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LiteCommerce.Admin.Controllers
{
    [Authorize(Roles = (WebUserRoles.MANAGEDATA))]
    public class CustomerController : Controller
    {
        // GET: Customer
        public ActionResult Index(int page = 1, string searchValue = "",string Countries = "")
        {
            int pageSize = 5;
            int rowCount = 0;
            List<Customer> ListOfCustomer = CatalogBLL.ListOfCustomers(page, pageSize, searchValue,Countries, out rowCount);
            var model = new CustomerPaginationResult()
            {
                Page = page,
                PageSize = pageSize,
                RowCount = rowCount,
                SearchValue = searchValue,
                Countries = Countries,
                Data = ListOfCustomer
            };
            return View(model);
        }
        [HttpGet]
        public ActionResult Input(string id ="")
        {
            if (string.IsNullOrEmpty(id))
            {
                ViewBag.Title = "Create a Customer";
                Customer newCustomer = new Customer()
                {
                    CustomerID = ""
                };
                ViewBag.kt = 0;
                return View(newCustomer);
            }
            else
            {
                ViewBag.Title = "Edit a Customer";
                Customer editCustomer = CatalogBLL.GetCustomer(id);
                if (editCustomer == null)
                    return RedirectToAction("Index");
                ViewBag.kt = 1;
                return View(editCustomer);
            }
        }
        [HttpPost]
        public ActionResult Input(Customer model,int kt)
        {
            try
            {
                //TODO :Kiểm tra tính hợp lệ của dữ liệu nhập vào
                if (string.IsNullOrEmpty(model.CustomerID))
                    ModelState.AddModelError("CustomerID", "CustomerID expected");
                if (string.IsNullOrEmpty(model.CompanyName))
                    ModelState.AddModelError("CompanyName", "CompanyName expected");
                if (string.IsNullOrEmpty(model.ContactName))
                    ModelState.AddModelError("ContactName", "ContactName expected");
                if (string.IsNullOrEmpty(model.ContactTitle))
                    ModelState.AddModelError("ContactTitle", "ContactTitle expected");
                if (string.IsNullOrEmpty(model.Address))
                    model.Address = "";
                if (string.IsNullOrEmpty(model.Country))
                    model.Country = "";
                if (string.IsNullOrEmpty(model.City))
                    model.City = "";
                if (string.IsNullOrEmpty(model.Phone))
                    model.Phone = "";
                if (string.IsNullOrEmpty(model.Fax))
                    model.Fax = "";
                
                //TODO :Lưu dữ liệu nhập vào
                if (Convert.ToInt32(kt) == 0) 
                {
                    ViewBag.kt = 0;
                    CatalogBLL.AddCustomerr(model);
                           
                }
                else if(Convert.ToInt32(kt) == 1)
                {
                    ViewBag.kt = 1;
                    CatalogBLL.UpdateCustomer(model);
                    
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message + ":" + ex.StackTrace);
                return View(model);
            }
        }
        [HttpPost]
        public ActionResult Delete(string[] customerIDs)
        {
            if (customerIDs != null)
            {
                CatalogBLL.DeleteCustomers(customerIDs);

            }
            return RedirectToAction("Index");

        }
    }
}
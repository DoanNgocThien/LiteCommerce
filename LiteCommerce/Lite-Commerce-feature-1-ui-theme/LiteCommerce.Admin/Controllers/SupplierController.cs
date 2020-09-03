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
    public class SupplierController : Controller
    {
        // GET: Supplier
        
        public ActionResult Index(int page = 1,string searchValue = "")
        {
            int pageSize = 3;
            int rowCount = 0;
            List<Supplier> ListOfSuppliers = CatalogBLL.ListOfSuppliers(page, pageSize, searchValue, out rowCount);
            var model = new SupplierPaginationResult()
            {
                Page = page,
                PageSize = pageSize,
                RowCount = rowCount,
                SearchValue = searchValue,
                Data = ListOfSuppliers
            };
            return View(model);
        }
        /// <summary>
        /// in put
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Input(string id="")
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    ViewBag.Title = "Create new Supplier";
                    Supplier newsSupplier = new Supplier()
                    {
                        SupplierID = 0
                    };
                    return View(newsSupplier);
                }
                else
                {
                    ViewBag.Title = "Edit a Supplier";
                    Supplier editSupplier = CatalogBLL.GetSupplier(Convert.ToInt32(id));
                    if (editSupplier == null)
                        return RedirectToAction("Index");
                    return View(editSupplier);
                }              
            }
            catch (Exception ex)
            {
                return Content(ex.Message + ":" + ex.StackTrace);
            }
        }

        [HttpPost]
        public ActionResult Input(Supplier model)
        {
            try
            {
                //TODO :Kiểm tra tính hợp lệ của dữ liệu nhập vào
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
                if (string.IsNullOrEmpty(model.HomePage))
                    model.HomePage = "";
                //TODO :Lưu dữ liệu nhập vào
                if (model.SupplierID == 0)
                {
                    CatalogBLL.AddSupplier(model);
                }
                else
                {
                    CatalogBLL.UpdateSupplier(model);
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message + ":" + ex.StackTrace);
                return View(model);
            }
        }
        /// <summary>
        /// Xóa danh sách suppliers
        /// </summary>
        /// <param name="supplierIDs"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Delete(int[] supplierIDs)
        {
            if(supplierIDs != null)
            {
                CatalogBLL.DeleteSuppliers(supplierIDs);
                
            }
            return RedirectToAction("Index");

        }
    }

}
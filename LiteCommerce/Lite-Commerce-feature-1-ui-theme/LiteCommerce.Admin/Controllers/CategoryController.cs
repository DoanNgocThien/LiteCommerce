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
    [Authorize(Roles =(WebUserRoles.MANAGEDATA))]
    public class CategoryController : Controller
    {
        // GET: Category
        public ActionResult Index(int page = 1, string searchValue = "")
        {
            int pageSize = 3;
            int rowCount = 0;
            List<Category> ListOfCategory = CatalogBLL.ListOfCategories(page, pageSize, searchValue, out rowCount);
            var model = new CategoryPaginationResult()
            {
                Page = page,
                PageSize = pageSize,
                RowCount = rowCount,
                SearchValue = searchValue,
                Data = ListOfCategory 
            };
            return View(model);
        }
        [HttpGet]
        public ActionResult Input(string id = "")
        {
            if (string.IsNullOrEmpty(id))
            {
                ViewBag.Title = "Create new Category";
                Category newCategory = new Category()
                {
                    CategoryID = 0
                };
                return View(newCategory);
            }
            else
            {
                ViewBag.Title = "Edit a Category";
                Category editCategory = CatalogBLL.GetCategory(Convert.ToInt32(id));
                if (editCategory == null)
                    return RedirectToAction("Index");
                return View(editCategory);
            }
           
        }
        [HttpPost]
        public ActionResult Input(Category model)
        {
            try
            {
                //TODO :Kiểm tra tính hợp lệ của dữ liệu nhập vào
                if (string.IsNullOrEmpty(model.CategoryName))
                    ModelState.AddModelError("CategoryName", "CategoryName expected");
                if (string.IsNullOrEmpty(model.Description))
                    model.Description = "";
                //TODO :Lưu dữ liệu nhập vào
                if (model.CategoryID == 0)
                {
                
                    CatalogBLL.AddCategory(model);
                }
                else
                {
                    CatalogBLL.UpdateCategory(model);
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
        /// Xóa danh sách category
        /// </summary>
        /// <param name="CategoriesID"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Delete(int[] CategoriesID)
        {
            if (CategoriesID != null)
            {
                CatalogBLL.DeleteCategories(CategoriesID);

            }
            return RedirectToAction("Index");

        }
    }
}
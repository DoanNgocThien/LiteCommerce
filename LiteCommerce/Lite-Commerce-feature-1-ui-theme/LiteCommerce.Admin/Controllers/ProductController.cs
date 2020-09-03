using LiteCommerce.Admin.Models;
using LiteCommerce.Admin.Models.Attribute;
using LiteCommerce.BusinessLayers;
using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LiteCommerce.Admin.Controllers
{
    [Authorize(Roles = (WebUserRoles.MANAGEDATA))]
    public class ProductController : Controller
    {
        // GET: Product

        public ActionResult Index(int page = 1, string searchValue = "",string Category = "",string Supplier = "")
        {
            int pageSize = 5;
            int rowCount = 0;
            List<Product> ListOfProducts = CatalogBLL.ListOfProducts(page, pageSize, searchValue,Category,Supplier, out rowCount);
            var model = new ProductPaginationResult()
            {
                Page = page,
                PageSize = pageSize,
                RowCount = rowCount,
                SearchValue = searchValue,
                Supplier = Supplier,
                Category = Category,
                Data = ListOfProducts
            };
            return View(model);
        }
        [HttpGet]
        public ActionResult Input(string id = "")
        {
            if (string.IsNullOrEmpty(id))
            {
                ViewBag.Title = "Create new Product";

                Product newProduct = new Product()
                {
                    ProductID = 0
                };
                ViewBag.Type = "Add";
                return View(newProduct);

            }
            else
            {
                ViewBag.Title = "Edit a Product";
                
                Product editProduct = CatalogBLL.GetProduct(Convert.ToInt32(id));
                if (editProduct == null)
                    return RedirectToAction("Index");
                ViewBag.Type = "Update";
                return View(editProduct);
            }
        }
        [HttpPost]
        public ActionResult Input(Product model, HttpPostedFileBase PhotoPath,string PhotoPathDraft,string Type="")
        {
            try
            {
                //TODO :Kiểm tra tính hợp lệ của dữ liệu nhập vào
                if (string.IsNullOrEmpty(model.ProductName))
                    ModelState.AddModelError("ProductName", "ProductName expected");
                if (model.CategoryID == 0)
                    ModelState.AddModelError("CategoryID", "CategoryID expected");
                if (model.SupplierID == 0)
                    ModelState.AddModelError("SupplierID", "SupplierID expected");
                if (string.IsNullOrEmpty(model.QuantityPerUnit))
                    ModelState.AddModelError("QuantityPerUnit", "QuantityPerUnit expected");
                if (string.IsNullOrEmpty(model.PhotoPath))
                    ModelState.AddModelError("PhotoPath", "PhotoPath expected");
                if (model.UnitPrice < 0)
                    ModelState.AddModelError("UnitPrice", "UnitPrice expected");
                if (string.IsNullOrEmpty(model.Descriptions))
                    model.Descriptions = "";
                if (PhotoPath != null)
                {
                    string FileName = $"{DateTime.Now.Ticks}{Path.GetExtension(PhotoPath.FileName)}";
                    string path = Path.Combine(Server.MapPath("~/Images/products"), FileName);
                    PhotoPath.SaveAs(path);
                    model.PhotoPath = FileName;
                }
                //TODO :Lưu dữ liệu nhập vào
                if (model.ProductID == 0)
                {

                    int productID = CatalogBLL.AddProduct(model);
                    return RedirectToAction("Attribute", new { CategoryID= model.CategoryID , ProductID = productID, Type=Type});
                }
                else
                {
                    if (string.IsNullOrEmpty(model.PhotoPath))
                    {
                        model.PhotoPath = PhotoPathDraft;
                    }                 
                    CatalogBLL.UpdateProduct(model);
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
        /// Xóa danh sách Product
        /// </summary>
        /// <param name="ProductIDs"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Delete(int[] ProductIDs)
        {
            if (ProductIDs != null)
            {
                ProductAttributeBLL.Delete(ProductIDs);
                CatalogBLL.DeleteProducts(ProductIDs);
                

            }
            return RedirectToAction("Index");

        }
        [HttpGet]
        public ActionResult Attribute(string CategoryID,string ProductID,string Type="")
        {
            /// Danh sach name attribute
            List<DomainModels.Attribute> getAttribute = new List<DomainModels.Attribute>();
            getAttribute = AttributeBLL.getAll(Convert.ToInt32(CategoryID));

            // Lấy danh sách tất cả các Attribute theo ProductID
            List<ProductAttributes> getAll = ProductAttributeBLL.getAll(Convert.ToInt32(ProductID));
            if (Type.Equals("Update"))
            {           
                if(getAll.Count == 0)
                {
                    Type = "Add";
                }
                else if(getAll != null)
                {
                    ProductAttributeModel editProductAttributeModel = new ProductAttributeModel()
                    {
                            ProductID = Convert.ToInt32(ProductID),
                            ListProductAttributes = getAll
                    };                  
                    ViewBag.Type = "Update";
                    ViewBag.ProductID = ProductID;
                    return View(editProductAttributeModel);
                }
            }

            if (Type.Equals("Add"))
            {
                List<DomainModels.ProductAttributes> getNameAttribute = new List<DomainModels.ProductAttributes>();
                foreach(var attribute in getAttribute)
                {
                    getNameAttribute.Add(new DomainModels.ProductAttributes()
                    {
                        AttributeName = attribute.AttributeName
                    });
                }
                ProductAttributeModel addProductAttribute = new ProductAttributeModel()
                {
                    ProductID = Convert.ToInt32(ProductID),
                    ListProductAttributes = getNameAttribute
                };

                ViewBag.Type = "Add";
                ViewBag.ProductID = ProductID;
                return View(addProductAttribute);
            }       
            return View();
        }
        [HttpPost]
        public ActionResult Attribute(ProductAttributeModel model,string ProductID , string[] AttributeNames, string[] AttributeValues,string[] DisplayOrders, string Type="",string AttributeID = "")
        {
            List<DomainModels.ProductAttributes> data = new List<ProductAttributes>();
           
            int productId = Convert.ToInt32(ProductID);
            int count = AttributeNames.Count();
            for ( int i = 0; i < count; i++)
            {
                data.Add(new DomainModels.ProductAttributes()
                {
                    AttributeID = Convert.ToInt32(AttributeID),
                    ProductID = productId,
                    AttributeName = AttributeNames[i],
                    AttributeValues = AttributeValues[i],
                    DisplayOrder =Convert.ToInt32(DisplayOrders[i])
                });
            }
            //if(data != null)
            //{
            //    return Json(data);
            //}
            // TODO : looi
            if (Type.Equals("Add"))
            {
                ProductAttributeBLL.Add(data);
            }
            else if (Type.Equals("Update"))
            {
                ProductAttributeBLL.Update(data);
            }
            return RedirectToAction("Input/" + model.ProductID);
        }
    }
}
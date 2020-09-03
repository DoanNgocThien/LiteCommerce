using LiteCommerce.Admin.Models;
using LiteCommerce.BusinessLayers;
using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace LiteCommerce.Admin.Controllers
{
    [Authorize(Roles = (WebUserRoles.MANAGEDATA))]
    //[Authorize]
    public class OrderController : Controller
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Index(int page = 1, string country = "", string category = "", string employee = "", string shipper ="")
        {
            int pageSize = 3;
            int rowCount = 0;

            List<Order> listOfOrders = OrderBLL.ListOfOrders(page, pageSize, country, category, employee,shipper, out rowCount);
            var model = new Models.OrderPaginationResult()
            {
                Data = listOfOrders,
                Page = page,
                PageSize = pageSize,
                RowCount = rowCount,
                Country = country,
                Category = category,
                Employee = employee,
                Shipper = shipper
                
            };
            return View(model);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Create(string id = "")
        {
            if (string.IsNullOrEmpty(id))
            {
                ViewBag.Title = " Create new Order";
                Order order = new Order()
                {
                    OrderID = 0
                };
                return View(order);
            }
            else
            {
                ViewBag.Title = "Edit a Order";
                Order order = OrderBLL.Get(Convert.ToInt32(id));
                return View(order);
            }
        }
        [HttpPost]
        public ActionResult Create(Order order)
        {
            try {
                //Kiểm tra hợp lệ dữ liệu
                if (order.OrderDate == null)
                {
                    order.OrderDate = new DateTime(2000, 1, 1);
                }
                if (order.RequiredDate == null)
                {
                    order.OrderDate = new DateTime(2000, 1, 1);
                }
                if (string.IsNullOrEmpty(order.ShipAddress))
                {
                    ModelState.AddModelError("ShipAddress", "Please enter ship address");
                }
                if (string.IsNullOrEmpty(order.ShipCity))
                {
                    ModelState.AddModelError("ShipCity", "Please enter ship city");
                }
                if (string.IsNullOrEmpty(order.ShipCountry))
                {
                    ModelState.AddModelError("ShipCountry", "Please enter ship country");
                }
                if (order.ShippedDate == null)
                {
                    order.ShippedDate = new DateTime(2000, 1, 1);
   
                }
                if (order.OrderID == 0)
                {
                        OrderBLL.AddOrder(order);
                }
                return RedirectToAction("Index");
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex);

                return View(order);
            }
        }
        public ActionResult OrderDetail(string id = "")
        {
            if (!string.IsNullOrEmpty(id))
            {
                ViewBag.Titile = "Order Detail";
                List<OrderDetail> ListDetail = CatalogBLL.ListOfOrderDetail(Convert.ToInt32(id));
                var model = new Models.OrderDetailPaginationResult()
                {
                    Data = ListDetail
                };
                return View(model);
            }
            return RedirectToAction("Index");
        }
        public ActionResult Delete(int[] orderIDs)
        {
            if (orderIDs.Length > 0)
            {
                OrderBLL.DeleteOrder(orderIDs);
            }
            return RedirectToAction("Index");
        }
    }
}
//        public ActionResult Create(EntityOrder model)
//        {
//            try
//            {

//                if (string.IsNullOrEmpty(model.CustomerCompanyName))
//                    ModelState.AddModelError("CustomerCompanyName", "CustomerCompanyName expected");
//                if (string.IsNullOrEmpty(model.ShipperCompanyName))
//                    ModelState.AddModelError("ShipperCompanyName", "ShipperCompanyName expected");
//                if (string.IsNullOrEmpty(model.FullName))
//                    ModelState.AddModelError("FullName", "Employee expected");
//                if (string.IsNullOrEmpty(model.ShipAddress))
//                    ModelState.AddModelError("ShipAddress", "ShipAddress expected");
//                if (string.IsNullOrEmpty(model.ShipCity))
//                    ModelState.AddModelError("ShipCity", "ShipCity expected");
//                if (string.IsNullOrEmpty(model.ShipCountry))
//                    ModelState.AddModelError("ShipCountry", "ShipCountry expected");
//                if (model.Freight <= 0)
//                    ModelState.AddModelError("Freight", "Freight expected");
//                if (model.OrderDate == DateTime.MinValue)
//                    ModelState.AddModelError("OrderDate", "OrderDate expected");
//                if (model.RequiredDate == DateTime.MinValue)
//                    ModelState.AddModelError("RequiredDate", "RequiredDate expected");
//                if (model.ShippedDate == DateTime.MinValue)
//                    ModelState.AddModelError("ShippedDate", "ShippedDate expected");
//                if (Convert.ToDateTime(model.RequiredDate).CompareTo(Convert.ToDateTime(model.OrderDate)) <= 0)
//                    ModelState.AddModelError("Date", "RequiredDate and OrderDate");

//                Order addOrder = new Order();
//                if (model.OrderID == 0)
//                {
//                    ViewBag.Title = "Create Order";
//                    addOrder.OrderID = model.OrderID;
//                    addOrder.EmployeeID = Convert.ToInt32(model.FullName);
//                    addOrder.ShipperID = Convert.ToInt32(model.ShipperCompanyName);
//                    addOrder.CustomerID = Convert.ToString(model.CustomerCompanyName);
//                    addOrder.RequiredDate = Convert.ToDateTime(model.RequiredDate);
//                    addOrder.OrderDate = Convert.ToDateTime(model.OrderDate);
//                    addOrder.ShippedDate = Convert.ToDateTime(model.ShippedDate);
//                    addOrder.ShipCity = Convert.ToString(model.ShipCity);
//                    addOrder.ShipCountry = Convert.ToString(model.ShipCountry);
//                    addOrder.ShipAddress = Convert.ToString(model.ShipAddress);
//                    addOrder.Freight = Convert.ToDecimal(model.Freight);
//                    OrderBLL.Add(addOrder);
//                }
//                return RedirectToAction("Index");
//            }
//            catch (Exception ex)
//            {
//                ModelState.AddModelError("", ex.Message + ":" + ex.StackTrace);
//                return View(model);
//            }
//        }
//    }
//}
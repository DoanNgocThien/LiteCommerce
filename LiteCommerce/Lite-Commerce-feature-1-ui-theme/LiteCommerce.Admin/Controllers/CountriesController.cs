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
    public class CountriesController : Controller
    {
        // GET: Countries
        public ActionResult Index(int page = 1, string searchValue = "")
        {
            int pageSize = 5;
            int rowCount = 0;
            List<Country> ListOfCountries = CountryBLL.ListOfCountries(page, pageSize, searchValue,out rowCount);
            var model = new CountriesPaginationResult()
            {
                Page = page,
                PageSize = pageSize,
                RowCount = rowCount,
                SearchValue = searchValue,
                Data = ListOfCountries
            };
            return View(model);
        }
        [HttpGet]
        public ActionResult Input(string id = "")
        {
            if (string.IsNullOrEmpty(id))
            {
                ViewBag.Title = "Create a Country";
                Country newCountry = new Country()
                {
                    CountryID = 0
                };
                return View(newCountry);
            }
            else
            {
                ViewBag.Title = "Edit a Country";
                Country editCountry = CountryBLL.GetCountries(id);
                if (editCountry == null)
                    return RedirectToAction("Index");
                return View(editCountry);
            }
        }
        [HttpPost]
        public ActionResult Input(Country model)
        {
            try
            {
                //TODO :Kiểm tra tính hợp lệ của dữ liệu nhập vào
                
                if (string.IsNullOrEmpty(model.CountryName))
                    ModelState.AddModelError("CountryName", "CountryName expected");
                if (string.IsNullOrEmpty(model.Abbreviation))
                    ModelState.AddModelError("Abbreviation", "Abbreviation expected");

                //TODO :Lưu dữ liệu nhập vào
                if (model.CountryID == 0)
                {
                   
                    CountryBLL.AddCountries(model);

                }
                else
                {
                  
                    CountryBLL.UpdateCountries(model);

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
        public ActionResult Delete(string[] countryIDs)
        {
            if (countryIDs != null)
            {
                CountryBLL.DeleteCountries(countryIDs);

            }
            return RedirectToAction("Index");

        }
    }
}
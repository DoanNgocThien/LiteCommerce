using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LiteCommerce.Admin.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {

        // GET: Dashboard
        
        public ActionResult Index()
        {
            //return RedirectToAction("~/Account/SignIn");
            return View();
        }
    }
}
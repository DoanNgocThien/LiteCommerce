
using LiteCommerce.Admin.Models.Password;
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
    [Authorize]
    public class AccountController : Controller
    {
        // GET: Account

        public ActionResult Index()
        {

            var userdata = User.GetUserData();
            Employee employeedata = EmloyeeBLL.GetEmployee(userdata.UserID);
            if (employeedata == null)
                return Content(Convert.ToString(userdata.ValueID));
            return View(employeedata);
        }
        public ActionResult SaveProfile(Employee model, HttpPostedFileBase PhotoPath, string PhotoPathDraft)
        {
            try
            {
                //TODO :Kiểm tra tính hợp lệ của dữ liệu nhập vào
                if (string.IsNullOrEmpty(model.FirstName))
                    ModelState.AddModelError("FirstName", "FirstName expected");
                if (string.IsNullOrEmpty(model.LastName))
                    ModelState.AddModelError("LastName", "LastName expected");
                if (string.IsNullOrEmpty(model.Title))
                    ModelState.AddModelError("Title", "Title expected");
                if (string.IsNullOrEmpty(model.Password))
                    ModelState.AddModelError("Password", "Password expected");
                if (model.BirthDate == DateTime.MinValue)
                    ModelState.AddModelError("BirthDate", "BirthDate expected");
                if (model.HireDate == DateTime.MinValue)
                    ModelState.AddModelError("HireDate", "HireDate expected");
                if (model.HireDate.CompareTo(model.BirthDate) > 0)
                    ModelState.AddModelError("Date", " expected");
                if (string.IsNullOrEmpty(model.Email))
                    model.Email = "";
                if (string.IsNullOrEmpty(model.Address))
                    model.Address = "";
                if (string.IsNullOrEmpty(model.Country))
                    model.Country = "";
                if (string.IsNullOrEmpty(model.City))
                    model.City = "";
                if (string.IsNullOrEmpty(model.HomePhone))
                    model.HomePhone = "";
                if (string.IsNullOrEmpty(model.Notes))
                    model.Notes = "";
                if (string.IsNullOrEmpty(model.PhotoPath))
                    model.PhotoPath = "";
                //TODO :upload image
                if (PhotoPath != null)
                {
                    string FileName = $"{DateTime.Now.Ticks}{Path.GetExtension(PhotoPath.FileName)}";
                    string path = Path.Combine(Server.MapPath("~/Images/uploads"), FileName);
                    PhotoPath.SaveAs(path);
                    model.PhotoPath = FileName;
                }
                //TODO :Lưu dữ liệu nhập vào            
                if (string.IsNullOrEmpty(model.PhotoPath))
                {
                    model.PhotoPath = PhotoPathDraft;
                }
                EmloyeeBLL.UpdateEmployee(model);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message + ":" + ex.StackTrace);
                return View(model);
            }
        }
        [HttpGet]
        public ActionResult ChangePassword(string Id = "")
        {
            EmployeePassword employeePassword = new EmployeePassword()
            {
                Id = Convert.ToInt32(Id)
            };
            ViewBag.Title = "ChangePassword";
            return View(employeePassword);
        }
        [HttpPost]
        public ActionResult ChangePassword(EmployeePassword model)
        {

            try
            {
                if (string.IsNullOrEmpty(model.password))
                    ModelState.AddModelError("password", "Old password expected");
                if (string.IsNullOrEmpty(model.nPassword))
                    ModelState.AddModelError("nPassword", "New password expected");
                if (string.IsNullOrEmpty(model.aPassword))
                    ModelState.AddModelError("aPassword", "Password expected");
                model.password = EncodeMD5.EncodeMD(model.password);
                model.nPassword = EncodeMD5.EncodeMD(model.nPassword);
                model.aPassword = EncodeMD5.EncodeMD(model.aPassword);
                EmloyeeBLL.ChangePassword(model.Id, model.password, model.nPassword, model.aPassword);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message + ":" + ex.StackTrace);
                return View(model);
            }
        }
        public ActionResult SignOut()
        {
            //TODO : sign out
            Session.Abandon();
            Session.Clear();
            System.Web.Security.FormsAuthentication.SignOut();
            return RedirectToAction("SignIn");
        }

        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult SignIn(string email = "", string password = "")
        {
            //kiểm tra thông tin tài khoản
            password = EncodeMD5.EncodeMD(password);
            UserAccount user = UserAccountBLL.Authorize(email, password, UserAccountTypes.Employee);
            if (user != null)
            {
                WebUserData userData = new WebUserData()
                {
                    UserID = user.UserID,
                    FullName = user.FullName,
                    GroupName = user.Roles,
                    LoginTime = DateTime.Now,
                    SessionID = Session.SessionID,
                    ClientIP = Request.UserHostAddress,
                    Photo = user.Photo,
                    Title = user.Title,
                    ValueID = user.ValueID
                };
                System.Web.Security.FormsAuthentication.SetAuthCookie(userData.ToCookieString(), false);
                return RedirectToAction("Index", "Dashboard");
            }
            else
            {
                ModelState.AddModelError("LoginError", "Login fail");
                ViewBag.Email = email;
                return View();
            }
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult SignIn()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Dashboard");
            }
            else
            {
                return View();
            }

        }
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }
    }
}
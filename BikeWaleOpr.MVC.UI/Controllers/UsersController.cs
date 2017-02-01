using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BikeWaleOpr.MVC.UI.Models.Users;

namespace BikeWaleOpr.MVC.UI.Controllers
{
    public class UsersController : Controller
    {
        // GET: Users
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        public ActionResult Authenticate(LoginViewModel model, string ReturnUrl)
        {
            bool auth = HttpContext.User.Identity.IsAuthenticated;

            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                Carwale.WebServices.OprAuthentication.OprAuthentication objOA = null;
                Carwale.WebServices.OprAuthentication.UserBasicInfo objBasicInfo = null;

                string loginId = model.Username;
                string password = model.Password;

                objOA = new Carwale.WebServices.OprAuthentication.OprAuthentication();
                objBasicInfo = objOA.AuthenticateUser(loginId, password);

                if (!string.IsNullOrEmpty(objBasicInfo.UserId) && objBasicInfo.UserId != "-1")
                {
                    //create a ticket and add it to the cookie
                    System.Web.Security.FormsAuthenticationTicket ticket;
                    //now add the id and the role to the ticket, concat the id and role, separated by ',' 
                    //ticket = new FormsAuthenticationTicket(1, oprId, DateTime.Now, DateTime.Now.AddHours(9), false, oprId);
                    ticket = new System.Web.Security.FormsAuthenticationTicket(1, objBasicInfo.UserId, DateTime.Now, DateTime.Now.AddHours(9), false, objBasicInfo.UserId + ":" + loginId + ":" + objBasicInfo.Name);


                    //add the ticket into the cookie
                    HttpCookie objCookie;
                    objCookie = new HttpCookie(System.Web.Security.FormsAuthentication.FormsCookieName);
                    objCookie.Value = System.Web.Security.FormsAuthentication.Encrypt(ticket);

                    ControllerContext.HttpContext.Response.Cookies.Add(objCookie);
                }
            }

            if (Url.IsLocalUrl(ReturnUrl) && ReturnUrl.Length > 1 && ReturnUrl.StartsWith("/")
                && !ReturnUrl.StartsWith("//") && !ReturnUrl.StartsWith("/\\"))
            {
                return Redirect(ReturnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult Logout()
        {
            System.Web.Security.FormsAuthentication.SignOut();

            return RedirectToAction("Login");
        }
    }
}
using BikewaleOpr.Entity.Users;
using BikewaleOpr.Interface.Users;
using BikeWaleOpr.Models.Users;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace BikeWaleOpr.MVC.UI.Controllers
{
    /// <summary>
    /// Created By : Ashish G. Kamble
    /// </summary>
    public class UsersController : Controller
    {
        private readonly IUsers _users = null;

        /// <summary>
        /// Constructor to pass the dependencies
        /// </summary>
        /// <param name="users"></param>
        public UsersController(IUsers users)
        {
            _users = users;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Written By : Ashish Kamble
        /// Summary : Action method will redirect user to home page if user is already authenticated 
        /// else user will be redirected to the login page
        /// </summary>
        /// <returns></returns>
        public ActionResult Login()
        {
            try
            {
                if (HttpContext.User.Identity.IsAuthenticated)
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass.LogError(ex, "UsersController/Login");
            }

            return View();
        }

        /// <summary>
        /// Modifier    : Kartik Rathod on 30 march
        /// Desc        : Added google authentication and fetched details from opr api
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <param name="strId"></param>
        /// <returns></returns>
        public ActionResult Authenticate(string returnUrl, string idtoken)
        {
            try
            {
                bool isAuthenticated = true;
                if (!string.IsNullOrEmpty(idtoken))
                {
                    string loginId = string.Empty;
                    
                    loginId = _users.GoogleApiAuthentication(idtoken);

                    if (!string.IsNullOrEmpty(loginId))
                    {                        
                        UserDetailsEntity objUserDetailsEntity = _users.GetUserDetails(loginId);

                        if (objUserDetailsEntity != null && objUserDetailsEntity.UserId > 0)
                        {
                            //create a ticket and add it to the cookie
                            System.Web.Security.FormsAuthenticationTicket ticket;
                            //now add the id and the role to the ticket, concat the id and role, separated by ',' 

                            string strUserData = Convert.ToString(objUserDetailsEntity.UserId) + ":" + loginId + ":" + objUserDetailsEntity.UserName;
                            ticket = new System.Web.Security.FormsAuthenticationTicket(1, Convert.ToString(objUserDetailsEntity.UserId), DateTime.Now, DateTime.Now.AddHours(12), false, strUserData);

                            //add the ticket into the cookie
                            HttpCookie objCookie;
                            objCookie = new HttpCookie(System.Web.Security.FormsAuthentication.FormsCookieName);
                            objCookie.Value = System.Web.Security.FormsAuthentication.Encrypt(ticket);
                            objCookie.Expires = DateTime.Now.AddHours(12);

                            ControllerContext.HttpContext.Response.Cookies.Add(objCookie);
                        }
                        else
                        {
                            isAuthenticated = false;
                        }
                    }
                    else
                    {
                        isAuthenticated = false;
                    }
                }

                if (!isAuthenticated)
                {
                    TempData["isAuthenticated"] = "false";
                    return RedirectToAction("Login");
                }

                if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/") && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                {
                    return Redirect(returnUrl);
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass.LogError(ex, "UsersController/Login");
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
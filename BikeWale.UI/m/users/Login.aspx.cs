using Bikewale.Common;
using Bikewale.Service.Controllers.Customer;
using Bikewale.UI.Entities.Customer;
using System;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace BikWale.Mobile.Users
{
    /// <summary>
    /// Created by: Sangram Nandkhile on 04 Nov 2016
    /// Desc: Login page for mobile
    /// </summary>

    public class Login : System.Web.UI.Page
    {
        protected Button btnLogin, btnSignup;
        protected HtmlGenericControl errEmail;
        protected TextBox txtLoginid, txtPasswd, txtNameSignup, txtEmailSignup, txtMobileSignup, txtRegPasswdSignup;
        protected HiddenField hdnAuthData;
        bool logout = false;

        protected override void OnInit(EventArgs e)
        {
            base.Load += new EventHandler(Page_Load);
            this.btnLogin.Click += new System.EventHandler(this.LoginUser);
            this.btnSignup.Click += new System.EventHandler(this.RegisterCustomer);
        }

        private void Page_Load(object sender, EventArgs e)
        {

        }

        private void LoginUser(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(hdnAuthData.Value))
                {
                    // Desirialize the response for the authentication
                    System.Web.Script.Serialization.JavaScriptSerializer cityjson = new System.Web.Script.Serialization.JavaScriptSerializer();
                    AuthenticatedCustomer objCust = (AuthenticatedCustomer)cityjson.Deserialize(hdnAuthData.Value, typeof(AuthenticatedCustomer));

                    if (objCust != null && objCust.IsAuthorized)
                    {
                        CreateAuthenticationCookie(objCust.AuthenticationTicket, true);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);

                RedirectPath();
            }
        }

        /// <summary>
        /// Written By : Ashish G. Kamble on 8 Sept 2015
        /// Summary : Function to register the customer with bikewale
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RegisterCustomer(object sender, EventArgs e)
        {
            RegisteredCustomer objRegCustomer = null;
            try
            {
                RegisterInputParameters objReg = new RegisterInputParameters();
                objReg.Name = txtNameSignup.Text.Trim();
                objReg.Email = txtEmailSignup.Text.Trim();
                objReg.Password = txtRegPasswdSignup.Text.Trim();
                objReg.Mobile = txtMobileSignup.Text.Trim();
                objReg.ClientIP = CommonOpn.GetClientIP();

                // Register customer                
                string _apiUrl = "/api/Customer/";
                string reEmail = @"^[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,6}$";

                if (Regex.IsMatch(txtEmailSignup.Text.Trim().ToLower(), reEmail))
                {
                    using (Bikewale.Utility.BWHttpClient objClient = new Bikewale.Utility.BWHttpClient())
                    {
                        objRegCustomer = objClient.PostSync<RegisterInputParameters, RegisteredCustomer>(Bikewale.Utility.APIHost.BW, Bikewale.Utility.BWConfiguration.Instance.APIRequestTypeJSON, _apiUrl, objReg);
                    }

                    if (objRegCustomer != null && objRegCustomer.IsNewCustomer)
                    {
                        // Authenticate the customer
                        CreateAuthenticationCookie(objRegCustomer.AuthenticationTicket, true);
                    }
                    else if (objRegCustomer != null && !objRegCustomer.IsNewCustomer)
                    {
                        errEmail.InnerText = "Already Registered. Please Login.";
                    }
                }
                else
                {
                    errEmail.InnerText = "Invalid Email!";
                    errEmail.Attributes.Remove("class");
                    errEmail.Attributes.Add("class", "bw-blackbg-tooltip error");
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);

                RedirectPath();
            }
        }

        /// <summary>
        /// Creates authenticate Coockie
        /// </summary>
        /// <param name="authTicket"></param>
        /// <param name="rememberMe"></param>
        private void CreateAuthenticationCookie(string authTicket, bool rememberMe)
        {
            // Add bikewale login cookie
            HttpCookie objCookie = new HttpCookie("_bikewale");

            objCookie.Value = authTicket;

            // If remember me checked, create persistent cookie
            if (rememberMe)
            {
                objCookie.Expires = DateTime.Now.AddYears(1);
            }

            // Add cookie into response
            HttpContext.Current.Response.Cookies.Add(objCookie);

            RedirectPath();
        }

        /// <summary>
        /// Desc: redirects users to return url
        /// Modified By : Sajal Gupta on 30-11-2016
        /// Desc : Added functionality to redirect to return uel path.
        /// </summary>
        private void RedirectPath()
        {
            // Redirect to the requested page.

            if (!String.IsNullOrEmpty(Request.QueryString["ReturnUrl"]))
            {
                string returnUrl = Request.QueryString["ReturnUrl"];
                if (!string.IsNullOrEmpty(returnUrl))
                    Response.Redirect(returnUrl, false);
                else
                    Response.Redirect("/", false);

            }
            else
            {
                Response.Redirect(CommonOpn.AppPath + "MyBikeWale/", false);
            }
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }
    }
}
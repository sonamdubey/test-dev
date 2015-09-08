using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using Bikewale.Common;
using Bikewale.Entities.Customer;
using Bikewale.UI.Entities.Customer;

namespace Bikewale.Controls
{
    /// <summary>
    /// Created By : Ashish G. Kamble on 3 Sept 2015
    /// Summary : Class have function to login, logout and register the user.
    /// </summary>
    public class LoginControlNew : System.Web.UI.UserControl
    {
        protected Button btnLogin, btnSignup;
        protected CheckBox chkRemMe;
        protected HiddenField hdnAuthData;
        protected TextBox txtLoginEmail, txtLoginPassword, txtNameSignup, txtEmailSignup, txtMobileSignup, txtRegPasswdSignup;

        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
            btnLogin.Click += new EventHandler(LoginUser);
            btnSignup.Click += new EventHandler(RegisterCustomer);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoginUser(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(hdnAuthData.Value))
                {
                    // Desirialize the response for the authentication
                    System.Web.Script.Serialization.JavaScriptSerializer cityjson = new System.Web.Script.Serialization.JavaScriptSerializer();
                    AuthenticatedCustomer objCust = (AuthenticatedCustomer)cityjson.Deserialize(hdnAuthData.Value, typeof(AuthenticatedCustomer));

                    if (objCust.IsAuthorized)
                    {
                        CreateAuthenticationCookie(objCust.AuthenticationTicket, chkRemMe.Checked);                        
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
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
                // Register customer
                string _bwHostUrl = ConfigurationManager.AppSettings["bwHostUrl"];
                string _requestType = "application/json";
                string _apiUrl = String.Format("/api/Customer/?name={0}&email={1}&mobile={2}&password={3}&clientIP={4}", txtNameSignup.Text, txtEmailSignup.Text, txtMobileSignup.Text, txtRegPasswdSignup.Text, CommonOpn.GetClientIP());

                objRegCustomer = Bikewale.Utility.BWHttpClient.PostDataSync<RegisteredCustomer>(_bwHostUrl, _requestType, _apiUrl, objRegCustomer);

                if (objRegCustomer != null && objRegCustomer.IsNewCustomer)
                {
                    // Authenticate the customer
                    CreateAuthenticationCookie(objRegCustomer.AuthenticationTicket, false);    
                }                
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }            
        }


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

            // Redirect to the requested page.
            Response.Redirect(Request.ServerVariables["HTTP_REFERER"], false);            
        }

    }
}
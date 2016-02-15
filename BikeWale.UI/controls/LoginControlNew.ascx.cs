using Bikewale.Common;
using Bikewale.Service.Controllers.Customer;
using Bikewale.UI.Entities.Customer;
using System;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

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
        protected HtmlGenericControl divLogin, divSignUp, loginPopUpWrapper, errorRegister;

        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
            btnLogin.Click += new EventHandler(LoginUser);
            btnSignup.Click += new EventHandler(RegisterCustomer);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            errorRegister.Visible = false;
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
                RegisterInputParameters objReg = new RegisterInputParameters();
                objReg.Name = txtNameSignup.Text.Trim();
                objReg.Email = txtEmailSignup.Text.Trim();
                objReg.Password = txtRegPasswdSignup.Text.Trim();
                objReg.Mobile = txtMobileSignup.Text.Trim();
                objReg.ClientIP = CommonOpn.GetClientIP();

                // Register customer                
                string _apiUrl = "/api/Customer/";

                using (Bikewale.Utility.BWHttpClient objClient = new Utility.BWHttpClient())
                {
                    //objRegCustomer = objClient.PostSync<RegisterInputParameters, RegisteredCustomer>(Utility.BWConfiguration.Instance.BwHostUrl, Utility.BWConfiguration.Instance.APIRequestTypeJSON, _apiUrl, objReg);
                    objRegCustomer = objClient.PostSync<RegisterInputParameters, RegisteredCustomer>(Utility.APIHost.BW, Utility.BWConfiguration.Instance.APIRequestTypeJSON, _apiUrl, objReg);
                }

                if (objRegCustomer != null && objRegCustomer.IsNewCustomer)
                {
                    // Authenticate the customer
                    CreateAuthenticationCookie(objRegCustomer.AuthenticationTicket, false);
                }
                else if (objRegCustomer != null && !objRegCustomer.IsNewCustomer)
                {
                    loginPopUpWrapper.Attributes.Add("style", "right:0px;");
                    divSignUp.Attributes.Add("style", "display:none;");
                    errorRegister.Visible = true;
                    divLogin.Attributes.Add("style", "display:block;");
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
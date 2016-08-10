using Bikewale.Common;
using Bikewale.Common.BWSecurity;
using Bikewale.Service.Controllers.Customer;
using Bikewale.UI.Entities.Customer;
using System;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace BikWale.Users
{
    public class Login : System.Web.UI.Page
    {
        protected Button btnLogin, btnSignup;
        protected HtmlGenericControl errEmail;
        protected TextBox txtLoginid, txtPasswd, txtNameSignup, txtEmailSignup, txtMobileSignup, txtRegPasswdSignup;
        protected HiddenField hdnAuthData;

        private string redirectUrl = "";

        private string header = "Registered Members : Please Login Here";
        private bool _showFooter = true;

        public bool ShowFooter
        {
            get { return _showFooter; }
            set { _showFooter = value; }
        }

        public string RedirectUrl
        {
            set { redirectUrl = value; }
        }

        public string EmailId
        {
            set { txtLoginid.Text = value; }
        }

        public string Header
        {
            get { return header; }
            set { header = value; }
        }

        protected override void OnInit(EventArgs e)
        {
            base.Load += new EventHandler(Page_Load);
            this.btnLogin.Click += new System.EventHandler(this.LoginUser);
            this.btnSignup.Click += new System.EventHandler(this.RegisterCustomer);
        }

        private void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                bool logout = false;
                if (Request["logout"] != null && Request.QueryString["logout"] == "logout")
                {
                    logout = true;
                    Trace.Warn("logout");
                }

                if (logout == true)
                {
                    CurrentUser.EndSession();

                    HttpCookie rememberMe = Request.Cookies.Get("RememberMe");

                    if (rememberMe != null)
                    {
                        rememberMe.Expires = DateTime.Now.AddDays(-1);
                        Response.Cookies.Add(rememberMe);
                    }

                    Response.Redirect("/");
                }
            }

            //check whether the user is already authenticated
            if (HttpContext.Current.User.Identity.IsAuthenticated == true)
            {
                Response.Redirect(CommonOpn.AppPath + "default.aspx");
            }
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

                    if (objCust.IsAuthorized)
                    {
                        CreateAuthenticationCookie(objCust.AuthenticationTicket, true);
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
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }

        /// <summary>
        /// 
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

            // Redirect to the requested page.

            //Response.Redirect(CommonOpn.AppPath + "MyBikeWale/");

            if (!String.IsNullOrEmpty(Request.QueryString["ReturnUrl"]))
            {
                string returnUrl = Request.QueryString["ReturnUrl"];

                if (ScreenInput.IsValidRedirectUrl(returnUrl) == true)
                    Response.Redirect(returnUrl);
                else
                    Response.Redirect("/");
            }
            else if (redirectUrl != "")
            {
                if (ScreenInput.IsValidRedirectUrl(redirectUrl) == true)
                    Response.Redirect(redirectUrl);
                else
                    Response.Redirect("/");
            }
            else
            {
                Response.Redirect(CommonOpn.AppPath + "MyBikeWale/");
            }
        }
    }
}
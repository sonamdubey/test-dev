/*THIS CLASS IS FOR ADDING, EDITING AND DELETING MarketS FROM AND TO THE MarketS TABLE INTO THE DATABASE
*/

using System;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Carwale.UI.Common;
using System.Web.Security;
using System.Web.UI.HtmlControls;
using Carwale.UI.Controls;
using Carwale.Interfaces;
using Carwale.Entity;
using Carwale.BL.Customers;
using Carwale.Utility;
using Carwale.Notifications.Logs;

namespace Carwale.UI.Users {
    public class Login : Page {

        override protected void OnInit(EventArgs e) {
            InitializeComponent();
            base.OnInit(e);
        }

        public string _returnUrl = string.Empty;
        private void InitializeComponent() {
            this.Load += new System.EventHandler(this.Page_Load);
        }

        private void Page_Load(object sender, System.EventArgs e) {
            _returnUrl = GetReturnUrl();
            if (!IsPostBack && Request.HttpMethod == "POST" && Request.Headers["end"] != null && Request.Headers["end"] == "1")
            {
                CurrentUser.EndSession();

                HttpCookie rememberMe = Request.Cookies.Get("RememberMe");

                if (rememberMe != null)
                {

                    if (!string.IsNullOrEmpty(rememberMe.Value))
                    {
                        string[] cred = rememberMe.Value.Split('~');
                        if (cred.Length == 6)
                        {
                            ICustomerBL<Customer, CustomerOnRegister> customerRepo = new CustomerActions<Customer, CustomerOnRegister>();
                            customerRepo.EndRememberMeSession(CurrentUser.Id, cred[3]);
                        }
                    }

                    rememberMe.Expires = DateTime.Now.AddDays(-1);
                    Response.Cookies.Add(rememberMe);
                }
                Response.Redirect(_returnUrl);
            }

            //check whether the user is already authenticated
            if (HttpContext.Current.User.Identity.IsAuthenticated == true)
            {
                Response.Redirect("/");
            }

        }
        private string GetReturnUrl() {
            try
            {
                string returnUrl = Request.QueryString["ReturnUrl"];
                if (!string.IsNullOrEmpty(returnUrl))
                {
                    if (returnUrl.IsAbsoluteUrl())
                    {
                        Uri uri = returnUrl.GetUri();
                        if (uri.Host == Request.Url.Host)
                        {
                            return returnUrl;
                        }
                    }
                    else { return returnUrl; }
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return "/";
        }

    }//class
}//namespace	

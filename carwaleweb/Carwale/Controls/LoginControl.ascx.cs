using System;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Web.UI.HtmlControls;
using Ajax;
using Carwale.Interfaces;
using Carwale.BL.Customers;
using Carwale.Entity;
using Carwale.UI.Common;
using CarwaleAjax;

namespace Carwale.UI.Controls
{
    public class LoginControl : UserControl
    {
        protected TextBox txtLoginid, txtPasswd;
        protected Button butLogin;
        protected HtmlGenericControl spnError;
        protected CheckBox chkRemember;

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

        override protected void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }


        private void InitializeComponent()
        {
            this.butLogin.Click += new System.EventHandler(this.LoginClick);
            this.Load += new System.EventHandler(this.Page_Load);
        }

        private void Page_Load(object sender, System.EventArgs e)
        {
            Ajax.Utility.RegisterTypeForAjax(typeof(AjaxCommon));

            if (!IsPostBack)
            {
                bool logout = false;
                if (Request.Headers["end"] != null && Request.Headers["end"]  == "1")
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

                    Response.Redirect("/");
                }
            }

            //check whether the user is already authenticated
            if (HttpContext.Current.User.Identity.IsAuthenticated == true)
            {
                Response.Redirect(CommonOpn.AppPath + "default.aspx");
            }
        }

        private void LoginClick(object sender, System.EventArgs e)
        {
            if (CurrentUser.Id != "-1")
            {
                if ((Request["ReturnUrl"] != null) && (Request.QueryString["ReturnUrl"] != ""))
                {
                    string returnUrl = Request.QueryString["ReturnUrl"];

                    //validating return url
                    if (ScreenInput.IsValidRedirectUrl(returnUrl) == true)
                        Response.Redirect(returnUrl);
                    else
                        Response.Redirect("/");

                    Trace.Warn("returnUrl " + returnUrl);
                }
                else if (redirectUrl != "")
                {
                    //validating redirect url
                    if (ScreenInput.IsValidRedirectUrl(redirectUrl) == true)
                        Response.Redirect(redirectUrl);
                    else
                        Response.Redirect("/");
                }
                else
                {
                    Response.Redirect(CommonOpn.AppPath + "MyCarwale/");
                }
            }

            Page.Validate();
            if (!Page.IsValid)
            {
                return;
            }

            string loginId = txtLoginid.Text.Trim();
            string pwdEntered = txtPasswd.Text.Trim();
            bool rememberMe = chkRemember.Checked ? true : false;
            CustomersLogin login = new CustomersLogin();

            if (login.DoLogin(loginId, pwdEntered, rememberMe) == true)
            {
                if ((Request["ReturnUrl"] != null) && (Request.QueryString["ReturnUrl"] != ""))
                {
                    string returnUrl = Request.QueryString["ReturnUrl"];

                    if (ScreenInput.IsValidRedirectUrl(returnUrl) == true)
                        Response.Redirect(returnUrl);
                    else
                        Response.Redirect("/");

                    Trace.Warn("returnUrl " + returnUrl);
                }
                else if (redirectUrl != "")
                {
                    Trace.Warn("Rediracting to : " + redirectUrl);
                    if (ScreenInput.IsValidRedirectUrl(redirectUrl) == true)
                        Response.Redirect(redirectUrl);
                    else
                        Response.Redirect("/");
                }
                else
                {
                    Response.Redirect(CommonOpn.AppPath + "MyCarwale/");
                }
            }
            else
            {
                //show the error message
                spnError.InnerText = "Wrong loginid or password.";
            }
        }
    }//class
}//namespace	

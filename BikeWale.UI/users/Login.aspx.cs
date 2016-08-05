using Bikewale.Common;
using Bikewale.Common.BWSecurity;
using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace BikWale.Users
{
    public class Login : System.Web.UI.Page
    {
        protected Button butLogin;
        protected HtmlGenericControl spnError, txtLoginid, txtPasswd, txtEmailSignup;
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
            set { txtLoginid.InnerText = value; }
        }

        public string Header
        {
            get { return header; }
            set { header = value; }
        }

        protected override void OnInit(EventArgs e)
        {
            base.Load += new EventHandler(Page_Load);
            this.butLogin.Click += new System.EventHandler(this.LoginClick);
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
                    Response.Redirect(CommonOpn.AppPath + "MyBikeWale/");
                }
            }

            Page.Validate();
            if (!Page.IsValid)
            {
                return;
            }

            string loginId = txtLoginid.InnerText.Trim();
            string pwdEntered = txtPasswd.InnerText.Trim();
            bool rememberMe = chkRemember.Checked ? true : false;
            CustomersLogin login = new CustomersLogin();

            if (login.DoLogin(loginId, pwdEntered, rememberMe) == true)
            {
                if (!String.IsNullOrEmpty(Request.QueryString["ReturnUrl"]))
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
                    Response.Redirect(CommonOpn.AppPath + "MyBikeWale/");
                }
            }
            else
            {
                //show the error message
                spnError.InnerText = "Wrong loginid or password.";
            }
        }

        void btnRegister_Click(object sender, EventArgs e)
        {
            if (CurrentUser.Id != "-1")
            {
                string returnUrl = "";
                if (Request["returnUrl"] != null && Request["returnUrl"] != "")
                    returnUrl += Request["returnUrl"];
                else if (redirectUrl != "")
                    returnUrl = redirectUrl;
                else
                    returnUrl += "/Users/registrationConfirmation.aspx?returnUrl=" + Request["returnUrl"];

                // redirect to the confirmation page.
                Response.Redirect(returnUrl);

                return;
            }

            string reEmail = @"^[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,6}$";

            if (Regex.IsMatch(txtEmail.Text.Trim().ToLower(), reEmail))
            {
                RegisterCustomer objCust = new RegisterCustomer();

                if (String.IsNullOrEmpty(objCust.IsRegisterdCustomer(txtEmail.Text.Trim())))
                {
                    string CustomerId = objCust.RegisterUser(txtName.Text.Trim(), txtEmail.Text.Trim(), txtMobile.Text.Trim(), "", txtPassword.Text.Trim(), "");
                    if (!String.IsNullOrEmpty(CustomerId))
                    {
                        // If customer registration is successfull then login the user automatically.                        
                        CustomersLogin login = new CustomersLogin();

                        if (login.DoLogin(txtEmail.Text.Trim().ToLower(), txtPassword.Text.Trim(), false) == true)
                        {
                            Response.Redirect(CommonOpn.AppPath + "MyBikeWale/");
                        }
                    }
                }
                else
                {
                    errEmail.InnerHtml = "This email is already registered with us.";
                }
            }
            else
            {
                errEmail.InnerText = "Invalid Email!";
            }
        } // btnSave_Click    
    }
}
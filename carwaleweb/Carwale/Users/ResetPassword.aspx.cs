using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Carwale.Entity;
using Carwale.BL.Customers;
using Carwale.Interfaces;
using Carwale.UI.Common;

namespace Carwale.UI.Users
{
    public class ResetPassword : Page
    {
        static string loginPageUrl = "/users/login.aspx";
        static string homePageUrl = "/";

        protected Customer cust = new Customer();
        protected HtmlInputHidden hdnAT;
        protected HtmlInputText txtNewPassword, txtConfirmPassword;
        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
        }

        void InitializeComponent()
        {
            base.Load += new EventHandler(Page_Load);
        }

        void Page_Load(object Sender, EventArgs e)
        {
            if (Request.RequestType == "GET")
            {
                processGet();
            }
            else if (Request.RequestType == "POST")
            {
                processPost();
            }
            else
                Response.Redirect("/");
        }

        void processGet()
        {
            if (!string.IsNullOrEmpty(Request.QueryString["at"]))
            {
                string AccessToken = Request.QueryString["at"];

                ICustomerBL<Customer, CustomerOnRegister> customerRepo = new CustomerActions<Customer, CustomerOnRegister>();
                cust = customerRepo.GetCustomerByAccessToken(AccessToken);
                if (!string.IsNullOrEmpty(cust.CustomerId) && cust.CustomerId != "-1")
                {
                    hdnAT.Value = AccessToken;
                }
                else
                    Response.Redirect(loginPageUrl);
            }
            else
                Response.Redirect(loginPageUrl);
        }

        void processPost()
        {
            if (!string.IsNullOrEmpty(getFV("hdnAT")))
            {
                string AccessToken = getFV("hdnAT");
                hdnAT.Value = AccessToken;

                ICustomerBL<Customer, CustomerOnRegister> customerRepo = new CustomerActions<Customer, CustomerOnRegister>();
                cust = customerRepo.GetCustomerByAccessToken(AccessToken);

                if (!string.IsNullOrEmpty(getFV("txtNewPassword")) && !string.IsNullOrEmpty(getFV("txtConfirmPassword")) && getFV("txtNewPassword").Trim() == getFV("txtConfirmPassword").Trim())
                {
                    if (customerRepo.ResetPassword(AccessToken, getFV("txtNewPassword").Trim()))
                    {
                        CurrentUser.StartSession(cust.Name, cust.CustomerId, cust.Email, false);
                        Response.Redirect(homePageUrl);
                    }
                    else
                        Response.Redirect(loginPageUrl);
                }
            }
            else
                Response.Redirect(loginPageUrl);
        }

        static string getFV(string paramName)
        {
            return HttpContext.Current.Request.Form[paramName];
        }
    }
}
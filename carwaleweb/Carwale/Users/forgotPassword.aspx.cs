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
using Carwale.Interfaces;
using Carwale.Entity;
using Carwale.BL.Customers;

namespace Carwale.UI.Users
{
    public class ForgotPassword : Page
    {
        protected TextBox txtLoginid;
        protected Button butLogin;
        protected Button butSignup;
        protected HtmlGenericControl spnError;

        override protected void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.butLogin.Click += new System.EventHandler(this.LoginClick);
            this.butSignup.Click += new System.EventHandler(this.Signup_Click);
            this.Load += new System.EventHandler(this.Page_Load);
        }

        private void Page_Load(object sender, System.EventArgs e)
        {
            if (!IsPostBack)
            {
                // Assign the emailId, user was trying while logging-n.
                if (Request["loginId"] != null)
                    txtLoginid.Text = Request["loginId"];
            }

            //check whether the user is already authenticated
            if (HttpContext.Current.User.Identity.IsAuthenticated == true)
            {
                //Response.Redirect(CommonOpn.AppPath + "default.aspx");	
            }
        }

        private void LoginClick(object sender, System.EventArgs e)
        {
            Page.Validate();
            if (!Page.IsValid)
            {
                return;
            }

            if (VerifyLoginId() == true)
            {
                spnError.InnerText = "Congratulations! An email with a link to reset the password has been sent to your email address.";
            }
            else
            {
                //show the error message
                spnError.InnerText = "This email is not registered in CarWale.com";
            }
        }

        private void Signup_Click(object sender, System.EventArgs e)
        {
            Response.Redirect("register.aspx");
        }

        private bool VerifyLoginId()
        {
            //Database db = new Database();

            bool retVal = false;
            string userName = txtLoginid.Text.Trim().Replace("'", "''");
            
            ICustomerBL<Customer, CustomerOnRegister> customerRepo = new CustomerActions<Customer, CustomerOnRegister>();
            retVal = customerRepo.GenPasswordChangeAT(userName);

            return retVal;
        }
    }//class
}//namespace	

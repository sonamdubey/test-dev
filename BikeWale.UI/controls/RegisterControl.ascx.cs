using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Bikewale.Common;

namespace Bikewale.Controls
{
    public class RegisterControl : System.Web.UI.UserControl
    {
        protected HtmlGenericControl spnError, errEmail;
        protected DropDownList cmbAboutCarwale, cmbStates;
        protected Button btnRegister;
        protected TextBox txtName, txtEmail, txtPassword,
                txtMobile, txtEmailConf;
        protected CheckBox chkNewsLetter;

        public bool showContactDetails = false;

        //protected HtmlTableRow trContactDetails;

        private string redirectUrl = "";

        public string RedirectUrl
        {
            set { redirectUrl = value; }
        }       

        public string City
        {
            get
            {
                if (ViewState["City"] != null && ViewState["City"].ToString() != "")
                    return ViewState["City"].ToString();
                else
                    return "";
            }
            set { ViewState["City"] = value; }
        }
		
        protected override void OnInit(EventArgs e)
        {
            base.Load += new EventHandler(Page_Load);
            btnRegister.Click += new EventHandler(btnRegister_Click);
        }
        private  void Page_Load(object sender, EventArgs e)
        {
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
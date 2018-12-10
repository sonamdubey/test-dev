using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Carwale.UI.Common;
using Carwale.UI.Controls;
using Carwale.Interfaces;
using Carwale.Entity;
using Carwale.BL.Customers;

namespace Carwale.UI.Users
{
    public class ChangePassword : Page
    {
        protected HtmlGenericControl spnError;
        protected TextBox txtCurPassword, txtNewPassword, txtConfirmNewPassword;
        protected Button butChange, butCancel;

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
        }

        void InitializeComponent()
        {
            base.Load += new EventHandler(Page_Load);
            butChange.Click += new EventHandler(butChange_Click);
            butCancel.Click += new EventHandler(butCancel_Click);
        }

        void Page_Load(object Sender, EventArgs e)
        {
            if (CurrentUser.Id == "-1")
                Response.Redirect(CommonOpn.AppPath + "Users/login.aspx?redirect=/Users/ChangePassword.aspx");
        } // Page_Load

        //change the password
        //first check whether the current password is correct or not
        //if the password is not correct then show the proper message
        void butChange_Click(object sender, EventArgs e)
        {
            Page.Validate();
            if (!Page.IsValid)
                return;

            string curPasswd = txtCurPassword.Text.Trim();
            string userId = CurrentUser.Id;
            string newPasswd = txtNewPassword.Text.Trim();
            string confPasswd = txtConfirmNewPassword.Text.Trim();

            ICustomerBL<Customer, CustomerOnRegister> customerRepo = new CustomerActions<Customer, CustomerOnRegister>();
            if (newPasswd == confPasswd)
            {
                if (customerRepo.ResetPassword(userId, curPasswd, newPasswd))
                    spnError.InnerText = "The password has been changed successfully.";
                else
                    spnError.InnerText = "The current password given by you is not right. Please try again.";
            }
            else
                spnError.InnerText = "New password and confirmation passwords don't match. Please try again";

        }

        void butCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect(CommonOpn.AppPath + "default.aspx");
        }
    } // class
} // namespace
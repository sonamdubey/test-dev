/*******************************************************************************************************
THIS CLASS IS FOR CHANGING THE PASSWORD .
*******************************************************************************************************/
using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Bikewale.Common;
using Bikewale.Controls;

namespace Bikewale.Users
{
	public class ChangePassword : Page
	{
		protected HtmlGenericControl spnError;
		protected TextBox txtCurPassword, txtNewPassword;
		protected Button butChange, butCancel;	
		
		protected override void OnInit( EventArgs e )
		{
			InitializeComponent();
		}
		
		void InitializeComponent()
		{
			base.Load += new EventHandler( Page_Load );
			butChange.Click += new EventHandler(butChange_Click);
			butCancel.Click += new EventHandler(butCancel_Click);
		}
		
		void Page_Load( object Sender, EventArgs e )
		{
			if ( CurrentUser.Id == "-1" )
                Response.Redirect(CommonOpn.AppPath + "Users/login.aspx?ReturnUrl=/Users/ChangePassword.aspx");
		} // Page_Load
		
		//change the password
		//first check whether the current password is correct or not
		//if the password is not correct then show the proper message
		void butChange_Click(object sender, EventArgs e)
		{
			Page.Validate();
			if(!Page.IsValid)
				return;
				
			string curPasswd = txtCurPassword.Text.Trim();	
			string userId = CurrentUser.Id;
            string email = CurrentUser.Email;
            
            RegisterCustomer objCust = new RegisterCustomer();
            Customers objCustomers = objCust.IsValidPassword(curPasswd, email);
            Trace.Warn("Customers Class user id : ", objCustomers.Id);

            if (!String.IsNullOrEmpty(objCustomers.Id))
			{
				//get the new passwword
				string newPasswd = txtNewPassword.Text.Trim().Replace("'","''");

                string newSalt = objCust.GenerateRandomSalt();
                string newHash = objCust.GenerateHashCode(newPasswd, newSalt);

                Trace.Warn("cust id : " + userId);
                Trace.Warn("new salt : " + newSalt);
                Trace.Warn("new hash : " + newHash);

                // Update salt and hash for the customer
                objCust.UpdatePassword(newSalt, newHash, userId);
                spnError.InnerText = "The password has been changed successfully.";				
			}
			else
			{
				//show the message that the current password is not right
				spnError.InnerText = "The current password given by you is not right. Please try again.";
			}
		}
		
		void butCancel_Click(object sender, EventArgs e)
		{
			Response.Redirect( CommonOpn.AppPath + "default.aspx" );
		}		
	} // class
} // namespace
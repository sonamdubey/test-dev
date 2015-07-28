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
    /// <summary>
    ///     Created By : Ashish G. Kamble on 6 Nov 2012
    ///     Summary : Class for reseting the customer password in case customer forget password.
    /// </summary>
    public class ResetCustomerPassword : Page
	{
        protected HtmlGenericControl spnError, divErrMsg;
		protected TextBox txtCurPassword, txtNewPassword;
		protected Button butChange, butCancel;
        protected HtmlTable tblPassword;
        protected string token = string.Empty, customerId = string.Empty;
		
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
            divErrMsg.Visible = false;

            ValidateToken();                    
		} // Page_Load

        protected void ValidateToken()
        {
            string email = string.Empty;
            
            token = Request.QueryString["tkn"].ToString();            
            Trace.Warn("token : " + token);

            if (!String.IsNullOrEmpty(token))
            {                
                try
                {
                    RegisterCustomer objCust = new RegisterCustomer();
                    try
                    {
                        // Check if customer token has space in it. If space is there replace it with +
                        if (token.IndexOf(" ") > 0)
                        {
                            token = token.Replace(" ", "+");
                        }

                        // Decrypt password token to get email id
                        email = objCust.DecryptPasswordToken(token);
                        Trace.Warn("emailid : " + email);

                        // Check if customer is registered or not
                        customerId = objCust.IsRegisterdCustomer(email);
                        Trace.Warn("customer id : " + customerId);

                        if (!String.IsNullOrEmpty(customerId))
                        {
                            // Check if password recovery token is active or not.
                            if (!objCust.IsValidPasswordRecoveryToken(customerId, token))
                            {
                                divErrMsg.InnerText = "Your link to reset the password is expired.";
                                divErrMsg.Visible = true;
                                tblPassword.Visible = false;
                            }
                        }
                        else
                        {
                            divErrMsg.InnerText = "You are not authorized to reset the password.";
                            divErrMsg.Visible = true;
                            tblPassword.Visible = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        HttpContext.Current.Trace.Warn("can not decr" + ex.Message);
                        ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                        objErr.SendMail();

                        divErrMsg.InnerText = "You are not authorized to reset the password.";
                        divErrMsg.Visible = true;
                        tblPassword.Visible = false;
                    }
                }
                catch (Exception ex)
                {
                    ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                    objErr.SendMail();
                }
            }
        }   // End of ValidateToken method

		//change the password
		//first check whether the current password is correct or not
		//if the password is not correct then show the proper message
		void butChange_Click(object sender, EventArgs e)
		{
			Page.Validate();
			if(!Page.IsValid)
				return;
							
			string userId = customerId;
            
            RegisterCustomer objCust = new RegisterCustomer();
				
			//get the new passwword
			string newPasswd = txtNewPassword.Text.Trim().Replace("'","''");

            // Generate random salt and hash from the password given by customer
            string newSalt = objCust.GenerateRandomSalt();
            string newHash = objCust.GenerateHashCode(newPasswd, newSalt);

            Trace.Warn("cust id : " + userId);
            Trace.Warn("new salt : " + newSalt);
            Trace.Warn("new hash : " + newHash);

            // Update salt and hash for the customer
            objCust.UpdatePassword(newSalt, newHash, userId);
            spnError.InnerText = "The password has been changed successfully.";

            // Make password recovery token inactive
            objCust.UpdatePasswordRecoveryTokenStatus(customerId);
		}
		
		void butCancel_Click(object sender, EventArgs e)
		{
			Response.Redirect( CommonOpn.AppPath + "default.aspx" );
		}		
	} // class
} // namespace
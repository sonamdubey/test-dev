using Bikewale.Common;
using Bikewale.Utility;
using log4net;
/*******************************************************************************************************
THIS CLASS IS FOR CHANGING THE PASSWORD .
*******************************************************************************************************/
using System;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

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
        protected bool enablePwdResetLogging = BWConfiguration.Instance.EnablePwdResetLogging;
        protected ILog _logger = LogManager.GetLogger("Logger-ResetCustomerPassword");
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
            divErrMsg.Visible = false;

            ValidateToken();
        } // Page_Load

        protected void ValidateToken()
        {
            string email = string.Empty;

            token = Request.QueryString["tkn"];
            Trace.Warn("token : " + token);
            StringBuilder msgQ = new StringBuilder();
            if (!String.IsNullOrEmpty(token))
            {
                try
                {
                    RegisterCustomer objCust = new RegisterCustomer();
                    try
                    {
                        msgQ.AppendLine(String.Format("ValidateToken called -> ({0})", token));
                        // Check if customer token has space in it. If space is there replace it with +
                        if (token.IndexOf(" ") > 0)
                        {
                            msgQ.AppendLine("token.IndexOf(\" \") > 0 is true");
                            token = token.Replace(" ", "+");
                            msgQ.AppendLine(String.Format("Token  -> ({0})", token));
                        }

                        // Decrypt password token to get email id
                        email = objCust.DecryptPasswordToken(token);
                        msgQ.AppendLine("emailid : " + email);

                        // Check if customer is registered or not
                        customerId = objCust.IsRegisterdCustomer(email);
                        msgQ.AppendLine("customer id : " + customerId);

                        if (!String.IsNullOrEmpty(customerId))
                        {
                            // Check if password recovery token is active or not.
                            if (!objCust.IsValidPasswordRecoveryToken(customerId, token))
                            {
                                divErrMsg.InnerText = "Your link to reset the password is expired.";
                                divErrMsg.Visible = true;
                                tblPassword.Visible = false;
                                msgQ.AppendLine("Your link to reset the password is expired.");
                            }
                        }
                        else
                        {
                            divErrMsg.InnerText = "You are not authorized to reset the password.";
                            divErrMsg.Visible = true;
                            tblPassword.Visible = false;
                            msgQ.AppendLine("You are not authorized to reset the password.");
                        }
                    }
                    catch (Exception ex)
                    {
                        ErrorClass.LogError(ex, "ValidateToken");
                        divErrMsg.InnerText = "You are not authorized to reset the password.";
                        divErrMsg.Visible = true;
                        tblPassword.Visible = false;

                        msgQ.AppendLine("Inner Exception block.");
                    }
                }
                catch (Exception ex)
                {
                    ErrorClass.LogError(ex, "ValidateToken");
                    msgQ.AppendLine("Outer Exception block.");
                }
                finally
                {
                    if (enablePwdResetLogging)
                    {
                        _logger.Error(msgQ.ToString());
                    }
                }
            }
        }   // End of ValidateToken method

        //change the password
        //first check whether the current password is correct or not
        //if the password is not correct then show the proper message
        void butChange_Click(object sender, EventArgs e)
        {
            Page.Validate();
            if (!Page.IsValid)
                return;

            string userId = customerId;

            RegisterCustomer objCust = new RegisterCustomer();

            //get the new passwword
            string newPasswd = txtNewPassword.Text.Trim().Replace("'", "''");

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
            Response.Redirect(CommonOpn.AppPath + "default.aspx");
        }
    } // class
} // namespace
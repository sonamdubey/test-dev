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


        // Modified by : Ashish G. Kamble on 22 Dec 2017 
        // Modified : Enabled Logging for password reset process
        protected void ValidateToken()
        {
            string email = string.Empty;

            token = Request.QueryString["tkn"];
            
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
                        if (token.IndexOf(" ") > -1)
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
                            else msgQ.AppendLine("Valid Password Token.");
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
        // Modified by : Ashish G. Kamble on 22 Dec 2017 
        // Modified : Enabled Logging for password reset process
        void butChange_Click(object sender, EventArgs e)
        {
            StringBuilder msgQ = new StringBuilder();

            try
            {
                Page.Validate();
                
                if (!Page.IsValid)
                {
                    msgQ.AppendLine("!Page.IsValid");
                    return;
                }

                string userId = customerId;

                RegisterCustomer objCust = new RegisterCustomer();

                msgQ.AppendLine("new password processing started");

                //get the new passwword
                string newPasswd = txtNewPassword.Text.Trim().Replace("'", "''");

                msgQ.AppendLine("new password processing done");

                // Generate random salt and hash from the password given by customer
                string newSalt = objCust.GenerateRandomSalt();
                string newHash = objCust.GenerateHashCode(newPasswd, newSalt);
                
                msgQ.AppendLine("userId : " + userId);
                msgQ.AppendLine("newSalt : " + newSalt);
                msgQ.AppendLine("newHash : " + newHash);

                msgQ.AppendLine("new password update started");

                // Update salt and hash for the customer
                objCust.UpdatePassword(newSalt, newHash, userId);
                spnError.InnerText = "The password has been changed successfully.";

                msgQ.AppendLine("new password update done");

                // Make password recovery token inactive
                objCust.UpdatePasswordRecoveryTokenStatus(customerId);

                msgQ.AppendLine("user token invalidated");
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "ResetCustomerPassword.butChange_Click");
                msgQ.AppendLine("ResetCustomerPassword.butChange_Click Exception block.");
            }
            finally
            {
                if (enablePwdResetLogging)
                {
                    _logger.Error(msgQ.ToString());
                }
            }
        }

        void butCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("/");
        }
    } // class
} // namespace
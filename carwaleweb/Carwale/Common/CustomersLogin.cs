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
using Carwale.Notifications;
using Carwale.DAL.CoreDAL;
using Carwale.Notifications.Logs;

namespace Carwale.UI.Common
{
    public class CustomersLogin
    {
        // Individual Login
        public bool DoLogin(string loginId, string passwdEnter, bool rememberMe)
        {
            bool retVal = false;           
            string userId = "";
            string name = "";
            string isEmailVerified = "";
            try
            {
                ICustomerBL<Customer, CustomerOnRegister> customerRepo = new CustomerActions<Customer, CustomerOnRegister>();
                Customer customer = customerRepo.GetCustomer(loginId, passwdEnter);
                userId = customer.CustomerId;
                name = customer.Name;
                isEmailVerified = customer.IsEmailVerified.ToString();
                HttpContext.Current.Trace.Warn("userId", userId);
                if (userId == "-1")
                {
                    retVal = false;
                }
                else
                {
                    CurrentUser.StartSession(name, userId, loginId, Convert.ToBoolean(isEmailVerified));
                    retVal = true;
                    // if visitor intends to remain login forever
                    if (rememberMe)
                        customerRepo.CreateRememberMeSession(userId);
                }
            }
            catch (Exception err)
            {
                HttpContext.Current.Trace.Warn(err.Message);
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }          
            return retVal;
        }
    }
}
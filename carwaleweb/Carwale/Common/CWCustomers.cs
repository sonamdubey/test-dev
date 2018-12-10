/*
	Comman class for customers related operations
	Written by: Satish Sharma On Jul 30, 2009 12:28 PM
*/

using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Configuration;
using Carwale.UI.Common;
using Carwale.Entity;
using Carwale.BL.Customers;
using Carwale.Interfaces;
using Carwale.Notifications;
using Carwale.Entity.Enum;

namespace Carwale.UI.Common
{
    public class Customers
    {
        //used for writing the debug messages
        private HttpContext objTrace = HttpContext.Current;

        public string RegisterCustomer(string customerName, string password, string email, string phone, string mobile, string cityId, string fbId = "", string gplusId = "", bool isOpenUserVerified = false)
        {
            if (customerName == "" || email == "" || (password == "" && fbId == "" && gplusId == ""))
            {
                return "";
            }

            string retStr = "";
            CustomerOnRegister customer = new CustomerOnRegister();
            string val = string.Empty;
            ICustomerBL<Customer, CustomerOnRegister> customerRepo = new CustomerActions<Customer, CustomerOnRegister>();
            try
            {
                customer = customerRepo.CreateCustomer(new Customer()
                {
                    Name = customerName,
                    Email = email,
                    Mobile = !string.IsNullOrEmpty(mobile) ? mobile : phone,
                    Password = password,
                    FacebookId = fbId,
                    GoogleId = gplusId,
                    openUserVerified = isOpenUserVerified
                });
                retStr = customer.CustomerId;
            }
            catch (Exception err)
            {
                objTrace.Trace.Warn(err.Message);
                ErrorClass objErr = new ErrorClass(err, objTrace.Request.ServerVariables["URL"]);
                objErr.SendMail();
                retStr = null;
                return retStr;
            } // catch Exception
            finally
            {
                //"S" is for Social Login
                if (customer.StatusOnRegister == "S")
                {
                    CurrentUser.StartSession(customerName, customer.CustomerId, email, isOpenUserVerified);
                    if (isOpenUserVerified) customerRepo.CreateRememberMeSession(customer.CustomerId);
                }

                // if its a fresh registration!
                else if (customer.StatusOnRegister == "N")
                {
                    // login this user.
                    CurrentUser.StartSession(customerName, customer.CustomerId, email, false);

                    //also update the SourceId
                    SourceIdCommon.UpdateSourceId(EnumTableType.Customers, customer.CustomerId);
                }
                // Already registered.
                else if (customer.StatusOnRegister == "O")
                {
                    retStr = "<p>This email-id is already registered in Carwale. "
                                + "If you have forgotten your password, "
                                + "<a style=\"color:blue;\" target=\"_blank\" href=\"forgotPassword.aspx?loginid="
                                + email + "\">Click Here</a> to recover "
                                + "Or try with another email-id.</p>";
                }
            }

            return retStr;
        }
    }//class
}//namespace

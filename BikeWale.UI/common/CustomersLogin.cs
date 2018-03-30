using System;
using System.Web;

namespace Bikewale.Common
{
    public class CustomersLogin
    {
        // Individual Login
        public bool DoLogin(string loginId, string passwdEnter, bool rememberMe)
        {
            bool retVal = false;
            string userId = "", name = "";

            try
            {
                // Check whether given password and password in db matches or not.
                RegisterCustomer objCust = new RegisterCustomer();
                Customers objCustomers = objCust.IsValidPassword(passwdEnter, loginId);

                //check the password is valid or not
                if (!String.IsNullOrEmpty(objCustomers.Id))
                {
                    userId = objCustomers.Id;
                    name = objCustomers.Name;

                    CurrentUser.StartSession(name, userId, loginId);

                    // if visitor intends to remain login forever
                    if (rememberMe)
                    {
                        string credentials = "";
                        // create credentials like in the following format
                        // userId~userName~Email~Password~isEmailVerified 
                        credentials = HttpUtility.UrlEncode(BikewaleSecurity.EncryptUsingSymmetric("rememberme", userId)) + "~"
                                    + HttpUtility.UrlEncode(BikewaleSecurity.EncryptUsingSymmetric("rememberme", name)) + "~"
                                    + HttpUtility.UrlEncode(BikewaleSecurity.EncryptUsingSymmetric("rememberme", loginId)) + "~";

                        HttpCookie rememberCookie = new HttpCookie("RememberMe");
                        rememberCookie.Value = credentials;
                        rememberCookie.Expires = DateTime.Now.AddYears(1);
                        HttpContext.Current.Response.Cookies.Add(rememberCookie);
                    }

                    retVal = true;
                }
                else
                {
                    retVal = false;
                }
            }
            catch (Exception err)
            {
                HttpContext.Current.Trace.Warn(err.Message);
                ErrorClass.LogError(err, HttpContext.Current.Request.ServerVariables["URL"]);
                
            } // catch Exception

            return retVal;
        }

        /* 
            Dealer login
            This function will return true if dealer login successfull else will return false;
            Added in this class on 16-May-09 By Satish Sharma			
        */
        public bool DoDealerLogin(string loginId, string passwdEnter, bool rememberMe)
        {
            ErrorClass.LogError(new Exception("Method not used/commented"), "CustomersLogin.DoDealerLogin");
            
            return false;
          
        }

        private void ClearAllCookiesValues()
        {
            //this function clears all the cookies values, and is to be updated 
            //in case new cookies are added
            int i;
            int limit = HttpContext.Current.Request.Cookies.Count - 1;
            for (i = 0; i <= limit; i++)
            {
                HttpCookie aCookie = HttpContext.Current.Request.Cookies[i];
                aCookie.Expires = DateTime.Now.AddDays(-1);
                HttpContext.Current.Response.Cookies.Add(aCookie);
            }
        }
    }
}
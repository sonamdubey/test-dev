// Current User Class
//

using System;
using System.Web;
using System.Configuration;
using System.Web.Mail;
using System.Text;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.IO;
using System.Web.Security;
using System.Security.Cryptography;

namespace Bikewale.Common 
{
	public class CurrentUser
	{
        ///<summary>
        /// This PopulateWhere gets the current user id as logged in. 
        ///if no user is logged in then it returns -1
        ///</summary>
        public static string Id
        {
            get
            {
                string userId = "-1";

                if (HttpContext.Current.User.Identity.IsAuthenticated == true)
                {
                    FormsIdentity fi = (FormsIdentity)HttpContext.Current.User.Identity;
                    FormsAuthenticationTicket ticket = fi.Ticket;

                    string strRole = ticket.UserData.Split(':')[1].ToString().ToUpper();
                    string strUserId = ticket.UserData.Split(':')[0].ToString();

                    if (strRole == "DEALERS")
                    {
                        string mappedCustId = GetMappedCustomerId(strUserId);

                        if (mappedCustId != "")
                            userId = mappedCustId;
                    }
                    else
                        userId = ticket.UserData.Split(':')[0].ToString();
                }

                return userId;
            }
        }


        public static string Role
        {
            get
            {
                string userRole = "-1";

                if (HttpContext.Current.User.Identity.IsAuthenticated == true)
                {
                    FormsIdentity fi = (FormsIdentity)HttpContext.Current.User.Identity;
                    FormsAuthenticationTicket ticket = fi.Ticket;

                    string strRole = ticket.UserData.Split(':')[1].ToString().ToUpper();

                    if (strRole == "DEALERS")
                        userRole = "DEALER";
                    else
                        userRole = "INDIVIDUAL";
                }

                return userRole;
            }
        }


        ///<summary>
        /// This PopulateWhere gets the current user email as logged in. 
        ///if no user is logged in then it returns ""
        ///</summary>
        public static string Email
        {
            get
            {
                string email = "";
                if (HttpContext.Current.User.Identity.IsAuthenticated == true)
                {
                    FormsIdentity fi = (FormsIdentity)HttpContext.Current.User.Identity;
                    FormsAuthenticationTicket ticket = fi.Ticket;
                    email = ticket.UserData.Split(':')[1].ToString();
                }

                return email;
            }
        }

        ///<summary>
        /// This PopulateWhere gets the current user name as logged in. 
        ///if no user is logged in then it returns ""
        ///</summary>
        public static string Name
        {
            get
            {
                string userName = "";
                if (HttpContext.Current.User.Identity.IsAuthenticated == true)
                {
                    userName = HttpContext.Current.User.Identity.Name;
                }

                return userName;
            }
        }

        ///<summary>
        /// This PopulateWhere gets the current user name as logged in. 
        ///if no user is logged in then it returns ""
        ///</summary>
        public static bool EmailVerified
        {
            get
            {
                bool isVerified = false;
                string userName = "";
                if (HttpContext.Current.User.Identity.IsAuthenticated == true)
                {
                    FormsIdentity fi = (FormsIdentity)HttpContext.Current.User.Identity;
                    FormsAuthenticationTicket ticket = fi.Ticket;
                    userName = ticket.UserData.Split(':')[2].ToString();
                }
                if (userName != "")
                {
                    if (userName.ToLower() == "false")
                        isVerified = false;
                    else
                        isVerified = true;
                }

                return isVerified;
            }
        }

        public static void StartSession(string userName, string userId, string email)
        {
            //create a ticket and add it to the cookie
            FormsAuthenticationTicket ticket;
            //now add the id and the role to the ticket, concat the id and role, separated by ',' 
            ticket = new FormsAuthenticationTicket(
                        1,
                        userName,
                        DateTime.Now,
                        DateTime.Now.AddDays(365),
                        false,
                        userId + ":" + email
                    );

            //add the ticket into the cookie
            HttpCookie objCookie;
            objCookie = new HttpCookie("_bikewale");
            objCookie.Value = FormsAuthentication.Encrypt(ticket);
            HttpContext.Current.Response.Cookies.Add(objCookie);

            // delete auction cookie if not exists
            // it will be reassign
            DeleteAuctionCookie();
        }

        public static void StartSession(string userName, string userId, string email, bool rememberMe)
        {
            //create a ticket and add it to the cookie
            FormsAuthenticationTicket ticket;
            //now add the id and the role to the ticket, concat the id and role, separated by ',' 
            ticket = new FormsAuthenticationTicket(
                        1,
                        userName,
                        DateTime.Now,
                        DateTime.Now.AddDays(365),
                        false,
                        userId + ":" + email
                    );

            //add the ticket into the cookie
            HttpCookie objCookie;
            objCookie = new HttpCookie("_bikewale");
            objCookie.Value = FormsAuthentication.Encrypt(ticket);

            if (rememberMe)
            {
                objCookie.Expires = DateTime.Now.AddYears(1);
            }

            HttpContext.Current.Response.Cookies.Add(objCookie);

            // delete auction cookie if not exists
            // it will be reassign
            DeleteAuctionCookie();
        }

        public static void EndSession()
        {
            FormsAuthentication.SignOut();

            // Added by : Ashish G. Kamble on 5 Sept 2015. Remove remember me cookie
            HttpCookie rememberMe = HttpContext.Current.Request.Cookies.Get("RememberMe");

            if (rememberMe != null)
            {
                rememberMe.Expires = DateTime.Now.AddDays(-1);
                HttpContext.Current.Response.Cookies.Add(rememberMe);
            }

            //also clear the cookie for the contact information
            CommonOpn op = new CommonOpn();
            op.ExpireNeedContactInformation();

            // delete auction cookie if not exists
            // it will be reassign
            DeleteAuctionCookie();
        }

        //this function checks whether this emial is with carwale or not.
        //if it exists then return true else return false
        public static bool CheckEmailWithBikeWale(string emailId)
        {
            Database db = new Database();
            string sql = "";
            SqlDataReader dr = null;
            bool exist = false;

            sql = " SELECT email FROM Customers With(NoLock) WHERE email = @emailId";

            SqlParameter[] param = { new SqlParameter("@emailId", emailId.Trim()) };

            try
            {
                dr = db.SelectQry(sql, param);
                if (dr.Read())
                {
                    exist = true;
                }
                
            }
            catch (Exception err)
            {
                HttpContext.Current.Trace.Warn(err.Message);
                ErrorClass objErr = new ErrorClass(err, "CurrentUser.CheckEmailWithCarwale");
                objErr.SendMail();
            }
            finally
            {
                if(dr != null)
                    dr.Close();

                db.CloseConnection();
            }
            return exist;
        }


        public static bool AutomaticLogin(string code)
        {
            bool allowed = false;
            string sql = "";
            Database db = new Database();

            try
            {
                string userIdTemp = BikewaleSecurity.GetCustomerIdFromKey(code);

                if (userIdTemp != "-1")
                    allowed = true;
                else
                    allowed = false;

                if (allowed == true)
                {
                    CustomerDetails cd = new CustomerDetails(userIdTemp);

                    // end current session.
                    EndSession();

                    // create fresh session.
                    StartSession(cd.Name, userIdTemp, cd.Email);

                    //update the isemailverified Budget of the customer
                    sql = " Update Customers Set IsVerified = 1 Where Id = @userIdTemp";
                    SqlParameter[] param = { new SqlParameter("@userIdTemp", userIdTemp) };
                    db.UpdateQry(sql, param);
                }
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, "CurrentUser.AutomaticLogin");
                objErr.SendMail();
            }

            return allowed;
        }

        private static string GetMappedCustomerId(string dealerId)
        {
            string sql = "";
            string mappedCustomerId = "";

            sql = " SELECT CustomerId FROM MapDealers With(NoLock) WHERE DealerId = @dealerId";

            SqlParameter[] param = { new SqlParameter("@dealerId", dealerId) };

            Database db = new Database();
            SqlDataReader dr = null;

            try
            {
                dr = db.SelectQry(sql, param);

                if (dr.Read())
                {
                    mappedCustomerId = dr[0].ToString();
                }                
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, "CurrentUser.AutomaticLogin");
                objErr.SendMail();
            }
            finally
            {
                if(dr != null)
                    dr.Close();

                db.CloseConnection();
            }
            return mappedCustomerId;
        }

        static void DeleteAuctionCookie()
        {
            //Delete auction cookies if exists
            if (HttpContext.Current.Request.Cookies["CookieBidderId"] != null)
            {
                HttpContext.Current.Response.Cookies["CookieBidderId"].Expires = DateTime.Now.AddYears(-1);
            }
        }
		
    }//class
}//namespace
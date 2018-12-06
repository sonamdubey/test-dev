// Current User Class
//

using MySql.CoreDAL;
using System;
using System.Data;
using System.Data.Common;
using System.Web;
using System.Web.Security;

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
                    FormsIdentity fi = HttpContext.Current.User.Identity as FormsIdentity;
                    if(fi != null)
                    {
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
                }

                return userId;
            }
        }
        /// <summary>
        /// Modified by: Sangram Nandkhile on 16 Nov 2016
        /// Desc: Added a Uint variable for ID which can be used to check user is logged in or not.
        /// This has been introduced to avoid string comparsion to check with variable Id
        /// </summary>
        public static uint UserId
        {
            get
            {
                uint outId = 0;
                if (!string.IsNullOrEmpty(Id))
                {
                    uint.TryParse(Id, out outId);
                }
                return outId;
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
            string sql = "";
            bool exist = false;

            sql = " select email from customers  where email = @v_emailid limit 1";

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand(sql))
                {
                    cmd.Parameters.Add(DbFactory.GetDbParam("@v_emailid", DbType.String, 100, emailId));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null && dr.Read())
                        {
                            exist = true;
                            dr.Close();
                        }
                    }
                }

            }
            catch (Exception err)
            {
                HttpContext.Current.Trace.Warn(err.Message);
                ErrorClass.LogError(err, "CurrentUser.CheckEmailWithCarwale");
                
            }

            return exist;
        }


        public static bool AutomaticLogin(string code)
        {
            bool allowed = false;
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
                    string sql = " update customers set isverified = 1 where id = @v_useridtemp";

                    using (DbCommand cmd = DbFactory.GetDBCommand(sql))
                    {
                        cmd.Parameters.Add(DbFactory.GetDbParam("@v_useridtemp", DbType.String, 50, userIdTemp));

                        MySqlDatabase.UpdateQuery(cmd, ConnectionType.MasterDatabase);
                    }

                }
            }
            catch (Exception err)
            {
                ErrorClass.LogError(err, "CurrentUser.AutomaticLogin");
                
            }

            return allowed;
        }

        private static string GetMappedCustomerId(string dealerId)
        {
            string sql = "";
            string mappedCustomerId = "";

            sql = " select customerid from mapdealers where dealerid = @v_dealerid";

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand(sql))
                {
                    cmd.Parameters.Add(DbFactory.GetDbParam("@v_dealerid", DbType.Int32, dealerId));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null && dr.Read())
                        {
                            mappedCustomerId = dr[0].ToString();
                            dr.Close();
                        }
                    }
                }
            }
            catch (Exception err)
            {
                ErrorClass.LogError(err, "CurrentUser.AutomaticLogin");
                
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
/*******************************************************************************************************
IN THIS CLASS THE NEW MEMBEERS WHO HAVE REQUESTED FOR REGISTRATION ARE SHOWN
*******************************************************************************************************/
using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Carwale.UI.Common;
using System.Text.RegularExpressions;
using Carwale.Notifications;
using Carwale.DAL.CoreDAL;
using Carwale.Notifications.Logs;
using Carwale.DAL.CoreDAL.MySql;
using System.Data.Common;

namespace Carwale.UI.Common
{
    public class ForumsCommon
    {
        // This function will manage when a user was last logged into Forums.
        public static void ManageLastLogin()
        {
            if (CurrentUser.Id != "-1") // user is logged in
            {
                // cookie is not available.
                if (HttpContext.Current.Request.Cookies["ForumLastLogin"] == null)
                {              
                    try
                    {                     
                        DateTime lastLogin = new DateTime();                      
                        // if found in db
                        using (DbCommand cmd = DbFactory.GetDBCommand("ManageForumLastLogin_v16_11_7"))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(DbFactory.GetDbParam("v_UserId", DbType.Int64, CurrentUser.Id));
                            using (IDataReader reader = MySqlDatabase.SelectQuery(cmd, DbConnections.ForumsMySqlMasterConnection))
                            {
                                if (reader.Read())
                                {
                                    if (reader["ForumLastLogin"].ToString() != "")
                                        lastLogin = Convert.ToDateTime(reader["ForumLastLogin"]);
                                }
                            }
                        }                                         
                        // in case not available in db, keep last visit as two days ago.
                        if (lastLogin.Year < 2000)
                        {
                            lastLogin = DateTime.Now.AddDays(-2);
                        }

                        // set last visit cookie.
                        HttpCookie lastLoginCookie = new HttpCookie("ForumLastLogin");
                        lastLoginCookie.Value = lastLogin.ToString();
                        lastLoginCookie.Expires = DateTime.Now.AddHours(2);
                        HttpContext.Current.Response.Cookies.Add(lastLoginCookie);

                        using (DbCommand cmd = DbFactory.GetDBCommand("UpdateForumLastLoginTime_v16_11_7"))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(DbFactory.GetDbParam("v_UserId", DbType.Int64, CurrentUser.Id));
                            MySqlDatabase.UpdateQuery(cmd, DbConnections.ForumsMySqlMasterConnection);
                        }                     
                    }
                    catch (Exception err)
                    {
                        HttpContext.Current.Trace.Warn(err.Message);
                        ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                        objErr.SendMail();
                    }
                }
            }
        }
    } // class
} // namespace
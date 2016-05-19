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
using Bikewale.Common;
using Bikewale.Community.PMS;

namespace Bikewale.Forums.Common
{
	public class ForumsCommon
	{
		public static string CreateNewThread( string customerId, string messageText, int alertType, string forumId, string title )
        {
            throw new Exception("Method not used/commented");

            //string threadId = "-1";
			
            //SqlConnection con;
            //SqlCommand cmd;
            //SqlParameter prm;
            //Database db = new Database();
            //CommonOpn op = new CommonOpn();
						
            //string conStr = db.GetConString();
			
            //con = new SqlConnection( conStr );
			
            //try
            //{		
            //    cmd = new SqlCommand("CreateForumThread", con);
            //    cmd.CommandType = CommandType.StoredProcedure;
			
            //    prm = cmd.Parameters.Add("@ForumSubCategoryId", SqlDbType.BigInt);
            //    prm.Value = forumId;
				
            //    prm = cmd.Parameters.Add("@CustomerId", SqlDbType.BigInt);
            //    prm.Value = customerId;
												
            //    prm = cmd.Parameters.Add("@Topic", SqlDbType.VarChar, 200);
            //    prm.Value = title;
								
            //    prm = cmd.Parameters.Add("@StartDateTime", SqlDbType.DateTime);
            //    prm.Value = DateTime.Now;
				
            //    prm = cmd.Parameters.Add("@Message", SqlDbType.VarChar, 4000);
            //    prm.Value = messageText;
				
            //    prm = cmd.Parameters.Add("@AlertType", SqlDbType.Int);
            //    prm.Value = alertType;
														
            //    prm = cmd.Parameters.Add("@ThreadId", SqlDbType.BigInt);
            //    prm.Direction = ParameterDirection.Output;
						
            //    con.Open();
            //    //run the command
            //    cmd.ExecuteNonQuery();
				
            //    threadId = cmd.Parameters[ "@ThreadId" ].Value.ToString();
            //}
            //catch(Exception err)
            //{
            //    HttpContext.Current.Trace.Warn(err.Message);
            //    ErrorClass objErr = new ErrorClass(err,HttpContext.Current.Request.ServerVariables["URL"]);
            //    objErr.SendMail();
            //} // catch Exception
            //finally
            //{
            //    //close the connection	
            //    if(con.State == ConnectionState.Open) con.Close();
            //}
			
            //return threadId;
		}
		
		public static string SavePost( string customerId, string messageText, int alertType, string threadId )
        {
            throw new Exception("Method not used/commented");

            //string postId = "-1";
						
            //SqlConnection con;
            //SqlCommand cmd;
            //SqlParameter prm;
            //Database db = new Database();
            //CommonOpn op = new CommonOpn();
						
            //string conStr = db.GetConString();
			
            //con = new SqlConnection( conStr );
					
            //try
            //{					
            //    cmd = new SqlCommand("EnterForumMessage", con);
            //    cmd.CommandType = CommandType.StoredProcedure;
			
            //    prm = cmd.Parameters.Add("@ForumId", SqlDbType.BigInt);
            //    prm.Value = threadId;
				
            //    prm = cmd.Parameters.Add("@CustomerId", SqlDbType.BigInt);
            //    prm.Value = customerId;
				
            //    prm = cmd.Parameters.Add("@Message", SqlDbType.VarChar, 4000);
            //    prm.Value = messageText;
								
            //    prm = cmd.Parameters.Add("@MsgDateTime", SqlDbType.DateTime);
            //    prm.Value = DateTime.Now;
				
            //    prm = cmd.Parameters.Add("@AlertType", SqlDbType.Int);
            //    prm.Value = alertType;
				
            //    prm = cmd.Parameters.Add("@PostId", SqlDbType.BigInt);
            //    prm.Direction = ParameterDirection.Output;
																
            //    con.Open();
            //    //run the command
            //    cmd.ExecuteNonQuery();
				
            //    postId = cmd.Parameters[ "@PostId" ].Value.ToString();
            //}
            //catch(Exception err)
            //{
            //    HttpContext.Current.Trace.Warn(err.Message);
            //    ErrorClass objErr = new ErrorClass(err,HttpContext.Current.Request.ServerVariables["URL"]);
            //    objErr.SendMail();
            //} // catch Exception
            //finally
            //{
            //    //close the connection	
            //    if(con.State == ConnectionState.Open)
            //    {
            //        con.Close();
            //    }
            //}
			
            //return postId;
		}
		
		// send mail to the thread starter and all the participants.
		// dont send the mail to the current user.
		public static void NotifySubscribers( string discussionUrl, string threadId, string threadName )
        {
            throw new Exception("Method not used/commented");

            //string sql = "";
            ////string repliername = currentuser.id == "-1" ? "anonymous" : currentuser.name;
            ////code changed by sentil on 29 dec 2009 (2 days to go !)
            //CommonMessage cm = new CommonMessage();
            //string repliername = CurrentUser.Id == "-1" ? "anonymous" : cm.GetHandleName(CurrentUser.Id);
			
            //string url = "http://www.bikewale.com" + discussionUrl;
			
            //Database db = new Database();
			
            //try
            //{
            //    //get the name and the email of the people who have subscribed to this thread
            //    sql = " SELECT C.Name, C.Email FROM Customers AS C With(NoLock) WHERE C.IsFake=0 AND C.ID IN "
            //        + " (SELECT CustomerId FROM ForumSubscriptions With(NoLock) WHERE EmailSubscriptionId=2 AND ForumThreadId = @ForumThreadId) "
            //        + " AND C.Id <> @CustomerId ";
				
            //    SqlCommand cmd = new SqlCommand(sql);

            //    cmd.Parameters.Add("@ForumThreadId", SqlDbType.BigInt).Value = threadId;
            //    cmd.Parameters.Add("@CustomerId", SqlDbType.BigInt).Value = CurrentUser.Id;
				
            //    DataSet ds = db.SelectAdaptQry(cmd);
				
            //    foreach(DataRow dr in ds.Tables[0].Rows)
            //    {
            //        string name = dr["Name"].ToString();
            //        string email = dr["Email"].ToString();
					
            //        HttpContext.Current.Trace.Warn( "sending mail to " + name );
					
            //        //send mail to the thread participants
            //        //if(email.ToLower() != CurrentUser.Email.ToLower() )
            //        //    Mails.NotifyForumSubscribedUsers( email, name, replierName, threadName, url);
            //    }
				
            //    //Mails.NotifyForumSubscribedUsers( "rajeev@carwale.com", "Rajeev", replierName, threadName, url);
				
            //}
            //catch(Exception err)
            //{
            //    HttpContext.Current.Trace.Warn(err.Message);
            //    ErrorClass objErr = new ErrorClass(err,HttpContext.Current.Request.ServerVariables["URL"]);
            //    objErr.SendMail();
            //}
		}
		
		// This function will manage when a user was last logged into Forums.
		public static void ManageLastLogin()
        {
            throw new Exception("Method not used/commented");

            //if ( CurrentUser.Id != "-1" ) // user is logged in
            //{
            //    // cookie is not available.
            //    if ( HttpContext.Current.Request.Cookies["ForumLastLogin"] == null )
            //    {
            //        SqlDataReader reader = null;
            //        Database db = null;

            //        // try fetching last login from database.
            //        try
            //        {
            //            string sql = "SELECT ForumLastLogin FROM UserProfile With(NoLock) WHERE UserId=" + CurrentUser.Id;
                        
            //            DateTime lastLogin = new DateTime();

            //            reader = db.SelectQry(sql);
            //            db = new Database();

            //            // if found in db
            //            if (reader.Read())
            //            {
            //                if (reader["ForumLastLogin"].ToString() != "")
            //                    lastLogin = Convert.ToDateTime(reader["ForumLastLogin"]);
            //            }

            //            // in case not available in db, keep last visit as two days ago.
            //            if (lastLogin.Year < 2000)
            //            {
            //                lastLogin = DateTime.Now.AddDays(-2);
            //            }

            //            // set last visit cookie.
            //            HttpCookie lastLoginCookie = new HttpCookie("ForumLastLogin");
            //            lastLoginCookie.Value = lastLogin.ToString();
            //            lastLoginCookie.Expires = DateTime.Now.AddHours(2);
            //            HttpContext.Current.Response.Cookies.Add(lastLoginCookie);

            //            // now save current datetime as ForumLastLogin in db
            //            sql = " UPDATE UserProfile SET ForumLastLogin='" + DateTime.Now + "'"
            //                + " WHERE UserId=" + CurrentUser.Id;

            //            db.UpdateQry(sql);
            //        }
            //        catch (Exception err)
            //        {
            //            HttpContext.Current.Trace.Warn(err.Message);
            //            ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
            //            objErr.SendMail();
            //        }
            //        finally
            //        {
            //            reader.Close();
            //            db.CloseConnection();
            //        }
            //    }
            //}
		}
		
		// an overloading function. Seek below this function for the original one.
		public static string GetLastPost(string title, string name, string date, string id, string posts, string startedById)
		{
			return GetLastPost(title, name, date, id, posts, startedById, false);
		}
		
		// will return linked title to thread along with who started it.
		public static string GetLastPost(string title, string name, string date, string id, string posts, string startedById, bool openInNewWindow)
		{
			string retVal = "";
			
			if(title != "")
			{
				string openWindow = "";
				
				if ( openInNewWindow ) // to be opened in new window.
					openWindow = " target='_blank'";
					
				// thread title
				retVal = "<a" + openWindow + " href='viewthread-" + id + ".html'>" + title + "</a>";
               // retVal = "<a" + openWindow + " href=\"ViewThreads.aspx?thread=" + id + "\">" + title + "</a>";
				// if more than 10 posts in a thread, add pages strip
				if ( Convert.ToInt32(posts) > 10 )
				{
					string pageStrip = "";
					string pageUrl = "ViewThread-" + id;
					
					// total pages
					int pages = (int)Math.Ceiling(Convert.ToDouble(posts)/10);
					int i = 0;

                    pageStrip = " (<img alt='Page' title='Go directly to page no' align='absmiddle' src='" + ImagingFunctions.GetRootImagePath() + "/images/forums/multipage.gif' /> ";
					
					i = pages < 5 ? pages : 5;
					
					for ( int j=1; j<=i; j++ )
					{
						if ( j > 1 ) pageStrip += " ";
						pageStrip += "<a href='" + pageUrl + "-p" + j.ToString() + ".html'>" 
												+ j.ToString() + "</a>";
					}
					
					if ( pages > 5 ) 
					{
						pageStrip += " ... ";
						pageStrip += "<a href='" + pageUrl + "-p" + pages + ".html'>Last</a>";
					}	
					
					pageStrip += ")";
					
					retVal += pageStrip;
				}
				
				if(name != "")
					if (name != "anonymous")
						retVal += "<br>by <a target='_blank' title=\"View " + name + "'s complete profile\" class='startBy' href='/community/members/" + name  + ".html'>" + name + "</a>";
					else
						retVal += "<br>by <span class='startBy'>" + name + "</span>";
						
				//if(date != "")
					//retVal += ",  <span class='startBy'>" + Convert.ToDateTime(date).ToString("dd-MMM, yyyy HH:mm") + "</span>";
			}
			else
				retVal = " ";
									
			return retVal;
		}
				
		public static string GetLastPostThread(string name, string date, string lastPostById)
		{
			string retVal = "";
			
			if(name != "")
			{
				if(date != "")
					retVal = Convert.ToDateTime(date).ToString("dd-MMM, yyyy hh:mm tt");
					
				//retVal += "<br>by <a target='_blank' title=\"View " + name + "'s complete profile\" class='startBy' href='/Users/Profile-" + CarwaleSecurity.EncryptUserId( long.Parse( lastPostById ) ) + ".html'>" + name + "</a>";									
				if(name != "")
					if (name != "anonymous")
						retVal += "<br>by <a target='_blank' title=\"View " + name + "'s complete profile\" class='startBy' href='/community/members/" + name  + ".html'>" + name + "</a>";
					else
						retVal += "<br>by <span class='startBy'>" + name + "</span>";

			}
			else
				retVal = " ";
									
			return retVal;
		}
		
		public static bool IsUserBanned(string customerId)
        {
            throw new Exception("Method not used/commented");

            ////return true if the user is in the banned list. else return false
            //string sql = "";
            //Database db = new Database();
			
            //SqlDataReader dr = null;
			
            //bool banned = false;

            //try
            //{
            //    sql = " Select CustomerId From Forum_BannedList With(NoLock) Where CustomerId = @CustomerId ";

            //    SqlCommand cmd = new SqlCommand(sql);

            //    cmd.Parameters.Add("@CustomerId", SqlDbType.BigInt).Value = customerId;

            //    dr = db.SelectQry(cmd);

            //    if (dr.Read())
            //    {
            //        banned = true;
            //    }
            //}
            //catch (Exception err)
            //{
            //    HttpContext.Current.Trace.Warn(err.Message);
            //    ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
            //    objErr.SendMail();
            //}
            //finally
            //{ 
            //    if(dr != null)
            //        dr.Close();

            //    db.CloseConnection();
            //}
			
            //return banned;
		}
		
		//Check whether Handle Exists for the User
		public static bool CheckUserHandle( string userId )
        {
            throw new Exception("Method not used/commented");

           // Database db = new Database();

           // SqlDataReader dr = null;			
			
           // string sql = "";
           // bool result = false;
			
           // if( userId == "")
           //     return result ;

           // sql = " SELECT HandleName, IsUpdated FROM UserProfile With(NoLock) "
           //     + " WHERE UserID = @UserId ";
			
           // HttpContext.Current.Trace.Warn("Check User Handle Query : " + sql);
			
           // try
           // {
           //     SqlCommand cmd = new SqlCommand(sql);

           //     cmd.Parameters.Add("@UserId", SqlDbType.BigInt).Value = userId;

           //     dr  = db.SelectQry(cmd);
			
           //     if(dr.Read())
           //     {
           //         if( dr["HandleName"].ToString()== "" || dr["IsUpdated"].ToString() == "False")
           //             result = true;
           //     }
           //     else
           //     {
           //         result = true;
           //     }
				
           // }
           // catch(Exception err)
           // {
           //     HttpContext.Current.Trace.Warn("Check User Handle Query : " + sql);				
           //     ErrorClass objErr = new ErrorClass(err, "ForumsCommon.CheckUserHandle()");
           //     objErr.SendMail();
           // }
           // finally
           // {
           //     if(dr != null)
           //         dr.Close();

           //     db.CloseConnection();
           // }

           //return result;
		}
		
		
		public bool GetModeratorLoginStatus( string customerId )
        {
            throw new Exception("Method not used/commented");

            //bool modLogin = false;
            //Database db = new Database();
            //SqlDataReader dr = null;
			
            //string sql = "";
			
            //sql = " SELECT Role "
            //    + " FROM BikeWaleRoles CR, ForumCustomerRoles FCR With(NoLock) "
            //    + " WHERE FCR.RoleId=CR.ID AND CustomerId= @CustomerId ";

            //try
            //{
            //    SqlCommand cmd = new SqlCommand(sql);

            //    cmd.Parameters.Add("@CustomerId", SqlDbType.BigInt).Value = customerId;

            //    HttpContext.Current.Trace.Warn("GetModeratorLoginStatus : " + sql);

            //    dr = db.SelectQry(cmd);

            //    if (dr.Read())
            //    {
            //        if (dr["Role"].ToString().ToLower() == "moderator")
            //        {
            //            modLogin = true;
            //        }
            //    }
            //    HttpContext.Current.Trace.Warn("GetModeratorLoginStatus : " + modLogin.ToString());

            //    db.CloseConnection();
            //}
            //catch (SqlException err)
            //{
            //    ErrorClass objErr = new ErrorClass(err, "ForumsCommon.GetModeratorLoginStatus()");
            //    objErr.SendMail();

            //    modLogin = false;
            //} // catch Exception
            //finally
            //{ 
            //    if(dr != null)
            //        dr.Close();
            //}
			
            //return modLogin;
		}
		
	} // class
} // namespace
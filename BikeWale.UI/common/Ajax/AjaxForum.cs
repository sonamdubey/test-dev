using System;
using System.Text;
using System.Data;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using Bikewale.Common;
using System.Collections;
using AjaxPro;


namespace Bikewale.Ajax
{
	public class AjaxForum
	{
		// write all the Forum Ajax functions here
		// used for writing the debug messages
		private HttpContext objTrace = HttpContext.Current;
		
		[AjaxPro.AjaxMethod()]		
		public bool AddSubscription( string subId, string customerId, int alertType )
		{
			bool returnVal = false;
			
			SqlConnection con;
			SqlCommand cmd;
			SqlParameter prm;
			Database db = new Database();
			CommonOpn op = new CommonOpn();
						
			string conStr = db.GetConString();
			
			con = new SqlConnection( conStr );

            try
            {
                cmd = new SqlCommand("Forum_InsertSubscription", con);
                cmd.CommandType = CommandType.StoredProcedure;

                prm = cmd.Parameters.Add("@customerId", SqlDbType.BigInt);
                prm.Value = customerId;

                prm = cmd.Parameters.Add("@subId", SqlDbType.BigInt);
                prm.Value = subId;

                prm = cmd.Parameters.Add("@alertType", SqlDbType.Int);
                prm.Value = alertType;

                con.Open();
                //run the command
                cmd.ExecuteNonQuery();

                returnVal = true;
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, objTrace.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }
			return returnVal;
		}
		
		[AjaxPro.AjaxMethod()]		
		public bool GetSubscribeLink( string subId, string customerId )
		{
			bool returnVal = false;
			
			Database db = new Database();
			CommonOpn op = new CommonOpn();
						
			SqlDataReader dr = null;
			
			string sql = "";
			
			sql = " SELECT CustomerId FROM ForumSubscriptions With(NoLock) WHERE CustomerId= @customerId AND ForumThreadId = @subId";
			SqlParameter [] param ={new SqlParameter("@customerId", customerId), new SqlParameter("@subId", subId)};
			try
			{
				dr = db.SelectQry(sql, param);
				if(dr.HasRows == true)
				{
					returnVal = true;
				}
				
			}
			catch(Exception err)
			{
				ErrorClass objErr = new ErrorClass(err, objTrace.Request.ServerVariables["URL"]);
				objErr.SendMail();
			}
			finally
			{
				
				db.CloseConnection();

                if(dr != null)
                    dr.Close();
			}
			return returnVal;
		}
		
		[AjaxPro.AjaxMethod()]
		public string GetTitle(string ThreadId)
		{
			if(ThreadId == "" || CommonOpn.CheckId(ThreadId) == false)
				return "";
			
			Database db = new Database();
			SqlDataReader dr = null;
			
			string sql = "";
			string retVal = "";
				
			sql = " SELECT Topic, ForumSubCategoryId CategoryId"
					+ " FROM Forums With(NoLock) "
					+ " WHERE Id = @ThreadId";
					
			SqlParameter [] param ={new SqlParameter("@ThreadId", ThreadId)};
			try
			{
				dr = db.SelectQry(sql, param);
				if(dr.Read())
				{
					retVal = dr["Topic"].ToString();
				}
				
			}
			catch(Exception err)
			{
				ErrorClass objErr = new ErrorClass(err, objTrace.Request.ServerVariables["URL"]);
				objErr.SendMail();
			}
			finally
			{
				
				db.CloseConnection();

                if(dr != null)
                    dr.Close();
			}
			return retVal;
		}
		
		[AjaxPro.AjaxMethod()]		
		public bool ForumInsertReportAbuse( string subId, string customerId, string postId, string comment )
		{
			bool returnVal = false;
			SqlConnection con;
			SqlCommand cmd;
			SqlParameter prm;
			Database db = new Database();
			CommonOpn op = new CommonOpn();
						
			string conStr = db.GetConString();
			
			con = new SqlConnection( conStr );

            try
            {
                cmd = new SqlCommand("Forum_InsertReportAbuse", con);
                cmd.CommandType = CommandType.StoredProcedure;

                prm = cmd.Parameters.Add("@subId", SqlDbType.BigInt);
                prm.Value = subId;

                prm = cmd.Parameters.Add("@postId", SqlDbType.BigInt);
                prm.Value = postId;

                prm = cmd.Parameters.Add("@customerId", SqlDbType.BigInt);
                prm.Value = customerId;

                prm = cmd.Parameters.Add("@comment", SqlDbType.VarChar, 100);
                prm.Value = comment;

                prm = cmd.Parameters.Add("@createDate", SqlDbType.DateTime);
                prm.Value = DateTime.Now;

                con.Open();
                //run the command
                cmd.ExecuteNonQuery();

                returnVal = true;
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, objTrace.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }
			return returnVal;
		}
		
		//Code Added By Sentil On 20 OCT 2009
		//Changes Done on 19 Nov 2009
		[AjaxPro.AjaxMethod()]
		//Used to Check the existance of Handle Name 
		public bool GetHandleName(string handleName)
		{
			Database db = new Database();
			
			SqlConnection con;
			SqlCommand cmd;
			SqlParameter prm;
			
			string retHandleName  = "";
			bool result = false;
			
			if(handleName == "")
				return result ;	

			string conStr = db.GetConString();
			con = new SqlConnection( conStr );
			
			cmd = new SqlCommand("Forum_HandleCheck", con);
			cmd.CommandType = CommandType.StoredProcedure;

			prm = cmd.Parameters.Add("@UserName", SqlDbType.VarChar, 500);
			prm.Value = handleName;
			HttpContext.Current.Trace.Warn("UserName = " + prm.Value);	
			
			prm = cmd.Parameters.Add("@UserID", SqlDbType.BigInt);
			prm.Value = CurrentUser.Id;
			HttpContext.Current.Trace.Warn("UserId = " + prm.Value);		
			
			prm = cmd.Parameters.Add("@HandleName", SqlDbType.VarChar, 500);
			prm.Direction = ParameterDirection.Output;
	
	
			try
			{
				con.Open() ;
				cmd.ExecuteNonQuery() ;
				
				retHandleName = cmd.Parameters["@HandleName"].Value.ToString();
				
				if(retHandleName != "")
				{
					result = true;
				}	
				
				HttpContext.Current.Trace.Warn("Return HandleName : " + retHandleName );
				
			}
			catch(Exception ex)
			{
				HttpContext.Current.Trace.Warn(ex.Message + ex.Source);				
				ErrorClass objErr = new ErrorClass(ex,HttpContext.Current.Request.ServerVariables["URL"]);
				objErr.SendMail();
			}
			finally
			{
				if(con.State == ConnectionState.Open)
				{
					con.Close();
				}
			}
			
			return result;
		}

		[AjaxPro.AjaxMethod()]
		//Used to Check the existance of Handle Name 
		public bool GetPMHandleName(string handleName)
		{
			Database db = new Database();
			
			SqlConnection con;
			SqlCommand cmd;
			SqlParameter prm;
			
			string retHandleName  = "";
			bool result = false;
			
			if(handleName == "")
				return result ;	

			string conStr = db.GetConString();
			con = new SqlConnection( conStr );
			
			cmd = new SqlCommand("PM_HandleCheck", con);
			cmd.CommandType = CommandType.StoredProcedure;

			prm = cmd.Parameters.Add("@UserName", SqlDbType.VarChar, 500);
			prm.Value = handleName;
			HttpContext.Current.Trace.Warn("UserName = " + prm.Value);	
			
			prm = cmd.Parameters.Add("@UserID", SqlDbType.BigInt);
			prm.Value = CurrentUser.Id;
			HttpContext.Current.Trace.Warn("UserId = " + prm.Value);		
			
			prm = cmd.Parameters.Add("@HandleName", SqlDbType.VarChar, 500);
			prm.Direction = ParameterDirection.Output;
	
	
			try
			{
				con.Open() ;
				cmd.ExecuteNonQuery() ;
				
				retHandleName = cmd.Parameters["@HandleName"].Value.ToString();
				
				if(retHandleName != "")
				{
					result = true;
				}	
				
				HttpContext.Current.Trace.Warn("Return HandleName : " + retHandleName );
				
			}
			catch(Exception ex)
			{
				HttpContext.Current.Trace.Warn(ex.Message + ex.Source);				
				ErrorClass objErr = new ErrorClass(ex,HttpContext.Current.Request.ServerVariables["URL"]);
				objErr.SendMail();
			}
			finally
			{
				if(con.State == ConnectionState.Open)
				{
					con.Close();
				}
			}
			
			return result;
		}

		[AjaxPro.AjaxMethod()]
		public string GetPMAutoComplete(string handleName)
		{
			Database db = new Database();
			DataSet ds = new DataSet();

			if(handleName != "")
			{
				//string sql = "SELECT TOP 10 HandleName FROM UserProfile WHERE HandleName like '" + handleName  + "%'";	
				string sql = "SELECT TOP 10 HandleName FROM UserProfile With(NoLock) WHERE REPLACE( HandleName, '.', '') like  REPLACE(@handleName,'.','')";					
				
				SqlParameter [] param ={new SqlParameter("@handleName", "" + handleName + "%")};
				
				HttpContext.Current.Trace.Warn("SQL AutoComplete : " + sql );
	
				try
				{
					ds = db.SelectAdaptQry(sql, param);
				}
				catch(Exception ex)
				{
					HttpContext.Current.Trace.Warn(ex.Message + ex.Source);				
					ErrorClass objErr = new ErrorClass(ex,HttpContext.Current.Request.ServerVariables["URL"]);
					objErr.SendMail();
				}
			}	
			
			if( ds.Tables[0].Rows.Count > 0 )
				return JSON.GetJSONString( ds.Tables[0] );
			else
				return "";				
		}
		
		[AjaxPro.AjaxMethod()]		
		public string GetOnlineForumUsers(string postedByIds)
		{
			string forumUsers = "";
			SqlCommand cmd = new SqlCommand();
			
			SqlDataReader dr = null;
			Database db = new Database();
			try
			{
				string sql  = " SELECT CONVERT(VARCHAR, UserID) + ','"
						+ " FROM ForumUserTracking With(NoLock) "
						+ " WHERE "
						+ " DATEDIFF(MINUTE, ActivityDateTime, getdate()) < 60 AND UserID IN ("+ db.GetInClauseValue(postedByIds, "PBId", cmd)  +")"
						+ " FOR XML PATH('')";
						
				cmd.CommandText = sql;
				dr = db.SelectQry(cmd);
				
				if (dr.Read())
					forumUsers = dr[0].ToString();
				else
					forumUsers = "";
			}
			catch(Exception ex)
			{
				forumUsers = "";
				ErrorClass objErr = new ErrorClass(ex,HttpContext.Current.Request.ServerVariables["URL"]);
				objErr.SendMail();
			}
			finally
			{
                if(dr != null)
				    dr.Close();

				db.CloseConnection();
			}		
			return forumUsers;	
		}
		
		[AjaxPro.AjaxMethod()]		
		public string ThankThePost(string postId)
		{
			string retVal = "";
			string customerId = "-1", handleExists = "-1", isSaved = "-1";
			try
			{
				customerId = CurrentUser.Id.ToString();
				
				if (customerId != "-1")
				{
					if (DoesHandleExists(customerId))
					{
						handleExists = "1";
						isSaved = SaveToPostThanks(customerId, postId);
					}
				}
				
				//customer id : -1 when not logged in and is present when logged in
				//handleExists : -1 when not present and 1 when present
				//isSaved : -1 when error occured, 0 when already thanked, 1 when newly thanked
				retVal = customerId + "|" + handleExists + "|" + isSaved;
			}
			catch(Exception ex)
			{
				retVal = "";
				ErrorClass objErr = new ErrorClass(ex,HttpContext.Current.Request.ServerVariables["URL"]);
				objErr.SendMail();
			}
			return retVal;
		}
		
		private bool DoesHandleExists(string cId)
		{
			bool handleExists = false;
			string sql = "SELECT ID FROM USERPROFILE With(NoLock) WHERE USERID = " + cId;
			SqlDataReader dr = null;
			Database db = new Database();
			try
			{
				dr = db.SelectQry(sql);
				if (dr.Read())
					handleExists = true;
			}
			catch(Exception ex)
			{
				handleExists = false;
				ErrorClass objErr = new ErrorClass(ex,HttpContext.Current.Request.ServerVariables["URL"]);
				objErr.SendMail();
			}
			finally
			{
                if(dr != null)
				    dr.Close();

				db.CloseConnection();
			}
			return handleExists;
		}
		
		private string SaveToPostThanks(string _customerId, string _postId)
		{
			Database db = new Database();
			
			SqlConnection con;
			SqlCommand cmd;
			SqlParameter prm;
			
			string isSaved  = "-1";
			string conStr = db.GetConString();
			con = new SqlConnection( conStr );
			
			cmd = new SqlCommand("PostThanksSave", con);
			cmd.CommandType = CommandType.StoredProcedure;

			prm = cmd.Parameters.Add("@CustomerID", SqlDbType.BigInt);
			prm.Value = _customerId;
				
			
			prm = cmd.Parameters.Add("@PostID", SqlDbType.BigInt);
			prm.Value = _postId;
				
			
			prm = cmd.Parameters.Add("@IsSaved", SqlDbType.Bit);
			prm.Direction = ParameterDirection.Output;
	
	
			try
			{
				con.Open() ;
				cmd.ExecuteNonQuery() ;
				
				if (Convert.ToBoolean(cmd.Parameters["@IsSaved"].Value))
					isSaved = "1";
				else
					isSaved = "0";		
			}
			catch(Exception ex)
			{
				isSaved = "-1";
				ErrorClass objErr = new ErrorClass(ex,HttpContext.Current.Request.ServerVariables["URL"]);
				objErr.SendMail();
			}
			finally
			{
				if(con.State == ConnectionState.Open)
				{
					con.Close();
				}
			}
			
			return isSaved;	
		}
		
		[AjaxPro.AjaxMethod()]		
		public string GetPostThanksCount(string postIds)
		{
			string retVal = "";
			
            string sql = "SELECT CONVERT(VARCHAR,PostId) + ',' + CONVERT(VARCHAR,COUNT(*)) AS PostCount FROM PostThanks With(NoLock) GROUP BY PostId HAVING PostId IN (" + postIds + ")";
			
            SqlDataReader dr = null;
			Database db = new Database();
			
            try
			{
				dr = db.SelectQry(sql);
				while (dr.Read())
				{
					if (retVal == "")
					{
						retVal = dr["PostCount"].ToString();
					}		
					else
					{
						retVal = retVal + "," + dr["PostCount"].ToString();	
					}
				}
			}
			catch(Exception ex)
			{
				retVal = ex.Message;
				ErrorClass objErr = new ErrorClass(ex,HttpContext.Current.Request.ServerVariables["URL"]);
				objErr.SendMail();
			}
			finally
			{
                if(dr != null)
				    dr.Close();

				db.CloseConnection();
			}
			
			return retVal;
		}
	}
}
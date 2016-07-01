using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Bikewale.Common;

namespace Bikewale.Forums.Common
{
	public class UserTracking
	{	
		public string ForumUserTrackingCookie
		{
			get
			{
				string val = "";	//default false
			
				if(HttpContext.Current.Request.Cookies["ForumUserTrackingCookie"] != null &&
					HttpContext.Current.Request.Cookies["ForumUserTrackingCookie"].Value.ToString() != "")
				{
					val = HttpContext.Current.Request.Cookies["ForumUserTrackingCookie"].Value.ToString();
				}	
				else
				{
					val = "-1";	
				}
				
				return val;
			}
			set
			{
				HttpCookie objCookie;
				objCookie = new HttpCookie("ForumUserTrackingCookie");
				objCookie.Value = value;
				HttpContext.Current.Response.Cookies.Add(objCookie);
			}
		}
		
		public void SaveActivity(string _UserID, string _ActivityId, string _CategoryId, string _ThreadId)
		{
            CommonOpn cwc = new CommonOpn();
            if (cwc.IsSearchEngine() == true)
                return;
                 
                if (ForumUserTrackingCookie != _UserID + "," + _ActivityId + "," + _CategoryId + "," + _ThreadId)
                {
                    ForumUserTrackingCookie = _UserID + "," + _ActivityId + "," + _CategoryId + "," + _ThreadId;

                    if (System.Web.HttpContext.Current.Request.Cookies["ASP.NET_SessionId"] != null)
                    {
                        SqlConnection con;
                        SqlCommand cmd;
                        SqlParameter prm;
                        Database db = new Database();

                        string conStr = db.GetConString();
                        con = new SqlConnection(conStr);

                        try
                        {
                            cmd = new SqlCommand("ForumUserTrackingSave", con);
                            cmd.CommandType = CommandType.StoredProcedure;

                            prm = cmd.Parameters.Add("@SessionID", SqlDbType.VarChar, 100);

                            //prm.Value = System.Web.HttpContext.Current.Request.Cookies["ASP.NET_SessionId"] != null ? System.Web.HttpContext.Current.Request.Cookies["ASP.NET_SessionId"].Value : string.Empty;

                            prm.Value = HttpContext.Current.Request.Cookies["ASP.NET_SessionId"].Value.ToString();


                            prm = cmd.Parameters.Add("@UserID", SqlDbType.BigInt);
                            prm.Value = _UserID;

                            prm = cmd.Parameters.Add("@ActivityId", SqlDbType.BigInt);
                            prm.Value = _ActivityId;

                            prm = cmd.Parameters.Add("@CategoryId", SqlDbType.BigInt);
                            prm.Value = _CategoryId;

                            prm = cmd.Parameters.Add("@ThreadId", SqlDbType.BigInt);
                            prm.Value = _ThreadId;
                            Bikewale.Notifications.LogLiveSps.LogSpInGrayLog(cmd);
                            con.Open();

                            cmd.ExecuteNonQuery();
                        }
                        catch (SqlException err)
                        {
                            //HttpContext.Current.Trace.Warn("SaveForumActivity : " + err.Message);
                            ErrorClass objErr = new ErrorClass(err, "SaveForumActivity");
                            objErr.SendMail();
                        }
                        catch (Exception err)
                        {
                            //HttpContext.Current.Trace.Warn("SaveForumActivity : " + err.Message);
                            ErrorClass objErr = new ErrorClass(err, "SaveForumActivity");
                            objErr.SendMail();
                        }
                        finally
                        {
                            if (con.State == ConnectionState.Open)
                            {
                                con.Close();
                            }
                        }
                }
			}
		}
		
		public string GetUsersCount()
		{
			SqlConnection con;
			SqlCommand cmd;
			SqlParameter prm;
			Database db = new Database();

			string conStr = db.GetConString();
			con = new SqlConnection( conStr );
			
			try
			{			
				cmd = new SqlCommand("ManageForumUserCount", con);
				cmd.CommandType = CommandType.StoredProcedure;
				
				prm = cmd.Parameters.Add("@Result", SqlDbType.VarChar, 100);
				prm.Direction = ParameterDirection.Output;
                Bikewale.Notifications.LogLiveSps.LogSpInGrayLog(cmd);
				con.Open();
				
				cmd.ExecuteNonQuery();
			
				return cmd.Parameters[ "@Result" ].Value.ToString();
			}
			catch(Exception ex)
			{
				ErrorClass objErr = new ErrorClass(ex, "GetUsersCount");
				objErr.SendMail();
				return "";
			}
			finally
			{
				if(con.State == ConnectionState.Open)
				{
					con.Close();
				}
			}
		}
		
		public DataSet LoadCategoryViews()
		{
			DataSet ds = new DataSet();
			Database db = new Database();
			string sql  = " SELECT	CategoryId, COUNT(SessionID) AS NoOfViews "
						+ " FROM	ForumUserTracking"
						+ " WHERE	ActivityId IN (2,3,4,5) AND CategoryId <> -1 AND DATEDIFF(MINUTE, ActivityDateTime, getdate()) < 60"
						+ " GROUP BY CategoryId";
			try
			{
				ds = db.SelectAdaptQry(sql);
			}
			catch(Exception ex)
			{
				ds = null;
				ErrorClass objErr = new ErrorClass(ex, "LoadCategoryViews");
				objErr.SendMail();
			}
			return ds;
		}
		
		public DataSet LoadThreadViews(string threadIds)
		{
			DataSet ds = new DataSet();
			Database db = new Database();
			string sql  = " SELECT	ThreadId, COUNT(SessionID) AS NoOfViews "
						+ " FROM	ForumUserTracking"
						+ " WHERE	ActivityId IN (4,5) AND ThreadId IN (" + threadIds + ") AND DATEDIFF(MINUTE, ActivityDateTime, getdate()) < 60"
						+ " GROUP BY ThreadId";
			try
			{
				ds = db.SelectAdaptQry(sql);
			}
			catch(Exception ex)
			{
				ds = null;
				ErrorClass objErr = new ErrorClass(ex, "LoadThreadViews");
				objErr.SendMail();
			}
			return ds;
		}
		
	}
}	
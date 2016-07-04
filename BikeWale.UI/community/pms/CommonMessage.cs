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

namespace Bikewale.Community.PMS
{
	public class CommonMessage
	{
		protected string conStr = "", retId = "", sql = "";
		
		public string ComposeMessage(string convId, string receiverId, string subject, string message, string isDraft, string status)
		{
			 throw new Exception("Method not used/commented");
            //SqlConnection con;
            //SqlCommand cmd;
            //SqlParameter prm;
            //Database db = new Database();
            //CommonOpn op = new CommonOpn();
						
            //conStr = db.GetConString();
			
            //con = new SqlConnection( conStr );
			
            //cmd = new SqlCommand("PM_ComposeMessage", con);
            //cmd.CommandType = CommandType.StoredProcedure;
		
            //prm = cmd.Parameters.Add("@ConversationId", SqlDbType.BigInt);
            //prm.Value = convId;

            //prm = cmd.Parameters.Add("@SenderId", SqlDbType.BigInt);
            //prm.Value = CurrentUser.Id;
			
            //prm = cmd.Parameters.Add("@ReceiverId", SqlDbType.BigInt);
            //prm.Value = receiverId != "" ? receiverId : "";
            //HttpContext.Current.Trace.Warn("ReceiverId=" + prm.Value);			
											
            //prm = cmd.Parameters.Add("@Subject", SqlDbType.VarChar, 100);
            //prm.Value = subject != "" ? subject : Convert.DBNull;
            //HttpContext.Current.Trace.Warn("Subject=" + prm.Value);			
							
            //prm = cmd.Parameters.Add("@Message", SqlDbType.VarChar, 2000);
            //prm.Value = message;
            //HttpContext.Current.Trace.Warn("Message=" + prm.Value);			

            //prm = cmd.Parameters.Add("@IsDraft", SqlDbType.Bit);
            //prm.Value = isDraft != "" ? Convert.ToInt32(isDraft) : 0;
            //HttpContext.Current.Trace.Warn("IsDraft=" + prm.Value);			

            //prm = cmd.Parameters.Add("@Status", SqlDbType.VarChar, 5);
            //prm.Value =  status != "" ? status : "";
            //HttpContext.Current.Trace.Warn("Status=" + prm.Value);			
			
            //prm = cmd.Parameters.Add("@CreatedBy", SqlDbType.BigInt);
            //prm.Value = CurrentUser.Id;

            //prm = cmd.Parameters.Add("@CreatedDate", SqlDbType.DateTime);
            //prm.Value = DateTime.Now;
			
            //prm = cmd.Parameters.Add("@RetInboxID", SqlDbType.BigInt);
            //prm.Direction = ParameterDirection.Output;
            Bikewale.Notifications.LogLiveSps.LogSpInGrayLog(cmd);	
            //try
            //{		
            //    con.Open();
            //    //run the command
            //    cmd.ExecuteNonQuery();
				
            //    retId = cmd.Parameters[ "@RetInboxID" ].Value.ToString();
            //    HttpContext.Current.Trace.Warn("retId =" + retId);

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
			
            //return retId;
		}

		public void SaveDrafts(string convId, string subject, string message, string receiverId, string operation)
		{
			throw new Exception("Method not used/commented");
            //SqlConnection con;
            //SqlCommand cmd;
            //SqlParameter prm;
            //Database db = new Database();
            //CommonOpn op = new CommonOpn();
						
            //conStr = db.GetConString();
			
            //con = new SqlConnection( conStr );
			
            //cmd = new SqlCommand("PM_SaveDraft", con);
            //cmd.CommandType = CommandType.StoredProcedure;
		
            //prm = cmd.Parameters.Add("@ConversationId", SqlDbType.BigInt);
            //prm.Value = convId;
            //HttpContext.Current.Trace.Warn("ConversationId=" + prm.Value);	

            //prm = cmd.Parameters.Add("@Subject", SqlDbType.VarChar, 100);
            //prm.Value = subject != "" ? subject : Convert.DBNull;
            //HttpContext.Current.Trace.Warn("Subject=" + prm.Value);			
							
            //prm = cmd.Parameters.Add("@Message", SqlDbType.VarChar, 2000);
            //prm.Value = message != "" ? message : Convert.DBNull;
            //HttpContext.Current.Trace.Warn("Message=" + prm.Value);	

            //prm = cmd.Parameters.Add("@ReceiverId", SqlDbType.BigInt);
            //prm.Value = receiverId != "" ? receiverId : "";
            //HttpContext.Current.Trace.Warn("ReceiverId=" + prm.Value);			

            //prm = cmd.Parameters.Add("@Operation", SqlDbType.Bit);
            //prm.Value = operation != "" ? Convert.ToInt32(operation) : 1;
            //HttpContext.Current.Trace.Warn("Opeartion=" + prm.Value);			

            //prm = cmd.Parameters.Add("@CreatedBy", SqlDbType.BigInt);
            //prm.Value = CurrentUser.Id;
            //HttpContext.Current.Trace.Warn("CreatedBy=" + prm.Value);	

            //prm = cmd.Parameters.Add("@CreatedDate", SqlDbType.DateTime);
            //prm.Value = DateTime.Now;
            Bikewale.Notifications.LogLiveSps.LogSpInGrayLog(cmd);
						
            //try
            //{		
            //    con.Open();
            //    //run the command
            //    cmd.ExecuteNonQuery();
				
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
			
		}

		public string GetHandleId(string handleName)
		{
            throw new Exception("Method not used/commented");

            //Database db =  new Database();

            //sql = "SELECT UserId FROM UserProfile With(NoLock) WHERE REPLACE(HandleName,'.','') = REPLACE(@HandleName,'.','')";

            //HttpContext.Current.Trace.Warn("GetUserId : " + sql);
            //try
            //{
            //    SqlParameter [] param = 
            //    { new SqlParameter("@HandleName",handleName) };
				
            //    retId = db.ExecuteScalar(sql,param);
            //}
            //catch(Exception err)
            //{
            //    HttpContext.Current.Trace.Warn(err.Message);
            //    ErrorClass objErr = new ErrorClass(err,HttpContext.Current.Request.ServerVariables["URL"]);
            //    objErr.SendMail();
            //} 
            //return retId;				
		}	
				
		public string GetUserId(string emailId)
		{
            throw new Exception("Method not used/commented");

            //Database db = new Database();

            //sql = "SELECT id FROM Customers With(NoLock) WHERE email = @Email";
			
            //HttpContext.Current.Trace.Warn("GETData from Customers:" + sql);
            //try
            //{		
            //    SqlParameter [] param = 
            //    { new SqlParameter("@Email",emailId) };
				
            //    retId = db.ExecuteScalar(sql,param);
				
            //    if( retId == " " )
            //        retId = "-1";
            //}
            //catch(Exception err)
            //{
            //    HttpContext.Current.Trace.Warn(err.Message);
            //    ErrorClass objErr = new ErrorClass(err,HttpContext.Current.Request.ServerVariables["URL"]);
            //    objErr.SendMail();
            //} 			
			
            //return retId;
		} 

		//To collect the Selected Ids
		public string CollectSelectedId(Repeater rptSent)
		{
			string id = "";
			
			for(int i = 0; i < rptSent.Items.Count; i++)
			{
				//find controls inside the repeater
				HtmlInputHidden  hdnId  	= (HtmlInputHidden) rptSent.Items[i].FindControl("hdnRptId");
				CheckBox chkSelected	  	= (CheckBox) rptSent.Items[i].FindControl("chkA");

				if(chkSelected.Checked)
				{
					id += hdnId.Value + ",";
				}
 			}	
			HttpContext.Current.Trace.Warn("ID Value : " + id);	
			return id;
		}			

		public void UpdateTable(string tableName, string columnName, string value, string selectedId)
		{
           
            throw new Exception("Method not used/commented");
            
            //Database db =  new Database();
            //SqlParameter[] param1 = null;
			
            //sql = "UPDATE " + tableName +" SET " + columnName +" = @Value WHERE "
            //    + " MessageId IN (SELECT ID FROM PM_Messages With(NoLock) WHERE ConversationId IN (" + db.GetInClauseValue(selectedId, "ConversationId", out param1) + ") )";

            //HttpContext.Current.Trace.Warn("InActive Inbox : " + sql);
            //try
            //{
            //    SqlParameter [] param2 = 
            //    { new SqlParameter("@Value",value) };
					
            //    SqlParameter [] param = db.ConcatenateParams(param1, param2);
				
            //    db.UpdateQry(sql,param);
            //}
            //catch(Exception err)
            //{
            //    HttpContext.Current.Trace.Warn(err.Message);
            //    ErrorClass objErr = new ErrorClass(err,HttpContext.Current.Request.ServerVariables["URL"]);
            //    objErr.SendMail();
            //} 			
			
		}

		public void GeneralProcess(string tableName, string columnName, string value, string userId, string selectedId)
		{
            throw new Exception("Method not used/commented");

            //Database db =  new Database();
			
            //SqlParameter[] param1 = null;
			
            //sql = "UPDATE " + tableName + " SET " + columnName + " = @ColValue WHERE "
            //    + " ConversationId IN (" + db.GetInClauseValue(selectedId, "ConversationId",out param1) + ") AND UserId = @UserId";

            //HttpContext.Current.Trace.Warn("InActive Inbox : " + sql);
            //try
            //{
            //    SqlParameter [] param2 = 
            //    { new SqlParameter("@ColValue",value),new SqlParameter("@UserId",userId) };
				
            //    SqlParameter [] param = db.ConcatenateParams(param1, param2);

            //    db.UpdateQry(sql,param);
            //}
            //catch(Exception err)
            //{
            //    HttpContext.Current.Trace.Warn(err.Message);
            //    ErrorClass objErr = new ErrorClass(err,HttpContext.Current.Request.ServerVariables["URL"]);
            //    objErr.SendMail();
            //} 			
			
		}
/*******************************************************************************************************
		Used to Check the user belongs to the conversation
*******************************************************************************************************/
		public string GetPMCheckUser(string conv,string username)
		{
            throw new Exception("Method not used/commented");

            //Database db =  new Database();

            //sql = "SELECT ID FROM PM_ConversationDetails With(NoLock) WHERE ConversationId = @ConversationId AND UserId = @UserId";
			
            //HttpContext.Current.Trace.Warn("SQL CheckUser : " + sql );
			
            //try
            //{
            //    //passing parameters
            //    SqlParameter[] param = 
            //    { 	
            //        new SqlParameter("@UserId", username),
            //        new SqlParameter("@ConversationId", conv)
            //    };
								
            //    retId = db.ExecuteScalar(sql,param);
            //}
            //catch(Exception ex)
            //{
            //    HttpContext.Current.Trace.Warn(ex.Message + ex.Source);				
            //    ErrorClass objErr = new ErrorClass(ex,HttpContext.Current.Request.ServerVariables["URL"]);
            //    objErr.SendMail();
            //}
            //return retId;
		}	
/*******************************************************************************************************
		Used to get the count the number of posts
*******************************************************************************************************/
		public string GetPMCheckPost(string userid)          

		{
            throw new Exception("Method not used/commented");


            //Database db =  new Database();

            //sql = "SELECT COUNT(FT1.ID) AS Posts FROM ForumThreads FT1, Forums F With(NoLock) WHERE F.Id=FT1.ForumId " 
            //    + " AND FT1.IsActive=1 AND F.IsActive=1 AND FT1.CustomerId = @UserId";
			
            //HttpContext.Current.Trace.Warn("SQL Number of Posts : " + sql );
			
            //try
            //{
            //    //passing parameters
            //    SqlParameter[] param = 
            //    { 	new SqlParameter("@UserId", userid) };
				
            //    retId = db.ExecuteScalar(sql,param);
            //}
            //catch(Exception ex)
            //{
            //    HttpContext.Current.Trace.Warn(ex.Message + ex.Source);				
            //    ErrorClass objErr = new ErrorClass(ex,HttpContext.Current.Request.ServerVariables["URL"]);
            //    objErr.SendMail();
            //}
            //return retId;
		}

/*******************************************************************************************************
		To send mail to specified user
*******************************************************************************************************/
		public void sendEmail(string receiverId, string subject)
		{
			try
			{
				if( receiverId != "" && receiverId != "-1" )
				{
					string fromMail = GetUserName(CurrentUser.Id);
	
					//get the to mail id
					string toMail = GetEmail(receiverId);
					
					string toUserName = GetUserName(receiverId);
					
					//prepare the subject
					string subj = fromMail + " of CarWale Community has sent you a private message!"; 
					
					string mesSub = "NoSubject";
	
					if(subject != "")
						mesSub  = subject.Replace("'", "''").Trim();
					
					//prepare the body
					StringBuilder sb = new StringBuilder();
	
					sb.Append("<p>Dear " + toUserName + ",</p>");							
					
					sb.Append("<p>" + fromMail + " of CarWale Community has sent you a private message, entitled '"+ mesSub +"'.</p>");
					sb.Append("<p>To read this message, you will have to go to your private messaging inbox at CarWale. </p>");
					sb.Append("Your private messaging inbox can be accessed by clicking the following link:<br />");
	
					sb.Append("<a href='http://www.carwale.com/community/pms/'>http://www.carwale.com/community/pms/</a><br />"); 
	
					sb.Append("<p>Note: It's an automated email, please DO NOT reply to it.</p>"); 
					sb.Append("Warm regards,<br />");
					sb.Append("--<br />");
					sb.Append("CarWale Community Mods Team<br />");
					sb.Append("Email: community@carwale.com<br />"); 
					
					//Trace.Warn(sb.ToString()+":fromMail:"+fromMail+":toMail:"+toMail+":subj:"+subj);
					
					CommonOpn op = new CommonOpn();
					op.SendMail( toMail, subj, sb.ToString(), true );
				}
			}
			catch(Exception ex)
			{
				HttpContext.Current.Trace.Warn("Common:Error:SendMail: " + ex.Message);
			}
		}
/*******************************************************************************************************
		To Get MessageCount from Current User 
*******************************************************************************************************/
		public string GetUnreadMessageCount()
		{
            throw new Exception("Method not used/commented");

            //Database db = new Database();
            //DataSet ds  = new DataSet();
			
            //string sql = "SELECT DISTINCT(PMM.ConversationId) AS Unread"
            //           + " FROM	PM_INBOX PMI With(NoLock) "
            //           + " LEFT JOIN PM_Messages PMM With(NoLock) ON PMM.Id = PMI.MessageId"
            //           + " LEFT JOIN PM_ConversationDetails PMD With(NoLock) ON PMD.ConversationId = PMM.ConversationId"
            //           + " LEFT JOIN PM_Conversations PMC With(NoLock) ON  PMC.Id = PMD.ConversationId "
            //           + " WHERE PMD.IsActive = 1 AND PMI.ReceiverId = @UserId AND "
            //           + " PMD.UserId = @UserId AND PMC.IsDraft = 0 AND PMD.IsRead = 0";
			
            //HttpContext.Current.Trace.Warn("GetUnreadMessageCount" + sql);
            //try
            //{
            //    //passing parameters
            //    SqlParameter[] param = 
            //    { 	new SqlParameter("@UserId", CurrentUser.Id) };
				
            //    ds = db.SelectAdaptQry(sql,param);

            //    if(ds.Tables[0].Rows.Count > 0)
            //    {
            //        retId = "(" + ds.Tables[0].Rows.Count.ToString() + ")";
            //    }			
				
            //}
            //catch(Exception ex)
            //{
            //    HttpContext.Current.Trace.Warn(ex.Message + ex.Source);
            //    ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
            //    objErr.SendMail();
            //}	
            //return retId;	
		}
/*******************************************************************************************************
		To Get Email-Id from userId
*******************************************************************************************************/
		public string GetEmail(string userId)
		{
            throw new Exception("Method not used/commented");

            //Database db = new Database();
            //string email = "";

            //sql = "SELECT email FROM Customers With(NoLock) WHERE ID = @UserId";

            ////Trace.Warn("GetEmail:" + sql);
            //try
            //{
            //    //passing parameters
            //    SqlParameter[] param = 
            //    { 	new SqlParameter("@UserId", userId) };
				
            //    email = db.ExecuteScalar(sql,param);
            //}
            //catch(Exception ex)
            //{
            //    HttpContext.Current.Trace.Warn(ex.Message + ex.Source);
            //    ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
            //    objErr.SendMail();
            //}		
		
            //return email;			
		}
/*******************************************************************************************************
		To Get UserName from userId
*******************************************************************************************************/
		public string GetUserName(string userId)
		{
            throw new Exception("Method not used/commented");

            //Database db = new Database();
            //string userName = "";

            //sql = "SELECT name FROM Customers With(NoLock) WHERE ID = @UserId";
			
            ////Trace.Warn("GetUserName:" + sql);
            //try
            //{
            //    //passing parameters
            //    SqlParameter[] param = 
            //    { 	new SqlParameter("@UserId", userId) };
				
            //    userName = db.ExecuteScalar(sql,param);
				
            //}
            //catch(Exception ex)
            //{
            //    HttpContext.Current.Trace.Warn(ex.Message + ex.Source);
            //    ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
            //    objErr.SendMail();
            //}		
		
            //return userName;			
		}		

/*******************************************************************************************************
		To Get Handle-Name from userid
*******************************************************************************************************/
		public string GetHandleName(string userId)
		{
            throw new Exception("Method not used/commented");

            //Database db = new Database();
            //string userName = "";

            //sql = "SELECT ISNULL(HandleName, 'anonymous') AS HandleName FROM UserProfile With(NoLock) WHERE UserId = @UserId";
			
            //try
            //{
            //    //passing parameters
            //    SqlParameter[] param = 
            //    { 	new SqlParameter("@UserId", userId) };
			
            //    userName = db.ExecuteScalar(sql,param);
				
            //}
            //catch(Exception ex)
            //{
            //    HttpContext.Current.Trace.Warn(ex.Message + ex.Source);
            //    ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
            //    objErr.SendMail();
            //}		
		
            //return userName;			
		}														
	}
}	
		
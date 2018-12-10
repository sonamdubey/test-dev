using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using Carwale.Notifications;
using Carwale.Interfaces.Forums;
using Carwale.DAL.CoreDAL;
using System.Data.Common;
using Carwale.DAL.CoreDAL.MySql;
/// <summary>
/// Summary description for NotificationsDAL
/// </summary>
/// 
namespace Carwale.DAL.Forums
{
    public class NotificationsDAL : INotification
    {
       
        #region Send Mails To Users
        /// <summary>
        /// Get people who have subscribed to a thread.
        /// </summary>
        /// <param name="discussionUrl"></param>
        /// <param name="threadId"></param>
        /// <param name="threadName"></param>
      public DataSet GetSubscribers(string discussionUrl, string threadId, string threadName, string handleName, string customerId, string eMail)
      {
        DataSet ds = new DataSet();
        try
        {
            using(DbCommand cmd = DbFactory.GetDBCommand("Forums_Notifications_v16_11_7"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(DbFactory.GetDbParam("v_ForumThreadId", DbType.Int64, Convert.ToInt32(threadId)));
                cmd.Parameters.Add(DbFactory.GetDbParam("v_CustomerId", DbType.Int64, customerId));
                ds = MySqlDatabase.SelectAdapterQuery(cmd, DbConnections.ForumsMySqlReadConnection);
            }
        }
        catch (Exception err)
        {
          ErrorClass objErr = new ErrorClass(err, "Notifications DAL");
          objErr.SendMail();
        }
        return ds;
      }
        #endregion
    }// class
}//namespace
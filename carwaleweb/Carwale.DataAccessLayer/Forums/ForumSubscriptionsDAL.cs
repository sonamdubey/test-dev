using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using Carwale.Notifications;
using Carwale.DAL.CoreDAL;
using Carwale.Interfaces.Forums;
using Carwale.Notifications.Logs;
using System.Data.Common;
using Carwale.DAL.CoreDAL.MySql;
using MySql.Data.MySqlClient;
/// <summary>
/// Implements ForumSubscription interface members.
/// </summary>
/// 
namespace Carwale.DAL.Forums
{
    public class ForumSubscriptionsDAL : IForumSubscription
    {
        #region Manage Subscriptions
        /// <summary>
        /// This method manages subsciption based on the action type. Action Type may be any of : Change Type Or unsubscribe.
        /// 
        /// </summary>
        /// <param name="actionType"></param>
        /// <param name="emailSubscriptionId"></param>
        /// <param name="customerId"></param>
        /// <param name="forumThreadId"></param>
        public void ManageSubscriptions(string actionType, int emailSubscriptionId, int customerId, int forumThreadId)
        {
            //Database db = new Database();
           // string conStr = db.GetConString();
            try
            {
                if (actionType == "changeType")
                {
                    using(DbCommand cmd = DbFactory.GetDBCommand("ForumsChangeSubscriptionType_v16_11_7"))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DbFactory.GetDbParam("v_EmailSubscriptionId", DbType.Int64, emailSubscriptionId));
                        cmd.Parameters.Add(DbFactory.GetDbParam("v_CustomerId", DbType.Int64, customerId));
                        cmd.Parameters.Add(DbFactory.GetDbParam("v_ForumThreadId", DbType.Int64, forumThreadId));
                        MySqlDatabase.UpdateQuery(cmd, DbConnections.ForumsMySqlMasterConnection);
                    }              
                }
                else
                {
                    using(DbCommand cmd = DbFactory.GetDBCommand("ForumsUnsubscribeToThread_v16_11_7"))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DbFactory.GetDbParam("v_CustomerId",DbType.Int64,customerId));
                        cmd.Parameters.Add(DbFactory.GetDbParam("v_ForumThreadId",DbType.Int64,forumThreadId));
                        MySqlDatabase.ExecuteNonQuery(cmd,DbConnections.ForumsMySqlMasterConnection);
                    }
                }
            }
            catch (MySqlException err)
            {
                ErrorClass objErr = new ErrorClass(err, "Subscription DAL - ManageSubscriptions - SQL Exception.");
                objErr.SendMail();
            } // catch Exception
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, "Subscription DAL - ManageSubscriptions");
                objErr.SendMail();
            }
        }
        #endregion

        #region Show Subscriptions
        /// <summary>
        /// This method gets the list of subscriptions for a customer.
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public DataSet ShowSubscriptions(string customerId)
        {
           // Database db = new Database();
          //  string conStr = db.GetConString();
            DataSet ds = new DataSet();
            try
            {
                using(DbCommand cmd = DbFactory.GetDBCommand("ForumsShowSubscriptions_v16_11_7"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_CustomerId",DbType.Int64,customerId));
                    ds = MySqlDatabase.SelectAdapterQuery(cmd,DbConnections.ForumsMySqlReadConnection);
                }
            }
            catch (MySqlException err)
            {
              ErrorClass objErr = new ErrorClass(err, "Subscription DAL - ManageSubscriptions - MYSQL Exception.");
              objErr.SendMail();
            } // catch Exception
            catch (Exception err)
            {
              ErrorClass objErr = new ErrorClass(err, "Subscription DAL - ManageSubscriptions");
              objErr.SendMail();
            }
            return ds;
        }
        #endregion

        #region Add Subscriptions
        public bool AddSubscription(string custID, string subId)
        {          
            bool returnVal = false;
            try
            {
                using(DbCommand cmd = DbFactory.GetDBCommand("Forum_InsertSubscription_v16_11_7"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_customerId", DbType.Int64, custID));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_subId", DbType.Int64, subId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_alertType", DbType.Int32, 2));
                    LogLiveSps.LogSpInGrayLog(cmd);
                    MySqlDatabase.ExecuteNonQuery(cmd, DbConnections.ForumsMySqlMasterConnection);
                    returnVal = true;
                }
            }
            catch (MySqlException err)
            {
                ErrorClass objErr = new ErrorClass(err, "Subscription DAL - Add Subscriptions - SQL Exception.");
                objErr.SendMail();
            } // catch Exception
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, "Subscription DAL - Add Subscriptions");
                objErr.SendMail();
            }
            return returnVal;
        }
        #endregion


    }//class
}//namespace
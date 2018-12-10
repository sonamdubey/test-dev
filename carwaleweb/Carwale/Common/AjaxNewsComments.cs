using System;
using System.Data;
using System.Web;
using Carwale.Notifications;
using System.Data.Common;
using Carwale.DAL.CoreDAL.MySql;
using MySql.Data.MySqlClient;

namespace Carwale.Community.Mods
{
    public class AjaxNewsComments
    {
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.None)]
        public bool ApproveComment(string commentId)
        {
            bool isApproved = false;       
            try
            {             
                using(DbCommand cmd = DbFactory.GetDBCommand("ApproveNewsComment_v16_11_7"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_CommentId",DbType.Int64,commentId));
                    isApproved = MySqlDatabase.UpdateQuery(cmd, DbConnections.EditCmsMySqlMasterConnection);
                }
            }
            catch (MySqlException ex)
            {
                isApproved = false;
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["Url"]);
                objErr.SendMail();
            }
            catch (Exception ex)
            {
                isApproved = false;
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["Url"]);
                objErr.SendMail();
            }          
            return isApproved;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.None)]
        public bool DeleteComment(string commentId)
        {
            bool isDeleted = false;
            try
            {          
                using(DbCommand cmd = DbFactory.GetDBCommand("DeleteNewsComment_v16_11_7"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_CommentId", DbType.Int64, commentId));
                    isDeleted = MySqlDatabase.UpdateQuery(cmd, DbConnections.EditCmsMySqlMasterConnection);
                }
            }
            catch (MySqlException ex)
            {
                isDeleted = false;
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["Url"]);
                objErr.SendMail();
            }
            catch (Exception ex)
            {
                isDeleted = false;
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["Url"]);
                objErr.SendMail();
            }          
            return isDeleted;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.None)]
        public bool SaveModEditComment(string commentId, string comment)
        {
            bool isSaved = false;
            try
            {          
                using(DbCommand cmd = DbFactory.GetDBCommand("SaveModeratorEditedComment_v16_11_7"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_CommentId", DbType.Int64, commentId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_Comment", DbType.String, 500, comment));
                    isSaved = MySqlDatabase.UpdateQuery(cmd, DbConnections.EditCmsMySqlMasterConnection);
                }
            }
            catch (MySqlException ex)
            {
                isSaved = false;
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["Url"]);
                objErr.SendMail();
            }
            catch (Exception ex)
            {
                isSaved = false;
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["Url"]);
                objErr.SendMail();
            }            
            return isSaved;
        }
    }
}
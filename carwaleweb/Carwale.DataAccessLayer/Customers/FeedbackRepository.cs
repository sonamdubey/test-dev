using Carwale.Entity.Customers;
using Carwale.Interfaces.Users;
using Carwale.Notifications;
using Carwale.Notifications.Logs;
using Dapper;
using System;
using System.Data;

namespace Carwale.DAL.Customers
{
    public class FeedbackRepository : RepositoryBase, IFeedbackRepository
    {
        public bool InsertFeedback(UserFeedback feedback)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("v_username", feedback.UserName, DbType.String);
                param.Add("v_email", feedback.Email, DbType.String);
                param.Add("v_feedback", feedback.Feedback, DbType.String);
                param.Add("v_userip", feedback.UserIp, DbType.String);
                param.Add("v_feedbackdatetime", feedback.FeedbackDateTime, DbType.DateTime);
                param.Add("v_fbsource", feedback.Source, DbType.String);
                param.Add("v_feedbackrating", feedback.FeedbackRating, DbType.Int16);
                param.Add("v_carinfo", feedback.CarInfo, DbType.String);
                param.Add("v_fbsourceid", feedback.SourceId, DbType.Int16);
                param.Add("v_reportid", feedback.ReportId);
                using (var con = ClassifiedMySqlMasterConnection)
                {
                    LogLiveSps.LogSpInGrayLog("InsertVisitorFeedback");
                    return con.Execute("InsertVisitorFeedback", param, commandType: CommandType.StoredProcedure) > 0;

                }
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, "FeedbackRepository.InsertFeedback");
                objErr.SendMail();
            }
            return false;
        }
    }
}

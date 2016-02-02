using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppNotification.Entity;
using AppNotification.Interfaces;
using System.Data.SqlClient;
using System.Data;
using AppNotification.DAL.Core;
using AppNotification.Notifications;

namespace AppNotification.DAL
{
    public class MobileAppAlertRepository : IMobileAppAlertRepository
    {
        public List<string> GetRegistrationIds(int alertTypeId, int startNum, int endNum)
        {
            var regList = new List<string>();
            try
            {
                using (SqlCommand cmd = new SqlCommand("[Mobile].[GetRegIds]"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@OBJ_TYPE_ID", SqlDbType.Int).Value = alertTypeId;
                    cmd.Parameters.Add("@START_NUM", SqlDbType.Int).Value = startNum;
                    cmd.Parameters.Add("@END_NUM", SqlDbType.Int).Value = endNum;
                    var db = new Database();
                    using (SqlDataReader dr = db.SelectQry(cmd))
                    {
                        while (dr.Read())
                        {
                            regList.Add(dr["GCMRegId"].ToString() + "," + dr["Os"]);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "IMobileAppAlertRepository.GetRegistrationIds()");
                objErr.LogException();
            }

            return regList;
        }

        public int GetTotalNumberOfSubs(int alertTypeId)
        {
            var numOfRegIds = 0;

            try
            {
                using (SqlCommand cmd = new SqlCommand("[Mobile].[GetNumberOfRegIds]"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@OBJ_TYPE_ID", SqlDbType.Int).Value = alertTypeId;
                    cmd.Parameters.Add("@Count", SqlDbType.Int).Direction = ParameterDirection.Output;

                    var db = new Database();
                    db.ExecuteScalar(cmd);
                    numOfRegIds = (int)cmd.Parameters["@Count"].Value;
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "IMobileAppAlertRepository.GetTotalNumberOfSubs(alertTypeId)");
                objErr.LogException();
            }

            return numOfRegIds;
        }


        public bool SubscriberActivity(MobileAppNotificationRegistration t)
        {
            bool isComplete = false;

            try
            {
                using (SqlCommand cmd = new SqlCommand("[Mobile].[SubscriptionActivity]"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@IMEI", SqlDbType.VarChar).Value = t.IMEI;
                    cmd.Parameters.Add("@Name", SqlDbType.VarChar).Value = t.Name;
                    cmd.Parameters.Add("@Email", SqlDbType.VarChar).Value = t.EmailId;
                    cmd.Parameters.Add("@ContactNo", SqlDbType.VarChar).Value = t.ContactNo;
                    cmd.Parameters.Add("@OsType", SqlDbType.TinyInt).Value = t.OsType;
                    cmd.Parameters.Add("@GCMId", SqlDbType.VarChar).Value = t.GCMId;
                    cmd.Parameters.Add("@SubsMasterId", SqlDbType.VarChar).Value = t.SubsMasterId;

                    var db = new Database();
                    db.ExecuteScalar(cmd);

                    isComplete = true;
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "IMobileAppAlertRepository.SubscriberActivity(t)");
                objErr.LogException();
            }

            return isComplete;
        }

        /// <summary>
        /// Added By : Sadhana Upadhyay
        /// </summary>
        /// <param name="alertTypeId"></param>
        /// <returns></returns>
        public bool CompleteNotificationProcess(int alertTypeId)
        {
            bool isNotificationComplete = false;

            try
            {
                using (SqlCommand cmd = new SqlCommand("[Mobile].[ResetSubscriptionMaster_IsProcessing]"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@OBJ_TYPE_ID", SqlDbType.Int).Value = alertTypeId;

                    var db = new Database();
                    isNotificationComplete = db.UpdateQry(cmd);
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "IMobileAppAlertRepositoryCompleteNotificationProcess(int alertTypeId)");
                objErr.LogException();
            }

            return isNotificationComplete;
        }
    }
}

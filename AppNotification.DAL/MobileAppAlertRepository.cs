using AppNotification.Entity;
using AppNotification.Interfaces;
using AppNotification.Notifications;
using MySql.CoreDAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace AppNotification.DAL
{
    public class MobileAppAlertRepository : IMobileAppAlertRepository
    {
        public List<string> GetRegistrationIds(int alertTypeId, int startNum, int endNum)
        {
            var regList = new List<string>();
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getregids"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_obj_type_id", DbType.Int32, alertTypeId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_start_num", DbType.Int32, startNum));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_end_num", DbType.Int32, endNum));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            while (dr.Read())
                            {
                                regList.Add(dr["gcmregid"].ToString() + "," + dr["os"]);
                            }
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
                using (DbCommand cmd = DbFactory.GetDBCommand("getnumberofregids"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_obj_type_id", DbType.Int32, alertTypeId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_count", DbType.Int32, ParameterDirection.Output));

                    MySqlDatabase.ExecuteNonQuery(cmd, ConnectionType.ReadOnly);

                    numOfRegIds = (int)cmd.Parameters["par_count"].Value;
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
                using (DbCommand cmd = DbFactory.GetDBCommand("subscriptionactivity"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_imei", DbType.String, t.IMEI));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_name", DbType.String, t.Name));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_email", DbType.String, t.EmailId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_contactno", DbType.String, t.ContactNo));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_ostype", DbType.Byte, t.OsType));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_gcmid", DbType.String, t.GCMId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_subsmasterid", DbType.String, t.SubsMasterId));

                    MySqlDatabase.ExecuteScalar(cmd, ConnectionType.MasterDatabase);

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
                using (DbCommand cmd = DbFactory.GetDBCommand("resetsubscriptionmaster_isprocessing"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_obj_type_id", DbType.Int32, alertTypeId));

                    isNotificationComplete = MySqlDatabase.UpdateQuery(cmd, ConnectionType.MasterDatabase);
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

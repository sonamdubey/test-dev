using System;
using System.Collections.Generic;
using AppNotification.Entity;
using AppNotification.Interfaces;
using System.Data.SqlClient;
using System.Data;
using AppNotification.DAL.Core;
using AppNotification.Notifications;
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
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_obj_type_id", DbParamTypeMapper.GetInstance[SqlDbType.Int], alertTypeId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_start_num", DbParamTypeMapper.GetInstance[SqlDbType.Int], startNum));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_end_num", DbParamTypeMapper.GetInstance[SqlDbType.Int], endNum));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd))
                    {
                        if (dr!=null)
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
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_obj_type_id", DbParamTypeMapper.GetInstance[SqlDbType.Int], alertTypeId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_count", DbParamTypeMapper.GetInstance[SqlDbType.Int], ParameterDirection.Output));

                    MySqlDatabase.ExecuteNonQuery(cmd);

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
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_imei", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], t.IMEI));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_name", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], t.Name));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_email", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], t.EmailId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_contactno", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], t.ContactNo));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_ostype", DbParamTypeMapper.GetInstance[SqlDbType.TinyInt], t.OsType));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_gcmid", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], t.GCMId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_subsmasterid", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], t.SubsMasterId));

                    MySqlDatabase.ExecuteScalar(cmd);

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
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_obj_type_id", DbParamTypeMapper.GetInstance[SqlDbType.Int], alertTypeId));

                    isNotificationComplete = MySqlDatabase.UpdateQuery(cmd);
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

using Bikewale.CoreDAL;
using Bikewale.Interfaces.AppAlert;
using Bikewale.Notifications;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Web;

namespace Bikewale.DAL.AppAlert
{
    public class AppAlertRepository : IAppAlert
    {

        /// <summary>
        /// Auth: Sangram Nandkhile on 5th January 2016
        /// Desc: Push IMEI, GCM id, OS Type, Subscription Master Id
        /// </summary>
        /// <param name="imei"></param>
        /// <param name="gcmId"></param>
        /// <param name="osType"></param>
        /// <param name="subsMasterId"></param>
        /// <returns></returns>
        public bool SaveImeiGcmData(string imei, string gcmId, string osType, string subsMasterId)
        {
            bool isResult = true;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandText = "subscriptionactivity";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_imei", DbType.String, 50, imei));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_gcmid", DbType.String, 200, gcmId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_ostype", DbType.Byte, osType));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_subsmasterid", DbType.String, 100, subsMasterId));
                        LogLiveSps.LogSpInGrayLog(cmd);
                    MySqlDatabase.ExecuteNonQuery(cmd);

                }
            }
            catch (SqlException ex)
            {
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
                isResult = false;
            }
            return isResult;
        }   // End of GetAreas method

    }
}

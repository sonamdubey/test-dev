using Bikewale.CoreDAL;
using Bikewale.Interfaces.AppAlert;
using Bikewale.Notifications;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
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
            Database db = null;
            bool isResult = true;
            try
            {
                db = new Database();

                using (SqlConnection con = new SqlConnection(db.GetConString()))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.CommandText = "Mobile.SubscriptionActivity";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = con;

                        cmd.Parameters.Add("@imei", SqlDbType.VarChar, 50).Value = imei;
                        cmd.Parameters.Add("@Gcmid", SqlDbType.VarChar, 200).Value = gcmId;
                        cmd.Parameters.Add("@osType", SqlDbType.TinyInt).Value = osType;
                        cmd.Parameters.Add("@subsMasterId", SqlDbType.VarChar).Value = subsMasterId;

                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException ex)
            {
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
                isResult = false;
            }
            finally
            {
                db.CloseConnection();
            }

            return isResult;
        }   // End of GetAreas method

    }
}

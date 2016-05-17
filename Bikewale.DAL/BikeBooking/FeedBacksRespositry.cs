using Bikewale.CoreDAL;
using Bikewale.Entities.BikeBooking;
using Bikewale.Interfaces.Cancellation;
using Bikewale.Notifications;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Bikewale.DAL.BikeBooking
{
    public class FeedBacksRespositry : IFeedBacks
    {
        /// <summary>
        /// Created By : Lucky Rathore
        /// Date : 21 November 2016
        /// </summary>
        public bool SaveFeedBack(FeedBackEntity feedback) {
            bool isSuccess = default(bool);
            //Database db = null;
            //try
            //{
            //    db = new Database();
            //    using (SqlConnection con = new SqlConnection(db.GetConString()))
            //    {
            //        using (SqlCommand cmd = new SqlCommand())
            //        {
            //            cmd.CommandType = CommandType.StoredProcedure;
            //            cmd.CommandText = "BookingCancelFeedBack";
            //            cmd.Connection = con;
            //            cmd.Parameters.Add("@BwId", SqlDbType.VarChar, 15).Value = feedback.BwId;
            //            cmd.Parameters.Add("@FeedBack", SqlDbType.VarChar, 500).Value = feedback.FeedBack;
            //            con.Open();
            //            //affectedRow = cmd.ExecuteNonQuery();
            //            isSuccess = db.InsertQry(cmd);
            //        }
            //    }
            //}
            //catch (SqlException err)
            //{
            //    HttpContext.Current.Trace.Warn(err.Message);
            //    ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
            //    objErr.SendMail();
            //    isSuccess = false;
            //}
            //catch (Exception err)
            //{
            //    HttpContext.Current.Trace.Warn(err.Message);
            //    ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
            //    objErr.SendMail();
            //    isSuccess = false;
            //}
            //finally
            //{
            //    db.CloseConnection();
            //}
            return isSuccess;
        }
    }
}

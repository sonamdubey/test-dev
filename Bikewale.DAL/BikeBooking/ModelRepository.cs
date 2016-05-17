using Bikewale.CoreDAL;
using Bikewale.Entities.BikeData;
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
    public class ModelRepository
    {
        public IEnumerable<BikeModelEntityBase> GetModelByMake(string requestType, Int32 makeId)
        {
            //Database db = null;
            //List<BikeModelEntityBase> lstModel = null;
            //BikeModelEntityBase model = null;
            //try
            //{
            //    using (SqlCommand cmd = new SqlCommand("GetBikeModels"))
            //    {
            //        cmd.CommandType = CommandType.StoredProcedure;
            //        cmd.Parameters.AddWithValue("@RequestType", requestType);
            //        cmd.Parameters.AddWithValue("@MakeId", makeId);

            //        db = new Database();
            //        using (SqlDataReader reader = db.SelectQry(cmd))
            //        {
            //            if (reader != null && reader.HasRows)
            //            {
            //                lstModel = new List<BikeModelEntityBase>();
            //                while (reader.Read())
            //                {
            //                    model = new BikeModelEntityBase();
            //                    model.ModelId = Convert.ToInt32(reader["Value"]);
            //                    model.ModelName = Convert.ToString(reader["Text"]);
            //                    lstModel.Add(model);
            //                }
            //            }
            //        }
            //    }
            //}
            //catch (SqlException ex)
            //{
            //    HttpContext.Current.Trace.Warn(ex.Message + " : Make Id : " + makeId + ", Request Type : " + requestType + ex.Source);
            //    ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
            //    objErr.SendMail();
            //}
            //catch (Exception ex)
            //{
            //    HttpContext.Current.Trace.Warn(ex.Message + ex.Source);
            //    ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
            //    objErr.SendMail();
            //}
            //finally
            //{
            //    db.CloseConnection();
            //}
            //return lstModel;

            return null;
        }
    }
}

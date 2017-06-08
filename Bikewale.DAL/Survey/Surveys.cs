using Bikewale.Interfaces.ServiceCenter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bikewale.Models.Survey;
using MySql.CoreDAL;
using Bikewale.CoreDAL;
using System.Data.Common;
using System.Data;
using Bikewale.Notifications;
using Bikewale.Interfaces;

namespace Bikewale.DAL
{
    /// <summary>
    /// Created by :Sangram Nandkhile on 07 Jun 2017
    /// Summary:
    /// </summary>
    public class SurveyRepository : ISurveyRepository
    {
        public void InsertBajajSurveyResponse(BajajSurveyVM surveryResponse)
        {
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandText = "insertbajajsurveydetails";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_currentbike", DbType.String, 50, surveryResponse.CurrentBike));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_biketopurchase", DbType.String, 50, surveryResponse.BikeToPurchase));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_hasuserseenad", DbType.String, 5, surveryResponse.HasUsedSeenAd));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_adbikemodel", DbType.String, 100, surveryResponse.AdSeenForModel));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_adviews", DbType.String, 100, surveryResponse.BikeToPurchase));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_admedium", DbType.String, 100, surveryResponse.BikeToPurchase));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_age", DbType.String, 100, surveryResponse.Age));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_handset", DbType.String, 100, surveryResponse.Handset));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_city", DbType.String, 100, surveryResponse.City));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_source", DbType.String, 10, surveryResponse.IsMobile ? "Mobile" : "Desktop"));
                    MySqlDatabase.ExecuteNonQuery(cmd, ConnectionType.MasterDatabase);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "SurveyRepository.InsertBajajSurveyResponse()");
            }
        }
    }
}
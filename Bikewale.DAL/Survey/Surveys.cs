using Bikewale.Interfaces;
using Bikewale.Models.Survey;
using Bikewale.Notifications;
using MySql.CoreDAL;
using System;
using System.Data;
using System.Data.Common;

namespace Bikewale.DAL
{
    /// <summary>
    /// Created by :Sangram Nandkhile on 07 Jun 2017
    /// Summary: DAL layer for Survey
    /// </summary>
    public class SurveyRepository : ISurveyRepository
    {
        public void InsertBajajSurveyResponse(BajajSurveyVM surveryResponse)
        {
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandText = "insertbajajsurveydetail";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_currentbike", DbType.String, 50, surveryResponse.CurrentBike));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_biketopurchase", DbType.String, 50, surveryResponse.BikeToPurchase));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_adbikemodel", DbType.String, 100, surveryResponse.RecentBikeCommercial));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_hasuserseenad", DbType.String, 5, surveryResponse.SeenThisAd));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_adviews", DbType.String, 100, surveryResponse.viewscount));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_admedium", DbType.String, 100, surveryResponse.AllMedium));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_age", DbType.String, 100, surveryResponse.Age));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_handset", DbType.String, 100, surveryResponse.Handset));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_city", DbType.String, 100, surveryResponse.City));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_source", DbType.String, 10, surveryResponse.Source));
                    MySqlDatabase.ExecuteNonQuery(cmd, ConnectionType.MasterDatabase);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "SurveyRepository.InsertBajajSurveyResponse()");
            }
        }
    }
}
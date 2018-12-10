using System;
using System.Data;
using System.Linq;
using System.Web;
using Carwale.Notifications;
using Dapper;
using Carwale.Entity.Classified.CarValuation;
using Carwale.Interfaces.Classified.CarValuation;
using Carwale.Notifications.Logs;

namespace Carwale.DAL.Classified.CarValuation
{
    public class ValuationRepository : RepositoryBase, IValuationRepository
    {
        /// <summary>
        /// Save valuation input parameters to Database
        /// </summary>
        /// <returns></returns>
        public int SaveValuationRequest(ValuationRequest valuationRequest, CarValuationResults valuationResults)
        {
            int valuationId = -1;
            try
            {
                var param = new DynamicParameters();
                param.Add("v_carversionid", valuationRequest.VersionId);
                param.Add("v_caryear", new DateTime(Convert.ToInt32(valuationRequest.ManufactureYear), Convert.ToInt16(valuationRequest.ManufactureMonth), 1));
                param.Add("v_carkms", valuationRequest.KmsTraveled);
                param.Add("v_customerid", valuationRequest.CustomerID);
                param.Add("v_cityid", valuationRequest.CityID);
                param.Add("v_city", valuationRequest.City, DbType.String);
                param.Add("v_actualcityid", valuationRequest.ActualCityID);
                param.Add("v_requestdatetime", DateTime.Now);
                param.Add("v_remotehost", HttpContext.Current.Request.ServerVariables["REMOTE_HOST"]);
                param.Add("v_requestsource", valuationRequest.RequestSource);
                param.Add("v_valueexcellent", valuationResults.IndividualValueExcellent);
                param.Add("v_valuegood", valuationResults.IndividualValueGood);
                param.Add("v_valuefair", valuationResults.IndividualValueFair);
                param.Add("v_valuepoor", valuationResults.IndividualValuePoor);
                param.Add("v_valueexcellentdealer", valuationResults.DealerPurchaseValueExcellent);
                param.Add("v_valuegooddealer", valuationResults.DealerPurchaseValueGood);
                param.Add("v_valuefairdealer", valuationResults.DealerPurchaseValueFair);
                param.Add("v_valuepoordealer", valuationResults.DealerPurchaseValuePoor);
                param.Add("v_valuationid", dbType: DbType.Int32, direction: ParameterDirection.Output);

                using (var con = ClassifiedMySqlMasterConnection)
                {
                    con.Execute("InsertValuation", param, commandType: CommandType.StoredProcedure);
                    LogLiveSps.LogSpInGrayLog("InsertValuation");
                }
                valuationId = param.Get<int>("v_valuationid");
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "ValuationRepository.SaveValuationRequest");
                objErr.SendMail();
            }
            return valuationId;
        }

        /// <summary>
        /// Guide Id indicates zones/divisions in India which include the three main cities Mumbai(West), Delhi(North) and Bangalore(South). 
        /// The North Zone also includes all areas in the East.
        /// </summary>
        /// <returns></returns>
        public ValuationBaseValue GetValuationBaseValue(int versionId, int cityId, int carYear)
        {
            ValuationBaseValue _baseValue = null;
            try
            {
                var param = new DynamicParameters();
                param.Add("v_year", carYear);
                param.Add("v_versionid", versionId);
                param.Add("v_cityid", cityId);

                using (var con = ClassifiedMySqlReadConnection)
                {
                    _baseValue = con.Query<ValuationBaseValue>("GetUsedCarValues", param, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    LogLiveSps.LogSpInGrayLog("GetUsedCarValues");
                }
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, "ValuationRepository.GetValuationBaseValue");
                objErr.SendMail();
            }
            return _baseValue;
        }

        /// <summary>
        /// getting nearets city of available valuation of cars
        /// </summary>
        /// <param name="cityId">cityid</param>
        /// <returns>nearest city > cityid:cityname</returns>
        public string GetNearestValuationCity(int cityId)
        {
            string nearestCityId = String.Empty;            
            try
            {
                var param = new DynamicParameters();
                param.Add("v_cityid", cityId);

                using (var con = ClassifiedMySqlReadConnection)
                {
                    nearestCityId = con.Query<string>("Valuation_GetNearestCity", param, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    LogLiveSps.LogSpInGrayLog("Valuation_GetNearestCity");
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "ValuationRepository.GetNearestValuationCity");
                objErr.SendMail();
            }
            return nearestCityId;
        }

        public ValuationRequest GetValuationRequest(int valuationId)
        {
            ValuationRequest valuationRequest = null;
            string sql = "SELECT CarVersionId, CarYear, Kms, CustomerId, CityId, City, ActualCityId, RequestDateTime, RemoteHost, RequestSource FROM carvaluations WHERE Id = @valuationId";
            try
            {
                var param = new DynamicParameters();
                param.Add("@valuationId", valuationId);
                LogLiveSps.LogSpInGrayLog(sql);
                using (var con = ClassifiedMySqlMasterConnection)
                {
                    using (var reader = con.ExecuteReader(sql, param, commandType: CommandType.Text))
                    {
                        if (reader.Read())
                        {
                            valuationRequest = new ValuationRequest();
                            valuationRequest.VersionId = Convert.ToInt32(reader["CarVersionId"]);
                            valuationRequest.ManufactureYear = Convert.ToDateTime(reader["CarYear"]).Year;
                            valuationRequest.ManufactureMonth = Convert.ToDateTime(reader["CarYear"]).Month;
                            valuationRequest.KmsTraveled = Convert.ToInt32(reader["Kms"]);
                            valuationRequest.CustomerID = Convert.ToInt32(reader["CustomerId"]);
                            valuationRequest.CityID = Convert.ToInt32(reader["CityId"]);
                            valuationRequest.City = Convert.ToString(reader["City"]);
                            valuationRequest.ActualCityID = Convert.ToInt32(reader["ActualCityId"]);
                            valuationRequest.RequestDateTime = Convert.ToDateTime(reader["RequestDateTime"]);
                            valuationRequest.RemoteHost = Convert.ToString(reader["RemoteHost"]);
                            valuationRequest.RequestSource = Convert.ToInt16(reader["RequestSource"]);
                        }
                    }
                }
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, "ValuationRepository.GetValuation");
                objErr.SendMail();
            }
            return valuationRequest;
        }

        //public bool SaveListingValuation(int inquiryId, int sellerType, int fairValue, int goodValue, int excellantValue)
        //{
        //    try
        //    {
        //        var param = new DynamicParameters();
        //        param.Add("v_inquiryid", inquiryId);
        //        param.Add("v_sellertype", sellerType);
        //        param.Add("v_fairvaluation", fairValue);
        //        param.Add("v_goodvaluation", goodValue);
        //        param.Add("v_excellantvaluation", excellantValue);
        //        param.Add("v_entrydate", DateTime.Now);

        //        using (var con = ConnectionMySqlMaster)
        //        {
        //            return con.Execute("InsertClassified_ListingValuation", param, commandType: CommandType.StoredProcedure) > 0 ? true : false;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
        //        objErr.SendMail();
        //    }
        //    return false;
        //}
    }
}

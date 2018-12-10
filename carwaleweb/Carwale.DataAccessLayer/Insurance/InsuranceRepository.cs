using Carwale.DAL.CoreDAL;
using Carwale.DAL.CoreDAL.MySql;
using Carwale.Entity.Enum;
using Carwale.Entity.Insurance;
using Carwale.Interfaces.Insurance;
using Carwale.Notifications;
using Carwale.Notifications.Logs;
using Carwale.Utility;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Web;

namespace Carwale.DAL.Insurance
{
    public class InsuranceRepository : IInsuranceRepository
    {
        public int SaveLead<T>(T t) where T : InsuranceLead
        {
            try
            {
                string clientIp = HttpContext.Current.Request.ServerVariables["HTTP_CLIENT_IP"];

                using (var cmd = DbFactory.GetDBCommand("INS_InsertPremiumLeads_v16_12_1"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_Id", DbType.Int64, -1));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_VersionId", DbType.Int64, t.VersionId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_CustomerId", DbType.Int64, t.CustomerId > 0 ? t.CustomerId : -1));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_CityId", DbType.String, 300, t.CityId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_Name", DbType.String, 300, string.IsNullOrWhiteSpace(t.Name) ? Convert.DBNull : t.Name));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_Mobile", DbType.String, 300, string.IsNullOrWhiteSpace(t.Mobile) ? Convert.DBNull : t.Mobile));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_Email", DbType.String, 300, string.IsNullOrWhiteSpace(t.Email) ? Convert.DBNull : t.Email));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_RequestDateTime", DbType.DateTime, DateTime.Now));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_InsTypeNew", DbType.Boolean, Convert.ToInt16(t.InsuranceNew)));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_CarRegistrationDate", DbType.DateTime, String.IsNullOrWhiteSpace(t.CarPurchaseDate) ? Convert.DBNull : CustomParser.parseDateObject(t.CarPurchaseDate.Trim())));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_NoClaimBonus", DbType.Int16, t.NCBPercent > 0 ? t.NCBPercent : Convert.DBNull));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_RegistrationArea", DbType.String, 50, t.clientId == Clients.Coverfox ? t.StateName : "N/A CARWALE"));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_ClientId", DbType.Int16, t.clientId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_Price", DbType.Int32, t.Price > 0 ? t.Price : 0));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_MakeYear", DbType.String, 20, t.CarManufactureYear));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_Displacement", DbType.Int64, Convert.DBNull));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_Premium", DbType.Decimal, Convert.DBNull));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_LeadSource", DbType.Int32, t.LeadSource > 0 ? t.LeadSource : (int)t.Platform));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_UtmCode", DbType.String, 50, t.UtmCode));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_StatusCode", DbType.String, 20, Convert.DBNull));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_ClientIp", DbType.String, 30, String.IsNullOrEmpty(clientIp) ? Convert.DBNull : clientIp));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_RecordId", DbType.Int64, ParameterDirection.Output));                        
                    LogLiveSps.LogMySqlSpInGrayLog(cmd);

                    MySqlDatabase.ExecuteNonQuery(cmd, DbConnections.EsMySqlMasterConnection);

                    return Convert.ToInt32(cmd.Parameters["v_RecordId"].Value);
                }
                
            }
            catch (SqlException err)
            {
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }

            return default(int);
        }

        public bool UpdateLeadResponse(int recordId, string apiResponse, double premiumAmount, string quotaion = null)
        {
            bool success = false;

            try
            {
                using (var cmd = DbFactory.GetDBCommand("UpdateInsuranceLeadStatus_v16_12_1"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_Id", DbType.Int64, recordId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_StatusId", DbType.Int16, Convert.DBNull));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_Status", DbType.String,!string.IsNullOrWhiteSpace(apiResponse) ? apiResponse : Convert.DBNull));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_Premium", DbType.Decimal, premiumAmount > 0 ? premiumAmount : Convert.DBNull));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_Quotation", DbType.String, String.IsNullOrEmpty(quotaion) ? Convert.DBNull : quotaion));
                    LogLiveSps.LogMySqlSpInGrayLog(cmd);

                    MySqlDatabase.ExecuteNonQuery(cmd, DbConnections.EsMySqlMasterConnection);
                }
            }
            catch (SqlException err)
            {
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            return success;
        }

        public InsuranceDiscount GetDiscount(int modelId, int cityId)
        {
            throw new NotImplementedException();
        }

        public int GetClient(InsuranceLead t)
        {
            try
            {
                using (var cmd = DbFactory.GetDBCommand("GetInsuranceClient_v16_12_1"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_LeadSource", DbType.Int32, t.LeadSource>0?t.LeadSource:Convert.DBNull));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_ApplicationId", DbType.Int32, (int)t.Application>0?(int)t.Application:Convert.DBNull));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_PlatformId", DbType.Int32, (int)t.Platform > 0 ? (int)t.Platform : Convert.DBNull));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_CityId", DbType.Int32, t.CityId > 0 ? t.CityId : Convert.DBNull));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_State", DbType.Int32, t.StateId>0 ? t.StateId : Convert.DBNull));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_Price", DbType.Int32, t.Price>0 ? t.Price : Convert.DBNull));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_isNew",DbType.Boolean, t.InsuranceNew));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_Age", DbType.Int32, t.CustomerAge== null || !RegExValidations.IsNumeric(t.CustomerAge) ? Convert.DBNull : Convert.ToInt32(t.CustomerAge)));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_Salary", DbType.Int32, Convert.DBNull));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_RegistrationDate", DbType.DateTime, string.IsNullOrWhiteSpace(t.CarPurchaseDate) ? Convert.DBNull : CustomParser.parseDateObject(t.CarPurchaseDate)));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_ClientId", DbType.Int32, ParameterDirection.Output));
                    LogLiveSps.LogMySqlSpInGrayLog(cmd);

                    MySqlDatabase.ExecuteNonQuery(cmd, DbConnections.EsMySqlReadConnection);

                    int clientId;
                    Int32.TryParse(cmd.Parameters["v_ClientId"].Value.ToString(), out clientId);

                    return clientId;
                }
            }
            catch (SqlException err)
            {
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            return default(int);
        }

        //discount, displacement, price
        public int GetInsuranceDiscount(int carVersionId, int cityId, int year)
        {
            int discount;
            try
            {
                using (var cmd = DbFactory.GetDBCommand("GetInsuranceDiscount_v16_12_1"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_CarVersionId", DbType.Int32, carVersionId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_CityId", DbType.Int32,cityId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_Discount", DbType.Int32, ParameterDirection.Output));
                    LogLiveSps.LogMySqlSpInGrayLog(cmd);

                    MySqlDatabase.ExecuteNonQuery(cmd, DbConnections.EsMySqlReadConnection);

                    discount = CustomParser.parseIntObject(cmd.Parameters["v_Discount"].Value);
                    return discount;
                }
            }
            catch (SqlException err)
            {
                ErrorClass objErr = new ErrorClass(err, "DAL.Insurance.InsuranceRepository.GetDiscountAndDisplacement(" + carVersionId + "," + cityId + "," + year + ")");
                objErr.SendMail();
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, "DAL.Insurance.InsuranceRepository.GetDiscountAndDisplacement(" + carVersionId + "," + cityId + "," + year + ")");
                objErr.SendMail();
            }
            return 0;
        }

    }
}

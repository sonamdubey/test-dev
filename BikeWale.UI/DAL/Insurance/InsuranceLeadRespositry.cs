using Bikewale.Entities.Insurance;
using Bikewale.Notifications;
using Bikewale.Utility;
using MySql.CoreDAL;
using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Web;


namespace Bikewale.DAL.Insurance
{
    /// <summary>
    /// Created By : Lucky Rathore
    /// Date : 20 November 2015
    /// Description : TO handle Insurace Page Data.
    /// </summary>
    public class InsuranceLeadRespositry
    {
        /// <summary>
        /// Created By : Lucky Rathore
        /// Date : 20 November 2015
        /// Description : TO save Insurace Page Data.
        /// </summary>
        /// <param name="lead">InsuraceLead Entity</param>
        /// <returns></returns>
        public bool SaveLeadDetail(InsuranceLead lead)
        {
            bool isSuccess;
            if (!isvalidLead(lead)) return false;

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "saveinsurancelead";

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_customerid", DbType.Int64, lead.CustomerId));

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_customername", DbType.String, 50, String.IsNullOrEmpty(lead.CustomerName) ? Convert.DBNull : lead.CustomerName));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_cityname", DbType.String, 50, String.IsNullOrEmpty(lead.CityName) ? Convert.DBNull : lead.CityName));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_statename", DbType.String, 50, String.IsNullOrEmpty(lead.StateName) ? Convert.DBNull : lead.StateName));

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_cityid", DbType.Int32, lead.CityId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_email", DbType.String, 100, String.IsNullOrEmpty(lead.Email) ? Convert.DBNull : lead.Email));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_mobile", DbType.String, 10, String.IsNullOrEmpty(lead.Mobile) ? Convert.DBNull : lead.Mobile));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_clientip", DbType.String, 30, String.IsNullOrEmpty(CurrentUser.GetClientIP()) ? "" : CurrentUser.GetClientIP()));

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_insurancepolicytype", DbType.Byte, lead.InsurancePolicyType));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_bikeregistrationdate", DbType.DateTime, lead.BikeRegistrationDate));

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_makename", DbType.String, 30, String.IsNullOrEmpty(lead.MakeName) ? Convert.DBNull : lead.MakeName));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_makeid", DbType.Int32, lead.MakeId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_modelname", DbType.String, 30, String.IsNullOrEmpty(lead.ModelName) ? Convert.DBNull : lead.ModelName));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_modelid", DbType.Int32, lead.ModelId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_versionname", DbType.String, 50, String.IsNullOrEmpty(lead.VersionName) ? Convert.DBNull : lead.VersionName));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_versionid", DbType.Int32, lead.VersionId));

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_clientprice", DbType.Int32, lead.ClientPrice));

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_submitstatus", DbType.String, 50, String.IsNullOrEmpty(lead.SubmitStatus) ? Convert.DBNull : lead.SubmitStatus));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_submitid", DbType.Int32, lead.SubmitStatusId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_clientid", DbType.Int32, Convert.ToUInt16(System.Configuration.ConfigurationManager.AppSettings["insuranceclientid"])));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_leadsourceid", DbType.Int32, lead.LeadSourceId));

                    MySqlDatabase.ExecuteNonQuery(cmd, ConnectionType.MasterDatabase);

                    isSuccess = true;

                }
            }
            catch (SqlException err)
            {
                HttpContext.Current.Trace.Warn(err.Message);
                ErrorClass.LogError(err, HttpContext.Current.Request.ServerVariables["URL"]);
                
                isSuccess = false;
            }
            catch (Exception err)
            {
                HttpContext.Current.Trace.Warn(err.Message);
                ErrorClass.LogError(err, HttpContext.Current.Request.ServerVariables["URL"]);
                
                isSuccess = false;
            }
            return isSuccess;
        }

        /// <summary>
        /// Creted By : Lucky Rathore
        /// Date : 20 November 2015
        /// Description : To validated data.
        /// </summary>
        /// <param name="lead"></param>
        /// <returns></returns>
        private bool isvalidLead(InsuranceLead lead)
        {
            bool isNULLDetail = String.IsNullOrEmpty(lead.CustomerName) || String.IsNullOrEmpty(lead.Email) ||
                String.IsNullOrEmpty(lead.Mobile) || String.IsNullOrEmpty(lead.CityName) || String.IsNullOrEmpty(lead.MakeName) || String.IsNullOrEmpty(lead.ModelName) || String.IsNullOrEmpty(lead.VersionName);

            if (isNULLDetail || lead.BikeRegistrationDate == DateTime.MinValue) return false;
            return true;
        }


    }
}

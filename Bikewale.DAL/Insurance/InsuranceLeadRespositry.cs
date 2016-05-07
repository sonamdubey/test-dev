using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bikewale.Entities.Insurance;
using Bikewale.CoreDAL;
using System.Data.SqlClient;
using System.Web;
using Bikewale.Notifications;
using System.Data;
using System.Net;
using Bikewale.Utility;
using Bikewale.Interfaces.Customer;
using Bikewale.Entities.Customer;
using Bikewale.DAL.Customer;
using System.Data.Common;

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

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_customerid", DbParamTypeMapper.GetInstance[SqlDbType.BigInt], lead.CustomerId));

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_customername", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], 50, String.IsNullOrEmpty(lead.CustomerName) ? Convert.DBNull : lead.CustomerName));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_cityname", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], 50, String.IsNullOrEmpty(lead.CityName) ? Convert.DBNull : lead.CityName));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_statename", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], 50, String.IsNullOrEmpty(lead.StateName) ? Convert.DBNull : lead.StateName));

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_cityid", DbParamTypeMapper.GetInstance[SqlDbType.Int], lead.CityId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_email", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], 100, String.IsNullOrEmpty(lead.Email) ? Convert.DBNull : lead.Email));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_mobile", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], 10, String.IsNullOrEmpty(lead.Mobile) ? Convert.DBNull : lead.Mobile));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_clientip", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], 30, String.IsNullOrEmpty(CommonOpn.GetClientIP()) ? "" : CommonOpn.GetClientIP()));

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_insurancepolicytype", DbParamTypeMapper.GetInstance[SqlDbType.TinyInt], lead.InsurancePolicyType));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_bikeregistrationdate", DbParamTypeMapper.GetInstance[SqlDbType.DateTime], lead.BikeRegistrationDate));

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_makename", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], 30, String.IsNullOrEmpty(lead.MakeName) ? Convert.DBNull : lead.MakeName));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_makeid", DbParamTypeMapper.GetInstance[SqlDbType.Int], lead.MakeId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_modelname", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], 30, String.IsNullOrEmpty(lead.ModelName) ? Convert.DBNull : lead.ModelName));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_modelid", DbParamTypeMapper.GetInstance[SqlDbType.Int], lead.ModelId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_versionname", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], 50, String.IsNullOrEmpty(lead.VersionName) ? Convert.DBNull : lead.VersionName));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_versionid", DbParamTypeMapper.GetInstance[SqlDbType.Int], lead.VersionId));

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_clientprice", DbParamTypeMapper.GetInstance[SqlDbType.Int], lead.ClientPrice));

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_submitstatus", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], 50, String.IsNullOrEmpty(lead.SubmitStatus) ? Convert.DBNull : lead.SubmitStatus));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_submitid", DbParamTypeMapper.GetInstance[SqlDbType.Int], lead.SubmitStatusId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_clientid", DbParamTypeMapper.GetInstance[SqlDbType.Int], Convert.ToUInt16(System.Configuration.ConfigurationManager.AppSettings["insuranceclientid"])));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_leadsourceid", DbParamTypeMapper.GetInstance[SqlDbType.Int], lead.LeadSourceId));

                    MySqlDatabase.ExecuteNonQuery(cmd);

                    isSuccess = true;
                }
            }
            catch (SqlException err)
            {
                HttpContext.Current.Trace.Warn(err.Message);
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
                isSuccess = false;
            }
            catch (Exception err)
            {
                HttpContext.Current.Trace.Warn(err.Message);
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
                isSuccess = false;
            }
            return isSuccess;
        }

        /// <summary>
        /// Creted By : Lucky Rathore
        /// Date : 20 November 2015
        /// Description : To validated data.
        /// TODO : Validate policyType.
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

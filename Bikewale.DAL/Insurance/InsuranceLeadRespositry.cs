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
            Database db = null;
            int affectedRow = 0;
            if (!isvalidLead(lead)) return false;
            
            try
            {
                db = new Database();
                using (SqlConnection con = new SqlConnection(db.GetConString()))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "SaveInsuranceLead";
                        cmd.Connection = con;

                        cmd.Parameters.Add("@CustomerId", SqlDbType.BigInt).Value = lead.CustomerId;

                        cmd.Parameters.Add("@CustomerName", SqlDbType.VarChar, 50).Value = String.IsNullOrEmpty(lead.CustomerName) ? Convert.DBNull : lead.CustomerName;
                        cmd.Parameters.Add("@CityName", SqlDbType.VarChar, 50).Value = String.IsNullOrEmpty(lead.CityName) ? Convert.DBNull : lead.CityName;
                        cmd.Parameters.Add("@StateName", SqlDbType.VarChar, 50).Value = String.IsNullOrEmpty(lead.StateName) ? Convert.DBNull : lead.StateName;
                       
                        cmd.Parameters.Add("@CityId", SqlDbType.Int).Value = lead.CityId;
                        cmd.Parameters.Add("@Email", SqlDbType.VarChar, 100).Value = String.IsNullOrEmpty(lead.Email) ? Convert.DBNull : lead.Email;
                        cmd.Parameters.Add("@Mobile", SqlDbType.VarChar, 10).Value = String.IsNullOrEmpty(lead.Mobile) ? Convert.DBNull : lead.Mobile;
                        cmd.Parameters.Add("@ClientIP", SqlDbType.VarChar, 30).Value = String.IsNullOrEmpty(CommonOpn.GetClientIP()) ? "" : CommonOpn.GetClientIP();

                        cmd.Parameters.Add("@InsurancePolicyType", SqlDbType.TinyInt).Value = lead.InsurancePolicyType;
                        cmd.Parameters.Add("@BikeRegistrationDate", SqlDbType.DateTime).Value = lead.BikeRegistrationDate;

                        cmd.Parameters.Add("@MakeName", SqlDbType.VarChar, 30).Value = String.IsNullOrEmpty(lead.MakeName) ? Convert.DBNull : lead.MakeName;
                        cmd.Parameters.Add("@MakeId", SqlDbType.Int).Value = lead.MakeId;
                        cmd.Parameters.Add("@ModelName", SqlDbType.VarChar, 30).Value = String.IsNullOrEmpty(lead.ModelName) ? Convert.DBNull : lead.ModelName;
                        cmd.Parameters.Add("@ModelId", SqlDbType.Int).Value = lead.ModelId;
                        cmd.Parameters.Add("@VersionName", SqlDbType.VarChar, 50).Value = String.IsNullOrEmpty(lead.VersionName) ? Convert.DBNull : lead.VersionName;
                        cmd.Parameters.Add("@VersionID", SqlDbType.Int).Value = lead.VersionId;
                        
                        cmd.Parameters.Add("@ClientPrice", SqlDbType.Int).Value = lead.ClientPrice;

                        
                        cmd.Parameters.Add("@RequestDateTime", SqlDbType.DateTime).Value = System.DateTime.Now;
                        cmd.Parameters.Add("@SubmitStatus", SqlDbType.VarChar, 50).Value = String.IsNullOrEmpty(lead.SubmitStatus) ? Convert.DBNull : lead.SubmitStatus;
                        cmd.Parameters.Add("@SubmitId", SqlDbType.Int).Value = lead.SubmitStatusId;
                        cmd.Parameters.Add("@ClientId", SqlDbType.Int).Value = Convert.ToUInt16(System.Configuration.ConfigurationManager.AppSettings["InsuranceClientId"]);
                        cmd.Parameters.Add("@LeadSourceId", SqlDbType.Int).Value = lead.LeadSourceId;


                        con.Open();
                        affectedRow = cmd.ExecuteNonQuery();
                        isSuccess = true;
                    }
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
            finally
            {
                db.CloseConnection();
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

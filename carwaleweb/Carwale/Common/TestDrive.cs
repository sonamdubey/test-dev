using System;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using Carwale.Notifications;
using Carwale.DAL.CoreDAL;
using Carwale.BL.HDFC_Bank_C2T;
using Carwale.Utility;
using System.Collections.Generic;
using Carwale.Notifications.Logs;
using System.Data.Common;
using Carwale.DAL.CoreDAL.MySql;
using MySql.Data.MySqlClient;

/// <summary>
/// Push the Lead from CarWale (For Test Drive) into CRM
/// </summary>
namespace Carwale.UI.Common
{
    public class TestDrive
    {
        public static Dictionary<string, string> FeaturedUpcomingText = ExtensionMethods.GetDict(System.Configuration.ConfigurationManager.AppSettings["UpcomingTextDict"] ?? "default=default;");
        #region Push to CRM
        /// <summary>
        /// Method to push the customer details to CRM
        /// </summary>
        /// <param name="carName">Name of the car selected</param>
        /// <param name="custName">Customer name</param>
        /// <param name="email">Customer email</param>
        /// <param name="mobile">Customer mobile</param>
        /// <param name="selectedCityId">City selected by the customer</param>
        /// <param name="versionId">Version selected by the customer</param>
        /// <param name="modelId">Model selected by the customer</param>
        /// <param name="makeId">Make selected by the customer</param>
        /// <returns>Bool value</returns>
        public bool PushCRM(string carName, string custName, string email, string mobile, string selectedCityId, string versionId, string modelId, string makeId, string leadtype = "-1", string cityName = "",string utm="")
        {
            bool isPushCRM = false;
            if (SaveData(carName, custName, email, mobile, versionId, modelId, selectedCityId, leadtype, cityName,utm))
            {
                CRMCommon crm = new CRMCommon();
                crm.SendCarData(makeId, custName, ""/* Customer Last Name */, email, mobile, ""/* Phone Number */,
                                        "-1"/*PQCustomerId*/, selectedCityId, versionId, ""/* Car Name */, "0"/* Price */,
                                            "0" /* Insurance */, "0"/* RTO */, "-1", "2" /*BuyTime*/, "3" /*sourceCategory*/,
                                            "33"/*sourceId*/, "CW Research TD Request Form" /*sourceName*/, modelId);

                isPushCRM = true;
                //divForm.Visible = false;
                //divMessage.Visible = true;
                //divMessage.InnerHtml = "Thank you. Your request has been submitted successfully. A CarWale executive will get back to you soon.<div style=\"margin-top:10px;\" class='buttons'><input type='button' onclick='window.close();' value='Close' class='buttons'/></div><p>&nbsp</p>";
            }
            else
            {
                //divMessage.Visible = true;
                //divMessage.InnerHtml = "We have taken your request already.";
            }
            return isPushCRM;
        }
        #endregion

        #region Save Data
        /// <summary>
        /// Save the details of the customer to the database.
        /// </summary>
        /// <param name="custName">Customer name</param>
        /// <param name="email">Customer email</param>
        /// <param name="mobile">Customer mobile</param>
        /// <param name="versionId">Version selected by the customer (if no version is selected, pass empty string)</param>
        /// <param name="modelId">Model selected by the customer</param>
        /// <returns>Bool value</returns>
        private bool SaveData(string carName, string custName, string email, string mobile, string versionId, string modelId, string selectedCityId, string leadtype = "-1", string cityName = "",string utm = "")
        {
            string sql = string.Empty;
            bool isSaved = false;                           
            string makeName = "", modelName = "", HDFCPushed = "";
            string[] carSplit;

            carSplit = carName.Split('|');
            if (carSplit.Length >= 2)
            {
                makeName = carSplit[0];
                modelName = carSplit[1];
            }     
            if (custName.Length > 0 && mobile.Length > 0)
            {             
                try
                {
                    using(DbCommand cmd = DbFactory.GetDBCommand("SaveUpcomingCarsLeads_v16_11_7"))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DbFactory.GetDbParam("v_ModelId",DbType.Int32,!string.IsNullOrEmpty(modelId) ? modelId : Convert.DBNull));
                        cmd.Parameters.Add(DbFactory.GetDbParam("v_VersionId",DbType.Int64,!string.IsNullOrEmpty(versionId) ? versionId : Convert.DBNull));
                        cmd.Parameters.Add(DbFactory.GetDbParam("v_CustName",DbType.String,100,custName));
                        cmd.Parameters.Add(DbFactory.GetDbParam("v_Email",DbType.String,100,email));
                        cmd.Parameters.Add(DbFactory.GetDbParam("v_Mobile",DbType.String,50,mobile));
                        cmd.Parameters.Add(DbFactory.GetDbParam("v_CreatedOn",DbType.DateTime,DateTime.Now));
                        cmd.Parameters.Add(DbFactory.GetDbParam("v_SelectedCityId",DbType.Int32,!string.IsNullOrEmpty(selectedCityId) ? Convert.ToInt32(selectedCityId) : 0));
                        cmd.Parameters.Add(DbFactory.GetDbParam("v_LeadType",DbType.Int32,!string.IsNullOrEmpty(leadtype) ? leadtype : "0"));
                        cmd.Parameters.Add(DbFactory.GetDbParam("v_HDFCResponse",DbType.String,100,HDFCPushed));
                        cmd.Parameters.Add(DbFactory.GetDbParam("v_UtmSource",DbType.String,500,!string.IsNullOrWhiteSpace(utm) ? utm : Convert.DBNull));
                        isSaved =  MySqlDatabase.InsertQuery(cmd,DbConnections.CarDataMySqlMasterConnection);
                    }
                }               
                catch (MySqlException err)
                {
                    HttpContext.Current.Trace.Warn(err.Message);
                    ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                    objErr.SendMail();
                }
                catch (Exception err)
                {
                    HttpContext.Current.Trace.Warn(err.Message);
                    ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                    objErr.SendMail();
                }
              
            }
            return isSaved;
        }
        #endregion

        /// <summary>
        /// Checks is modelId for Featured Upcoming car (from web.config)
        /// </summary>
        /// <param name="modelId"></param>
        /// <returns></returns>
        public static bool isFeaturedUpcoming(string modelId)
        {
            if (string.IsNullOrWhiteSpace(modelId)) return false;
            return ExtensionMethods.CommaSeperatedContains(System.Configuration.ConfigurationManager.AppSettings["UpcomingModelId"] ?? string.Empty, modelId);
        }

        public static string GetFeaturedUpcomingString(string modelId)
        {
            if (string.IsNullOrWhiteSpace(modelId)) return string.Empty;
            string text;
            if (FeaturedUpcomingText.TryGetValue(modelId, out text)) return text;
            return string.Empty;
        }
    }
}
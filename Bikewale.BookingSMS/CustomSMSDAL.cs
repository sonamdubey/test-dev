using Consumer;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Bikewale.BookingSMS
{
    /// <summary>
    /// Author  : Sumit Kate
    /// Created : 16 July 2015
    /// Use to communicate with BikeWale Db using ADO.NET
    /// </summary>
    public class CustomSMSDAL
    {
        protected string connectionString = String.Empty;

        /// <summary>
        /// Constructor used to initialize the connectionString member variable
        /// by reading AppSettings key bwconnectionstring
        /// </summary>
        public CustomSMSDAL()
        {
            this.connectionString = ConfigurationManager.AppSettings["bwconnectionstring"];
        }

        /// <summary>
        /// Saves the SMS by calling InsertSMSSent stored procedure
        /// </summary>
        /// <param name="number">Recepient</param>
        /// <param name="message">Message</param>
        /// <param name="smsType">Type of SMS</param>        
        /// <param name="retMsg"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public string InsertSMS(string number, string message, EnumSMSServiceType smsType, string retMsg, bool status)
        {
            SqlConnection connection = null;
            SqlCommand command = null;
            string currentId = string.Empty;
            try
            {
                using (connection = new SqlConnection(this.connectionString))
                {
                    using (command = new SqlCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "InsertSMSSent";
                        command.Connection = connection;
                        command.Parameters.AddWithValue("@Number", number);
                        command.Parameters.AddWithValue("@Message", message);
                        command.Parameters.AddWithValue("@ServiceType", smsType);
                        command.Parameters.AddWithValue("@SMSSentDateTime", DateTime.Now);
                        command.Parameters.AddWithValue("@Successfull", status);
                        command.Parameters.AddWithValue("@ReturnedMsg", String.Empty);
                        command.Parameters.AddWithValue("@SMSPageUrl", String.Empty);
                        connection.Open();
                        currentId = Convert.ToString(command.ExecuteScalar());
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                Logs.WriteErrorLog("SqlException in InsertSMS : " + sqlEx.Message);
            }
            catch (Exception ex)
            {
                Logs.WriteErrorLog("Exception in InsertSMS : " + ex.Message);
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
            return currentId;
        }

        /// <summary>
        /// Gets the Offer SMS data by calling GetRecipientForOfferSMS.
        /// </summary>
        /// <returns>Enumerable collection of CustomSMSEntity</returns>
        public IEnumerable<CustomSMSEntity> FetchSMSData()
        {
            SqlConnection connection = null;
            SqlCommand command = null;
            SqlDataReader objReader = null;
            CustomSMSEntity objEntity = null;
            List<CustomSMSEntity> lstSMS = null;
            try
            {
                using (connection = new SqlConnection(this.connectionString))
                {
                    using (command = new SqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = "GetRecipientForOfferSMS";
                        command.CommandType = CommandType.StoredProcedure;
                        connection.Open();
                        objReader = command.ExecuteReader();
                        if (objReader != null && objReader.HasRows)
                        {
                            lstSMS = new List<CustomSMSEntity>();
                            while (objReader.Read())
                            {
                                objEntity = new CustomSMSEntity();
                                objEntity.BikeName = Convert.ToString(objReader["Bike"]);
                                objEntity.CustomerContact = Convert.ToString(objReader["CustomerMobile"]);
                                objEntity.DealerContact = Convert.ToString(objReader["DealerMobileNo"]);
                                objEntity.DealerName = Convert.ToString(objReader["Organization"]);
                                lstSMS.Add(objEntity);
                            }
                            Logs.WriteInfoLog(String.Format("Total Records fetched : {0}", lstSMS.Count));
                        }
                        else
                        {
                            Logs.WriteInfoLog("No Records fetched");
#if DEBUG
                            //This is executed ONLY Debug VS Profile is selected
                            lstSMS = new List<CustomSMSEntity>();
                            lstSMS.Add(new CustomSMSEntity() { BikeName = "Test Bike", DealerName = "Test Dealer", DealerContact = "9028300490", CustomerContact = "9028300490" });
#endif
                        }
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                Logs.WriteErrorLog("SqlException in FetchSMSData : " + sqlEx.Message);
            }
            catch (Exception ex)
            {
                Logs.WriteErrorLog("Exception in FetchSMSData : " + ex.Message);
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
            return lstSMS;
        }
    }
}

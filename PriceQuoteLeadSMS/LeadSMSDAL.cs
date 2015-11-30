using Consumer;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bikewale.Notifications;

namespace PriceQuoteLeadSMS
{
    internal class LeadSMSDAL
    {
        private string _connectionString = String.Empty;
        public LeadSMSDAL()
        {
            _connectionString = ConfigurationManager.AppSettings["connectionstring"];
        }

        /// <summary>
        /// Save sms sent to entry to the sms sent table
        /// </summary>
        /// <param name="number"></param>
        /// <param name="message"></param>
        /// <param name="smsType"></param>
        /// <param name="retMsg"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public string InsertSMS(string number, string message, EnumSMSServiceType smsType, string retMsg, bool status)
        {
            string currentId = string.Empty;
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    using (SqlCommand command = new SqlCommand())
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

                        connection.Close();
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
            return currentId;
        }

        /// <summary>
        /// Created By : Sadhana Upadhyay on 3 Nov 2014
        /// Summary : To update ispushedtoab flag in pq_newbikedealerpricequote
        /// </summary>
        /// <param name="pqId"></param>
        /// <returns></returns>
        public bool PushedToAB(uint pqId, uint abInquiryId)
        {
            bool isSuccess = false;
            
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.CommandText = "IsPushedToAB";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = conn;

                        cmd.Parameters.Add("@pqId", SqlDbType.BigInt).Value = pqId;
                        cmd.Parameters.Add("@ABInquiryId", SqlDbType.BigInt).Value = abInquiryId;

                        conn.Open();
                        cmd.ExecuteNonQuery();
                        conn.Close();
                    }
                }
            }
            catch (Exception sqEx)
            {
                Logs.WriteErrorLog("PushedToAB sqlex : " + sqEx.Message + sqEx.Source);
                ErrorClass objErr = new ErrorClass(sqEx, "PriceQuoteLeadSMS.LeadSMSDAL.PushedToAB");
                objErr.SendMail();
                isSuccess = false;
            }            

            return isSuccess;
        }

    }   // class
}   // namespace

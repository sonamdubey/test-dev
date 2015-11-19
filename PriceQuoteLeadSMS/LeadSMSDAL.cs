using Consumer;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceQuoteLeadSMS
{
    class LeadSMSDAL
    {
        private string _connectionString = String.Empty;
        public LeadSMSDAL()
        {
            _connectionString = ConfigurationManager.AppSettings["connectionstring"];
        }

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
    }
}

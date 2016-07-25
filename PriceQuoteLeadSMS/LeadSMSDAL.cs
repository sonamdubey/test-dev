using Bikewale.Notifications;
using Consumer;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

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
        public uint InsertSMS(string number, string message, ushort smsType, string retMsg, bool status)
        {
            throw new Exception("InsertSMS(string number, string message, ushort smsType, string retMsg, bool status) Method is NotImplemented ");

            //uint currentId = 0;
            //try
            //{

                
                //using (SqlConnection connection = new SqlConnection(_connectionString))
                //{
                //    using (SqlCommand command = new SqlCommand())
                //    {
                //        command.CommandType = CommandType.StoredProcedure;
                //        command.CommandText = "InsertSMSSent";
                //        command.Connection = connection;
                //        command.Parameters.AddWithValue("@Number", number);
                //        command.Parameters.AddWithValue("@Message", message);
                //        command.Parameters.Add("@ServiceType", SqlDbType.Int).Value = smsType;
                //        command.Parameters.AddWithValue("@SMSSentDateTime", DateTime.Now);
                //        command.Parameters.AddWithValue("@Successfull", status);
                //        command.Parameters.AddWithValue("@ReturnedMsg", String.Empty);
                //        command.Parameters.AddWithValue("@SMSPageUrl", String.Empty);
                        
                //        connection.Open();
                        
                //        currentId = Convert.ToUInt32(command.ExecuteScalar());

                //        connection.Close();
                //    }
                //}
            //}
            //catch (SqlException sqlEx)
            //{
            //    Logs.WriteErrorLog("SqlException in InsertSMS : " + sqlEx.Message);
            //}
            //catch (Exception ex)
            //{
            //    Logs.WriteErrorLog("Exception in InsertSMS : " + ex.Message);
            //}
            //return currentId;
        }

        /// <summary>
        /// Created By : Sadhana Upadhyay on 3 Nov 2014
        /// Summary : To update ispushedtoab flag in pq_newbikedealerpricequote
        /// </summary>
        /// <param name="pqId"></param>
        /// <returns></returns>
        public bool PushedToAB(uint pqId, uint abInquiryId)
        {
            throw new Exception("PushedToAB(uint pqId, uint abInquiryId) Method is NotImplemented ");

            //bool isSuccess = false;
            
            //try
            //{
            //    using (SqlConnection conn = new SqlConnection(_connectionString))
            //    {
            //        using (SqlCommand cmd = new SqlCommand())
            //        {
            //            cmd.CommandText = "IsPushedToAB";
            //            cmd.CommandType = CommandType.StoredProcedure;
            //            cmd.Connection = conn;

            //            cmd.Parameters.Add("@pqId", SqlDbType.Int).Value = pqId;
            //            cmd.Parameters.Add("@ABInquiryId", SqlDbType.Int).Value = abInquiryId;

            //            conn.Open();
            //            int noOfRows=cmd.ExecuteNonQuery();

            //            if (noOfRows>0)
            //            {
            //                isSuccess = true;
            //            }
            //            conn.Close();
            //        }
            //    }
            //}
            //catch (Exception sqEx)
            //{
            //    Logs.WriteErrorLog("PushedToAB sqlex : " + sqEx.Message + sqEx.Source);
            //    ErrorClass objErr = new ErrorClass(sqEx, "PriceQuoteLeadSMS.LeadSMSDAL.PushedToAB");
            //    objErr.SendMail();
            //}            

            //return isSuccess;
        }

        /// <summary>
        /// Created By : Sadhana Upadhyay on 30 Nov 2015
        /// Summary : Update IsNotified flag for perticular Pqid.
        /// </summary>
        /// <param name="pqId"></param>
        /// <returns></returns>
        public bool UpdatePQLeadNotifiedFlag(uint pqId)
        {

            throw new Exception("UpdatePQLeadNotifiedFlag(uint pqId) Method is NotImplemented ");

            //bool isUpdated = false;
            //try
            //{
            //    using (SqlConnection conn = new SqlConnection(_connectionString))
            //    {
            //        using (SqlCommand cmd = new SqlCommand())
            //        {
            //            cmd.CommandText = "UpdatePQLeadNotifiedFlag";
            //            cmd.CommandType = CommandType.StoredProcedure;
            //            cmd.Connection = conn;

            //            cmd.Parameters.Add("@PQId", SqlDbType.Int).Value = pqId;

            //            conn.Open();
            //            int noOfRows= cmd.ExecuteNonQuery();

            //            if (noOfRows > 0)
            //            {
            //                isUpdated = true;
            //            }
            //            conn.Close();
            //        }
            //    }
            //}
            //catch (SqlException ex)
            //{
            //    Logs.WriteErrorLog("UpdatePQLeadNotifiedFlag sqlex : " + ex.Message + ex.Source);
            //    ErrorClass objErr = new ErrorClass(ex, "PriceQuoteLeadSMS.LeadSMSDAL.UpdatePQLeadNotifiedFlag sqlex");
            //    objErr.SendMail();
            //}
            //catch (Exception ex)
            //{
            //    Logs.WriteErrorLog("UpdatePQLeadNotifiedFlag ex : " + ex.Message + ex.Source);
            //    ErrorClass objErr = new ErrorClass(ex, "PriceQuoteLeadSMS.LeadSMSDAL.UpdatePQLeadNotifiedFlag ex");
            //    objErr.SendMail();
            //}
            //return isUpdated;
        }

        /// <summary>
        /// Created By : Sadhana Upadhyay on 31 Dec 2014
        /// Summary : to get status of dealer notification
        /// </summary>
        /// <param name="dealerId"></param>
        /// <param name="date"></param>
        /// <param name="customerMobile"></param>
        /// <param name="customerEmail"></param>
        /// <returns></returns>
        public bool IsDealerNotified(uint dealerId, string customerMobile, ulong customerId)
        {
            throw new Exception("IsDealerNotified(uint dealerId, string customerMobile, ulong customerId) Method is NotImplemented ");
            //bool isNotified = false;
            //try
            //{
            //    using (SqlConnection con = new SqlConnection(_connectionString))
            //    {
            //        using (SqlCommand cmd = new SqlCommand())
            //        {
            //            cmd.CommandText = "IsDealerNotified";
            //            cmd.CommandType = CommandType.StoredProcedure;
            //            cmd.Connection = con;

            //            cmd.Parameters.Add("@DealerId", SqlDbType.Int).Value = dealerId;
            //            cmd.Parameters.Add("@CustomerId", SqlDbType.VarChar, 50).Value = customerId;
            //            cmd.Parameters.Add("@CustomerMobile", SqlDbType.VarChar, 50).Value = customerMobile;
            //            cmd.Parameters.Add("@IsDealerNotified", SqlDbType.Bit).Direction = ParameterDirection.Output;

            //            con.Open();

            //            cmd.ExecuteNonQuery();

            //            isNotified = Convert.ToBoolean(cmd.Parameters["@IsDealerNotified"].Value);
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Logs.WriteErrorLog("IsDealerNotified ex : " + ex.Message + ex.Source);
            //    ErrorClass objErr = new ErrorClass(ex, "PriceQuoteLeadSMS.LeadSMSDAL.IsDealerNotified ex");
            //    objErr.SendMail();
            //    isNotified = false;
            //}
            //return isNotified;
        }
    }   // class
}   // namespace

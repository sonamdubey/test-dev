using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Notifications.NotificationDAL
{
    internal class SavePQNotification
    {
        private string _connectionString = ConfigurationManager.AppSettings["bwconnectionstring"];


        /// <summary>
        /// Created By : Sadhana Upadhyay on 1 Dec 2015
        /// Summary : To save Dealer pricequote sms template
        /// </summary>
        /// <param name="pqId"></param>
        /// <param name="message"></param>
        /// <param name="smsType"></param>
        /// <param name="dealerMobileNo"></param>
        /// <param name="pageUrl"></param>
        internal void SaveDealerPQSMSTemplate(uint pqId, string message, int smsType, string dealerMobileNo, string pageUrl)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd=new SqlCommand())
                    {
                        if (pqId > 0)
                        {
                            cmd.CommandText = "SavePQLeadSMSToDealer";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Connection = con;

                            cmd.Parameters.Add("@PQId", SqlDbType.BigInt).Value = pqId;
                            cmd.Parameters.Add("@SMSToDealerMessage", SqlDbType.VarChar, -1).Value = message;
                            cmd.Parameters.Add("@SMSToDealerNumbers", SqlDbType.VarChar, 100).Value = dealerMobileNo;
                            cmd.Parameters.Add("@SMSToDealerServiceType", SqlDbType.TinyInt).Value = smsType;
                            cmd.Parameters.Add("@SMSToDealerPageUrl", SqlDbType.VarChar, 500).Value = pageUrl;

                            con.Open();
                            cmd.ExecuteNonQuery();
                            con.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.Notifications.NotificationDAL.SavePQNotification.SaveDealerPQTemplate");
                objErr.SendMail();
            }
        }

        /// <summary>
        /// Created By : Sadhana Upadhyay on 1 Dec 2015
        /// Summary : To save customer price quote sms template
        /// </summary>
        /// <param name="pqId"></param>
        /// <param name="message"></param>
        /// <param name="smsType"></param>
        /// <param name="customerMobile"></param>
        /// <param name="pageUrl"></param>
        internal void SaveCustomerPQSMSTemplate(uint pqId, string message, int smsType, string customerMobile, string pageUrl)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        if (pqId > 0)
                        {
                            cmd.CommandText = "SavePQLeadSMSToCustomer";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Connection = con;

                            cmd.Parameters.Add("@PQId", SqlDbType.BigInt).Value = pqId;
                            cmd.Parameters.Add("@SMSToCustomerMessage", SqlDbType.VarChar).Value = message;
                            cmd.Parameters.Add("@SMSToCustomerNumbers", SqlDbType.VarChar, 100).Value = customerMobile;
                            cmd.Parameters.Add("@SMSToCustomerServiceType", SqlDbType.TinyInt).Value = smsType;
                            cmd.Parameters.Add("@SMSToCustomerPageUrl", SqlDbType.VarChar, 500).Value = pageUrl;

                            con.Open();
                            cmd.ExecuteNonQuery();
                            con.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.Notifications.NotificationDAL.SavePQNotification.SaveCustomerPQTemplate");
                objErr.SendMail();
            }
        }

        /// <summary>
        /// Created By : Sadhana Upadhyay on 1 Dec 2015
        /// Summary : 
        /// </summary>
        /// <param name="pqId"></param>
        /// <param name="emailBody"></param>
        /// <param name="emailsubject"></param>
        /// <param name="dealerEmail"></param>
        internal void SaveDealerPQEmailTemplate(uint pqId, string emailBody, string emailsubject, string dealerEmail)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        if (pqId > 0)
                        {
                            cmd.CommandText = "SavePQLeadEmailToDealer";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Connection = con;

                            cmd.Parameters.Add("@PQId", SqlDbType.BigInt).Value = pqId;
                            cmd.Parameters.Add("@EmailToDealerMessageBody", SqlDbType.VarChar).Value = emailBody;
                            cmd.Parameters.Add("@EmailToDealerSubject", SqlDbType.VarChar, 500).Value = emailsubject;
                            cmd.Parameters.Add("@EmailToDealerReplyTo", SqlDbType.VarChar,200).Value = dealerEmail;

                            con.Open();
                            cmd.ExecuteNonQuery();
                            con.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.Notifications.NotificationDAL.SavePQNotification.SaveDealerPQEmailTemplate");
                objErr.SendMail();
            }
        }



        internal void SaveCustomerPQEmailTemplate(uint pqId, string emailBody, string emailSubject, string customerEmail)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        if (pqId > 0)
                        {
                            cmd.CommandText = "SavePQLeadEmailToCustomer";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Connection = con;

                            cmd.Parameters.Add("@PQId", SqlDbType.BigInt).Value = pqId;
                            cmd.Parameters.Add("@EmailToCustomerMessageBody", SqlDbType.VarChar).Value = emailBody;
                            cmd.Parameters.Add("@EmailToCustomerSubject", SqlDbType.VarChar, 500).Value = emailSubject;
                            cmd.Parameters.Add("@EmailToCustomerReplyTo", SqlDbType.VarChar, 200).Value = customerEmail;

                            con.Open();
                            cmd.ExecuteNonQuery();
                            con.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.Notifications.NotificationDAL.SavePQNotification.SaveDealerPQEmailTemplate");
                objErr.SendMail();
            }
        }
    }   //End of class
}   //End of namespace

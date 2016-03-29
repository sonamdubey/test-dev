using Bikewale.Notifications;
using Consumer;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace PriceQuoteLeadSMS
{
    class LeadSMS
    {
        private string _connectionString = ConfigurationManager.AppSettings["connectionstring"];
        #region GetLeadInformation method
        /// <summary>
        /// Created By : Sadhana Upadhyay on 30 Nov 2015
        /// Summary : To get Lead information
        /// Modified By :   Sumit Kate on 14 Jan 206
        /// Description :   Fetch Customer Id
        /// </summary>
        /// <returns></returns>
        private IEnumerable<LeadNotificationEntity> GetLeadInformation()
        {
            IList<LeadNotificationEntity> objLeadEntity = null;
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.CommandText = "GetPQLeadNotificationsInfo_29032016";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = conn;

                        conn.Open();

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr != null && dr.HasRows)
                            {
                                objLeadEntity = new List<LeadNotificationEntity>();
                                while (dr.Read())
                                {
                                    objLeadEntity.Add(new LeadNotificationEntity()
                                    {
                                        PQId = Convert.ToUInt32(dr["PQId"]),
                                        DealerId = Convert.ToUInt32(dr["DealerId"]),
                                        CustomerEmail = Convert.ToString(dr["CustomerEmail"]),
                                        CustomerMobile = Convert.ToString(dr["CustomerMobile"]),
                                        CustomerName = Convert.ToString(dr["CustomerName"]),
                                        BikeVersionId = Convert.ToUInt32(dr["BikeVersionId"]),
                                        CityId = Convert.ToUInt32(dr["CityId"]),
                                        SMSToCustomerMessage = Convert.ToString(dr["SMSToCustomerMessage"]),
                                        SMSToCustomerNumbers = Convert.ToString(dr["SMSToCustomerNumbers"]),
                                        SMSToCustomerServiceType = Convert.ToUInt16(dr["SMSToCustomerServiceType"]),
                                        SMSToCustomerPageUrl = Convert.ToString(dr["SMSToCustomerPageUrl"]),
                                        EmailToCustomerMessageBody = dr["EmailToCustomerMessageBody"].ToString(),
                                        EmailToCustomerReplyTo = dr["EmailToCustomerReplyTo"].ToString(),
                                        EmailToCustomerSubject = dr["EmailToCustomerSubject"].ToString(),
                                        SMSToDealerMessage = dr["SMSToDealerMessage"].ToString(),
                                        SMSToDealerNumbers = dr["SMSToDealerNumbers"].ToString(),
                                        SMSToDealerPageUrl = dr["SMSToDealerPageUrl"].ToString(),
                                        SMSToDealerServiceType = Convert.ToUInt16(dr["SMSToDealerServiceType"]),
                                        EmailToDealerMessageBody = dr["EmailToDealerMessageBody"].ToString(),
                                        EmailToDealerReplyTo = dr["EmailToDealerReplyTo"].ToString(),
                                        EmailToDealerSubject = dr["EmailToDealerSubject"].ToString(),
                                        CustomerId = Convert.ToUInt64(dr["CustomerId"]),
                                        CampaignId = Convert.ToString(dr["CampaignId"])
                                    });
                                }
                            }
                        }
                        conn.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Logs.WriteErrorLog("Exception in GetLeadInformation : " + ex.Message);
            }
            return objLeadEntity;
        }   //End of GetLeadInformation Method
        #endregion

        #region SendLeadsToCustDealer Method
        /// <summary>
        /// Modified by :   Sumit Kate on 14 Jan 2016
        /// Description :   No notification is sent to Dealer if already notified
        /// Modified by :   Sumit Kate on 29 Mar 2016
        /// Description :   Pass CampaignId to AutoBizAdaptor.PushInquiryInAB
        /// </summary>
        internal void SendLeadsToCustDealer()
        {
            try
            {
                // Get lead info
                IEnumerable<LeadNotificationEntity> objLeads = GetLeadInformation();
                SMSCommon objLead = new SMSCommon();
                LeadSMSDAL objSmsDal = new LeadSMSDAL();
                Bikewale.Notifications.SendMails sendEmail = new SendMails();

                if (objLeads != null)
                {
                    foreach (LeadNotificationEntity item in objLeads)
                    {
                        Logs.WriteInfoLog(string.Format("process Lead for pqId {0}, Dealer Id : {1}", item.PQId, item.DealerId));
                        if (!String.IsNullOrEmpty(item.SMSToCustomerMessage))
                        {
                            uint smsId = objSmsDal.InsertSMS(item.SMSToCustomerNumbers, item.SMSToCustomerMessage, item.SMSToCustomerServiceType, string.Empty, true);
                            objLead.PushSMSInQueue(smsId, item.SMSToCustomerMessage, item.SMSToCustomerNumbers);

                            sendEmail.SendMail(item.CustomerEmail, item.EmailToCustomerSubject, item.EmailToCustomerMessageBody, item.EmailToCustomerReplyTo.Split(',')[0]);
                        }

                        if (!String.IsNullOrEmpty(item.SMSToDealerMessage))
                        {
                            string[] dealerMobiles = item.SMSToDealerNumbers.Split(',');
                            bool isDealerNotified = objSmsDal.IsDealerNotified(item.DealerId, item.CustomerMobile, item.CustomerId);
                            if (!isDealerNotified)
                            {
                                foreach (string mobileNo in dealerMobiles)
                                {
                                    uint smsId = objSmsDal.InsertSMS(mobileNo.Trim(), item.SMSToDealerMessage, item.SMSToDealerServiceType, string.Empty, true);
                                    objLead.PushSMSInQueue(smsId, item.SMSToDealerMessage, mobileNo.Trim());
                                }

                                string[] dealerEmails = item.EmailToCustomerReplyTo.Split(',');

                                foreach (string email in dealerEmails)
                                {
                                    sendEmail.SendMail(email, item.EmailToDealerSubject, item.EmailToDealerMessageBody, item.EmailToDealerReplyTo);
                                }
                            }
                        }

                        //SendMail(string email, string subject, string body, string replyTo)
                        AutoBizAdaptor.PushInquiryInAB(item.DealerId.ToString(), item.PQId, item.CustomerName, item.CustomerMobile, item.CustomerEmail, item.BikeVersionId.ToString(), item.CityId.ToString(),item.CampaignId);
                        objSmsDal.UpdatePQLeadNotifiedFlag(item.PQId);
                    }
                }
            }
            catch (Exception ex)
            {
                Logs.WriteErrorLog("Exception in SendSMS Method : " + ex.Message);
            }
        }
        #endregion
    }   //End of class
}   //End of namespace

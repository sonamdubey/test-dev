
using MySql.CoreDAL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Notifications.NotificationDAL
{
    internal class SavePQNotification
    {
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
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    if (pqId > 0)
                    {
                        cmd.CommandText = "savepqleadsmstodealer";
                        cmd.CommandType = CommandType.StoredProcedure;

                        //cmd.Parameters.Add("@PQId", SqlDbType.BigInt).Value = pqId;
                        //cmd.Parameters.Add("@SMSToDealerMessage", SqlDbType.VarChar, -1).Value = message;
                        //cmd.Parameters.Add("@SMSToDealerNumbers", SqlDbType.VarChar, 100).Value = dealerMobileNo;
                        //cmd.Parameters.Add("@SMSToDealerServiceType", SqlDbType.TinyInt).Value = smsType;
                        //cmd.Parameters.Add("@SMSToDealerPageUrl", SqlDbType.VarChar, 500).Value = pageUrl;
                            // LogLiveSps.LogSpInGrayLog(cmd);
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_pqid", DbType.Int64, pqId));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_smstodealermessage", DbType.String, message));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_smstodealernumbers", DbType.String, 100, dealerMobileNo));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_smstodealerservicetype", DbType.Byte, smsType));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_smstodealerpageurl", DbType.String, 500, smsType));


                        MySqlDatabase.ExecuteNonQuery(cmd, ConnectionType.ReadOnly);
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
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    if (pqId > 0)
                    {
                        cmd.CommandText = "savepqleadsmstocustomer";
                        cmd.CommandType = CommandType.StoredProcedure;

                        //cmd.Parameters.Add("@PQId", SqlDbType.BigInt).Value = pqId;
                        //cmd.Parameters.Add("@SMSTocustomerMessage", SqlDbType.VarChar).Value = message;
                        //cmd.Parameters.Add("@SMSToCustomerNumbers", SqlDbType.VarChar, 100).Value = customerMobile;
                        //cmd.Parameters.Add("@SMSToCustomerServiceType", SqlDbType.TinyInt).Value = smsType;
                        //cmd.Parameters.Add("@SMSToCustomerPageUrl", SqlDbType.VarChar, 500).Value = pageUrl;
                            // LogLiveSps.LogSpInGrayLog(cmd);
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_pqid", DbType.Int64, pqId));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_smstocustomermessage", DbType.String, message));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_smstocustomernumbers", DbType.String, 100, customerMobile));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_smstocustomerservicetype", DbType.Byte, smsType));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_smstocustomerpageurl", DbType.String, 500, smsType));

                        MySqlDatabase.ExecuteNonQuery(cmd, ConnectionType.ReadOnly);
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
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    if (pqId > 0)
                    {
                        cmd.CommandText = "savepqleademailtodealer";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_pqid", DbType.Int64, pqId));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_emailtodealermessagebody", DbType.String, 10000, emailBody));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_emailtodealersubject", DbType.String, 500, emailsubject));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_emailtodealerreplyto", DbType.String, 200, dealerEmail));
                            // LogLiveSps.LogSpInGrayLog(cmd);
                        MySqlDatabase.ExecuteNonQuery(cmd, ConnectionType.ReadOnly);
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
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    if (pqId > 0)
                    {
                        cmd.CommandText = "savepqleademailtocustomer";
                        cmd.CommandType = CommandType.StoredProcedure;

                        //cmd.Parameters.Add("@PQId", SqlDbType.BigInt).Value = pqId;
                        //cmd.Parameters.Add("@emailtocustomermessagebody", SqlDbType.VarChar).Value = emailBody;
                        //cmd.Parameters.Add("@EmailToCustomerSubject", SqlDbType.VarChar, 500).Value = emailSubject;
                        //cmd.Parameters.Add("@EmailToCustomerReplyTo", SqlDbType.VarChar, 200).Value = customerEmail;
                            
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_pqid", DbType.Int64, pqId));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_emailtocustomermessagebody", DbType.String, emailBody));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_emailtocustomersubject", DbType.String, 500, emailSubject));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_emailtocustomerreplyto", DbType.String, 200, customerEmail));
// LogLiveSps.LogSpInGrayLog(cmd);

                        MySqlDatabase.ExecuteNonQuery(cmd, ConnectionType.ReadOnly);
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

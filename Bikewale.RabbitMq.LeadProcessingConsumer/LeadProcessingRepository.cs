
using Consumer;
using MySql.CoreDAL;
using System;
using System.Data;
using System.Data.Common;
namespace Bikewale.RabbitMq.LeadProcessingConsumer
{
    /// <summary>
    /// Created by  :   Sumit Kate on 24 Feb 2017
    /// Desription  :   LeadProcessingRepository contains required functions to perform Db operations
    /// </summary>
    internal class LeadProcessingRepository
    {
        /// <summary>
        /// Created By : Sadhana Upadhyay on 3 Nov 2014
        /// Summary : To update ispushedtoab flag in pq_newbikedealerpricequote
        /// </summary>
        /// <param name="pqId"></param>
        /// <returns></returns>
        public bool PushedToAB(uint pqId, uint abInquiryId, UInt16 retryCount)
        {
            bool isSuccess = false;

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandText = "ispushedtoab_24022017";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_pqid", DbType.Int64, pqId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_abinquiryid", DbType.Int64, abInquiryId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_leadPushRetries", DbType.Int16, retryCount));
                    if (Convert.ToBoolean(MySqlDatabase.UpdateQuery(cmd, ConnectionType.MasterDatabase)))
                        isSuccess = true;
                }
            }
            catch (Exception ex)
            {
                Logs.WriteErrorLog(String.Format("Error in PushedToAB({0},{1}) : Msg : {2}", pqId, abInquiryId, ex.Message));
                isSuccess = false;
            }

            return isSuccess;
        }

        /// <summary>
        /// Created By : Sushil Kumar on 29th Nov 2016
        /// Description : To update dealer daily limit count  
        /// </summary>
        /// <param name="dealerId"></param>
        /// <param name="abInquiryId"></param>
        public bool UpdateDealerDailyLeadCount(uint campaignId, uint abInquiryId)
        {
            bool isUpdateDealerCount = false;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandText = "updatedealerdailyleadcount";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_campaignid", DbType.Int32, campaignId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_abinquiryid", DbType.Int32, abInquiryId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_isupdatedealercount", DbType.Boolean, ParameterDirection.Output));

                    MySqlDatabase.ExecuteNonQuery(cmd, ConnectionType.MasterDatabase);

                    isUpdateDealerCount = Convert.ToBoolean(!Convert.IsDBNull(cmd.Parameters["par_isupdatedealercount"].Value) ? cmd.Parameters["par_isupdatedealercount"].Value : false);

                }
            }
            catch (Exception ex)
            {
                Logs.WriteErrorLog(String.Format("Error in UpdateDealerDailyLeadCount({0},{1}) : Msg : {2}", campaignId, abInquiryId, ex.Message));
            }
            return isUpdateDealerCount;
        }
    }
}

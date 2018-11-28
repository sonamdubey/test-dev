using Bikewale.Interfaces.Used;
using Bikewale.Notifications;
using Bikewale.Utility;
using MySql.CoreDAL;
using System;
using System.Data;
using System.Data.Common;

namespace Bikewale.DAL.Used
{
    /// <summary>
    /// Created by  :   Sumit Kate on 01 Sep 2016
    /// Description :   Used Bike Buyer Repository
    /// </summary>
    public class UsedBikeBuyerRepository : IUsedBikeBuyerRepository
    {
        /// <summary>
        /// Created by  :   Sumit Kate on 01 Sep 2016
        /// Description :   Checks Buyers eligibility
        /// </summary>
        /// <param name="mobile"></param>
        /// <returns></returns>
        public bool IsBuyerEligible(string mobile)
        {
            bool status = false;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("classified_restricbuyer_sp"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_requestdate", DbType.DateTime, DateTime.Today));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_mobile", DbType.String, 10, mobile));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_status", DbType.Boolean, ParameterDirection.Output));

                    MySqlDatabase.ExecuteNonQuery(cmd, ConnectionType.MasterDatabase);

                    status = Convert.ToBoolean(cmd.Parameters["par_status"].Value);
                }
            }
            catch (Exception err)
            {
                ErrorClass.LogError(err, "UsedBikeBuyerRepository.IsBuyerEligible");

            }
            return status;
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 01 Sep 2016
        /// Description :   Saves the photo request into Database
        /// </summary>
        /// <param name="sellInquiryId"></param>
        /// <param name="buyerId"></param>
        /// <param name="consumerType"></param>
        /// <param name="buyerMessage"></param>
        /// <returns></returns>
        public bool UploadPhotosRequest(string sellInquiryId, UInt64 buyerId, byte consumerType, string buyerMessage)
        {
            bool isDone = false;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("classified_uploadphotosrequest_sp"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_sellinquiryid", DbType.Int64, sellInquiryId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_buyerid", DbType.Int64, Convert.ToUInt64(buyerId)));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_consumertype", DbType.Byte, consumerType));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_buyermessage", DbType.String, 200, buyerMessage));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_clientip", DbType.String, 40, CurrentUser.GetClientIP()));
                    isDone = MySqlDatabase.ExecuteNonQuery(cmd, ConnectionType.MasterDatabase) > 0 ? true : false;

                }
            }
            catch (Exception err)
            {
                ErrorClass.LogError(err, String.Format("UploadPhotosRequest({0},{1},{2},{3})", sellInquiryId, buyerId, consumerType, buyerMessage));

            }
            return isDone;
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 01 Sep 2016
        /// Description :   tells if a particular customer has shown request for a particular Bike?
        /// </summary>
        /// <param name="isDealer"></param>
        /// <param name="inquiryId"></param>
        /// <param name="buyerId"></param>
        /// <returns></returns>
        public bool HasShownInterestInUsedBike(bool isDealer, string inquiryId, UInt64 buyerId)
        {
            bool shownInterest = false;
            string sql = "";

            if (!isDealer) // if it's an individual's Bike
            {
                sql = " select id as requestid  from classifiedrequests where sellinquiryid=@inquiryid and customerid=@customerid";
            }

            try
            {
                if (!String.IsNullOrEmpty(sql))
                {
                    using (DbCommand cmd = DbFactory.GetDBCommand(sql))
                    {
                        cmd.Parameters.Add(DbFactory.GetDbParam("@inquiryid", DbType.Int64, inquiryId));
                        cmd.Parameters.Add(DbFactory.GetDbParam("@customerid", DbType.Int64, buyerId));

                        using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                        {
                            if (dr != null && dr.Read())
                            {
                                shownInterest = true;
                                dr.Close();
                            }
                        }
                    }
                }
            }
            catch (Exception err)
            {
                ErrorClass.LogError(err, String.Format("HasShownInterestInUsedBike({0},{1},{2})", isDealer, inquiryId, buyerId));

            }
            return shownInterest;
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 27 Sep 2016
        /// Description :   Checks if photo request is done
        /// </summary>
        /// <param name="sellInquiryId"></param>
        /// <param name="buyerId"></param>
        /// <param name="isDealer"></param>
        /// <returns></returns>
        public bool IsPhotoRequestDone(string sellInquiryId, UInt64 buyerId, bool isDealer)
        {
            bool isDone = false;

            string sql = "";
            sql = "select sellinquiryid from classified_uploadphotosrequest where sellinquiryid = @par_sellinquiryid and buyerid = @par_buyerid and consumertype = @par_consumertype ";

            string consumerType = isDealer ? "1" : "2";
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand(sql))
                {
                    cmd.Parameters.Add(DbFactory.GetDbParam("@par_sellinquiryid", DbType.Int64, sellInquiryId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("@par_buyerid", DbType.Int64, buyerId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("@par_consumertype", DbType.Byte, consumerType));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null && dr.Read())
                        {
                            isDone = true;
                            dr.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("IsPhotoRequestDone({0},{1},{2})", sellInquiryId, buyerId, isDealer));

            }

            return isDone;
        }
    }
}

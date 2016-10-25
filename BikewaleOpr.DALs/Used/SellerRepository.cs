using Bikewale.Entities.Customer;
using Bikewale.Entities.Used;
using Bikewale.Notifications;
using Bikewale.Utility;
using BikewaleOpr.Interface.Used;
using MySql.CoreDAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace BikewaleOpr.Used
{
    /// <summary>
    /// Created By: Aditi Srivastava on 18 Oct 2016
    /// Description: Used Bike Seller Repository
    /// </summary>
    public class SellerRepository : ISellerRepository
    {
        /// <summary>
        /// Created By: Aditi Srivastava on 18 Oct 2016
        /// Description: Get used bike seller details
        /// </summary>
        /// <param name="inquiryId"></param>
        /// <param name="isDealer"></param>
        /// <returns></returns>
        public UsedBikeSellerBase GetSellerDetails(int inquiryId, bool isDealer)
        {
            UsedBikeSellerBase seller = null;
            try
            {
                if (!isDealer)
                {
                    using (DbCommand cmd = DbFactory.GetDBCommand("classified_getsellerdetails"))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_inquiryid", DbType.Int32, inquiryId));

                        using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.MasterDatabase))
                        {
                            if (dr != null && dr.Read())
                            {
                                seller = new UsedBikeSellerBase();
                                seller.Details = new CustomerEntityBase();
                                seller.Details.CustomerId = !Convert.IsDBNull(dr["SellerId"]) ? Convert.ToUInt64(dr["SellerId"]) : default(UInt64);
                                seller.Details.CustomerName = !Convert.IsDBNull(dr["SellerName"]) ? Convert.ToString(dr["SellerName"]) : string.Empty;
                                seller.Details.CustomerMobile = !Convert.IsDBNull(dr["Contact"]) ? Convert.ToString(dr["Contact"]) : string.Empty;
                                seller.Details.CustomerEmail = !Convert.IsDBNull(dr["selleremail"]) ? Convert.ToString(dr["selleremail"]) : string.Empty;
                                seller.Address = !Convert.IsDBNull(dr["selleraddress"]) ? Convert.ToString(dr["selleraddress"]) : string.Empty;
                                dr.Close();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "GetSellerDetails" + inquiryId);
                objErr.SendMail();
            }
            return seller;
        }
        /// <summary>
        /// Created by:Sangram Nandkhile on 24 Oct 2016
        /// Desc: Get used bikes edited inquiries
        /// </summary>
        public IEnumerable<SellBikeAd> GetClassifiedPendingInquiries()
        {
            List<SellBikeAd> sellerListing = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getclassifiedpendinginquiries"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    //cmd.Parameters.Add(DbFactory.GetDbParam("par_inquiryid", DbType.Int32, inquiryId));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.MasterDatabase))
                    {
                        sellerListing = new List<SellBikeAd>();
                        if (dr != null)
                        {
                            while (dr.Read()) 
                            {
                                SellBikeAd ad = new SellBikeAd();
                                ad.Version = new Bikewale.Entities.BikeData.BikeVersionEntityBase();
                                ad.InquiryId = SqlReaderConvertor.ToUInt32(dr["InquiryId"]);
                                ad.ProfileId = Convert.ToString(dr["ProfileId"]);
                                ad.Version = new Bikewale.Entities.BikeData.BikeVersionEntityBase();
                                ad.Version.VersionName = Convert.ToString(dr["BikeName"]);
                                ad.KiloMeters = SqlReaderConvertor.ToUInt32(dr["Kilometers"]);
                                ad.Expectedprice = SqlReaderConvertor.ToUInt64(dr["Price"]);
                                ad.ManufacturingYear = SqlReaderConvertor.ToDateTime(dr["MakeYear"]);
                                ad.PhotoCount = SqlReaderConvertor.ToUInt16(dr["PhotoCount"]);
                                sellerListing.Add(ad);
                            }
                            dr.Close();
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "GetClassifiedPendingInquiries");
                objErr.SendMail();
            }
            return sellerListing;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="inquiryId"></param>
        /// <param name="isApproved"></param>
        /// <param name="approvedBy"></param>
        /// <returns></returns>
        public bool SaveEditedInquiry(uint inquiryId, short isApproved, int approvedBy)
        {
            bool isSuccess = false;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("saveapprovallog"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_inquiryid", DbType.Int32, inquiryId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_isapproved", DbType.Int32, isApproved));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_approvedby", DbType.Int32, approvedBy));
                    if (Convert.ToBoolean(MySqlDatabase.ExecuteNonQuery(cmd, ConnectionType.MasterDatabase)))
                    {
                        isSuccess = true;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, String.Format("SaveEditedInquiry: InquiryId:{0}, IsApproved:{1}, Approvedby{2}",inquiryId,isApproved,approvedBy));
                objErr.SendMail();
            }
            return isSuccess;
        }

    }
}

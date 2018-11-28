using Bikewale.Entities.Used;
using Bikewale.Interfaces.Used;
using Bikewale.Notifications;
using Bikewale.Utility;
using MySql.CoreDAL;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Common;

namespace Bikewale.DAL.Used
{
    /// <summary>
    /// Created by  :   Sumit Kate on 01 Sep 2016
    /// Description :   Used Bike Seller Repository
    /// </summary>
    public class UsedBikeSellerRepository : IUsedBikeSellerRepository
    {
        /// <summary>
        /// Created by  :   Sumit Kate on 01 Sep 2016
        /// Description :   Returns the sellers info of a classified listing
        /// </summary>
        /// <param name="inquiryId"></param>
        /// <param name="isDealer"></param>
        /// <returns></returns>
        public Entities.Used.UsedBikeSellerBase GetSellerDetails(string inquiryId, bool isDealer)
        {
            Entities.Used.UsedBikeSellerBase seller = null;
            try
            {
                if (!isDealer)
                {
                    using (DbCommand cmd = DbFactory.GetDBCommand("classified_getsellerdetails"))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_inquiryid", DbType.Int32, Convert.ToInt32(inquiryId)));

                        using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                        {
                            if (dr != null && dr.Read())
                            {
                                seller = new Entities.Used.UsedBikeSellerBase();
                                seller.Details = new Entities.Customer.CustomerEntityBase();
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
                ErrorClass.LogError(ex, "GetSellerDetails" + inquiryId);

            }
            return seller;
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 01 Sep 2016
        /// Description :   Saves used bike customer inquiry request
        /// Modified by :   Sumit Kate on 23 Sep 2016
        /// Description :   save sourceid(1-Desktop/2-Mobile/3-Android) for tracking the source of the lead
        /// added isnew parameter
        /// </summary>
        /// <param name="inquiryId"></param>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public int SaveCustomerInquiry(string inquiryId, ulong customerId, UInt16 sourceId, out bool isNew)
        {
            int inqId = 0;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("insertclassifiedrequests_23092016"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_sourceid", DbType.SByte, sourceId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_sellinquiryid", DbType.Int64, inquiryId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_customerid", DbType.Int64, customerId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_requestdatetime", DbType.DateTime, DateTime.Now));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_comments", DbType.String, 500, ""));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_clientip", DbType.String, 40, CurrentUser.GetClientIP()));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_inquiryid", DbType.Int64, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_isnew", DbType.Boolean, ParameterDirection.Output));
                    MySqlDatabase.ExecuteNonQuery(cmd, ConnectionType.MasterDatabase);
                    inqId = Utility.SqlReaderConvertor.ToInt32(cmd.Parameters["par_inquiryid"].Value);
                    isNew = Utility.SqlReaderConvertor.ToBoolean(cmd.Parameters["par_isnew"].Value);
                }
            }
            catch (Exception err)
            {
                isNew = false;
                ErrorClass.LogError(err, String.Format("SubmitInquiry({0}{1})", inquiryId, customerId));

            }

            return inqId;
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 01 Sep 2016
        /// Description :   Returns the Min Inquiry Details
        /// Modified by :   Sumit Kate on 29 Sep 2016
        /// Description :   Populate citymaskingname, makemaskingname and modelmaskingname
        /// </summary>
        /// <param name="inquiryId"></param>
        /// <returns></returns>
        public ClassifiedInquiryDetailsMin GetInquiryDetails(string inquiryId)
        {
            ClassifiedInquiryDetailsMin objInquiry = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("classified_getminprofiledetails"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_inquiryid", DbType.Int32, inquiryId));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.MasterDatabase))
                    {
                        if (dr != null && dr.Read())
                        {
                            objInquiry = new ClassifiedInquiryDetailsMin();
                            objInquiry.BikeName = Convert.ToString(dr["bikeName"]);
                            objInquiry.City = Convert.ToString(dr["city"]);
                            objInquiry.KmsDriven = !Convert.IsDBNull(dr["kilometers"]) ? Convert.ToUInt32(dr["kilometers"]) : default(uint);
                            objInquiry.MakeYear = !Convert.IsDBNull(dr["MakeYear"]) ? Convert.ToDateTime(dr["MakeYear"]) : DateTime.Now;
                            objInquiry.Price = !Convert.IsDBNull(dr["Price"]) ? Convert.ToUInt32(dr["Price"]) : default(uint);
                            objInquiry.Seller = new Entities.Customer.CustomerEntityBase()
                            {
                                CustomerName = Convert.ToString(dr["sellername"]),
                                CustomerEmail = Convert.ToString(dr["selleremail"]),
                                CustomerMobile = Convert.ToString(dr["sellermobile"])
                            };
                            objInquiry.CityMaskingName = Convert.ToString(dr["citymaskingname"]);
                            objInquiry.MakeMaskingName = Convert.ToString(dr["makemaskingname"]);
                            objInquiry.ModelMaskingName = Convert.ToString(dr["modelmaskingname"]);
                        }
                    }
                }
            }
            catch (Exception err)
            {
                ErrorClass.LogError(err, String.Format("GetInquiryDetails({0})", inquiryId));

            }

            return objInquiry;
        }
        /// <summary>
        /// Created By  : Aditi Srivastava on 27 Oct 2016
        /// Description : Function to delete used bikes photos
        /// </summary>
        /// <param name="inquiryId"></param>
        /// <param name="photoId"></param>
        /// <returns></returns>
        public bool RemoveBikePhotos(int inquiryId, string photoId)
        {
            bool isRemoved = false;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("classified_bikephotos_remove"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_inquiryid", DbType.Int64, inquiryId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_photoid", DbType.Int64, photoId));

                    MySqlDatabase.ExecuteNonQuery(cmd, ConnectionType.MasterDatabase);

                    isRemoved = true;
                }
            }
            catch (Exception err)
            {
                ErrorClass.LogError(err, String.Format("RemoveBikePhotos({0},{1})", inquiryId, photoId));

            }
            return isRemoved;
        }
        /// <summary>
        /// Created by: Sangram Nandkhile on 25 Nov 2016
        /// Desc: To repost ad listing
        /// </summary>
        /// <returns></returns>
        public bool RepostSellBikeAd(int inquiryId, ulong customerId)
        {
            bool isPosted = false;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("classified_repostlisting"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_inquiryid", DbType.Int32, inquiryId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_userid", DbType.Int64, customerId));
                    MySqlDatabase.ExecuteNonQuery(cmd, ConnectionType.MasterDatabase);
                    isPosted = true;
                }
            }
            catch (Exception err)
            {
                ErrorClass.LogError(err, String.Format("UsedBikeSellerRepository.RepostSellBikeAd(inquiryId: {0},customerId: {1})", inquiryId, customerId));

            }
            return isPosted;
        }
        /// <summary>
        /// Created by : sajal gupta on 25-11-2016
        /// Desc : To fetch liting details from db.
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public IEnumerable<CustomerListingDetails> GetCustomerListingDetails(uint customerId)
        {
            ICollection<CustomerListingDetails> objDetailsList = null;
            CustomerListingDetails objDetails = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getclassifiedindividuallistings_sp_25112016"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_customerid", DbType.Int32, customerId));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            objDetailsList = new Collection<CustomerListingDetails>();

                            while (dr.Read())
                            {
                                objDetails = new CustomerListingDetails();
                                objDetails.Photo = new BikePhoto();

                                objDetails.BikeName = Convert.ToString(dr["bike"]);
                                objDetails.InquiryId = SqlReaderConvertor.ToInt32(dr["inquiryid"]);
                                objDetails.StatusId = SqlReaderConvertor.ToUInt16(dr["statusid"]);
                                objDetails.CityMaskingName = Convert.ToString(dr["citymaskingname"]);
                                objDetails.MakeMaskingName = Convert.ToString(dr["makemaskingname"]);
                                objDetails.ModelMaskingName = Convert.ToString(dr["modelmaskingname"]);
                                objDetails.TotalViews = SqlReaderConvertor.ToUInt16(dr["totalviews"]);
                                objDetails.Color = Convert.ToString(dr["color"]);
                                objDetails.IsApproved = SqlReaderConvertor.ToBoolean(dr["isapproved"]);
                                objDetails.Owner = SqlReaderConvertor.ToUInt16(dr["owner"]);
                                objDetails.DaysRemaining = SqlReaderConvertor.ToUInt16(dr["daysafterlastupdated"]);
                                objDetails.SellerType = SqlReaderConvertor.ToUInt16(dr["sellertype"]);
                                objDetails.Photo.HostUrl = Convert.ToString(dr["hosturl"]);
                                objDetails.Photo.OriginalImagePath = Convert.ToString(dr["originalimagepath"]);
                                objDetails.AskingPrice = SqlReaderConvertor.ToUInt32(dr["price"]);
                                objDetails.ModelYear = SqlReaderConvertor.ToDateTime(dr["makeyear"]);
                                objDetails.KmsDriven = SqlReaderConvertor.ToUInt32(dr["kilometers"]);
                                objDetails.RegisteredAt = Convert.ToString(dr["registrationplace"]);
                                objDetails.EntryDate = SqlReaderConvertor.ToDateTime(dr["entrydate"]);

                                objDetailsList.Add(objDetails);
                            }
                        }
                    }
                }
            }
            catch (Exception err)
            {
                ErrorClass.LogError(err, String.Format("GetCustomerListingDetails({0})", customerId));

            }

            return objDetailsList;
        }
    }
}

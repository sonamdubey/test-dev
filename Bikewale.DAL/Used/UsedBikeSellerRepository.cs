using Bikewale.CoreDAL;
using Bikewale.Entities.Used;
using Bikewale.Interfaces.Used;
using Bikewale.Notifications;
using MySql.CoreDAL;
using System;
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
                ErrorClass objErr = new ErrorClass(ex, "GetSellerDetails" + inquiryId);
                objErr.SendMail();
            }
            return seller;
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 01 Sep 2016
        /// Description :   Saves used bike customer inquiry request
        /// </summary>
        /// <param name="inquiryId"></param>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public string SaveCustomerInquiry(string inquiryId, string customerId)
        {
            string inqId = "-1";
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("insertclassifiedrequests"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_sellinquiryid", DbType.Int64, inquiryId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_customerid", DbType.Int64, customerId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_requestdatetime", DbType.DateTime, DateTime.Now));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_comments", DbType.String, 500, ""));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_inquiryid", DbType.Int64, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_clientip", DbType.String, 40, CommonOpn.GetClientIP()));
                    MySqlDatabase.ExecuteNonQuery(cmd, ConnectionType.MasterDatabase);
                    inqId = cmd.Parameters["par_inquiryid"].Value.ToString();
                }
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, String.Format("SubmitInquiry({0}{1})", inquiryId, customerId));
                objErr.SendMail();
            }

            return inqId;
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 01 Sep 2016
        /// Description :   Returns the Min Inquiry Details
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
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_inquiryid", DbType.Int32, inquiryId));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
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
                        }
                    }
                }
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, String.Format("GetInquiryDetails({0})", inquiryId));
                objErr.SendMail();
            }

            return objInquiry;
        }
    }
}

using Bikewale.Entities.Used;
using Bikewale.Entities.Customer;
using BikewaleOpr.Interface.Used;
using MySql.CoreDAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bikewale.Notifications;

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
    }
}

using Bikewale.Utility;
using Consumer;
using MySql.CoreDAL;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Common;

namespace Bikewale.ExpiringListingReminder
{
    public class ExpiringListingSellerDetailsRepository
    {
        /// <summary>
        /// Created By Sajal Gupta on 23-11-2016.
        /// Desc : Send Lists of seller data for expiry listing.
        /// </summary>
        public SellerDetailsListsEntity getExpiringListings()
        {
            SellerDetailsListsEntity objSellerDetailsListsEntity = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "classified_getexpiringlistings";

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.MasterDatabase))
                    {
                        if (dr != null)
                        {
                            objSellerDetailsListsEntity = new SellerDetailsListsEntity();
                            ICollection<SellerDetailsEntity> objListSevenDays = new Collection<SellerDetailsEntity>();
                            ICollection<SellerDetailsEntity> objListOneDays = new Collection<SellerDetailsEntity>();

                            while (dr.Read())
                            {
                                SellerDetailsEntity seller = new SellerDetailsEntity();

                                seller.daysToExpire = 7;
                                seller.inquiryId = Convert.ToString(dr["Inquiryid"]);
                                seller.makeName = Convert.ToString(dr["MakeName"]);
                                seller.makeId = SqlReaderConvertor.ToInt32(dr["MakeId"]);
                                seller.customerId = SqlReaderConvertor.ToInt32(dr["CustomerId"]);
                                seller.modelName = Convert.ToString(dr["ModelName"]);
                                seller.modelId = SqlReaderConvertor.ToInt32(dr["ModelId"]);
                                seller.sellerName = Convert.ToString(dr["CustomerName"]);
                                seller.number = Convert.ToString(dr["CustomerMobile"]);
                                seller.sellerEmail = Convert.ToString(dr["CustomerEmail"]);

                                objListSevenDays.Add(seller);
                            }

                            if (dr.NextResult() && dr.Read())
                            {
                                while (dr.Read())
                                {
                                    SellerDetailsEntity seller = new SellerDetailsEntity();

                                    seller.daysToExpire = 1;
                                    seller.inquiryId = Convert.ToString(dr["Inquiryid"]);
                                    seller.makeName = Convert.ToString(dr["MakeName"]);
                                    seller.makeId = SqlReaderConvertor.ToInt32(dr["MakeId"]);
                                    seller.customerId = SqlReaderConvertor.ToInt32(dr["CustomerId"]);
                                    seller.modelName = Convert.ToString(dr["ModelName"]);
                                    seller.modelId = SqlReaderConvertor.ToInt32(dr["ModelId"]);
                                    seller.sellerName = Convert.ToString(dr["CustomerName"]);
                                    seller.number = Convert.ToString(dr["CustomerMobile"]);
                                    seller.sellerEmail = Convert.ToString(dr["CustomerEmail"]);

                                    objListOneDays.Add(seller);
                                }
                            }

                            objSellerDetailsListsEntity.sellerDetailsSevenDaysRemaining = objListSevenDays;
                            objSellerDetailsListsEntity.sellerDetailsOneDayRemaining = objListOneDays;

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                SendMail.HandleException(ex, "ExpiringListingSellerDetailsRepository");
            }

            return objSellerDetailsListsEntity;
        }
    }
}

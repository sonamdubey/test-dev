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
        public SellerDetailsListEntity getExpiringListings()
        {
            SellerDetailsListEntity objSellerDetailsListsEntity = null;
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
                            objSellerDetailsListsEntity = new SellerDetailsListEntity();
                            ICollection<SellerDetailsEntity> objListSevenDays = new Collection<SellerDetailsEntity>();

                            while (dr.Read())
                            {

                                objListSevenDays.Add(new SellerDetailsEntity()
                                {
                                    inquiryId = SqlReaderConvertor.ToUInt16(dr["Inquiryid"]),
                                    makeName = Convert.ToString(dr["MakeName"]),
                                    modelName = Convert.ToString(dr["ModelName"]),
                                    sellerName = Convert.ToString(dr["CustomerName"]),
                                    sellerMobileNumber = Convert.ToString(dr["CustomerMobile"]),
                                    sellerEmail = Convert.ToString(dr["CustomerEmail"])
                                });
                            }

                            objSellerDetailsListsEntity.sellerDetailsSevenDaysRemaining = objListSevenDays;

                            if (dr.NextResult())
                            {
                                ICollection<SellerDetailsEntity> objListOneDays = new Collection<SellerDetailsEntity>();

                                while (dr.Read())
                                {
                                    objListOneDays.Add(new SellerDetailsEntity()
                                    {
                                        inquiryId = SqlReaderConvertor.ToUInt16(dr["Inquiryid"]),
                                        makeName = Convert.ToString(dr["MakeName"]),
                                        modelName = Convert.ToString(dr["ModelName"]),
                                        sellerName = Convert.ToString(dr["CustomerName"]),
                                        sellerMobileNumber = Convert.ToString(dr["CustomerMobile"]),
                                        sellerEmail = Convert.ToString(dr["CustomerEmail"])
                                    });
                                }

                                objSellerDetailsListsEntity.sellerDetailsOneDayRemaining = objListOneDays;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logs.WriteErrorLog("Exception in getExpiringListings : " + ex.Message);
                SendMail.HandleException(ex, "ExpiringListingSellerDetailsRepository.getExpiringListings");
            }

            return objSellerDetailsListsEntity;
        }
    }
}

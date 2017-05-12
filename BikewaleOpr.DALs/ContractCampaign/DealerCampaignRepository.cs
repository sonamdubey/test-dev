﻿using Bikewale.Notifications;
using Bikewale.Utility;
using BikewaleOpr.Entities;
using BikewaleOpr.Entity.ContractCampaign;
using BikewaleOpr.Interface.ContractCampaign;
using MySql.CoreDAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
namespace BikewaleOpr.DALs.ContractCampaign
{
    /// <summary>
    /// Created by  :   Sumit Kate on 29 Dec 2016
    /// Description :   DealerCampaign Repository
    /// </summary>
    public class DealerCampaignRepository : IDealerCampaignRepository
    {
        /// <summary>
        /// Created by  :   Sumit Kate on 29 Dec 2016
        /// Description :   Fetch Dealer Campaign
        /// </summary>
        /// <param name="campaignId"></param>
        /// <returns></returns>
        public DealerCampaignEntity FetchBWDealerCampaign(uint campaignId)
        {
            DealerCampaignEntity dealerCampaign = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("bw_fetchbwdealercampaign_29122016"))
                {

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_campaignid", DbType.Int32, campaignId));


                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.MasterDatabase))
                    {
                        if (dr != null)
                        {
                            if (dr.Read())
                            {
                                dealerCampaign = new DealerCampaignEntity();
                                dealerCampaign.CampaignId = SqlReaderConvertor.ToInt32(dr["campaignid"]);
                                dealerCampaign.CampaignName = Convert.ToString(dr["dealername"]);
                                dealerCampaign.EmailId = Convert.ToString(dr["dealeremailid"]);
                                dealerCampaign.MaskingNumber = Convert.ToString(dr["number"]);
                                dealerCampaign.ServingRadius = SqlReaderConvertor.ToInt32(dr["dealerleadservingradius"]);
                                dealerCampaign.DailyLeadLimit = SqlReaderConvertor.ToUInt32(dr["dailyleadlimit"]);
                                dealerCampaign.CallToAction = SqlReaderConvertor.ToUInt16(dr["calltoaction"]);
                                dealerCampaign.DealerMobile = Convert.ToString(dr["dealermobile"]);
                            }
                            dr.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, String.Format("DealerCampaignRepository.FetchBWDealerCampaign({0})", campaignId));
            }
            return dealerCampaign;
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 29 Dec 2016
        /// Description :   Fetch Dealer Call To Actions
        /// </summary>
        /// <returns></returns>
        public ICollection<CallToActionEntityBase> FetchDealerCallToActions()
        {
            ICollection<CallToActionEntityBase> callToActions = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("fetchdealercalltoactions"))
                {

                    cmd.CommandType = CommandType.StoredProcedure;
                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.MasterDatabase))
                    {
                        if (dr != null)
                        {
                            callToActions = new List<CallToActionEntityBase>();
                            while (dr.Read())
                            {
                                callToActions.Add(
                                    new CallToActionEntityBase()
                                    {
                                        Id = SqlReaderConvertor.ToUInt16(dr["id"]),
                                        DisplayTextSmall = Convert.ToString(dr["displaytextsmall"]),
                                        DisplayTextLarge = Convert.ToString(dr["displaytextlarge"])
                                    }
                                    );
                            }
                            dr.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "DealerCampaignRepository.FetchDealerCallToActions()");
            }
            return callToActions;
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 29 Dec 2016
        /// Description :   Update Dealer Campaign
        /// Modified by :   Sumit Kate on 12 May 2017
        /// Description :   Modified the SP and removed dealer lead serving radius parameter
        /// </summary>
        /// <param name="isActive"></param>
        /// <param name="campaignId"></param>
        /// <param name="userId"></param>
        /// <param name="dealerId"></param>
        /// <param name="contractId"></param>
        /// <param name="dealerLeadServingRadius"></param>
        /// <param name="maskingNumber"></param>
        /// <param name="dealerName"></param>
        /// <param name="dealerEmailId"></param>
        /// <param name="dailyleadlimit"></param>
        /// <param name="callToAction"></param>
        /// <param name="isBookingAvailable"></param>
        /// <returns></returns>
        public bool UpdateBWDealerCampaign(bool isActive, int campaignId, int userId, int dealerId, int contractId, string maskingNumber, string dealerName, string dealerEmailId, int dailyleadlimit, ushort callToAction, bool isBookingAvailable = false)
        {
            bool isSuccess = false;

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("updatedealercampaign_12052017"))
                {

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_campaignid", DbType.Int32, campaignId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_dealername", DbType.String, 200, dealerName));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_phone", DbType.String, 50, maskingNumber));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_dealeremail", DbType.String, 200, dealerEmailId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_dailyleadlimit", DbType.Int32, dailyleadlimit > 0 ? dailyleadlimit : Convert.DBNull));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_isbookingavailable", DbType.Boolean, isBookingAvailable));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_isactive", DbType.Boolean, isActive));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_updatedby", DbType.Int32, userId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_contractid", DbType.Int32, contractId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_calltoaction", DbType.Int16, callToAction));
                    isSuccess = MySqlDatabase.UpdateQuery(cmd, ConnectionType.MasterDatabase);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "DealerCampaignRepository.UpdateBWDealerCampaign");
                objErr.SendMail();
            }

            return isSuccess;
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 29 Dec 2016
        /// Description :   Insert new Dealer Campaign
        /// Modified by :   Sumit Kate on 12 May 2017
        /// Description :   Modified the SP and removed dealer lead serving radius parameter
        /// </summary>
        /// <param name="isActive"></param>
        /// <param name="userId"></param>
        /// <param name="dealerId"></param>
        /// <param name="contractId"></param>
        /// <param name="dealerLeadServingRadius"></param>
        /// <param name="maskingNumber"></param>
        /// <param name="dealerName"></param>
        /// <param name="dealerEmailId"></param>
        /// <param name="dailyleadlimit"></param>
        /// <param name="callToAction"></param>
        /// <param name="isBookingAvailable"></param>
        /// <returns></returns>
        public int InsertBWDealerCampaign(bool isActive, int userId, int dealerId, int contractId, string maskingNumber, string dealerName, string dealerEmailId, int dailyleadlimit, ushort callToAction, bool isBookingAvailable = false)
        {
            int newCampaignId = 0;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("insertdealercampaign_12052017"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_dealername", DbType.String, 200, dealerName));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_phone", DbType.String, 50, maskingNumber));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_dealeremail", DbType.String, 200, dealerEmailId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_updatedby", DbType.Int32, userId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_isactive", DbType.Boolean, isActive));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_contractid", DbType.Int32, contractId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_dailyleadlimit", DbType.Int32, dailyleadlimit > 0 ? dailyleadlimit : Convert.DBNull));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_isbookingavailable", DbType.Boolean, isBookingAvailable));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_dealerid", DbType.Int32, dealerId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_calltoaction", DbType.Int16, callToAction));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_newcampaignid", DbType.Int32, ParameterDirection.Output));
                    MySqlDatabase.ExecuteNonQuery(cmd, ConnectionType.MasterDatabase);
                    newCampaignId = SqlReaderConvertor.ToInt32(cmd.Parameters["par_newcampaignid"].Value);

                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "DealerCampaignRepository.InsertBWDealerCampaign");
                objErr.SendMail();
            }

            return newCampaignId;
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 18 Jan 2017
        /// Description :   Calls opr_getmakesbydealercity
        /// </summary>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public ICollection<BikeMakeEntityBase> MakesByDealerCity(uint cityId)
        {
            ICollection<BikeMakeEntityBase> callToActions = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("opr_getmakesbydealercity"))
                {

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_cityid", DbType.Int32, cityId));
                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.MasterDatabase))
                    {
                        if (dr != null)
                        {
                            callToActions = new List<BikeMakeEntityBase>();
                            while (dr.Read())
                            {
                                callToActions.Add(
                                    new BikeMakeEntityBase()
                                    {
                                        MakeId = SqlReaderConvertor.ToUInt16(dr["id"]),
                                        MakeName = Convert.ToString(dr["Name"])
                                    }
                                    );
                            }
                            dr.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, String.Format("DealerCampaignRepository.MakesByDealerCity({0})", cityId));
            }
            return callToActions;
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 18 jan 2017
        /// Description :   Calls opr_getdealerbymakecity
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="makeId"></param>
        /// <param name="activecontract"></param>
        /// <returns></returns>
        public ICollection<DealerEntityBase> DealersByMakeCity(uint cityId, uint makeId, bool activecontract)
        {
            ICollection<DealerEntityBase> callToActions = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("opr_getdealerbymakecity"))
                {
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_cityid", DbType.Int32, cityId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_makeid", DbType.Int32, makeId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_activecontract", DbType.Int16, activecontract ? 1 : 0));
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.MasterDatabase))
                    {
                        if (dr != null)
                        {
                            callToActions = new List<DealerEntityBase>();
                            while (dr.Read())
                            {
                                callToActions.Add(
                                    new DealerEntityBase()
                                    {
                                        Id = SqlReaderConvertor.ToUInt16(dr["dealerId"]),
                                        Name = Convert.ToString(dr["Organization"])
                                    }
                                    );
                            }
                            dr.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, String.Format("DealerCampaignRepository.DealersByMakeCity({0},{1},{2})", cityId, makeId, activecontract));
            }
            return callToActions;
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 18 jan 2017
        /// Description :   Call opr_getdealercampaigns
        /// </summary>
        /// <param name="dealerId"></param>
        /// <param name="activecontract"></param>
        /// <returns></returns>
        public ICollection<DealerCampaignDetailsEntity> DealerCampaigns(uint dealerId, bool activecontract)
        {
            ICollection<DealerCampaignDetailsEntity> callToActions = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("opr_getdealercampaigns"))
                {
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_dealerid", DbType.Int32, dealerId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_activecontract", DbType.Int16, activecontract ? 1 : 0));
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.MasterDatabase))
                    {
                        if (dr != null)
                        {
                            callToActions = new List<DealerCampaignDetailsEntity>();
                            while (dr.Read())
                            {
                                callToActions.Add(
                                    new DealerCampaignDetailsEntity()
                                    {
                                        CampaignId = SqlReaderConvertor.ToUInt16(dr["campaignid"]),
                                        CampaignName = Convert.ToString(dr["campaignname"]),
                                        MaskingNumber = Convert.ToString(dr["maskingnumber"]),
                                        EmailId = Convert.ToString(dr["campaignemailid"]),
                                        ServingRadius = SqlReaderConvertor.ToInt32(dr["CampaignLeadServingRadius"]),
                                        ContractID = SqlReaderConvertor.ToUInt32(dr["ContractId"]),
                                        DealerId = SqlReaderConvertor.ToUInt32(dr["DealerId"]),
                                        PackageName = Convert.ToString(dr["packagename"]),
                                        ContractStartDate = SqlReaderConvertor.ToDateTime(dr["startdate"]),
                                        ContractEndDate = SqlReaderConvertor.ToDateTime(dr["enddate"]),
                                        ContractStatus = SqlReaderConvertor.ToUInt32(dr["contractstatus"]),
                                        ContractStatusText = Convert.ToString(dr["status"]),
                                        DailyLeadLimit = SqlReaderConvertor.ToUInt32(dr["dailyleadlimit"]),
                                        DailyLeads = SqlReaderConvertor.ToUInt32(dr["dailyleads"]),
                                        RulesCount = SqlReaderConvertor.ToUInt32(dr["NoOfRules"])
                                    }
                                    );
                            }
                            dr.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, String.Format("DealerCampaignRepository.DealerCampaigns({0},{1})", dealerId, activecontract));
            }
            return callToActions;
        }
    }
}

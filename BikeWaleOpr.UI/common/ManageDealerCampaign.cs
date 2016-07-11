﻿using BikeWaleOpr.Common;
using BikeWaleOPR.Utilities;
using MySql.CoreDAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace BikewaleOpr.Common
{
    /// <summary>
    /// Created by  :   Sumit Kate on 19 Mar 2016
    /// Description :   Manage Dealer Campaign
    /// </summary>
    public class ManageDealerCampaign
    {
        /// <summary>
        /// Created by  :   Sumit Kate on 19 Mar 2016
        /// Description :   Fetch the Dealer Campaign details
        ///                 SP Called : BW_FetchBWDealerCampaign
        /// </summary>
        /// <param name="campaignId">Campaign Id</param>
        /// <returns></returns>
        public DataTable FetchBWDealerCampaign(int campaignId)
        {
            DataTable dtDealerCampaign = null;
            
            try
            {
                if (campaignId > 0)
                {
                    using (DbCommand cmd = DbFactory.GetDBCommand("bw_fetchbwdealercampaign"))
                    {
                        
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_campaignid", DbType.Int32, campaignId));

                        using (DataSet ds = MySqlDatabase.SelectAdapterQuery(cmd, ConnectionType.ReadOnly))
                        {
                            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
                                dtDealerCampaign = ds.Tables[0];
                        }
                    }
                }                
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "ManageDealerCampaign.FetchBWDealerCampaign");
                objErr.SendMail();
            }

            return dtDealerCampaign;
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 19 Mar 2016
        /// Description :   Save the new Dealer Campaign
        ///                 SP Called : BW_InsertBWDealerCampaign
        /// Updated by  :   Sangram Nandkhile on 31st March 2016
        /// Description :   Used out parameter 'NewCampaignId' and changed return type
        /// </summary>
        /// <param name="isActive"></param>
        /// <param name="userId"></param>
        /// <param name="dealerId"></param>
        /// <param name="contractId"></param>
        /// <param name="dealerLeadServingRadius"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="maskingNumber"></param>
        /// <param name="dealerName"></param>
        /// <param name="dealerEmailId"></param>
        /// <param name="isBookingAvailable"></param>
        /// <returns></returns>
        public int InsertBWDealerCampaign(bool isActive,  int userId, int dealerId, int contractId, int dealerLeadServingRadius, string maskingNumber, string dealerName, string dealerEmailId, bool isBookingAvailable = false)
        {
            int newCampaignId = 0;
            try
            {
                 using (DbCommand cmd = DbFactory.GetDBCommand("bw_insertbwdealercampaign"))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_dealerid", DbType.Int32,  dealerId));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_dealername", DbType.String,200,  dealerName));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_phone", DbType.String,50,  maskingNumber));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_dealeremail", DbType.String,200 , dealerEmailId));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_isactive", DbType.Boolean, isActive));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_contractid", DbType.Int32, contractId));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_dealerleadservingradius", DbType.Int32, dealerLeadServingRadius));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_updatedby", DbType.Int32, userId));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_isbookingavailable", DbType.Boolean, isBookingAvailable));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_newcampaignid", DbType.Int32, ParameterDirection.Output));
                        MySqlDatabase.ExecuteNonQuery(cmd, ConnectionType.ReadOnly);
                        newCampaignId = Convert.ToInt32(cmd.Parameters["par_newcampaignid"].Value);

                    }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "ManageDealerCampaign.InsertBWDealerCampaign");
                objErr.SendMail();
            }

            return newCampaignId;
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 19 Mar 2016
        /// Description :   Save the new Dealer Campaign
        ///                 SP Called : BW_UpdateBWDealerCampaign
        /// </summary>
        /// <param name="isActive"></param>
        /// <param name="campaignId"></param>
        /// <param name="userId"></param>
        /// <param name="dealerId"></param>
        /// <param name="contractId"></param>
        /// <param name="dealerLeadServingRadius"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="maskingNumber"></param>
        /// <param name="dealerName"></param>
        /// <param name="dealerEmailId"></param>
        /// <param name="isBookingAvailable"></param>
        /// <returns></returns>
        public bool UpdateBWDealerCampaign(bool isActive, int campaignId, int userId, int dealerId, int contractId, int dealerLeadServingRadius, string maskingNumber, string dealerName, string dealerEmailId, bool isBookingAvailable = false)
        {
            bool isSuccess = false;
            
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("bw_updatebwdealercampaign"))
                {

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_dealerid", DbType.Int32, dealerId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_dealername", DbType.String, 200, dealerName));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_phone", DbType.String, 50, maskingNumber));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_dealeremail", DbType.String, 200, dealerEmailId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_isactive", DbType.Boolean, isActive));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_contractid", DbType.Int32, contractId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_dealerleadservingradius", DbType.Int32, dealerLeadServingRadius));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_updatedby", DbType.Int32, userId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_campaignid", DbType.Int32, campaignId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_isbookingavailable", DbType.Boolean, isBookingAvailable));

                    isSuccess = MySqlDatabase.UpdateQuery(cmd,ConnectionType.MasterDatabase);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "ManageDealerCampaign.InsertBWDealerCampaign");
                objErr.SendMail();
            }

            return isSuccess;
        }

        /// <summary>
        /// Created by  :   Sangram Nandkhile on 22 Mar 2016
        /// Description :   Fetch the Dealer Campaigns for contacts
        ///                 SP Called : BW_FetchBWCampaigns
        /// </summary>
        /// <param name="campaignId">Campaign Id</param>
        /// <returns></returns>
        public DataTable FetchBWCampaigns(int contractId)
        {
            DataTable dtDealerCampaign = null;
            
            try
            {
                if (contractId > 0)
                {
                    using (DbCommand cmd = DbFactory.GetDBCommand("bw_fetchbwcampaigns"))
                    {
                        
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_contractid", DbType.Int32, contractId));

                        using (DataSet ds = MySqlDatabase.SelectAdapterQuery(cmd, ConnectionType.ReadOnly))
                        {
                            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
                                dtDealerCampaign = ds.Tables[0];
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "ManageDealerCampaign.FetchBWCampaigns");
                objErr.SendMail();
            }

            return dtDealerCampaign;
        }

        /// <summary>
        /// Created by  :   Sangram Nandkhile on 04 Apr 2016
        /// Description :   Fetch the Dealer Campaigns for contacts
        ///                 SP Called : GetMaskingNumbers
        /// </summary>
        /// <param name="dealerId"></param>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public DataTable BindMaskingNumbers(int dealerId)
        {
            
            DataTable dtb = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("bw_getmaskingnumbers"))
                {
                    
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_dealerid", DbType.Int32, dealerId));

                    using (DataSet ds = MySqlDatabase.SelectAdapterQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
                            dtb = ds.Tables[0];
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"] + "BindMaskingNumbers");
                objErr.SendMail();
            }

            return dtb;
        }

        void AutoPauseCampaign(int CampaignId)
        {
            try
            {
                bool activateCampaign = false;
                using (DbCommand cmd = DbFactory.GetDBCommand("updatecampaignstatus"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_ids", DbType.String, 500, CampaignId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_deletedby", DbType.Int32, CurrentUser.Id));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_isactive", DbType.Boolean, activateCampaign));

                    MySqlDatabase.ExecuteNonQuery(cmd, ConnectionType.ReadOnly);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"] + "AutoPauseCampaign");
                objErr.SendMail();
            }
        }

        /// <summary>
        ///  Created By : Sushil Kumar
        ///  Created On : 18th April 2016
        ///  Description : To get dealer camapigns and contracts mapping based on dealerId
        /// </summary>
        /// <param name="dealerId"></param>
        /// <returns></returns>
        public DataTable GetDealerCampaigns(uint dealerId)
        {
            DataTable dtDealerCampaigns = null;
            
            try
            {
                if (dealerId > 0)
                {
                    using (DbCommand cmd = DbFactory.GetDBCommand("getdealercampaigns"))
                    {
                        
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_dealerid", DbType.Int32, dealerId > 0 ? dealerId : Convert.DBNull));

                        using (DataSet ds = MySqlDatabase.SelectAdapterQuery(cmd, ConnectionType.ReadOnly))
                        {
                            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
                                dtDealerCampaigns = ds.Tables[0];
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "ManageDealerCampaign.GetDealerCampaigns");
                objErr.SendMail();
            }
            return dtDealerCampaigns;
        }
    }
}

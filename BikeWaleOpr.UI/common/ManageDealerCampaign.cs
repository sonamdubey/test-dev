using BikeWaleOpr.Common;
using System;
using System.Collections.Generic;
using System.Data;
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
            Database db = null;
            try
            {
                if (campaignId > 0)
                {
                    using (SqlCommand cmd = new SqlCommand("BW_FetchBWDealerCampaign"))
                    {
                        db = new Database();
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@CampaignId", campaignId);
                        DataSet ds = db.SelectAdaptQry(cmd);
                        if (ds != null && ds.Tables.Count > 0)
                        {
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
            finally
            {
                if (db != null)
                    db.CloseConnection();
                db = null;
            }
            return dtDealerCampaign;
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 19 Mar 2016
        /// Description :   Save the new Dealer Campaign
        ///                 SP Called : BW_InsertBWDealerCampaign
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
        public bool InsertBWDealerCampaign(bool isActive,  int userId, int dealerId, int contractId, int dealerLeadServingRadius, string maskingNumber, string dealerName, string dealerEmailId, bool isBookingAvailable = false)
        {
            bool isSuccess = false;
            Database db = null;
            try
            {
                using (SqlCommand cmd = new SqlCommand("BW_InsertBWDealerCampaign"))
                {
                    db = new Database();
                    cmd.CommandType = CommandType.StoredProcedure;
                    
                    cmd.Parameters.AddWithValue("@DealerId",dealerId);
                    cmd.Parameters.AddWithValue("@DealerName",dealerName);
                    cmd.Parameters.AddWithValue("@Phone",maskingNumber);
                    cmd.Parameters.AddWithValue("@DealerEmail",dealerEmailId);
                    cmd.Parameters.AddWithValue("@IsActive",isActive);
                    cmd.Parameters.AddWithValue("@ContractId",contractId);
                    cmd.Parameters.AddWithValue("@DealerLeadServingRadius", dealerLeadServingRadius);
                    cmd.Parameters.AddWithValue("@UpdatedBy", userId);
                    //Optional Parameters
                    cmd.Parameters.AddWithValue("@IsBookingAvailable",isBookingAvailable);
                    //if(startDate.HasValue)
                    //    cmd.Parameters.AddWithValue("@StartDate",startDate.Value);
                    //if(endDate.HasValue)
                    //    cmd.Parameters.AddWithValue("@EndDate", endDate.Value);
                    isSuccess = db.InsertQry(cmd);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "ManageDealerCampaign.InsertBWDealerCampaign");
                objErr.SendMail();
            }
            finally
            {
                if (db != null)
                    db.CloseConnection();
                db = null;
            }
            return isSuccess;
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
            Database db = null;
            try
            {
                using (SqlCommand cmd = new SqlCommand("BW_UpdateBWDealerCampaign"))
                {
                    db = new Database();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@CampaignId", campaignId);                    
                    cmd.Parameters.AddWithValue("@DealerId", dealerId);
                    cmd.Parameters.AddWithValue("@DealerName", dealerName);
                    cmd.Parameters.AddWithValue("@Phone", maskingNumber);
                    cmd.Parameters.AddWithValue("@DealerEmail", dealerEmailId);
                    cmd.Parameters.AddWithValue("@IsActive", isActive);
                    cmd.Parameters.AddWithValue("@ContractId", contractId);
                    cmd.Parameters.AddWithValue("@DealerLeadServingRadius", dealerLeadServingRadius);
                    cmd.Parameters.AddWithValue("@UpdatedBy", userId);
                    //Optional Parameters
                    cmd.Parameters.AddWithValue("@IsBookingAvailable", isBookingAvailable);
                    //if (startDate.HasValue)
                    //    cmd.Parameters.AddWithValue("@StartDate", startDate.Value);
                    //if (endDate.HasValue)
                    //    cmd.Parameters.AddWithValue("@EndDate", endDate.Value);
                    isSuccess = db.UpdateQry(cmd);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "ManageDealerCampaign.InsertBWDealerCampaign");
                objErr.SendMail();
            }
            finally
            {
                if (db != null)
                    db.CloseConnection();
                db = null;
            }
            return isSuccess;
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 19 Mar 2016
        /// Description :   Fetch the Dealer Campaign details
        ///                 SP Called : BW_FetchBWDealerCampaign
        /// </summary>
        /// <param name="campaignId">Campaign Id</param>
        /// <returns></returns>
        public DataTable FetchBWCampaigns(int contractId)
        {
            DataTable dtDealerCampaign = null;
            Database db = null;
            try
            {
                if (contractId > 0)
                {
                    using (SqlCommand cmd = new SqlCommand("BW_FetchBWCampaigns"))
                    {
                        db = new Database();
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ContractId", contractId);
                        DataSet ds = db.SelectAdaptQry(cmd);
                        if (ds != null && ds.Tables.Count > 0)
                        {
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
            finally
            {
                if (db != null)
                    db.CloseConnection();
                db = null;
            }
            return dtDealerCampaign;
        }
    }
}
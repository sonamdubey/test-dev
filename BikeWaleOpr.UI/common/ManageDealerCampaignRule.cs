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
    /// Description :   Manage Dealer Campaign Rule
    /// </summary>
    public class ManageDealerCampaignRule
    {
        /// <summary>
        /// Created by  :   Sumit Kate on 19 Mar 2016
        /// Description :   Fetches Campaign Rules
        ///                 SP Called : BW_FetchBWDealerCampaignRules
        /// </summary>
        /// <param name="campaignId"></param>
        /// <param name="dealerId"></param>
        /// <returns></returns>
        public DataTable FetchBWDealerCampaignRules(int campaignId, int dealerId)
        {
            DataTable dtDealerCampaign = null;
            Database db = null;
            try
            {
                if (campaignId > 0 && dealerId > 0)
                {
                    using (SqlCommand cmd = new SqlCommand("BW_FetchBWDealerCampaignRules"))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@CampaignId", campaignId);
                        cmd.Parameters.AddWithValue("@DealerID", dealerId);
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
                ErrorClass objErr = new ErrorClass(ex, "ManageDealerCampaignRule.FetchBWDealerCampaignRules");
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
        /// Description :   Insert new Campaign Rule/s
        ///                 SP Called : BW_InsertCampaignRule
        ///                 If valid modelId is passed then it adds the rules for all the available dealer bikes of a particular make.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="campaignId"></param>
        /// <param name="cityId"></param>
        /// <param name="dealerId"></param>
        /// <param name="makeId"></param>
        /// <param name="stateId"></param>
        /// <param name="modelId"></param>
        /// <returns></returns>
        public bool InsertBWDealerCampaignRules(int userId, int campaignId, int cityId, int dealerId, int makeId, int stateId,int? modelId)
        {
            bool isSuccess = false;
            Database db = null;
            try
            {
                using (SqlCommand cmd = new SqlCommand("BW_InsertCampaignRule"))
                {
                    db = new Database();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CampaignId", campaignId);
                    cmd.Parameters.AddWithValue("@DealerId", dealerId);
                    cmd.Parameters.AddWithValue("@CityId", cityId);                    
                    cmd.Parameters.AddWithValue("@StateId", stateId);
                    cmd.Parameters.AddWithValue("@MakeId", makeId);
                    cmd.Parameters.AddWithValue("@UserID", userId);
                    //Optional Parameters                    
                    if (modelId.HasValue && modelId.Value > 0)
                        cmd.Parameters.AddWithValue("@ModelId", modelId.Value);
                    isSuccess = db.InsertQry(cmd);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "ManageDealerCampaignRule.InsertBWDealerCampaignRules");
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
        /// Description :   updates a Campaign rule
        ///                 SP Called : BW_UpdateCampaignRule 
        /// </summary>
        /// <param name="isActive"></param>
        /// <param name="userId"></param>
        /// <param name="ruleId"></param>
        /// <param name="campaignId"></param>
        /// <param name="cityId"></param>
        /// <param name="dealerId"></param>
        /// <param name="makeId"></param>
        /// <param name="stateId"></param>
        /// <param name="modelId"></param>
        /// <returns></returns>
        public bool UpdateBWDealerCampaignRule(bool isActive, int userId, int ruleId, int campaignId, int cityId, int dealerId, int makeId, int stateId, int modelId)
        {
            bool isSuccess = false;
            Database db = null;
            try
            {
                using (SqlCommand cmd = new SqlCommand("BW_UpdateCampaignRule"))
                {
                    db = new Database();
                    cmd.CommandType = CommandType.StoredProcedure;                    
                    cmd.Parameters.AddWithValue("@RuleId",ruleId);	
                    cmd.Parameters.AddWithValue("@DealerId",dealerId);	
                    cmd.Parameters.AddWithValue("@CityId",cityId);	
                    cmd.Parameters.AddWithValue("@ModelId",modelId);	
                    cmd.Parameters.AddWithValue("@StateId",stateId);	
                    cmd.Parameters.AddWithValue("@MakeId",makeId);		
                    cmd.Parameters.AddWithValue("@UserId",userId);
                    cmd.Parameters.AddWithValue("@IsActive", isActive);	
                    isSuccess = db.UpdateQry(cmd);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "ManageDealerCampaignRule.UpdateBWDealerCampaignRule");
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
        /// Description :   Delete Dealer Campaign Rules
        ///                 SP Called : BW_DeleteCampaignRules 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="ruleIds">Comma separated values</param>
        /// <returns></returns>
        public bool DeleteDealerCampaignRules(int userId, string ruleIds)
        {
            bool isDeleted = false;
            Database db = null;
            try
            {
                using (SqlCommand cmd = new SqlCommand("BW_DeleteCampaignRules"))
                {
                    db = new Database();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@RuleIds", ruleIds);
                    cmd.Parameters.AddWithValue("@userId", userId);
                    isDeleted = db.UpdateQry(cmd);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "ManageDealerCampaignRule.DeleteDealerCampaignRules");
                objErr.SendMail();
            }
            finally
            {
                if (db != null)
                    db.CloseConnection();
                db = null;
            }
            return isDeleted;
        }
    }
}
using BikeWaleOpr.Common;
using BikeWaleOPR.DAL.CoreDAL;
using BikeWaleOPR.Utilities;
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
            
            try
            {
                if (campaignId > 0 && dealerId > 0)
                {
                    using (DbCommand cmd = DbFactory.GetDBCommand("bw_fetchbwdealercampaignrules"))
                    {
                        
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_campaignid", DbParamTypeMapper.GetInstance[SqlDbType.Int], campaignId));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_dealerid", DbParamTypeMapper.GetInstance[SqlDbType.Int], dealerId));

                        using (DataSet ds = MySqlDatabase.SelectAdapterQuery(cmd))
                        {
                            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
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
        public bool InsertBWDealerCampaignRules(int userId, int campaignId, int cityId, int dealerId, int makeId, int stateId, string modelId)
        {
            bool isSuccess = false;
            
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("bw_insertcampaignrule"))
                {
                    
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_campaignid", DbParamTypeMapper.GetInstance[SqlDbType.Int], campaignId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_dealerid", DbParamTypeMapper.GetInstance[SqlDbType.Int], dealerId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_cityid", DbParamTypeMapper.GetInstance[SqlDbType.Int], cityId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_modelid", DbParamTypeMapper.GetInstance[SqlDbType.Int], modelId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_stateid", DbParamTypeMapper.GetInstance[SqlDbType.Int], stateId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_makeid", DbParamTypeMapper.GetInstance[SqlDbType.Int], makeId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_userid", DbParamTypeMapper.GetInstance[SqlDbType.Int], userId));

                    isSuccess = MySqlDatabase.InsertQuery(cmd);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "ManageDealerCampaignRule.InsertBWDealerCampaignRules");
                objErr.SendMail();
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
            
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("bw_updatecampaignrule"))
                {
                    
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_ruleid", DbParamTypeMapper.GetInstance[SqlDbType.Int], ruleId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_dealerid", DbParamTypeMapper.GetInstance[SqlDbType.Int], dealerId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_cityid", DbParamTypeMapper.GetInstance[SqlDbType.Int], cityId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_modelid", DbParamTypeMapper.GetInstance[SqlDbType.Int], modelId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_stateid", DbParamTypeMapper.GetInstance[SqlDbType.Int], stateId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_makeid", DbParamTypeMapper.GetInstance[SqlDbType.Int], makeId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_userid", DbParamTypeMapper.GetInstance[SqlDbType.Int], userId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_isactive", DbParamTypeMapper.GetInstance[SqlDbType.Bit], isActive));
                    isSuccess = MySqlDatabase.UpdateQuery(cmd);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "ManageDealerCampaignRule.UpdateBWDealerCampaignRule");
                objErr.SendMail();
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
            
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("bw_deletecampaignrules"))
                {
                    
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_ruleids", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], 100, ruleIds));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_userid", DbParamTypeMapper.GetInstance[SqlDbType.Int], userId));
                    isDeleted = MySqlDatabase.UpdateQuery(cmd);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "ManageDealerCampaignRule.DeleteDealerCampaignRules");
                objErr.SendMail();
            }

            return isDeleted;
        }
    }
}
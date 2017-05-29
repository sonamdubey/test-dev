using BikeWaleOpr.Common;
using MySql.CoreDAL;
using System;
using System.Data;
using System.Data.Common;

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
                    using (DbCommand cmd = DbFactory.GetDBCommand("bw_fetchbwdealercampaignrules_15052017"))
                    {

                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_campaignid", DbType.Int32, campaignId));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_dealerid", DbType.Int32, dealerId));

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

        public bool InsertBWDealerCampaignRules(int userId, int campaignId, int dealerId, int makeId, string modelIds)
        {
            bool isSuccess = false;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("bw_insertcampaignrule_15052017"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_campaignid", DbType.Int32, campaignId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_dealerid", DbType.Int32, dealerId));
                    //cmd.Parameters.Add(DbFactory.GetDbParam("par_stateid", DbType.Int32, stateId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_makeid", DbType.Int32, makeId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_userid", DbType.Int32, userId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_modelid", DbType.String, modelIds));
                    MySqlDatabase.ExecuteNonQuery(cmd, ConnectionType.MasterDatabase);

                    isSuccess = true;
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
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_ruleids", DbType.String, 100, ruleIds));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_userid", DbType.Int32, userId));
                    isDeleted = MySqlDatabase.UpdateQuery(cmd, ConnectionType.MasterDatabase);
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
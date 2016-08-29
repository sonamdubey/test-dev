using BikeWaleOpr.Common;
using MySql.CoreDAL;
using System;
using System.Data;
using System.Data.Common;

namespace BikewaleOpr.Common
{
    public class ManageDealerCampaignRule
    {
        
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

     
        public bool InsertBWDealerCampaignRules(int userId, int campaignId, int cityId, int dealerId, int makeId, int stateId, string modelIds)
        {
            bool isSuccess = false;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("bw_insertcampaignrule"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_campaignid", DbType.Int32, campaignId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_cityid", DbType.Int32, cityId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_dealerid", DbType.Int32, dealerId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_stateid", DbType.Int32, stateId));
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

       
        public bool UpdateBWDealerCampaignRule(bool isActive, int userId, int ruleId, int campaignId, int cityId, int dealerId, int makeId, int stateId, int modelId)
        {
            bool isSuccess = false;

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("bw_updatecampaignrule"))
                {

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_ruleid", DbType.Int32, ruleId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_dealerid", DbType.Int32, dealerId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_cityid", DbType.Int32, cityId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_modelid", DbType.Int32, modelId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_stateid", DbType.Int32, stateId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_makeid", DbType.Int32, makeId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_userid", DbType.Int32, userId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_isactive", DbType.Boolean, isActive));
                    isSuccess = MySqlDatabase.UpdateQuery(cmd, ConnectionType.MasterDatabase);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "ManageDealerCampaignRule.UpdateBWDealerCampaignRule");
                objErr.SendMail();
            }

            return isSuccess;
        }

    
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
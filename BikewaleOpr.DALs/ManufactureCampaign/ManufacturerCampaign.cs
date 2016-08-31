using Bikewale.Notifications;
using BikewaleOpr.Entities;
using BikewaleOpr.Interface.ManufacturerCampaign;
using MySql.CoreDAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikewaleOpr.DALs.ManufactureCampaign
{
    public class ManufacturerCampaign : IManufacturerCampaign
    {
        /// <summary>
        /// Created By: Aditi Srivastava on 29 Aug 2016
        /// Description: Fetches a list of all cities and states
        /// </summary>
        /// <returns></returns>
        public List<MfgCityEntity> GetManufacturerCities()
        {
            List<MfgCityEntity> AllMfgcities = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getmanufacturercities"))
                    {

                        cmd.CommandType = CommandType.StoredProcedure;
                        using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                        {
                            if (dr != null)
                            {
                                AllMfgcities = new List<MfgCityEntity>();

                                while (dr.Read())
                                {
                                    AllMfgcities.Add(new MfgCityEntity()
                                    {
                                        CityId = Convert.ToInt32(dr["Id"]),
                                        CityName = dr["Name"].ToString(),
                                        StateName = dr["StateName"].ToString()
                                    });
                                }
                                dr.Close();
                            }
                        }
                    }
                
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "ManageDealerCampaignRule.GetManufacturerCities");
                objErr.SendMail();
            }
            return AllMfgcities;

        }
        /// <summary>
        /// Created By: Aditi Srivastava on 29 Aug 2016
        /// Description: Fetches a list of manufacturer campaign rules by campaign id
        /// </summary>
        /// <param name="campaignId"></param>
        /// <param name="dealerId"></param>
        /// <returns></returns>
            public List<MfgCampaignRulesEntity> FetchManufacturerCampaignRules(int campaignId)
            {
                List<MfgCampaignRulesEntity> dtManufacturerCampaignRules = null;

                try
                {
                    if (campaignId > 0)
                    {
                        using (DbCommand cmd = DbFactory.GetDBCommand("fetchmanufacturercampaignrules"))
                        {

                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(DbFactory.GetDbParam("par_campaignid", DbType.Int32, campaignId));

                            using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                            {
                                if (dr != null)
                                {
                                    dtManufacturerCampaignRules = new List<MfgCampaignRulesEntity>();

                                    while (dr.Read())
                                    {
                                        dtManufacturerCampaignRules.Add(new MfgCampaignRulesEntity()
                                        { 
                                            CampaignRuleId=Convert.ToInt32(dr["campaignruleid"]),
                                            ModelName =dr["modelname"].ToString() ,
                                            MakeName = dr["makename"].ToString(),
                                            CityName = dr["cityname"].ToString(),
                                            StateName=dr["statename"].ToString()
                                        });
                                    }
                                    dr.Close();
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    ErrorClass objErr = new ErrorClass(ex, "ManageDealerCampaignRule.FetchManufacturerCampaignRules");
                    objErr.SendMail();
                }

                return dtManufacturerCampaignRules;
            }
            
        /// <summary>
        ///  Created By: Aditi Srivastava on 29 Aug 2016
        /// Description: Inserts new manufacturer campaign rules
        /// </summary>
        /// <param name="MgfRules"></param>
        /// <returns></returns>
            public bool SaveManufacturerCampaignRules(MfgNewRulesEntity MgfRules)
            {
                bool isSuccess = false;
                try
                {
                    using (DbCommand cmd = DbFactory.GetDBCommand("savemanufacturercampaignrules"))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_campaignid", DbType.Int32, MgfRules.CampaignId));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_modelcityids", DbType.String, MgfRules.CityIds));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_userid", DbType.Int32, MgfRules.UserId));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_modelids", DbType.String, MgfRules.ModelIds));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_isallindia", DbType.Boolean, MgfRules.IsAllIndia));
                        MySqlDatabase.ExecuteNonQuery(cmd, ConnectionType.MasterDatabase);

                        isSuccess = true;
                    }
                }
                catch (Exception ex)
                {
                    ErrorClass objErr = new ErrorClass(ex, "ManageDealerCampaignRule.SaveManufacturerCampaignRules");
                    objErr.SendMail();
                }
                return isSuccess;
            }

            /// <summary>
            ///  Created By: Aditi Srivastava on 29 Aug 2016
            /// Description: Deletes selected manufacturer campaign rules
            /// </summary>
            /// <param name="userId"></param>
            /// <param name="ruleIds"></param>
            /// <returns></returns>
           public bool DeleteManufacturerCampaignRules(int userId, string ruleIds)
            {
                bool isDeleted = false;

                try
                {
                    using (DbCommand cmd = DbFactory.GetDBCommand("deletemanufacturercampaignrules"))
                    {

                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_userid", DbType.Int32, userId));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_ruleids", DbType.String, 100, ruleIds));
                        isDeleted = MySqlDatabase.UpdateQuery(cmd, ConnectionType.MasterDatabase);
                    }
                }
                catch (Exception ex)
                {
                    ErrorClass objErr = new ErrorClass(ex, "ManageDealerCampaignRule.DeleteManufacturerCampaignRules");
                    objErr.SendMail();
                }

                return isDeleted;
            }

        /// <summary>
        /// 
        /// </summary>
           public void SearchCampaign()
           {
        
           }

        
    }

}

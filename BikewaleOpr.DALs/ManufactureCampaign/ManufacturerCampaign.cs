using Bikewale.Notifications;
using BikewaleOpr.Entities;
using BikewaleOpr.Entity.ManufacturerCampaign;
using BikewaleOpr.Entity.ContractCampaign;
using BikewaleOpr.Interface.ManufacturerCampaign;
using MySql.CoreDAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace BikewaleOpr.DALs.ManufactureCampaign
{
    public class ManufacturerCampaign : IManufacturerCampaignRepository
    {
        /// <summary>
        /// Description : Return all the campaigns for given manufacturer id

                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "ManufacturerCampaign.searchmanufacturercampaign");
                objErr.SendMail();
            }
            return dtManufactureCampaigns;
        }
        /// <summary>
        /// Created by Subodh Jain 29 aug 2016
        /// Description : Change Status of the campaign
        /// </summary>
        /// <param name="dealerId"></param>
        /// <returns></returns>
        public bool UpdateCampaignStatus(uint id, bool isactive)
        {
            bool isSuccess = false;

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("statuschangeCampaigns"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_id", DbType.UInt32, id));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_isactive", DbType.Boolean, isactive));

                    if (Convert.ToBoolean(MySqlDatabase.ExecuteNonQuery(cmd, ConnectionType.MasterDatabase)))
                        isSuccess = true;
                }
       
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "ManufacturerCampaign.statuschangeCampaigns");
                objErr.SendMail();
            }
            return isSuccess;
            }
        /// <param name="templateId1"></param>
        /// <param name="templateHtml2"></param>
        /// <param name="templateId2"></param>
        /// <param name="templateHtml3"></param>
        /// <param name="templateId3"></param>
        /// <param name="templateHtml4"></param>
        /// <param name="templateId4"></param>
        /// <param name="userId"></param>
        /// <param name="campaignId"></param>
        public void SaveManufacturerCampaignTemplate(string templateHtml1, int templateId1, string templateHtml2, int templateId2, string templateHtml3, int templateId3, string templateHtml4, int templateId4, int userId, int campaignId)
        {
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("savemanufacturecampaigntemplate"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_templateHtml1", DbType.String, 150, templateHtml1));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_templateId1", DbType.Int32, templateId1));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_templateHtml2", DbType.String, 150, templateHtml2));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_templateId2", DbType.Int32, templateId2));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_templateHtml3", DbType.String, 150, templateHtml3));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_templateId3", DbType.Int32, templateId3));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_templateHtml4", DbType.String, 150, templateHtml4));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_templateId4", DbType.Int32, templateId4));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_userId", DbType.Int32, userId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_campaignId", DbType.Int32, campaignId));

                    MySqlDatabase.ExecuteNonQuery(cmd, ConnectionType.MasterDatabase);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "MaufacturerCampaign.SaveManufacturerCampaignTemplate");
                objErr.SendMail();
            }
        }
        
        //public void SaveManufacturerCampaignTemplateMapping(int campaignId, int templateId, int pageId, int isActive, int userId)
        //{
        //    try
        //    {
        //        using (DbCommand cmd = DbFactory.GetDBCommand("savemanufacturercampaigntemplatemapping"))
        //        {
        //            cmd.CommandType = CommandType.StoredProcedure;
        //            cmd.Parameters.Add(DbFactory.GetDbParam("par_campaignId", DbType.Int32, campaignId));
        //            cmd.Parameters.Add(DbFactory.GetDbParam("par_templateId", DbType.Int32, templateId));
        //            cmd.Parameters.Add(DbFactory.GetDbParam("par_pageId", DbType.Int32, pageId));
        //            cmd.Parameters.Add(DbFactory.GetDbParam("par_isActive", DbType.Int32, isActive));
        //            cmd.Parameters.Add(DbFactory.GetDbParam("par_userId", DbType.Int32, userId));
        //            MySqlDatabase.ExecuteNonQuery(cmd, ConnectionType.MasterDatabase);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorClass objErr = new ErrorClass(ex, "MaufacturerCampaign.SaveManufacturerCampaignTemplateMapping");
        //        objErr.SendMail();
        //    }
        //}

        /// <summary>
        /// Created by :Sajal Gupta on 30/08/2016
        /// Description : This method fetches campaign and template data from database.
        /// </summary>
        /// <param name="campaignId"></param>
        /// <returns></returns>
        public List<BikewaleOpr.Entity.ManufacturerCampaign.ManufacturerCampaignEntity> FetchCampaignDetails(int campaignId)
        {
            List<BikewaleOpr.Entity.ManufacturerCampaign.ManufacturerCampaignEntity> objManufacturerCampaignDetails = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("fetchcampaigndetails"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_campaignId", DbType.Int32, campaignId));
                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            objManufacturerCampaignDetails = new List<BikewaleOpr.Entity.ManufacturerCampaign.ManufacturerCampaignEntity>();
                            while (dr.Read())
                            {
                                objManufacturerCampaignDetails.Add(new BikewaleOpr.Entity.ManufacturerCampaign.ManufacturerCampaignEntity() { CampaignDescription = dr["Description"].ToString(), CampaignMaskingNumber = dr["maskingnumber"].ToString(), IsActive = Convert.ToInt32(dr["isactive"]), IsDefault = Convert.ToInt32(dr["isdefault"]), PageId = Convert.ToInt32(dr["pageid"]), TemplateHtml = dr["templatehtml"].ToString(), TemplateId = Convert.ToInt32(dr["id"]) });
                            }
                        }
                    }
                    return objManufacturerCampaignDetails;
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "MaufacturerCampaign.FetchCampaignDetails");
                objErr.SendMail();

                return objManufacturerCampaignDetails;
            }
        }
        
        /// <summary>
        /// Created by Subodh Jain 29 aug 2016
        /// Description : Return all the campaigns for selected dealer
        /// </summary>
        /// <param name="dealerId"></param>
        /// <returns></returns>
        public IEnumerable<ManufactureDealerCampaign> SearchManufactureCampaigns(uint dealerId)
        {
            IList<ManufactureDealerCampaign> dtManufactureCampaigns = null;
            try
            {
                if (dealerId > 0)
                {
                    using (DbCommand cmd = DbFactory.GetDBCommand("searchmanufacturercampaign"))
                    {

                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_dealerid", DbType.Int32, dealerId));


                        using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.MasterDatabase))
                        {
                            if (dr != null)
                            {
                                dtManufactureCampaigns = new List<ManufactureDealerCampaign>();
                                while (dr.Read())
                                {
                                    dtManufactureCampaigns.Add(new ManufactureDealerCampaign
                                    {
                                        id = Convert.ToInt32(dr["id"]),
                                        dealerid = Convert.ToInt32(dr["dealerid"]),
                                        description = Convert.ToString(dr["description"]),
                                        isactive = Convert.ToInt32(dr["isactive"])

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
                objErr.SendMail();
            }
        }
        
        //public void SaveManufacturerCampaignTemplateMapping(int campaignId, int templateId, int pageId, int isActive, int userId)
        //{
        //    try
        //    {
        //        using (DbCommand cmd = DbFactory.GetDBCommand("savemanufacturercampaigntemplatemapping"))
        //        {
        //            cmd.CommandType = CommandType.StoredProcedure;
        //            cmd.Parameters.Add(DbFactory.GetDbParam("par_campaignId", DbType.Int32, campaignId));
        //            cmd.Parameters.Add(DbFactory.GetDbParam("par_templateId", DbType.Int32, templateId));
        //            cmd.Parameters.Add(DbFactory.GetDbParam("par_pageId", DbType.Int32, pageId));
        //            cmd.Parameters.Add(DbFactory.GetDbParam("par_isActive", DbType.Int32, isActive));
        //            cmd.Parameters.Add(DbFactory.GetDbParam("par_userId", DbType.Int32, userId));
        //            MySqlDatabase.ExecuteNonQuery(cmd, ConnectionType.MasterDatabase);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorClass objErr = new ErrorClass(ex, "MaufacturerCampaign.SaveManufacturerCampaignTemplateMapping");
        //        objErr.SendMail();
        //    }
        //}

        /// <summary>
        /// Created by :Sajal Gupta on 30/08/2016
        /// Description : This method fetches campaign and template data from database.
        /// </summary>
        /// <param name="campaignId"></param>
        /// <returns></returns>
        public List<ManufacturerCampaignEntity> FetchCampaignDetails(int campaignId)
        {
            List<ManufacturerCampaignEntity> objManufacturerCampaignDetails = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("fetchcampaigndetails"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_campaignId", DbType.Int32, campaignId));
                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            objManufacturerCampaignDetails = new List<ManufacturerCampaignEntity>();
                            while (dr.Read())
                            {
                                objManufacturerCampaignDetails.Add(new ManufacturerCampaignEntity() { CampaignDescription = dr["Description"].ToString(), CampaignMaskingNumber = dr["maskingnumber"].ToString(), IsActive = Convert.ToInt32(dr["isactive"]), IsDefault = Convert.ToInt32(dr["isdefault"]), PageId = Convert.ToInt32(dr["pageid"]), TemplateHtml = dr["templatehtml"].ToString(), TemplateId = Convert.ToInt32(dr["id"]) });
                            }
                        }
                    }
                    return objManufacturerCampaignDetails;
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "MaufacturerCampaign.FetchCampaignDetails");
                objErr.SendMail();

                return objManufacturerCampaignDetails;
            }
        }


        }
        /// <summary>
        /// Created by Subodh Jain 29 aug 2016
        /// Description :To fetch all the manufacturer in dropdown
        /// </summary>
        /// <param name="dealerId"></param>
        /// <returns></returns>
        public IEnumerable<ManufacturerEntity> GetManufacturersList()
        {
            IList<ManufacturerEntity> manufacturers = null;

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getdealerasmanufacturer"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (IDataReader reader = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (reader != null)
                        {
                            manufacturers = new List<ManufacturerEntity>();
                            while (reader.Read())
                            {
                                manufacturers.Add(
                                    new ManufacturerEntity()
                                    {
                                        Id = Convert.ToInt32(reader["Id"]),
                                        Name = Convert.ToString(reader["Name"]),
                                        Organization = Convert.ToString(reader["Organization"])
                                    }
                                    );
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "ManageManufacturerCampaign.GetDealerAsManuFacturer");
                }

            return manufacturers;
            }
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
                                        CampaignRuleId = Convert.ToInt32(dr["campaignruleid"]),
                                        ModelName = dr["modelname"].ToString(),
                                        MakeName = dr["makename"].ToString(),
                                        CityName = dr["cityname"].ToString(),
                                        StateName = dr["statename"].ToString()
                                    });
                                }
                                dr.Close();
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "ManageDealerCampaignRule.FetchManufacturerCampaignRules");

            return dtManufacturerCampaignRules;
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
    }

}
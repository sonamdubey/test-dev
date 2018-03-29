using Bikewale.DAL.CoreDAL;
using Bikewale.ManufacturerCampaign.Entities.SearchCampaign;
using Bikewale.Notifications;
using Bikewale.Utility;
using BikewaleOpr.Entities;
using BikewaleOpr.Entities.ContractCampaign;
using BikewaleOpr.Entities.ManufacturerCampaign;
using BikewaleOpr.Interface.ManufacturerCampaign;
using Dapper;
using MySql.CoreDAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace BikewaleOpr.DALs.ManufactureCampaign
{
    /// <summary>
    /// Created by Subodh Jain 22 jun 2017
    /// Description :For Manufacturer Campaign
    /// Modified by : Ashutosh Sharma on 25 Jan 2017
    /// Description : Replaced sp from 'getmanufacturecampaignsdetails' to 'getmanufacturecampaignsdetails_25012018' to get campaign start and end time.
    /// Modified By : Rajan Chauhan on 08 Mar 2018
    /// Description : Replaced sp from 'getmanufacturecampaignsdetails_25012018' to 'getmanufacturecampaignsdetails_08032018' to get campaignDays.
    /// </summary>
    /// <param name="dealerId"></param>
    /// <returns></returns>
    public class ManufacturerCampaign : IManufacturerCampaignRepository
    {
        public IEnumerable<ManufacturerCampaignDetailsList> GetManufactureCampaigns(uint dealerId,uint allActiveCampaign)
        {

            IEnumerable<ManufacturerCampaignDetailsList> dtManufactureCampaigns = null;
            try
            {
                using (IDbConnection connection = DatabaseHelper.GetMasterConnection())
                {

                    var param = new DynamicParameters();

                    param.Add("par_dealerid", dealerId);
                    param.Add("par_allactivecampaign", allActiveCampaign);
                    dtManufactureCampaigns = connection.Query<ManufacturerCampaignDetailsList>("getmanufacturecampaignsdetails_08032018", param: param, commandType: CommandType.StoredProcedure);

                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("BikewaleOpr.DALs.ManufactureCampaign.GetManufactureCampaigns dealerId: {0} allActiveCampaign: {1}", dealerId, allActiveCampaign));
            }
            return dtManufactureCampaigns;
        }
        /// <summary>
        /// Created by Subodh Jain 29 aug 2016
        /// Description : Return all the campaigns for given manufacturer id
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
                                        id = SqlReaderConvertor.ToInt32(dr["id"]),
                                        dealerid = SqlReaderConvertor.ToInt32(dr["dealerid"]),
                                        description = Convert.ToString(dr["description"]),
                                        isactive = SqlReaderConvertor.ToInt32(dr["isactive"]),
                                        maskingnumber = Convert.ToString(dr["maskingnumber"])

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
                ErrorClass.LogError(ex, "ManufacturerCampaign.searchmanufacturercampaign");
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
                    isSuccess = Convert.ToBoolean(MySqlDatabase.ExecuteNonQuery(cmd, ConnectionType.MasterDatabase));
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "ManufacturerCampaign.statuschangeCampaigns");
            }
            return isSuccess;
        }
        /// <summary>
        /// Created by Subodh Jain 22 jun 2017
        /// Description : Change Status of the campaign
        /// </summary>
        /// <param name="dealerId"></param>
        /// <returns></returns>
        public bool UpdateCampaignStatus(uint campaignId, uint status)
        {
            int isSuccess = 0;
            try
            {
                using (IDbConnection connection = DatabaseHelper.GetMasterConnection())
                {
                    var param = new DynamicParameters();

                    param.Add("par_campaignid", campaignId);
                    param.Add("par_status", status);

                    isSuccess= connection.Execute("updatemanufacturercampaignstatus", param: param, commandType: CommandType.StoredProcedure);

                }
                
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("BikewaleOpr.DALs.ManufactureCampaign.UpdateCampaignStatus campaignid: {0} status : {1}", campaignId, status));
            }
            return isSuccess > 0;
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
                                        Id = SqlReaderConvertor.ToInt32(reader["Id"]),
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
                ErrorClass.LogError(ex, "ManageManufacturerCampaign.GetDealerAsManuFacturer");
            }

            return manufacturers;
        }
        /// <summary>
        /// Created By: Aditi Srivastava on 29 Aug 2016
        /// Description: Fetches a list of all cities and states
        /// </summary>
        /// <returns></returns>
        public IEnumerable<MfgCityEntity> GetManufacturerCities()
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
                                    CityId = SqlReaderConvertor.ToInt32(dr["Id"]),
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
                ErrorClass.LogError(ex, "ManageDealerCampaignRule.GetManufacturerCities");
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
        public IEnumerable<MfgCampaignRulesEntity> FetchManufacturerCampaignRules(int campaignId)
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
                                        CampaignRuleId = SqlReaderConvertor.ToInt32(dr["campaignruleid"]),
                                        ModelName = dr["modelname"].ToString(),
                                        MakeName = dr["makename"].ToString(),
                                        CityName = dr["cityname"].ToString(),
                                        StateName = dr["statename"].ToString()
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
                ErrorClass.LogError(ex, "ManageDealerCampaignRule.FetchManufacturerCampaignRules");
            }

            return dtManufacturerCampaignRules;
        }

        /// <summary>
        ///  Created By: Aditi Srivastava on 29 Aug 2016
        /// Description: Inserts new manufacturer campaign rules
        /// </summary>
        /// <param name="MgfRules"></param>
        /// <returns></returns>
        public int SaveManufacturerCampaignRules(MfgNewRulesEntity MgfRules)
        {
            int rowsInserted = 0;
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
                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.MasterDatabase))
                    {
                        if (dr != null)
                        {

                            dr.Read();
                            rowsInserted = SqlReaderConvertor.ToInt32(dr["rowsAffected"]);
                            dr.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "ManageDealerCampaignRule.SaveManufacturerCampaignRules");
                rowsInserted = -1;
            }
            return rowsInserted;
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
                ErrorClass.LogError(ex, "ManageDealerCampaignRule.DeleteManufacturerCampaignRules");
            }

            return isDeleted;
        }
        /// <summary>
        /// Created by : Sajal Gupta on 30/08/2016
        /// Description : This function inserts campaigns data and returns campaign id.
        /// </summary>
        /// <param name="description"></param>
        /// <param name="isActive"></param>
        /// <param name="maskingNumber"></param>
        /// <param name="dealerId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public int InsertBWDealerCampaign(string description, int isActive, string maskingNumber, int dealerId, int userId)
        {
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("savemanufacturercampaign_08092016"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_description", DbType.String, 45, description));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_isActive", DbType.Int32, isActive));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_maskingNumber", DbType.String, 10, maskingNumber));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_dealerId", DbType.Int32, dealerId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_userId", DbType.Int32, userId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_newId", DbType.Int32, ParameterDirection.Output));
                    MySqlDatabase.ExecuteNonQuery(cmd, ConnectionType.MasterDatabase);
                    int campaignId = Convert.ToInt32(cmd.Parameters["par_newId"].Value);
                    return campaignId;
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "MaufacturerCampaign.InsertBWDealerCampaign");
                return 0;
            }
        }

        /// <summary>
        /// Created by : Sajal Gupta on 30/08/2016
        /// Description : This function updates template data in database.
        /// Modified By:- Subodh Jain 1 march 2017
        /// Description :- Added LeadCapturePopupMessage,LeadCapturePopupHeading,LeadCapturePopupDescription
        /// </summary>
        /// <param name="description"></param>
        /// <param name="isActive"></param>
        /// <param name="maskingNumber"></param>
        /// <param name="dealerId"></param>
        /// <param name="userId"></param>
        /// <param name="campaignId"></param>
        /// <param name="templateHtml1"></param>
        /// <param name="templateId1"></param>
        /// <param name="templateHtml2"></param>
        /// <param name="templateId2"></param>
        /// <param name="templateHtml3"></param>
        /// <param name="templateId3"></param>
        /// <param name="templateHtml4"></param>
        /// <param name="templateId4"></param>
        public bool UpdateBWDealerCampaign(string description, int isActive, string maskingNumber, int dealerId, int userId, int campaignId, List<ManuCamEntityForTemplate> objList, string LeadCapturePopupMessage, string LeadCapturePopupDescription, string LeadCapturePopupHeading, bool pinCodeRequired,bool emailIdRequired)
        {
            bool isSuccess = false;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("updatemanufacturercampaign_06062017"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_description", DbType.String, 45, description));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_isActive", DbType.Int32, isActive));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_maskingNumber", DbType.String, 10, maskingNumber));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_dealerId", DbType.Int32, dealerId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_userId", DbType.Int32, userId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_campaignId", DbType.Int32, campaignId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_templateHtml1", DbType.String, objList[0].TemplateHtml));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_templateId1", DbType.Int32, objList[0].TemplateId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_templateHtml2", DbType.String, objList[1].TemplateHtml));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_templateId2", DbType.Int32, objList[1].TemplateId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_templateHtml3", DbType.String, objList[2].TemplateHtml));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_templateId3", DbType.Int32, objList[2].TemplateId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_templateHtml4", DbType.String, objList[3].TemplateHtml));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_templateId4", DbType.Int32, objList[3].TemplateId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_LeadCapturePopupMessage", DbType.String, LeadCapturePopupMessage));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_LeadCapturePopupDescription", DbType.String, LeadCapturePopupDescription));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_LeadCapturePopupHeading", DbType.String, LeadCapturePopupHeading));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_pinCodeRequired", DbType.Boolean, pinCodeRequired));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_emailidrequired", DbType.Boolean, emailIdRequired));
                    MySqlDatabase.ExecuteNonQuery(cmd, ConnectionType.MasterDatabase);
                    isSuccess = true;
                }
            }
            catch (Exception ex)
            {

                ErrorClass.LogError(ex, "MaufacturerCampaign.UpdateBWDealerCampaign");
            }
            return isSuccess;
        }

        /// <summary>
        /// Created By : Sajal Gupta on 30/08/2016
        /// Description : This function saves template html and also maps them in database.
        /// Modified By:- Subodh Jain 1 march 2017
        /// Description :- Added LeadCapturePopupMessage,LeadCapturePopupHeading,LeadCapturePopupDescription
        /// <param name="templateHtml1"></param>
        /// <param name="templateId1"></param>
        /// <param name="templateHtml2"></param>
        /// <param name="templateId2"></param>
        /// <param name="templateHtml3"></param>
        /// <param name="templateId3"></param>
        /// <param name="templateHtml4"></param>
        /// <param name="templateId4"></param>
        /// <param name="userId"></param>
        /// <param name="campaignId"></param>
        public bool SaveManufacturerCampaignTemplate(List<ManuCamEntityForTemplate> objList, int userId, int campaignId, string LeadCapturePopupMessage, string LeadCapturePopupDescription, string LeadCapturePopupHeading, int dealerId, bool pinCodeRequired,bool emailIdRequired)
        {
            bool isSuccess = false;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("savemanufacturecampaigntemplate_06062017"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_templateHtml1", DbType.String, objList[0].TemplateHtml));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_templateId1", DbType.Int32, objList[0].TemplateId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_templateHtml2", DbType.String, objList[1].TemplateHtml));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_templateId2", DbType.Int32, objList[1].TemplateId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_templateHtml3", DbType.String, objList[2].TemplateHtml));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_templateId3", DbType.Int32, objList[2].TemplateId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_templateHtml4", DbType.String, objList[3].TemplateHtml));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_templateId4", DbType.Int32, objList[3].TemplateId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_userId", DbType.Int32, userId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_campaignId", DbType.Int32, campaignId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_dealerId", DbType.Int32, dealerId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_LeadCapturePopupMessage", DbType.String, LeadCapturePopupMessage));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_LeadCapturePopupDescription", DbType.String, LeadCapturePopupDescription));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_LeadCapturePopupHeading", DbType.String, LeadCapturePopupHeading));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_pinCodeRequired", DbType.Boolean, pinCodeRequired));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_emailidrequired", DbType.Boolean, emailIdRequired));
                    isSuccess = Convert.ToBoolean(MySqlDatabase.ExecuteNonQuery(cmd, ConnectionType.MasterDatabase));
                }
            }
            catch (Exception ex)
            {

                ErrorClass.LogError(ex, "MaufacturerCampaign.SaveManufacturerCampaignTemplate");
            }
            return isSuccess;
        }


        /// <summary>
        /// Created by :Sajal Gupta on 30/08/2016
        /// Description : This method fetches campaign and template data from database.
        /// Modified By:- Subodh Jain 1 march 2017
        /// Description :- Added LeadCapturePopupMessage,LeadCapturePopupHeading,LeadCapturePopupDescription
        /// <param name="campaignId"></param>
        /// <returns></returns>
        public List<BikewaleOpr.Entities.ManufacturerCampaign.ManufacturerCampaignEntity> FetchCampaignDetails(int campaignId)
        {
            List<BikewaleOpr.Entities.ManufacturerCampaign.ManufacturerCampaignEntity> objManufacturerCampaignDetails = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("fetchcampaigndetails_06062017"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_campaignId", DbType.Int32, campaignId));
                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            objManufacturerCampaignDetails = new List<BikewaleOpr.Entities.ManufacturerCampaign.ManufacturerCampaignEntity>();
                            while (dr.Read())
                            {
                                objManufacturerCampaignDetails.Add(new BikewaleOpr.Entities.ManufacturerCampaign.ManufacturerCampaignEntity()
                                {
                                    CampaignDescription = Convert.ToString(dr["Description"]),
                                    CampaignMaskingNumber = Convert.ToString(dr["maskingnumber"]),
                                    IsActive = SqlReaderConvertor.ToInt32(dr["isactive"]),
                                    IsDefault = SqlReaderConvertor.ToInt32(dr["isdefault"]),
                                    PageId = SqlReaderConvertor.ToInt32(dr["pageid"]),
                                    TemplateHtml = Convert.ToString(dr["templatehtml"]),
                                    TemplateId = SqlReaderConvertor.ToInt32(dr["id"]),
                                    LeadCapturePopupHeading = Convert.ToString(dr["LeadCapturePopupHeading"]),
                                    LeadCapturePopupDescription = Convert.ToString(dr["LeadCapturePopupDescription"]),
                                    LeadCapturePopupMessage = Convert.ToString(dr["LeadCapturePopupMessage"]),
                                    PinCodeRequire = SqlReaderConvertor.ToBoolean(dr["PinCodeRequired"]),
                                    EmailRequire = SqlReaderConvertor.ToBoolean(dr["EmailIDRequired"])
                                });
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "MaufacturerCampaign.FetchCampaignDetails");

            }
            return objManufacturerCampaignDetails;
        }



        /// <summary>
        /// Created By : Sajal Gupta on 31/08/2016
        /// Description : THis method will relese masking number from db against given campaign id;
        /// </summary>
        /// <param name="campaignId"></param>
        /// <returns></returns>
        public bool ReleaseCampaignMaskingNumber(int campaignId)
        {
            bool isSuccess = false;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("releasemaskingnumber"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_campaignId", DbType.Int32, campaignId));
                    MySqlDatabase.ExecuteNonQuery(cmd, ConnectionType.MasterDatabase);
                    isSuccess = true;
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "ManufacturerCampaign.ReleaseCampaignMaskingNumber");
            }
            return isSuccess;
        }


    }

}

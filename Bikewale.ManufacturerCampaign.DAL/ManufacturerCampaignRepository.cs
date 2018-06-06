
using Bikewale.DAL.CoreDAL;
using Bikewale.ManufacturerCampaign.Entities;
using Bikewale.Notifications;
using Bikewaleopr.ManufacturerCampaign.Entities;
using BikewaleOpr.Entities;
using BikewaleOpr.Entity.ManufacturerCampaign;
using BikewaleOpr.Models.ManufacturerCampaign;
using Dapper;
using MySql.CoreDAL;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;

namespace Bikewale.ManufacturerCampaign.DAL
{
    public class ManufacturerCampaignRepository : Interface.IManufacturerCampaignRepository
    {
        /// <summary>
        /// Modified by :- Subodh Jain 10 july 2017
        /// summary :- Get manufacturer list
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ManufacturerEntity> GetManufacturersList()
        {
            IEnumerable<ManufacturerEntity> manufacturers = null;

            try
            {
                using (IDbConnection connection = DatabaseHelper.GetMasterConnection())
                {
                    var param = new DynamicParameters();
                    manufacturers = connection.Query<ManufacturerEntity>("getdealerasmanufacturer", param: param, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "BikewaleOpr.DALs.ManufactureCampaign.GetManufactureCampaigns");
            }


            return manufacturers;

        }

        /// <summary>
        /// Created by : Aditi Srivastava on 22 Jun 2017
        /// Summary    : Get all bike makes
        /// </summary>
        public IEnumerable<BikeMakeEntity> GetBikeMakes()
        {
            IEnumerable<BikeMakeEntity> bikeMakes = null;
            try
            {
                using (IDbConnection connection = DatabaseHelper.GetMasterConnection())
                {
                    var param = new DynamicParameters();
                    bikeMakes = connection.Query<BikeMakeEntity>("getmanufacturermakes", param: param, commandType: CommandType.StoredProcedure);

                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.ManufacturerCampaign.DAL.GetBikeMakes");
            }
            return bikeMakes;
        }


        /// <summary>
        /// Modified by : Ashutosh Sharma on 25 Jan 2017
        /// Description : Replaced sp from 'getmanufacturercampaign' to 'getmanufacturercampaign_25012018', added check for daily campaign start and end time.
        /// Modified by : Pratibha Verma on 8 Mar, 2018
        /// Description : Replace sp from 'getmanufacturercampaign_25012018' to 'getmanufacturercampaign_07032018', added campaign days
        /// </summary>
        /// <param name="dealerId"></param>
        /// <param name="campaignId"></param>
        /// <returns></returns>
        public ConfigureCampaignEntity GetManufacturerCampaign(uint dealerId, uint campaignId)
        {
            ConfigureCampaignEntity objEntity = null;
            try
            {
                using (IDbConnection connection = DatabaseHelper.GetMasterConnection())
                {
                    var param = new DynamicParameters();
                    param.Add("par_campaignId", campaignId);
                    param.Add("par_dealerId", dealerId);
                    objEntity = new ConfigureCampaignEntity();
                    using (var results = connection.QueryMultiple("getmanufacturercampaign_07032018", param: param, commandType: CommandType.StoredProcedure))
                    {
                        objEntity.DealerDetails = results.Read<ManufacturerCampaignDetails>().SingleOrDefault();
                        objEntity.CampaignPages = results.Read<ManufacturerCampaignPages>();
                    }

                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.ManufacturerCampaign.DAL.getManufacturerCampaign");
            }
            return objEntity;
        }
        /// <summary>
        /// Modified by :- Subodh Jain 10 july 2017
        /// summary :- Get manufacturer list
        /// </summary>
        /// <returns></returns>
        public ManufacturerCampaignPopup getManufacturerCampaignPopup(uint campaignId)
        {
            ManufacturerCampaignPopup objEntity = null;
            try
            {
                using (IDbConnection connection = DatabaseHelper.GetMasterConnection())
                {
                    var param = new DynamicParameters();
                    param.Add("par_campaignId", campaignId);
                    objEntity = new ManufacturerCampaignPopup();
                    objEntity = connection.Query<ManufacturerCampaignPopup>("getmanufacturercampaignpopup", param: param, commandType: CommandType.StoredProcedure).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.ManufacturerCampaign.DAL.getManufacturerCampaign");
            }
            return objEntity;
        }
        /// <summary>
        /// Created by Sangram Nandkhile on 22 Jun 2017
        /// Summary: Fetch campaign properties
        /// </summary>
        /// <param name="campaignId"></param>
        /// <returns></returns>
        public CampaignPropertyEntity GetManufacturerCampaignProperties(uint campaignId)
        {
            CampaignPropertyEntity campaign = null;
            try
            {
                using (IDbConnection connection = DatabaseHelper.GetMasterConnection())
                {
                    var param = new DynamicParameters();
                    param.Add("par_campaignid", campaignId);
                    campaign = new CampaignPropertyEntity();

                    using (var results = connection.QueryMultiple("getmanufacturercampaignproperties_28092017", param: param, commandType: CommandType.StoredProcedure))
                    {
                        campaign.EMI = results.Read<CampaignEMIPropertyEntity>().SingleOrDefault();
                        if (campaign.EMI == null) { campaign.EMI = new CampaignEMIPropertyEntity(); }
                        campaign.EMIPriority = results.Read<PriorityEntity>();
                        campaign.Lead = results.Read<CampaignLeadPropertyEntity>().SingleOrDefault();
                        if (campaign.Lead == null) { campaign.Lead = new CampaignLeadPropertyEntity(); }
                        campaign.LeadPriority = results.Read<PriorityEntity>();
                    }

                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Bikewale.ManufacturerCampaign.DAL.getManufacturerCampaign. CampaignId {0}", campaignId));
            }

            return campaign;
        }

        /// <summary>
        /// Created by : Sangram Nandkhile on 28 Jun 2017
        /// Summary    : Save manufacturer campaign rules
        /// </summary>
        public bool SaveManufacturerCampaignProperties(CampaignPropertiesVM objCampaign)
        {
            bool isSaved = false;
            try
            {
                using (IDbConnection connection = DatabaseHelper.GetMasterConnection())
                {
                    var param = new DynamicParameters();
                    param.Add("par_campaignId", objCampaign.CampaignId);
                    param.Add("par_hasEmiProperties", objCampaign.HasEmiProperties);
                    param.Add("par_emiButtonTextMobile", objCampaign.EmiButtonTextMobile);
                    param.Add("par_emiPropertyTextMobile", objCampaign.EmiPropertyTextMobile);
                    param.Add("par_emiButtonTextDesktop", objCampaign.EmiButtonTextDesktop);
                    param.Add("par_emiPropertyTextDesktop", objCampaign.EmiPropertyTextDesktop);
                    param.Add("par_emiPriority", objCampaign.EmiPriority);
                    param.Add("par_hasLeadProperties", objCampaign.HasLeadProperties);
                    param.Add("par_leadButtonTextMobile", objCampaign.LeadButtonTextMobile);
                    param.Add("par_leadPropertyTextMobile", objCampaign.LeadPropertyTextMobile);
                    param.Add("par_leadButtonTextDesktop", objCampaign.LeadButtonTextDesktop);
                    param.Add("par_leadPropertyTextDesktop", objCampaign.LeadPropertyTextDesktop);
                    param.Add("par_leadPriority", objCampaign.LeadPriority);
                    param.Add("par_leadHtmlMobile", objCampaign.FormattedHtmlMobile);
                    param.Add("par_leadHtmlDesktop", objCampaign.FormattedHtmlDesktop);
                    param.Add("par_priceBreakUpLinkTextMobile", objCampaign.PriceBreakUpLinkTextMobile);
                    param.Add("par_PriceBreakUpLinkMobile", objCampaign.PriceBreakUpLinkMobile);
                    param.Add("par_priceBreakUpLinkTextDesktop", objCampaign.PriceBreakUpLinkTextDesktop);
                    param.Add("par_priceBreakUpLinkDesktop", objCampaign.PriceBreakUpLinkDesktop);


                    connection.Query<dynamic>("savemanufacturercampaignproperties_28092017", param: param, commandType: CommandType.StoredProcedure);

                    isSaved = true;
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.ManufacturerCampaign.DAL.SaveManufacturerCampaignProperties");
            }
            return isSaved;
        }
        /// <summary>
        /// Created by : Aditi Srivastava on 22 Jun 2017
        /// Summary    : Get all bike models by make Id
        /// </summary>
        public IEnumerable<BikeModelEntity> GetBikeModels(uint makeId)
        {
            IEnumerable<BikeModelEntity> bikeModels = null;
            try
            {
                using (IDbConnection connection = DatabaseHelper.GetMasterConnection())
                {
                    var param = new DynamicParameters();
                    param.Add("par_makeid", makeId);
                    bikeModels = connection.Query<BikeModelEntity>("getmanufacturermodels", param: param, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Bikewale.ManufacturerCampaign.DAL.GetBikeModels. MakeId : {0}", makeId));
            }
            return bikeModels;
        }
        /// <summary>
        /// Created by : Aditi Srivastava on 22 Jun 2017
        /// Summary    : Get all states
        /// </summary>
        public IEnumerable<StateEntity> GetStates()
        {
            IEnumerable<StateEntity> states = null;
            try
            {
                using (IDbConnection connection = DatabaseHelper.GetMasterConnection())
                {
                    var param = new DynamicParameters();
                    states = connection.Query<StateEntity>("getstates_20062017", param: param, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.ManufacturerCampaign.DAL.GetStates");
            }
            return states;
        }
        /// <summary>
        /// Created by : Aditi Srivastava on 22 Jun 2017
        /// Summary    : Get all cities by state Id
        /// </summary>
        public IEnumerable<CityEntity> GetCitiesByState(uint stateId)
        {
            IEnumerable<CityEntity> cities = null;
            try
            {
                using (IDbConnection connection = DatabaseHelper.GetMasterConnection())
                {
                    var param = new DynamicParameters();
                    param.Add("par_stateid", stateId);
                    cities = connection.Query<CityEntity>("getcitiesbystate", param: param, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Bikewale.ManufacturerCampaign.DAL.GetCitiesByState. StateId : {0}", stateId));
            }
            return cities;
        }
        /// <summary>
        /// Created by : Aditi Srivastava on 22 Jun 2017
        /// Summary    : Get manufacturer campaign rules by campaignId
        /// </summary>
        public ManufacturerCampaignRulesWrapper GetManufacturerCampaignRules(uint campaignId)
        {
            ManufacturerCampaignRulesWrapper mfgRules = null;
            try
            {
                using (IDbConnection connection = DatabaseHelper.GetMasterConnection())
                {
                    var param = new DynamicParameters();
                    param.Add("par_campaignid", campaignId);
                    mfgRules = new ManufacturerCampaignRulesWrapper();
                    using (var results = connection.QueryMultiple("getmanufacturercampaignrules", param: param, commandType: CommandType.StoredProcedure))
                    {
                        mfgRules.ManufacturerCampaignRules = results.Read<ManufacturerRuleEntity>();
                        mfgRules.ShowOnExShowroom = results.Read<bool>().SingleOrDefault();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Bikewale.ManufacturerCampaign.DAL.GetManufacturerCampaignRules. CampaignId : {0}", campaignId));
            }
            return mfgRules;
        }

        /// <summary>
        /// Modified by : Ashutosh Sharma on 25 Jan 2017
        /// Description : Replaced sp from 'savemanufacturercampaign_21062017' to 'savemanufacturercampaign_25012018' to also save daily campaign start and end time.
        /// Modified by : Pratibha Verma on 8 Mar, 2018
        /// Description : Replace sp from 'savemanufacturercampaign_25012018' to 'savemanufacturercampaign_08032018' to save campain days
        /// </summary>
        /// <param name="objCampaign"></param>
        /// <returns></returns>
        public uint saveManufacturerCampaign(ConfigureCampaignSave objCampaign)
        {
            uint campaignId = 0;
            try
            {
                using (IDbConnection connection = DatabaseHelper.GetMasterConnection())
                {
                    var param = new DynamicParameters();
                    param.Add("par_userid", objCampaign.UserId);
                    param.Add("par_dealerid", objCampaign.DealerId);
                    param.Add("par_description", objCampaign.Description);
                    param.Add("par_maskingnumber", objCampaign.MaskingNumber);
                    if (objCampaign.DailyLeadLimit > 0)
                        param.Add("par_dailyleadlimit", objCampaign.DailyLeadLimit);
                    else
                        param.Add("par_dailyleadlimit", null);
                    if (objCampaign.TotalLeadLimit > 0)
                        param.Add("par_totalleadlimit", objCampaign.TotalLeadLimit);
                    else
                        param.Add("par_totalleadlimit", null);
                    param.Add("par_campaignpages", objCampaign.CampaignPages);
                    param.Add("par_startDate", objCampaign.StartDate);
                    param.Add("par_endDate", objCampaign.EndDate ?? null);
                    param.Add("par_dailyStartTime", objCampaign.DailyStartTime);
                    param.Add("par_dailyEndTime", objCampaign.DailyEndTime);
                    param.Add("par_campaignDays", objCampaign.CampaignDays);
                    param.Add("par_showonexshowroomprice", objCampaign.ShowOnExShowroomPrice);
                    param.Add("par_campaignid", objCampaign.CampaignId, dbType: DbType.Int32, direction: ParameterDirection.InputOutput);
                    connection.Query<dynamic>("savemanufacturercampaign_08032018", param: param, commandType: CommandType.StoredProcedure);
                    campaignId = (uint)param.Get<int>("par_campaignid");

                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.ManufacturerCampaign.DAL.saveManufacturerCampaign");
            }
            return campaignId;
        }


        public void saveManufacturerCampaignPopup(ManufacturerCampaignPopup objCampaign)
        {

            try
            {
                using (IDbConnection connection = DatabaseHelper.GetMasterConnection())
                {
                    var param = new DynamicParameters();
                    param.Add("par_campaignid", objCampaign.CampaignId);
                    param.Add("par_PopupHeading", objCampaign.PopupHeading);
                    param.Add("par_PopupDescription", objCampaign.PopupDescription);
                    param.Add("par_PopupSuccessMessage", objCampaign.PopupSuccessMessage);
                    param.Add("par_EmailRequired", objCampaign.EmailRequired ? 1 : 0);
                    param.Add("par_PincodeRequired", objCampaign.PinCodeRequired ? 1 : 0);
                    param.Add("par_DealerRequired", objCampaign.DealerRequired ? 1 : 0);
                    connection.Query<dynamic>("savemanufacturercampaignpopup_28092017", param: param, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.ManufacturerCampaign.DAL.saveManufacturerCampaignPopup");
            }

        }

        /// <summary>
        /// Created by : Aditi Srivastava on 22 Jun 2017
        /// Summary    : Save manufacturer campaign rules
        /// </summary>
        public bool SaveManufacturerCampaignRules(uint campaignId, string modelIds, string stateIds, string cityIds, bool isAllIndia, uint userId)
        {
            bool isSuccess = false;
            try
            {
                using (IDbConnection connection = DatabaseHelper.GetMasterConnection())
                {
                    var param = new DynamicParameters();
                    param.Add("par_campaignid", campaignId);
                    param.Add("par_modelids", modelIds);
                    param.Add("par_stateids", stateIds);
                    param.Add("par_cityids", cityIds);
                    param.Add("par_enteredby", userId);
                    param.Add("par_isAllIndia", isAllIndia);
                    connection.Query<dynamic>("savemanufacturercampaignrules_20062017", param: param, commandType: CommandType.StoredProcedure);
                    isSuccess = true;
                }

            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Bikewale.ManufacturerCampaign.DAL.SaveManufacturerCampaignRules. CampaignId : {0}, ModelIds : {1},StateIds : {2}, CityIds : {3}, IsAllIndia : {4}, UserId : {5}", campaignId, modelIds, stateIds, cityIds, isAllIndia, userId));
            }
            return isSuccess;
        }

        /// <summary>
        /// Created by : Aditi Srivastava on 22 Jun 2017
        /// Summary    : Delete manufacturer campaign rules 
        /// </summary>
        public bool DeleteManufacturerCampaignRules(uint campaignId, uint modelId, uint stateId, uint cityId, uint userId, bool isAllIndia)
        {
            bool isSuccess = false;
            try
            {

                using (IDbConnection connection = DatabaseHelper.GetMasterConnection())
                {
                    var param = new DynamicParameters();
                    param.Add("par_campaignid", campaignId);
                    param.Add("par_modelid", modelId);
                    param.Add("par_stateid", stateId);
                    param.Add("par_cityid", cityId);
                    param.Add("par_enteredby", userId);
                    param.Add("par_isAllIndia", isAllIndia);
                    connection.Query<dynamic>("deletemanufacturercampaignrules_20062017", param: param, commandType: CommandType.StoredProcedure);
                    isSuccess = true;
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Bikewale.ManufacturerCampaign.DAL.DeleteManufacturerCampaignRules. CampaignId : {0}, ModelId : {1},StateId : {2}, CityId : {3}, UserId : {4}", campaignId, modelId, stateId, cityId, userId));
            }
            return isSuccess;
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 29 Jun 2017
        /// Description :   Returns Lead Campaign and EMI campaign by model,city and page
        /// Modified by : Ashutosh Sharma on 25 Jan 2017
        /// Description : Replaced sp from 'getmanufacturercampaignbymodelcity_28092017' to 'getmanufacturercampaignbymodelcity_25012018' to get daily campaign start and end time.
        /// Modified by : Pratibha Verma on 8 Mar, 2018
        /// Description : Replace sp 'getmanufacturercampaignbymodelcity_25012018' with 'getmanufacturercampaignbymodelcity_07032018' to add check for campaign days
        /// </summary>
        /// <param name="modelId"></param>
        /// <param name="cityId"></param>
        /// <param name="pageId"></param>
        /// <returns></returns>
        public Entities.ManufacturerCampaignEntity GetCampaigns(uint modelId, uint cityId, ManufacturerCampaignServingPages pageId)
        {
            Entities.ManufacturerCampaignEntity config = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "getmanufacturercampaignbymodelcity_07032018";
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_modelId", DbType.Int32, modelId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_cityId", DbType.Int32, cityId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_pageId", DbType.Int32, pageId));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            config = new Entities.ManufacturerCampaignEntity();
                            if (dr.Read())
                            {
                                config.LeadCampaign = new ManufacturerCampaignLeadConfiguration();
                                config.LeadCampaign.CampaignId = Utility.SqlReaderConvertor.ParseToUInt32(dr["CampaignId"]);
                                config.LeadCampaign.DealerId = Utility.SqlReaderConvertor.ParseToUInt32(dr["DealerId"]);
                                config.LeadCampaign.DealerRequired = Utility.SqlReaderConvertor.ToBoolean(dr["DealerRequired"]);
                                config.LeadCampaign.EmailRequired = Utility.SqlReaderConvertor.ToBoolean(dr["EmailRequired"]);
                                config.LeadCampaign.LeadsButtonTextDesktop = Convert.ToString(dr["LeadsButtonTextDesktop"]);
                                config.LeadCampaign.LeadsButtonTextMobile = Convert.ToString(dr["LeadsButtonTextMobile"]);
                                config.LeadCampaign.LeadsHtmlDesktop = Convert.ToString(dr["LeadsHtmlDesktop"]);
                                config.LeadCampaign.LeadsHtmlMobile = Convert.ToString(dr["LeadsHtmlMobile"]);
                                config.LeadCampaign.LeadsPropertyTextDesktop = Convert.ToString(dr["LeadsPropertyTextDesktop"]);
                                config.LeadCampaign.LeadsPropertyTextMobile = Convert.ToString(dr["LeadsPropertyTextMobile"]);
                                config.LeadCampaign.MaskingNumber = Convert.ToString(dr["MaskingNumber"]);
                                config.LeadCampaign.Organization = Convert.ToString(dr["Organization"]);
                                config.LeadCampaign.PincodeRequired = Utility.SqlReaderConvertor.ToBoolean(dr["PincodeRequired"]);
                                config.LeadCampaign.PopupDescription = Convert.ToString(dr["PopupDescription"]);
                                config.LeadCampaign.PopupHeading = Convert.ToString(dr["PopupHeading"]);
                                config.LeadCampaign.PopupSuccessMessage = Convert.ToString(dr["PopupSuccessMessage"]);
                                config.LeadCampaign.PriceBreakUpLinkDesktop = Convert.ToString(dr["PriceBreakUpLinkDesktop"]);
                                config.LeadCampaign.PriceBreakUpLinkMobile = Convert.ToString(dr["PriceBreakUpLinkMobile"]);
                                config.LeadCampaign.PriceBreakUpLinkTextDesktop = Convert.ToString(dr["PriceBreakUpLinkTextDesktop"]);
                                config.LeadCampaign.PriceBreakUpLinkTextMobile = Convert.ToString(dr["PriceBreakUpLinkTextMobile"]);
                                config.LeadCampaign.ShowOnExshowroom = Utility.SqlReaderConvertor.ToBoolean(dr["ShowOnExshowroom"]);
                            }


                            if (dr.NextResult() && dr.Read())
                            {
                                config.EMICampaign = new ManufacturerCampaignEMIConfiguration();
                                config.EMICampaign.CampaignId = Utility.SqlReaderConvertor.ParseToUInt32(dr["CampaignId"]);
                                config.EMICampaign.DealerId = Utility.SqlReaderConvertor.ParseToUInt32(dr["DealerId"]);
                                config.EMICampaign.PincodeRequired = Utility.SqlReaderConvertor.ToBoolean(dr["PincodeRequired"]);
                                config.EMICampaign.DealerRequired = Utility.SqlReaderConvertor.ToBoolean(dr["DealerRequired"]);
                                config.EMICampaign.EmailRequired = Utility.SqlReaderConvertor.ToBoolean(dr["EmailRequired"]);
                                config.EMICampaign.EMIButtonTextDesktop = Convert.ToString(dr["EMIButtonTextDesktop"]);
                                config.EMICampaign.EMIButtonTextMobile = Convert.ToString(dr["EMIButtonTextMobile"]);
                                config.EMICampaign.EMIPropertyTextDesktop = Convert.ToString(dr["EMIPropertyTextDesktop"]);
                                config.EMICampaign.EMIPropertyTextMobile = Convert.ToString(dr["EMIPropertyTextMobile"]);
                                config.EMICampaign.MaskingNumber = Convert.ToString(dr["MaskingNumber"]);
                                config.EMICampaign.Organization = Convert.ToString(dr["Organization"]);
                                config.EMICampaign.PopupDescription = Convert.ToString(dr["PopupDescription"]);
                                config.EMICampaign.PopupHeading = Convert.ToString(dr["PopupHeading"]);
                                config.EMICampaign.PopupSuccessMessage = Convert.ToString(dr["PopupSuccessMessage"]);
                                config.EMICampaign.ShowOnExshowroom = Utility.SqlReaderConvertor.ToBoolean(dr["ShowOnExshowroom"]);
                            }

                        }
                    }
                }

            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("ManufacturerCampaignRepository.GetCampaigns({0},{1},{2})", modelId, cityId, pageId));
            }
            return config;
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 30 Jun 2017
        /// Description :   Save Manufacturer Lead to PQ table
        /// </summary>
        /// <param name="dealerid"></param>
        /// <param name="pqId"></param>
        /// <param name="customerName"></param>
        /// <param name="customerEmail"></param>
        /// <param name="customerMobile"></param>
        /// <param name="colorId"></param>
        /// <param name="leadSourceId"></param>
        /// <param name="utma"></param>
        /// <param name="utmz"></param>
        /// <param name="deviceId"></param>
        /// <param name="campaignId"></param>
        /// <returns></returns>
        public uint SaveManufacturerCampaignLead(uint dealerid, uint pqId, UInt64 customerId, string customerName, string customerEmail, string customerMobile, uint leadSourceId, string utma, string utmz, string deviceId, uint campaignId, uint leadId)
        {
            uint retLeadId = 0;
            try
            {
                using (IDbConnection connection = DatabaseHelper.GetMasterConnection())
                {
                    var param = new DynamicParameters();
                    param.Add("par_dealerid", dealerid);
                    param.Add("par_pqid", pqId);
                    param.Add("par_customerid", customerId);
                    param.Add("par_customername", customerName);
                    param.Add("par_customeremail", customerEmail);
                    param.Add("par_customermobile", customerMobile);
                    param.Add("par_leadsourceid", leadSourceId);
                    param.Add("par_utma", utma);
                    param.Add("par_utmz", utmz);
                    param.Add("par_deviceid", deviceId);
                    param.Add("par_campaignId", campaignId);
                    param.Add("par_leadId", leadId, direction: ParameterDirection.InputOutput);
                    connection.Execute("savemanufacturerpqlead", param: param, commandType: CommandType.StoredProcedure);
                    retLeadId = param.Get<UInt32>("par_leadId");
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("ManufacturerCampaignRepository.SaveManufacturerCampaignLead({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10})", dealerid, pqId, customerName, customerEmail, customerMobile, leadSourceId, utma, utmz, deviceId, campaignId, retLeadId));
            }

            return retLeadId;
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 04 Aug 2017
        /// Description :   Calls a sp to reset the total lead delivered to zero
        /// </summary>
        /// <param name="campaignId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool ResetTotalLeadDelivered(uint campaignId, uint userId)
        {
            bool isSuccess = false;
            try
            {
                using (IDbConnection connection = DatabaseHelper.GetMasterConnection())
                {
                    var param = new DynamicParameters();
                    param.Add("par_campaignid", campaignId);
                    param.Add("par_userId", userId);
                    connection.Execute("resetmanufacturerleadcount", param: param, commandType: CommandType.StoredProcedure);
                    isSuccess = true;
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("ManufacturerCampaignRepository.ResetTotalLeadDelivered({0},{1})", campaignId, userId));
            }
            return isSuccess;
        }

        /// <summary>
        /// Created By : Deepak Israni on 3 May 2018
        /// Description: Save manufacturer lead to PQ table.
        /// </summary>
        /// <param name="campaignDetails"></param>
        /// <returns></returns>
        public uint SaveManufacturerCampaignLead(ES_SaveEntity campaignDetails)
        {
            uint leadId = 0;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "savemanufacturerpqlead_14052018";

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_dealerid", DbType.UInt32, campaignDetails.DealerId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_pqid", DbType.UInt32, campaignDetails.PQId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_customerid", DbType.UInt32, campaignDetails.CustomerId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_customername", DbType.String, campaignDetails.CustomerName));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_customeremail", DbType.String, campaignDetails.CustomerEmail));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_customermobile", DbType.String, campaignDetails.CustomerMobile));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_leadsourceid", DbType.UInt32, campaignDetails.LeadSourceId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_utma", DbType.String, campaignDetails.UTMA));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_utmz", DbType.String, campaignDetails.UTMZ));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_deviceid", DbType.String, campaignDetails.DeviceId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_campaignId", DbType.UInt32, campaignDetails.CampaignId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_leadId", DbType.UInt32,ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_spamscore", DbType.Double, campaignDetails.SpamScore));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_rejectionreason", DbType.String, campaignDetails.Reason));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_isaccepted", DbType.Boolean, campaignDetails.IsAccepted));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_overallscore", DbType.Int16, campaignDetails.OverallSpamScore));
                    MySqlDatabase.ExecuteNonQuery(cmd, ConnectionType.MasterDatabase);                    
                    leadId = Convert.ToUInt32(cmd.Parameters["par_leadId"].Value);
                }

            }
            catch (Exception ex)
            {  
                ErrorClass.LogError(ex, string.Format("ManufacturerCampaignRepository.SaveManufacturerCampaignLead: ({0})", JsonConvert.SerializeObject(campaignDetails)));
            }
            return leadId;
        }
    }
}

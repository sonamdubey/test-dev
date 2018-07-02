
using Bikewale.DAL.CoreDAL;
using Bikewale.ManufacturerCampaign.Entities;
using Bikewale.Notifications;
using Bikewale.Utility;
using Bikewaleopr.ManufacturerCampaign.Entities;
using BikewaleOpr.Entities;
using BikewaleOpr.Entity.ManufacturerCampaign;
using BikewaleOpr.Models.ManufacturerCampaign;
using Dapper;
using MySql.CoreDAL;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
            ICollection<ManufacturerEntity> manufacturers = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getdealerasmanufacturer"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            manufacturers = new Collection<ManufacturerEntity>();

                            while (dr.Read())
                            {
                                manufacturers.Add(new ManufacturerEntity()
                                {
                                    Id = SqlReaderConvertor.ToInt32(dr["id"]),
                                    Name = Convert.ToString(dr["name"]),
                                    Organization = Convert.ToString(dr["organization"])
                                });
                            }
                            dr.Close();
                        }
                    }
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
            ICollection<BikeMakeEntity> bikeMakes = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getmanufacturermakes"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            bikeMakes = new Collection<BikeMakeEntity>();

                            while (dr.Read())
                            {
                                bikeMakes.Add(new BikeMakeEntity()
                                {
                                    MakeId = SqlReaderConvertor.ToUInt32(dr["MakeId"]),
                                    MakeName = Convert.ToString(dr["MakeName"])
                                });
                            }
                            dr.Close();
                        }
                    }
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
        /// Modifier    : Kartik on 17 may 2018, replace getmanufacturercampaign_07032018 with getmanufacturercampaign_17052018  fetched SendLeadSMSCustomer
        /// </summary>
        /// <param name="dealerId"></param>
        /// <param name="campaignId"></param>
        /// <returns></returns>
        public ConfigureCampaignEntity GetManufacturerCampaign(uint dealerId, uint campaignId)
        {
            ConfigureCampaignEntity objEntity = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getmanufacturercampaign_17052018"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_campaignId", DbType.Int32, campaignId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_dealerId", DbType.Int32, dealerId));
                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        objEntity = new ConfigureCampaignEntity();
                        if (dr != null)
                        {
                            if (dr.Read())
                            {
                                objEntity.DealerDetails = new ManufacturerCampaignDetails()
                                {
                                    DealerName = Convert.ToString(dr["DealerName"]),
                                    MobileNo = Convert.ToString(dr["MobileNo"]),
                                    Description = Convert.ToString(dr["Description"]),
                                    MaskingNumber = Convert.ToString(dr["MaskingNumber"]),
                                    CampaignStartDate = SqlReaderConvertor.ToDateTime(dr["CampaignStartDate"]),
                                    CampaignEndDate = SqlReaderConvertor.ToDateTime(dr["CampaignEndDate"]),
                                    DailyLeadLimit = SqlReaderConvertor.ToInt32(dr["DailyLeadLimit"]),
                                    TotalLeadLimit = SqlReaderConvertor.ToInt32(dr["TotalLeadLimit"]),
                                    DailyLeadsDelivered = SqlReaderConvertor.ToInt32(dr["DailyLeadsDelivered"]),
                                    TotalLeadsDelivered = SqlReaderConvertor.ToInt32(dr["TotalLeadsDelivered"]),
                                    CampaignStatus = Convert.ToString(dr["CampaignStatus"]),
                                    ShowCampaignOnExshowroom = SqlReaderConvertor.ToBoolean(dr["ShowCampaignOnExshowroom"]),
                                    DailyStartTime = SqlReaderConvertor.ToDateTime(dr["DailyStartTime"]),
                                    DailyEndTime = SqlReaderConvertor.ToDateTime(dr["DailyEndTime"]),
                                    CampaignDays = SqlReaderConvertor.ToUInt16(dr["CampaignDays"]),
                                    SendLeadSMSCustomer = SqlReaderConvertor.ToBoolean(dr["SendLeadSMSCustomer"])
                                };
                            };

                            if (dr.NextResult())
                            {

                                var lstCampaignPages = new List<ManufacturerCampaignPages>();
                                while (dr.Read())
                                {
                                    lstCampaignPages.Add(new ManufacturerCampaignPages()
                                    {
                                        PageId= SqlReaderConvertor.ToInt32(dr["PageId"]),
                                        PageName= Convert.ToString(dr["PageName"]),
                                        IsSelected= SqlReaderConvertor.ToBoolean(dr["IsSelected"])
                                    }
                                    );
                                }

                                objEntity.CampaignPages = lstCampaignPages;
                            }
                            dr.Close();
                        }
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
                using (IDbConnection connection = DatabaseHelper.GetReadonlyConnection())
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
                using (IDbConnection connection = DatabaseHelper.GetReadonlyConnection())
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
                using (IDbConnection connection = DatabaseHelper.GetReadonlyConnection())
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
                using (DbCommand cmd = DbFactory.GetDBCommand("getmanufacturercampaignrules"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_campaignid", DbType.Int32, campaignId));
                    mfgRules = new ManufacturerCampaignRulesWrapper();
                    ICollection<ManufacturerRuleEntity> campaignRules =  new Collection<ManufacturerRuleEntity>();
                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {

                            while (dr.Read())
                            {
                                campaignRules.Add(new ManufacturerRuleEntity()
                                {
                                    ModelId = SqlReaderConvertor.ToUInt32(dr["ModelId"]),
                                    MakeId = SqlReaderConvertor.ToUInt32(dr["MakeId"]),
                                    MakeName = Convert.ToString(dr["MakeName"]),
                                    ModelName = Convert.ToString(dr["ModelName"]),
                                    CityId = SqlReaderConvertor.ToUInt32(dr["CityId"]),
                                    CityName = Convert.ToString(dr["CityName"]),
                                    StateId = SqlReaderConvertor.ToUInt32(dr["StateId"]),
                                    StateName = Convert.ToString(dr["StateName"])
                                });
                            }
                            if (dr.NextResult() && dr.Read())
                            {
                                mfgRules.ShowOnExShowroom = SqlReaderConvertor.ToBoolean(dr["ShowCampaignOnExshowroom"]);
                            }
                            dr.Close();
                        }
                    }
                    mfgRules.ManufacturerCampaignRules = campaignRules;
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
        /// Modifier    : Kartik Rathod on 14 may 2018, added par_sendleadsmscustomer in savemanufacturercampaign_14052018 to send or not send sms to customer on lead submmision es only
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
                    param.Add("par_sendleadsmscustomer", objCampaign.SendLeadSMSCustomer);

                    connection.Query<dynamic>("savemanufacturercampaign_14052018", param: param, commandType: CommandType.StoredProcedure);
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
        /// Modifier    : Kartik on 17 may 2018, replace getmanufacturercampaignbymodelcity_07032018 with getmanufacturercampaignbymodelcity_17052018  fetched SendLeadSMSCustomer
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
                    cmd.CommandText = "getmanufacturercampaignbymodelcity_17052018";
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
                                config.LeadCampaign.SendLeadSMSCustomer = Utility.SqlReaderConvertor.ToBoolean(dr["SendLeadSMSCustomer"]);
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
                                config.EMICampaign.SendLeadSMSCustomer = Utility.SqlReaderConvertor.ToBoolean(dr["SendLeadSMSCustomer"]);
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
                
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "savemanufacturerpqlead";

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_dealerid", DbType.UInt32, dealerid));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_pqid", DbType.UInt32, pqId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_customerid", DbType.UInt32, customerId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_customername", DbType.String, customerName));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_customeremail", DbType.String, customerEmail));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_customermobile", DbType.String, customerMobile));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_leadsourceid", DbType.UInt32, leadSourceId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_utma", DbType.String, utma));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_utmz", DbType.String, utmz));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_deviceid", DbType.String, deviceId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_campaignId", DbType.UInt32, campaignId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_leadId", DbType.UInt32, ParameterDirection.Output, leadId));
                    MySqlDatabase.ExecuteNonQuery(cmd, ConnectionType.MasterDatabase);
                    retLeadId = Convert.ToUInt32(cmd.Parameters["par_leadId"].Value);
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
        
        /// <summary>
        /// Created By  : Rajan Chauhan on 4 May 2018
        /// Description : Return UnmappedHondaModels if dealerId belong to honda dealer 
        /// </summary>
        /// <param name="dealerId"></param>
        /// <returns></returns>
        public IEnumerable<BikeModelEntity> GetUnmappedHondaModels(uint dealerId)
        {
            IEnumerable<BikeModelEntity> unmappedModels = null;
            try
            {
                using (IDbConnection connection = DatabaseHelper.GetReadonlyConnection())
                {
                    var param = new DynamicParameters();
                    param.Add("par_dealerid", dealerId);
                    unmappedModels = connection.Query<BikeModelEntity>("getmissinghondamodelmapping", param: param, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("ManufacturerCampaignRepository.GetUnmappedHondaModels({0})", dealerId));
            }
            return unmappedModels;
        }
    }
}

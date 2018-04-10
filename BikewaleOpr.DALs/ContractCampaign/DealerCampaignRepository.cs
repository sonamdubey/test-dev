using Bikewale.DAL.CoreDAL;
using Bikewale.Notifications;
using Bikewale.Utility;
using BikewaleOpr.Entities;
using BikewaleOpr.Entity.ContractCampaign;
using BikewaleOpr.Entity.DealerCampaign;
using BikewaleOpr.Interface.ContractCampaign;
using Dapper;
using MySql.CoreDAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;

namespace BikewaleOpr.DALs.ContractCampaign
{
    /// <summary>
    /// Created by  :   Sumit Kate on 29 Dec 2016
    /// Description :   DealerCampaign Repository
    /// </summary>
    public class DealerCampaignRepository : IDealerCampaignRepository
    {
        /// <summary>
        /// Created by  :   Sumit Kate on 29 Dec 2016
        /// Description :   Fetch Dealer Campaign
        /// </summary>
        /// <param name="campaignId"></param>
        /// <returns></returns>
        public DealerCampaignEntity FetchBWDealerCampaign(uint campaignId)
        {
            DealerCampaignEntity dealerCampaign = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("bw_fetchbwdealercampaign_29122016"))
                {

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_campaignid", DbType.Int32, campaignId));


                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.MasterDatabase))
                    {
                        if (dr != null)
                        {
                            if (dr.Read())
                            {
                                dealerCampaign = new DealerCampaignEntity();
                                dealerCampaign.CampaignId = SqlReaderConvertor.ToInt32(dr["campaignid"]);
                                dealerCampaign.CampaignName = Convert.ToString(dr["dealername"]);
                                dealerCampaign.EmailId = Convert.ToString(dr["dealeremailid"]);
                                dealerCampaign.MaskingNumber = Convert.ToString(dr["number"]);
                                dealerCampaign.ServingRadius = SqlReaderConvertor.ToInt32(dr["dealerleadservingradius"]);
                                dealerCampaign.DailyLeadLimit = SqlReaderConvertor.ToUInt32(dr["dailyleadlimit"]);
                                dealerCampaign.CallToAction = SqlReaderConvertor.ToUInt16(dr["calltoaction"]);
                                dealerCampaign.DealerMobile = Convert.ToString(dr["dealermobile"]);
                            }
                            dr.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("DealerCampaignRepository.FetchBWDealerCampaign({0})", campaignId));
            }
            return dealerCampaign;
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 29 Dec 2016
        /// Description :   Fetch Dealer Call To Actions
        /// </summary>
        /// <returns></returns>
        public ICollection<CallToActionEntityBase> FetchDealerCallToActions()
        {
            ICollection<CallToActionEntityBase> callToActions = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("fetchdealercalltoactions"))
                {

                    cmd.CommandType = CommandType.StoredProcedure;
                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.MasterDatabase))
                    {
                        if (dr != null)
                        {
                            callToActions = new List<CallToActionEntityBase>();
                            while (dr.Read())
                            {
                                callToActions.Add(
                                    new CallToActionEntityBase()
                                    {
                                        Id = SqlReaderConvertor.ToUInt16(dr["id"]),
                                        DisplayTextSmall = Convert.ToString(dr["displaytextsmall"]),
                                        DisplayTextLarge = Convert.ToString(dr["displaytextlarge"])
                                    }
                                    );
                            }
                            dr.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "DealerCampaignRepository.FetchDealerCallToActions()");
            }
            return callToActions;
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 29 Dec 2016
        /// Description :   Update Dealer Campaign
        /// Modified by :   Sumit Kate on 12 May 2017
        /// Description :   Modified the SP and removed dealer lead serving radius parameter
        /// </summary>
        /// <param name="isActive"></param>
        /// <param name="campaignId"></param>
        /// <param name="userId"></param>
        /// <param name="dealerId"></param>
        /// <param name="contractId"></param>
        /// <param name="dealerLeadServingRadius"></param>
        /// <param name="maskingNumber"></param>
        /// <param name="dealerName"></param>
        /// <param name="dealerEmailId"></param>
        /// <param name="dailyleadlimit"></param>
        /// <param name="callToAction"></param>
        /// <param name="isBookingAvailable"></param>
        /// <returns></returns>
        public bool UpdateBWDealerCampaign(bool isActive, int campaignId, int userId, int dealerId, int contractId, string maskingNumber, string dealerName, string dealerEmailId, int dailyleadlimit, ushort callToAction, bool isBookingAvailable = false)
        {
            bool isSuccess = false;

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("updatedealercampaign_12052017"))
                {

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_campaignid", DbType.Int32, campaignId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_dealername", DbType.String, 200, dealerName));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_phone", DbType.String, 50, maskingNumber));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_dealeremail", DbType.String, 200, dealerEmailId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_dailyleadlimit", DbType.Int32, dailyleadlimit > 0 ? dailyleadlimit : Convert.DBNull));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_isbookingavailable", DbType.Boolean, isBookingAvailable));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_isactive", DbType.Boolean, isActive));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_updatedby", DbType.Int32, userId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_contractid", DbType.Int32, contractId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_calltoaction", DbType.Int16, callToAction));
                    isSuccess = MySqlDatabase.UpdateQuery(cmd, ConnectionType.MasterDatabase);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "DealerCampaignRepository.UpdateBWDealerCampaign");
                
            }

            return isSuccess;
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 29 Dec 2016
        /// Description :   Insert new Dealer Campaign
        /// Modified by :   Sumit Kate on 12 May 2017
        /// Description :   Modified the SP and removed dealer lead serving radius parameter
        /// </summary>
        /// <param name="isActive"></param>
        /// <param name="userId"></param>
        /// <param name="dealerId"></param>
        /// <param name="contractId"></param>
        /// <param name="dealerLeadServingRadius"></param>
        /// <param name="maskingNumber"></param>
        /// <param name="dealerName"></param>
        /// <param name="dealerEmailId"></param>
        /// <param name="dailyleadlimit"></param>
        /// <param name="callToAction"></param>
        /// <param name="isBookingAvailable"></param>
        /// <returns></returns>
        public int InsertBWDealerCampaign(bool isActive, int userId, int dealerId, int contractId, string maskingNumber, string dealerName, string dealerEmailId, int dailyleadlimit, ushort callToAction, bool isBookingAvailable = false)
        {
            int newCampaignId = 0;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("insertdealercampaign_12052017"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_dealername", DbType.String, 200, dealerName));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_phone", DbType.String, 50, maskingNumber));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_dealeremail", DbType.String, 200, dealerEmailId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_updatedby", DbType.Int32, userId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_isactive", DbType.Boolean, isActive));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_contractid", DbType.Int32, contractId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_dailyleadlimit", DbType.Int32, dailyleadlimit > 0 ? dailyleadlimit : Convert.DBNull));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_isbookingavailable", DbType.Boolean, isBookingAvailable));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_dealerid", DbType.Int32, dealerId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_calltoaction", DbType.Int16, callToAction));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_newcampaignid", DbType.Int32, ParameterDirection.Output));
                    MySqlDatabase.ExecuteNonQuery(cmd, ConnectionType.MasterDatabase);
                    newCampaignId = SqlReaderConvertor.ToInt32(cmd.Parameters["par_newcampaignid"].Value);

                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "DealerCampaignRepository.InsertBWDealerCampaign");
                
            }

            return newCampaignId;
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 18 Jan 2017
        /// Description :   Calls opr_getmakesbydealercity
        /// </summary>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public ICollection<BikeMakeEntityBase> MakesByDealerCity(uint cityId)
        {
            ICollection<BikeMakeEntityBase> callToActions = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("opr_getmakesbydealercity"))
                {

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_cityid", DbType.Int32, cityId));
                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.MasterDatabase))
                    {
                        if (dr != null)
                        {
                            callToActions = new List<BikeMakeEntityBase>();
                            while (dr.Read())
                            {
                                callToActions.Add(
                                    new BikeMakeEntityBase()
                                    {
                                        MakeId = SqlReaderConvertor.ToUInt16(dr["id"]),
                                        MakeName = Convert.ToString(dr["Name"])
                                    }
                                    );
                            }
                            dr.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("DealerCampaignRepository.MakesByDealerCity({0})", cityId));
            }
            return callToActions;
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 18 jan 2017
        /// Description :   Calls opr_getdealerbymakecity
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="makeId"></param>
        /// <param name="activecontract"></param>
        /// <returns></returns>
        public ICollection<DealerEntityBase> DealersByMakeCity(uint cityId, uint makeId, bool activecontract)
        {
            ICollection<DealerEntityBase> callToActions = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("opr_getdealerbymakecity"))
                {
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_cityid", DbType.Int32, cityId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_makeid", DbType.Int32, makeId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_activecontract", DbType.Int16, activecontract ? 1 : 0));
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.MasterDatabase))
                    {
                        if (dr != null)
                        {
                            callToActions = new List<DealerEntityBase>();
                            while (dr.Read())
                            {
                                callToActions.Add(
                                    new DealerEntityBase()
                                    {
                                        Id = SqlReaderConvertor.ToUInt16(dr["dealerId"]),
                                        Name = Convert.ToString(dr["Organization"])
                                    }
                                    );
                            }
                            dr.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("DealerCampaignRepository.DealersByMakeCity({0},{1},{2})", cityId, makeId, activecontract));
            }
            return callToActions;
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 18 jan 2017
        /// Description :   Call opr_getdealercampaigns
        /// Modified by :   Sumit Kate on 12 May 2017
        /// Description :   Populate CampaignServingStatus property
        /// </summary>
        /// <param name="dealerId"></param>
        /// <param name="activecontract"></param>
        /// <returns></returns>
        public ICollection<DealerCampaignDetailsEntity> DealerCampaigns(uint dealerId, uint cityId, uint makeId, bool activecontract)
        {
            ICollection<DealerCampaignDetailsEntity> callToActions = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("opr_getdealercampaigns_31052017"))
                {
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_dealerid", DbType.Int32, dealerId > 0 ? dealerId : Convert.DBNull));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_cityid", DbType.Int32, cityId > 0 ? cityId : Convert.DBNull));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_makeid", DbType.Int32, makeId > 0 ? makeId : Convert.DBNull));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_activecontract", DbType.Int16, activecontract ? 1 : 0));
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.MasterDatabase))
                    {
                        if (dr != null)
                        {
                            callToActions = new List<DealerCampaignDetailsEntity>();
                            while (dr.Read())
                            {
                                callToActions.Add(
                                    new DealerCampaignDetailsEntity()
                                    {
                                        CampaignId = SqlReaderConvertor.ToUInt16(dr["campaignid"]),
                                        CampaignName = Convert.ToString(dr["campaignname"]),
                                        MaskingNumber = Convert.ToString(dr["maskingnumber"]),
                                        EmailId = Convert.ToString(dr["campaignemailid"]),
                                        ServingRadius = SqlReaderConvertor.ToInt32(dr["CampaignLeadServingRadius"]),
                                        ContractID = SqlReaderConvertor.ToUInt32(dr["ContractId"]),
                                        DealerId = SqlReaderConvertor.ToUInt32(dr["DealerId"]),
                                        PackageName = Convert.ToString(dr["packagename"]),
                                        ContractStartDate = SqlReaderConvertor.ToDateTime(dr["startdate"]),
                                        ContractEndDate = SqlReaderConvertor.ToDateTime(dr["enddate"]),
                                        ContractStatus = SqlReaderConvertor.ToUInt32(dr["contractstatus"]),
                                        ContractStatusText = Convert.ToString(dr["status"]),
                                        DailyLeadLimit = SqlReaderConvertor.ToUInt32(dr["dailyleadlimit"]),
                                        DailyLeads = SqlReaderConvertor.ToUInt32(dr["dailyleads"]),
                                        RulesCount = SqlReaderConvertor.ToUInt32(dr["NoOfRules"]),
                                        CampaignServingStatus = Convert.ToString(dr["campaignservingstatus"]),
                                        DealerName = Convert.ToString(dr["organization"])
                                    }
                                    );
                            }
                            dr.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("DealerCampaignRepository.DealerCampaigns({0},{1})", dealerId, activecontract));
            }
            return callToActions;
        }


        #region GetMappedDealerCampaignAreas method
        /// <summary>
        /// Written By : Ashish G. Kamble on 15 May 2017
        /// Summary : Function to get all mapped areas to a particular dealer's location
        /// </summary>
        /// <param name="dealerId"></param>
        /// <returns></returns>        
        public DealerCampaignArea GetMappedDealerCampaignAreas(uint dealerId)
        {
            DealerCampaignArea objDealerCampaignArea = null;

            try
            {
                using (IDbConnection connection = DatabaseHelper.GetMasterConnection())
                {

                    var param = new DynamicParameters();

                    param.Add("par_dealerId", dealerId);

                    objDealerCampaignArea = new DealerCampaignArea();
                    using (var results = connection.QueryMultiple("GetMappedDealerCampaignAreas", param: param, commandType: CommandType.StoredProcedure))
                    {

                        objDealerCampaignArea.DealerName = results.Read<string>().SingleOrDefault();
                        objDealerCampaignArea.Areas = results.Read<CampaignAreas>();
                    }

                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "BikewaleOpr.DALs.UserReviews.GetMappedDealerCampaignAreas");
            }

            return objDealerCampaignArea;
        }
        #endregion  // End of GetMappedDealerCampaignAreas


        #region SaveDealerCampaignAreaMapping method
        /// <summary>
        /// Written By : Ashish G. Kamble on 15 May 2017
        /// Summary : Function to save the dealer's location to the list of areas for the given campaign
        /// </summary>
        /// <param name="dealerId"></param>
        /// <param name="campaignServingStatus">Status of the serving areas to the particular campaign.</param>
        /// <param name="servingRadius">Serving radius for the given dealer (campaign serving radius).</param>
        /// <param name="cityIdList">Comma separated city id list. e.g. cityid1, cityid2, cityid3</param>
        public void SaveDealerCampaignAreaMapping(uint dealerId,uint campaignid, ushort campaignServingStatus, ushort servingRadius, string cityIdList, string stateIdList)
        {
            try
            {
                using (IDbConnection connection = DatabaseHelper.GetMasterConnection())
                {
                    
                    var param = new DynamicParameters();

                    param.Add("par_dealerid", dealerId);
                    param.Add("par_campaignid", campaignid);
                    param.Add("par_campaignServingStatus", campaignServingStatus);                    
                    param.Add("par_servingRadius", servingRadius);
                    param.Add("par_cityIds", cityIdList);
                    param.Add("par_stateIds", stateIdList);

                    connection.Query("SaveDealerCampaignAreasMapping", param: param, commandType: CommandType.StoredProcedure);

                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "BikewaleOpr.DALs.UserReviews.SaveDealerCampaignAreaMapping");
            }
        }   // end of SaveDealerCampaignAreaMapping 
        #endregion


        #region GetDealerToAreasDistance method
        /// <summary>
        /// Written By : Ashish G. Kamble on 15 May 2017
        /// Summary : Function to get the distance of dealer's location to areas in within serving radius and based on the campaign serving status. 
        /// Distance calculated on basis of haversine formula.
        /// </summary>
        /// <param name="dealerId"></param>
        /// <param name="campaignServingStatus">Status of the serving areas to the particular campaign.</param>
        /// <param name="servingRadius">Serving radius for the given dealer (campaign serving radius).</param>
        /// <param name="cityIdList">Comma separated city id list. e.g. cityid1, cityid2, cityid3</param>
        /// <returns></returns>
        public DealerAreaDistance GetDealerToAreasDistance(uint dealerId, ushort campaignServingStatus, ushort servingRadius, string cityIdList, string stateIdList)
        {
            DealerAreaDistance objDistances = null;
            
            try
            {
                using (IDbConnection connection = DatabaseHelper.GetMasterConnection())
                {

                    var param = new DynamicParameters();

                    param.Add("par_dealerid", dealerId);
                    param.Add("par_campaignServingStatus", campaignServingStatus);
                    param.Add("par_leadservingdistance", servingRadius);
                    param.Add("par_cityIds", cityIdList);
                    param.Add("par_stateIds", stateIdList);

                    objDistances = new DealerAreaDistance();

                    using (var results = connection.QueryMultiple("GetDealerToAreasDistance", param: param, commandType: CommandType.StoredProcedure))
                    {
                        objDistances.DealerLocation = results.Read<GeoLocationEntity>().SingleOrDefault();
                        objDistances.Areas = results.Read<GeoLocationEntity>();
                    }

                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "BikewaleOpr.DALs.UserReviews.GetDealerToAreasDistance");
            }

            return objDistances;
        }   // End of GetDealerToAreasDistance 
        #endregion


        #region GetDealerAreasWithLatLong method
        /// <summary>
        /// Written By : Ashish G. Kamble on 15 May 2017
        /// Summary : Function to get the additional areas to the dealer's location along with latitude and longitude.
        /// </summary>
        /// <param name="dealerId"></param>
        /// <param name="areasList">Provide list in (,) separated format separated by comma e.g. 'areaid1,areaid2'</param>
        public DealerAreaDistance GetDealerAreasWithLatLong(uint dealerId, string areasList)
        {
            DealerAreaDistance objDistances = null;

            try
            {
                using (IDbConnection connection = DatabaseHelper.GetMasterConnection())
                {


                    var param = new DynamicParameters();

                    param.Add("par_dealerid", dealerId);
                    param.Add("par_areaslist", areasList);

                    using (var results = connection.QueryMultiple("GetDealerAreasWithLatLong", param: param, commandType: CommandType.StoredProcedure))
                    {
                        objDistances = new DealerAreaDistance();
                        objDistances.DealerLocation = results.Read<GeoLocationEntity>().SingleOrDefault();
                        objDistances.Areas = results.Read<GeoLocationEntity>();
                    }


                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "BikewaleOpr.DALs.UserReviews.SaveAdditionalAreasMapping");
            }

            return objDistances;

        }   // End of GetDealerAreasWithLatLong 
        #endregion


        #region SaveAdditionalAreasMapping method
        /// <summary>
        /// Written By : Ashish G. Kamble on 15 May 2017
        /// Summary : Function to map the additional areas to the dealer's location along with distance between them. Distance calculated based on google distance api.
        /// </summary>
        /// <param name="dealerId"></param>
        /// <param name="areasList">Provide list in (,) separated format separated by comma e.g. 'areaid1,areaid2'</param>
        public void SaveAdditionalAreasMapping(uint dealerId, string areasList)
        {
            try
            {
                using (IDbConnection connection = DatabaseHelper.GetMasterConnection())
                {
                    var param = new DynamicParameters();

                    param.Add("par_dealerid", dealerId);
                    param.Add("par_arealist", areasList);

                    connection.Query("SaveAdditionalAreasMapping", param: param, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "BikewaleOpr.DALs.UserReviews.SaveAdditionalAreasMapping");
            }
        }   // End of SaveAdditionalAreasMapping 
        #endregion


        #region DeleteAdditionalMappedAreas method
        /// <summary>
        /// Written By : Ashish G. Kamble on 15 May 2017
        /// Summary : Function to delete the additionally mapped areas to the dealer's location.
        /// </summary>
        /// <param name="dealerId"></param>
        /// <param name="areadIdList">Comma separated area id list. e.g. areaid1, areaid2, areaid3</param>
        public void DeleteAdditionalMappedAreas(uint dealerId, string areadIdList)
        {
            try
            {
                using (IDbConnection connection = DatabaseHelper.GetMasterConnection())
                {
                    var param = new DynamicParameters();

                    param.Add("par_dealerId", dealerId);
                    param.Add("par_areaIds", areadIdList);

                    connection.Query("deleteAdditionalMappedAreas", param: param, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "BikewaleOpr.DALs.UserReviews.DeleteAdditionalMappedAreas");
            }
        }   // End of DeleteAdditionalMappedAreas 
        #endregion

    }   // class
}   // namespace

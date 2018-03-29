using Bikewale.DAL.CoreDAL;
using Bikewale.Notifications;
using Bikewale.Utility;
using BikewaleOpr.Entities;
using BikewaleOpr.Entity.ContractCampaign;
using BikewaleOpr.Entity.Dealers;
using BikewaleOpr.Interface;
using Dapper;
using MySql.CoreDAL;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Common;
using System.Web;

namespace BikewaleOpr.DAL
{

    public class DealersRepository : IDealers
    {


        /// <summary>
        /// Written By :Snehal Dange on 5th August 2017
        /// Summary : Function to get all facilities provided by the dealer.
        /// </summary>
        /// <param name="dealerId">Id of the dealer whose facilities are required</param>
        /// <returns>Returns list of the facilities for the given dealer id</returns>

        public IEnumerable<FacilityEntity> GetDealerFacilities(uint dealerId)
        {
            IEnumerable<FacilityEntity> objFacilities = null;
            try
            {
                if (dealerId > 0)
                {
                    using (IDbConnection connection = DatabaseHelper.GetMasterConnection())
                    {
                        var param = new DynamicParameters();
                        param.Add("par_DealerId", dealerId);

                        objFacilities = connection.Query<FacilityEntity>("BW_GetDealerFacilities", param: param, commandType: CommandType.StoredProcedure);

                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("BikewaleOpr.DALs.ServiceCenter.GetDealerFacilities ,DealerId:{0}", dealerId));
            }
            return objFacilities;
        }



        /// <summary>
        /// Written By : Ashish G. Kamble on 7 Nov 2014
        /// Summary : Function to save the dealer facility
        /// Modified by: Snehal Dange on 7th August 2017
        /// Description: Changed Input type from individual parameters to entity.Added parameters 'par_updatedby' ,'par_latestInsertId' , 
        /// </summary>
        /// <param name="objData"></param>
        public UInt16 SaveDealerFacility(FacilityEntity objData)
        {
            UInt16 newID = 0;
            try
            {
                using (IDbConnection connection = DatabaseHelper.GetMasterConnection())
                {
                    var param = new DynamicParameters();
                    param.Add("par_Facility", objData.Facility);
                    param.Add("par_IsActive", Convert.ToUInt16(objData.IsActive));
                    param.Add("par_DealerId", objData.Id);
                    param.Add("par_updatedby", objData.LastUpdatedById);
                    param.Add("par_latestInsertId", dbType: DbType.UInt16, direction: ParameterDirection.Output);
                    connection.Execute("bw_savedealerfacility", param: param, commandType: CommandType.StoredProcedure);
                    newID = param.Get<UInt16>("par_latestInsertId");
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("BikewaleOpr.DAL.SaveDealerFacility DealerId: {0} Facility: {1} FacilityId: {2} ActiveStatus: {3}", objData.Id, objData.Facility, newID, objData.IsActive));
            }

            return newID;
        }

        /// <summary>
        /// Written By : Ashish G. Kamble on 7 Nov 2014
        /// Summary : Function to update the dealer facility.
        /// Modified by: Snehal Dange on 7th August 2017
        /// Description: Changed Input type from individual parameters to entity. Added parameter 'par_updatedby'.
        /// </summary>

        public bool UpdateDealerFacility(FacilityEntity objData)
        {
            byte status = 0;
            try
            {
                using (IDbConnection connection = DatabaseHelper.GetMasterConnection())

                {
                    var param = new DynamicParameters();
                    param.Add("par_facility", objData.Facility);
                    param.Add("par_isactive", Convert.ToUInt16(objData.IsActive));
                    param.Add("par_facilityid", objData.FacilityId);
                    param.Add("par_updatedby", objData.LastUpdatedById);

                    status = (byte)connection.Execute("BW_UpdateDealerFacility", param: param, commandType: CommandType.StoredProcedure);

                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("BikewaleOpr.DAL.UpdateDealerFacility DealerId: {0} Facility: {1} FacilityId: {2} ActiveStatus: {3}", objData.Id, objData.Facility, objData.FacilityId, objData.IsActive));

            }

            return status > 0;
        }

        
        /// <summary>
        /// Written By : Ashish G. Kamble on 9 Nov 2014
        /// Summary : Function to get the dealer loan amounts for the given dealer id.
        /// Modified by :   Sumit Kate on 11 Mar 2016
        /// Description :   Updated the SP and populate the EMI entity with newly added column values
        /// Modified by :   Sangram Nandkhile on 15 Mar 2016
        /// Description :   commented few parameters and added minLtv and MaxLtv/// </summary>
        /// <param name="dealerId"></param>
        /// <returns></returns>
        public EMI GetDealerLoanAmounts(uint dealerId)
        {
            EMI objEmi = null;

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("bw_getdealerloanamounts_10032016"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_DealerId", DbType.Int32, dealerId));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr.Read())
                        {
                            objEmi = new EMI()
                            {
                                LoanToValue = !Convert.IsDBNull(dr["LTV"]) ? Convert.ToUInt16(dr["LTV"]) : default(UInt16),
                                RateOfInterest = !Convert.IsDBNull(dr["RateOfInterest"]) ? Convert.ToSingle(dr["RateOfInterest"]) : default(float),
                                Tenure = !Convert.IsDBNull(dr["Tenure"]) ? Convert.ToUInt16(dr["Tenure"]) : default(ushort),
                                LoanProvider = !Convert.IsDBNull(dr["LoanProvider"]) ? Convert.ToString(dr["LoanProvider"]) : default(string),
                                MinDownPayment = !Convert.IsDBNull(dr["MinDownPayment"]) ? Convert.ToSingle(dr["MinDownPayment"]) : default(float),
                                MaxDownPayment = !Convert.IsDBNull(dr["MaxDownPayment"]) ? Convert.ToSingle(dr["MaxDownPayment"]) : default(float),
                                MinTenure = !Convert.IsDBNull(dr["MinTenure"]) ? Convert.ToUInt16(dr["MinTenure"]) : default(UInt16),
                                MaxTenure = !Convert.IsDBNull(dr["MaxTenure"]) ? Convert.ToUInt16(dr["MaxTenure"]) : default(UInt16),
                                MinRateOfInterest = !Convert.IsDBNull(dr["MinRateOfInterest"]) ? Convert.ToSingle(dr["MinRateOfInterest"]) : default(float),
                                MaxRateOfInterest = !Convert.IsDBNull(dr["MaxRateOfInterest"]) ? Convert.ToSingle(dr["MaxRateOfInterest"]) : default(float),
                                MinLoanToValue = !Convert.IsDBNull(dr["minLtv"]) ? Convert.ToUInt32(dr["minLtv"]) : default(uint),
                                MaxLoanToValue = !Convert.IsDBNull(dr["maxLtv"]) ? Convert.ToUInt32(dr["maxLtv"]) : default(uint),
                                ProcessingFee = !Convert.IsDBNull(dr["ProcessingFee"]) ? Convert.ToSingle(dr["ProcessingFee"]) : default(float),
                                Id = Convert.ToUInt32(dr["Id"])
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "GetDealerLoanAmounts");
                
            }
            return objEmi;
        }

        /// <summary>
        /// Written By : Suresh Prajapati on 29th Oct 2014.
        /// Modified By :   Vishnu Teja Yalakuntla on 01 Aug 2017
        /// Description : To Get Dealer's Name By Selected City.
        /// </summary>
        /// <param name="cityId"></param>
        /// <returns>Dealer's Name</returns>
        public IEnumerable<DealerMakeEntity> GetDealersByCity(UInt32 cityId)
        {
            IEnumerable<DealerMakeEntity> dealers = null;

            try
            {
                using (IDbConnection connection = DatabaseHelper.GetMasterConnection())
                {
                    var param = new DynamicParameters();
                    param.Add("par_cityid", cityId);

                    dealers = connection.Query<DealerMakeEntity>("bw_getbikedealers_01082017", param: param, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("GetDealersByCity cityId={0}", cityId));
            }

            return dealers;
        }


        /// <summary>
        /// Created By : Suresh Prajapati on 03rd Nov 2014
        /// Description : To Get Offer Types for Drop down.
        /// </summary>
        /// <returns>Offer Types</returns>

        public DataTable GetOfferTypes()
        {

            DataTable dt = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("BW_GetOfferTypes"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    dt = MySqlDatabase.SelectAdapterQuery(cmd, ConnectionType.ReadOnly).Tables[0];
                }
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("GetOfferTypes ex : " + ex.Message + ex.Source);
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                
            }
            return dt;
        }

        /// <summary>
        /// Created By  : Suresh Prajapati on 03rd Nov, 2014
        /// Description : To Get Dealer Offer Details for particular Dealer.
        /// 
        /// Modified By : Suresh Prajapati on 30th Dec, 2014
        /// Description : Added City name column in the list
        /// 
        ///Modified By : Aditi Srivastava on 3rd Aug, 2016
        /// Description : Added terms and conditions parameter in  offerEntity
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="dealerId"></param>
        /// <returns></returns>

        public List<OfferEntity> GetDealerOffers(int dealerId)
        {

            List<OfferEntity> objOffers = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("bw_getdealeroffers_07012016"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_dealerId", DbType.Int32, dealerId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_modelid", DbType.Int32, Convert.DBNull));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            objOffers = new List<OfferEntity>();

                            OfferEntity objOffer = null;

                            //Get offers with details
                            while (dr.Read())
                            {
                                objOffer = new OfferEntity();
                                objOffer.objMake = new BikeMakeEntityBase() { MakeName = Convert.ToString(dr["MakeName"]) };
                                objOffer.objModel = new BikeModelEntityBase() { ModelName = Convert.ToString(dr["ModelName"]) };
                                objOffer.objCity = new CityEntityBase() { CityName = Convert.ToString(dr["CityName"]) };
                                objOffer.OfferId = Convert.ToUInt32(dr["Id"]);
                                objOffer.OfferType = Convert.ToString(dr["OfferType"]);
                                objOffer.OfferTypeId = Convert.ToUInt32(dr["OfferTypeId"]);
                                objOffer.OfferText = Convert.ToString(dr["OfferText"]);
                                objOffer.OfferValue = Convert.ToUInt32(dr["OfferValue"]);
                                if (!String.IsNullOrEmpty(Convert.ToString(dr["OfferValidTill"])))
                                    objOffer.OffervalidTill = DateTime.Parse(Convert.ToString(dr["OfferValidTill"]));
                                objOffer.IsPriceImpact = Convert.ToBoolean(Convert.ToString(dr["IsPriceImpact"]));
                                if (!String.IsNullOrEmpty(Convert.ToString(dr["Terms"])))
                                    objOffer.Terms = Convert.ToString(dr["Terms"]);

                                objOffers.Add(objOffer);
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("GetDealerOffers ex : " + ex.Message + ex.Source);
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                
            }
            return objOffers;
        }

        /// <summary>
        /// Created By  : Suresh Prajapati on 05th Nov, 2014.
        /// Description : To Save a New Offer by Dealer.
        /// 
        /// Modified By : Suresh Prajapati on 30th Dec, 2014.
        /// Description : Added UserId saving feature for saved dealer offer.
        /// 
        /// Modified By: Aditi Srivastava on 8th Aug, 2016
        /// Description: To save terms and conditions enclosed in <ol><li> tags
        /// 
        /// </summary>
        /// <param name="dealerId"></param>
        /// <param name="cityId"></param>
        /// <param name="modelId"></param>
        /// <param name="offercategoryId"></param>
        /// <param name="offerText"></param>
        /// <param name="offerValue"></param>
        /// <param name="offervalidTill"></param>
        /// <returns></returns>

        public bool SaveDealerOffer(int dealerId, uint userId, int cityId, string modelId, int offercategoryId, string offerText, int? offerValue, DateTime offervalidTill, bool isPriceImpact, string terms)
        {

            bool isSuccess = false;


            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("bw_savedealeroffers_07012016"))
                {

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_dealerId", DbType.Int32, dealerId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_cityId", DbType.Int32, cityId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_modelId", DbType.String, -1, modelId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_offercategoryId", DbType.Int32, offercategoryId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_offerText", DbType.String, -1, HttpContext.Current.Server.HtmlDecode(offerText)));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_offerValue", DbType.Int32, offerValue));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_UserId", DbType.Int32, userId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_offerValidTill", DbType.DateTime, offervalidTill));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_isPriceImpact", DbType.Boolean, isPriceImpact));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_IsActive", DbType.Boolean, Convert.DBNull));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_result", DbType.Byte, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_terms", DbType.String, -1, terms));

                    MySqlDatabase.InsertQuery(cmd, ConnectionType.MasterDatabase);
                    isSuccess = Convert.ToBoolean(cmd.Parameters["par_result"].Value);

                }
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("SaveDealerOffer ex : " + ex.Message + ex.Source);
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                
            }
            return isSuccess;
        }

        /// <summary>
        /// Created By  : Suresh Prajapati on 28th Dec, 2014.
        /// Description : To Edit/Update added Bike offers
        /// 
        /// Modified By: Aditi Srivastava
        /// Description: Added html formatting for updating terms and conditions
        /// </summary>
        /// <param name="offerId"></param>
        /// <param name="userId"></param>
        /// <param name="offerCategoryId"></param>
        /// <param name="offerText"></param>
        /// <param name="offerValue"></param>
        /// <param name="offerValidTill"></param>
        public bool UpdateDealerBikeOffers(DealerOffersEntity dealerOffers)
        {
            bool isUpdated = false;

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("bw_updatedealeroffers_07012016"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_OfferId", DbType.Int32, dealerOffers.OfferId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_UserId", DbType.Int64, dealerOffers.UserId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_OfferCategoryId", DbType.Int32, dealerOffers.OfferCategoryId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_OfferText", DbType.String, dealerOffers.OfferText));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_OfferValue", DbType.Int32, dealerOffers.OfferValue));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_OfferValidTill", DbType.DateTime, dealerOffers.OfferValidTill));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_isPriceImpact", DbType.Boolean, dealerOffers.IsPriceImpact));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_terms", DbType.String, dealerOffers.Terms));
                    MySqlDatabase.UpdateQuery(cmd, ConnectionType.MasterDatabase);
                    isUpdated = true;

                }
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("Exception at UpdateDealerBikeOffers : " + ex.Message + ex.Source);
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                
            }
            return isUpdated;
        }
        /// <summary>
        /// Created By  : Suresh Prajapati on 04th Nov, 2014.
        /// Description : To Delete an Offer specified by "offerId".
        /// Modified By : Sadhana Upadhyay on 9 Oct 2015
        /// Summary : To delete Multiple offers
        /// </summary>
        /// <param name="offerId"></param>
        /// <returns></returns>
        public bool DeleteDealerOffer(string offerId)
        {
            bool isdeleteSuccess = true;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("bw_deletedealeroffers"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_OfferIds", DbType.String, -1, offerId));
                    MySqlDatabase.UpdateQuery(cmd, ConnectionType.MasterDatabase);
                }
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("DeleteDealerOffer ex : " + ex.Message + ex.Source);
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                
            }
            return isdeleteSuccess;
        }

        /// <summary>
        /// Created By  : Suresh Prajapati on 11th Nov, 2014.
        /// Description : Method to save New Bike Availability.
        /// </summary>
        /// <param name="dealerId"></param>
        /// <param name="bikemodelId"></param>
        /// <param name="bikeversionId"></param>
        /// <param name="numofDays"></param>
        /// <returns></returns>

        public bool SaveBikeAvailability(uint dealerId, uint bikemodelId, uint? bikeversionId, UInt16 numOfDays)
        {

            bool isSuccess = false;

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("bw_savebikeavailability"))
                {

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_DealerId", DbType.Int32, dealerId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_BikeVersionId", DbType.Int32, bikeversionId > 0 ? bikeversionId : Convert.DBNull));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_NumOfDays", DbType.Int32, numOfDays));


                    if (MySqlDatabase.InsertQuery(cmd, ConnectionType.MasterDatabase))
                        isSuccess = true;
                }
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("SaveBikeAvailability ex : " + ex.Message + ex.Source);
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                
            }
            return isSuccess;
        }

        /// <summary>
        /// Created By  : Suresh Prajapati on 11th Nov, 2014.
        /// Modified by :   Vishnu Teja Yalakuntla on 03 Aug 2017
        /// Description : Method to save New Bike Availability.
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public bool SaveVersionAvailability(uint dealerId, string bikeVersionIds, string numberOfDays)
        {
            bool isSaved = false;
            try
            {
                using (IDbConnection connection = DatabaseHelper.GetMasterConnection())
                {
                    var param = new DynamicParameters();
                    param.Add("par_dealerid", dealerId);
                    param.Add("par_bikeversionids", bikeVersionIds);
                    param.Add("par_numofdays", numberOfDays);

                    connection.Execute("bw_savebikeavailability_03082017", param: param, commandType: CommandType.StoredProcedure);
                    isSaved = true;
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format(
                    "SaveVersionAvailability dealerId={0} bikeVersionId={1} numberOfDays={2}", dealerId, bikeVersionIds, numberOfDays));
            }

            return isSaved;
        }
        /// <summary>
        /// Created By  :   Vishnu Teja Yalakuntla on 03 Aug 2017
        /// Description :   Deletes the availability of specified version.
        /// </summary>
        /// <param name="dealerId"></param>
        /// <param name="bikeVersionId"></param>
        /// <returns></returns>
        public bool DeleteVersionAvailability(uint dealerId, string bikeVersionId)
        {
            bool isDeleted = false;
            try
            {
                using (IDbConnection connection = DatabaseHelper.GetMasterConnection())
                {
                    var param = new DynamicParameters();
                    param.Add("par_dealerid", dealerId);
                    param.Add("par_bikeversionids", bikeVersionId);

                    connection.Execute("bw_deletebikeavailability_03082017", param: param, commandType: CommandType.StoredProcedure);
                    isDeleted = true;
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format(
                    "DeleteVersionAvailability dealerId={0} bikeVersionId={1}", dealerId, bikeVersionId));
            }

            return isDeleted;
        }

        /// <summary>
        /// Created By  : Suresh Prajapati On 11th Nov, 2014.
        /// Description : To Get Added Bikes Availability by specific Dealer.
        /// </summary>
        /// <param name="dealerId"></param>
        /// <returns></returns>

        public List<OfferEntity> GetBikeAvailability(uint dealerId)
        {

            List<OfferEntity> objAvailabilities = null;

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("BW_GetBikeAvailabilitiy"))
                {

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_dealerId", DbType.Int32, dealerId));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            objAvailabilities = new List<OfferEntity>();

                            OfferEntity objAvailability = null;

                            //Get offers with details
                            while (dr.Read())
                            {
                                objAvailability = new OfferEntity();
                                objAvailability.AvailabilityId = Convert.ToInt32(dr["ID"]);
                                objAvailability.objMake = new BikeMakeEntityBase() { MakeName = dr["Make"].ToString() };
                                objAvailability.objModel = new BikeModelEntityBase() { ModelName = dr["Model"].ToString() };
                                objAvailability.objVersion = new BikeVersionEntityBase() { VersionName = dr["Version"].ToString() };
                                objAvailability.AvailableLimit = Convert.ToUInt16(dr["AvailableLimit"]);

                                objAvailabilities.Add(objAvailability);
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("GetBikeAvailability ex : " + ex.Message + ex.Source);
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                
            }

            return objAvailabilities;
        }

        /// <summary>
        /// Created By  : Suresh Prajapati On 12th Nov, 2014.
        /// Description : To Edit Availability Limit Days of Bike By Specified Dealer.
        /// </summary>
        /// <param name="availabilityId"></param>
        /// <param name="days"></param>
        /// <returns></returns>

        public bool EditAvailabilityDays(int availabilityId, int days)
        {

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("bw_updatedealerbikeavailability"))
                {

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_AvailabilityId", DbType.Int32, availabilityId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_Days", DbType.Int32, days));

                    return (MySqlDatabase.UpdateQuery(cmd, ConnectionType.MasterDatabase));
                }
            }

            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("EditAvailabilityDays ex : " + ex.Message + ex.Source);
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                
            }
            return false;
        }

        /// <summary>
        /// Created By  : Ashwini Todakar on 13th Nov, 2014.
        /// Description : Method to Get Availability Days for particular Dealer and Bike Version.
        /// </summary>
        /// <param name="dealerId"></param>
        /// <param name="versionId"></param>
        /// <returns></returns>
        public uint GetAvailabilityDays(uint dealerId, uint versionId)
        {

            uint numOfDays = 0;

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("BW_GetAvailabilityDays"))
                {

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_DealerId", DbType.Int32, dealerId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_VersionId", DbType.Int32, versionId));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            if (dr.Read())
                                numOfDays = Convert.ToUInt32(dr["NumOfDays"]);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("GetAvailabilityDays ex : " + ex.Message + ex.Source);
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                
            }

            return numOfDays;
        }

        /// <summary>
        /// Created By  : Suresh Prajapati on 03rd Dec, 2014.
        /// Description : To Get Disclaimer By Specified Dealer ID.
        /// </summary>
        /// <param name="dealerId"></param>
        /// <returns></returns>

        public List<DealerDisclaimerEntity> GetDealerDisclaimer(uint dealerId)
        {
            List<DealerDisclaimerEntity> objDisclaimer = null;

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("bw_getdealerdisclaimer"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_dealerId", DbType.Int32, dealerId));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            objDisclaimer = new List<DealerDisclaimerEntity>();

                            DealerDisclaimerEntity _objDisclaimer = null;

                            //Get offers with details
                            while (dr.Read())
                            {
                                _objDisclaimer = new DealerDisclaimerEntity();
                                _objDisclaimer.DisclaimerId = Convert.ToUInt32(dr["ID"]);
                                _objDisclaimer.objMake = new BikeMakeEntityBase() { MakeName = dr["Make"].ToString() };
                                _objDisclaimer.objModel = new BikeModelEntityBase() { ModelName = dr["Model"].ToString() };
                                _objDisclaimer.objVersion = new BikeVersionEntityBase() { VersionName = dr["Version"].ToString() };
                                _objDisclaimer.DisclaimerText = dr["Disclaimer"].ToString();

                                objDisclaimer.Add(_objDisclaimer);
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("Exception at GetDealerDisclaimer : " + ex.Message + ex.Source);
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                
            }
            return objDisclaimer;
        }

        /// <summary>
        /// Created By  : Suresh Prajapati on 03rd Dec, 2014.
        /// Description : To Save Disclaimer for specified Bike and Dealer
        /// </summary>
        /// <param name="dealerId"></param>
        /// <param name="makeId"></param>
        /// <param name="modelId"></param>
        /// <param name="versionId"></param>
        /// <param name="disclaimer"></param>
        public void SaveDealerDisclaimer(uint dealerId, uint makeId, uint? modelId, uint? versionId, string disclaimer)
        {
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("bw_savedealerdisclaimer"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_DealerId", DbType.Int32, dealerId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_BikeMakeId", DbType.Int32, makeId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_BikeModelId", DbType.Int32, modelId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_BikeVersionId", DbType.Int32, versionId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_Disclaimer", DbType.String, disclaimer));
                    MySqlDatabase.InsertQuery(cmd, ConnectionType.MasterDatabase);
                }
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("Exception at SaveDealerDisclaimer : " + ex.Message + ex.Source);
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                
            }
        }


        /// <summary>
        /// Created By : Sadhana Upadhyay on 3 Dec 2014
        /// Summary : To Delete dealer disclaimer
        /// </summary>
        /// <param name="desclaimerId"></param>
        /// <returns></returns>
        public bool DeleteDealerDisclaimer(uint disclaimerId)
        {
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("bw_deletedealerdisclaimer"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_DisclaimerId", DbType.Int32, disclaimerId));
                    return MySqlDatabase.UpdateQuery(cmd, ConnectionType.MasterDatabase);
                }
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("DeleteDealerDisclaimer ex : " + ex.Message + ex.Source);
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                
            }
            return false;
        }

        /// <summary>
        /// Created By : Suresh Prajapati on 3 Dec 2014
        /// Summary : To edit dealer disclaimer
        /// </summary>
        /// <param name="disclaimerId"></param>
        /// <param name="newDisclaimerText"></param>
        /// <returns></returns>
        public bool EditDisclaimer(uint disclaimerId, string newDisclaimerText)
        {
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("bw_editdealerdisclaimer"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_DisclaimerId", DbType.Int32, disclaimerId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_NewDisclaimer", DbType.String, newDisclaimerText));

                    return MySqlDatabase.UpdateQuery(cmd, ConnectionType.MasterDatabase);
                }
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("EditAvailabilityDays ex : " + ex.Message + ex.Source);
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                
            }

            return false;
        }

        #region Booking Amount functionality

        /// <summary>
        /// Written By : Ashwini Todkar on 17 Dec 2014
        /// Summary    : Method to insert bike booking amount for a dealer
        /// Modified By: Vivek Singh Tomar On 9th Aug 2017
        /// Summary: Changed implementation using dapper and added new required parameters
        /// </summary>
        /// <param name="objBookingAmt"></param>
        /// <param name="updatedBy"></param>
        /// <returns>isrecord inserted</returns>
        public bool SaveBookingAmount(BookingAmountEntity objBookingAmt, UInt32 updatedBy)
        {
            bool isUpdated = false;
            try
            {

                using (IDbConnection connection = DatabaseHelper.GetMasterConnection())
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("par_bookingid", objBookingAmt.BookingAmountBase.Id);
                    param.Add("par_dealerid", objBookingAmt.DealerId);
                    param.Add("par_bikemodelid", objBookingAmt.BikeModel.ModelId);
                    param.Add("par_bikeversionid", objBookingAmt.BikeVersion.VersionId);
                    param.Add("par_amount", objBookingAmt.BookingAmountBase.Amount);
                    param.Add("par_updatedby", updatedBy);

                    connection.Execute("bw_savebookingamount_08072017", param: param, commandType: CommandType.StoredProcedure);
                    isUpdated = true;

                }
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("SaveBookingAmount ex : " + ex.Message + ex.Source);
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                
            }

            return isUpdated;
        }

        /// <summary>
        /// Written By : Ashwini Todkar on 17 Dec 2014
        /// Summary    : Method to update booking amount of a bike 
        /// </summary>
        /// <param name="bookingAmtId">Id of booking amount</param>
        /// <param name="amount">bike booking amount</param>
        /// <returns>isUpdated</returns>
        public bool UpdateBookingAmount(BookingAmountEntityBase objBookingAmt)
        {

            bool isSuccess = false;

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("bw_updatebikebookingamount"))
                {

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_BookingId", DbType.Int32, objBookingAmt.Id));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_BookingAmount", DbType.Int32, objBookingAmt.Amount));


                    if (MySqlDatabase.UpdateQuery(cmd, ConnectionType.MasterDatabase))
                        isSuccess = true;
                }
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("UpdateBookingAmount ex : " + ex.Message + ex.Source);
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                
            }

            return isSuccess;
        }

        /// <summary>
        /// Written By : Ashwini Todkar on 17 Dec 2014
        /// Summary    : Method to get bike booking details
        /// Modified by : Vivek Singh Tomar
        /// Summary : Implemented dapper
        /// </summary>
        /// <param name="dealerId"></param>
        /// <returns></returns>
        public IEnumerable<BookingAmountEntity> GetBikeBookingAmount(uint dealerId)
        {
            IEnumerable<BookingAmountEntity> objBookingAmt = null;

            try
            {
                using (IDbConnection connection = DatabaseHelper.GetMasterConnection())
                {
                    var param = new DynamicParameters();
                    param.Add("par_dealerid", dealerId);
                    objBookingAmt = connection.Query<BookingAmountEntityBase, BikeMakeEntityBase, BikeModelEntityBase,
                                        BikeVersionEntityBase, BookingAmountEntity, BookingAmountEntity>
                                    (
                                        "bw_getbikebookingamount_05082017",
                                        (bookingAmountBase, bikeMake, bikeModel, bikeVersion, bookingAmount) =>
                                        {
                                            bookingAmount.BookingAmountBase = bookingAmountBase;
                                            bookingAmount.BikeMake = bikeMake;
                                            bookingAmount.BikeModel = bikeModel;
                                            bookingAmount.BikeVersion = bikeVersion;
                                            bookingAmount.DealerId = dealerId;
                                            return bookingAmount;
                                        }, splitOn: "MakeId, ModelId, VersionId, LastUpdatedBy", param: param, commandType: CommandType.StoredProcedure
                                    );
                }
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("GetBikeBookingAmount ex : " + ex.Message + ex.Source);
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                
            }

            return objBookingAmt;
        }

        #endregion



        /// <summary>
        /// Written By : Suresh Prajapati on 02nd Jan, 2015
        /// Summary    : To Delete a bike booking amount.
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        public bool DeleteBookingAmount(uint bookingId)
        {

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("BW_DeleteBikeBookingAmount"))
                {

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_BookingId", DbType.Int32, bookingId));

                    return MySqlDatabase.UpdateQuery(cmd, ConnectionType.MasterDatabase);
                }
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("DeleteBookingAmount ex : " + ex.Message + ex.Source);
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                
            }
            return false;
        }

        #region Pivotal Tracker #95410582
        /// <summary>
        /// Author  :   Sumit Kate
        /// Copies the dealer's offers in cities
        /// </summary>
        /// <param name="dealerId">Dealer Id</param>
        /// <param name="lstOfferIds">Offer Ids (Comma Separated Values)</param>
        /// <param name="lstCityId">City Ids (Comma Separated Values)</param>
        /// <returns></returns>
        public bool CopyOffersToCities(uint dealerId, string lstOfferIds, string lstCityId)
        {
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("BW_CopyDealerOffers"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_DealerId", DbType.Int32, dealerId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_CityIds", DbType.String, 250, lstCityId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_OfferIds", DbType.String, 250, lstOfferIds));

                    return (MySqlDatabase.InsertQuery(cmd, ConnectionType.MasterDatabase));
                }
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("CopyOffersToCities ex : " + ex.Message + ex.Source);
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                
            }

            return false;
        }
        #endregion

        #region Pivotal Tracker #115238879
        /// <summary>
        /// Created by  :   Sumit Kate on 10 Mar 2016
        /// Description :   Gets Dealer Benefits
        /// </summary>
        /// <param name="dealerId"></param>
        /// <returns></returns>
        public IEnumerable<DealerBenefitEntity> GetDealerBenefits(uint dealerId)
        {

            IList<DealerBenefitEntity> objOffers = null;
            DealerBenefitEntity objOffer = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("bw_getdealerbenefits"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_dealerid", DbType.Int32, dealerId));
                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            objOffers = new List<DealerBenefitEntity>();
                            while (dr.Read())
                            {
                                objOffer = new DealerBenefitEntity();
                                objOffer.BenefitId = Convert.ToInt32(dr["BenefitId"]);
                                objOffer.CatId = Convert.ToInt32(dr["CatId"]);
                                objOffer.CategoryText = Convert.ToString(dr["CategoryText"]);
                                objOffer.DealerId = Convert.ToInt32(dr["DealerId"]);
                                objOffer.EntryDate = Convert.ToDateTime(dr["EntryDate"]);
                                objOffer.BenefitText = Convert.ToString(dr["BenefitText"]);
                                objOffer.City = Convert.ToString(dr["CityName"]);
                                objOffers.Add(objOffer);
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("GetDealerOffers ex : " + ex.Message + ex.Source);
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                
            }

            return objOffers;
        }
        /// <summary>
        /// Created by  :   Sumit Kate on 10 Mar 2016
        /// Description :   Deletes Dealer Benefits
        /// </summary>
        /// <param name="benefitIds">comma separated benefit ids</param>
        /// <returns></returns>
        public bool DeleteDealerBenefits(string benefitIds)
        {

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("bw_deletedealerbenefit"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_BenefitIds", DbType.String, 255, benefitIds));
                    MySqlDatabase.ExecuteNonQuery(cmd, ConnectionType.MasterDatabase);
                    return true;
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "DeleteDealerBenefits");
                
            }

            return false;
        }
        /// <summary>
        /// Created by  :   Sumit Kate on 10 Mar 2016
        /// Description :   Saves the Dealer Benefits
        /// </summary>
        /// <param name="dealerId"></param>
        /// <param name="cityId"></param>
        /// <param name="catId"></param>
        /// <param name="benefitText"></param>
        /// <param name="userId"></param>
        /// <param name="benefitId"></param>
        /// <returns></returns>
        public bool SaveDealerBenefit(uint dealerId, uint cityId, uint catId, string benefitText, uint userId, uint benefitId)
        {

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("bw_save_dealerbenefit"))
                {

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_DealerId", DbType.Int32, dealerId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_CityId", DbType.Int32, cityId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_UserId", DbType.Int32, userId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_BenefitText", DbType.String, 200, benefitText));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_CatId", DbType.Int16, catId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_BenefitId", DbType.Int32, (benefitId > 0) ? benefitId : Convert.DBNull));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_isactive", DbType.Boolean, Convert.DBNull));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_result", DbType.Byte, ParameterDirection.Output));
                    MySqlDatabase.InsertQuery(cmd, ConnectionType.MasterDatabase);
                    return (Convert.ToBoolean(cmd.Parameters["par_result"].Value));
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "SaveDealerBenefit");
                
            }

            return false;
        }
        #endregion

        #region Pivotal Tracker #115238929
        /// <summary>
        /// Inserts or Updates the Dealer EMI values
        /// If id parameter is passed then it updates the existing record. Else inserts a new row.
        /// Modified by :   Sangram Nandkhile on 11 Mar 2016
        /// Description :   Removed parameters and re-ordered
        /// </summary>
        /// <param name="dealerId">Dealer Id</param>
        /// <param name="tenure"></param>
        /// <param name="rateOfInterest"></param>
        /// <param name="ltv"></param>
        /// <param name="loanProvider"></param>
        /// <param name="MinDownPayment">Optional</param>
        /// <param name="MaxDownPayment">Optional</param>
        /// <param name="MinTenure">Optional</param>
        /// <param name="MaxTenure">Optional</param>
        /// <param name="MinRateOfInterest">Optional</param>
        /// <param name="MaxRateOfInterest">Optional</param>
        /// <param name="ProcessingFee">Optional</param>
        /// <param name="id">This is the ID of the EMI</param>
        /// <param name="UserID">This is the ID of the EMI</param>
        /// <returns></returns>
        public bool SaveDealerEMI(uint dealerId, //ushort tenure, float rateOfInterest, 

            float? MinDownPayment, float? MaxDownPayment,
            ushort? MinTenure, ushort? MaxTenure,
            float? MinRateOfInterest, float? MaxRateOfInterest,
            float? MinLtv, float? MaxLtv,
            string loanProvider,
            float? ProcessingFee,
            uint? id,
            UInt32 UserID)
        {

            try
            {

                using (DbCommand cmd = DbFactory.GetDBCommand("bw_savedealerloanamounts_10032016"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_DealerId", DbType.Int32, dealerId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_LoanProvider", DbType.String, 100, String.IsNullOrEmpty(loanProvider) ? Convert.DBNull : loanProvider));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_UserId", DbType.Int32, UserID));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_minDownPayment", DbType.Double, (MinDownPayment.HasValue) ? MinDownPayment.Value : Convert.DBNull));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_maxDownPayment", DbType.Double, (MaxDownPayment.HasValue) ? MaxDownPayment.Value : Convert.DBNull));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_minTenure", DbType.Int32, (MinTenure.HasValue) ? MinTenure.Value : Convert.DBNull));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_maxTenure", DbType.Int32, (MaxTenure.HasValue) ? MaxTenure.Value : Convert.DBNull));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_minRateOfInterest", DbType.Double, (MinRateOfInterest.HasValue) ? MinRateOfInterest.Value : Convert.DBNull));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_maxRateOfInterest", DbType.Double, (MaxRateOfInterest.HasValue) ? MaxRateOfInterest.Value : Convert.DBNull));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_minLtv", DbType.Double, (MinRateOfInterest.HasValue) ? MinLtv.Value : Convert.DBNull));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_maxLtv", DbType.Double, (MaxLtv.HasValue) ? MaxLtv.Value : Convert.DBNull));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_processingFee", DbType.Double, (ProcessingFee.HasValue) ? ProcessingFee.Value : Convert.DBNull));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_ID", DbType.Double, (id.HasValue && id > 0) ? id.Value : Convert.DBNull));

                    return MySqlDatabase.InsertQuery(cmd, ConnectionType.MasterDatabase);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "SaveDealerEMI");
                
            }
            return false;
        }
        #endregion


        public bool DeleteDealerEMI(uint id)
        {
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("bw_deletedealerloanamounts"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_Id", DbType.Int32, id));
                    cmd.Parameters.Add(DbFactory.GetDbParam("result", DbType.Int32, ParameterDirection.Output));

                    MySqlDatabase.InsertQuery(cmd, ConnectionType.MasterDatabase);
                    return Convert.ToBoolean(cmd.Parameters["result"].Value);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "SaveDealerEMI");
                
            }
            return false;
        }

        /// <summary>
        /// Created by : Aditi Srivastava on 9 Feb 2017
        /// Summary    : To get Makes having dealers in a city
        /// </summary>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public IEnumerable<BikewaleOpr.Entities.BikeData.BikeMakeEntityBase> GetDealerMakesByCity(int cityId)
        {
            ICollection<BikewaleOpr.Entities.BikeData.BikeMakeEntityBase> objMakeList = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getdealermakes"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_cityid", DbType.Int32, cityId));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            objMakeList = new Collection<BikewaleOpr.Entities.BikeData.BikeMakeEntityBase>();

                            BikewaleOpr.Entities.BikeData.BikeMakeEntityBase objMake = null;
                            while (dr.Read())
                            {
                                objMake = new BikewaleOpr.Entities.BikeData.BikeMakeEntityBase();
                                objMake.MakeId = SqlReaderConvertor.ToInt32(dr["MakeId"]);
                                objMake.MakeName = Convert.ToString(dr["MakeName"]);
                                objMakeList.Add(objMake);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("BikewaleOpr.DAL.DealerRepository.GetDealerMakesByCity:CityId :{0}", cityId));
            }
            return objMakeList;
        }
        /// <summary>
        /// Created by : Aditi Srivastava on 9 Feb 2017
        /// Summary    : To get dealers of a make in a city
        /// </summary>
        /// <returns></returns>
        public IEnumerable<DealerEntityBase> GetDealersByMake(uint makeId, uint cityId)
        {
            ICollection<DealerEntityBase> objDealerList = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getbikedealersbymake"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_makeid", DbType.Int32, makeId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_cityid", DbType.Int32, cityId));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            objDealerList = new Collection<DealerEntityBase>();

                            DealerEntityBase objDealer = null;
                            while (dr.Read())
                            {
                                objDealer = new DealerEntityBase();
                                objDealer.Id = SqlReaderConvertor.ToUInt32(dr["DealerId"]);
                                objDealer.Name = Convert.ToString(dr["DealerName"]);
                                objDealerList.Add(objDealer);
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("BikewaleOpr.DAL.DealerRepository.GetDealersByMake: MakeId:{0},CityId:{1}", makeId, cityId));
            }
            return objDealerList;
        }
    }
}

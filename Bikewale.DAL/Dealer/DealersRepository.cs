using Bikewale.DTO.MobileVerification;
using Bikewale.Entities.BikeBooking;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Dealer;
using Bikewale.Entities.DealerLocator;
using Bikewale.Entities.Location;
using Bikewale.Entities.MobileVerification;
using Bikewale.Entities.PriceQuote;
using Bikewale.Interfaces.Dealer;
using Bikewale.Notifications;
using Bikewale.Utility;
using MySql.CoreDAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Web;
namespace Bikewale.DAL.Dealer
{
    /// <summary>
    /// Created By : Ashwini Todkar on4 June 2014
    /// Summary    : Implements logic to access New Bike Dealers data
    /// </summary>
    public class DealersRepository : IDealerRepository
    {
        /// <summary>
        /// Method to get list of make and total dealers for which dealers are available in india
        /// </summary>
        /// <returns></returns>
        public List<NewBikeDealersMakeEntity> GetDealersMakesList()
        {
            List<NewBikeDealersMakeEntity> objMakeList = null;

            try
            {

                using (DbCommand cmd = DbFactory.GetDBCommand("getdealersmakelist"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    objMakeList = new List<NewBikeDealersMakeEntity>();

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            while (dr.Read())
                            {
                                objMakeList.Add(new NewBikeDealersMakeEntity
                                {
                                    MakeId = Convert.ToInt32(dr["MakeId"]),
                                    MakeName = Convert.ToString(dr["BikeMake"]),
                                    MaskingName = Convert.ToString(dr["MaskingName"]),
                                    DealersCount = Convert.ToInt32(dr["TotalCount"])
                                });
                            }
                            dr.Close();
                        }
                    }
                }
            }

            catch (Exception ex)
            {

                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);

            }

            return objMakeList;
        }

        /// <summary>
        /// Method to get list of dealers cities for given make in india 
        /// </summary>
        /// <param name="makeId"></param>
        /// <returns></returns>
        public NewBikeDealersListEntity GetDealersCitiesListByMakeId(uint makeId)
        {
            NewBikeDealersListEntity objDealerList = null;
            try
            {

                using (DbCommand cmd = DbFactory.GetDBCommand("getdealerscitiesbymakeid"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_makeid", DbType.Int32, makeId));

                    objDealerList = new NewBikeDealersListEntity();
                    objDealerList.TotalDealers = 0;

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        objDealerList.CityWiseDealers = new List<CityWiseDealersCountEntity>();
                        objDealerList.StatesList = new List<StateEntityBase>();

                        if (dr != null)
                        {
                            while (dr.Read())
                            {
                                objDealerList.CityWiseDealers.Add(new CityWiseDealersCountEntity
                                {
                                    CityId = Convert.ToUInt32(dr["CityId"]),
                                    CityName = Convert.ToString(dr["City"]),
                                    CityMaskingName = Convert.ToString(dr["CityMaskingName"]),
                                    StateId = Convert.ToInt32(dr["StateId"]),
                                    DealersCount = Convert.ToInt32(dr["TotalDealers"])
                                });

                                //get state list
                                if (Convert.ToUInt32(dr["StateRank"]) == 1)
                                {
                                    objDealerList.StatesList.Add(new StateEntityBase
                                    {
                                        StateId = Convert.ToUInt32(dr["StateId"]),
                                        StateName = Convert.ToString(dr["StateName"])
                                    });
                                }

                                objDealerList.TotalDealers = objDealerList.TotalDealers + Convert.ToInt32(dr["TotalDealers"]);
                            }
                            dr.Close();
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);
            }

            return objDealerList;
        }

        /// <summary>
        /// Method to get list of dealers with its details for given city and make
        /// </summary>
        /// <param name="makeId"></param>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public List<NewBikeDealerEntity> GetDealersList(uint makeId, uint cityId)
        {
            List<NewBikeDealerEntity> objDealerList = null;

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "getdealerslist";
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_makeid", DbType.Int32, makeId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_cityid", DbType.Int32, cityId));

                    objDealerList = new List<NewBikeDealerEntity>();
                    StateEntityBase objState = null;
                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        objState = new StateEntityBase();

                        if (dr != null)
                        {
                            while (dr.Read())
                            {
                                objState.StateName = dr["STATE"].ToString();
                                objDealerList.Add(new NewBikeDealerEntity
                                {
                                    Name = Convert.ToString(dr["DealerName"]),
                                    Address = Convert.ToString(dr["Address"]),
                                    PhoneNo = Convert.ToString(dr["ContactNo"]),
                                    Email = Convert.ToString(dr["EMailId"]),
                                    PinCode = Convert.ToString(dr["PinCode"]),
                                    CityName = Convert.ToString(dr["City"]),
                                    Website = Convert.ToString(dr["WebSite"]),
                                    Fax = Convert.ToString(dr["FaxNo"]),
                                    State = objState
                                });
                            }
                            dr.Close();
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);
            }

            return objDealerList;
        }

        /// <summary>
        /// Method to get list of dealers with its details for given city and make
        /// </summary>
        /// <param name="makeId"></param>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public IEnumerable<NewBikeDealerEntityBase> GetNewBikeDealersList(int makeId, int cityId, EnumNewBikeDealerClient? clientId = null)
        {
            IList<NewBikeDealerEntityBase> objDealerList = null;

            try
            {

                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "getnewbikedealers";
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_makeid", DbType.Int32, makeId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_cityid", DbType.Int32, cityId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_clientid", DbType.Int32, (clientId.HasValue && clientId.Value > 0) ? clientId.Value : Convert.DBNull));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            objDealerList = new List<NewBikeDealerEntityBase>();
                            while (dr.Read())
                            {
                                NewBikeDealerEntityBase objDealer = new NewBikeDealerEntityBase();
                                objDealer.Id = Convert.ToInt32(dr["DealerId"]);
                                objDealer.Name = Convert.ToString(dr["DealerName"]);
                                objDealer.Address = Convert.ToString(dr["Address"]);
                                objDealer.ContactNo = Convert.ToString(dr["ContactNo"]);
                                objDealer.Email = Convert.ToString(dr["EMailId"]);
                                objDealer.PinCode = Convert.ToString(dr["PinCode"]);
                                objDealer.Website = Convert.ToString(dr["WebSite"]);
                                objDealer.Fax = Convert.ToString(dr["FaxNo"]);
                                objDealer.WorkingHours = Convert.ToString(dr["WorkingHours"]);
                                objDealer.BikeMake = Convert.ToString(dr["BikeMake"]);
                                objDealer.MakeMaskingName = Convert.ToString(dr["MakeMaskingName"]);
                                objDealer.City = Convert.ToString(dr["City"]);
                                objDealer.State = Convert.ToString(dr["State"]);
                                objDealerList.Add(objDealer);
                            }
                            dr.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);
            }

            return objDealerList;
        }

        /// <summary>
        /// Method to get list of makes for which dealers are available in given city.
        /// </summary>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public List<BikeMakeEntityBase> GetDealersMakeListByCityId(uint cityId)
        {
            List<BikeMakeEntityBase> objMakeList = null;

            try
            {

                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "getdealermakesbycityid";
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_cityid", DbType.Int32, cityId));

                    objMakeList = new List<BikeMakeEntityBase>();

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            while (dr.Read())
                            {
                                objMakeList.Add(new BikeMakeEntityBase
                                {
                                    MakeId = Convert.ToInt32(dr["ID"]),
                                    MakeName = Convert.ToString(dr["Name"]),
                                    MaskingName = Convert.ToString(dr["MaskingName"])
                                });
                            }
                            dr.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);

            }

            return objMakeList;
        }

        /// <summary>
        /// Created By : Sushil Kumar
        /// Created On : 7th October 2015 
        /// Summary : Method to get list of all cities in which dealers are available
        /// </summary>
        /// <returns></returns>
        public List<CityEntityBase> GetDealersCitiesList()
        {
            List<CityEntityBase> objCityList = null;

            try
            {

                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "getdealercities_new";

                    objCityList = new List<CityEntityBase>();

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            while (dr.Read())
                            {
                                objCityList.Add(new CityEntityBase
                                {
                                    CityId = Convert.ToUInt32(dr["ID"]),
                                    CityName = Convert.ToString(dr["NAME"]),
                                    CityMaskingName = Convert.ToString(dr["CityMaskingName"])
                                });
                            }
                            dr.Close();
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);
            }
            return objCityList;
        }

        /// <summary>
        /// Created By : Sushil Kumar
        /// Created On : 21th October 2015
        /// Summary : To capture maufacturer lead for bikewale pricequotes 
        /// </summary>
        /// <param name="objLead"></param>
        /// <returns>Lead submission status</returns>
        public bool SaveManufacturerLead(ManufacturerLeadEntity objLead)
        {
            bool status = false;
            try
            {
                if (objLead != null && objLead.PQId > 0 && objLead.DealerId > 0)
                {
                    using (DbCommand cmd = DbFactory.GetDBCommand())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "savemanufacturerlead_03032017";

                        cmd.Parameters.Add(DbFactory.GetDbParam("par_name", DbType.String, 50, objLead.Name));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_email", DbType.String, 150, objLead.Email));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_mobile", DbType.String, 10, objLead.Mobile));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_pqid", DbType.Int64, objLead.PQId));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_leadsourceid", DbType.Int16, objLead.LeadSourceId));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_pincode", DbType.String, objLead.PinCode));

                        cmd.Parameters.Add(DbFactory.GetDbParam("par_dealerid", DbType.Int64, objLead.DealerId));

                        status = SqlReaderConvertor.ToBoolean(MySqlDatabase.ExecuteNonQuery(cmd, ConnectionType.MasterDatabase));
                    }
                }
            }

            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "DealersRepository.SaveManufacturerLead");
            }

            return status;

        }

        /// <summary>
        /// Created By : Sushil Kumar
        /// Created On : 7th October 2015 
        /// Summary : Method to get list of all cities in which dealers are available
        /// </summary>
        /// <returns></returns>
        public List<CityEntityBase> GetDealersBookingCitiesList()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Created By : Lucky Rathore on 21 March 2016
        /// Description : Return Dealers deatail list. 
        /// Modified By : Vivek Gupta on 31-05-2016
        /// Desc : MakeName , CityName, CityMaskingName and MakeMaskingName retrieved
        /// Modified by :   Sumit Kate on 19 Jun 2016
        /// Description :   Added Optional parameter(inherited from Interface) and pass model id if value is > 0
        /// Modified By : Sajal Gupta on 29-12-2016
        /// Description : Read CallToActionLongText, CallToActionSmallText parameters.
        /// Modified by :   Sumit Kate on 15 may 2017
        /// Description :   Refer new SP
        /// </summary>
        /// <param name="cityId">e.g. 1</param>
        /// <param name="makeId">e.g. 9</param>
        /// <returns></returns>
        public DealersEntity GetDealerByMakeCity(uint cityId, uint makeId, uint modelid = 0)
        {
            DealersEntity dealers = null;
            IList<DealersList> dealerList = null;

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "getdealerbymakecity_04082017";
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_makeid", DbType.Int32, makeId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_cityid", DbType.Int32, cityId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_modelid", DbType.Int32, modelid > 0 ? modelid : Convert.DBNull));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.MasterDatabase))
                    {
                        if (dr != null)
                        {
                            DealersList dealerdetail = null;
                            dealerList = new List<DealersList>();
                            dealers = new DealersEntity();
                            while (dr.Read())
                            {
                                dealerdetail = new DealersList();
                                dealerdetail.DealerId = SqlReaderConvertor.ToUInt16(dr["DealerId"]);
                                dealerdetail.DealerType = SqlReaderConvertor.ToUInt16(dr["DealerPackage"]);
                                dealerdetail.Name = Convert.ToString(dr["DealerName"]);
                                DealerPackageTypes dpType;
                                Enum.TryParse(dealerdetail.DealerType.ToString(), out dpType);
                                dealerdetail.DealerPackageType = dpType;
                                dealerdetail.City = Convert.ToString(dr["City"]);
                                dealerdetail.MaskingNumber = Convert.ToString(dr["MaskingNumber"]);
                                dealerdetail.EMail = Convert.ToString(dr["EMail"]);
                                dealerdetail.Address = Convert.ToString(dr["Address"]);
                                dealerdetail.CampaignId = SqlReaderConvertor.ParseToUInt32(dr["CampaignId"]);
                                dealerdetail.objArea = new Bikewale.Entities.Location.AreaEntityBase();
                                dealerdetail.objArea.AreaName = Convert.ToString(dr["Area"]);
                                dealerdetail.objArea.Longitude = SqlReaderConvertor.ParseToDouble(dr["Longitude"]);
                                dealerdetail.objArea.Latitude = SqlReaderConvertor.ParseToDouble(dr["Lattitude"]);
                                dealerdetail.objArea.PinCode = Convert.ToString(dr["dealerpincode"]);
                                dealerdetail.DisplayTextLarge = Convert.ToString(dr["CtaLongText"]);
                                dealerdetail.DisplayTextSmall = Convert.ToString(dr["CtaSmallText"]);
                                dealerdetail.IsFeatured = SqlReaderConvertor.ToBoolean(dr["isfeatured"]);
                                // dealerdetail.IsBwDealer = SqlReaderConvertor.ToBoolean(dr["isbwdealer"]);
                                dealerList.Add(dealerdetail);
                            }

                            if (dr.NextResult() && dr.Read())
                            {
                                dealers.TotalCount = SqlReaderConvertor.ToUInt16(dr["TotalCount"]);
                            }

                            if (dr.NextResult() && dr.Read())
                            {
                                dealers.MakeName = Convert.ToString(dr["MakeName"]);
                                dealers.CityName = Convert.ToString(dr["CityName"]);
                                dealers.CityMaskingName = Convert.ToString(dr["CityMaskingName"]);
                                dealers.MakeMaskingName = Convert.ToString(dr["MakeMaskingName"]);
                            }

                            dealers.Dealers = dealerList;
                            dr.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("DealersRepository.GetDealerByMakeCity({0},{1},{2})", cityId, makeId, modelid));
            }

            return dealers;
        }

        /// <summary>
        /// Created By : Lucky Rathore
        /// Created on : 22 march 2016
        /// Description : for getting dealer detail and bike detail w.r.t dealer.
        /// </summary>
        /// <param name="dealerId">e.g. 4</param>
        /// <returns>DealerBikesEntity Entity object.</returns>
        public DealerBikesEntity GetDealerDetailsAndBikes(uint dealerId, uint campaignId)
        {
            DealerBikesEntity dealers = new DealerBikesEntity();

            try
            {

                using (DbCommand cmd = DbFactory.GetDBCommand("getdealerbikedetails"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_dealerid", DbType.Int32, dealerId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_campaignid", DbType.Int32, campaignId));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            if (dr.Read())
                            {
                                dealers.DealerDetails = new DealerDetailEntity();
                                dealers.DealerDetails.Name = Convert.ToString(dr["DealerName"]);
                                dealers.DealerDetails.Address = Convert.ToString(dr["Address"]);
                                dealers.DealerDetails.objArea = new Bikewale.Entities.Location.AreaEntityBase
                                {
                                    AreaName = Convert.ToString(dr["Area"]),
                                    Longitude = SqlReaderConvertor.ParseToDouble(dr["Longitude"]),
                                    Latitude = SqlReaderConvertor.ParseToDouble(dr["Lattitude"])

                                };
                                dealers.DealerDetails.City = Convert.ToString(dr["City"]);
                                dealers.DealerDetails.DealerType = SqlReaderConvertor.ToUInt16(dr["DealerType"]);
                                dealers.DealerDetails.EMail = Convert.ToString(dr["EMail"]);
                                dealers.DealerDetails.MaskingNumber = Convert.ToString(dr["MaskingNumber"]);
                                dealers.DealerDetails.DealerId = dealerId;
                                dealers.DealerDetails.WorkingHours = Convert.ToString(dr["WorkingHours"]);

                            }
                            if (dr.NextResult())
                            {
                                IList<MostPopularBikesBase> models = new List<MostPopularBikesBase>();
                                MostPopularBikesBase bikes = new MostPopularBikesBase();
                                BikeMakeEntityBase objMake;
                                BikeModelEntityBase objModel;
                                BikeVersionsListEntity objVersion;
                                MinSpecsEntity specs;
                                while (dr.Read())
                                {
                                    bikes = new MostPopularBikesBase();
                                    bikes.BikeName = Convert.ToString(dr["Bike"]);
                                    bikes.HostURL = Convert.ToString(dr["HostURL"]);
                                    bikes.OriginalImagePath = Convert.ToString(dr["OriginalImagePath"]);
                                    bikes.VersionPrice = SqlReaderConvertor.ToInt64(dr["OnRoadPrice"]);

                                    objMake = new BikeMakeEntityBase();
                                    objModel = new BikeModelEntityBase();
                                    objVersion = new BikeVersionsListEntity();
                                    specs = new MinSpecsEntity();

                                    objMake.MakeId = !Convert.IsDBNull(dr["MakeId"]) ? Convert.ToUInt16(dr["MakeId"]) : default(int);
                                    objMake.MakeName = Convert.ToString(dr["Make"]);
                                    objMake.MaskingName = Convert.ToString(dr["MakeMaskingName"]);

                                    objModel.ModelId = !Convert.IsDBNull(dr["ModelId"]) ? Convert.ToUInt16(dr["ModelId"]) : default(int);
                                    objModel.ModelName = Convert.ToString(dr["Model"]);
                                    objModel.MaskingName = Convert.ToString(dr["ModelMaskingName"]);

                                    objVersion.VersionId = !Convert.IsDBNull(dr["VersionId"]) ? Convert.ToUInt16(dr["VersionId"]) : default(int);
                                    objVersion.VersionName = Convert.ToString(dr["Version"]);

                                    specs.Displacement = SqlReaderConvertor.ToNullableFloat(dr["Displacement"]);
                                    specs.FuelEfficiencyOverall = SqlReaderConvertor.ToNullableUInt16(dr["FuelEfficiencyOverall"]);
                                    specs.MaxPower = SqlReaderConvertor.ToNullableFloat(dr["MaxPower"]);
                                    specs.MaximumTorque = SqlReaderConvertor.ToNullableFloat(dr["MaxPowerRPM"]);

                                    bikes.objMake = objMake;
                                    bikes.objModel = objModel;
                                    bikes.objVersion = objVersion;
                                    bikes.Specs = specs;

                                    models.Add(bikes);
                                }
                                dealers.Models = models;
                            }
                            dr.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "GetDealerDetailsAndBikes");
            }
            return dealers;
        }

        /// <summary>
        /// Created By : Sajal Gupta on 26/09/2016
        /// Description: DAL method to get dealer's bikes and details on the basis of dealerId and makeId.
        /// Modeified By:- Subodh Jain 15 dec 2016
        /// Summary:- Added pincode data
        /// Modified by : Sajal Gupta on 29-12-2016
        /// Description : Added DisplayTextLarge, DisplayTextSmall
        /// Modified by :   Sumit Kate on 19 Jan 2017
        /// Description :   Populate AreaId
        /// </summary>
        public DealerBikesEntity GetDealerDetailsAndBikesByDealerAndMake(uint dealerId, int makeId)
        {
            DealerBikesEntity dealers = new DealerBikesEntity();

            try
            {

                using (DbCommand cmd = DbFactory.GetDBCommand("getdealerdetails_04082017"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_dealerid", DbType.Int32, dealerId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_makeid", DbType.Int32, makeId));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            if (dr.Read())
                            {
                                dealers.DealerDetails = new DealerDetailEntity();
                                dealers.DealerDetails.Name = Convert.ToString(dr["DealerName"]);
                                dealers.DealerDetails.Address = Convert.ToString(dr["Address"]);
                                dealers.DealerDetails.MakeName = Convert.ToString(dr["makename"]);
                                dealers.DealerDetails.MakeMaskingName = Convert.ToString(dr["makemaskingname"]);
                                dealers.DealerDetails.MakeId = SqlReaderConvertor.ToInt32(dr["makeid"]);
                                dealers.DealerDetails.objArea = new Bikewale.Entities.Location.AreaEntityBase
                                {
                                    AreaId = SqlReaderConvertor.ParseToUInt32(dr["areaid"]),
                                    AreaName = Convert.ToString(dr["Area"]),
                                    Longitude = SqlReaderConvertor.ParseToDouble(dr["Longitude"]),
                                    Latitude = SqlReaderConvertor.ParseToDouble(dr["Lattitude"])

                                };
                                dealers.DealerDetails.CityMaskingName = Convert.ToString(dr["citymaskingname"]);
                                dealers.DealerDetails.City = Convert.ToString(dr["City"]);
                                dealers.DealerDetails.DealerType = SqlReaderConvertor.ToUInt16(dr["DealerType"]);
                                dealers.DealerDetails.IsFeatured = SqlReaderConvertor.ToUInt16(dr["isfeatured"]) != 0;
                                dealers.DealerDetails.EMail = Convert.ToString(dr["EMail"]);
                                dealers.DealerDetails.MaskingNumber = Convert.ToString(dr["MaskingNumber"]);
                                dealers.DealerDetails.DealerId = Convert.ToUInt16(dealerId);
                                dealers.DealerDetails.WorkingHours = Convert.ToString(dr["WorkingHours"]);
                                dealers.DealerDetails.CampaignId = SqlReaderConvertor.ToUInt32(dr["id"]);
                                dealers.DealerDetails.CityId = Convert.ToInt32(dr["cityid"]);
                                dealers.DealerDetails.Pincode = Convert.ToString(dr["Pincode"]);
                                dealers.DealerDetails.DisplayTextLarge = Convert.ToString(dr["DisplayTextLarge"]);
                                dealers.DealerDetails.DisplayTextSmall = Convert.ToString(dr["DisplayTextSmall"]);
                            }
                            if (dr.NextResult())
                            {
                                IList<MostPopularBikesBase> models = new List<MostPopularBikesBase>();
                                MostPopularBikesBase bikes = new MostPopularBikesBase();
                                BikeMakeEntityBase objMake;
                                BikeModelEntityBase objModel;
                                BikeVersionsListEntity objVersion;
                                MinSpecsEntity specs;
                                while (dr.Read())
                                {
                                    bikes = new MostPopularBikesBase();
                                    bikes.BikeName = Convert.ToString(dr["Bike"]);
                                    bikes.HostURL = Convert.ToString(dr["HostURL"]);
                                    bikes.OriginalImagePath = Convert.ToString(dr["OriginalImagePath"]);
                                    bikes.VersionPrice = SqlReaderConvertor.ToInt64(dr["OnRoadPrice"]);

                                    objMake = new BikeMakeEntityBase();
                                    objModel = new BikeModelEntityBase();
                                    objVersion = new BikeVersionsListEntity();
                                    specs = new MinSpecsEntity();

                                    objMake.MakeId = !Convert.IsDBNull(dr["MakeId"]) ? Convert.ToUInt16(dr["MakeId"]) : default(int);
                                    objMake.MakeName = Convert.ToString(dr["Make"]);
                                    objMake.MaskingName = Convert.ToString(dr["MakeMaskingName"]);

                                    objModel.ModelId = !Convert.IsDBNull(dr["ModelId"]) ? Convert.ToUInt16(dr["ModelId"]) : default(int);
                                    objModel.ModelName = Convert.ToString(dr["Model"]);
                                    objModel.MaskingName = Convert.ToString(dr["ModelMaskingName"]);

                                    objVersion.VersionId = !Convert.IsDBNull(dr["VersionId"]) ? Convert.ToUInt16(dr["VersionId"]) : default(int);
                                    objVersion.VersionName = Convert.ToString(dr["Version"]);

                                    specs.Displacement = SqlReaderConvertor.ToNullableFloat(dr["Displacement"]);
                                    specs.FuelEfficiencyOverall = SqlReaderConvertor.ToNullableUInt16(dr["FuelEfficiencyOverall"]);
                                    specs.MaxPower = SqlReaderConvertor.ToNullableFloat(dr["MaxPower"]);
                                    specs.MaximumTorque = SqlReaderConvertor.ToNullableFloat(dr["MaxPowerRPM"]);
                                    specs.KerbWeight = SqlReaderConvertor.ToNullableUInt16(dr["KerbWeight"]);

                                    bikes.objMake = objMake;
                                    bikes.objModel = objModel;
                                    bikes.objVersion = objVersion;
                                    bikes.Specs = specs;
                                    bikes.MakeId = !Convert.IsDBNull(dr["MakeId"]) ? Convert.ToUInt16(dr["MakeId"]) : default(int);
                                    bikes.MakeMaskingName = Convert.ToString(dr["MakeMaskingName"]);
                                    bikes.MakeName = Convert.ToString(dr["make"]);

                                    models.Add(bikes);
                                }
                                dealers.Models = models;
                            }
                            dr.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "DealersRepository.GetDealerDetailsAndBikes");
            }
            return dealers;
        }

        /// <summary>
        /// Created by  : Vivek Singh Tomar on 21st Dec 2017
        /// Description : Get bike models for given dealer and make
        /// </summary>
        /// <param name="dealerId"></param>
        /// <param name="makeId"></param>
        /// <returns></returns>
        public DealerBikeModelsEntity GetBikesByDealerAndMake(uint dealerId, uint makeId)
        {
            DealerBikeModelsEntity dealers = new DealerBikeModelsEntity();

            try
            {

                using (DbCommand cmd = DbFactory.GetDBCommand("modelsavailablefordealer_21122017"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_dealerid", DbType.Int32, dealerId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_makeid", DbType.Int32, makeId));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            IList<MostPopularBikesBase> models = new List<MostPopularBikesBase>();
                            MostPopularBikesBase bikes;
                            BikeMakeEntityBase objMake;
                            BikeModelEntityBase objModel;
                            BikeVersionsListEntity objVersion;
                            MinSpecsEntity specs;

                            while (dr.Read())
                            {

                                bikes = new MostPopularBikesBase();
                                bikes.BikeName = Convert.ToString(dr["Bike"]);
                                bikes.HostURL = Convert.ToString(dr["HostURL"]);
                                bikes.OriginalImagePath = Convert.ToString(dr["OriginalImagePath"]);
                                bikes.VersionPrice = SqlReaderConvertor.ToInt64(dr["OnRoadPrice"]);

                                objMake = new BikeMakeEntityBase();
                                objModel = new BikeModelEntityBase();
                                objVersion = new BikeVersionsListEntity();
                                specs = new MinSpecsEntity();

                                objMake.MakeId = SqlReaderConvertor.ToUInt16(dr["MakeId"]);
                                objMake.MakeName = Convert.ToString(dr["Make"]);
                                objMake.MaskingName = Convert.ToString(dr["MakeMaskingName"]);

                                objModel.ModelId = SqlReaderConvertor.ToUInt16(dr["ModelId"]);
                                objModel.ModelName = Convert.ToString(dr["Model"]);
                                objModel.MaskingName = Convert.ToString(dr["ModelMaskingName"]);

                                objVersion.VersionId = SqlReaderConvertor.ToUInt16(dr["VersionId"]);
                                objVersion.VersionName = Convert.ToString(dr["Version"]);

                                specs.Displacement = SqlReaderConvertor.ToNullableFloat(dr["Displacement"]);
                                specs.FuelEfficiencyOverall = SqlReaderConvertor.ToNullableUInt16(dr["FuelEfficiencyOverall"]);
                                specs.MaxPower = SqlReaderConvertor.ToNullableFloat(dr["MaxPower"]);
                                specs.MaximumTorque = SqlReaderConvertor.ToNullableFloat(dr["MaxPowerRPM"]);
                                specs.KerbWeight = SqlReaderConvertor.ToNullableUInt16(dr["KerbWeight"]);

                                bikes.objMake = objMake;
                                bikes.objModel = objModel;
                                bikes.objVersion = objVersion;
                                bikes.Specs = specs;
                                bikes.MakeId = SqlReaderConvertor.ToUInt16(dr["MakeId"]);
                                bikes.MakeMaskingName = Convert.ToString(dr["MakeMaskingName"]);
                                bikes.MakeName = Convert.ToString(dr["make"]);

                                models.Add(bikes);
                            }

                            dealers.Models = models;

                            if (dr.NextResult() && dr.Read())
                            {
                                dealers.CityName = Convert.ToString(dr["cityname"]);
                            }

                            dr.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("DealersRepository.GetBikesByDealerAndMake. dealerId = {0}, makeId = {1}", dealerId, makeId));
            }
            return dealers;
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 22 Mar 2016
        /// Description :   FetchDealerCitiesByMake. It Includes BW Dealer Cities and AB Dealer Cities
        /// </summary>
        /// <param name="makeId"></param>
        /// <returns></returns>
        public IEnumerable<CityEntityBase> FetchDealerCitiesByMake(uint makeId)
        {
            IList<CityEntityBase> objCityList = null;
            try
            {
                if (makeId > 0)
                {

                    using (DbCommand cmd = DbFactory.GetDBCommand("getdealerscitiesbymakeid_22032016"))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_makeid", DbType.Int32, Convert.ToInt32(makeId)));

                        using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                        {
                            if (dr != null)
                            {
                                objCityList = new List<CityEntityBase>();
                                while (dr.Read())
                                {
                                    objCityList.Add(new CityEntityBase
                                    {
                                        CityId = !Convert.IsDBNull(dr["CityId"]) ? Convert.ToUInt32(dr["CityId"]) : default(UInt32),
                                        CityName = !Convert.IsDBNull(dr["City"]) ? Convert.ToString(dr["City"]) : default(string),
                                        CityMaskingName = !Convert.IsDBNull(dr["CityMaskingName"]) ? Convert.ToString(dr["CityMaskingName"]) : default(String)
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
                ErrorClass.LogError(ex, "FetchDealerCitiesByMake");
            }

            return objCityList;
        }

        /// <summary>
        /// Craeted by  :   Sumit Kate on 21 Jun 2016
        /// Description :   Get Popular City Dealer Count.
        ///                 Calls: GetPopularCityDealer
        /// Modified by :  Subodh Jain on 21 Dec 2016
        /// Description :   Merge Dealer and service center for make and model page
        /// Modified by sajal Gupta on 23-11-2017
        /// Desc : added TotalCitiesCount
        /// <param name="makeId"></param>
        /// <returns></returns>
        public PopularDealerServiceCenter GetPopularCityDealer(uint makeId, uint topCount)
        {

            PopularDealerServiceCenter objDealerServiceDetails = null;
            try
            {
                if (makeId > 0)
                {

                    using (DbCommand cmd = DbFactory.GetDBCommand())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "getpopularcitydealer_23112017";

                        cmd.Parameters.Add(DbFactory.GetDbParam("par_makeid", DbType.Int32, Convert.ToInt32(makeId)));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_topcount", DbType.Int32, Convert.ToInt32(topCount)));


                        using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                        {
                            objDealerServiceDetails = new PopularDealerServiceCenter();
                            if (dr != null)
                            {

                                objDealerServiceDetails.DealerDetails = new List<PopularCityDealerEntity>();
                                while (dr.Read())
                                {
                                    objDealerServiceDetails.DealerDetails.Add(new PopularCityDealerEntity
                                    {
                                        CityId = SqlReaderConvertor.ToUInt32(dr["CityId"]),
                                        CityName = Convert.ToString(dr["Name"]),
                                        CityMaskingName = Convert.ToString(dr["CityMaskingName"]),
                                        DealerCount = SqlReaderConvertor.ToUInt32(dr["dealerscnt"]),
                                        ServiceCenterCount = SqlReaderConvertor.ToUInt32(dr["ServiceCenterCount"])
                                    });

                                }
                            }

                            if (dr.NextResult())
                            {
                                while (dr.Read())
                                {
                                    objDealerServiceDetails.TotalDealerCount = SqlReaderConvertor.ToUInt32(dr["DealerCount"]);
                                }
                            }
                            if (dr.NextResult())
                            {
                                while (dr.Read())
                                {
                                    objDealerServiceDetails.TotalServiceCenterCount = SqlReaderConvertor.ToUInt32(dr["ServiceCenterCount"]);
                                }
                            }
                            if (dr.NextResult())
                            {
                                while (dr.Read())
                                {
                                    objDealerServiceDetails.TotalCitiesCount = SqlReaderConvertor.ToUInt32(dr["cityCount"]);
                                }
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("GetPopularCityDealer(makeId : {0})", makeId));
            }

            return objDealerServiceDetails;
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 18 Aug 2016
        /// Description :   Update Manufacturer Lead with received response from external API
        /// </summary>
        /// <param name="pqId"></param>
        /// <param name="custEmail"></param>
        /// <param name="mobile"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        public bool UpdateManufaturerLead(uint pqId, string custEmail, string mobile, string response)
        {
            bool status = false;
            try
            {
                if (pqId > 0 && !String.IsNullOrEmpty(custEmail) && !String.IsNullOrEmpty(mobile))
                {
                    using (DbCommand cmd = DbFactory.GetDBCommand())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "updatemanufacturerlead";

                        cmd.Parameters.Add(DbFactory.GetDbParam("par_email", DbType.String, 150, custEmail));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_mobile", DbType.String, 10, mobile));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_pqid", DbType.Int64, pqId));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_response", DbType.String, 250, response));
                        if (MySqlDatabase.ExecuteNonQuery(cmd, ConnectionType.MasterDatabase) > 0)
                            status = true;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);
            }

            return status;
        }
        /// <summary>
        /// Created By : Subodh Jain on 20 Dec 2016
        /// Summary    : To bind dealers data by brand
        /// </summary>
        public IEnumerable<DealerBrandEntity> GetDealerByBrandList()
        {
            IList<DealerBrandEntity> objDealerList = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getalldealersbybrand"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            DealerBrandEntity objDealer = null;
                            objDealerList = new List<DealerBrandEntity>();
                            while (dr.Read())
                            {


                                objDealer = new DealerBrandEntity();
                                objDealer.MakeId = SqlReaderConvertor.ToInt32(dr["MakeId"]);
                                objDealer.MakeName = Convert.ToString(dr["MakeName"]);
                                objDealer.MaskingName = Convert.ToString(dr["MakeMaskingName"]);
                                objDealer.DealerCount = SqlReaderConvertor.ToInt32(dr["DealerCount"]);
                                objDealerList.Add(objDealer);
                            }
                            dr.Close();

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "GetDealerByBrandList");
            }
            return objDealerList;

        }

        /// <summary>
        /// Created by  :   Sajal Gupta on 19-12-2016
        /// Description :   Fetch dealers count for nearby city.
        /// </summary>
        /// <param name="makeId"></param>
        /// <returns></returns>
        public IEnumerable<NearByCityDealerCountEntity> FetchNearByCityDealersCount(uint makeId, uint cityId)
        {
            IList<NearByCityDealerCountEntity> objDealerCountList = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getdealerinnearbycity_31032017"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_makeid", DbType.Int32, Convert.ToInt32(makeId)));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_cityid", DbType.Int32, Convert.ToInt32(cityId)));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            objDealerCountList = new List<NearByCityDealerCountEntity>();
                            while (dr.Read())
                            {
                                objDealerCountList.Add(new NearByCityDealerCountEntity
                                {
                                    DealersCount = SqlReaderConvertor.ToUInt32(dr["dealerscnt"]),
                                    CityId = SqlReaderConvertor.ToUInt32(dr["CityId"]),
                                    CityName = Convert.ToString(dr["name"]),
                                    CityMaskingName = Convert.ToString(dr["citymaskingname"]),
                                    Lattitude = SqlReaderConvertor.ParseToDouble(dr["Lattitude"]),
                                    Longitude = SqlReaderConvertor.ParseToDouble(dr["Longitude"]),
                                    GoogleMapImg = Convert.ToString(dr["googlemapimgurl"])
                                });
                            }
                            dr.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("exception in Dal for FetchNearByCityDealersCount {0}, {1}", makeId, cityId));
            }
            return objDealerCountList;
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 27 Dec 2017
        /// Description :   returns dealer versions price breakup
        /// </summary>
        /// <param name="dealerId"></param>
        /// <param name="versionId"></param>
        /// <returns></returns>
        public DealerVersionPrices GetBikeVersionPrice(uint dealerId, uint versionId)
        {
            DealerVersionPrices versionPrice = null;

            try
            {

                using (DbCommand cmd = DbFactory.GetDBCommand("getdealerversionprice"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_dealerid", DbType.Int32, dealerId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_versionid", DbType.Int32, versionId));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            versionPrice = new DealerVersionPrices();
                            versionPrice.PriceList = new List<PQ_Price>();
                            while (dr.Read())
                            {
                                versionPrice.PriceList.Add(new PQ_Price()
                                {
                                    CategoryId = SqlReaderConvertor.ToUInt32(dr["itemid"]),
                                    CategoryName = Convert.ToString(dr["itemname"]),
                                    DealerId = SqlReaderConvertor.ToUInt32(dr["dealerid"]),
                                    Price = SqlReaderConvertor.ToUInt32(dr["price"])
                                });
                            }
                            if (versionPrice.PriceList != null && versionPrice.PriceList.Any())
                            {
                                versionPrice.OnRoadPrice = Convert.ToUInt32(versionPrice.PriceList.Sum(m => m.Price));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("DealersRepository.GetBikeVersionPrice. dealerId = {0}, versionId = {1}", dealerId, versionId));
            }
            return versionPrice;
        }

        /// <summary>
        /// Created by Snehal Dange on 18TH Jan 2018
        /// Descritpion:DAL layer Function for fetching dealer details for sending sms.
        /// </summary>
        /// <param name="dealerId"></param>
        /// <param name="mobileNumber"></param>
        /// <returns></returns>
        public SMSData GetDealerShowroomSMSData(MobileSmsVerification objData)
        {
            SMSData objSMSData = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getdealerdetailsandlogsms"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_dealerid", DbType.Int32, objData.Id));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_mobilenumber", DbType.String, objData.MobileNumber));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_isbwdealer", DbType.Boolean, objData.IsBwDealer));


                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            objSMSData = new SMSData();
                            int status;
                            if (dr.Read())
                            {
                                status = SqlReaderConvertor.ToUInt16(dr["status"]);
                                if (status == 1)
                                {
                                    if (dr.NextResult() && dr.Read())
                                    {
                                        objSMSData.SMSStatus = EnumSMSStatus.Success;
                                        objSMSData.Name = Convert.ToString(dr["name"]);
                                        objSMSData.Address = Convert.ToString(dr["address"]);
                                        objSMSData.Phone = Convert.ToString(dr["phone"]);
                                        objSMSData.CityId = SqlReaderConvertor.ToUInt32(dr["cityId"]);
                                        objSMSData.CityName = Convert.ToString(dr["cityname"]);
                                        objSMSData.Area = Convert.ToString(dr["area"]);
                                        dr.Close();
                                    }
                                }
                                else if (status == 2)
                                {
                                    objSMSData.SMSStatus = EnumSMSStatus.Daily_Limit_Exceeded;
                                }
                                dr.Close();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("DealersRepository.GetDealerShowroomSMSData: DealerId : {0}, MobileNumber : {1}", objData.Id, objData.MobileNumber));

            }
            return objSMSData;
        }
    }//End class
}

using Bikewale.CoreDAL;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Dealer;
using Bikewale.Entities.DealerLocator;
using Bikewale.Entities.Location;
using Bikewale.Entities.PriceQuote;
using Bikewale.Interfaces.Dealer;
using Bikewale.Notifications;
using Bikewale.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web;

namespace Bikewale.DAL.Dealer
{
    /// <summary>
    /// Created By : Ashwini Todkar on4 June 2014
    /// Summary    : Implements logic to access New Bike Dealers data
    /// </summary>
    public class DealersRepository : IDealer
    {
        /// <summary>
        /// Method to get list of make and total dealers for which dealers are available in india
        /// </summary>
        /// <returns></returns>
        public List<NewBikeDealersMakeEntity> GetDealersMakesList()
        {
            List<NewBikeDealersMakeEntity> objMakeList = null;
            Database db = null;

            try
            {
                db = new Database();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "GetDealersMakeList";

                    objMakeList = new List<NewBikeDealersMakeEntity>();

                    using (SqlDataReader dr = db.SelectQry(cmd))
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
                    }
                }
            }
            catch (SqlException ex)
            {
                HttpContext.Current.Trace.Warn("GetDealersMakesList sql ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("GetDealersMakesList ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            finally
            {
                db.CloseConnection();
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
            Database db = null;

            try
            {
                db = new Database();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "GetDealersCitiesByMakeId";
                    cmd.Parameters.Add("@MakeId", SqlDbType.Int).Value = makeId;

                    objDealerList = new NewBikeDealersListEntity();
                    objDealerList.TotalDealers = 0;

                    using (SqlDataReader dr = db.SelectQry(cmd))
                    {
                        objDealerList.CityWiseDealers = new List<CityWiseDealersCountEntity>();
                        objDealerList.StatesList = new List<StateEntityBase>();

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
                    }
                }
            }
            catch (SqlException sqEx)
            {
                HttpContext.Current.Trace.Warn("GetDealersCitiesListByMakeId sqlex : " + sqEx.Message + sqEx.Source);
                ErrorClass objErr = new ErrorClass(sqEx, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("GetDealersCitiesListByMakeId ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            finally
            {
                db.CloseConnection();
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
            Database db = null;

            try
            {
                db = new Database();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "GetDealersList";
                    cmd.Parameters.Add("@MakeId", SqlDbType.Int).Value = makeId;
                    cmd.Parameters.Add("@CityId", SqlDbType.Int).Value = cityId;

                    objDealerList = new List<NewBikeDealerEntity>();
                    StateEntityBase objState = null;
                    using (SqlDataReader dr = db.SelectQry(cmd))
                    {
                        objState = new StateEntityBase();

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
                    }
                }
            }
            catch (SqlException ex)
            {
                HttpContext.Current.Trace.Warn("GetDealersList sql ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("GetDealersList ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            finally
            {
                db.CloseConnection();
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
            Database db = null;

            try
            {
                db = new Database();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "GetNewBikeDealers";
                    cmd.Parameters.Add("@MakeId", SqlDbType.Int).Value = makeId;
                    cmd.Parameters.Add("@CityId", SqlDbType.Int).Value = cityId;

                    if (clientId.HasValue)
                        cmd.Parameters.AddWithValue("@ClientId", clientId.Value);

                    using (SqlDataReader dr = db.SelectQry(cmd))
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
                    }
                }
            }
            catch (SqlException ex)
            {
                HttpContext.Current.Trace.Warn("GetBikeShowrooms sql ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("GetBikeShowrooms ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            finally
            {
                db.CloseConnection();
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
            Database db = null;

            try
            {
                db = new Database();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "GetDealerMakesByCityId";
                    cmd.Parameters.Add("@CityId", SqlDbType.Int).Value = cityId;

                    objMakeList = new List<BikeMakeEntityBase>();

                    using (SqlDataReader dr = db.SelectQry(cmd))
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
                    }
                }
            }
            catch (SqlException ex)
            {
                HttpContext.Current.Trace.Warn("GetDealersMakeListByCityId sql ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("GetDealersMakeListByCityId ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            finally
            {
                db.CloseConnection();
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
            Database db = null;

            try
            {
                db = new Database();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "GetDealerCities_New";

                    objCityList = new List<CityEntityBase>();

                    using (SqlDataReader dr = db.SelectQry(cmd))
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
                    }
                }
            }
            catch (SqlException ex)
            {
                HttpContext.Current.Trace.Warn("GetDealersCitiesList sql ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("GetDealersCitiesList ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            finally
            {
                db.CloseConnection();
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
            Database db = null;
            bool status = false;
            try
            {
                db = new Database();

                if (objLead != null && objLead.PQId > 0 && objLead.DealerId > 0)
                {
                    using (SqlConnection conn = new SqlConnection(db.GetConString()))
                    {
                        using (SqlCommand cmd = new SqlCommand())
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.CommandText = "SaveManufacturerLead";
                            cmd.Connection = conn;

                            cmd.Parameters.Add("@Name", SqlDbType.VarChar, 50).Value = objLead.Name;
                            cmd.Parameters.Add("@Email", SqlDbType.VarChar, 150).Value = objLead.Email;
                            cmd.Parameters.Add("@Mobile", SqlDbType.VarChar, 10).Value = objLead.Mobile;
                            cmd.Parameters.Add("@PQId", SqlDbType.BigInt).Value = objLead.PQId;

                            //TVS Dealer ID to be sent to update pricequote ID
                            cmd.Parameters.Add("@DealerId", SqlDbType.BigInt).Value = objLead.DealerId;

                            conn.Open();
                            if (cmd.ExecuteNonQuery() < 0)
                                status = true;

                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                HttpContext.Current.Trace.Warn("SaveManufacturerLead sql ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("SaveManufacturerLead ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            finally
            {
                db.CloseConnection();
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
        /// </summary>
        /// <param name="cityId">e.g. 1</param>
        /// <param name="makeId">e.g. 9</param>
        /// <returns></returns>
        public DealersEntity GetDealerByMakeCity(uint cityId, uint makeId, uint modelId = 0)
        {
            DealersEntity dealers = null;
            IList<DealersList> dealerList = null;
            Database db = null;

            try
            {
                db = new Database();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "GetDealerByMakeCity_31052016";
                    cmd.Parameters.Add("@CityId", SqlDbType.Int).Value = cityId;
                    cmd.Parameters.Add("@MakeId", SqlDbType.Int).Value = makeId;
                    if (modelId > 0)
                        cmd.Parameters.Add("@ModelId", SqlDbType.Int).Value = modelId;
                    using (SqlDataReader dr = db.SelectQry(cmd))
                    {
                        if (dr != null)
                        {
                            if (dr.HasRows)
                            {
                                DealersList dealerdetail;
                                dealerList = new List<DealersList>();
                                dealers = new DealersEntity();
                                while (dr.Read())
                                {
                                    dealerdetail = new DealersList();
                                    dealerdetail.DealerId = SqlReaderConvertor.ParseToInt16(dr["DealerId"]);
                                    dealerdetail.Name = Convert.ToString(dr["DealerName"]);
                                    dealerdetail.DealerType = SqlReaderConvertor.ParseToInt16(dr["DealerPackage"]);
                                    dealerdetail.City = Convert.ToString(dr["City"]);
                                    dealerdetail.MaskingNumber = Convert.ToString(dr["MaskingNumber"]);
                                    dealerdetail.EMail = Convert.ToString(dr["EMail"]);
                                    dealerdetail.Address = Convert.ToString(dr["Address"]);
                                    dealerdetail.CampaignId = SqlReaderConvertor.ParseToUInt32(dr["CampaignId"]);
                                    dealerdetail.objArea = new AreaEntityBase();
                                    dealerdetail.objArea.AreaName = Convert.ToString(dr["Area"]);
                                    dealerdetail.objArea.Longitude = SqlReaderConvertor.ParseToDouble(dr["Longitude"]);
                                    dealerdetail.objArea.Latitude = SqlReaderConvertor.ParseToDouble(dr["Lattitude"]);

                                    dealerList.Add(dealerdetail);
                                }

                                if (dr.NextResult() && dr.Read())
                                {
                                    dealers.TotalCount = !Convert.IsDBNull(dr["TotalCount"]) ? Convert.ToUInt16(dr["TotalCount"]) : default(UInt16);
                                }

                                if (dr.NextResult() && dr.Read())
                                {
                                    dealers.MakeName = !Convert.IsDBNull(dr["MakeName"]) ? Convert.ToString(dr["MakeName"]) : default(string);
                                    dealers.CityName = !Convert.IsDBNull(dr["CityName"]) ? Convert.ToString(dr["CityName"]) : default(string);
                                    dealers.CityMaskingName = !Convert.IsDBNull(dr["CityMaskingName"]) ? Convert.ToString(dr["CityMaskingName"]) : default(string);
                                    dealers.MakeMaskingName = !Convert.IsDBNull(dr["MakeMaskingName"]) ? Convert.ToString(dr["MakeMaskingName"]) : default(string);
                                }

                                dealers.Dealers = dealerList;

                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("GetDealerByMakeCity ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            finally
            {
                db.CloseConnection();
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
            Database db = null;

            try
            {
                db = new Database();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "GetDealerBikeDetails";
                    cmd.Parameters.Add("@DealerId", SqlDbType.Int).Value = dealerId;
                    cmd.Parameters.Add("@CampaignId", SqlDbType.Int).Value = campaignId;

                    using (SqlDataReader dr = db.SelectQry(cmd))
                    {
                        if (dr != null)
                        {
                            if (dr.Read() && dr.HasRows)
                            {
                                DealerPackageTypes dpType;
                                dealers.DealerDetails = new DealerDetailEntity();
                                dealers.DealerDetails.Name = Convert.ToString(dr["DealerName"]);
                                dealers.DealerDetails.Address = Convert.ToString(dr["Address"]);
                                dealers.DealerDetails.Area = new AreaEntityBase
                                {
                                    AreaName = Convert.ToString(dr["Area"]),
                                    Longitude = SqlReaderConvertor.ParseToDouble(dr["Longitude"]),
                                    Latitude = SqlReaderConvertor.ParseToDouble(dr["Lattitude"])

                                };
                                dealers.DealerDetails.City = Convert.ToString(dr["City"]);
                                dealers.DealerDetails.DealerType = SqlReaderConvertor.ParseToInt16(dr["DealerType"]);
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
                                    bikes.VersionPrice = SqlReaderConvertor.ToNullableInt64(dr["OnRoadPrice"]);

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
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "GetDealerDetailsAndBikes");
                objErr.SendMail();
            }
            finally
            {
                db.CloseConnection();
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
            Database db = null;
            try
            {
                if (makeId > 0)
                {
                    db = new Database();

                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "GetDealersCitiesByMakeId_22032016";
                        cmd.Parameters.AddWithValue("@MakeId", Convert.ToInt32(makeId));

                        using (SqlDataReader dr = db.SelectQry(cmd))
                        {
                            if (dr.HasRows)
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
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "FetchDealerCitiesByMake");
                objErr.SendMail();
            }
            finally
            {
                db.CloseConnection();
            }
            return objCityList;
        }

        /// <summary>
        /// Craeted by  :   Sumit Kate on 21 Jun 2016
        /// Description :   Get Popular City Dealer Count.
        ///                 Calls: GetPopularCityDealer
        /// </summary>
        /// <param name="makeId"></param>
        /// <returns></returns>
        public IEnumerable<PopularCityDealerEntity> GetPopularCityDealer(uint makeId, uint topCount)
        {
            IList<PopularCityDealerEntity> cityDealers = null;
            Database db = null;
            try
            {
                if (makeId > 0)
                {
                    db = new Database();

                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "GetPopularCityDealer";
                        cmd.Parameters.AddWithValue("@MakeId", Convert.ToInt32(makeId));
                        cmd.Parameters.AddWithValue("@TopCount", Convert.ToInt16(topCount));
                        using (SqlDataReader dr = db.SelectQry(cmd))
                        {
                            if (dr != null && dr.HasRows)
                            {
                                cityDealers = new List<PopularCityDealerEntity>();
                                while (dr.Read())
                                {
                                    cityDealers.Add(new PopularCityDealerEntity()
                                    {
                                        CityBase = new CityEntityBase()
                                        {
                                            CityId = !Convert.IsDBNull(dr["CityId"]) ? Convert.ToUInt32(dr["CityId"]) : default(UInt32),
                                            CityName = !Convert.IsDBNull(dr["Name"]) ? Convert.ToString(dr["Name"]) : default(string),
                                            CityMaskingName = !Convert.IsDBNull(dr["CityMaskingName"]) ? Convert.ToString(dr["CityMaskingName"]) : default(String)
                                        },
                                        NumOfDealers = !Convert.IsDBNull(dr["DealersCnt"]) ? Convert.ToUInt32(dr["DealersCnt"]) : default(UInt32)
                                    });
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, string.Format("GetPopularCityDealer(makeId : {0})", makeId));
                objErr.SendMail();
            }
            finally
            {
                db.CloseConnection();
            }
            return cityDealers;
        }
    }//End class
}

using Bikewale.Entities.BikeBooking;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Location;
using Bikewale.Interfaces.AutoBiz;
using Bikewale.Notifications;
using BikeWale.Entities.AutoBiz;
using MySql.CoreDAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Web;

namespace Bikewale.DAL.AutoBiz
{
    /// <summary>
    /// Written By  : Ashwini Todkar on 29 Oct 2014
    /// Modified by : Pratibha Verma on 27 April 2018
    /// Description : changed sp 'bw_getdealerdetails_08012016' to 'bw_getdealerdetails_27042018'
    /// </summary>
    public class DealersRepository : IDealers
    {
        /// <summary>
        /// Written By : Ashwini Todkar on 29 Oct 2014
        /// </summary>
        /// <param name="objParams"></param>
        /// <returns></returns>
        public PQ_DealerDetailEntity GetDealerDetailsPQ(PQParameterEntity objParams)
        {

            PQ_DealerDetailEntity objDetailPQ = null;
            List<PQ_BikeVarient> varients = null;
            IList<PQ_VersionPrice> priceSplits = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("bw_getdealerdetails_27042018"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_dealerId", DbType.Int64, Convert.ToInt64(objParams.DealerId)));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_versionId", DbType.Int64, Convert.ToInt64(objParams.VersionId)));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_cityId", DbType.Int64, Convert.ToInt64(objParams.CityId)));


                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        objDetailPQ = new PQ_DealerDetailEntity();

                        objDetailPQ.objQuotation = null;

                        //Get vehicle details
                        if (dr.Read())
                        {
                            objDetailPQ.objQuotation = new PQ_QuotationEntity()
                            {
                                objMake = new BikeMakeEntityBase() { MakeName = dr["MakeName"].ToString(), MaskingName = dr["MakeMaskingName"].ToString() },
                                objModel = new BikeModelEntityBase() { ModelName = dr["ModelName"].ToString(), MaskingName = dr["ModelMaskingName"].ToString() },
                                objVersion = new BikeVersionEntityBase() { VersionName = dr["VersionName"].ToString() },
                                HostUrl = dr["HostURL"].ToString(),
                                LargePicUrl = dr["largePic"].ToString(),
                                SmallPicUrl = dr["smallPic"].ToString(),
                                OriginalImagePath = dr["OriginalImagePath"].ToString()
                            };
                        }

                        //Get price details
                        dr.NextResult();

                        objDetailPQ.objQuotation.PriceList = new List<PQ_Price>();

                        while (dr.Read())
                        {
                            if (Convert.ToUInt32(dr["Price"]) > 0)
                                objDetailPQ.objQuotation.PriceList.Add(new PQ_Price() { CategoryName = dr["ItemName"].ToString(), Price = Convert.ToUInt32(dr["Price"]), CategoryId = Convert.ToUInt32(dr["ItemId"]) });
                        }

                        dr.NextResult();

                        // Get dealer disclaimer
                        objDetailPQ.objQuotation.Disclaimer = new List<string>();
                        while (dr.Read())
                            objDetailPQ.objQuotation.Disclaimer.Add(dr["Disclaimer"].ToString());

                        //Get Offer details
                        dr.NextResult();
                        objDetailPQ.objOffers = new List<OfferEntity>();

                        while (dr.Read())
                        {
                            objDetailPQ.objOffers.Add(new OfferEntity() { OfferText = dr["OfferText"].ToString(), OfferType = dr["OfferType"].ToString(), OfferValue = Convert.ToUInt32(dr["OfferValue"]), OfferCategoryId = Convert.ToUInt32(dr["OfferCategoryId"]), OfferId = Convert.ToUInt32(dr["OfferId"]), IsOfferTerms = Convert.ToBoolean(dr["IsOfferTerms"]), IsPriceImpact = Convert.ToBoolean(dr["IsPriceImpact"]) });
                        }

                        //On road price for the varients
                        if (dr.NextResult())
                        {
                            varients = new List<PQ_BikeVarient>();
                            while (dr.Read())
                            {
                                varients.Add(new PQ_BikeVarient()
                                {
                                    objMake = new BikeMakeEntityBase()
                                    {
                                        MakeId = Convert.ToInt32(dr["MakeId"]),
                                        MakeName = Convert.ToString(dr["MakeName"]),
                                        MaskingName = Convert.ToString(dr["MakeMaskingName"])
                                    },
                                    objModel = new BikeModelEntityBase()
                                    {
                                        ModelId = Convert.ToInt32(dr["ModelId"]),
                                        ModelName = Convert.ToString(dr["ModelName"]),
                                        MaskingName = Convert.ToString(dr["ModelMaskingName"])
                                    },
                                    objVersion = new BikeVersionEntityBase()
                                    {
                                        VersionId = Convert.ToInt32(dr["VersionId"]),
                                        VersionName = Convert.ToString(dr["VersionName"])
                                    },
                                    HostUrl = Convert.ToString(dr["HostURL"]),
                                    OriginalImagePath = Convert.ToString(dr["OriginalImagePath"]),
                                    OnRoadPrice = Convert.ToUInt32(dr["OnRoadPrice"]),
                                    BookingAmount = Convert.ToUInt32(dr["BookingAmount"]),
                                    PriceList = new List<PQ_Price>()
                                });
                            }
                            if (dr.NextResult())
                            {
                                priceSplits = new List<PQ_VersionPrice>();
                                while (dr.Read())
                                {
                                    priceSplits.Add(
                                        new PQ_VersionPrice()
                                        {
                                            CategoryId = Convert.ToUInt32(dr["ItemId"]),
                                            CategoryName = Convert.ToString(dr["ItemName"]),
                                            DealerId = Convert.ToUInt32(dr["DealerId"]),
                                            Price = Convert.ToUInt32(dr["Price"]),
                                            VersionId = Convert.ToUInt32(dr["VersionId"])
                                        }
                                    );
                                }
                            }

                            if ((varients != null && varients.Count > 0) && (priceSplits != null && priceSplits.Count > 0))
                            {
                                varients.ForEach(
                                    varient => varient.PriceList =
                                        (
                                            from price in priceSplits
                                            where price.VersionId == varient.objVersion.VersionId
                                            select new PQ_Price()
                                            {
                                                CategoryId = price.CategoryId,
                                                CategoryName = price.CategoryName,
                                                DealerId = price.DealerId,
                                                Price = price.Price
                                            }
                                        ).ToList()
                                    );
                            }
                            objDetailPQ.objQuotation.Varients = varients;
                        }

                        //Get Dealer details
                        dr.NextResult();

                        if (dr.Read())
                        {
                            objDetailPQ.objDealer = new NewBikeDealers();

                            objDetailPQ.objDealer.DealerId = objParams.DealerId;
                            objDetailPQ.objDealer.Name = (dr["FirstName"] != null) ? dr["FirstName"].ToString() : "";
                            objDetailPQ.objDealer.Address = dr["Address"].ToString();
                            objDetailPQ.objDealer.EmailId = dr["EmailId"].ToString();

                            objDetailPQ.objDealer.MobileNo = (dr["MobileNo"] != null) ? dr["MobileNo"].ToString() : "";
                            objDetailPQ.objDealer.PhoneNo = dr["PhoneNo"].ToString();

                            objDetailPQ.objDealer.objArea = new Bikewale.Entities.BikeBooking.AreaEntityBase() { Latitude = Convert.ToDouble(dr["Lattitude"]), Longitude = Convert.ToDouble(dr["Longitude"]), AreaName = dr["AreaName"].ToString(), PinCode = dr["Pincode"].ToString() };

                            objDetailPQ.objDealer.objCity = new CityEntityBase() { CityName = dr["CityName"].ToString() };

                            objDetailPQ.objDealer.objState = new StateEntityBase() { StateName = dr["StateName"].ToString() };
                            objDetailPQ.objDealer.Website = (dr["WebsiteUrl"] != null) ? dr["WebsiteUrl"].ToString() : "";
                            objDetailPQ.objDealer.Organization = (dr["Organization"] != null) ? dr["Organization"].ToString() : "";
                            objDetailPQ.objDealer.WorkingTime = (dr["ContactHours"] != null) ? dr["ContactHours"].ToString() : "";
                            objDetailPQ.objDealer.AdditionalNumbers = dr["AdditionalNumbers"].ToString();
                            objDetailPQ.objDealer.AdditionalEmails = dr["AdditionalEmails"].ToString();
                        }

                        //Get facilities list provided by dealer

                        dr.NextResult();

                        objDetailPQ.objFacilities = new List<FacilityEntity>();

                        while (dr.Read())
                        {
                            objDetailPQ.objFacilities.Add(new FacilityEntity() { Facility = dr["Facility"].ToString() });
                        }

                        //Get EMI options provided by dealers

                        dr.NextResult();

                        if (dr.Read())
                        {
                            objDetailPQ.objEmi = new EMI() { RateOfInterest = Convert.ToSingle(dr["RateOfInterest"]), LoanToValue = Convert.ToUInt16(dr["LTV"]), Tenure = Convert.ToUInt16(dr["Tenure"]), LoanProvider = dr["LoanProvider"].ToString() };
                        }

                        //Get Bike Booking amount
                        dr.NextResult();

                        if (dr.Read())
                        {
                            objDetailPQ.objBookingAmt = new BookingAmountEntityBase() { Amount = Convert.ToUInt32(dr["Amount"]) };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("Exception at GetDealerDetails : " + ex.Message + ex.Source);
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);
               
            }

            return objDetailPQ;
        }

        /// <summary>
        /// Created By : Suresh Prajapati on 29th Oct 2014
        /// Description : To Get Dealer Cities for which Bike Dealer exists.
        /// </summary>
        /// <returns>City Name</returns>
        public DataTable GetDealerCities()
        {
            DataTable dt = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("bw_getbikedealercities"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    dt = MySqlDatabase.SelectAdapterQuery(cmd, ConnectionType.ReadOnly).Tables[0];
                }
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("GetDealerCities ex : " + ex.Message + ex.Source);
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);
               
            }
            return dt;
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
        /// Created By : Sadhana Upadhyay on 18 Dec 2014
        /// Summary : To get dealer booking amount by version id
        /// Modified By : Ashwini Todkar on 23 Dec 2014
        /// </summary>
        /// <param name="versionId"></param>
        /// <param name="dealerId"></param>
        /// <returns></returns>
        public BookingAmountEntity GetDealerBookingAmount(uint versionId, uint dealerId)
        {
            BookingAmountEntity objBookingDetails = null;

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("BW_GetDealerBookingAmount"))
                {

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_VersionId", DbType.Int32, versionId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_DealerId", DbType.Int32, dealerId));


                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr.Read())
                        {
                            objBookingDetails = new BookingAmountEntity()
                            {
                                objBookingAmountEntityBase = new BookingAmountEntityBase()
                                {
                                    Amount = Convert.ToUInt32(dr["Amount"]),
                                    IsActive = Convert.ToBoolean(dr["IsActive"]),
                                    Id = Convert.ToUInt32(dr["Id"])
                                },
                                objMake = new BikeMakeEntityBase()
                                {
                                    MakeName = dr["MakeName"].ToString()
                                },
                                objModel = new BikeModelEntityBase()
                                {
                                    ModelName = dr["ModelName"].ToString()
                                }

                            };

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("GetBikeBookingAmount ex : " + ex.Message + ex.Source);
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);
               
            }
            return objBookingDetails;
        }
    }
}

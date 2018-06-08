using Bikewale.Entities;
using Bikewale.Entities.BikeBooking;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Location;
using Bikewale.Entities.PriceQuote;
using Bikewale.Interfaces.AutoBiz;
using Bikewale.Notifications;
using Bikewale.Utility;
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
    public class DealerPriceQuoteRepository : IDealerPriceQuote
    {
        /// <summary>
        /// Created By : Sadhana Upadhyay on 27th Oct 2014
        /// Summary : function to get dealer price quote
        /// Modified by : Ashutosh Sharma on 30 Aug 2017 
        /// Description : Changed SP from 'bw_getdealerpricequote_28062016' to 'bw_getdealerpricequote_30082017', removed IsGstPrice flag
        /// </summary>
        /// <param name="objParams"></param>
        /// <returns></returns>
        public PQ_QuotationEntity GetDealerPriceQuote(PQParameterEntity objParams)
        {
            PQ_QuotationEntity objPriceQuote = null;
            List<PQ_BikeVarient> varients = null;
            IList<PQ_VersionPrice> priceSplits = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("bw_getdealerpricequote_30082017"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_cityid", DbType.Int32, objParams.CityId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_versionid", DbType.Int64, objParams.VersionId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_dealerid", DbType.Int32, objParams.DealerId > 0 ? objParams.DealerId : Convert.DBNull));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        objPriceQuote = new PQ_QuotationEntity();
                        // Get version details
                        if (dr.Read())
                        {
                            objPriceQuote.objMake = new BikeMakeEntityBase() { MakeName = dr["MakeName"].ToString(), MaskingName = dr["MakeMaskingName"].ToString() };
                            objPriceQuote.objModel = new BikeModelEntityBase() { ModelName = dr["ModelName"].ToString(), MaskingName = dr["ModelMaskingName"].ToString() };
                            objPriceQuote.objVersion = new BikeVersionEntityBase() { VersionName = dr["VersionName"].ToString() };
                            objPriceQuote.HostUrl = dr["HostURL"].ToString();
                            objPriceQuote.LargePicUrl = dr["largePic"].ToString();
                            objPriceQuote.SmallPicUrl = dr["smallPic"].ToString();
                            objPriceQuote.OriginalImagePath = Convert.ToString(dr["OriginalImagePath"]);
                        }
                        dr.NextResult();
                        // Get pricing
                        objPriceQuote.PriceList = new List<PQ_Price>();
                        while (dr.Read())
                        {
                            objPriceQuote.PriceList.Add(new PQ_Price() { CategoryName = dr["ItemName"].ToString(), Price = Convert.ToUInt32(dr["Price"]), DealerId = Convert.ToUInt32(dr["DealerId"]) });
                        }

                        dr.NextResult();
                        // Get dealer disclaimer
                        objPriceQuote.Disclaimer = new List<string>();
                        while (dr.Read())
                            objPriceQuote.Disclaimer.Add(dr["Disclaimer"].ToString());

                        //Get Offer details
                        dr.NextResult();
                        objPriceQuote.objOffers = new List<OfferEntity>();

                        while (dr.Read())
                        {
                            objPriceQuote.objOffers.Add(new OfferEntity() { OfferText = dr["OfferText"].ToString(), OfferType = dr["OfferType"].ToString(), OfferValue = Convert.ToUInt32(dr["OfferValue"]), OfferCategoryId = Convert.ToUInt32(dr["OfferCategoryId"]), OfferId = Convert.ToUInt32(dr["OfferId"]), IsOfferTerms = Convert.ToBoolean(dr["IsOfferTerms"]), IsPriceImpact = Convert.ToBoolean(dr["IsPriceImpact"]) });
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

                            if (varients.Count > 0 && (priceSplits != null && priceSplits.Count > 0))
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
                            objPriceQuote.Varients = varients;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("GetDealerPriceQuote ex : " + ex.Message + ex.Source);
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);

            }

            return objPriceQuote;
        }

        /// <summary>
        /// Created By  : Pratibha Verma on 8 June 2018
        /// Description : returns dealer price for all versions
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="modelId"></param>
        /// <param name="dealerId"></param>
        /// <returns></returns>
        public IEnumerable<PQ_VersionPrice> GetDealerPriceQuotesByModelCity(uint cityId, uint modelId, uint dealerId)
        {
            IList<PQ_VersionPrice> dealerPriceObj = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("bw_getdealerpricebymodelcity"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_cityid", DbType.Int32, cityId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_modelId", DbType.Int64, modelId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_dealerid", DbType.Int32, dealerId));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            dealerPriceObj = new List<PQ_VersionPrice>();
                            while (dr.Read())
                            {
                                dealerPriceObj.Add(new PQ_VersionPrice
                                {
                                    VersionId = SqlReaderConvertor.ToUInt32(dr["versionid"]),
                                    Price = SqlReaderConvertor.ToUInt32(dr["onroadprice"])
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.DAL.AutoBiz.DealerPriceQuoteRepository.GetDealerPriceQuotesByModelCity");
            }
            return dealerPriceObj;
        }
        /// <summary>
        /// Written By : Ashish G. Kamble on 10 May 2015
        /// Summary : Function to get the list of cities where bike booking is available.
        /// </summary>
        /// <returns>Returns list of the cities.</returns>
        public List<CityEntityBase> GetBikeBookingCities(uint? modelId)
        {

            List<CityEntityBase> cities = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("bw_getbikebookingcities"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_BikeModelId", DbType.Int32, (modelId.HasValue) ? modelId : Convert.DBNull));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            cities = new List<CityEntityBase>();

                            while (dr.Read())
                            {
                                cities.Add(new CityEntityBase() { CityId = Convert.ToUInt32(dr["CityId"]), CityName = Convert.ToString(dr["City"]) });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("GetBikeBookingCities ex : " + ex.Message + ex.Source);
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);

            }
            return cities;
        }   // End of GetBikeBookingCities

        /// <summary>
        /// Written By : Ashish G. Kamble on 10 May 2015
        /// Summary : Function to get the list of bike makes in the particular city where booking option is available.
        /// </summary>
        /// <param name="cityId">Should be greater than 0.</param>
        /// <returns>Returns list of bike makes</returns>
        public List<BikeMakeEntityBase> GetBikeMakesInCity(uint cityId)
        {

            List<BikeMakeEntityBase> makes = null;

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("bw_getbikemakesincity"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_CityId", DbType.Int32, cityId));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            makes = new List<BikeMakeEntityBase>();

                            while (dr.Read())
                            {
                                makes.Add(new BikeMakeEntityBase() { MakeId = Convert.ToInt32(dr["MakeId"]), MakeName = Convert.ToString(dr["Make"]) });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("GetBikeMakesInCity ex : " + ex.Message + ex.Source);
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);

            }
            return makes;
        }   // end of GetBikeMakesInCity
        /// <summary>
        /// Written By : Sangram Nandkhile on 05 October 2015
        /// Summary : Function to Retrun HTML for the masking name provided in url
        /// </summary>
        /// <param name="offerMaskingName"></param>
        /// <param name="isExpired">To check if offer is expired or not</param>
        /// <returns></returns>
        public OfferHtmlEntity GetOfferTerms(string offerMaskingName, int? offerId)
        {
            string termsHtml = string.Empty;
            OfferHtmlEntity offerTerms = null;
            DbCommand cmd = null;
            try
            {
                using (cmd = DbFactory.GetDBCommand("bw_getofferterms_25082016"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    #region params
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_offerId", DbType.Int32, offerId));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            offerTerms = new OfferHtmlEntity();
                            while (dr.Read())
                            {
                                offerTerms.Html = Convert.ToString(dr["terms"]); ;
                                offerTerms.IsExpired = Convert.ToBoolean(dr["IsExpired"]);
                            }
                        }
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);

            }
            return offerTerms;
        }


        /// <summary>
        /// Created By : Sadhana Upadhyay on 23 Oct 2015
        /// Summary : To get priceQuote for all dealer who can serve for perticular area
        /// Modified By : Sushil Kumar on 10th January 2016
        /// Description : Added provision to retrieve bike availability by color
        /// </summary>
        /// <param name="versionId"></param>
        /// <param name="cityId"></param>
        /// <param name="dealerIds"></param>
        /// <returns></returns>
        public DealerPriceQuoteEntity GetPriceQuoteForAllDealer(uint versionId, uint cityId, string dealerIds)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 14 Mar 2016
        /// Description :   Get Dealer's Price Quotes
        /// Modified by :   Sumit Kate on 22 Mar 2016
        /// Description :   Removed redundant DataReader checks and added DbNull checks
        /// Modified by :   Sumit Kate on 30 Jan 2017
        /// Description :   Replaced Convert methods with SqlReaderConvertor and Set IsDSA flag
        /// </summary>
        /// <param name="objParams"></param>
        /// <returns></returns>
        public DetailedDealerQuotationEntity GetDealerPriceQuoteByPackage(PQParameterEntity objParams)
        {
            DetailedDealerQuotationEntity objDetailedDealerQuotationEntity = null;
            DealerQuotationEntity dealerQuotation = null;
            NewBikeDealers primaryDealer = null;
            IList<PQ_Price> PriceList = null;
            IList<BikeColorAvailability> ColorwiseAvailabilty = null;
            EMI objEMI = null;
            IList<OfferEntityBase> OfferList = null;
            IList<DealerBenefitEntity> benefits = null;
            IList<NewBikeDealerBase> secondaryDealers = null;
            IList<string> disclaimer = null;

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("bw_getdealerdetails_14032016"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_dealerId", DbType.Int32, objParams.DealerId > 0 ? Convert.ToInt64(objParams.DealerId) : Convert.DBNull));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_versionId", DbType.Int32, Convert.ToInt64(objParams.VersionId)));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_cityId", DbType.Int32, Convert.ToInt64(objParams.CityId)));


                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            objDetailedDealerQuotationEntity = new DetailedDealerQuotationEntity();
                            dealerQuotation = new DealerQuotationEntity();
                            #region Bike Details
                            if (dr.Read())
                            {
                                BikeMakeEntityBase objMake = new BikeMakeEntityBase();
                                objMake.MakeId = SqlReaderConvertor.ToInt32(dr["MakeId"]);
                                objMake.MakeName = Convert.ToString(dr["MakeName"]);
                                objMake.MaskingName = Convert.ToString(dr["MakeMaskingName"]);
                                objDetailedDealerQuotationEntity.objMake = objMake;
                                BikeModelEntityBase objModel = new BikeModelEntityBase();
                                objModel.ModelId = SqlReaderConvertor.ToInt32(dr["ModelId"]);
                                objModel.ModelName = Convert.ToString(dr["ModelName"]);
                                objModel.MaskingName = Convert.ToString(dr["ModelMaskingName"]);
                                objDetailedDealerQuotationEntity.objModel = objModel;
                                BikeVersionEntityBase objVersion = new BikeVersionEntityBase();
                                objVersion.VersionId = SqlReaderConvertor.ToInt32(dr["VersionId"]);
                                objVersion.VersionName = Convert.ToString(dr["VersionName"]);
                                objDetailedDealerQuotationEntity.objVersion = objVersion;

                                objDetailedDealerQuotationEntity.HostUrl = Convert.ToString(dr["HostURL"]);
                                objDetailedDealerQuotationEntity.OriginalImagePath = Convert.ToString(dr["OriginalImagePath"]);
                            }
                            #endregion

                            #region Price Break-Up
                            if (dr.NextResult())
                            {
                                PriceList = new List<PQ_Price>();
                                while (dr.Read())
                                {
                                    PriceList.Add(new PQ_Price()
                                    {
                                        CategoryId = SqlReaderConvertor.ToUInt32(dr["ItemId"]),
                                        CategoryName = Convert.ToString(dr["ItemName"]),
                                        Price = SqlReaderConvertor.ToUInt32(dr["Price"]),
                                        DealerId = SqlReaderConvertor.ToUInt32(dr["DealerId"])
                                    });
                                }
                                dealerQuotation.PriceList = PriceList;
                            }
                            #endregion
                            //Read Dealer Details
                            #region Dealer Details
                            if (dr.NextResult())
                            {
                                if (dr.Read())
                                {
                                    primaryDealer = new NewBikeDealers();
                                    primaryDealer.DealerId = SqlReaderConvertor.ToUInt32(dr["ID"]);
                                    primaryDealer.Organization = Convert.ToString(dr["Organization"]);
                                    primaryDealer.Name = String.Format("{0} {1}", Convert.ToString(dr["FirstName"]), Convert.ToString(dr["LastName"]));
                                    primaryDealer.Address = Convert.ToString(dr["Address"]);
                                    primaryDealer.PhoneNo = Convert.ToString(dr["PhoneNo"]);
                                    primaryDealer.MobileNo = Convert.ToString(dr["MobileNo"]);
                                    primaryDealer.MaskingNumber = Convert.ToString(dr["PhoneNo"]);
                                    primaryDealer.WorkingTime = Convert.ToString(dr["ContactHours"]);
                                    primaryDealer.objArea = new Bikewale.Entities.BikeBooking.AreaEntityBase()
                                    {
                                        AreaId = SqlReaderConvertor.ToUInt32(dr["AreaId"]),
                                        AreaName = Convert.ToString(dr["AreaName"]),
                                        Latitude = SqlReaderConvertor.ParseToDouble(dr["Lattitude"]),
                                        Longitude = SqlReaderConvertor.ParseToDouble(dr["Longitude"]),
                                        PinCode = Convert.ToString(dr["Pincode"])
                                    };
                                    primaryDealer.objCity = new CityEntityBase() { CityName = Convert.ToString(dr["CityName"]) };
                                    primaryDealer.objState = new StateEntityBase() { StateName = Convert.ToString(dr["StateName"]) };
                                    primaryDealer.Website = Convert.ToString(dr["WebsiteUrl"]);
                                    primaryDealer.EmailId = Convert.ToString(dr["EmailId"]);
                                    DealerPackageTypes s;
                                    if (Enum.TryParse(Convert.ToString(dr["DealerPackageType"]), out s))
                                        primaryDealer.DealerPackageType = s;

                                    primaryDealer.IsDSA = SqlReaderConvertor.ToBoolean(dr["isDSA"]);
                                }
                                dealerQuotation.DealerDetails = primaryDealer;
                            }
                            #endregion
                            //Booking Amount
                            if (dr.NextResult())
                            {
                                if (dr.Read())
                                {
                                    dealerQuotation.BookingAmount = Convert.ToUInt32(dr["Amount"]);
                                }
                            }

                            //Booking Num Of Days
                            if (dr.NextResult())
                            {
                                if (dr.Read())
                                {
                                    dealerQuotation.BikeAvailability = Convert.ToUInt32(dr["NumOfDays"]);
                                }
                            }

                            #region Color and Color level availability
                            if (dr.NextResult())
                            {
                                ColorwiseAvailabilty = new List<BikeColorAvailability>();
                                while (dr.Read())
                                {
                                    ColorwiseAvailabilty.Add(new BikeColorAvailability()
                                    {
                                        ColorId = SqlReaderConvertor.ToUInt32(dr["ColorId"]),
                                        NoOfDays = SqlReaderConvertor.ToInt16(dr["NumOfDays"]),
                                        DealerId = SqlReaderConvertor.ToUInt32(dr["DealerId"]),
                                        ColorName = Convert.ToString(dr["ColorName"]),
                                        HexCode = Convert.ToString(dr["HexCode"]),
                                        VersionId = SqlReaderConvertor.ToUInt32(dr["BikeVersionId"])
                                    });
                                }
                                dealerQuotation.AvailabilityByColor = ColorwiseAvailabilty;
                            }
                            #endregion
                            #region EMI Values
                            if (dr.NextResult())
                            {
                                if (dr.Read())
                                {
                                    objEMI = new EMI();
                                    objEMI.Id = SqlReaderConvertor.ToUInt32(dr["Id"]);
                                    objEMI.LoanProvider = Convert.ToString(dr["LoanProvider"]);
                                    objEMI.LoanToValue = SqlReaderConvertor.ToUInt16(dr["LTV"]);
                                    objEMI.MaxDownPayment = SqlReaderConvertor.ToUInt32(dr["MaxDownPayment"]);
                                    objEMI.MaxRateOfInterest = SqlReaderConvertor.ToUInt32(dr["MaxRateOfInterest"]);
                                    objEMI.MaxTenure = SqlReaderConvertor.ToUInt16(dr["MaxTenure"]);
                                    objEMI.MinDownPayment = SqlReaderConvertor.ToUInt32(dr["MinDownPayment"]);
                                    objEMI.MinRateOfInterest = SqlReaderConvertor.ToUInt32(dr["MinRateOfInterest"]);
                                    objEMI.MinTenure = SqlReaderConvertor.ToUInt16(dr["MinTenure"]);
                                    objEMI.ProcessingFee = SqlReaderConvertor.ToUInt32(dr["ProcessingFee"]);
                                    objEMI.RateOfInterest = SqlReaderConvertor.ToFloat(dr["RateOfInterest"]);
                                    objEMI.Tenure = SqlReaderConvertor.ToUInt16(dr["Tenure"]);
                                }
                                dealerQuotation.EMIDetails = objEMI;
                            }
                            #endregion
                            #region Dealer Offers
                            if (dr.NextResult())
                            {
                                OfferList = new List<OfferEntityBase>();
                                while (dr.Read())
                                {
                                    OfferList.Add(new OfferEntityBase()
                                    {
                                        OfferId = SqlReaderConvertor.ToUInt32(dr["OfferId"]),
                                        OfferCategoryId = SqlReaderConvertor.ToUInt32(dr["OfferCategoryId"]),
                                        OfferType = Convert.ToString(dr["OfferType"]),
                                        OfferText = Convert.ToString(dr["OfferText"]),
                                        OfferValue = SqlReaderConvertor.ToUInt32(dr["OfferValue"]),
                                        OffervalidTill = SqlReaderConvertor.ToDateTime(dr["OfferValidTill"]),
                                        DealerId = SqlReaderConvertor.ToUInt32(dr["DealerId"]),
                                        IsOfferTerms = SqlReaderConvertor.ToBoolean(dr["IsOfferTerms"]),
                                    });
                                }
                                dealerQuotation.OfferList = OfferList;
                            }
                            #endregion
                            #region Dealer Benefits
                            if (dr.NextResult())
                            {
                                benefits = new List<DealerBenefitEntity>();
                                while (dr.Read())
                                {
                                    DealerBenefitEntity objDealerBenefitEntity = new DealerBenefitEntity();
                                    objDealerBenefitEntity.BenefitId = SqlReaderConvertor.ToInt32(dr["BenefitId"]);
                                    objDealerBenefitEntity.DealerId = SqlReaderConvertor.ToInt32(dr["DealerId"]);
                                    objDealerBenefitEntity.CatId = SqlReaderConvertor.ToInt32(dr["CatId"]);
                                    objDealerBenefitEntity.CategoryText = Convert.ToString(dr["CategoryText"]);
                                    objDealerBenefitEntity.BenefitText = Convert.ToString(dr["BenefitText"]);
                                    objDealerBenefitEntity.City = Convert.ToString(dr["City"]);
                                    benefits.Add(objDealerBenefitEntity);
                                }
                                dealerQuotation.Benefits = benefits;
                            }
                            #endregion
                            #region Disclaimer
                            if (dr.NextResult())
                            {
                                disclaimer = new List<string>();
                                while (dr.Read())
                                {
                                    disclaimer.Add(Convert.ToString(dr["Disclaimer"]));
                                }
                                dealerQuotation.Disclaimer = disclaimer;
                            }
                            #endregion

                            if (dr.NextResult())
                            {
                                secondaryDealers = new List<NewBikeDealerBase>();
                                DealerPackageTypes s;
                                while (dr.Read())
                                {
                                    secondaryDealers.Add(
                                        new NewBikeDealerBase()
                                        {
                                            Area = Convert.ToString(dr["AreaName"]),
                                            DealerId = SqlReaderConvertor.ToUInt32(dr["ID"]),
                                            Name = Convert.ToString(dr["Organization"]),
                                            MaskingNumber = Convert.ToString(dr["MaskingNumber"]),
                                            DealerPackageType = (Enum.TryParse(Convert.ToString(dr["DealerPackageType"]), out s)) ? s : DealerPackageTypes.Invalid
                                        }
                                        );
                                }
                            }
                        }
                        objDetailedDealerQuotationEntity.PrimaryDealer = dealerQuotation;
                        objDetailedDealerQuotationEntity.SecondaryDealers = secondaryDealers;
                        objDetailedDealerQuotationEntity.SecondaryDealerCount = secondaryDealers != null ? secondaryDealers.Count() : 0;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.DealerPriceQuoteRepository.GetDealerPriceQuoteByPackage");

            }
            return objDetailedDealerQuotationEntity;
        }

        /// <summary>
        /// Created by  :   Sushil Kumar on 16th June 2016
        /// Description :   Get Dealer's Price Quotes with versionprices
        /// Modified by :   Sumit Kate on 01 Aug 2016
        /// Description :   Secondary Dealer Offer count and secondary dealer distance from given area
        /// Modified by :   Sumit Kate on 03 Aug 2016
        /// Description :   Set the Selected version price for secondary dealers for easy binding
        /// Modified By : Sangram Nandkhile on 29 Dec 2016
        /// Description : Added DisplayTextLarge, DisplayTextSmall
        /// Modified by :   Sumit Kate on 30 Jan 2017
        /// Description :   Replaced Convert methods with SqlReaderConvertor and Set IsDSA flag
        /// Modified by : Ashutosh Sharma on 30 Aug 2017 
        /// Description : Changed SP from 'bw_getdealerdetails_10082017' to 'bw_getdealerdetails_30082017', removed IsGstPrice flag
        /// </summary>
        /// <param name="objParams"></param>
        /// <returns></returns>
        public Bikewale.Entities.PriceQuote.v2.DetailedDealerQuotationEntity GetDealerPriceQuoteByPackageV2(PQParameterEntity objParams)
        {
            Bikewale.Entities.PriceQuote.v2.DetailedDealerQuotationEntity objDetailedDealerQuotationEntity = null;
            DealerQuotationEntity dealerQuotation = null;
            NewBikeDealers primaryDealer = null;
            IList<PQ_Price> PriceList = null;
            IList<BikeColorAvailability> ColorwiseAvailabilty = null;
            EMI objEMI = null;
            IList<OfferEntityBase> OfferList = null;
            IList<DealerBenefitEntity> benefits = null;
            IList<Bikewale.Entities.PriceQuote.v2.NewBikeDealerBase> secondaryDealers = null;
            IList<string> disclaimer = null;

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("bw_getdealerdetails_30082017"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_dealerId", DbType.Int32, objParams.DealerId > 0 ? Convert.ToInt32(objParams.DealerId) : Convert.DBNull));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_versionId", DbType.Int32, Convert.ToInt32(objParams.VersionId)));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_cityId", DbType.Int32, Convert.ToInt32(objParams.CityId)));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_areaId", DbType.Int32, Convert.ToInt32(objParams.AreaId)));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            objDetailedDealerQuotationEntity = new Bikewale.Entities.PriceQuote.v2.DetailedDealerQuotationEntity();
                            dealerQuotation = new DealerQuotationEntity();
                            #region Bike Details
                            if (dr.Read())
                            {
                                BikeMakeEntityBase objMake = new BikeMakeEntityBase();
                                objMake.MakeId = SqlReaderConvertor.ToInt32(dr["MakeId"]);
                                objMake.MakeName = Convert.ToString(dr["MakeName"]);
                                objMake.MaskingName = Convert.ToString(dr["MakeMaskingName"]);
                                objDetailedDealerQuotationEntity.objMake = objMake;
                                BikeModelEntityBase objModel = new BikeModelEntityBase();
                                objModel.ModelId = SqlReaderConvertor.ToInt32(dr["ModelId"]);
                                objModel.ModelName = Convert.ToString(dr["ModelName"]);
                                objModel.MaskingName = Convert.ToString(dr["ModelMaskingName"]);
                                objDetailedDealerQuotationEntity.objModel = objModel;
                                BikeVersionEntityBase objVersion = new BikeVersionEntityBase();
                                objVersion.VersionId = SqlReaderConvertor.ToInt32(dr["VersionId"]);
                                objVersion.VersionName = Convert.ToString(dr["VersionName"]);
                                objDetailedDealerQuotationEntity.objVersion = objVersion;

                                objDetailedDealerQuotationEntity.HostUrl = Convert.ToString(dr["HostURL"]);
                                objDetailedDealerQuotationEntity.OriginalImagePath = Convert.ToString(dr["OriginalImagePath"]);
                            }
                            #endregion

                            #region Price Break-Up
                            if (dr.NextResult())
                            {
                                PriceList = new List<PQ_Price>();
                                while (dr.Read())
                                {
                                    PriceList.Add(new PQ_Price()
                                    {
                                        CategoryId = SqlReaderConvertor.ToUInt32(dr["ItemId"]),
                                        CategoryName = Convert.ToString(dr["ItemName"]),
                                        Price = SqlReaderConvertor.ToUInt32(dr["Price"]),
                                        DealerId = SqlReaderConvertor.ToUInt32(dr["DealerId"]),

                                    });
                                }
                                dealerQuotation.PriceList = PriceList;
                            }
                            #endregion
                            //Read Dealer Details
                            #region Dealer Details
                            if (dr.NextResult())
                            {
                                if (dr.Read())
                                {
                                    primaryDealer = new NewBikeDealers();
                                    primaryDealer.DealerId = SqlReaderConvertor.ToUInt32(dr["ID"]);
                                    primaryDealer.Organization = Convert.ToString(dr["Organization"]);
                                    primaryDealer.Name = String.Format("{0} {1}", Convert.ToString(dr["FirstName"]), (Convert.ToString(dr["LastName"])));
                                    primaryDealer.Address = Convert.ToString(dr["Address"]);
                                    primaryDealer.PhoneNo = Convert.ToString(dr["PhoneNo"]);
                                    primaryDealer.MobileNo = Convert.ToString(dr["MobileNo"]);
                                    primaryDealer.MaskingNumber = Convert.ToString(dr["PhoneNo"]);
                                    primaryDealer.WorkingTime = Convert.ToString(dr["ContactHours"]);
                                    primaryDealer.objArea = new Bikewale.Entities.BikeBooking.AreaEntityBase()
                                    {
                                        AreaId = SqlReaderConvertor.ToUInt32(dr["AreaId"]),
                                        AreaName = Convert.ToString(dr["AreaName"]),
                                        Latitude = SqlReaderConvertor.ParseToDouble(dr["Lattitude"]),
                                        Longitude = SqlReaderConvertor.ParseToDouble(dr["Longitude"]),
                                        PinCode = Convert.ToString(dr["Pincode"])
                                    };
                                    primaryDealer.objCity = new CityEntityBase() { CityName = Convert.ToString(dr["CityName"]) };
                                    primaryDealer.objState = new StateEntityBase() { StateName = Convert.ToString(dr["StateName"]) };
                                    primaryDealer.Website = Convert.ToString(dr["WebsiteUrl"]);
                                    primaryDealer.Distance = Convert.ToString(dr["Distance"]);
                                    primaryDealer.EmailId = Convert.ToString(dr["EmailId"]);
                                    DealerPackageTypes s;
                                    if (Enum.TryParse((Convert.ToString(dr["DealerPackageType"])), out s))
                                        primaryDealer.DealerPackageType = s;
                                    primaryDealer.DisplayTextLarge = Convert.ToString(dr["DisplayTextLarge"]);
                                    primaryDealer.DisplayTextSmall = Convert.ToString(dr["DisplayTextSmall"]);
                                    primaryDealer.IsDSA = SqlReaderConvertor.ToBoolean(dr["isDSA"]);

                                }
                                dealerQuotation.DealerDetails = primaryDealer;
                            }
                            #endregion
                            //Booking Amount
                            if (dr.NextResult())
                            {
                                if (dr.Read())
                                {
                                    dealerQuotation.BookingAmount = SqlReaderConvertor.ToUInt32(dr["Amount"]);
                                }
                            }

                            //Booking Num Of Days
                            if (dr.NextResult())
                            {
                                if (dr.Read())
                                {
                                    dealerQuotation.BikeAvailability = SqlReaderConvertor.ToUInt32(dr["NumOfDays"]);
                                }
                            }

                            #region Color and Color level availability
                            if (dr.NextResult())
                            {
                                ColorwiseAvailabilty = new List<BikeColorAvailability>();
                                while (dr.Read())
                                {
                                    ColorwiseAvailabilty.Add(new BikeColorAvailability()
                                    {
                                        ColorId = SqlReaderConvertor.ToUInt32(dr["ColorId"]),
                                        NoOfDays = SqlReaderConvertor.ToInt16(dr["NumOfDays"]),
                                        DealerId = SqlReaderConvertor.ToUInt32(dr["DealerId"]),
                                        ColorName = Convert.ToString(dr["ColorName"]),
                                        HexCode = Convert.ToString(dr["HexCode"]),
                                        VersionId = SqlReaderConvertor.ToUInt32(dr["BikeVersionId"])
                                    });
                                }
                                dealerQuotation.AvailabilityByColor = ColorwiseAvailabilty;
                            }
                            #endregion
                            #region EMI Values
                            if (dr.NextResult())
                            {
                                if (dr.Read())
                                {
                                    objEMI = new EMI();
                                    objEMI.Id = SqlReaderConvertor.ToUInt32(dr["Id"]);
                                    objEMI.LoanProvider = Convert.ToString(dr["LoanProvider"]);
                                    objEMI.LoanToValue = SqlReaderConvertor.ToUInt16(dr["LTV"]);
                                    objEMI.MaxDownPayment = SqlReaderConvertor.ToUInt32(dr["MaxDownPayment"]);
                                    objEMI.MaxRateOfInterest = SqlReaderConvertor.ToUInt32(dr["MaxRateOfInterest"]);
                                    objEMI.MaxTenure = SqlReaderConvertor.ToUInt16(dr["MaxTenure"]);
                                    objEMI.MinDownPayment = SqlReaderConvertor.ToUInt32(dr["MinDownPayment"]);
                                    objEMI.MinRateOfInterest = SqlReaderConvertor.ToUInt32(dr["MinRateOfInterest"]);
                                    objEMI.MinTenure = SqlReaderConvertor.ToUInt16(dr["MinTenure"]);
                                    objEMI.ProcessingFee = SqlReaderConvertor.ToUInt32(dr["ProcessingFee"]);
                                    objEMI.RateOfInterest = SqlReaderConvertor.ToFloat(dr["RateOfInterest"]);
                                    objEMI.Tenure = SqlReaderConvertor.ToUInt16(dr["Tenure"]);
                                }
                                dealerQuotation.EMIDetails = objEMI;
                            }
                            #endregion
                            #region Dealer Offers
                            if (dr.NextResult())
                            {
                                OfferList = new List<OfferEntityBase>();
                                while (dr.Read())
                                {
                                    OfferList.Add(new OfferEntityBase()
                                    {
                                        OfferId = SqlReaderConvertor.ToUInt32(dr["OfferId"]),
                                        OfferCategoryId = SqlReaderConvertor.ToUInt32(dr["OfferCategoryId"]),
                                        OfferType = Convert.ToString(dr["OfferType"]),
                                        OfferText = Convert.ToString(dr["OfferText"]),
                                        OfferValue = SqlReaderConvertor.ToUInt32(dr["OfferValue"]),
                                        OffervalidTill = SqlReaderConvertor.ToDateTime(dr["OfferValidTill"]),
                                        DealerId = SqlReaderConvertor.ToUInt32(dr["DealerId"]),
                                        IsOfferTerms = SqlReaderConvertor.ToBoolean(dr["IsOfferTerms"])
                                    });
                                }
                                dealerQuotation.OfferList = OfferList;
                            }
                            #endregion
                            #region Dealer Benefits
                            if (dr.NextResult())
                            {
                                benefits = new List<DealerBenefitEntity>();
                                while (dr.Read())
                                {
                                    DealerBenefitEntity objDealerBenefitEntity = new DealerBenefitEntity();
                                    objDealerBenefitEntity.BenefitId = SqlReaderConvertor.ToInt32(dr["BenefitId"]);
                                    objDealerBenefitEntity.DealerId = SqlReaderConvertor.ToInt32(dr["DealerId"]);
                                    objDealerBenefitEntity.CatId = SqlReaderConvertor.ToInt32(dr["CatId"]);
                                    objDealerBenefitEntity.CategoryText = Convert.ToString(dr["CategoryText"]);
                                    objDealerBenefitEntity.BenefitText = Convert.ToString(dr["BenefitText"]);
                                    objDealerBenefitEntity.City = Convert.ToString(dr["City"]);
                                    benefits.Add(objDealerBenefitEntity);
                                }
                                dealerQuotation.Benefits = benefits;
                            }
                            #endregion
                            #region Disclaimer
                            if (dr.NextResult())
                            {
                                disclaimer = new List<string>();
                                while (dr.Read())
                                {
                                    disclaimer.Add(Convert.ToString(dr["Disclaimer"]));
                                }
                                dealerQuotation.Disclaimer = disclaimer;
                            }
                            #endregion

                            #region Secondary Dealer list
                            if (dr.NextResult())
                            {
                                secondaryDealers = new List<Bikewale.Entities.PriceQuote.v2.NewBikeDealerBase>();
                                DealerPackageTypes s;
                                while (dr.Read())
                                {
                                    secondaryDealers.Add(
                                        new Bikewale.Entities.PriceQuote.v2.NewBikeDealerBase()
                                        {
                                            Area = Convert.ToString(dr["AreaName"]),
                                            DealerId = SqlReaderConvertor.ToUInt32(dr["ID"]),
                                            Name = Convert.ToString(dr["Organization"]),
                                            MaskingNumber = Convert.ToString(dr["MaskingNumber"]),
                                            DealerPackageType = (Enum.TryParse((Convert.ToString(dr["DealerPackageType"])), out s)) ? s : DealerPackageTypes.Invalid,
                                            Distance = SqlReaderConvertor.ParseToDouble(dr["distance"]),
                                            OfferCount = SqlReaderConvertor.ToUInt16(dr["offerCount"]),
                                            DisplayTextLarge = Convert.ToString(dr["DisplayTextLarge"]),
                                            DisplayTextSmall = Convert.ToString(dr["DisplayTextSmall"]),
                                            IsDSA = SqlReaderConvertor.ToBoolean(dr["isDSA"])
                                        }
                                        );
                                }
                            }

                            #endregion

                            #region Secondary Dealer with version prices
                            IList<VersionPriceEntity> versionprices = null;
                            if (dr.NextResult())
                            {
                                versionprices = new List<VersionPriceEntity>();
                                while (dr.Read())
                                {
                                    versionprices.Add(new VersionPriceEntity()
                                    {
                                        DealerId = SqlReaderConvertor.ToUInt32(dr["DealerId"]),
                                        VersionId = SqlReaderConvertor.ToUInt32(dr["BikeVersionId"]),
                                        VersionPrice = SqlReaderConvertor.ToUInt32(dr["OnRoadPrice"])
                                    });
                                }

                                if (versionprices != null && versionprices.Count > 0)
                                {
                                    var dealerversionPrices = from item in versionprices
                                                              group item by item.DealerId into newGroup
                                                              orderby newGroup.Key
                                                              select newGroup;

                                    foreach (var dealer in dealerversionPrices)
                                    {
                                        if (dealer.Key > 0)
                                        {
                                            var curSecondaryDealer = secondaryDealers.Where(i => i.DealerId == dealer.Key).FirstOrDefault();
                                            if (curSecondaryDealer != null)
                                            {
                                                curSecondaryDealer.Versions = dealer.ToList();
                                            }
                                        }
                                    }
                                    //Set the Selected version price for secondary dealers for easy binding
                                    foreach (var secDealer in secondaryDealers)
                                    {
                                        secDealer.SelectedVersionPrice = (from verPrice in versionprices
                                                                          where (verPrice.DealerId == secDealer.DealerId)
                                                                          && (verPrice.VersionId == objParams.VersionId)
                                                                          select verPrice.VersionPrice).FirstOrDefault();

                                    }

                                }
                            }

                            #endregion
                        }
                        objDetailedDealerQuotationEntity.PrimaryDealer = dealerQuotation;
                        objDetailedDealerQuotationEntity.SecondaryDealers = secondaryDealers;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.DealerPriceQuoteRepository.GetDealerPriceQuoteByPackage");

            }
            return objDetailedDealerQuotationEntity;
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 18 Jul 2017
        /// Description :   Returns the primary dealer by model and city
        /// Primary dealer allocation is by random logic. because area is not given
        /// </summary>
        /// <param name="modelId"></param>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public DealerInfo GetNearestDealer(uint modelId, uint cityId)
        {
            DealerInfo objDealersList = null;

            try
            {
                if (modelId > 0 && cityId > 0)
                {
                    using (DbCommand cmd = DbFactory.GetDBCommand("getnearestdealerbymodelcity"))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(DbFactory.GetDbParam("par_modelId", DbType.Int32, modelId));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_cityid", DbType.Int32, cityId));

                        using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                        {
                            objDealersList = new DealerInfo();

                            while (dr.Read())
                            {
                                objDealersList.DealerId = SqlReaderConvertor.ToUInt32(dr["DealerId"]);
                                objDealersList.IsDealerAvailable = SqlReaderConvertor.ToBoolean(dr["IsDealerAvailable"]);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("GetNearestDealer({0},{1})", modelId, cityId));
            }
            return objDealersList;
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 18 Jul 2017
        /// Description :   Nearest primary dealer is returned
        /// </summary>
        /// <param name="modelId"></param>
        /// <param name="cityId"></param>
        /// <param name="areaId"></param>
        /// <returns></returns>
        public DealerInfo GetNearestDealer(uint modelId, uint cityId, uint areaId)
        {
            DealerInfo objDealersList = null;

            try
            {
                if (modelId > 0 && cityId > 0 && areaId > 0)
                {
                    using (DbCommand cmd = DbFactory.GetDBCommand("getnearestdealerbymodelcityarea"))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(DbFactory.GetDbParam("par_modelid", DbType.Int32, modelId));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_cityid", DbType.Int32, cityId));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_areaid", DbType.Int32, areaId));

                        using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                        {
                            objDealersList = new DealerInfo();

                            while (dr.Read())
                            {
                                objDealersList.DealerId = SqlReaderConvertor.ToUInt32(dr["DealerId"]);
                                objDealersList.IsDealerAvailable = SqlReaderConvertor.ToBoolean(dr["IsDealerAvailable"]);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("GetNearestDealer({0},{1},{2})", modelId, cityId, areaId));
            }
            return objDealersList;
        }
    } // class
} // namespace

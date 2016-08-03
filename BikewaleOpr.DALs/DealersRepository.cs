using Bikewale.Notifications;
using BikewaleOpr.Entities;
using BikewaleOpr.Interface;
using MySql.CoreDAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Web;

namespace BikewaleOpr.DAL
{
    public class DealersRepository : IDealers
    {
        /// <summary>
        /// Written By : Ashwini Todkar on 29 Oct 2014
        /// </summary>
        /// <param name="objParams"></param>
        /// <returns></returns>
        public PQ_DealerDetailEntity GetDealerDetailsPQ(PQParameterEntity objParams)
        {
            throw new NotImplementedException();
            //PQ_DealerDetailEntity objDetailPQ = null;
            //List<PQ_BikeVarient> varients = null;
            //IList<PQ_VersionPrice> priceSplits = null;
            //try
            //{
            //    using (DbCommand cmd = DbFactory.GetDBCommand("BW_GetDealerDetails_08012016"))
            //    {
            //        cmd.CommandType = CommandType.StoredProcedure;
            //        cmd.Parameters.Add(DbFactory.GetDbParam("par_dealerId", DbType.Int64, Convert.ToInt64(objParams.DealerId)));
            //        cmd.Parameters.Add(DbFactory.GetDbParam("par_versionId", DbType.Int64, Convert.ToInt64(objParams.VersionId)));
            //        cmd.Parameters.Add(DbFactory.GetDbParam("par_cityId", DbType.Int64, Convert.ToInt64(objParams.CityId)));

            //        using (IDataReader dr = MySqlDatabase.SelectQuery(cmd))
            //        {
            //            objDetailPQ = new PQ_DealerDetailEntity();

            //            objDetailPQ.objQuotation = null;

            //            //Get vehicle details
            //            if (dr.Read())
            //            {
            //                objDetailPQ.objQuotation = new PQ_QuotationEntity()
            //                {
            //                    objMake = new BikeMakeEntityBase() { MakeName = dr["MakeName"].ToString(), MaskingName = dr["MakeMaskingName"].ToString() },
            //                    objModel = new BikeModelEntityBase() { ModelName = dr["ModelName"].ToString(), MaskingName = dr["ModelMaskingName"].ToString() },
            //                    objVersion = new BikeVersionEntityBase() { VersionName = dr["VersionName"].ToString() },
            //                    HostUrl = dr["HostURL"].ToString(),
            //                    LargePicUrl = dr["largePic"].ToString(),
            //                    SmallPicUrl = dr["smallPic"].ToString(),
            //                    OriginalImagePath = dr["OriginalImagePath"].ToString()
            //                };
            //            }

            //            //Get price details
            //            dr.NextResult();

            //            objDetailPQ.objQuotation.PriceList = new List<PQ_Price>();

            //            while (dr.Read())
            //            {
            //                if (Convert.ToUInt32(dr["Price"]) > 0)
            //                    objDetailPQ.objQuotation.PriceList.Add(new PQ_Price() { CategoryName = dr["ItemName"].ToString(), Price = Convert.ToUInt32(dr["Price"]), CategoryId = Convert.ToUInt32(dr["ItemId"]) });
            //            }

            //            dr.NextResult();

            //            // Get dealer disclaimer
            //            objDetailPQ.objQuotation.Disclaimer = new List<string>();
            //            while (dr.Read())
            //                objDetailPQ.objQuotation.Disclaimer.Add(dr["Disclaimer"].ToString());

            //            //Get Offer details
            //            dr.NextResult();
            //            objDetailPQ.objOffers = new List<OfferEntity>();

            //            while (dr.Read())
            //            {
            //                objDetailPQ.objOffers.Add(new OfferEntity() { OfferText = dr["OfferText"].ToString(), OfferType = dr["OfferType"].ToString(), OfferValue = Convert.ToUInt32(dr["OfferValue"]), OfferCategoryId = Convert.ToUInt32(dr["OfferCategoryId"]), OfferId = Convert.ToUInt32(dr["OfferId"]), IsOfferTerms = Convert.ToBoolean(dr["IsOfferTerms"]), IsPriceImpact = Convert.ToBoolean(dr["IsPriceImpact"]) });
            //            }

            //            //On road price for the varients
            //            if (dr.NextResult())
            //            {
            //                varients = new List<PQ_BikeVarient>();
            //                while (dr.Read())
            //                {
            //                    varients.Add(new PQ_BikeVarient()
            //                    {
            //                        objMake = new BikeMakeEntityBase()
            //                        {
            //                            MakeId = Convert.ToInt32(dr["MakeId"]),
            //                            MakeName = Convert.ToString(dr["MakeName"]),
            //                            MaskingName = Convert.ToString(dr["MakeMaskingName"])
            //                        },
            //                        objModel = new BikeModelEntityBase()
            //                        {
            //                            ModelId = Convert.ToInt32(dr["ModelId"]),
            //                            ModelName = Convert.ToString(dr["ModelName"]),
            //                            MaskingName = Convert.ToString(dr["ModelMaskingName"])
            //                        },
            //                        objVersion = new BikeVersionEntityBase()
            //                        {
            //                            VersionId = Convert.ToInt32(dr["VersionId"]),
            //                            VersionName = Convert.ToString(dr["VersionName"])
            //                        },
            //                        HostUrl = Convert.ToString(dr["HostURL"]),
            //                        OriginalImagePath = Convert.ToString(dr["OriginalImagePath"]),
            //                        OnRoadPrice = Convert.ToUInt32(dr["OnRoadPrice"]),
            //                        BookingAmount = Convert.ToUInt32(dr["BookingAmount"]),
            //                        PriceList = new List<PQ_Price>()
            //                    });
            //                }
            //                if (dr.NextResult())
            //                {
            //                    priceSplits = new List<PQ_VersionPrice>();
            //                    while (dr.Read())
            //                    {
            //                        priceSplits.Add(
            //                            new PQ_VersionPrice()
            //                            {
            //                                CategoryId = Convert.ToUInt32(dr["ItemId"]),
            //                                CategoryName = Convert.ToString(dr["ItemName"]),
            //                                DealerId = Convert.ToUInt32(dr["DealerId"]),
            //                                Price = Convert.ToUInt32(dr["Price"]),
            //                                VersionId = Convert.ToUInt32(dr["VersionId"])
            //                            }
            //                        );
            //                    }
            //                }

            //                if ((varients != null && varients.Count > 0) && (priceSplits != null && priceSplits.Count > 0))
            //                {
            //                    varients.ForEach(
            //                        varient => varient.PriceList =
            //                            (
            //                                from price in priceSplits
            //                                where price.VersionId == varient.objVersion.VersionId
            //                                select new PQ_Price()
            //                                {
            //                                    CategoryId = price.CategoryId,
            //                                    CategoryName = price.CategoryName,
            //                                    DealerId = price.DealerId,
            //                                    Price = price.Price
            //                                }
            //                            ).ToList()
            //                        );
            //                }
            //                objDetailPQ.objQuotation.Varients = varients;
            //            }

            //            //Get Dealer details
            //            dr.NextResult();

            //            if (dr.Read())
            //            {
            //                objDetailPQ.objDealer = new NewBikeDealers();

            //                objDetailPQ.objDealer.DealerId = objParams.DealerId;
            //                objDetailPQ.objDealer.Name = (dr["FirstName"] != null) ? dr["FirstName"].ToString() : "";
            //                objDetailPQ.objDealer.Address = dr["Address"].ToString();
            //                objDetailPQ.objDealer.EmailId = dr["EmailId"].ToString();

            //                objDetailPQ.objDealer.MobileNo = (dr["MobileNo"] != null) ? dr["MobileNo"].ToString() : "";
            //                objDetailPQ.objDealer.PhoneNo = dr["PhoneNo"].ToString();

            //                objDetailPQ.objDealer.objArea = new AreaEntityBase() { Latitude = Convert.ToDouble(dr["Lattitude"]), Longitude = Convert.ToDouble(dr["Longitude"]), AreaName = dr["AreaName"].ToString(), PinCode = dr["Pincode"].ToString() };

            //                objDetailPQ.objDealer.objCity = new CityEntityBase() { CityName = dr["CityName"].ToString() };

            //                objDetailPQ.objDealer.objState = new StateEntityBase() { StateName = dr["StateName"].ToString() };
            //                objDetailPQ.objDealer.Website = (dr["WebsiteUrl"] != null) ? dr["WebsiteUrl"].ToString() : "";
            //                objDetailPQ.objDealer.Organization = (dr["Organization"] != null) ? dr["Organization"].ToString() : "";
            //                objDetailPQ.objDealer.WorkingTime = (dr["ContactHours"] != null) ? dr["ContactHours"].ToString() : "";
            //            }

            //            //Get facilities list provided by dealer

            //            dr.NextResult();

            //            objDetailPQ.objFacilities = new List<FacilityEntity>();

            //            while (dr.Read())
            //            {
            //                objDetailPQ.objFacilities.Add(new FacilityEntity() { Facility = dr["Facility"].ToString() });
            //            }

            //            //Get EMI options provided by dealers

            //            dr.NextResult();

            //            if (dr.Read())
            //            {
            //                objDetailPQ.objEmi = new EMI() { RateOfInterest = Convert.ToSingle(dr["RateOfInterest"]), LoanToValue = Convert.ToUInt16(dr["LTV"]), Tenure = Convert.ToUInt16(dr["Tenure"]), LoanProvider = dr["LoanProvider"].ToString() };
            //            }

            //            //Get Bike Booking amount
            //            dr.NextResult();

            //            if (dr.Read())
            //            {
            //                objDetailPQ.objBookingAmt = new BookingAmountEntityBase() { Amount = Convert.ToUInt32(dr["Amount"]) };
            //            }
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    HttpContext.Current.Trace.Warn("Exception at GetDealerDetails : " + ex.Message + ex.Source);
            //    ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
            //    objErr.SendMail();
            //}

            //return objDetailPQ;
        }


        /// <summary>
        /// Written By : Ashish G. Kamble on 31 Oct 2014.
        /// Summary : Function to get all facilities provided by the dealer.
        /// </summary>
        /// <param name="dealerId">Id of the dealer whose facilities are required.</param>
        /// <returns>Returns list of the facilities for the given dealer id.</returns>
        public List<FacilityEntity> GetDealerFacilities(uint dealerId)
        {
            List<FacilityEntity> objFacilities = null;

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("BW_GetDealerFacilities"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_DealerId", DbType.Int32, dealerId));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        objFacilities = new List<FacilityEntity>();

                        while (dr.Read())
                        {
                            objFacilities.Add(new FacilityEntity()
                            {
                                Facility = dr["Facility"].ToString(),
                                Id = Convert.ToInt32(dr["Id"]),
                                IsActive = Convert.ToBoolean(dr["IsActive"])
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("Exception at GetDealerFacilities : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            return objFacilities;

        }   // End of GetDealerFacilities


        /// <summary>
        /// Written By : Ashish G. Kamble on 7 Nov 2014
        /// Summary : Function to save the dealer facility
        /// </summary>
        /// <param name="dealerId"></param>
        /// <param name="facility"></param>
        /// <param name="isActive"></param>
        public void SaveDealerFacility(uint dealerId, string facility, bool isActive)
        {
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("bw_savedealerfacility"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_Facility", DbType.String, 500, facility));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_IsActive", DbType.Boolean, isActive));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_DealerId", DbType.Int32, dealerId));

                    MySqlDatabase.InsertQuery(cmd, ConnectionType.MasterDatabase);

                }
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("Exception at SaveDealerFacility : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }

        /// <summary>
        /// Written By : Ashish G. Kamble on 7 Nov 2014
        /// Summary : Function to update the dealer facility.
        /// </summary>
        /// <param name="facilityId"></param>
        /// <param name="facility"></param>
        /// <param name="isActive"></param>
        public void UpdateDealerFacility(uint facilityId, string facility, bool isActive)
        {
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("BW_UpdateDealerFacility"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_Facility", DbType.String, 500, facility));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_IsActive", DbType.Boolean, isActive));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_FacilityId", DbType.Int32, facilityId));
                    MySqlDatabase.UpdateQuery(cmd, ConnectionType.MasterDatabase);
                }
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("Exception at UpdateDealerFacility : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }

        public void SaveDealerLoanAmounts(uint dealerId, ushort tenure, float rateOfInterest, ushort ltv, string loanProvider)
        {
            throw new NotImplementedException();
            //try
            //{
            //    using (DbCommand cmd = DbFactory.GetDBCommand("BW_SaveDealerLoanAmounts"))
            //    {
            //        cmd.CommandType = CommandType.StoredProcedure;
            //        cmd.Parameters.Add(DbFactory.GetDbParam("par_DealerId", DbType.Int32, dealerId));
            //        cmd.Parameters.Add(DbFactory.GetDbParam("par_Tenure", DbType.Byte, tenure));
            //        cmd.Parameters.Add(DbFactory.GetDbParam("par_RateOfInterest", DbType.String, 20, rateOfInterest));
            //        cmd.Parameters.Add(DbFactory.GetDbParam("par_LTV", DbType.Byte, ltv));
            //        cmd.Parameters.Add(DbFactory.GetDbParam("par_LoanProvider", DbType.String, 100, String.IsNullOrEmpty(loanProvider) ? Convert.DBNull : loanProvider));
            //        MySqlDatabase.InsertQuery(cmd, ConnectionType.MasterDatabase);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    HttpContext.Current.Trace.Warn("Exception at SaveDealerLoanAmounts : " + ex.Message + ex.Source);
            //    ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
            //    objErr.SendMail();
            //}
        }

        public void UpdateDealerLoanAmounts(uint dealerId, ushort tenure, float rateOfInterest, ushort ltv, string loanProvider)
        {
            throw new NotImplementedException();
            //try
            //{
            //    using (DbCommand cmd = DbFactory.GetDBCommand("BW_UpdateDealerLoanAmounts"))
            //    {
            //        cmd.CommandType = CommandType.StoredProcedure;
            //        cmd.Parameters.Add(DbFactory.GetDbParam("par_DealerId", DbType.Int32, dealerId));
            //        cmd.Parameters.Add(DbFactory.GetDbParam("par_Tenure", DbType.Byte, tenure));
            //        cmd.Parameters.Add(DbFactory.GetDbParam("par_RateOfInterest", DbType.String, 20, rateOfInterest));
            //        cmd.Parameters.Add(DbFactory.GetDbParam("par_LTV", DbType.Byte, ltv));
            //        cmd.Parameters.Add(DbFactory.GetDbParam("par_LoanProvider", DbType.String, 100, String.IsNullOrEmpty(loanProvider) ? Convert.DBNull : loanProvider));

            //        MySqlDatabase.InsertQuery(cmd, ConnectionType.MasterDatabase);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    HttpContext.Current.Trace.Warn("Exception at UpdateDealerLoanAmounts : " + ex.Message + ex.Source);
            //    ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
            //    objErr.SendMail();
            //}
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
                ErrorClass objErr = new ErrorClass(ex, "GetDealerLoanAmounts");
                objErr.SendMail();
            }
            return objEmi;
        }

        /// <summary>
        /// Written By : Suresh Prajapati on 29th Oct 2014.
        /// Description : To Get Dealer's Name By Selected City.
        /// </summary>
        /// <param name="cityId"></param>
        /// <returns>Dealer's Name</returns>

        public DataTable GetAllDealers(UInt32 cityId)
        {

            DataTable dt = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("bw_getbikedealers"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_cityId", DbType.Int32, cityId));
                    dt = MySqlDatabase.SelectAdapterQuery(cmd, ConnectionType.ReadOnly).Tables[0];

                }
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("GetAllDealers ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            return dt;
        }

        /// <summary>
        /// Created By : Suresh Prajapati on 29th Oct 2014
        /// Description : To Get Dealer Cities for which Bike Dealer exists.
        /// </summary>
        /// <returns>City Name</returns>

        public DataTable GetDealerCities()
        {

            throw new NotImplementedException();
            //DataTable dt = null;
            //try
            //{
            //    using (DbCommand cmd = DbFactory.GetDBCommand("BW_GetBikeDealerCities"))
            //    {
            //        cmd.CommandType = CommandType.StoredProcedure;
            //        dt = MySqlDatabase.SelectAdapterQuery(cmd).Tables[0];
            //    }
            //}
            //catch (Exception ex)
            //{
            //    HttpContext.Current.Trace.Warn("GetDealerCities ex : " + ex.Message + ex.Source);
            //    ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
            //    objErr.SendMail();
            //}
            //return dt;
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
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            return dt;
        }

        /// <summary>
        /// Created By  : Suresh Prajapati on 03rd Nov, 2014
        /// Description : To Get Dealer Offer Details for particular Dealer.
        /// 
        /// Modified By : Suresh Prajapati on 30th Dec, 2014
        /// Description : Added City name column in the list
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
                                objOffer.objMake = new BikeMakeEntityBase() { MakeName = dr["MakeName"].ToString() };
                                objOffer.objModel = new BikeModelEntityBase() { ModelName = dr["ModelName"].ToString() };
                                objOffer.objCity = new CityEntityBase() { CityName = dr["CityName"].ToString() };
                                objOffer.OfferId = Convert.ToUInt32(dr["Id"]);
                                objOffer.OfferType = dr["OfferType"].ToString();
                                objOffer.OfferTypeId = Convert.ToUInt32(dr["OfferTypeId"].ToString());
                                objOffer.OfferText = dr["OfferText"].ToString();
                                objOffer.OfferValue = Convert.ToUInt32(dr["OfferValue"].ToString());
                                if (!String.IsNullOrEmpty(dr["OfferValidTill"].ToString()))
                                    objOffer.OffervalidTill = DateTime.Parse(dr["OfferValidTill"].ToString());
                                objOffer.IsPriceImpact = Convert.ToBoolean(Convert.ToString(dr["IsPriceImpact"]));
                                objOffer.Terms = dr["Terms"].ToString();
                                objOffers.Add(objOffer);
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("GetDealerOffers ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
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
        /// </summary>
        /// <param name="dealerId"></param>
        /// <param name="cityId"></param>
        /// <param name="modelId"></param>
        /// <param name="offercategoryId"></param>
        /// <param name="offerText"></param>
        /// <param name="offerValue"></param>
        /// <param name="offervalidTill"></param>
        /// <returns></returns>

        public bool SaveDealerOffer(int dealerId, uint userId, int cityId, string modelId, int offercategoryId, string offerText, int? offerValue, DateTime offervalidTill, bool isPriceImpact, string termsConditions)
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

                   cmd.Parameters.Add(DbFactory.GetDbParam("par_terms", DbType.String, -1, HttpContext.Current.Server.HtmlDecode(termsConditions)));
                    
                    MySqlDatabase.InsertQuery(cmd, ConnectionType.MasterDatabase);
                    isSuccess = Convert.ToBoolean(cmd.Parameters["par_result"].Value);

                }
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("SaveDealerOffer ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            return isSuccess;
        }

        /// <summary>
        /// Created By  : Suresh Prajapati on 28th Dec, 2014.
        /// Description : To Edit/Update added Bike offers
        /// </summary>
        /// <param name="offerId"></param>
        /// <param name="userId"></param>
        /// <param name="offerCategoryId"></param>
        /// <param name="offerText"></param>
        /// <param name="offerValue"></param>
        /// <param name="offerValidTill"></param>
        public void UpdateDealerBikeOffers(uint offerId, uint userId, uint offerCategoryId, string offerText, uint? offerValue, DateTime offerValidTill, bool isPriceImpact, string termsConditions)
        {
            try
            {

                using (DbCommand cmd = DbFactory.GetDBCommand("bw_updatedealeroffers_07012016"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_OfferId", DbType.Int32, offerId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_UserId", DbType.Int64, userId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_OfferCategoryId", DbType.Int32, offerCategoryId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_OfferText", DbType.String, offerText));
                    if (offerValue == null)
                    {
                        offerValue = 0;
                    }
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_OfferValue", DbType.Int32, offerValue));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_OfferValidTill", DbType.DateTime, offerValidTill));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_isPriceImpact", DbType.Boolean, isPriceImpact));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_terms", DbType.String, termsConditions));
                    MySqlDatabase.UpdateQuery(cmd, ConnectionType.MasterDatabase);
                }
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("Exception at UpdateDealerBikeOffers : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
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
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
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
                    //    cmd.Parameters.Add(DbFactory.GetDbParam("par_BikeModelId", DbType.Int32, bikemodelId;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_BikeVersionId", DbType.Int32, bikeversionId > 0 ? bikeversionId : Convert.DBNull));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_NumOfDays", DbType.Int32, numOfDays));


                    if (MySqlDatabase.InsertQuery(cmd, ConnectionType.MasterDatabase))
                        isSuccess = true;
                }
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("SaveBikeAvailability ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            return isSuccess;
        }

        /// <summary>
        /// Created By  : Suresh Prajapati on 11th Nov, 2014.
        /// Description : Method to save New Bike Availability.
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public bool SaveBikeAvailability(DataTable dt)
        {

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("bw_savebikeavailability"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.UpdatedRowSource = UpdateRowSource.None;

                    cmd.Parameters.Add(DbFactory.GetDbParamWithColumnName("par_BikeVersionId", DbType.Int32, 8, dt.Columns[1].ColumnName));
                    cmd.Parameters.Add(DbFactory.GetDbParamWithColumnName("par_DealerId", DbType.Int32, 8, dt.Columns[0].ColumnName));
                    cmd.Parameters.Add(DbFactory.GetDbParamWithColumnName("par_NumOfDays", DbType.Int32, 8, dt.Columns[2].ColumnName));

                    //run the command

                    return (MySqlDatabase.InsertQueryViaAdaptor(cmd, dt, ConnectionType.MasterDatabase) > 0);
                }

            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("SaveDealerPrice ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }

            return false;
        }

        public bool DeleteBikeAvailabilityDays(DataTable dt)
        {

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("BW_DeleteBikeAvailability"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.UpdatedRowSource = UpdateRowSource.None;

                    cmd.Parameters.Add(DbFactory.GetDbParamWithColumnName("par_BikeVersionId", DbType.Int32, 8, dt.Columns[0].ColumnName));
                    cmd.Parameters.Add(DbFactory.GetDbParamWithColumnName("par_DealerId", DbType.Int32, 8, dt.Columns[1].ColumnName));

                    //run the command

                    return (MySqlDatabase.UpdateQueryViaAdaptor(cmd, dt, ConnectionType.MasterDatabase) > 0);
                }

            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("DeleteBikeAvailabilityDays ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }

            return false;
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
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
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
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
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
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
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
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
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
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }

        /// <summary>
        /// Created By  : Suresh Prajapati on 03rd Dec, 2014.
        /// Description : To Update Disclaimer of a Dealer.
        /// </summary>
        /// <param name="dealerId"></param>
        /// <param name="disclaimer"></param>
        public void UpdateDealerDisclaimer(uint dealerId, uint versionId, string disclaimer)
        {
            throw new NotImplementedException();
            //try
            //{
            //    using (DbCommand cmd = DbFactory.GetDBCommand("BW_UpdateDealerDisclaimer"))
            //    {
            //        cmd.CommandType = CommandType.StoredProcedure;
            //        cmd.Parameters.Add(DbFactory.GetDbParam("par_DealerId", DbType.Int32, dealerId));
            //        cmd.Parameters.Add(DbFactory.GetDbParam("par_VersionId", DbType.Int32, versionId));
            //        cmd.Parameters.Add(DbFactory.GetDbParam("par_Disclaimer", DbType.String, disclaimer));
            //        MySqlDatabase.UpdateQuery(cmd, ConnectionType.MasterDatabase);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    HttpContext.Current.Trace.Warn("Exception at UpdateDealerDisclaimer : " + ex.Message + ex.Source);
            //    ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
            //    objErr.SendMail();
            //}
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
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
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
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }

            return false;
        }

        #region Booking Amount functionality

        /// <summary>
        /// Written By : Ashwini Todkar on 17 Dec 2014
        /// Summary    : Method to insert bike booking amount for a dealer
        /// </summary>
        /// <param name="dealerId"></param>
        /// <param name="modelId"></param>
        /// <param name="versionId"></param>
        /// <param name="amount">booking amount</param>
        /// <returns>isrecord inserted</returns>
        public bool SaveBookingAmount(BookingAmountEntity objBookingAmt)
        {
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("bw_savebookingamount"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_DealerId", DbType.Int32, objBookingAmt.objDealer.DealerId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_BikeModelId", DbType.Int32, objBookingAmt.objModel.ModelId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_BikeVersionId", DbType.Int32, (objBookingAmt.objVersion.VersionId > 0) ? objBookingAmt.objVersion.VersionId : Convert.DBNull));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_Amount", DbType.Int32, objBookingAmt.objBookingAmountEntityBase.Amount));

                    return (MySqlDatabase.InsertQuery(cmd, ConnectionType.MasterDatabase));
                }
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("SaveBookingAmount ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }

            return false;
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
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }

            return isSuccess;
        }

        /// <summary>
        /// Written By : Ashwini Todkar on 17 Dec 2014
        /// Summary    : Method to get bike booking details
        /// </summary>
        /// <param name="dealerId"></param>
        /// <returns></returns>
        public List<BookingAmountEntity> GetBikeBookingAmount(uint dealerId)
        {
            List<BookingAmountEntity> objBookingAmt = null;

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("BW_GetBikeBookingAmount"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_DealerId", DbType.Int32, dealerId));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            objBookingAmt = new List<BookingAmountEntity>();

                            BookingAmountEntity objAmount = null;

                            //Get booking amount with details
                            while (dr.Read())
                            {
                                objAmount = new BookingAmountEntity()
                                {
                                    objMake = new BikeMakeEntityBase { MakeName = dr["BikeMake"].ToString() },
                                    objModel = new BikeModelEntityBase { ModelName = dr["BikeModel"].ToString() },
                                    objVersion = new BikeVersionEntityBase { VersionName = dr["BikeVersion"].ToString() },
                                    objBookingAmountEntityBase = new BookingAmountEntityBase { Amount = Convert.ToUInt32(dr["Amount"]), Id = Convert.ToUInt32(dr["id"]) }
                                };
                                objBookingAmt.Add(objAmount);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("GetBikeBookingAmount ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }

            return objBookingAmt;
        }

        #endregion


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
            throw new NotImplementedException();
            //BookingAmountEntity objBookingDetails = null;

            //try
            //{
            //    using (DbCommand cmd = DbFactory.GetDBCommand("BW_GetDealerBookingAmount"))
            //    {

            //        cmd.CommandType = CommandType.StoredProcedure;
            //        cmd.Parameters.Add(DbFactory.GetDbParam("par_VersionId", DbType.Int32, versionId));
            //        cmd.Parameters.Add(DbFactory.GetDbParam("par_DealerId", DbType.Int32, dealerId));


            //        using (IDataReader dr = MySqlDatabase.SelectQuery(cmd))
            //        {
            //            if (dr.Read())
            //            {
            //                objBookingDetails = new BookingAmountEntity()
            //                {
            //                    objBookingAmountEntityBase = new BookingAmountEntityBase()
            //                    {
            //                        Amount = Convert.ToUInt32(dr["Amount"]),
            //                        IsActive = Convert.ToBoolean(dr["IsActive"]),
            //                        Id = Convert.ToUInt32(dr["Id"])
            //                    },
            //                    objMake = new BikeMakeEntityBase()
            //                    {
            //                        MakeName = dr["MakeName"].ToString()
            //                    },
            //                    objModel = new BikeModelEntityBase()
            //                    {
            //                        ModelName = dr["ModelName"].ToString()
            //                    }

            //                };

            //            }
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    HttpContext.Current.Trace.Warn("GetBikeBookingAmount ex : " + ex.Message + ex.Source);
            //    ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
            //    objErr.SendMail();
            //}
            //return objBookingDetails;
        }

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
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
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
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
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
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
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
                ErrorClass objErr = new ErrorClass(ex, "DeleteDealerBenefits");
                objErr.SendMail();
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
                ErrorClass objErr = new ErrorClass(ex, "SaveDealerBenefit");
                objErr.SendMail();
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
                ErrorClass objErr = new ErrorClass(ex, "SaveDealerEMI");
                objErr.SendMail();
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
                ErrorClass objErr = new ErrorClass(ex, "SaveDealerEMI");
                objErr.SendMail();
            }
            return false;
        }
    }
}

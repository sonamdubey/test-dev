﻿using Bikewale.Interfaces.BikeBooking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bikewale.Entities.BikeBooking;
using Bikewale.Notifications;
using Bikewale.CoreDAL;
using System.Data.SqlClient;
using System.Data;
using Bikewale.Entities.PriceQuote;
using Bikewale.Utility;

namespace Bikewale.DAL.BikeBooking
{
    /// <summary>
    /// Created by  :   Sumit Kate on 05 Feb 2016
    /// Description :   Booking Listing Repository
    /// </summary>
    public class BookingListingRepository : IBookingListing
    {
        /// <summary>
        /// Created by  :   Sumit Kate on 05 Feb 2016
        /// Description :   
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="areaId"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public IEnumerable<Entities.BikeBooking.BikeBookingListingEntity> FetchBookingList(int cityId, uint areaId, Entities.BikeBooking.BookingListingFilterEntity filter, out int totalCount, out int fetchedCount,out PagingUrl pageUrl)
        {
            IEnumerable<BikeBookingListingEntity> lstSearchResult = null;
            List<BikeBookingListingEntity> lstBikeBookingListingEntity = null;
            IList<DealerPriceCategoryItemEntity> lstVersionPrice = null;
            IList<PQ_Price> lstPQList = null;
            IList<BookingOfferEntity> lstBookingOffer = null;
            Database db = null;
            int currentPage = 0;
            totalCount = 0;
            fetchedCount = 0;
            pageUrl = null;
            try
            {
                db = new Database();

                if (areaId > 0)
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "GetBookingListing";
                        cmd.Parameters.Add("@AreaId", SqlDbType.Int).Value = Convert.ToInt32(areaId);
                        #region Filters
                        if (filter != null)
                        {
                            if (!String.IsNullOrEmpty(filter.MakeIds))
                            {
                                cmd.Parameters.AddWithValue("@paramMakeIds", filter.MakeIds.Replace(' ', ','));
                            }
                            if (!String.IsNullOrEmpty(filter.Budget))
                            {
                                cmd.Parameters.AddWithValue("@paramMinValBudget", filter.Budget.Split('-')[0]);
                            }
                            if (!String.IsNullOrEmpty(filter.Budget))
                            {
                                cmd.Parameters.AddWithValue("@paramMinValBudget", filter.Budget.Split('-')[1]);
                            }
                            if (!String.IsNullOrEmpty(filter.Mileage))
                            {
                                cmd.Parameters.AddWithValue("@paramMileageCategoryIds", filter.Mileage.Replace(' ', ','));
                            }
                            if (!String.IsNullOrEmpty(filter.Displacement))
                            {
                                cmd.Parameters.AddWithValue("@paramDisplacementFilterIds", filter.Displacement.Replace(' ', ','));
                            }
                            if (!String.IsNullOrEmpty(filter.RideStyle))
                            {
                                cmd.Parameters.AddWithValue("@paramRideStyleId", filter.RideStyle.Replace(' ', ','));
                            }
                            if (!String.IsNullOrEmpty(filter.AntiBreakingSystem))
                            {
                                cmd.Parameters.AddWithValue("@paramHasABS", Convert.ToBoolean(filter.AntiBreakingSystem));
                            }
                            if (!String.IsNullOrEmpty(filter.BrakeType))
                            {
                                cmd.Parameters.AddWithValue("@paramDrumDisc", Convert.ToBoolean(filter.BrakeType));
                            }
                            if (!String.IsNullOrEmpty(filter.AlloyWheel))
                            {
                                cmd.Parameters.AddWithValue("@paramSpokeAlloy", filter.AlloyWheel);
                            }
                            if (!String.IsNullOrEmpty(filter.StartType))
                            {
                                cmd.Parameters.AddWithValue("@paramHasElectric", Convert.ToBoolean(filter.StartType));
                            }
                            if (!String.IsNullOrEmpty(filter.sc))
                            {
                                cmd.Parameters.AddWithValue("@paramSortCategoryId", filter.sc);
                            }
                            if (!String.IsNullOrEmpty(filter.so))
                            {
                                cmd.Parameters.AddWithValue("@paramSortOrder", filter.so);
                            }
                        }
                        #endregion
                        using (SqlDataReader dr = db.SelectQry(cmd))
                        {
                            lstBikeBookingListingEntity = new List<BikeBookingListingEntity>();
                            if (dr != null && dr.HasRows)
                            {
                                #region Bike details
                                while (dr.Read())
                                {
                                    BikeBookingListingEntity objBikeBookingListingEntity = new BikeBookingListingEntity();
                                    objBikeBookingListingEntity.MakeEntity = new Entities.BikeData.BikeMakeEntityBase()
                                    {
                                        MakeId = Convert.ToInt32(dr["MakeId"]),
                                        MakeName = Convert.ToString(dr["MakeName"]),
                                        MaskingName = Convert.ToString(dr["MakeMaskingName"])
                                    };
                                    objBikeBookingListingEntity.ModelEntity = new Entities.BikeData.BikeModelEntityBase()
                                    {
                                        ModelId = Convert.ToInt32(dr["ModelId"]),
                                        ModelName = Convert.ToString(dr["ModelName"]),
                                        MaskingName = Convert.ToString(dr["ModelMaskingName"])
                                    };
                                    objBikeBookingListingEntity.VersionEntity = new Entities.BikeData.BikeVersionEntityBase()
                                    {
                                        VersionId = Convert.ToInt32(dr["VersionId"]),
                                        VersionName = Convert.ToString(dr["VersionName"])
                                    };
                                    objBikeBookingListingEntity.BikeName = Convert.ToString(dr["Bike"]);
                                    objBikeBookingListingEntity.ExShowroom = Convert.ToUInt32(dr["VersionPrice"]);
                                    objBikeBookingListingEntity.HostUrl = Convert.ToString(dr["HostUrl"]);
                                    objBikeBookingListingEntity.OriginalImagePath = Convert.ToString(dr["ImgPath"]);
                                    objBikeBookingListingEntity.DealerId = Convert.ToUInt32(dr["DealerId"]);
                                    objBikeBookingListingEntity.Displacement = Convert.ToSingle(dr["Displacement"]);
                                    objBikeBookingListingEntity.Mileage = Convert.ToUInt16(dr["Mileage"]);
                                    objBikeBookingListingEntity.HasABS = Convert.ToBoolean(dr["hasABS"]);
                                    objBikeBookingListingEntity.HasDisc = Convert.ToBoolean(dr["discDrum"]);
                                    objBikeBookingListingEntity.HasAlloyWheels = Convert.ToBoolean(dr["hasAlloyWheels"]);
                                    objBikeBookingListingEntity.HasElectricStart = Convert.ToBoolean(dr["hasElectricStart"]);
                                    objBikeBookingListingEntity.BookingAmount = Convert.ToUInt32(dr["BookingAmount"]);
                                    objBikeBookingListingEntity.PopularityIndex = Convert.ToUInt32(dr["PopularityIndex"]);

                                    lstBikeBookingListingEntity.Add(objBikeBookingListingEntity);
                                }
                                #endregion
                                #region Price Breakup
                                if (dr.NextResult())
                                {
                                    lstVersionPrice = new List<DealerPriceCategoryItemEntity>();
                                    lstPQList = new List<PQ_Price>();
                                    while (dr.Read())
                                    {
                                        lstVersionPrice.Add(
                                            new DealerPriceCategoryItemEntity()
                                            {
                                                DealerId = Convert.ToUInt32(dr["DealerId"]),
                                                ItemId = Convert.ToUInt32(dr["Id"]),
                                                ItemName = Convert.ToString(dr["ItemName"]),
                                                Price = Convert.ToInt32(dr["ItemValue"]),
                                                VersionId = Convert.ToUInt32(dr["VersionId"])
                                            }
                                            );
                                        lstPQList.Add(new PQ_Price()
                                        {
                                            DealerId = Convert.ToUInt32(dr["DealerId"]),
                                            CategoryName = Convert.ToString(dr["ItemName"]),
                                            CategoryId = Convert.ToUInt32(dr["Id"]),
                                            Price = Convert.ToUInt32(dr["ItemValue"]),
                                        });
                                    }
                                }
                                #endregion
                                #region Offer Count
                                if (dr.NextResult())
                                {
                                    while (dr.Read())
                                    {
                                        lstBookingOffer = new List<BookingOfferEntity>();
                                        lstBookingOffer.Add(
                                            new BookingOfferEntity()
                                            {
                                                ModelId = Convert.ToInt32(dr["ModelId"]),
                                                OfferCount = Convert.ToUInt16(dr["OfferCount"]),
                                                DealerId = Convert.ToInt32(dr["DealerId"])
                                            }
                                            );
                                    }
                                }
                                #endregion
                            }
                        }
                        if (lstBikeBookingListingEntity != null && lstBikeBookingListingEntity.Count > 0)
                        {
                            lstBikeBookingListingEntity.ForEach(
                                bike => bike.PriceList = (from price in lstVersionPrice
                                                          where price.VersionId == bike.VersionEntity.VersionId
                                                          && price.DealerId == bike.DealerId
                                                          select new PQ_Price()
                                                          {
                                                              CategoryId = price.ItemId,
                                                              CategoryName = price.ItemName,
                                                              DealerId = price.DealerId,
                                                              Price = Convert.ToUInt32(price.Price)
                                                          }).ToList()
                                );

                            lstBikeBookingListingEntity.ForEach(
                                bike => bike.OnRoadPrice = (from price in lstVersionPrice
                                                            where price.VersionId == bike.VersionEntity.VersionId
                                                            && price.DealerId == bike.DealerId
                                                            select price).Sum(m => m.Price)
                                );
                            lstBikeBookingListingEntity.ForEach(
                                bike => bike.lstOffer = FetchOffer(bike.DealerId, bike.ModelEntity.ModelId, cityId)
                                );
                            lstBikeBookingListingEntity.ForEach(bike => bike.Discount = OfferHelper.GetTotalDiscount(bike.lstOffer, bike.PriceList));
                            currentPage = (filter.PageNo == 0 ? 1 : filter.PageNo) - 1;
                            lstSearchResult = lstBikeBookingListingEntity.Skip(filter.PageSize * currentPage).Take(filter.PageSize);

                            totalCount = lstBikeBookingListingEntity.Count;
                            fetchedCount = lstSearchResult != null ? lstSearchResult.Count() : 0;
                            pageUrl = GetPrevNextUrl(filter, totalCount);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "BookingListingRepository.FetchBookingList");
                objErr.SendMail();
            }
            finally
            {
                db.CloseConnection();
            }
            return lstSearchResult;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dealerId"></param>
        /// <param name="modelId"></param>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public List<OfferEntity> FetchOffer(uint dealerId, int modelId, int cityId)
        {
            Database db = null;
            List<OfferEntity> lstOffer = null;
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "GetDealerOffers";
                    cmd.Parameters.AddWithValue("@DealerId", Convert.ToInt32(dealerId));
                    cmd.Parameters.AddWithValue("@ModelId", Convert.ToInt32(modelId));
                    cmd.Parameters.AddWithValue("@CityID", Convert.ToInt32(cityId));
                    db = new Database();
                    using (SqlDataReader dr = db.SelectQry(cmd))
                    {
                        if (dr != null && dr.HasRows)
                        {
                            lstOffer = new List<OfferEntity>();
                            while (dr.Read())
                            {
                                lstOffer.Add(new OfferEntity()
                                {
                                    OfferId = Convert.ToUInt32(dr["Id"]),
                                    OfferText = Convert.ToString(dr["OfferText"]),
                                    OfferCategoryId = Convert.ToUInt32(dr["OfferTypeId"]),
                                    OfferValue = Convert.ToUInt32(dr["OfferValue"]),
                                    IsPriceImpact = Convert.ToBoolean(dr["isPriceImpact"])
                                });
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "BookingListingRepository.FetchOffer");
                objErr.SendMail();
            }
            finally
            {
                db.CloseConnection();
            }
            return lstOffer;
        }

        private PagingUrl GetPrevNextUrl(BookingListingFilterEntity filterInputs,int totalRecordCount)
        {
            PagingUrl objPager = null;
            int totalPageCount = 0;
            try
            {
                objPager = new PagingUrl();
                string apiUrlStr = GetApiUrl(filterInputs);
                totalPageCount = Paging.GetTotalPages(totalRecordCount, Convert.ToInt32(filterInputs.PageSize));

                if (totalPageCount > 0)
                {
                    string controllerurl = "/api/BikeBookingListing/?";

                    if (filterInputs.PageNo == totalPageCount)
                        objPager.NextPageUrl = string.Empty;
                    else
                        objPager.NextPageUrl = controllerurl + "PageNo=" + (Convert.ToInt32(filterInputs.PageNo) + 1) + apiUrlStr;

                    if (filterInputs.PageNo == 1)
                        objPager.PrevPageUrl = string.Empty;
                    else
                        objPager.PrevPageUrl = controllerurl + "PageNo=" + (Convert.ToInt32(filterInputs.PageNo) - 1) + apiUrlStr;
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.DAL.NewBikeSearch.SearchResult.GetPrevNextUrl");
                objErr.SendMail();
            }
            return objPager;
        }

        private string GetApiUrl(BookingListingFilterEntity filterInputs)
        {
            string apiUrlstr = string.Empty;
            try
            {
                if (!String.IsNullOrEmpty(filterInputs.MakeIds))
                    apiUrlstr += "&MakeIds=" + filterInputs.MakeIds.Replace(" ", "+");
                if (!String.IsNullOrEmpty(filterInputs.BrakeType))
                    apiUrlstr += "&BrakeType=" + filterInputs.BrakeType.Replace(" ", "+");
                if (!String.IsNullOrEmpty(filterInputs.Budget))
                    apiUrlstr += "&Budget=" + filterInputs.Budget.Replace(" ", "+");
                if (!String.IsNullOrEmpty(filterInputs.Displacement))
                    apiUrlstr += "&Displacement=" + filterInputs.Displacement.Replace(" ", "+");
                if (!String.IsNullOrEmpty(filterInputs.Mileage))
                    apiUrlstr += "&Mileage=" + filterInputs.Mileage.Replace(" ", "+");
                if (filterInputs.PageSize > 0)
                    apiUrlstr += "&PageSize=" + filterInputs.PageSize;
                if (!String.IsNullOrEmpty(filterInputs.RideStyle))
                    apiUrlstr += "&RideStyle=" + filterInputs.RideStyle.Replace(" ", "+");
                if (!String.IsNullOrEmpty(filterInputs.sc))
                    apiUrlstr += "&sc=" + filterInputs.sc;
                if (!String.IsNullOrEmpty(filterInputs.so))
                    apiUrlstr += "&so=" + filterInputs.so;
                if (!String.IsNullOrEmpty(filterInputs.StartType))
                    apiUrlstr += "&StartType=" + filterInputs.StartType.Replace(" ", "+");
                if (!String.IsNullOrEmpty(filterInputs.AlloyWheel))
                    apiUrlstr += "&AlloyWheel=" + filterInputs.AlloyWheel.Replace(" ", "+");
                if (!String.IsNullOrEmpty(filterInputs.AntiBreakingSystem))
                    apiUrlstr += "&AntiBreakingSystem=" + filterInputs.AntiBreakingSystem.Replace(" ", "+");
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.DAL.BikeBooking.BookingListingRepository.GetApiUrl");
                objErr.SendMail();
            }
            return apiUrlstr;
        }
    }
}

using Bikewale.Entities.BikeBooking;
using Bikewale.Entities.PriceQuote;
using Bikewale.Interfaces.BikeBooking;
using Bikewale.Notifications;
using Bikewale.Utility;
using MySql.CoreDAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;

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
        /// Modified by : Ashutosh Sharma on 13 Apr 2018
        /// Description : Changed sp from 'getbookinglisting' to 'getbookinglisting_13042018' to remove filters based on specs.
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="areaId"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public IEnumerable<Entities.BikeBooking.BikeBookingListingEntity> FetchBookingList(int cityId, uint areaId, Entities.BikeBooking.BookingListingFilterEntity filter, out int totalCount, out int fetchedCount, out PagingUrl pageUrl)
        {
            IEnumerable<BikeBookingListingEntity> lstSearchResult = null;
            List<BikeBookingListingEntity> lstBikeBookingListingEntity = null;
            IList<DealerPriceCategoryItemEntity> lstVersionPrice = null;
            IList<PQ_Price> lstPQList = null;
            IList<BookingOfferEntity> lstBookingOffer = null;
            int currentPage = 0;
            totalCount = 0;
            fetchedCount = 0;
            pageUrl = null;
            string errStr = string.Empty;
            try
            {
                if (areaId > 0)
                {
                    using (DbCommand cmd = DbFactory.GetDBCommand())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "getbookinglisting_13042018";

                        cmd.Parameters.Add(DbFactory.GetDbParam("par_areaid", DbType.Int32, Convert.ToInt32(areaId)));

                        cmd.Parameters.Add(DbFactory.GetDbParam("par_parammakeids", DbType.String, 50, (!String.IsNullOrEmpty(filter.MakeIds)) ? filter.MakeIds.Replace(' ', ',') : Convert.DBNull));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_paramminvalbudget", DbType.String, 50, (!String.IsNullOrEmpty(filter.Budget)) ? filter.Budget.Split('-')[0] : Convert.DBNull));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_parammaxvalbudget", DbType.String, 50, (!String.IsNullOrEmpty(filter.Budget)) ? filter.Budget.Split('-')[1] : Convert.DBNull));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_paramridestyleid", DbType.String, 50, (!String.IsNullOrEmpty(filter.RideStyle)) ? filter.RideStyle.Replace(' ', ',') : Convert.DBNull));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_paramsortcategoryid", DbType.String, 50, (!String.IsNullOrEmpty(filter.sc)) ? filter.sc : Convert.DBNull));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_paramsortorder", DbType.String, 50, (!String.IsNullOrEmpty(filter.so)) ? filter.so : Convert.DBNull));

                        using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                        {
                            lstBikeBookingListingEntity = new List<BikeBookingListingEntity>();
                            if (dr != null)
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
                                dr.Close();
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
                            pageUrl = GetPrevNextUrl(filter, totalCount, fetchedCount);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "BookingListingRepository.FetchBookingList");
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
            List<OfferEntity> lstOffer = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "getdealeroffers";
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_dealerid", DbType.Int32, dealerId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_modelid", DbType.Int32, modelId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_cityid", DbType.Int32, cityId));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
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
                                    IsPriceImpact = Convert.ToBoolean(dr["isPriceImpact"]),
                                    IsOfferTerms = !Convert.IsDBNull(dr["IsOfferTerms"]) ? Convert.ToBoolean(dr["IsOfferTerms"]) : default(Boolean),
                                });
                            }
                            dr.Close();
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "BookingListingRepository.FetchOffer");
                
            }

            return lstOffer;
        }

        private PagingUrl GetPrevNextUrl(BookingListingFilterEntity filterInputs, int totalRecordCount, int fetchedCount)
        {
            PagingUrl objPager = null;
            int totalPageCount = 0;
            int currentPageNo = 0;
            try
            {
                objPager = new PagingUrl();
                string apiUrlStr = GetApiUrl(filterInputs);
                totalPageCount = Paging.GetTotalPages(totalRecordCount, Convert.ToInt32(filterInputs.PageSize));

                if (totalPageCount > 0)
                {
                    string controllerurl = "/api/BikeBookingListing/?";
                    currentPageNo = (filterInputs.PageNo == 0) ? 1 : filterInputs.PageNo;

                    if (currentPageNo == totalPageCount)
                    {
                        objPager.NextPageUrl = string.Empty;
                    }
                    else
                    {
                        objPager.NextPageUrl = controllerurl + "pageNo=" + (Convert.ToInt32(filterInputs.PageNo) + 1) + apiUrlStr;
                    }

                    if (filterInputs.PageNo == 1 || filterInputs.PageNo == 0)
                    {
                        objPager.PrevPageUrl = string.Empty;
                    }
                    else
                    {
                        objPager.PrevPageUrl = controllerurl + "pageNo=" + (Convert.ToInt32(filterInputs.PageNo) - 1) + apiUrlStr;
                    }

                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.DAL.NewBikeSearch.SearchResult.GetPrevNextUrl");
                
            }
            return objPager;
        }

        /// <summary>
        /// Modified by : Ashutosh Sharma on 13 Apr 2018
        /// Description : Removed specs from filters.
        /// </summary>
        /// <param name="filterInputs"></param>
        /// <returns></returns>
        private string GetApiUrl(BookingListingFilterEntity filterInputs)
        {
            string apiUrlstr = string.Empty;
            try
            {
                if (!String.IsNullOrEmpty(filterInputs.MakeIds))
                    apiUrlstr += "&makeIds=" + filterInputs.MakeIds.Replace(" ", "+");
                
                if (!String.IsNullOrEmpty(filterInputs.Budget))
                    apiUrlstr += "&budget=" + filterInputs.Budget.Replace(" ", "+");
                if (filterInputs.PageSize > 0)
                    apiUrlstr += "&pageSize=" + filterInputs.PageSize;
                if (!String.IsNullOrEmpty(filterInputs.RideStyle))
                    apiUrlstr += "&rideStyle=" + filterInputs.RideStyle.Replace(" ", "+");
                if (!String.IsNullOrEmpty(filterInputs.sc))
                    apiUrlstr += "&sc=" + filterInputs.sc;
                if (!String.IsNullOrEmpty(filterInputs.so))
                    apiUrlstr += "&so=" + filterInputs.so;
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.DAL.BikeBooking.BookingListingRepository.GetApiUrl");
                
            }
            return apiUrlstr;
        }
    }
}

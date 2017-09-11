﻿
using System;
using Bikewale.Common;
using Bikewale.Entities;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.DealerLocator;
using Bikewale.Entities.Location;
using Bikewale.Entities.PriceQuote;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Dealer;
using Bikewale.Interfaces.ServiceCenter;
using Bikewale.Memcache;
using Bikewale.Models.ServiceCenters;
using Bikewale.Utility;
using Bikewale.Entities.Schema;
using System.Collections.Generic;
using System.Linq;
using Bikewale.Entities.Models;

namespace Bikewale.Models
{
    /// <summary>
    /// Created By :- Subodh Jain 27 March 2017
    /// Summary :- To fetch data for dealer detail Page
    /// </summary>
    /// <returns></returns>
    public class DealerShowroomDealerDetail
    {
        private readonly IDealerCacheRepository _objDealerCache = null;
        private readonly IBikeMakesCacheRepository<int> _bikeMakesCache = null;
        private readonly IBikeModels<BikeModelEntity, int> _bikeModels = null;
        private readonly IServiceCenter _objSC = null;
        public uint cityId, makeId, dealerId, TopCount;
        public StatusCodes status;
        public MakeMaskingResponse objResponse;
        public BikeMakeEntityBase objMake;
        public CityEntityBase CityDetails;
        public DealerShowroomDealerDetailsVM objDealerDetails;

        /// <summary>
        /// Constructor 
        /// </summary>
        /// <param name="objSC"></param>
        /// <param name="objDealerCache"></param>
        /// <param name="bikeMakesCache"></param>
        /// <param name="bikeModels"></param>
        /// <param name="makeMaskingName"></param>
        /// <param name="dealerId"></param>
        public DealerShowroomDealerDetail(IServiceCenter objSC, IDealerCacheRepository objDealerCache, IBikeMakesCacheRepository<int> bikeMakesCache, IBikeModels<BikeModelEntity, int> bikeModels, string makeMaskingName, string cityMaskingName, uint dealerId, uint topCount)
        {
            _objDealerCache = objDealerCache;
            _bikeMakesCache = bikeMakesCache;
            _bikeModels = bikeModels;
            _objSC = objSC;
            TopCount = topCount;
            ProcessQuery(makeMaskingName, cityMaskingName, dealerId);
        }

        /// <summary>
        /// Created By :- Subodh Jain 27 March 2017
        /// Summary :- To fetch data for dealer detail Page
        /// Modified by : Aditi Srivastava on 24 Apr 2017
        /// Summary     : Added null check for dealer details before functiion calls
        /// </summary>
        /// <returns></returns>
        public DealerShowroomDealerDetailsVM GetData()
        {
            objDealerDetails = new DealerShowroomDealerDetailsVM();
            try
            {
                objMake = _bikeMakesCache.GetMakeDetails(makeId);
                objDealerDetails.DealerDetails = BindDealersData();
                if (objMake != null)
                    objDealerDetails.Make = objMake;
                if (objDealerDetails.DealerDetails != null && objDealerDetails.DealerDetails.DealerDetails != null)
                {
                    cityId = (uint)objDealerDetails.DealerDetails.DealerDetails.CityId;
                    CityDetails = new CityHelper().GetCityById(cityId);
                    objDealerDetails.CityDetails = CityDetails;

                    ProcessGlobalLocationCookie();
                    objDealerDetails.DealersList = BindOtherDealerWidget();

                    objDealerDetails.PopularBikes = BindMostPopularBikes();
                    objDealerDetails.ServiceCenterDetails = BindServiceCenterWidget();
                    BindPageMetas(objDealerDetails);
                    BindLeadCapture(objDealerDetails);
                }
            }
            catch (System.Exception ex)
            {

                ErrorClass objErr = new ErrorClass(ex, "DealerShowroomDealerDetail.GetData()");
            }


            return objDealerDetails;
        }

        /// <summary>
        /// Created by :- Subodh Jain 30 March 2017
        /// Summary :- Added lead popup
        /// </summary>
        /// <param name="objDealerDetails"></param>
        private void BindLeadCapture(DealerShowroomDealerDetailsVM objDealerDetails)
        {
            objDealerDetails.LeadCapture = new LeadCaptureEntity()
            {

                CityId = cityId,
                AreaId = objDealerDetails.DealerDetails.DealerDetails.Area.AreaId,
                Area = objDealerDetails.DealerDetails.DealerDetails.Area.AreaName,
                City = CityDetails.CityName

            };
            objDealerDetails.GALabel = string.Format("{0}_{1}_{2}", objDealerDetails.Make.MakeName, CityDetails.CityName, objDealerDetails.DealerDetails.DealerDetails.Area.AreaName);
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 19 Jan 2017
        /// Description :   Process Global Cookie
        /// </summary>
        private void ProcessGlobalLocationCookie()
        {
            GlobalCityAreaEntity location = GlobalCityArea.GetGlobalCityArea();
            uint customerCityId = location.CityId;
            uint customerAreaId = location.AreaId;
            if (customerCityId == cityId && customerAreaId > 0)
            {
                objDealerDetails.PQCityId = cityId;
                objDealerDetails.PQAreaID = customerAreaId;
                objDealerDetails.CustomerAreaName = location.Area.Replace('-', ' ');
                objDealerDetails.PQAreaName = objDealerDetails.CustomerAreaName;
            }
            else
            {
                objDealerDetails.PQCityId = cityId;
                objDealerDetails.PQAreaID = customerAreaId;
                if (objDealerDetails.DealerDetails != null && objDealerDetails.DealerDetails.DealerDetails != null && objDealerDetails.DealerDetails.DealerDetails.Area != null)
                    objDealerDetails.PQAreaName = objDealerDetails.DealerDetails.DealerDetails.Area.AreaName;
            }
        }

        /// <summary>
        /// Created By:- Subodh Jain 23 March 2017
        /// Summary:- Fetching data about dealers of other brands
        /// Modified by : Ashutosh Sharma on 23 Aug 2017
        /// Description : Page title and description changed.
        /// </summary>
        /// <returns></returns>
        private void BindPageMetas(DealerShowroomDealerDetailsVM objDealerDetails)
        {
            PageMetaTags objPage = objDealerDetails.PageMetaTags;
            try
            {
                objPage.Keywords = string.Format("{0}, {0} dealer, {0} Showroom, {0} {1}", objDealerDetails.DealerDetails.DealerDetails.Name, CityDetails.CityName);

                objPage.CanonicalUrl = String.Format("{0}/{1}-dealer-showrooms-in-{2}/{3}-{4}/", BWConfiguration.Instance.BwHostUrl, objDealerDetails.Make.MaskingName, objDealerDetails.CityDetails.CityMaskingName, objDealerDetails.DealerDetails.DealerDetails.DealerId, Bikewale.Utility.UrlFormatter.RemoveSpecialCharUrl(objDealerDetails.DealerDetails.DealerDetails.Name));
                objPage.AlternateUrl = String.Format("{0}/m/{1}-dealer-showrooms-in-{2}/{3}-{4}/", BWConfiguration.Instance.BwHostUrl, objDealerDetails.Make.MaskingName, objDealerDetails.CityDetails.CityMaskingName, objDealerDetails.DealerDetails.DealerDetails.DealerId, Bikewale.Utility.UrlFormatter.RemoveSpecialCharUrl(objDealerDetails.DealerDetails.DealerDetails.Name));
                
                if (objDealerDetails.DealerDetails.DealerDetails.Area != null && !string.IsNullOrEmpty(objDealerDetails.DealerDetails.DealerDetails.Area.AreaName))
                {
                    objPage.Title = string.Format("{0}, {1} - {2} | {3} showroom in {2} - BikeWale",
                                objDealerDetails.DealerDetails.DealerDetails.Name,
                                objDealerDetails.DealerDetails.DealerDetails.Area.AreaName,
                                CityDetails.CityName,
                                objDealerDetails.Make.MakeName);
                    objPage.Description = string.Format("{0}, {1} - {2} is an authorized {3} showroom in {2}. Get address, contact details direction, EMI quotes etc. of {0} {3} showroom.",
                               objDealerDetails.DealerDetails.DealerDetails.Name,
                               objDealerDetails.DealerDetails.DealerDetails.Area.AreaName,
                               CityDetails.CityName,
                               objMake.MakeName);
                }
                else
                {
                    objPage.Title = string.Format("{0} - {1} | {2} showroom in {1} - BikeWale",
                                objDealerDetails.DealerDetails.DealerDetails.Name,
                                CityDetails.CityName,
                                objDealerDetails.Make.MakeName);
                    objPage.Description = string.Format("{0} - {1} is an authorized {2} showroom in {1}. Get address, contact details direction, EMI quotes etc. of {0} {2} showroom.",
                               objDealerDetails.DealerDetails.DealerDetails.Name,
                               CityDetails.CityName,
                               objMake.MakeName);
                }

                List<BreadCrumb> BreadCrumbs = new List<BreadCrumb>();

                BreadCrumbs.Add(new BreadCrumb
                {
                    ListUrl = "/",
                    Name = "Home"
                });

                BreadCrumbs.Add(new BreadCrumb
                {
                    ListUrl = "/dealer-showroom-locator/",
                    Name = "Showroom Locator"
                });

                if (objDealerDetails.Make != null)
                {
                    BreadCrumbs.Add(new BreadCrumb
                    {
                        ListUrl = string.Format("/{0}-dealer-showrooms-in-india/", objDealerDetails.Make.MaskingName),
                        Name = objDealerDetails.Make.MakeName + " Showroom"
                    });
                }

                if (objDealerDetails.Make != null && objDealerDetails.CityDetails != null)
                {
                    BreadCrumbs.Add(new BreadCrumb
                    {
                        ListUrl =  string.Format("/{0}-dealer-showrooms-in-{1}", objDealerDetails.Make.MaskingName, objDealerDetails.CityDetails.CityMaskingName),
                        Name = string.Format("{0} Showroom in {1}", objDealerDetails.Make.MakeName, objDealerDetails.CityDetails.CityName)
                    });
                }

                objDealerDetails.BreadCrumbsList.Breadcrumbs = BreadCrumbs;

                if (objDealerDetails.DealerDetails != null && objDealerDetails.DealerDetails.DealerDetails != null)
                    objDealerDetails.BreadCrumbsList.PageName = objDealerDetails.DealerDetails.DealerDetails.Name;

                SetPageJSONLDSchema(objDealerDetails);
            }
            catch (System.Exception ex)
            {

                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, "DealerShowroomIndiaPage.BindPageMetas()");
            }
        }

        /// <summary>
        /// Created By  : Sushil Kumar on 25th Aug 2017
        /// Description : To load json schema for the dealer items
        /// </summary>
        /// <param name="objDealerDetails"></param>
        private void SetPageJSONLDSchema(DealerShowroomDealerDetailsVM objDealerDetails)
        {
            var dealerDetails = objDealerDetails.DealerDetails.DealerDetails;

            var objSchema = new MotorcycleDealer();
            objSchema.Name = dealerDetails.Name;
            objSchema.Email = dealerDetails.EMail;
            objSchema.Telephone = dealerDetails.MaskingNumber;
            objSchema.Address = new Address();
            objSchema.Address.StreetAddress = dealerDetails.Address;
            objSchema.Address.PinCode = dealerDetails.Pincode;
            objSchema.Address.City = dealerDetails.City;
            objSchema.ImageUrl = Image.GetPathToShowImages(null, null, ImageSize._310x174);
            objSchema.OpeningHours = new List<string>() { dealerDetails.WorkingHours };
            objSchema.PageUrl = objDealerDetails.PageMetaTags.CanonicalUrl;
            objSchema.Logo = "https://imgd.aeplcdn.com/0x0/bw/static/design15/mailer-images/bw-logo.png";
            objSchema.Description = objDealerDetails.PageMetaTags.Description;

            if (dealerDetails.Area != null)
            {
                objSchema.Address.State = dealerDetails.Area.AreaName;
                objSchema.Address.PinCode = dealerDetails.Area.PinCode;
                objSchema.Location = new GeoCoordinates();
                objSchema.Location.Latitude = dealerDetails.Area.Latitude;
                objSchema.Location.Longitude = dealerDetails.Area.Longitude;
                objSchema.AreaServed = dealerDetails.Area.AreaName;
                objSchema.GoogleMapUrl = string.Format("https://www.google.com/maps/place/{0},{1}", dealerDetails.Area.Latitude, dealerDetails.Area.Longitude);
            }

            if (objDealerDetails.DealerDetails.Models != null && objDealerDetails.DealerDetails.Models.Count() > 0)
            {
                var minPrice = objDealerDetails.DealerDetails.Models.Min(bike => bike.VersionPrice);
                var maxPrice = objDealerDetails.DealerDetails.Models.Max(bike => bike.VersionPrice);

                objSchema.PriceRange = string.Format("{0} - {1}", Format.FormatPrice(minPrice.ToString()), Format.FormatPrice(maxPrice.ToString()));
            }
            else
            {
                objSchema.PriceRange = "Not available";
            }

            if (objDealerDetails.DealerDetails.Models != null)
            {
                var firstModel = objDealerDetails.DealerDetails.Models.FirstOrDefault();
                if (firstModel != null)
                {
                    objSchema.ImageUrl = Image.GetPathToShowImages(firstModel.OriginalImagePath, firstModel.HostURL, ImageSize._310x174);
                }

            }


            objDealerDetails.PageMetaTags.SchemaJSON = Newtonsoft.Json.JsonConvert.SerializeObject(objSchema);
        }

        /// <summary>
        /// Created By :- Subodh Jain 27 March 2017
        /// Summary :- To fetch data for service center
        /// </summary>
        /// <returns></returns>
        private ServiceCenterDetailsWidgetVM BindServiceCenterWidget()
        {
            ServiceCenterDetailsWidgetVM ServiceCenterVM = null;
            try
            {
                ServiceCentersCard objServcieCenter = new ServiceCentersCard(_objSC, TopCount, (uint)objMake.MakeId, CityDetails.CityId);
                ServiceCenterVM = objServcieCenter.GetData();
            }
            catch (System.Exception ex)
            {

                ErrorClass objErr = new ErrorClass(ex, "DealerShowroomDealerDetail.BindServiceCenterWidget()");
            }

            return ServiceCenterVM;

        }

        /// <summary>
        /// Created By :- Subodh Jain 27 March 2017
        /// Summary :- To fetch data for most popular bikes
        /// </summary>
        /// <returns></returns>
        private MostPopularBikeWidgetVM BindMostPopularBikes()
        {
            MostPopularBikeWidgetVM objPopularBikes = new MostPopularBikeWidgetVM();
            try
            {
                MostPopularBikesWidget popularBikes = new MostPopularBikesWidget(_bikeModels, EnumBikeType.All, false, true, PQSourceEnum.Desktop_DealerLocator_Detail_AvailableModels, 0, (uint)objMake.MakeId);
                popularBikes.TopCount = 9;
                popularBikes.CityId = cityId;
                objPopularBikes = popularBikes.GetData();
                objPopularBikes.PageCatId = 5;
                objPopularBikes.PQSourceId = PQSourceEnum.Desktop_HP_MostPopular;
            }
            catch (System.Exception ex)
            {

                ErrorClass objErr = new ErrorClass(ex, "DealerShowroomDealerDetail.BindMostPopularBikes()");
            }
            return objPopularBikes;
        }

        /// <summary>
        /// Created By :- Subodh Jain 27 March 2017
        /// Summary :- To fetch data for Dealers details 
        /// </summary>
        /// <returns></returns>
        private DealerBikesEntity BindDealersData()
        {
            DealerBikesEntity objDealerDetails = null;
            try
            {
                objDealerDetails = _objDealerCache.GetDealerDetailsAndBikesByDealerAndMake(dealerId, (int)makeId);
            }
            catch (System.Exception ex)
            {

                ErrorClass objErr = new ErrorClass(ex, "DealerShowroomDealerDetail.BindDealersData()");
            }
            return objDealerDetails;

        }

        /// <summary>
        /// Created By :- Subodh Jain 27 March 2017
        /// Summary :- To fetch data for Other dealer widget
        /// </summary>
        /// <returns></returns>   
        private DealerCardVM BindOtherDealerWidget()
        {
            DealerCardVM objDealerList = null;
            try
            {

                DealerCardWidget objDealer = new DealerCardWidget(_objDealerCache, CityDetails.CityId, (uint)objMake.MakeId);
                objDealer.DealerId = dealerId;
                objDealer.TopCount = TopCount;
                objDealerList = objDealer.GetData();
            }
            catch (System.Exception ex)
            {

                ErrorClass objErr = new ErrorClass(ex, "DealerShowroomDealerDetail.BindOtherDealerWidget()");
            }
            return objDealerList;
        }

        /// <summary>
        /// Created By :- Subodh Jain 27 March 2017
        /// Summary :- To Processing query
        /// </summary>
        /// <returns></returns>
        private void ProcessQuery(string makeMaskingName, string cityMaskingName, uint dealerId)
        {
            objResponse = _bikeMakesCache.GetMakeMaskingResponse(makeMaskingName);
            if (objResponse != null)
            {
                if (objResponse.StatusCode == 200)
                {
                    makeId = objResponse.MakeId;
                    cityId = CitiMapping.GetCityId(cityMaskingName);
                    this.dealerId = dealerId;
                    status = StatusCodes.ContentFound;
                    if (cityId <= 0)
                        status = StatusCodes.ContentNotFound;
                }
                else if (objResponse.StatusCode == 301)
                {
                    status = StatusCodes.RedirectPermanent;
                }
                else
                {
                    status = StatusCodes.ContentNotFound;
                }
            }
            else
            {
                status = StatusCodes.ContentNotFound;
            }
        }

    }
}
﻿
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
                    BindPageMetas(objDealerDetails.PageMetaTags);
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
        /// </summary>
        /// <returns></returns>
        private void BindPageMetas(PageMetaTags objPage)
        {

            try
            {
                objPage.Keywords = string.Format("{0}, {0} dealer, {0} Showroom, {0} {1}", objDealerDetails.DealerDetails.DealerDetails.Name, CityDetails.CityName);
                objPage.Title = string.Format("{0} | {0} showroom in {1} - BikeWale", objMake.MakeName, CityDetails.CityName);
                objPage.Description = string.Format("{2} is an authorized {0} showroom in {1}. Get address, contact details direction, EMI quotes etc. of {2} {0} showroom.", objMake.MakeName, CityDetails.CityName, objDealerDetails.DealerDetails.DealerDetails.Name);

            }
            catch (System.Exception ex)
            {

                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, "DealerShowroomIndiaPage.BindPageMetas()");
            }
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
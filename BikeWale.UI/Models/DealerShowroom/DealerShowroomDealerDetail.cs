
using Bikewale.Common;
using Bikewale.Entities;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.DealerLocator;
using Bikewale.Entities.Location;
using Bikewale.Entities.PriceQuote;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Dealer;
using Bikewale.Interfaces.ServiceCenter;
using Bikewale.Models.ServiceCenters;
using Bikewale.Utility;
using System.Linq;
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
        private readonly IServiceCenter _objSC;
        public uint cityId, makeId, dealerId;
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
        public DealerShowroomDealerDetail(IServiceCenter objSC, IDealerCacheRepository objDealerCache, IBikeMakesCacheRepository<int> bikeMakesCache, IBikeModels<BikeModelEntity, int> bikeModels, string makeMaskingName, uint dealerId)
        {
            _objDealerCache = objDealerCache;
            _bikeMakesCache = bikeMakesCache;
            _bikeModels = bikeModels;
            _objSC = objSC;
            ProcessQuery(makeMaskingName, dealerId);
        }

        /// <summary>
        /// Created By :- Subodh Jain 27 March 2017
        /// Summary :- To fetch data for dealer detail Page
        /// </summary>
        /// <returns></returns>
        public DealerShowroomDealerDetailsVM GetData()
        {
            objDealerDetails = new DealerShowroomDealerDetailsVM();
            try
            {
                objMake = new MakeHelper().GetMakeNameByMakeId(makeId);
                if (objMake != null)
                    objDealerDetails.Make = objMake;
                cityId = GlobalCityArea.GetGlobalCityArea().CityId;
                if (cityId > 0)
                {
                    CityDetails = new CityHelper().GetCityById(cityId);
                    objDealerDetails.CityDetails = CityDetails;
                }
                objDealerDetails.DealersList = BindOtherDealerWidget();
                objDealerDetails.DealerDetails = BindDealersData();
                objDealerDetails.PopularBikes = BindMostPopularBikes();
                objDealerDetails.ServiceCenterDetails = BindServiceCenterWidget();
                BindPageMetas(objDealerDetails.PageMetaTags);
            }
            catch (System.Exception ex)
            {

                ErrorClass objErr = new ErrorClass(ex, "DealerShowroomDealerDetail.GetData()");
            }


            return objDealerDetails;
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
                objPage.Title = string.Format("{0}, {0} dealer, {0} Showroom, {0} {1}", objDealerDetails.DealerDetails.DealerDetails.Name, CityDetails.CityName);
                objPage.Keywords = string.Format("{0} bike dealers, {0} bike showrooms, {0} dealers, {0} showrooms, {0} dealerships, dealerships, test drive, {0} dealer contact number", objMake.MakeName);
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
                uint topCount = 9;
                ServiceCentersCard objServcieCenter = new ServiceCentersCard(_objSC, topCount, objMake, CityDetails);
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
                MostPopularBikesWidget popularBikes = new MostPopularBikesWidget(_bikeModels, EnumBikeType.All, true, false);
                popularBikes.TopCount = 9;
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
        private DealersEntity BindOtherDealerWidget()
        {
            DealersEntity objDealerList = null;
            try
            {
                int topCount = 3;
                objDealerList = _objDealerCache.GetDealerByMakeCity(cityId, makeId);

                objDealerList.Dealers = objDealerList.Dealers.Where(m => m.DealerId != dealerId);

                objDealerList.Dealers = objDealerList.Dealers.Take(topCount);
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
        private void ProcessQuery(string makeMaskingName, uint dealerId)
        {
            objResponse = _bikeMakesCache.GetMakeMaskingResponse(makeMaskingName);
            if (objResponse != null)
            {
                if (objResponse.StatusCode == 200)
                {
                    makeId = objResponse.MakeId;
                    this.dealerId = dealerId;
                    status = StatusCodes.ContentFound;
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
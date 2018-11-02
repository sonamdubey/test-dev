
using Bikewale.Entities;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Dealer;
using Bikewale.Entities.DealerLocator;
using Bikewale.Entities.Location;
using Bikewale.Entities.PriceQuote;
using Bikewale.Entities.Schema;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.BikeData.NewLaunched;
using Bikewale.Interfaces.BikeData.UpComing;
using Bikewale.Interfaces.Dealer;
using Bikewale.Interfaces.Location;
using Bikewale.Interfaces.Used;
using Bikewale.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
namespace Bikewale.Models
{
    /// <summary>
    /// Created By :- Subodh Jain 23 March 2017
    /// Summary :- Model For Dealer India Page
    /// </summary>
    public class DealerShowroomIndiaPage
    {
        private readonly IDealerCacheRepository _objDealerCache = null;
        private readonly IUpcoming _upcoming = null;
        private readonly IUsedBikeDetailsCacheRepository _objUsedCache = null;
        private readonly IStateCacheRepository _objStateCache = null;
        private readonly IBikeMakesCacheRepository _bikeMakesCache = null;
        private readonly INewBikeLaunchesBL _newLaunches = null;

        private uint makeId, cityId;
        public CityEntityBase cityDetails { get; set; }
        public StatusCodes status { get; set; }
        public MakeMaskingResponse objResponse { get; set; }
        public BikeMakeEntityBase objMake { get; set; }
        private DealerShowroomIndiaPageVM objDealerVM;
        public string RedirectUrl { get; set; }
        public bool IsMobile { get; set; }
        public ushort TopCount { get; internal set; }

        //Constructor
        public DealerShowroomIndiaPage(INewBikeLaunchesBL newLaunches, IDealerCacheRepository objDealerCache, IUpcoming upcoming, IUsedBikeDetailsCacheRepository objUsedCache, IStateCacheRepository objStateCache, IBikeMakesCacheRepository bikeMakesCache, string makeMaskingName)
        {
            _objDealerCache = objDealerCache;
            _upcoming = upcoming;
            _objUsedCache = objUsedCache;
            _objStateCache = objStateCache;
            _bikeMakesCache = bikeMakesCache;
            _newLaunches = newLaunches;
            ProcessQuery(makeMaskingName);
        }

        /// <summary>
        /// Created By :- Subodh Jain 23 March 2017
        /// Summary :- Dealer Locator view model data fetching
        /// </summary>
        /// <returns></returns>
        public DealerShowroomIndiaPageVM GetData()
        {
            objDealerVM = new DealerShowroomIndiaPageVM();
            try
            {

                objMake = _bikeMakesCache.GetMakeDetails(makeId);
                if (objMake != null)
                    objDealerVM.Make = objMake;

                objDealerVM.States = BindDealersMapCity();
                objDealerVM.DealerCount = objDealerVM.States.TotalDealers;
                objDealerVM.CitiesCount = objDealerVM.States.TotalCities;
                objDealerVM.AllDealers = BindOtherBrandWidget();
                objDealerVM.objUpcomingBikes = BindUpCompingBikesWidget();
                objDealerVM.UsedBikeModel = BindUsedBikeByModel();
                objDealerVM.NewLaunchedBikes = BindNewLaunchesBikes();
                BindPageMetas(objDealerVM);
                objDealerVM.Page = Entities.Pages.GAPages.Dealer_Locator_Country_Page;


            }
            catch (Exception ex)
            {

                Bikewale.Notifications.ErrorClass.LogError(ex, "DealerShowroomIndiaPage.GetData()");
            }
            return objDealerVM;
        }

        /// <summary>
        /// Created By:- Subodh Jain 23 March 2017
        /// Summary:- Fetching data about New Bike Launches
        /// </summary>
        /// <returns></returns>
        private NewLaunchedWidgetVM BindNewLaunchesBikes()
        {
            NewLaunchedWidgetVM NewLaunchedbikes = new NewLaunchedWidgetVM();
            try
            {
                NewLaunchedWidgetModel objNewLaunched = new NewLaunchedWidgetModel(9, _newLaunches);
                objNewLaunched.MakeId = makeId;
                NewLaunchedbikes.PQSourceId = (uint)PQSourceEnum.Mobile_DealerLocator_Landing_Check_on_road_price;
                NewLaunchedbikes = objNewLaunched.GetData();
            }
            catch (Exception ex)
            {

                Bikewale.Notifications.ErrorClass.LogError(ex, "DealerShowroomIndiaPage.BindNewLaunchesBikes()");
            }
            return NewLaunchedbikes;

        }

        /// <summary>
        /// Created By:- Subodh Jain 23 March 2017
        /// Summary:- Fetching data about dealers of other brands
        /// </summary>
        /// <returns></returns>
        private void BindPageMetas(DealerShowroomIndiaPageVM objPage)
        {

            try
            {
                objPage.PageMetaTags.Title = string.Format("{0} Bike Showrooms in India | {0} Bike Dealers in India - BikeWale", objMake.MakeName);
                objPage.PageMetaTags.Keywords = string.Format("{0} bike dealers, {0} bike showrooms, {0} dealers, {0} showrooms, {0} dealerships, dealerships, test drive, {0} dealer contact number", objMake.MakeName);
                objPage.PageMetaTags.Description = string.Format("Find the nearest {0} showroom in your city. There are {1} {0} showrooms in {2} cities in India. Get contact details, address, and direction of {0} dealers.", objMake.MakeName, objDealerVM.DealerCount, objDealerVM.CitiesCount);
                objPage.Page_H1 = objDealerVM.Make.MakeName + " Showrooms in India";

                SetBreadcrumList(objPage);
                SetPageJSONLDSchema(objPage);

            }
            catch (Exception ex)
            {

                Bikewale.Notifications.ErrorClass.LogError(ex, "DealerShowroomIndiaPage.BindPageMetas()");
            }
        }


        /// <summary>
        /// Created By  : Sushil Kumar on 14th Sep 2017
        /// Description : Added breadcrum and webpage schema
        /// </summary>
        private void SetPageJSONLDSchema(DealerShowroomIndiaPageVM objPageMeta)
        {
            //set webpage schema for the model page
            WebPage webpage = SchemaHelper.GetWebpageSchema(objPageMeta.PageMetaTags, objPageMeta.SchemaBreadcrumbList);

            if (webpage != null)
            {
                objPageMeta.PageMetaTags.SchemaJSON = SchemaHelper.JsonSerialize(webpage);
            }
        }

        /// <summary>
        /// Created By : Sushil Kumar on 12th Sep 2017
        /// Description : Function to create page level schema for breadcrum
        /// Modified by :Snehal Dange on 2th Nov 2017
        /// Description : Added makename in breadcrum
        /// Modified by : Snehal Dange on 27th Dec 2017
        /// Desc        : Added 'New Bikes' in breadcrumb
        /// </summary>
        private void SetBreadcrumList(DealerShowroomIndiaPageVM objPage)
        {

            try
            {
                if (objPage != null)
                {
                    IList<BreadcrumbListItem> BreadCrumbs = new List<BreadcrumbListItem>();
                    string url = string.Format("{0}/", Utility.BWConfiguration.Instance.BwHostUrl);
                    ushort position = 1;
                    if (IsMobile)
                    {
                        url += "m/";
                    }

                    BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position++, url, "Home"));
                    BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position++, string.Format("{0}new-bikes-in-india/", url), "New Bikes"));
                    if (objPage.Make != null)
                    {
                        BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position++, string.Format("{0}{1}-bikes/", url, objPage.Make.MaskingName), string.Format("{0} Bikes", objPage.Make.MakeName)));
                    }

                    BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position, null, objPage.Page_H1));


                    objPage.BreadcrumbList.BreadcrumListItem = BreadCrumbs;
                    objPage.SchemaBreadcrumbList.BreadcrumListItem = BreadCrumbs.Take(BreadCrumbs.Count - 1);
                }

            }
            catch (Exception ex)
            {

                Bikewale.Notifications.ErrorClass.LogError(ex, "DealerShowroomIndiaPage.SetBreadcrumList()");
            }

        }


        /// <summary>
        /// Created By :- Subodh Jain 25 March 2017
        /// Summary :- State wise dealers count in different cities
        /// </summary>
        /// <returns></returns>
        private DealerLocatorList BindDealersMapCity()
        {
            DealerLocatorList states = new DealerLocatorList();
            try
            {

                states = _objStateCache.GetDealerStatesCities(makeId);
            }
            catch (Exception ex)
            {

                Bikewale.Notifications.ErrorClass.LogError(ex, "DealerShowroomIndiaPage.BindDealersMapCity()");
            }
            return states;
        }

        /// <summary>
        /// Created By:- Subodh Jain 23 March 2017
        /// Summary:- Bind data for used bike widget
        /// </summary>
        /// <returns></returns>
        private UsedBikeModelsWidgetVM BindUsedBikeByModel()
        {
            UsedBikeModelsWidgetVM UsedBikeModel = new UsedBikeModelsWidgetVM();
            try
            {

                UsedBikeModelsWidgetModel objUsedBike = new UsedBikeModelsWidgetModel(TopCount, _objUsedCache);
                if (makeId > 0)
                    objUsedBike.makeId = makeId;

                UsedBikeModel = objUsedBike.GetData();
            }
            catch (Exception ex)
            {

                Bikewale.Notifications.ErrorClass.LogError(ex, "DealerShowroomIndiaPage.BindUsedBikeByModel()");
            }

            return UsedBikeModel;

        }

        /// <summary>
        /// Created By:- Subodh Jain 23 March 2017
        /// Summary:- Binding data for upcoming bike widget
        /// </summary>
        /// <returns></returns>
        private UpcomingBikesWidgetVM BindUpCompingBikesWidget()
        {
            UpcomingBikesWidgetVM objUpcomingBikes = null;
            try
            {
                UpcomingBikesWidget objUpcoming = new UpcomingBikesWidget(_upcoming);

                objUpcoming.Filters = new Bikewale.Entities.BikeData.UpcomingBikesListInputEntity()
                {
                    PageSize = 9,
                    PageNo = 1,
                    MakeId = (int)makeId
                };
                objUpcoming.SortBy = Bikewale.Entities.BikeData.EnumUpcomingBikesFilter.Default;
                objUpcomingBikes = objUpcoming.GetData();
            }
            catch (Exception ex)
            {

                Bikewale.Notifications.ErrorClass.LogError(ex, "DealerShowroomIndiaPage.BindUpCompingBikesWidget()");
            }
            return objUpcomingBikes;
        }

        /// <summary>
        /// Created By:- Subodh Jain 23 March 2017
        /// Summary:- Binding data for other dealer brands
        /// </summary>
        /// <returns></returns>
        private IEnumerable<DealerBrandEntity> BindOtherBrandWidget()
        {
            IEnumerable<DealerBrandEntity> AllDealerList = null;
            try
            {
                AllDealerList = _objDealerCache.GetDealerByBrandList();
                if (AllDealerList != null && AllDealerList.Any())
                {
                    AllDealerList = AllDealerList.Where(v => v.MakeId != makeId);
                }

            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass.LogError(ex, "DealerShowroomIndiaPage.BindOtherBrandWidget()");
            }
            return AllDealerList;
        }

        /// <summary>
        /// Created By:- Subodh Jain 23 March 2017
        /// Summary:- Process the input query
        /// </summary>
        /// <returns></returns>
        private void ProcessQuery(string makeMaskingName)
        {
            try
            {
                GlobalCityAreaEntity currentCityArea = GlobalCityArea.GetGlobalCityArea();
                cityId = currentCityArea.CityId;

                objResponse = _bikeMakesCache.GetMakeMaskingResponse(makeMaskingName);

                if (objResponse != null)
                {
                    if (objResponse.StatusCode == 200)
                    {
                        makeId = objResponse.MakeId;
                        var _cities = _objDealerCache.FetchDealerCitiesByMake(makeId);
                        if (_cities != null && _cities.Any())
                        {

                            var _city = _cities.FirstOrDefault(x => x.CityId == cityId);
                            if (_city != null)
                            {
                                RedirectUrl = String.Format("/dealer-showrooms/{0}/{1}/", makeMaskingName, _city.CityMaskingName);
                                status = StatusCodes.RedirectTemporary;
                            }
                        }
                        if (status == 0)
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
            catch (Exception ex)
            {

                Bikewale.Notifications.ErrorClass.LogError(ex, string.Format("DealerShowroomIndiaPage.ProcessQuery() makeMaskingName:{0}", makeMaskingName));
            }
        }

    }
}
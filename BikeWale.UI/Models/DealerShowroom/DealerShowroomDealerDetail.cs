
using Bikewale.BAL.ApiGateway.Adapters.BikeData;
using Bikewale.BAL.ApiGateway.ApiGatewayHelper;
using Bikewale.BAL.ApiGateway.Entities.BikeData;
using Bikewale.Common;
using Bikewale.Entities;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.DealerLocator;
using Bikewale.Entities.Location;
using Bikewale.Entities.Pages;
using Bikewale.Entities.PriceQuote;
using Bikewale.Entities.Schema;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Dealer;
using Bikewale.Interfaces.ServiceCenter;
using Bikewale.Memcache;
using Bikewale.Models.ServiceCenters;
using Bikewale.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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
        private readonly IBikeMakesCacheRepository _bikeMakesCache = null;
        private readonly IBikeModels<BikeModelEntity, int> _bikeModels = null;
        private readonly IServiceCenter _objSC = null;
        private readonly IApiGatewayCaller _apiGatewayCaller;
        private uint cityId, makeId, dealerId, TopCount;
        public StatusCodes status;
        public MakeMaskingResponse objResponse;
        public BikeMakeEntityBase objMake;
        public CityEntityBase CityDetails;
        public DealerShowroomDealerDetailsVM objDealerDetails = null;

        public bool IsMobile { get; internal set; }

        /// <summary>
        /// Constructor 
        /// </summary>
        /// <param name="objSC"></param>
        /// <param name="objDealerCache"></param>
        /// <param name="bikeMakesCache"></param>
        /// <param name="bikeModels"></param>
        /// <param name="makeMaskingName"></param>
        /// <param name="dealerId"></param>
        public DealerShowroomDealerDetail(IServiceCenter objSC, IDealerCacheRepository objDealerCache, IBikeMakesCacheRepository bikeMakesCache, IBikeModels<BikeModelEntity, int> bikeModels, string makeMaskingName, string cityMaskingName, uint dealerId, uint topCount, bool isMobile, IApiGatewayCaller apiGatewayCaller)
        {
            _objDealerCache = objDealerCache;
            _bikeMakesCache = bikeMakesCache;
            _bikeModels = bikeModels;
            _objSC = objSC;
            TopCount = topCount;
            IsMobile = isMobile;
            objDealerDetails = new DealerShowroomDealerDetailsVM();
            _apiGatewayCaller = apiGatewayCaller;
            ProcessQuery(makeMaskingName, cityMaskingName, dealerId);
        }

        /// <summary>
        /// Created By :- Subodh Jain 27 March 2017
        /// Summary :- To fetch data for dealer detail Page
        /// Modified by : Aditi Srivastava on 24 Apr 2017
        /// Summary     : Added null check for dealer details before functiion calls
        /// Modified by : Vivek Singh Tomar on 8th Sep 2017
        /// Summary     : Moved BindDealerData to Parse query string section
        /// </summary>
        /// <returns></returns>
        public DealerShowroomDealerDetailsVM GetData()
        {
            try
            {
                objMake = _bikeMakesCache.GetMakeDetails(makeId);

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
                    objDealerDetails.Page = GAPages.Dealer_Locator_Details_Page;
                }
            }
            catch (System.Exception ex)
            {

                ErrorClass.LogError(ex, "DealerShowroomDealerDetail.GetData()");
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
                AreaId = objDealerDetails.DealerDetails.DealerDetails.objArea.AreaId,
                Area = objDealerDetails.DealerDetails.DealerDetails.objArea.AreaName,
                City = CityDetails.CityName

            };
            objDealerDetails.GALabel = string.Format("{0}_{1}_{2}", objDealerDetails.Make.MakeName, CityDetails.CityName, objDealerDetails.DealerDetails.DealerDetails.objArea.AreaName);
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
                    objDealerDetails.PQAreaName = objDealerDetails.DealerDetails.DealerDetails.objArea.AreaName;
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

                objPage.CanonicalUrl = String.Format("{0}/dealer-showrooms/{1}/{2}/{3}-{4}/", BWConfiguration.Instance.BwHostUrl, objDealerDetails.Make.MaskingName, objDealerDetails.CityDetails.CityMaskingName, Bikewale.Utility.UrlFormatter.RemoveSpecialCharUrl(objDealerDetails.DealerDetails.DealerDetails.Name), objDealerDetails.DealerDetails.DealerDetails.DealerId);
                objPage.AlternateUrl = String.Format("{0}/m/dealer-showrooms/{1}/{2}/{3}-{4}/", BWConfiguration.Instance.BwHostUrl, objDealerDetails.Make.MaskingName, objDealerDetails.CityDetails.CityMaskingName, Bikewale.Utility.UrlFormatter.RemoveSpecialCharUrl(objDealerDetails.DealerDetails.DealerDetails.Name), objDealerDetails.DealerDetails.DealerDetails.DealerId);

                string dealerName = objDealerDetails.DealerDetails.DealerDetails.Name;

                if (objDealerDetails.DealerDetails.DealerDetails.Area != null && !string.IsNullOrEmpty(objDealerDetails.DealerDetails.DealerDetails.objArea.AreaName))
                {
                    dealerName = string.Format("{0},{1}", dealerName, objDealerDetails.DealerDetails.DealerDetails.objArea.AreaName);
                }


                objPage.Title = string.Format("{0} - {1} | {2} showroom in {1} - BikeWale", dealerName, CityDetails.CityName, objDealerDetails.Make.MakeName);
                objPage.Description = string.Format(@"{0} - {1} is an authorized {2} showroom in {1}. Get address, contact details direction, EMI quotes etc. of {0} {2} showroom.", objDealerDetails.DealerDetails.DealerDetails.Name, CityDetails.CityName, objMake.MakeName);

                objDealerDetails.Page_H1 = objDealerDetails.DealerDetails.DealerDetails.Name;


                SetBreadcrumList(objDealerDetails);
                SetPageJSONLDSchema(objDealerDetails);
            }
            catch (System.Exception ex)
            {

                ErrorClass.LogError(ex, "DealerShowroomIndiaPage.BindPageMetas()");
            }
        }


        /// <summary>
        /// Created By : Sushil Kumar on 12th Sep 2017
        /// Description : Function to create page level schema for breadcrum
        /// Modified by : Snehal Dange on 27th Dec 2017
        /// Desc        : Added 'New Bikes' in breadcrumb
        /// </summary>
        private void SetBreadcrumList(DealerShowroomDealerDetailsVM objPage)
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
            BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position++, string.Format("{0}{1}", url, "dealer-showrooms/"), "Showroom Locator"));

            if (objDealerDetails != null && objDealerDetails.Make != null)
            {
                BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position++, string.Format("{0}dealer-showrooms/{1}/", url, objDealerDetails.Make.MaskingName), objDealerDetails.Make.MakeName + " Showrooms"));

                if (objDealerDetails.CityDetails != null)
                {
                    BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position++, string.Format("{0}dealer-showrooms/{1}/{2}/", url, objDealerDetails.Make.MaskingName, objDealerDetails.CityDetails.CityMaskingName), string.Format("{0} Showroom in {1}", objDealerDetails.Make.MakeName, objDealerDetails.CityDetails.CityName)));
                }

            }

            BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position++, null, objPage.Page_H1));

            objPage.BreadcrumbList.BreadcrumListItem = BreadCrumbs;

        }

        /// <summary>
        /// Created By  : Sushil Kumar on 25th Aug 2017
        /// Description : To load json schema for the dealer items
        /// </summary>
        /// <param name="objDealerDetails"></param>
        private void SetPageJSONLDSchema(DealerShowroomDealerDetailsVM objDealerDetails)
        {
            //set webpage schema for the model page
            WebPage webpage = SchemaHelper.GetWebpageSchema(objDealerDetails.PageMetaTags, objDealerDetails.BreadcrumbList);

            if (webpage != null)
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
                    objSchema.Address.State = dealerDetails.objArea.AreaName;
                    objSchema.Address.PinCode = dealerDetails.objArea.PinCode;
                    objSchema.Location = new GeoCoordinates();
                    objSchema.Location.Latitude = dealerDetails.objArea.Latitude;
                    objSchema.Location.Longitude = dealerDetails.objArea.Longitude;
                    objSchema.AreaServed = dealerDetails.objArea.AreaName;
                    objSchema.GoogleMapUrl = string.Format("https://www.google.com/maps/place/{0},{1}", dealerDetails.objArea.Latitude, dealerDetails.objArea.Longitude);
                }

                if (objDealerDetails.DealerDetails.Models != null && objDealerDetails.DealerDetails.Models.Any())
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

                objDealerDetails.PageMetaTags.SchemaJSON = SchemaHelper.JsonSerialize(webpage);
                objDealerDetails.PageMetaTags.PageSchemaJSON = SchemaHelper.JsonSerialize(objSchema);
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

                ErrorClass.LogError(ex, "DealerShowroomDealerDetail.BindServiceCenterWidget()");
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

                ErrorClass.LogError(ex, "DealerShowroomDealerDetail.BindMostPopularBikes()");
            }
            return objPopularBikes;
        }

        /// <summary>
        /// Created By :- Subodh Jain 27 March 2017
        /// Summary :- To fetch data for Dealers details 
        /// Modified by : Ashutosh Sharma on 03 Apr 2018.
        /// Description : Calling Specs Features service to fetch min specs for bike models available with dealer.
        /// </summary>
        /// <returns></returns>
        private DealerBikesEntity BindDealersData()
        {
            DealerBikesEntity objDealerDetails = null;
            try
            {
                objDealerDetails = _objDealerCache.GetDealerDetailsAndBikesByDealerAndMake(dealerId, (int)makeId);
                if (objDealerDetails != null && objDealerDetails.Models != null)
                {
                    GetVersionSpecsByItemIdAdapter adapt1 = new GetVersionSpecsByItemIdAdapter();
                    VersionsDataByItemIds_Input specItemInput = new VersionsDataByItemIds_Input
                    {
                        Versions = objDealerDetails.Models.Select(m => m.objVersion.VersionId),
                        Items = new List<EnumSpecsFeaturesItems>
                        {
                            EnumSpecsFeaturesItems.Displacement,
                            EnumSpecsFeaturesItems.FuelEfficiencyOverall,
                            EnumSpecsFeaturesItems.MaxPowerBhp,
                            EnumSpecsFeaturesItems.KerbWeight
                        }
                    };
                    adapt1.AddApiGatewayCall(_apiGatewayCaller, specItemInput);
                    _apiGatewayCaller.Call();
                    var specsList = adapt1.Output;
                    if (specsList != null)
                    {
                        var specsEnumerator = specsList.GetEnumerator();
                        var bikesEnumerator = objDealerDetails.Models.GetEnumerator();
                        while (bikesEnumerator.MoveNext() && specsEnumerator.MoveNext())
                        {
                            bikesEnumerator.Current.MinSpecsList = specsEnumerator.Current.MinSpecsList;
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {

                ErrorClass.LogError(ex, "DealerShowroomDealerDetail.BindDealersData()");
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

                ErrorClass.LogError(ex, "DealerShowroomDealerDetail.BindOtherDealerWidget()");
            }
            return objDealerList;
        }

        /// <summary>
        /// Created By :- Subodh Jain 27 March 2017
        /// Summary :- To Processing query
        /// Created by : Vivek Singh Tomar 8th Sep 2017
        /// Summary    : Added processing of dealer Id as to identify if it's featured or not
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
                    objDealerDetails.RedirectUrl = HttpContext.Current.Request.RawUrl.Replace(makeMaskingName, objResponse.MaskingName);
                }
                else
                {
                    status = StatusCodes.ContentNotFound;
                }

                if (status.Equals(StatusCodes.ContentFound) && dealerId > 0)
                {
                    objDealerDetails.DealerDetails = BindDealersData();
                    if (objDealerDetails.DealerDetails != null && (objDealerDetails.DealerDetails.DealerDetails == null || !objDealerDetails.DealerDetails.DealerDetails.IsFeatured))
                    {
                        status = StatusCodes.RedirectPermanent;
                        objDealerDetails.RedirectUrl = string.Format("{0}/{1}dealer-showrooms/{2}/{3}/", BWConfiguration.Instance.BwHostUrl, IsMobile ? "m/" : "", makeMaskingName, cityMaskingName);
                    }
                }
            }
            else
            {
                status = StatusCodes.ContentNotFound;
            }
        }

    }
}
using Bikewale.BAL.BikeData;
using Bikewale.BAL.Pager;
using Bikewale.BindViewModels.Controls;
using Bikewale.Cache.BikeData;
using Bikewale.Cache.Core;
using Bikewale.Cache.DealersLocator;
using Bikewale.Cache.Location;
using Bikewale.Common;
using Bikewale.Controls;
using Bikewale.DAL.BikeData;
using Bikewale.DAL.Dealer;
using Bikewale.DAL.Location;
using Bikewale.DAL.PriceQuote;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.DealerLocator;
using Bikewale.Entities.GenericBikes;
using Bikewale.Entities.Location;
using Bikewale.Entities.PriceQuote;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.Dealer;
using Bikewale.Interfaces.Location;
using Bikewale.Interfaces.Pager;
using Bikewale.Interfaces.PriceQuote;
using Bikewale.Utility;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace Bikewale.New
{
    /// <summary>
    /// Created By : Ashish G. Kamble on 23 May 2016
    /// Modified By : Sushil Kumar on 2nd June 2016
    /// Description :  Added and Linked LeadCapture Widget
    ///                Added  PQSourceId for DealerCard Widget
    /// Modified By : Sushil Kumar on 17th Jan 2016
    /// Description : Added chnage location prompt widget        
    /// </summary>
    public class ModelPricesInCity : System.Web.UI.Page
    {
        protected ModelPriceInNearestCities ctrlTopCityPrices;
        protected DealerCard ctrlDealers;
        public BikeQuotationEntity firstVersion;
        protected NewAlternativeBikes ctrlAlternativeBikes;
        protected LeadCaptureControl ctrlLeadCapture;
        protected ServiceCenterCard ctrlServiceCenterCard;
        public Repeater rprVersionPrices, rpVersioNames;
        protected uint modelId = 0, cityId = 0, versionId, makeId, dealerCount;
        public int versionCount, colourCount;
        public string makeName = string.Empty, makeMaskingName = string.Empty, modelName = string.Empty, modelMaskingName = string.Empty, bikeName = string.Empty, modelImage = string.Empty, cityName = string.Empty, cityMaskingName = string.Empty, pageDescription = string.Empty;
        string redirectUrl = string.Empty;
        private bool redirectToPageNotFound = false, redirectPermanent = false;
        protected bool isAreaAvailable, isAreaSelected, isDiscontinued;
        protected String clientIP = CommonOpn.GetClientIP();
        protected BikeRankingEntity bikeRankObj;
        protected string styleName = string.Empty, rankText = string.Empty, bikeType = string.Empty;
        protected ChangeLocationPopup ctrlChangeLocation;
        protected GenericBikeInfoControl ctrlGenericBikeInfo;

        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }

        /// <summary>
        /// Modified by :   Sumit Kate on 17 Jun 2016
        /// Description :   Pass ModelId to get the dealers for Price in city page
        /// Modified By :-Subodh Jain on 1 Dec 2016
        /// Summary :- Added Service center Widget
        /// Modified By :-Subodh Jain on 16 Dec 2016
        /// Summary :- Added heading to dealer widget
        /// Modified By :-Aditi Srivastava on 13 Jan 2017
        /// Summary :- Added generic bike listing slug
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            // Modified By :Lucky Rathore on 12 July 2016.
            Form.Action = Request.RawUrl;
            string originalUrl = Request.ServerVariables["HTTP_X_ORIGINAL_URL"];
            if (String.IsNullOrEmpty(originalUrl))
                originalUrl = Request.ServerVariables["URL"];

            DeviceDetection dd = new DeviceDetection(originalUrl);
            dd.DetectDevice();

            ParseQueryString();
            CheckLocationCookie();
            if (redirectToPageNotFound || redirectPermanent)
            {
                DoPageNotFounRedirection();
            }
            else
            {
                FetchVersionPrices();
                BindControl();
                BindDealers();
                ColorCount();
                BindDescription();
                GetBikeRankingCategory(modelId);
            }
        }

        /// <summary>
        /// Created by:-Subodh Jain 26 Dec 2016
        /// Summary:- Bind Controls
        /// Modified By : Sushil Kumar on 17th Jan 2016
        /// Description : Added chnage location prompt widget
        ///  Modified  By :- Sajal Gupta on 13 Feb 2017
        /// Summary :- BikeInfo Slug details
        /// Modified By :-Subodh Jain on 15 March 2017
        /// Summary :Removed Used Bike widget
        /// </summary>
        private void BindControl()
        {
            ctrlTopCityPrices.ModelId = modelId;
            ctrlTopCityPrices.CityId = cityId;
            ctrlTopCityPrices.IsDiscontinued = isDiscontinued;
            ctrlTopCityPrices.TopCount = 8;
            ctrlTopCityPrices.cityName = cityName;
            ctrlDealers.MakeId = makeId;
            ctrlDealers.CityId = cityId;
            ctrlDealers.IsDiscontinued = isDiscontinued;
            ctrlDealers.TopCount = 3;
            ctrlDealers.ModelId = modelId;
            ctrlDealers.PQSourceId = (int)PQSourceEnum.Desktop_PriceInCity_DealersCard_GetOfferButton;
            ctrlDealers.ModelId = modelId;
            ctrlDealers.pageName = "Price_In_City_Page";
            ctrlDealers.widgetHeading = string.Format("{0} showrooms in {1}", makeName, cityName);

            ctrlLeadCapture.CityId = cityId;
            ctrlLeadCapture.ModelId = modelId;
            ctrlLeadCapture.AreaId = 0;



            ctrlServiceCenterCard.MakeId = makeId;
            ctrlServiceCenterCard.CityId = cityId;
            ctrlServiceCenterCard.makeName = makeName;
            ctrlServiceCenterCard.cityName = cityName;
            ctrlServiceCenterCard.makeMaskingName = makeMaskingName;
            ctrlServiceCenterCard.cityMaskingName = cityMaskingName;
            ctrlServiceCenterCard.TopCount = 3;
            ctrlServiceCenterCard.widgetHeading = string.Format("{0} service centers in {1}", makeName, cityName);

            ctrlAlternativeBikes.TopCount = 9;
            ctrlAlternativeBikes.PQSourceId = (int)PQSourceEnum.Desktop_PriceInCity_Alternative;
            ctrlAlternativeBikes.WidgetTitle = bikeName;
            ctrlAlternativeBikes.model = modelName;
            ctrlAlternativeBikes.cityId = cityId;
            ctrlAlternativeBikes.cityName = cityName;
            if (firstVersion != null)
                ctrlAlternativeBikes.VersionId = firstVersion.VersionId;

            if (ctrlChangeLocation != null)
            {
                ctrlChangeLocation.UrlCityId = cityId;
                ctrlChangeLocation.UrlCityName = cityName;
            }

            if (ctrlGenericBikeInfo != null)
            {
                ctrlGenericBikeInfo.ModelId = modelId;
                ctrlGenericBikeInfo.CityId = cityId;
                ctrlGenericBikeInfo.PageId = BikeInfoTabType.PriceInCity;
                ctrlGenericBikeInfo.TabCount = 4;
            }

        }
        /// <summary>
        /// Created by : Aditi Srivastava on 13 Jan 2017
        /// Description: To get model ranking details
        /// </summary>
        /// <param name="modelId"></param>
        private void GetBikeRankingCategory(uint modelId)
        {
            BindGenericBikeRankingControl bikeRankingSlug = new BindGenericBikeRankingControl();
            bikeRankingSlug.ModelId = modelId;
            bikeRankObj = bikeRankingSlug.GetBikeRankingByModel();
            if (bikeRankObj != null)
            {
                styleName = bikeRankingSlug.StyleName;
                rankText = bikeRankingSlug.RankText;
                bikeType = bikeRankingSlug.BikeType;
            }
        }
        /// <summary>
        /// Created By Subodh Jain 18 oct 2016
        /// Desc:- for Bind descri[tion
        /// </summary>
        public void BindDescription()
        {
            char multiVersion = '\0', multiDealer = '\0';

            if (versionCount > 1)
                multiVersion = 's';

            if (dealerCount > 1)
                multiDealer = 's';

            string multiColour = ".";

            if (colourCount > 1)
                multiColour = string.Format(" and {0} colors.", colourCount);
            else if (colourCount == 1)
                multiColour = string.Format(" and 1 colour.");

            if (firstVersion != null)
            {
                string newBikeDescription = string.Format("{0} {1} on-road price in {2} - Rs. {3} onwards. It is available in {4} version{5}{6}", makeName, modelName, cityName, CommonOpn.FormatPrice(firstVersion.OnRoadPrice.ToString()), versionCount, multiVersion, multiColour);

                if (dealerCount > 0)
                    newBikeDescription = string.Format("{0} {1} is sold by {2} dealership{3} in {4}.", newBikeDescription, modelName, dealerCount, multiDealer, cityName);

                newBikeDescription = string.Format("{0} All the colour options and versions of {1} might not be available at all the dealerships in {2}. Click on a {1} version name to know on-road price in {2}.", newBikeDescription, modelName, cityName);

                string discontinuedDescription = string.Format("The last known ex-showroom price of {0} {1} in {2} was Rs. {3} onwards. This bike has now been discontinued. It was available in {4} version{5}{6} Click on a {1} version name to know the last known ex-showroom price in {2}.", makeName, modelName, cityName, CommonOpn.FormatPrice(firstVersion.OnRoadPrice.ToString()), versionCount, multiVersion, multiColour);

                if (!isDiscontinued)
                    pageDescription = newBikeDescription;
                else
                    pageDescription = discontinuedDescription;
            }
        }
        /// <summary>
        /// Created By Subodh Jain 10 oct 2016
        /// Desc:- for count of Colors in bike model
        /// </summary>
        protected void ColorCount()
        {
            if (modelId > 0)
            {
                IEnumerable<NewBikeModelColor> objModelColours = null;
                try
                {
                    using (IUnityContainer container = new UnityContainer())
                    {

                        container.RegisterType<IPager, Pager>()
                            .RegisterType<IBikeMakesCacheRepository<int>, BikeMakesCacheRepository<BikeMakeEntity, int>>()
                                .RegisterType<IBikeModelsCacheRepository<int>, BikeModelsCacheRepository<BikeModelEntity, int>>()
                                .RegisterType<IBikeMakes<BikeMakeEntity, int>, BikeMakesRepository<BikeMakeEntity, int>>()
                                .RegisterType<IBikeModels<BikeModelEntity, int>, BikeModels<BikeModelEntity, int>>()
                                .RegisterType<IBikeModelsRepository<BikeModelEntity, int>, BikeModelsRepository<BikeModelEntity, int>>()
                                .RegisterType<ICacheManager, MemcacheManager>();
                        var objModelCache = container.Resolve<IBikeModelsCacheRepository<int>>();
                        objModelColours = objModelCache.GetModelColor(Convert.ToInt16(modelId));
                        colourCount = objModelColours.Count();

                    }
                }
                catch (Exception err)
                {
                    ErrorClass objErr = new ErrorClass(err, "ModelPricesInCity.ColorCount");

                }
            }

        }
        /// <summary>
        /// Created By Subodh Jain 10 oct 2016
        /// Desc:- for count of dealers
        /// </summary>
        protected void BindDealers()
        {
            DealersEntity dealers = null;
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IDealerCacheRepository, DealerCacheRepository>()
                             .RegisterType<ICacheManager, MemcacheManager>()
                             .RegisterType<IDealerRepository, DealersRepository>()
                            ;
                    var objCache = container.Resolve<IDealerCacheRepository>();
                    dealers = objCache.GetDealerByMakeCity(cityId, makeId);

                    if (dealers != null)
                    {
                        dealerCount = dealers.TotalCount;
                    }
                }
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, "ModelPricesInCity.BindDealers");
                objErr.SendMail();
            }
        }
        /// <summary>
        /// Author : Created by Sangram Nandkhile on 25 May 2016
        /// Summary: Fetch version Prices according to model and city
        /// Modified By : Sushil Kumar on 8th June 2016
        /// Description : Added check for isdicontinued bikes and remove discontinued version if model is new
        /// </summary>
        private void FetchVersionPrices()
        {
            try
            {
                IPriceQuote objPQ = null;
                bool hasArea;
                using (IUnityContainer objPQCont = new UnityContainer())
                {
                    objPQCont.RegisterType<IPriceQuote, PriceQuoteRepository>();
                    objPQ = objPQCont.Resolve<IPriceQuote>();
                    IEnumerable<BikeQuotationEntity> bikePrices = objPQ.GetVersionPricesByModelId(modelId, cityId, out hasArea);
                    isAreaAvailable = hasArea;
                    if (bikePrices != null && bikePrices.Count() != 0)
                    {
                        isDiscontinued = !bikePrices.FirstOrDefault().IsModelNew;

                        if (!isDiscontinued)
                        {
                            bikePrices = from bike in bikePrices
                                         where bike.IsVersionNew == true
                                         select bike;
                        }

                        SetModelDetails(bikePrices);

                        rprVersionPrices.DataSource = bikePrices;
                        rprVersionPrices.DataBind();
                        rpVersioNames.DataSource = bikePrices;
                        rpVersioNames.DataBind();


                    }
                    else
                    {
                        redirectToPageNotFound = true;
                        DoPageNotFounRedirection();
                    }
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, Request.ServerVariables["URL"] + "-FetchVersionPrices");
                objErr.SendMail();
            }

        }

        /// <summary>
        /// Sets model details
        /// </summary>
        /// <param name="bikePrices"></param>
        private void SetModelDetails(IEnumerable<BikeQuotationEntity> bikePrices)
        {
            try
            {
                versionCount = bikePrices.Count();
                if (versionCount > 0)
                {
                    firstVersion = bikePrices.FirstOrDefault();
                    if (firstVersion != null)
                    {
                        makeName = firstVersion.MakeName;
                        makeMaskingName = firstVersion.MakeMaskingName;
                        modelName = firstVersion.ModelName;
                        modelMaskingName = firstVersion.ModelMaskingName;
                        cityMaskingName = firstVersion.CityMaskingName;
                        bikeName = String.Format("{0} {1}", makeName, modelName);
                        modelImage = Utility.Image.GetPathToShowImages(firstVersion.OriginalImage, firstVersion.HostUrl, Bikewale.Utility.ImageSize._310x174);
                        cityName = firstVersion.City;
                        versionId = firstVersion.VersionId;
                        makeId = firstVersion.MakeId;
                    }
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, Request.ServerVariables["URL"] + "-SetModelDetails");
                objErr.SendMail();
            }
        }

        /// <summary>
        /// Function to do the redirection on different pages.
        /// </summary>
        private void DoPageNotFounRedirection()
        {
            // Redirection
            if (redirectToPageNotFound)
            {
                Response.Redirect(CommonOpn.AppPath + "pageNotFound.aspx", false);
            }
            else if (redirectPermanent)
                CommonOpn.RedirectPermanent(redirectUrl);
        }

        /// <summary>
        /// Function to get parameters from the query string.
        /// </summary>
        private void ParseQueryString()
        {
            ModelMaskingResponse objModelResponse = null;
            CityMaskingResponse objCityResponse = null;
            string model = string.Empty, city = string.Empty, _make = string.Empty;
            try
            {
                model = Request.QueryString["model"];
                city = Request.QueryString["city"];

                if (!string.IsNullOrEmpty(city))
                {
                    using (IUnityContainer container = new UnityContainer())
                    {
                        container.RegisterType<ICityMaskingCacheRepository, CityMaskingCache>()
                                 .RegisterType<ICacheManager, MemcacheManager>()
                                 .RegisterType<ICity, CityRepository>()
                                ;
                        var objCache = container.Resolve<ICityMaskingCacheRepository>();
                        objCityResponse = objCache.GetCityMaskingResponse(city);
                    }
                }

                if (!string.IsNullOrEmpty(model))
                {
                    using (IUnityContainer container = new UnityContainer())
                    {
                        container.RegisterType<IBikeMaskingCacheRepository<BikeModelEntity, int>, BikeModelMaskingCache<BikeModelEntity, int>>()
                                 .RegisterType<ICacheManager, MemcacheManager>()
                                 .RegisterType<IBikeModelsRepository<BikeModelEntity, int>, BikeModelsRepository<BikeModelEntity, int>>()
                                ;
                        var objCache = container.Resolve<IBikeMaskingCacheRepository<BikeModelEntity, int>>();
                        objModelResponse = objCache.GetModelMaskingResponse(model);
                    }
                }

            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, Request.ServerVariables["URL"] + "ParseQueryString");
                objErr.SendMail();

                Response.Redirect("/customerror.aspx", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
                this.Page.Visible = false;
            }
            finally
            {
                if (objCityResponse != null && objModelResponse != null)
                {
                    // Get cityId
                    // Code to check whether masking name is changed or not. If changed redirect to appropriate url
                    if (objCityResponse.StatusCode == 200)
                    {
                        cityId = objCityResponse.CityId;
                    }
                    else if (objCityResponse.StatusCode == 301)
                    {
                        //redirect permanent to new page                         
                        redirectUrl = Request.RawUrl.Replace(city, objCityResponse.MaskingName);
                        redirectPermanent = true;
                    }
                    else
                    {
                        redirectToPageNotFound = true;
                    }

                    // Get ModelId
                    // Code to check whether masking name is changed or not. If changed redirect to appropriate url
                    if (objModelResponse.StatusCode == 200)
                    {
                        modelId = objModelResponse.ModelId;
                    }
                    else if (objModelResponse.StatusCode == 301)
                    {
                        //redirect permanent to new page                         
                        redirectUrl = Request.RawUrl.Replace(model, objModelResponse.MaskingName);
                        redirectPermanent = true;
                    }
                    else
                    {
                        redirectToPageNotFound = true;
                    }
                }
                else
                {
                    redirectToPageNotFound = true;
                }

            }
        }

        /// <summary>
        /// Returns City Masking Name from City Id
        ///Modified By :Subodh Jain 26 Dec 2016
        ///Summary:- Added cache layer for city masking name retrival
        /// </summary>
        /// <param name="cityId">city id</param>
        /// <returns></returns>
        public uint GetCityMaskingName(string maskingName)
        {
            IEnumerable<CityEntityBase> objCityList = null;
            uint _cityId = 0;
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<ICity, CityRepository>();
                    container.RegisterType<ICacheManager, MemcacheManager>(); ;
                    container.RegisterType<ICityCacheRepository, CityCacheRepository>();
                    ICityCacheRepository cityCacheRepository = container.Resolve<ICityCacheRepository>();
                    objCityList = cityCacheRepository.GetAllCities(EnumBikeType.All);

                    CityEntityBase SelectedCity = objCityList.FirstOrDefault(c => c.CityMaskingName == maskingName);
                    if (SelectedCity != null)
                    {
                        _cityId = SelectedCity.CityId;
                    }

                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : GetCityMaskingName - ModelPricesInCity");
                objErr.SendMail();
            }
            return _cityId;
        }
        /// <summary>
        /// Created By : Sangram Nandkhile 
        /// Created On : 16th Jan 2016
        /// Read location cookie values and set flag
        /// </summary>
        private void CheckLocationCookie()
        {
            GlobalCityAreaEntity currentCityArea = GlobalCityArea.GetGlobalCityArea();
            if (currentCityArea.CityId > 0 && currentCityArea.CityId == cityId && currentCityArea.AreaId > 0)
            {
                isAreaSelected = true;
            }
        }
    }   // class
}   // namespace
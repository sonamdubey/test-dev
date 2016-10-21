﻿using Bikewale.BAL.BikeBooking;
using Bikewale.BAL.BikeData;
using Bikewale.BAL.Used.Search;
using Bikewale.BindViewModels.Webforms;
using Bikewale.Cache.BikeData;
using Bikewale.Cache.Core;
using Bikewale.Cache.Location;
using Bikewale.common;
using Bikewale.Common;
using Bikewale.Controls;
using Bikewale.DAL.BikeData;
using Bikewale.DAL.Location;
using Bikewale.DAL.Used.Search;
using Bikewale.DTO.Version;
using Bikewale.Entities.BikeBooking;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Location;
using Bikewale.Entities.PriceQuote;
using Bikewale.Entities.Used.Search;
using Bikewale.Interfaces.BikeBooking;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.Location;
using Bikewale.Interfaces.PriceQuote;
using Bikewale.Interfaces.Used.Search;
using Bikewale.Utility;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace Bikewale.New
{
    /// <summary>
    /// Modified By : Lucky Rathore on 09 May 2016.
    /// Description : modelImage declare used for bike model default image url.
    /// </summary>
    public class bikeModel : PageBase //inherited page base class to move viewstate from top of the html page to the end
    {
        #region Global Variables

        protected News_Widget ctrlNews;
        protected NewExpertReviews ctrlExpertReviews;
        protected NewVideosControl ctrlVideos;
        protected NewUserReviewsList ctrlUserReviews;
        protected ModelGallery ctrlModelGallery;
        protected PriceInTopCities ctrlTopCityPrices;
        protected BikeModelPageEntity modelPageEntity;
        protected PopularModelCompare ctrlPopularCompare;
        protected VersionSpecifications bikeSpecs;
        protected PQOnRoadPrice pqOnRoad;
        protected int grid1_size = 9, grid2_size = 3;
        protected Repeater rptModelPhotos, rptNavigationPhoto, rptVarients, rptColor, rptOffers, rptVariants, rptSecondaryDealers;
        protected string cityName = string.Empty, mpqQueryString = string.Empty, areaName = string.Empty, variantText = string.Empty, pqId = string.Empty, bikeName = string.Empty, bikeModelName = string.Empty, bikeMakeName = string.Empty, modelImage = string.Empty;
        protected String clientIP = CommonOpn.GetClientIP();
        protected bool isCitySelected, isAreaSelected, isBikeWalePQ, isDiscontinued, isOnRoadPrice, toShowOnRoadPriceButton;
        //Varible to Hide or show controlers
        protected bool isUserReviewZero = true, isExpertReviewZero = true, isNewsZero = true, isVideoZero = true, isAreaAvailable, isDealerAssitance, isBookingAvailable, isOfferAvailable;
        protected bool isUserReviewActive, isExpertReviewActive, isNewsActive, isVideoActive;
        protected NewAlternativeBikes ctrlAlternativeBikes;
        protected LeadCaptureControl ctrlLeadCapture;
        protected short reviewTabsCnt;
        //Variable to Assing ACTIVE class

        static readonly string _PageNotFoundPath;
        static readonly string _bwHostUrl;
        protected static bool isManufacturer;
        protected string cssOffers = "noOffers", offerDivHide = "hide";
        protected uint onRoadPrice, totalDiscountedPrice, price, bookingAmt, cityId, campaignId, manufacturerId, modelId, variantId, areaId, dealerId;
        protected IEnumerable<CityEntityBase> objCityList = null;
        protected IEnumerable<Bikewale.Entities.Location.AreaEntityBase> objAreaList = null;
        protected OtherVersionInfoEntity objSelectedVariant = null;
        protected Label defaultVariant;
        protected HiddenField hdnVariant;
        protected string pq_leadsource = "32";
        protected string pq_sourcepage = "57";
        protected string hide = "";
        //protected BikeModelPageEntity modelPg;
        protected string pgDescription = string.Empty;
        protected UsedBikes ctrlRecentUsedBikes;
        public Bikewale.Entities.Used.Search.SearchResult UsedBikes = null;
        protected uint totalUsedBikes;

        #region Subscription model variables

        protected ModelPageVM viewModel = null;
        #endregion Subscription model ends

        #endregion

        #region enums
        public enum Overviews
        {
            Capacity,
            Mileage,
            MaxPower,
            Weight
        }
        public enum SummarySpec
        {
            Displacement,
            MaxPower,
            MaximumTorque,
            NoofGears,
            FuelEfficiency,
            BrakeType,
            FrontDisc,
            RearDisc,
            AlloyWheels,
            KerbWeight,
            ChassisType,
            TopSpeed,
            TubelessTyres,
            FuelTankCapacity
        }
        public enum EnT
        {
            Displacement,
            Cylinders,
            MaxPower,
            MaximumTorque,
            Bore,
            Stroke,
            ValvesPerCylinder,
            FuelDeliverySystem,
            FuelType,
            Ignition,
            SparkPlugs,
            CoolingSystem,
            GearboxType,
            NoOfGears,
            TransmissionType,
            Clutch
        }
        public enum BWS
        {
            BrakeType,
            FrontDisc,
            FrontDiscDrumSize,
            RearDisc,
            RearDiscDrumSize,
            CalliperType,
            WheelSize,
            FrontTyre,
            RearTyre,
            TubelessTyres,
            RadialTyres,
            AlloyWheels,
            FrontSuspension,
            RearSuspension
        }
        public enum FEP
        {
            FuelTankCapacity,
            ReserveFuelCapacity,
            FuelEfficiencyOverall,
            FuelEfficiencyRange,
            Zeroto60kmph,
            Zeroto80kmph,
            Zeroto40m,
            TopSpeed,
            Sixityto0Kmph,
            Eightyto0kmph
        }
        public enum DC
        {
            KerbWeight,
            OverallLength,
            OverallWidth,
            OverallHeight,
            Wheelbase,
            GroundClearance,
            SeatHeight,
            ChassisType
        }

        #endregion

        #region Events

        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }

        /// <summary>
        /// Modified By : Lucky Rathore on 01 March 2016.
        /// Description : set make masking name, model Making Name and model ID for video controller
        /// Modified By : Lucky Rathore on 04 July 2016.
        /// Description : function "SetBWUtmz" called.
        /// Modified By : Sajal Gupta on 15/09/2016
        /// Description : Added details for usedBikes.ascx usert control.
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {
            Trace.Warn("Trace 1 : DeviceDetection Start");
            // Modified By :Ashish Kamble on 5 Feb 2016
            Form.Action = Request.RawUrl;
            //device detection
            string originalUrl = Request.ServerVariables["HTTP_X_ORIGINAL_URL"];
            if (String.IsNullOrEmpty(originalUrl))
                originalUrl = Request.ServerVariables["URL"];
            DeviceDetection dd = new DeviceDetection(originalUrl);
            dd.DetectDevice();
            Trace.Warn("Trace 2 : DeviceDetection End");

            #region Do Not change the sequence

            Trace.Warn("Trace 3 : ParseQueryString Start");
            ParseQueryString();
            Trace.Warn("Trace 4 : ParseQueryString End");

            #endregion
            try
            {
                if (modelId > 0)
                {
                    Trace.Warn("Trace 5 : CheckCityCookie Start");
                    CheckCityCookie();
                    Trace.Warn("Trace 6 : CheckCityCookie End");
                    if (hdnVariant != null && hdnVariant.Value != "0")
                        variantId = Convert.ToUInt32(hdnVariant.Value);
                    Trace.Warn("Trace 7 : FetchModelPageDetails Start");
                    modelPageEntity = FetchModelPageDetails(modelId);
                    Trace.Warn("Trace 8 : FetchModelPageDetails End");
                    if (modelPageEntity != null && modelPageEntity.ModelDetails != null && modelPageEntity.ModelDetails.New)
                    {
                        Trace.Warn("Trace 9 : FetchOnRoadPrice Start");
                        FetchOnRoadPrice(modelPageEntity);
                        FillViewModel();
                        Trace.Warn("Trace 10 : FetchOnRoadPrice End");
                    }

                    BindPhotoRepeater(modelPageEntity);
                    LoadVariants(modelPageEntity);
                    Trace.Warn("Trace 18 : BindAlternativeBikeControl Start");
                    BindAlternativeBikeControl(modelPageEntity);
                    Trace.Warn("Trace 19 : BindAlternativeBikeControl End");
                    Trace.Warn("Trace 20 : LoadNewsVidsReviews starts");
                    LoadNewsVidsReviews(modelId, modelPageEntity);
                    Trace.Warn("Trace 21 : LoadNewsVidsReviews ends");
                    ToggleOfferDiv();
                    //calling _bwutmz cookie logic.
                    Trace.Warn("Trace 22 : Clear trailing Query");
                    Trace.Warn("Trace 23 : Page Load ends");
                    BindControls();
                    TotalUsedBikes();
                    CreateMetas();
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, String.Format("Page_load({0})", Request.QueryString["model"]));
                objErr.SendMail();
            }
        }
        /// <summary>
        /// Created By :-Subodh Jain 07 oct 2016
        /// Desc:- To get total number of used bikes
        /// </summary>
        private void TotalUsedBikes()
        {
            try
            {
                ISearch objSearch = null;
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<ISearchFilters, ProcessSearchFilters>()
                    .RegisterType<ISearchQuery, SearchQuery>()
                        .RegisterType<ISearchRepository, SearchRepository>()
                        .RegisterType<ISearch, SearchBikes>();
                    objSearch = container.Resolve<ISearch>();
                    InputFilters objFilters = new InputFilters();
                    // If inputs are set by hash, hash overrides the query string parameters
                    if (cityId > 0)
                        objFilters.City = cityId;
                    if (modelId > 0)
                        objFilters.Model = Convert.ToString(modelId);
                    UsedBikes = objSearch.GetUsedBikesList(objFilters);
                    if (UsedBikes != null)
                        totalUsedBikes = (uint)UsedBikes.TotalCount;
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, "bikeModel.TotalUsedBikes");
                objErr.SendMail();
            }

        }
        /// <summary>
        /// Created By :-Subodh Jain 07 oct 2016
        /// Desc:- values to controls field
        /// </summary>
        private void BindControls()
        {
            ctrlLeadCapture.AreaId = areaId;
            ctrlLeadCapture.ModelId = modelId;
            ctrlLeadCapture.CityId = cityId;

            ctrlRecentUsedBikes.CityId = (int?)cityId;
            ctrlRecentUsedBikes.TopCount = 6;
            ctrlRecentUsedBikes.ModelId = Convert.ToUInt32(modelId);


            ctrlPopularCompare.TopCount = 6;
            ctrlPopularCompare.ModelName = modelPageEntity.ModelDetails.ModelName;
            ctrlPopularCompare.cityid = Convert.ToInt32(cityId);

            if (!isDiscontinued)
                ctrlPopularCompare.versionId = Convert.ToString(variantId);
        }
        /// <summary>
        /// Created By :-Subodh Jain 07 oct 2016
        /// Desc:- Metas description according to discountinue,upcoming,continue bikes
        /// </summary>
        private void CreateMetas()
        {
            if (modelPageEntity.ModelDetails.Futuristic)
            {
                pgDescription = string.Format("{0} {1} Price in India is expected between Rs. {2} and Rs. {3}. Check out {0} {1}  specifications, reviews, mileage, versions, news & photos at BikeWale.com. Launch date of {1} is around {4}", modelPageEntity.ModelDetails.MakeBase.MakeName, modelPageEntity.ModelDetails.ModelName, Bikewale.Utility.Format.FormatNumeric(Convert.ToString(modelPageEntity.UpcomingBike.EstimatedPriceMin)), Bikewale.Utility.Format.FormatNumeric(Convert.ToString(modelPageEntity.UpcomingBike.EstimatedPriceMax)), modelPageEntity.UpcomingBike.ExpectedLaunchDate);
            }
            else if (!modelPageEntity.ModelDetails.New)
            {
                pgDescription = string.Format("{0} {1} Price in India - Rs. {2}. It has been discontinued in India. There are {3} used {1} bikes for sale. Check out {1} specifications, reviews, mileage, versions, news & photos at BikeWale.com", modelPageEntity.ModelDetails.MakeBase.MakeName, modelPageEntity.ModelDetails.ModelName, Bikewale.Utility.Format.FormatNumeric(price.ToString()), totalUsedBikes);
            }
            else
            {
                pgDescription = String.Format("{0} Price in India - Rs. {1}. Find {0} Reviews, Specs, Features, Mileage, On Road Price. See {2} Colors, Images at Bikewale.", bikeName, Bikewale.Utility.Format.FormatNumeric(price.ToString()), bikeModelName);

            }
        }

        private void LoadNewsVidsReviews(uint modelId, BikeModelPageEntity modelPage)
        {
            if (modelPage != null && modelPage.ModelDetails != null)
            {
                int _modelId = Convert.ToInt32(modelId);
                ctrlNews.TotalRecords = 3;
                ctrlNews.ModelId = _modelId;
                ctrlNews.WidgetTitle = bikeName;

                ctrlExpertReviews.TotalRecords = 3;
                ctrlExpertReviews.ModelId = _modelId;
                ctrlExpertReviews.MakeMaskingName = modelPage.ModelDetails.MakeBase.MaskingName.Trim();
                ctrlExpertReviews.ModelMaskingName = modelPage.ModelDetails.MaskingName.Trim();
                ctrlExpertReviews.MakeName = modelPage.ModelDetails.MakeBase.MakeName;
                ctrlExpertReviews.ModelName = modelPage.ModelDetails.ModelName;

                ctrlVideos.TotalRecords = 3;
                ctrlVideos.ModelId = _modelId;
                ctrlVideos.MakeId = modelPage.ModelDetails.MakeBase.MakeId;
                ctrlVideos.MakeMaskingName = modelPage.ModelDetails.MakeBase.MaskingName.Trim();
                ctrlVideos.ModelMaskingName = modelPage.ModelDetails.MaskingName.Trim();
                ctrlVideos.WidgetTitle = bikeName;
                ctrlVideos.MakeName = modelPage.ModelDetails.MakeBase.MakeName;
                ctrlVideos.ModelName = modelPage.ModelDetails.ModelName;

                ctrlRecentUsedBikes.MakeId = Convert.ToUInt32(modelPage.ModelDetails.MakeBase.MakeId);

                ctrlUserReviews.ReviewCount = 3;
                ctrlUserReviews.PageNo = 1;
                ctrlUserReviews.PageSize = 3;
                ctrlUserReviews.ModelId = _modelId;
                ctrlUserReviews.Filter = Entities.UserReviews.FilterBy.MostRecent;
                ctrlUserReviews.MakeName = modelPage.ModelDetails.MakeBase.MakeName;
                ctrlUserReviews.ModelName = modelPage.ModelDetails.ModelName;

                if (!modelPage.ModelDetails.Futuristic || modelPageEntity.ModelDetails.New)
                    ctrlTopCityPrices.ModelId = Convert.ToUInt32(_modelId);
                else ctrlTopCityPrices.ModelId = 0;

                ctrlTopCityPrices.IsDiscontinued = isDiscontinued;
                ctrlTopCityPrices.TopCount = 8;
                ctrlPopularCompare.ModelName = modelPageEntity.ModelDetails.ModelName;
            }
        }


        /// <summary>
        /// Modified by     :   Sumit Kate on 15 Feb 2016
        /// Description     :   Replace First() with FirstOrDefault() for BPQOutput.Varient.Where function call
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rptVarients_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    if (pqOnRoad != null)
                    {
                        Label currentTextBox = (Label)e.Item.FindControl("txtComment");
                        HiddenField hdn = (HiddenField)e.Item.FindControl("hdnVariant");
                        Label lblExOn = (Label)e.Item.FindControl("lblExOn");

                        var totalDiscount = totalDiscountedPrice;

                        if (isOnRoadPrice)
                            lblExOn.Text = "On-road price";

                        if (pqOnRoad.IsDealerPriceAvailable && pqOnRoad.DPQOutput != null && pqOnRoad.DPQOutput.Varients != null)
                        {
                            var selecteVersionList = pqOnRoad.DPQOutput.Varients.Where(p => Convert.ToString(p.objVersion.VersionId) == hdn.Value);
                            if (selecteVersionList != null && selecteVersionList.Count() > 0)
                                currentTextBox.Text = Bikewale.Utility.Format.FormatPrice(Convert.ToString(selecteVersionList.First().OnRoadPrice - totalDiscount));
                        }
                        else if (pqOnRoad.BPQOutput != null && pqOnRoad.BPQOutput.Varients != null)
                        {
                            var selected = pqOnRoad.BPQOutput.Varients.Where(p => Convert.ToString(p.VersionId) == hdn.Value).FirstOrDefault();
                            if (selected != null)
                                currentTextBox.Text = Bikewale.Utility.Format.FormatPrice(Convert.ToString(selected.OnRoadPrice));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, Request.ServerVariables["URL"] + "rptVarients_ItemDataBound");
                objErr.SendMail();
            }
        }

        protected void btnVariant_Command(object sender, CommandEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.CommandName))
            {
                variantId = Convert.ToUInt32(e.CommandName);
                FetchVariantDetails(variantId);
                defaultVariant.Text = Convert.ToString(e.CommandArgument);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Author          :   Sangram Nandkhile
        /// Created Date    :   20 Nov 2015
        /// Description     :   To Load Specs for each variant
        /// </summary>
        private void LoadVariants(BikeModelPageEntity modelPg)
        {
            try
            {
                if (modelPg != null)
                {
                    if (modelPg.ModelVersionSpecs != null && variantId <= 0)
                    {
                        variantId = modelPg.ModelVersionSpecs.BikeVersionId;

                    }

                    if (modelPg.ModelVersions != null && !modelPg.ModelDetails.Futuristic)
                    {
                        if (modelPg.ModelVersions.Count > 1)
                        {
                            if (modelPg.ModelVersionSpecs != null && modelPg.ModelVersionSpecs.BikeVersionId != 0)
                            {
                                var firstVer = modelPg.ModelVersions.Where(p => p.VersionId == variantId).FirstOrDefault();
                                if (firstVer != null)
                                    defaultVariant.Text = firstVer.VersionName;

                                if (variantId == 0)
                                    hdnVariant.Value = Convert.ToString(modelPg.ModelVersionSpecs.BikeVersionId);
                                else
                                    hdnVariant.Value = Convert.ToString(variantId);


                            }
                            else if (modelPg.ModelVersions.Count > 1)
                            {
                                var firstVer = modelPg.ModelVersions.FirstOrDefault();
                                if (firstVer != null)
                                    defaultVariant.Text = firstVer.VersionName;
                            }
                            rptVariants.DataSource = modelPg.ModelVersions;
                            rptVariants.DataBind();
                        }
                        else if (modelPg.ModelVersions.Count == 1)
                        {
                            var firstVer = modelPg.ModelVersions.FirstOrDefault();
                            if (firstVer != null)
                                variantText = firstVer.VersionName;
                        }

                        if (string.IsNullOrEmpty(variantText))
                        {
                            variantText = defaultVariant.Text;
                        }

                    }
                    if (modelPg.ModelVersions != null && modelPg.ModelVersions.Count > 0)
                    {
                        rptVarients.DataSource = modelPg.ModelVersions;
                        rptVarients.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, Request.ServerVariables["URL"] + " : LoadVariants");
                objErr.SendMail();
            }
        }

        private void BindAlternativeBikeControl(BikeModelPageEntity modelPage)
        {
            ctrlAlternativeBikes.TopCount = 6;
            ctrlAlternativeBikes.PQSourceId = (int)PQSourceEnum.Desktop_ModelPage_Alternative;
            ctrlAlternativeBikes.WidgetTitle = bikeName;
            ctrlAlternativeBikes.model = modelPage.ModelDetails.ModelName;
            ctrlAlternativeBikes.cityId = Convert.ToInt32(cityId);
            if (modelPage != null)
            {
                var modelVersions = modelPage.ModelVersions;
                if (modelVersions != null && modelVersions.Count > 0)
                {
                    ctrlAlternativeBikes.VersionId = modelVersions[0].VersionId;
                }
            }
        }


        private void BindPhotoRepeater(BikeModelPageEntity modelPage)
        {
            if (modelPage != null)
            {
                var photos = modelPage.Photos;
                if (photos != null && photos.Count > 0)
                {
                    rptModelPhotos.DataSource = photos;
                    rptModelPhotos.DataBind();
                    rptNavigationPhoto.DataSource = photos;
                    rptNavigationPhoto.DataBind();

                    ctrlModelGallery.bikeName = bikeName;
                    ctrlModelGallery.modelId = Convert.ToInt32(modelId);
                    ctrlModelGallery.Photos = photos;
                }

                //bind model colors
                if (modelPage.ModelColors != null && modelPage.ModelColors.Count() > 0)
                {

                    rptColor.DataSource = modelPage.ModelColors;
                    rptColor.DataBind();
                }

                if (!String.IsNullOrEmpty(modelPage.ModelDetails.OriginalImagePath))
                {
                    modelImage = Utility.Image.GetPathToShowImages(modelPage.ModelDetails.OriginalImagePath, modelPage.ModelDetails.HostUrl, Bikewale.Utility.ImageSize._476x268);
                }
            }
        }

        private void ParseQueryString()
        {
            ModelMaskingResponse objResponse = null;
            string modelQuerystring = Request.QueryString["model"];
            string VersionIdStr = Request.QueryString["vid"];
            if (!string.IsNullOrEmpty(VersionIdStr))
            {
                UInt32.TryParse(VersionIdStr, out variantId);
            }
            Trace.Warn("modelQuerystring 1 : ", modelQuerystring);
            try
            {
                if (!string.IsNullOrEmpty(modelQuerystring))
                {
                    using (IUnityContainer container = new UnityContainer())
                    {
                        container.RegisterType<IBikeMaskingCacheRepository<BikeModelEntity, int>, BikeModelMaskingCache<BikeModelEntity, int>>()
                                 .RegisterType<ICacheManager, MemcacheManager>()
                                 .RegisterType<IBikeModelsRepository<BikeModelEntity, int>, BikeModelsRepository<BikeModelEntity, int>>()
                                ;
                        var objCache = container.Resolve<IBikeMaskingCacheRepository<BikeModelEntity, int>>();
                        objResponse = objCache.GetModelMaskingResponse(modelQuerystring);
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.Warn("exception 1 : ");
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, Request.ServerVariables["URL"] + "ParseQueryString");
                objErr.SendMail();

                Response.Redirect("/new/", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
                this.Page.Visible = false;
            }
            finally
            {
                Trace.Warn("finally");
                if (!string.IsNullOrEmpty(modelQuerystring))
                {
                    if (objResponse != null)
                    {
                        Trace.Warn(" objResponse.StatusCode : ", objResponse.StatusCode.ToString());
                        Trace.Warn(" objResponse.ModelId : ", objResponse.ModelId.ToString());
                        //Trace.Warn(" objResponse.MaskingName : ", objResponse.MaskingName.ToString());
                        if (objResponse.StatusCode == 200)
                        {
                            modelId = objResponse.ModelId;
                        }
                        else if (objResponse.StatusCode == 301)
                        {
                            //redirect permanent to new page 
                            CommonOpn.RedirectPermanent(Request.RawUrl.Replace(modelQuerystring, objResponse.MaskingName));
                        }
                        else
                        {
                            Trace.Warn("pageNotFound.aspx 1");
                            Response.Redirect(CommonOpn.AppPath + "pageNotFound.aspx", false);
                            HttpContext.Current.ApplicationInstance.CompleteRequest();
                            this.Page.Visible = false;
                        }
                    }
                    else
                    {
                        Trace.Warn("pageNotFound.aspx 2");
                        Response.Redirect(CommonOpn.AppPath + "pageNotFound.aspx", false);
                        HttpContext.Current.ApplicationInstance.CompleteRequest();
                        this.Page.Visible = false;
                    }
                }
                else
                {
                    Trace.Warn("pageNotFound.aspx 3");
                    Response.Redirect(CommonOpn.AppPath + "pageNotFound.aspx", false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                    this.Page.Visible = false;
                }
            }
        }

        /// <summary>
        /// Modified by :   Sumit Kate on 05 Jan 2016
        /// Description :   Replaced the Convert.ToXXX with XXX.TryParse method
        /// Modified By : Sushil Kumar on 26th August 2016
        /// Description : Replaced location name from location cookie to selected location objects for city and area respectively.
        /// </summary>
        private void CheckCityCookie()
        {
            // Read current cookie values
            // Check if there are areas for current model and City
            // If No then drop area cookie
            string location = String.Empty;
            var cookies = this.Context.Request.Cookies;
            if (cookies.AllKeys.Contains("location"))
            {
                location = cookies["location"].Value;
                if (!String.IsNullOrEmpty(location) && location.IndexOf('_') != -1)
                {
                    string[] locArray = location.Split('_');
                    if (locArray.Length > 0)
                    {
                        UInt32.TryParse(locArray[0], out cityId);
                        if (modelId > 0)
                        {
                            objCityList = FetchCityByModelId(modelId);
                            if (objCityList != null)
                            {
                                // If Model doesn't have current City then don't show it, Show Ex-showroom Mumbai
                                var _objCity = objCityList.FirstOrDefault(p => p.CityId == cityId);
                                if (_objCity != null)
                                {
                                    cityName = _objCity.CityName;

                                    isCitySelected = true;
                                }
                            }
                        }
                    }
                    // This function will check if Areas are available for city and Model
                    objAreaList = GetAreaForCityAndModel();
                    // locArray.Length = 4 Means City and area exists
                    if (locArray.Length > 3 && cityId != 0)
                    {
                        if (objAreaList != null && UInt32.TryParse(locArray[2], out areaId))
                        {
                            var _objArea = objAreaList.FirstOrDefault(p => p.AreaId == areaId);
                            if (_objArea != null && isAreaAvailable)
                            {
                                areaName = _objArea.AreaName;
                                isAreaSelected = true;
                            }
                        }

                    }
                }
            }
            SetFlags();
        }
        //static readonly string apiURL = "/api/model/details/?modelId={0}&variantId={1}";
        //static readonly string onRoadApi = "/api/OnRoadPrice/?cityId={0}&modelId={1}&clientIP={2}&sourceType={3}&areaId={4}";
        //static readonly string _requestType = "application/json";
        /// <summary>
        /// Author          :   Sangram Nandkhile
        /// Created Date    :   18 Nov 2015
        /// 
        /// </summary>
        private BikeModelPageEntity FetchModelPageDetails(uint modelID)
        {
            var modelPg = new BikeModelPageEntity();
            try
            {
                if (modelID > 0)
                {
                    using (IUnityContainer container = new UnityContainer())
                    {
                        container.RegisterType<IBikeModelsCacheRepository<int>, BikeModelsCacheRepository<BikeModelEntity, int>>()
                                 .RegisterType<IBikeModels<BikeModelEntity, int>, BikeModels<BikeModelEntity, int>>()
                                 .RegisterType<IBikeModelsRepository<BikeModelEntity, int>, BikeModelsRepository<BikeModelEntity, int>>()
                                 .RegisterType<ICacheManager, MemcacheManager>();
                        var objCache = container.Resolve<IBikeModelsCacheRepository<int>>();
                        modelPg = objCache.GetModelPageDetails(Convert.ToInt16(modelID));
                        if (modelPg != null)
                        {

                            if (!modelPg.ModelDetails.Futuristic && modelPg.ModelVersionSpecs != null)
                            {
                                modelImage = string.Format("{0} {1}", modelPg.ModelDetails.HostUrl, modelPg.ModelDetails.OriginalImagePath);
                                price = Convert.ToUInt32(modelPg.ModelDetails.MinPrice);
                                if (variantId == 0 && cityId == 0)
                                {
                                    variantId = modelPg.ModelVersionSpecs.BikeVersionId;
                                }
                                // Check it versionId passed through url exists in current model's versions
                                else
                                {
                                    if (variantId > 0)
                                    {
                                        if (!modelPg.ModelVersions.Exists(p => p.VersionId == variantId))
                                        {
                                            variantId = modelPg.ModelVersionSpecs.BikeVersionId;
                                        }
                                    }
                                }
                            }
                            if (!modelPg.ModelDetails.New)
                                isDiscontinued = true;
                            if (modelPg.ModelDetails != null)
                            {
                                if (modelPg.ModelDetails.ModelName != null)
                                    bikeModelName = modelPg.ModelDetails.ModelName;
                                if (modelPg.ModelDetails.MakeBase != null)
                                    bikeMakeName = modelPg.ModelDetails.MakeBase.MakeName;
                                bikeName = string.Format("{0} {1}", bikeMakeName, bikeModelName);
                                modelImage = Utility.Image.GetPathToShowImages(modelPg.ModelDetails.OriginalImagePath, modelPg.ModelDetails.HostUrl, Bikewale.Utility.ImageSize._476x268);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, Request.ServerVariables["URL"] + "FetchmodelPgDetails");
                objErr.SendMail();
            }
            return modelPg;
        }

        /// <summary>
        /// Author          :   Sangram Nandkhile
        /// Created Date    :   20 Nov 2015
        /// Description     :   Fetch On road price depending on City, Area and DealerPQ and BWPQ
        /// Modified By     :   Sushil Kumar on 19th April 2016
        /// Description     :   Removed repeater binding for rptCategory and rptDiscount as view breakup popup removed 
        /// </summary>
        private void FetchOnRoadPrice(BikeModelPageEntity modelPage)
        {
            var errorParams = string.Empty;
            try
            {
                if (cityId != 0)
                {
                    pqOnRoad = GetOnRoadPrice();
                    // Set Pricequote Cookie
                    if (pqOnRoad != null)
                    {
                        if (pqOnRoad.BPQOutput != null && !String.IsNullOrEmpty(pqOnRoad.BPQOutput.ManufacturerAd))
                            pqOnRoad.BPQOutput.ManufacturerAd = Format.FormatManufacturerAd(pqOnRoad.BPQOutput.ManufacturerAd, pqOnRoad.BPQOutput.CampaignId, pqOnRoad.BPQOutput.ManufacturerName, pqOnRoad.BPQOutput.MaskingNumber, Convert.ToString(pqOnRoad.BPQOutput.ManufacturerId), pqOnRoad.BPQOutput.Area, pq_leadsource, pq_sourcepage, string.Empty, string.Empty, string.Empty, string.IsNullOrEmpty(pqOnRoad.BPQOutput.MaskingNumber) ? "hide" : string.Empty);

                        variantId = pqOnRoad.PriceQuote.VersionId;
                        if (pqOnRoad.PriceQuote != null)
                        {
                            dealerId = pqOnRoad.PriceQuote.DealerId;
                            if (dealerId > 0)
                            {
                                DealerAssistance dealerAssisteance = new DealerAssistance();
                                isDealerAssitance = dealerAssisteance.IsDealerAssistance(dealerId.ToString());
                            }
                            pqId = Convert.ToString(pqOnRoad.PriceQuote.PQId);
                        }
                        mpqQueryString = EncodingDecodingHelper.EncodeTo64(PriceQuoteQueryString.FormQueryString(cityId.ToString(), pqId, areaId.ToString(), variantId.ToString(), dealerId.ToString()));
                        if (pqOnRoad.IsDealerPriceAvailable && pqOnRoad.DPQOutput != null && pqOnRoad.DPQOutput.Varients != null && pqOnRoad.DPQOutput.Varients.Count() > 0)
                        {
                            #region when dealer Price is Available

                            var selectedVariant = pqOnRoad.DPQOutput.Varients.Where(p => p.objVersion.VersionId == variantId).FirstOrDefault();
                            if (selectedVariant != null)
                            {
                                onRoadPrice = selectedVariant.OnRoadPrice;
                                price = onRoadPrice;
                                if (selectedVariant.PriceList != null)
                                {
                                    totalDiscountedPrice = CommonModel.GetTotalDiscount(pqOnRoad.discountedPriceList);
                                }
                                bookingAmt = selectedVariant.BookingAmount;
                                if (bookingAmt > 0)
                                    isBookingAvailable = true;

                                if (pqOnRoad.discountedPriceList != null && pqOnRoad.discountedPriceList.Count > 0)
                                {
                                    price = (onRoadPrice - totalDiscountedPrice);
                                }
                            }
                            #endregion
                        }
                        else
                        {
                            if (pqOnRoad.BPQOutput != null && pqOnRoad.BPQOutput.Varients != null)
                            {
                                #region BikeWale PQ
                                if (hdnVariant.Value != "0")
                                {
                                    variantId = Convert.ToUInt32(hdnVariant.Value);
                                    if (variantId != 0)
                                    {
                                        objSelectedVariant = pqOnRoad.BPQOutput.Varients.Where(p => p.VersionId == variantId).FirstOrDefault();
                                        if (objSelectedVariant != null)
                                            price = Convert.ToUInt32(objSelectedVariant.OnRoadPrice);
                                    }
                                }

                                else if (variantId != 0 && isOnRoadPrice)
                                {
                                    objSelectedVariant = pqOnRoad.BPQOutput.Varients.Where(p => p.VersionId == variantId).FirstOrDefault();
                                    if (objSelectedVariant != null)

                                        price = Convert.ToUInt32(objSelectedVariant.OnRoadPrice);

                                }

                                else if (isOnRoadPrice)
                                {
                                    objSelectedVariant = pqOnRoad.BPQOutput.Varients.FirstOrDefault();
                                    price = Convert.ToUInt32(objSelectedVariant.OnRoadPrice);
                                }

                                campaignId = pqOnRoad.BPQOutput.CampaignId;
                                manufacturerId = pqOnRoad.BPQOutput.ManufacturerId;

                                isBikeWalePQ = true;
                                #endregion
                            }
                        }
                    }
                    else // On road PriceQuote is Null so get price from the modelpage variants
                    {
                        if (variantId != 0)
                        {
                            var modelVersions = modelPage.ModelVersions.Where(p => p.VersionId == variantId).FirstOrDefault();
                            if (modelVersions != null)
                                price = Convert.ToUInt32(modelVersions.Price);
                        }
                        else
                        {
                            price = Convert.ToUInt32(modelPage.ModelDetails.MinPrice);
                        }
                    }
                }
                else
                {
                    var modelVersions = modelPage.ModelVersions.Where(p => p.VersionId == variantId).FirstOrDefault();

                    if (modelVersions != null)
                        price = Convert.ToUInt32(modelVersions.Price);
                }
            }
            catch (Exception ex)
            {
                if (string.IsNullOrEmpty(errorParams))
                    errorParams = "=== modelpage ===" + Newtonsoft.Json.JsonConvert.SerializeObject(modelPage);
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, Request.ServerVariables["URL"] + "-" + "FetchOnRoadPrice" + " ===== parameters ========= " + errorParams);
                objErr.SendMail();
            }
        }
        /// <summary>
        /// Author: Sangram Nandkhile
        /// Desc: Removed API Call for on road Price Quote
        /// Modified By : Lucky Rathore on 09 May 2016.
        /// Description : modelImage intialize.
        /// Modified By : Lucky Rathore on 27 June 2016
        /// Description : replace cookie __utmz with _bwutmz
        /// </summary>
        /// <returns></returns>
        private PQOnRoadPrice GetOnRoadPrice()
        {
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IBikeModels<BikeModelEntity, int>, BikeModels<BikeModelEntity, int>>();
                    IBikeModels<BikeModelEntity, int> objClient = container.Resolve<IBikeModels<BikeModelEntity, int>>();
                    container.RegisterType<IPriceQuote, BAL.PriceQuote.PriceQuote>();
                    IPriceQuote objPq = container.Resolve<IPriceQuote>();
                    container.RegisterType<IDealerPriceQuote, DealerPriceQuote>();
                    IDealerPriceQuote objDealer = container.Resolve<IDealerPriceQuote>();
                    BikeQuotationEntity bpqOutput = null;
                    PriceQuoteParametersEntity objPQEntity = new PriceQuoteParametersEntity();
                    objPQEntity.CityId = Convert.ToUInt16(cityId);
                    objPQEntity.AreaId = Convert.ToUInt32(areaId);
                    objPQEntity.ClientIP = clientIP;
                    objPQEntity.SourceId = 1;
                    objPQEntity.ModelId = modelId;
                    objPQEntity.VersionId = variantId;
                    objPQEntity.PQLeadId = Convert.ToUInt16(PQSourceEnum.Desktop_ModelPage);
                    objPQEntity.UTMA = Request.Cookies["__utma"] != null ? Request.Cookies["__utma"].Value : "";
                    objPQEntity.UTMZ = Request.Cookies["_bwutmz"] != null ? Request.Cookies["_bwutmz"].Value : "";
                    objPQEntity.DeviceId = Request.Cookies["BWC"] != null ? Request.Cookies["BWC"].Value : "";
                    PQOutputEntity objPQOutput = objDealer.ProcessPQ(objPQEntity);

                    if (variantId == 0)
                    {
                        if (objPQOutput != null)
                        {
                            variantId = objPQOutput.VersionId;
                        }
                    }

                    if (objPQOutput != null)
                    {
                        pqOnRoad = new PQOnRoadPrice();
                        pqOnRoad.PriceQuote = objPQOutput;

                        string api = string.Empty;
                        if (objPQOutput != null && objPQOutput.PQId > 0)
                        {
                            bpqOutput = objPq.GetPriceQuoteById(objPQOutput.PQId, LeadSourceEnum.Model_Desktop);
                            bpqOutput.Varients = objPq.GetOtherVersionsPrices(objPQOutput.PQId);
                            if (bpqOutput != null)
                            {
                                pqOnRoad.BPQOutput = bpqOutput;
                            }
                            if (objPQOutput.DealerId != 0)
                            {
                                // call another api
                                PQ_QuotationEntity oblDealerPQ = null;
                                AutoBizCommon dealerPq = new AutoBizCommon();
                                try
                                {
                                    oblDealerPQ = dealerPq.GetDealePQEntity(cityId, objPQOutput.DealerId, variantId);

                                    if (oblDealerPQ != null)
                                    {
                                        uint insuranceAmount = 0;
                                        foreach (var price in oblDealerPQ.PriceList)
                                        {
                                            pqOnRoad.IsInsuranceFree = Bikewale.Utility.DealerOfferHelper.HasFreeInsurance(objPQOutput.DealerId.ToString(), string.Empty, price.CategoryName, price.Price, ref insuranceAmount);
                                        }
                                        pqOnRoad.IsInsuranceFree = true;
                                        pqOnRoad.DPQOutput = oblDealerPQ;
                                        if (pqOnRoad.DPQOutput.objOffers != null && pqOnRoad.DPQOutput.objOffers.Count > 0)
                                            pqOnRoad.DPQOutput.discountedPriceList = OfferHelper.ReturnDiscountPriceList(pqOnRoad.DPQOutput.objOffers, pqOnRoad.DPQOutput.PriceList);
                                        pqOnRoad.InsuranceAmount = insuranceAmount;
                                        if (oblDealerPQ.discountedPriceList != null && oblDealerPQ.discountedPriceList.Count > 0)
                                        {
                                            pqOnRoad.IsDiscount = true;
                                            pqOnRoad.discountedPriceList = oblDealerPQ.discountedPriceList;
                                        }
                                    }
                                    //}
                                }
                                catch (Exception ex)
                                {
                                    Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, Request.ServerVariables["URL"] + "GetOnRoadPrice" + "-" + api);
                                    objErr.SendMail();
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, Request.ServerVariables["URL"] + "GetOnRoadPrice");
                objErr.SendMail();
            }
            return pqOnRoad;
        }


        protected string FormatShowReview(string makeName, string modelName)
        {
            return string.Format("/{0}-bikes/{1}/user-reviews/", makeName, modelName);
        }

        protected string FormatWriteReviewLink()
        {
            return String.Format("/content/userreviews/writereviews.aspx?bikem={0}", modelId);
        }

        static readonly String strSpec = "<span class=\"font26 text-bold text-black\">{0}</span><span class=\"font24 text-light-grey margin-left5\">{1}</span>";

        protected string FormatOverview(object spec, Overviews overview)
        {

            if (spec != null && !string.IsNullOrEmpty(spec.ToString()))
            {
                switch (overview)
                {
                    case Overviews.Capacity:
                        return String.Format(strSpec, spec, "cc");
                    case Overviews.Mileage:
                        return String.Format(strSpec, spec, "kmpl");
                    case Overviews.MaxPower:
                        return String.Format(strSpec, spec, "bhp");
                    case Overviews.Weight:
                        return String.Format(strSpec, spec, "kgs");
                    default:
                        return String.Format(strSpec, "-", "");
                }
            }
            else
            {
                return String.Format(strSpec, "-", "");
            }
        }

        static readonly string formatMaxPower = "<div class=\"text-bold\">{0} bhp @ {1} rpm</div>";

        protected string FormatMaxPower(object bhp, object rpm)
        {

            if (bhp != null && !String.IsNullOrEmpty(bhp.ToString()) && rpm != null && !String.IsNullOrEmpty(rpm.ToString()) && rpm.ToString() != "0")
            {
                return String.Format(formatMaxPower, bhp.ToString(), rpm.ToString());
            }
            return "<div class=\"text-bold\">-</div>";
        }

        protected string FormatMaxTorque(object nm, object rpm)
        {
            string format = "<div class=\"text-bold\">{0} Nm @ {1} rpm</div>";
            if (nm != null && !String.IsNullOrEmpty(nm.ToString()) && rpm != null && !String.IsNullOrEmpty(rpm.ToString()) && rpm.ToString() != "0")
            {
                return String.Format(format, nm.ToString(), rpm.ToString());
            }
            return "<div class=\"text-bold\">-</div>";
        }

        protected string FormatValue(object val)
        {
            string format = "<div class=\"text-bold\">{0}</div>";
            if (val != null && !String.IsNullOrEmpty(val.ToString()) && val.ToString() != "0")
            {
                return String.Format(format, val.ToString());
            }
            return "<div class=\"text-bold\">-</div>";
        }

        protected string FormatValue(short val)
        {
            string format = "<div class=\"text-bold\">{0}</div>";
            if (val > 0)
            {
                return String.Format(format, val.ToString());
            }
            return "<div class=\"text-bold\">-</div>";
        }

        protected string FormatValue(string val)
        {
            string format = "<div class=\"text-bold\">{0}</div>";
            if (!String.IsNullOrEmpty(val))
            {
                return String.Format(format, val.ToString());
            }
            return "<div class=\"text-bold\">-</div>";
        }

        protected string FormatValue(float val)
        {
            string format = "<div class=\"text-bold\">{0}</div>";
            if (val > 0.0f)
            {
                return String.Format(format, val.ToString());
            }
            return "<div class=\"text-bold\">-</div>";
        }

        protected string FormatFuelEfficiency(object val)
        {
            string format = "<div class=\"text-bold\">{0} kmpl</div>";
            if (val != null && !String.IsNullOrEmpty(val.ToString()))
            {
                return String.Format(format, val.ToString());
            }
            return "<div class=\"text-bold\">-</div>";
        }

        protected string FormatKerbWeight(object val)
        {
            string format = "<div class=\"text-bold\">{0} kgs</div>";
            if (val != null && !String.IsNullOrEmpty(val.ToString()))
            {
                return String.Format(format, val.ToString());
            }
            return "<div class=\"text-bold\">-</div>";
        }

        protected string FormatSpeed(object val)
        {
            string format = "<div class=\"text-bold\">{0} kmph</div>";
            if (val != null && !String.IsNullOrEmpty(val.ToString()))
            {
                return String.Format(format, val.ToString());
            }
            return "<div class=\"text-bold\">-</div>";
        }

        protected string FormatLiquid(object val)
        {
            string format = "<div class=\"text-bold\">{0} litres</div>";
            if (val != null && !String.IsNullOrEmpty(val.ToString()))
            {
                return String.Format(format, val.ToString());
            }
            return "<div class=\"text-bold\">-</div>";
        }

        protected string FormatDimension(ushort val, string dim)
        {
            string format = "<div class=\"text-bold\">{0} {1}</div>";
            if (val > 0)
            {
                return String.Format(format, val.ToString(), dim);
            }
            return "<div class=\"text-bold\">-</div>";
        }

        protected string FormatDimension(float val, string dim)
        {
            string format = "<div class=\"text-bold\">{0} {1}</div>";
            if (val > 0.0f)
            {
                return String.Format(format, val.ToString(), dim);
            }
            return "<div class=\"text-bold\">-</div>";
        }

        protected string FormatVarientMinSpec(bool alloyWheel, bool elecStart, bool abs, string breakType)
        {
            string format = string.Empty;
            if (alloyWheel)
            {
                format = String.Concat(format.Trim(), " Alloy Wheels,");
            }
            else
            {
                format = String.Concat(format.Trim(), " Spoke Wheels,");
            }

            if (elecStart)
            {
                format = String.Concat(format.Trim(), " Electric Start,");
            }
            else
            {
                format = String.Concat(format.Trim(), " Kick Start,");
            }

            if (abs)
            {
                format = String.Concat(format.Trim(), " ABS,");
            }

            if (!String.IsNullOrEmpty(breakType))
            {
                format = String.Concat(format.Trim(), breakType, " Brake,");
            }

            if (String.IsNullOrEmpty(format.Trim()))
            {
                return "No specifications.";
            }
            return format.Trim().Substring(0, format.Length - 1);
        }

        public override void Dispose()
        {
            if (modelPageEntity != null)
            {
                modelPageEntity.Photos = null;
                modelPageEntity.ModelColors = null;
                modelPageEntity.ModelDesc = null;
                modelPageEntity.ModelDetails = null;
                modelPageEntity.ModelVersions = null;
                modelPageEntity.ModelVersionSpecs = null;
                modelPageEntity.UpcomingBike = null;
                modelPageEntity = null;
            }
            base.Dispose();
        }

        static bikeModel()
        {
            _PageNotFoundPath = Bikewale.Common.CommonOpn.AppPath + "pageNotFound.aspx";
            _bwHostUrl = ConfigurationManager.AppSettings["bwHostUrl"];
            isManufacturer = (ConfigurationManager.AppSettings["TVSManufacturerId"] != "0") ? true : false;
        }

        /// <summary>
        /// Author          :   Sangram Nandkhile
        /// Created Date    :   18 Nov 2015
        /// Description     :   Sends the notification to Customer and Dealer
        /// </summary>
        private BikeSpecificationEntity FetchVariantDetails(uint versionId)
        {
            BikeSpecificationEntity specsFeature = null;
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IBikeModelsRepository<BikeModelEntity, int>, BikeModelsRepository<BikeModelEntity, int>>();
                    IBikeModelsRepository<BikeModelEntity, int> objVersion = container.Resolve<IBikeModelsRepository<BikeModelEntity, int>>();
                    specsFeature = objVersion.MVSpecsFeatures((int)versionId);
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, Request.ServerVariables["URL"] + "FetchVariantDetails");
                objErr.SendMail();
            }
            return specsFeature;
        }
        /// <summary>
        /// Author          :   Sangram Nandkhile
        /// Created Date    :   24 Nov 2015
        /// Description     :   Gets City Details by ModelId
        /// </summary>
        /// <param name="modelId">Model Id</param>
        private IEnumerable<CityEntityBase> FetchCityByModelId(uint modelId)
        {
            IEnumerable<CityEntityBase> cityList = null;
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<ICity, CityRepository>()
                                 .RegisterType<ICacheManager, MemcacheManager>()
                                 .RegisterType<ICityCacheRepository, CityCacheRepository>();
                    ICityCacheRepository objcity = container.Resolve<ICityCacheRepository>();
                    cityList = objcity.GetPriceQuoteCities(modelId);
                    return cityList;
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, Request.ServerVariables["URL"] + "FetchCityByModelId");
                objErr.SendMail();
            }
            return cityList;
        }

        /// <summary>
        /// Author          :   Sangram Nandkhile
        /// Created Date    :   24 Nov 2015
        /// Description     :   Get List of Area depending on City and Model Id
        /// </summary>
        private IEnumerable<Bikewale.Entities.Location.AreaEntityBase> GetAreaForCityAndModel()
        {
            IEnumerable<Bikewale.Entities.Location.AreaEntityBase> areaList = null;
            try
            {
                if (CommonOpn.CheckId(modelId.ToString()))
                {
                    using (IUnityContainer container = new UnityContainer())
                    {
                        container.RegisterType<IDealerPriceQuote, DealerPriceQuote>()
                            .RegisterType<ICacheManager, MemcacheManager>()
                            .RegisterType<IAreaCacheRepository, AreaCacheRepository>();

                        IAreaCacheRepository objArea = container.Resolve<IAreaCacheRepository>();
                        areaList = objArea.GetAreaList(modelId, cityId);
                        isAreaAvailable = (areaList != null && areaList.Count() > 0);
                        return areaList;
                    }
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, Request.ServerVariables["URL"] + "GetAreaForCityAndModel");
                objErr.SendMail();
            }

            return areaList;
        }

        /// <summary>
        /// Author          :   Sangram Nandkhile
        /// Created Date    :   20 Nov 2015
        /// Description     :   These values need to be set to reflect UI HTML changes
        /// </summary>
        private void ToggleOfferDiv()
        {
            if (isOfferAvailable)
            {
                grid1_size = 5;
                grid2_size = 7;
                cssOffers = string.Empty;
                offerDivHide = string.Empty;
            }
            else
            {
                grid1_size = 9;
                grid2_size = 3;
                cssOffers = "noOffers";
                offerDivHide = "hide";
            }
        }
        /// <summary>
        /// Author: Sangram Nandkhile
        /// Created on: 21-12-2015
        /// Desc: Set flags for aspx mark up to show and hide buttons, insurance links
        /// </summary>
        private void SetFlags()
        {
            if (isCitySelected)
            {
                if (isAreaAvailable)
                {
                    if (isAreaSelected)
                    {
                        isOnRoadPrice = true;
                    }
                }
                else
                {
                    isOnRoadPrice = true;
                }
            }
            // if city and area is not selected OR if city is selected & area is available but not selected
            //if ((!isCitySelected && !isAreaSelected) || (isCitySelected && isAreaAvailable && !isAreaSelected))
            if ((!isCitySelected) || (isCitySelected && isAreaAvailable && !isAreaSelected))
            {
                toShowOnRoadPriceButton = true;
            }
        }

        /// <summary>
        /// Created By: Sangram Nandkhile on 16-Mar-2016
        /// Summary   : To create Viewmodel for Version Page View
        /// </summary>
        private void FillViewModel()
        {
            try
            {
                if (cityId > 0 && areaId > 0 && variantId > 0)
                {
                    viewModel = new ModelPageVM(cityId, variantId, dealerId);
                    if (viewModel.Offers != null && viewModel.Offers.Count() > 0)
                    {
                        rptOffers.DataSource = viewModel.Offers;
                        rptOffers.DataBind();
                        isOfferAvailable = true;
                    }
                    if (viewModel.SecondaryDealers != null && viewModel.SecondaryDealerCount > 0)
                    {
                        rptSecondaryDealers.DataSource = viewModel.SecondaryDealers;
                        rptSecondaryDealers.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, Request.ServerVariables["URL"] + "FillViewModel");
                objErr.SendMail();
            }

        }


        #endregion
    }
}
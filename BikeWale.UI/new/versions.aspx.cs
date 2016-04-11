using Bikewale.BAL.BikeBooking;
using Bikewale.BAL.BikeData;
using Bikewale.BindViewModels.Webforms;
using Bikewale.Cache.BikeData;
using Bikewale.Cache.Core;
using Bikewale.Cache.Location;
using Bikewale.Common;
using Bikewale.Controls;
using Bikewale.DAL.BikeData;
using Bikewale.DAL.Location;
using Bikewale.DTO.Version;
using Bikewale.Entities.BikeBooking;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Location;
using Bikewale.Entities.PriceQuote;
using Bikewale.Interfaces.BikeBooking;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.Location;
using Bikewale.Interfaces.PriceQuote;
using Bikewale.Utility;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI.WebControls;

namespace Bikewale.New
{
    public class bikeModel : PageBase //inherited page base class to move viewstate from top of the html page to the end
    {
        #region Global Variables

        protected News_new ctrlNews;
        protected ExpertReviews ctrlExpertReviews;
        protected VideosControl ctrlVideos;
        protected UserReviewsList ctrlUserReviews;
        protected ModelGallery ctrlModelGallery;
        protected BikeModelPageEntity modelPage;
        protected VersionSpecifications bikeSpecs;
        //protected PQOnRoad pqOnRoad;
        protected PQOnRoadPrice pqOnRoad;
        protected string modelId = string.Empty;
        protected int variantId = 0;
        protected Repeater rptModelPhotos, rptNavigationPhoto, rptVarients, rptColor, rptOffers, rptCategory, rptVariants, rptDiscount, rptSecondaryDealers;
        protected String bikeName = String.Empty;
        protected String clientIP = string.Empty;
        protected uint cityId = 0;
        protected int areaId = 0;
        protected string cityName = string.Empty;
        protected string areaName = string.Empty;
        protected bool isCitySelected, isAreaSelected, isBikeWalePQ, isDiscontinued, isOnRoadPrice, toShowOnRoadPriceButton;
        protected AlternativeBikes ctrlAlternativeBikes;
        protected short reviewTabsCnt = 0;
        //Variable to Assing ACTIVE class
        protected bool isUserReviewActive = false, isExpertReviewActive = false, isNewsActive = false, isVideoActive = false;
        //Varible to Hide or show controlers
        protected bool isUserReviewZero = true, isExpertReviewZero = true, isNewsZero = true, isVideoZero = true, isAreaAvailable;
        protected bool isBookingAvailable, isOfferAvailable = false;
        static readonly string _PageNotFoundPath;
        static readonly string _bwHostUrl;
        protected static bool isManufacturer = false;
        protected DropDownList ddlVariant;
        protected string variantText = string.Empty;
        protected uint bookingAmt = 0;
        protected int urlVersionId = 0, grid1_size = 9, grid2_size = 3;
        protected string cssOffers = "noOffers", offerDivHide = "hide", price = string.Empty;
        //protected string viewbreakUpText = string.Empty;
        protected UInt32 onRoadPrice = 0;
        protected UInt32 totalDiscountedPrice = 0;
        protected IEnumerable<CityEntityBase> objCityList = null;
        protected IEnumerable<Bikewale.Entities.Location.AreaEntityBase> objAreaList = null;
        protected OtherVersionInfoEntity objSelectedVariant = null;
        protected ListView ListBox1;
        protected Label defaultVariant;
        protected HiddenField hdnVariant;
        protected IList<PQ_Price> priceList { get; set; }

        protected string dealerId = string.Empty;
        protected string pqId = string.Empty;
        protected string mpqQueryString = String.Empty;
        protected bool isDealerAssitance = false;
        protected uint campaignId, manufacturerId;
        protected string bikeModelName = string.Empty, bikeMakeName = string.Empty;

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
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {

            Trace.Warn("Trace 1 : DeviceDetection Start");
            //device detection
            // Modified By :Ashish Kamble on 5 Feb 2016
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
            try
            {
                if (!String.IsNullOrEmpty(modelId))
                {
                    Trace.Warn("Trace 5 : CheckCityCookie Start");
                    CheckCityCookie();
                    SetFlags();
                    Trace.Warn("Trace 6 : CheckCityCookie End");

                    if (hdnVariant.Value != "0")
                        variantId = Convert.ToInt32(hdnVariant.Value);

            #endregion

                    Trace.Warn("Trace 7 : FetchModelPageDetails Start");
                    FetchModelPageDetails();
                    Trace.Warn("Trace 8 : FetchModelPageDetails End");

                    if (modelPage != null && modelPage.ModelDetails != null && modelPage.ModelDetails.New)
                    {
                        Trace.Warn("Trace 9 : FetchOnRoadPrice Start");
                        FetchOnRoadPrice();
                        FillViewModel();
                        Trace.Warn("Trace 10 : FetchOnRoadPrice End");
                    }
                    BindPhotoRepeater();
                    clientIP = CommonOpn.GetClientIP();
                    LoadVariants();
                    Trace.Warn("Trace 18 : BindAlternativeBikeControl Start");
                    BindAlternativeBikeControl();
                    Trace.Warn("Trace 19 : BindAlternativeBikeControl End");

                    int _modelId;
                    Int32.TryParse(modelId, out _modelId);
                    ////news,videos,revews, user reviews
                    ctrlNews.TotalRecords = 3;
                    ctrlNews.ModelId = _modelId;

                    ctrlExpertReviews.TotalRecords = 3;
                    ctrlExpertReviews.ModelId = _modelId;
                    ctrlExpertReviews.MakeMaskingName = modelPage.ModelDetails.MakeBase.MaskingName.Trim();
                    ctrlExpertReviews.ModelMaskingName = modelPage.ModelDetails.MaskingName.Trim();

                    ctrlVideos.TotalRecords = 3;
                    ctrlVideos.ModelId = _modelId;
                    ctrlVideos.MakeId = modelPage.ModelDetails.MakeBase.MakeId;
                    ctrlVideos.MakeMaskingName = modelPage.ModelDetails.MakeBase.MaskingName.Trim();
                    ctrlVideos.ModelMaskingName = modelPage.ModelDetails.MaskingName.Trim();

                    ctrlUserReviews.ReviewCount = 4;
                    ctrlUserReviews.PageNo = 1;
                    ctrlUserReviews.PageSize = 4;
                    ctrlUserReviews.ModelId = _modelId;
                    ctrlUserReviews.Filter = Entities.UserReviews.FilterBy.MostRecent;

                    ToggleOfferDiv();
                    if (variantId != 0)
                    {
                        FetchVariantDetails(variantId);
                    }
                    Trace.Warn("Trace 20 : Page Load ends");
                    // Clear trailing query string -- added on 09-feb-2016 by Sangram
                    PropertyInfo isreadonly = typeof(System.Collections.Specialized.NameValueCollection).GetProperty("IsReadOnly", BindingFlags.Instance | BindingFlags.NonPublic);
                    if (isreadonly != null)
                    {
                        isreadonly.SetValue(this.Request.QueryString, false, null);
                        this.Request.QueryString.Clear();
                    }
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, Request.ServerVariables["URL"].ToString());
                objErr.SendMail();
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
                        //if ((isCitySelected && !isAreaAvailable))
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
                variantId = Convert.ToInt32(e.CommandName);
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
        private void LoadVariants()
        {
            try
            {
                if (modelPage != null)
                {
                    if (modelPage.ModelVersionSpecs != null && variantId <= 0)
                    {
                        variantId = Convert.ToInt32(modelPage.ModelVersionSpecs.BikeVersionId);
                    }

                    if (modelPage.ModelVersions != null && !modelPage.ModelDetails.Futuristic)
                    {
                        if (modelPage.ModelVersions.Count > 1)
                        {
                            if (modelPage.ModelVersionSpecs != null && modelPage.ModelVersionSpecs.BikeVersionId != 0)
                            {
                                var firstVer = modelPage.ModelVersions.Where(p => p.VersionId == variantId).FirstOrDefault();
                                if (firstVer != null)
                                    defaultVariant.Text = firstVer.VersionName;

                                if (variantId == 0)
                                    hdnVariant.Value = Convert.ToString(modelPage.ModelVersionSpecs.BikeVersionId);
                                else
                                    hdnVariant.Value = Convert.ToString(variantId);
                            }
                            else if (modelPage.ModelVersions.Count > 1)
                            {
                                var firstVer = modelPage.ModelVersions.FirstOrDefault();
                                if (firstVer != null)
                                    defaultVariant.Text = firstVer.VersionName;
                            }
                            rptVariants.DataSource = modelPage.ModelVersions;
                            rptVariants.DataBind();
                        }
                        else if (modelPage.ModelVersions.Count == 1)
                        {
                            var firstVer = modelPage.ModelVersions.FirstOrDefault();
                            if (firstVer != null)
                                variantText = firstVer.VersionName;
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, Request.ServerVariables["URL"] + " : LoadVariants");
                objErr.SendMail();
            }
        }

        private void BindAlternativeBikeControl()
        {
            ctrlAlternativeBikes.TopCount = 6;
            ctrlAlternativeBikes.PQSourceId = (int)PQSourceEnum.Desktop_ModelPage_Alternative;

            if (modelPage != null)
            {
                var modelVersions = modelPage.ModelVersions;
                if (modelVersions != null && modelVersions.Count > 0)
                {
                    ctrlAlternativeBikes.VersionId = modelVersions[0].VersionId;
                }
            }
        }



        private void BindPhotoRepeater()
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

                if (modelPage.ModelVersions != null && modelPage.ModelVersions.Count > 0)
                {
                    rptVarients.DataSource = modelPage.ModelVersions;
                    rptVarients.DataBind();
                }

                //bind model colors
                if (modelPage.ModelColors != null && modelPage.ModelColors.Count() > 0)
                {

                    rptColor.DataSource = modelPage.ModelColors;
                    rptColor.DataBind();
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
                Int32.TryParse(VersionIdStr, out variantId);
            }
            Trace.Warn("modelQuerystring 1 : ", modelQuerystring);
            try
            {
                if (!string.IsNullOrEmpty(modelQuerystring))
                {
                    if (modelQuerystring.Contains("/"))
                    {
                        modelQuerystring = modelQuerystring.Split('/')[0];
                    }

                    Trace.Warn("modelQuerystring 2 : ", modelQuerystring);
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
                            modelId = objResponse.ModelId.ToString();
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
                        if (!string.IsNullOrEmpty(modelId))
                        {
                            objCityList = FetchCityByModelId(modelId);
                            if (objCityList != null)
                            {
                                // If Model doesn't have current City then don't show it, Show Ex-showroom Mumbai
                                isCitySelected = objCityList.Any(p => p.CityId == cityId);
                                if (isCitySelected)
                                {
                                    cityName = locArray[1];
                                }
                            }
                        }
                    }
                    // locArray.Length = 4 Means City and area exists
                    if (locArray.Length > 3 && cityId != 0)
                    {
                        objAreaList = GetAreaForCityAndModel();
                        Int32.TryParse(locArray[2], out areaId);
                        if (objAreaList != null)
                        {
                            isAreaSelected = objAreaList.Any(p => p.AreaId == areaId);
                            if (isAreaAvailable)
                            {
                                areaName = locArray[3] + ",";
                            }
                        }
                    }
                }
            }
        }
        //static readonly string apiURL = "/api/model/details/?modelId={0}&variantId={1}";
        //static readonly string onRoadApi = "/api/OnRoadPrice/?cityId={0}&modelId={1}&clientIP={2}&sourceType={3}&areaId={4}";
        //static readonly string _requestType = "application/json";
        /// <summary>
        /// Author          :   Sangram Nandkhile
        /// Created Date    :   18 Nov 2015
        /// 
        /// </summary>
        private void FetchModelPageDetails()
        {
            try
            {
                if (!string.IsNullOrEmpty(modelId))
                {
                    using (IUnityContainer container = new UnityContainer())
                    {
                        container.RegisterType<IBikeModelsCacheRepository<int>, BikeModelsCacheRepository<BikeModelEntity, int>>()
                                 .RegisterType<IBikeModels<BikeModelEntity, int>, BikeModels<BikeModelEntity, int>>()
                                 .RegisterType<ICacheManager, MemcacheManager>();

                        var objCache = container.Resolve<IBikeModelsCacheRepository<int>>();
                        modelPage = objCache.GetModelPageDetails(Convert.ToInt16(modelId));
                        if (modelPage != null)
                        {
                            if (modelPage != null)
                            {
                                if (!modelPage.ModelDetails.Futuristic && modelPage.ModelVersionSpecs != null)
                                {
                                    price = Convert.ToString(modelPage.ModelDetails.MinPrice);
                                    if (variantId == 0 && cityId == 0)
                                    {
                                        variantId = Convert.ToInt32(modelPage.ModelVersionSpecs.BikeVersionId);
                                    }
                                    // Check it versionId passed through url exists in current model's versions
                                    else if (!modelPage.ModelVersions.Exists(p => p.VersionId == variantId))
                                    {
                                        variantId = Convert.ToInt32(modelPage.ModelVersionSpecs.BikeVersionId);
                                    }
                                }
                                if (!modelPage.ModelDetails.New)
                                    isDiscontinued = true;
                                if (modelPage.ModelDetails != null)
                                {
                                    if (modelPage.ModelDetails.ModelName != null)
                                        bikeModelName = modelPage.ModelDetails.ModelName;
                                    if (modelPage.ModelDetails.MakeBase != null)
                                        bikeMakeName = modelPage.ModelDetails.MakeBase.MakeName;
                                    bikeName = bikeMakeName + " " + bikeModelName;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, Request.ServerVariables["URL"] + "FetchModelPageDetails");
                objErr.SendMail();
            }
        }
        /// <summary>
        /// Author          :   Sangram Nandkhile
        /// Created Date    :   20 Nov 2015
        /// Description     :   Fetch On road price depending on City, Area and DealerPQ and BWPQ
        /// </summary>
        private void FetchOnRoadPrice()
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
                        variantId = Convert.ToInt32(pqOnRoad.PriceQuote.VersionId);
                        if (pqOnRoad.PriceQuote != null)
                        {
                            dealerId = Convert.ToString(pqOnRoad.PriceQuote.DealerId);
                            if (!String.IsNullOrEmpty(dealerId))
                            {
                                DealerAssistance dealerAssisteance = new DealerAssistance();
                                isDealerAssitance = dealerAssisteance.IsDealerAssistance(dealerId);
                            }
                            pqId = Convert.ToString(pqOnRoad.PriceQuote.PQId);
                        }
                        //PriceQuoteCookie.SavePQCookie(cityId.ToString(), pqId, Convert.ToString(areaId), Convert.ToString(variantId), dealerId);                                                
                        mpqQueryString = EncodingDecodingHelper.EncodeTo64(PriceQuoteQueryString.FormQueryString(cityId.ToString(), pqId, Convert.ToString(areaId), Convert.ToString(variantId), dealerId));
                        if (pqOnRoad.IsDealerPriceAvailable && pqOnRoad.DPQOutput != null && pqOnRoad.DPQOutput.Varients != null && pqOnRoad.DPQOutput.Varients.Count() > 0)
                        {
                            #region when dealer Price is Available

                            var selectedVariant = pqOnRoad.DPQOutput.Varients.Where(p => p.objVersion.VersionId == variantId).FirstOrDefault();
                            if (selectedVariant != null)
                            {
                                onRoadPrice = selectedVariant.OnRoadPrice;
                                price = onRoadPrice.ToString();
                                if (selectedVariant.PriceList != null)
                                {
                                    priceList = selectedVariant.PriceList;
                                    rptCategory.DataSource = selectedVariant.PriceList;
                                    rptCategory.DataBind();
                                    if (pqOnRoad.IsDiscount)
                                    {
                                        rptDiscount.DataSource = pqOnRoad.discountedPriceList;
                                        rptDiscount.DataBind();
                                    }
                                    totalDiscountedPrice = CommonModel.GetTotalDiscount(pqOnRoad.discountedPriceList);
                                }

                                bookingAmt = selectedVariant.BookingAmount;
                                if (bookingAmt > 0)
                                    isBookingAvailable = true;

                                if (pqOnRoad.discountedPriceList != null && pqOnRoad.discountedPriceList.Count > 0)
                                {
                                    price = Convert.ToString(onRoadPrice - totalDiscountedPrice);
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
                                    variantId = Convert.ToInt32(hdnVariant.Value);
                                    if (variantId != 0)
                                    {
                                        objSelectedVariant = pqOnRoad.BPQOutput.Varients.Where(p => p.VersionId == variantId).FirstOrDefault();
                                        if (objSelectedVariant != null)
                                            price = Convert.ToString(objSelectedVariant.OnRoadPrice);
                                    }
                                }
                                else
                                {
                                    objSelectedVariant = pqOnRoad.BPQOutput.Varients.FirstOrDefault();
                                    price = Convert.ToString(objSelectedVariant.OnRoadPrice);
                                }

                                campaignId = pqOnRoad.BPQOutput.CampaignId;
                                manufacturerId = pqOnRoad.BPQOutput.ManufacturerId;

                                isBikeWalePQ = true;
                                #endregion
                            }
                        }
                        // If DPQ or BWPQ Found change Version Pricing as well
                        if (modelPage.ModelVersions != null && modelPage.ModelVersions.Count > 0)
                        {
                            rptVarients.DataSource = modelPage.ModelVersions;
                            rptVarients.DataBind();
                        }
                    }
                    else // On road PriceQuote is Null so get price from the modelpage variants
                    {
                        if (variantId != 0)
                        {
                            var modelVersions = modelPage.ModelVersions.Where(p => p.VersionId == variantId).FirstOrDefault();
                            if (modelVersions != null)
                                price = Convert.ToString(modelVersions.Price);
                        }
                        else
                        {
                            price = Convert.ToString(modelPage.ModelDetails.MinPrice);
                        }
                    }
                }
                else
                {
                    var modelVersions = modelPage.ModelVersions.Where(p => p.VersionId == variantId).FirstOrDefault();

                    if (modelVersions != null)
                        price = Convert.ToString(modelVersions.Price);
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
                    objPQEntity.ModelId = Convert.ToUInt32(modelId);
                    objPQEntity.VersionId = Convert.ToUInt32(variantId);
                    objPQEntity.PQLeadId = Convert.ToUInt16(PQSourceEnum.Desktop_ModelPage);
                    objPQEntity.UTMA = Request.Cookies["__utma"] != null ? Request.Cookies["__utma"].Value : "";
                    objPQEntity.UTMZ = Request.Cookies["__utmz"] != null ? Request.Cookies["__utmz"].Value : "";
                    objPQEntity.DeviceId = Request.Cookies["BWC"] != null ? Request.Cookies["BWC"].Value : "";
                    PQOutputEntity objPQOutput = objDealer.ProcessPQ(objPQEntity);
                    if (variantId == 0)
                    {
                        if (objPQOutput != null)
                        {
                            variantId = Convert.ToInt32(objPQOutput.VersionId);
                        }
                    }
                    if (objPQOutput != null)
                    {
                        pqOnRoad = new PQOnRoadPrice();
                        pqOnRoad.PriceQuote = objPQOutput;
                        BikeModelEntity bikemodelEnt = objClient.GetById(Convert.ToInt32(modelId));
                        pqOnRoad.BikeDetails = bikemodelEnt;
                        string api = string.Empty;
                        if (objPQOutput != null && objPQOutput.PQId > 0)
                        {
                            bpqOutput = objPq.GetPriceQuoteById(objPQOutput.PQId);
                            bpqOutput.Varients = objPq.GetOtherVersionsPrices(objPQOutput.PQId);
                            if (bpqOutput != null)
                            {
                                pqOnRoad.BPQOutput = bpqOutput;
                            }
                            if (objPQOutput.DealerId != 0)
                            {
                                // call another api
                                PQ_QuotationEntity oblDealerPQ = null;
                                try
                                {
                                    api = String.Format("/api/DealerPriceQuote/GetDealerPriceQuote/?cityid={0}&versionid={1}&dealerid={2}", cityId, variantId, objPQOutput.DealerId);
                                    using (Utility.BWHttpClient objDealerPqClient = new Utility.BWHttpClient())
                                    {
                                        oblDealerPQ = objDealerPqClient.GetApiResponseSync<PQ_QuotationEntity>(Utility.APIHost.AB, Utility.BWConfiguration.Instance.APIRequestTypeJSON, api, oblDealerPQ);
                                        if (oblDealerPQ != null)
                                        {
                                            uint insuranceAmount = 0;
                                            foreach (var price in oblDealerPQ.PriceList)
                                            {
                                                pqOnRoad.IsInsuranceFree = Bikewale.Utility.DealerOfferHelper.HasFreeInsurance(objPQOutput.DealerId.ToString(), "", price.CategoryName, price.Price, ref insuranceAmount);
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
                                    }
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
            if (modelPage != null)
            {
                modelPage.Photos = null;
                modelPage.ModelColors = null;
                modelPage.ModelDesc = null;
                modelPage.ModelDetails = null;
                modelPage.ModelVersions = null;
                modelPage.ModelVersionSpecs = null;
                modelPage.UpcomingBike = null;
                modelPage = null;
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
        private void FetchVariantDetails(int versionId)
        {
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IBikeModelsRepository<BikeModelEntity, int>, BikeModelsRepository<BikeModelEntity, int>>();
                    IBikeModelsRepository<BikeModelEntity, int> objVersion = container.Resolve<IBikeModelsRepository<BikeModelEntity, int>>();
                    modelPage.ModelVersionSpecs = objVersion.MVSpecsFeatures(Convert.ToInt32(variantId));
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, Request.ServerVariables["URL"] + "FetchVariantDetails");
                objErr.SendMail();
            }
        }
        /// <summary>
        /// Author          :   Sangram Nandkhile
        /// Created Date    :   24 Nov 2015
        /// Description     :   Gets City Details by ModelId
        /// </summary>
        /// <param name="modelId">Model Id</param>
        private IEnumerable<CityEntityBase> FetchCityByModelId(string modelId)
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
                    cityList = objcity.GetPriceQuoteCities(Convert.ToUInt16(modelId));
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
                if (CommonOpn.CheckId(modelId))
                {
                    using (IUnityContainer container = new UnityContainer())
                    {
                        container.RegisterType<IDealerPriceQuote, DealerPriceQuote>()
                            .RegisterType<ICacheManager, MemcacheManager>()
                            .RegisterType<IAreaCacheRepository, AreaCacheRepository>();

                        IAreaCacheRepository objArea = container.Resolve<IAreaCacheRepository>();
                        areaList = objArea.GetAreaList(Convert.ToUInt32(modelId), Convert.ToUInt32(cityId));
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
        /// Creted By : Lucky Rathore
        /// Created on : 08 January 2016
        /// </summary>
        /// <param name="price">dicount price</param>
        /// <param name="CategoryId">category of specified discount</param>
        /// <returns>Price in string formate.</returns>
        protected string GetDiscountPrice(string price, UInt32 CategoryId)
        {
            if (price.CompareTo("0") == 0)
            {
                foreach (var priceListIterator in priceList)
                {
                    if (priceListIterator.CategoryId == CategoryId) return Bikewale.Utility.Format.FormatPrice(Convert.ToString(priceListIterator.Price));
                }
            }
            return Bikewale.Utility.Format.FormatPrice(Convert.ToString(price));
        }

        /// <summary>
        /// Created By: Sangram Nandkhile on 16-Mar-2016
        /// Summary   : To create Viewmodel for Version Page View
        /// </summary>
        private void FillViewModel()
        {
            try
            {
                if (cityId > 0 && variantId > 0)
                {
                    viewModel = new ModelPageVM(cityId, Convert.ToUInt32(variantId), Convert.ToUInt32(dealerId));
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
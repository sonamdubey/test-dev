using Bikewale.Cache.BikeData;
using Bikewale.Cache.Core;
using Bikewale.Common;
using Bikewale.DAL.BikeData;
using Bikewale.DTO.Model;
using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Cache.Core;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.UI.WebControls;
using Bikewale.Controls;
using Bikewale.DTO.PriceQuote;
using Bikewale.DTO.Version;
using Bikewale.DTO.PriceQuote.DealerPriceQuote;
using Newtonsoft.Json;
using Bikewale.DTO.PriceQuote.City;
using Bikewale.DTO.PriceQuote.Area;
using Bikewale.DTO.PriceQuote.BikeQuotation;
using System.Reflection;
using Bikewale.Mobile.PriceQuote;
using Bikewale.Utility;
using Bikewale.BAL.BikeBooking;
using Bikewale.Interfaces.BikeBooking;
using Bikewale.Entities.Location;
using Bikewale.Interfaces.Location;
using Bikewale.DAL.Location;
using Bikewale.BAL.BikeData;
using Bikewale.Service.Controllers.Model;
using Bikewale.Entities.CMS.Photos;
using Bikewale.Interfaces.PriceQuote;
using Bikewale.Entities.PriceQuote;
using Bikewale.Entities.BikeBooking;

namespace Bikewale.New
{
    public class bikeModel : System.Web.UI.Page
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
        protected int versionId = 0;
        protected Repeater rptModelPhotos, rptNavigationPhoto, rptVarients, rptColor, rptOffers, rptMoreOffers, rptCategory, rptVariants;
        protected String bikeName = String.Empty;
        protected String clientIP = string.Empty;
        protected int cityId = 0;
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
        protected int grid1_size = 9;
        protected int grid2_size = 3;
        protected string cssOffers = "noOffers";
        protected string offerDivHide = "hide";
        protected string price = string.Empty;
        protected string viewbreakUpText = string.Empty;
        protected UInt32 onRoadPrice = 0;
        protected List<CityEntityBase> objCityList = null;
        protected List<Bikewale.Entities.Location.AreaEntityBase> objAreaList = null;
        protected OtherVersionInfoEntity objSelectedVariant = null;
        protected ListView ListBox1;
        protected Label defaultVariant;
        protected HiddenField hdnVariant;


        protected string dealerId = string.Empty;
        protected string pqId = string.Empty;
        #endregion

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

        #region Events

        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //device detection
            DeviceDetection dd = new DeviceDetection(Request.ServerVariables["HTTP_X_REWRITE_URL"]);
            dd.DetectDevice();

            #region Do Not change the sequence
            ParseQueryString();
            CheckCityCookie();

            //if (!string.IsNullOrEmpty(ddlVariant.SelectedValue) && ddlVariant.SelectedValue != "0")
            if (hdnVariant.Value != "0")
                variantId = Convert.ToInt32(hdnVariant.Value);
            #endregion

            FetchModelPageDetails();
            if (!IsPostBack)
            {
                #region Do not change the sequence
                BindPhotoRepeater();
                clientIP = CommonOpn.GetClientIP();
                LoadVariants();
                #endregion
            }
            else
            {
                if (modelPage.ModelVersions != null && modelPage.ModelVersions.Count > 0)
                {
                    rptVarients.DataSource = modelPage.ModelVersions;
                    rptVarients.DataBind();
                }
            }
            SetFlags();
            BindAlternativeBikeControl();
            // Set BikeName
            if (modelPage.ModelDetails != null)
                bikeName = modelPage.ModelDetails.MakeBase.MakeName + ' ' + modelPage.ModelDetails.ModelName;
            if (modelPage.ModelDetails.New)
            {
                FetchOnRoadPrice();
            }
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

            ctrlUserReviews.ReviewCount = 4;
            ctrlUserReviews.PageNo = 1;
            ctrlUserReviews.PageSize = 4;
            ctrlUserReviews.ModelId = _modelId;
            ctrlUserReviews.Filter = Entities.UserReviews.FilterBy.MostRecent;

            ToggleOfferDiv();
        }

        /// <summary>
        /// Author          :   Sangram Nandkhile
        /// Created Date    :   18 Nov 2015
        /// Description     :   To change price and Minspecs when variant changes
        /// </summary>
        public void ddlVariant_SelectedIndexChanged(object sender, EventArgs e)
        {
            variantId = Convert.ToInt32(ddlVariant.SelectedValue);
            FetchVariantDetails(variantId);
        }

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
                        //if ((isCitySelected && !isAreaAvailable))
                        if (isOnRoadPrice)
                            lblExOn.Text = "On-road price";

                        if (pqOnRoad.IsDealerPriceAvailable && pqOnRoad.DPQOutput != null && pqOnRoad.DPQOutput.Varients != null)
                        {
                            var selecteVersionList = pqOnRoad.DPQOutput.Varients.Where(p => Convert.ToString(p.objVersion.VersionId) == hdn.Value);
                            if (selecteVersionList != null && selecteVersionList.Count() > 0)
                                currentTextBox.Text = Bikewale.Utility.Format.FormatPrice(Convert.ToString(selecteVersionList.First().OnRoadPrice));
                        }
                        else if (pqOnRoad.BPQOutput != null && pqOnRoad.BPQOutput.Varients != null)
                        {
                            var selected = pqOnRoad.BPQOutput.Varients.Where(p => Convert.ToString(p.VersionId) == hdn.Value).First();
                            if (selected != null)
                                currentTextBox.Text = Bikewale.Utility.Format.FormatPrice(Convert.ToString(selected.OnRoadPrice));
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"] + MethodBase.GetCurrentMethod().Name);
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
            if (modelPage != null)
            {
                if (modelPage.ModelVersions != null && !modelPage.ModelDetails.Futuristic)
                {
                    if (modelPage.ModelVersions != null && modelPage.ModelVersions.Count > 1)
                    {
                        if (modelPage.ModelVersionSpecs != null && modelPage.ModelVersionSpecs.BikeVersionId != 0)
                        {
                            defaultVariant.Text = modelPage.ModelVersions.Where(p => p.VersionId == (int)modelPage.ModelVersionSpecs.BikeVersionId).First().VersionName;
                            hdnVariant.Value = Convert.ToString(modelPage.ModelVersionSpecs.BikeVersionId);
                        }
                        else if (modelPage.ModelVersions.Count > 1)
                        {
                            defaultVariant.Text = modelPage.ModelVersions.First().VersionName;
                        }
                        rptVariants.DataSource = modelPage.ModelVersions;
                        rptVariants.DataBind();
                    }
                    else if (modelPage.ModelVersions.Count == 1)
                    {
                        variantText = modelPage.ModelVersions.First().VersionName;
                    }

                }
            }
        }

        private void BindAlternativeBikeControl()
        {
            ctrlAlternativeBikes.TopCount = 6;

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
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"] + System.Reflection.MethodBase.GetCurrentMethod().Name);
                objErr.SendMail();

                Response.Redirect("/new/", true);
            }
            finally
            {
                if (!string.IsNullOrEmpty(modelQuerystring))
                {
                    if (objResponse != null)
                    {
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
                            Response.Redirect(CommonOpn.AppPath + "pageNotFound.aspx", true);
                        }
                    }
                    else
                    {
                        Response.Redirect(CommonOpn.AppPath + "pageNotFound.aspx", true);
                    }
                }
                else
                {
                    Response.Redirect(CommonOpn.AppPath + "pageNotFound.aspx", true);
                }
            }
        }

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
                        cityId = Convert.ToInt16(locArray[0]);
                        objCityList = FetchCityByModelId(modelId);

                        // If Model doesn't have current City then don't show it, Show Ex-showroom Mumbai
                        if (objCityList != null && !objCityList.Any(p => p.CityId == cityId))
                        {
                            //cityId = 0;
                        }
                        else
                        {
                            cityName = locArray[1];
                            isCitySelected = true;
                        }
                        if (GetAreaForCityAndModel() != null)
                        {
                            //isAreaAvailable = true;
                        }
                    }
                    if (locArray.Length > 3 && cityId != 0)
                    {
                        areaId = Convert.ToInt32(locArray[2]);
                        objAreaList = GetAreaForCityAndModel();
                        if (objAreaList != null)
                        {
                            if (!objAreaList.Any(p => p.AreaId == areaId))
                            {
                                areaId = 0;
                            }
                            else
                            {
                                areaName = locArray[3] + ",";
                                isAreaSelected = true;
                            }
                        }
                        else
                        {
                            areaId = 0;
                        }
                        objAreaList = GetAreaForCityAndModel();
                    }
                }
            }
        }

        static readonly string apiURL = "/api/model/details/?modelId={0}&variantId={1}";
        static readonly string onRoadApi = "/api/OnRoadPrice/?cityId={0}&modelId={1}&clientIP={2}&sourceType={3}&areaId={4}";
        static readonly string _requestType = "application/json";
        /// <summary>
        /// Author          :   Sangram Nandkhile
        /// Created Date    :   18 Nov 2015
        /// 
        /// </summary>
        private void FetchModelPageDetails()
        {
            try
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
                                if (variantId == 0)
                                    variantId = Convert.ToInt32(modelPage.ModelVersionSpecs.BikeVersionId);
                            }
                            if (!modelPage.ModelDetails.New)
                                isDiscontinued = true;

                            //string jsonModel = JsonConvert.SerializeObject(modelPage);
                            //ViewState["modelPage"] = jsonModel;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"] + MethodBase.GetCurrentMethod().Name);
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
                if (cityId !=0)
                {
                    pqOnRoad = GetOnRoadPrice();
                    // Set Pricequote Cookie
                    if (pqOnRoad != null)
                    {
                        if (pqOnRoad.PriceQuote != null)
                        {
                            dealerId = Convert.ToString(pqOnRoad.PriceQuote.DealerId);
                            pqId = Convert.ToString(pqOnRoad.PriceQuote.PQId);
                        }
                        PriceQuoteCookie.SavePQCookie(cityId.ToString(), pqId, Convert.ToString(areaId), Convert.ToString(variantId), dealerId);
                        if (pqOnRoad.IsDealerPriceAvailable && pqOnRoad.DPQOutput != null && pqOnRoad.DPQOutput.Varients != null && pqOnRoad.DPQOutput.Varients.Count() > 0)
                        {
                            #region when dealer Price is Available

                            var selectedVariant = pqOnRoad.DPQOutput.Varients.Where(p => p.objVersion.VersionId == variantId).FirstOrDefault();
                            if (selectedVariant != null)
                            {
                                onRoadPrice = selectedVariant.OnRoadPrice;
                                price = onRoadPrice.ToString();
                                if (pqOnRoad.DPQOutput.objOffers!= null && pqOnRoad.DPQOutput.objOffers.Count > 0)
                                {
                                    IEnumerable<OfferEntity> moreOffers = null;
                                    rptOffers.DataSource = pqOnRoad.DPQOutput.objOffers.Take<OfferEntity>(2);
                                    rptOffers.DataBind();
                                    if (pqOnRoad.DPQOutput.objOffers.Count > 2)
                                    {
                                        moreOffers = pqOnRoad.DPQOutput.objOffers.Skip(2).Take<OfferEntity>(pqOnRoad.DPQOutput.objOffers.Count - 2);
                                        rptMoreOffers.DataSource = moreOffers;
                                        rptMoreOffers.DataBind();
                                    }
                                    isOfferAvailable = true;
                                }
                                if (selectedVariant.PriceList != null)
                                {
                                    rptCategory.DataSource = selectedVariant.PriceList;
                                    rptCategory.DataBind();

                                    // String operation
                                    viewbreakUpText = "(";
                                    foreach (var text in selectedVariant.PriceList)
                                    {
                                        viewbreakUpText += " + " + text.CategoryName;
                                    }
                                    if (viewbreakUpText.Length > 2)
                                    {
                                        viewbreakUpText = viewbreakUpText.Remove(2, 1);
                                    }
                                    viewbreakUpText += ")";
                                }
                                // String operation ends
                                bookingAmt = selectedVariant.BookingAmount;
                                if (bookingAmt > 0)
                                    isBookingAvailable = true;

                                if (pqOnRoad.IsInsuranceFree && pqOnRoad.InsuranceAmount > 0)
                                {
                                    price = Convert.ToString(onRoadPrice - pqOnRoad.InsuranceAmount);
                                }
                            }
                            #endregion
                        }
                        else
                        {
                            if (pqOnRoad.BPQOutput != null && pqOnRoad.BPQOutput.Varients!= null)
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
                if(string.IsNullOrEmpty(errorParams))
                    errorParams = "=== modelpage ===" + Newtonsoft.Json.JsonConvert.SerializeObject(modelPage);
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"] + "-" + MethodBase.GetCurrentMethod().Name + " ===== parameters ========= " + errorParams);
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
                    objPQEntity.UTMA = Request.Cookies["__utma"]!=null ? Request.Cookies["__utma"].Value : "";
                    objPQEntity.UTMZ = Request.Cookies["__utmz"] != null ? Request.Cookies["__utmz"].Value : "";
                    objPQEntity.DeviceId = Request.Cookies["BWC"] != null ? Request.Cookies["BWC"].Value : "";
                    PQOutputEntity objPQOutput = objDealer.ProcessPQ(objPQEntity);
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
                                            pqOnRoad.InsuranceAmount = insuranceAmount;
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"] + MethodBase.GetCurrentMethod().Name + "-" + api);
                                    objErr.SendMail();
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"] + MethodBase.GetCurrentMethod().Name);
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
                    modelPage.ModelVersionSpecs = objVersion.MVSpecsFeatures(Convert.ToInt16(variantId));
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"] + MethodBase.GetCurrentMethod().Name);
                objErr.SendMail();
            }
        }
        /// <summary>
        /// Author          :   Sangram Nandkhile
        /// Created Date    :   24 Nov 2015
        /// Description     :   Gets City Details by ModelId
        /// </summary>
        /// <param name="modelId">Model Id</param>
        private List<CityEntityBase> FetchCityByModelId(string modelId)
        {
            List<CityEntityBase> cityList = null;
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<ICity, CityRepository>();
                    ICity objcity = container.Resolve<CityRepository>();
                    cityList = objcity.GetPriceQuoteCities(Convert.ToUInt16(modelId));
                    return cityList;
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"] + MethodBase.GetCurrentMethod().Name);
                objErr.SendMail();
            }
            return cityList;
        }

        /// <summary>
        /// Author          :   Sangram Nandkhile
        /// Created Date    :   24 Nov 2015
        /// Description     :   Get List of Area depending on City and Model Id
        /// </summary>
        private List<Bikewale.Entities.Location.AreaEntityBase> GetAreaForCityAndModel()
        {
            List<Bikewale.Entities.Location.AreaEntityBase> areaList = null;
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IDealerPriceQuote, DealerPriceQuote>();
                    IDealerPriceQuote objDealer = container.Resolve<IDealerPriceQuote>();
                    areaList = objDealer.GetAreaList(Convert.ToUInt32(modelId), Convert.ToUInt32(cityId));
                    if (areaList != null && areaList.Count > 0)
                        isAreaAvailable = true;
                    return areaList;
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"] + MethodBase.GetCurrentMethod().Name);
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
            if(isCitySelected)
            {
                if(isAreaAvailable)
                {
                    if(isAreaSelected)
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
            if ((!isCitySelected && !isAreaSelected) || (isCitySelected && isAreaAvailable && !isAreaSelected))
            {
                toShowOnRoadPriceButton = true;
            }
        }
        #endregion
    }
}
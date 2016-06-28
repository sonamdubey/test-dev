using Bikewale.BAL.BikeBooking;
using Bikewale.BAL.BikeData;
using Bikewale.BindViewModels.Webforms;
using Bikewale.Cache.BikeData;
using Bikewale.Cache.Core;
using Bikewale.Cache.Location;
using Bikewale.Common;
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
using Bikewale.m.controls;
using Bikewale.Mobile.controls;
using Bikewale.Mobile.Controls;
using Bikewale.Utility;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Web.UI.WebControls;

namespace Bikewale.Mobile.New
{
    /// <summary>
    /// Created By : Ashish G. Kamble on 9 Sept 2015    
    /// Modified By : Lucky Rathore on 09 May 2016
    /// Description : modelImage declare used for bike model default image url.
    /// </summary>
    public class NewBikeModels : PageBase //inherited page base class to move viewstate from top of the html page to the end
    {
        // Register controls
        protected NewNewsWidget ctrlNews;
        protected NewExpertReviewsWidget ctrlExpertReviews;
        protected NewVideosWidget ctrlVideos;
        protected NewUserReviewList ctrlUserReviews;
        protected ModelGallery ctrlModelGallery;
        protected BikeModelPageEntity modelPage;
        protected VersionSpecifications bikeSpecs;
        protected PQOnRoadPrice pqOnRoad;
        protected Repeater rptModelPhotos, rptNavigationPhoto, rptVarients, rptColors, rptOffers, rptVariants, rptSecondaryDealers;
        protected string cityName = string.Empty, mpqQueryString = string.Empty, areaName = string.Empty, variantText = string.Empty, pqId = string.Empty, bikeName = string.Empty, bikeModelName = string.Empty, bikeMakeName = string.Empty, modelImage = string.Empty;
        protected String clientIP = CommonOpn.GetClientIP();
        protected bool isCitySelected, isAreaSelected, isBikeWalePQ, isDiscontinued, isOnRoadPrice, toShowOnRoadPriceButton;
        //Varible to Hide or show controlers
        protected bool isUserReviewZero = true, isExpertReviewZero = true, isNewsZero = true, isVideoZero = true, isAreaAvailable, isDealerPQ, isDealerAssitance, isBookingAvailable, isOfferAvailable;
        protected bool isUserReviewActive, isExpertReviewActive, isNewsActive, isVideoActive;
        protected NewAlternativeBikes ctrlAlternativeBikes;
        protected short reviewTabsCnt;
        //Variable to Assing ACTIVE class

        static readonly string _PageNotFoundPath;
        static readonly string _bwHostUrl;
        protected static bool isManufacturer;
        //protected string cssOffers = "noOffers", offerDivHide = "hide"; protected int grid1_size = 9, grid2_size = 3;
        protected uint onRoadPrice, totalDiscountedPrice, price, bookingAmt, cityId, campaignId, manufacturerId, modelId, versionId, areaId, dealerId;
        protected IEnumerable<CityEntityBase> objCityList = null;
        protected IEnumerable<Bikewale.Entities.Location.AreaEntityBase> objAreaList = null;
        protected OtherVersionInfoEntity objSelectedVariant = null;
        protected Label defaultVariant;
        protected HiddenField hdnVariant;
        protected MPriceInTopCities ctrlTopCityPrices;

        #region Subscription model variables
        protected ModelPageVM viewModel = null;

        #endregion Subscription model ends

        #region Events
        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
            //ddlVariant.SelectedIndexChanged += new EventHandler(ddlVariant_SelectedIndexChanged);
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            // Do not change the sequence of the function calls
            Trace.Warn("Trace 3 : ParseQueryString Start");
            ParseQueryString();
            Trace.Warn("Trace 4 : ParseQueryString End");
            try
            {
                if (!string.IsNullOrEmpty(modelId.ToString()))
                {
                    Trace.Warn("Trace 5 : CheckCityCookie Start");
                    CheckCityCookie();
                    SetFlags();
                    Trace.Warn("Trace 6 : CheckCityCookie End");
                    if (hdnVariant.Value != "0")
                        versionId = Convert.ToUInt32(hdnVariant.Value);
                    Trace.Warn("Trace 7 : FetchModelPageDetails Start");
                    FetchModelPageDetails();
                    Trace.Warn("Trace 8 : FetchModelPageDetails End");
                    if (modelPage != null && modelPage.ModelDetails != null && modelPage.ModelDetails.New)
                    {

                        Trace.Warn("Trace 9 : FetchOnRoadPrice Start");
                        FetchOnRoadPrice();
                        Trace.Warn("Trace 10 : FetchOnRoadPrice End");
                        FillViewModel();
                    }

                    Trace.Warn("Trace 11 : !IsPostBack");
                    #region Do not change the sequence of these functions
                    Trace.Warn("Trace 12 : BindRepeaters Start");
                    BindRepeaters();
                    Trace.Warn("Trace 13 : BindRepeaters End");
                    //BindModelGallery();
                    Trace.Warn("Trace 14 : BindAlternativeBikeControl Start");
                    BindAlternativeBikeControl();
                    Trace.Warn("Trace 15 : BindAlternativeBikeControl End");
                    Trace.Warn("Trace 16 : GetClientIP Start");
                    Trace.Warn("Trace 17 : GetClientIP End");
                    Trace.Warn("Trace 18 : LoadVariants Start");
                    LoadVariants();
                    Trace.Warn("Trace 19 : LoadVariants End");
                    #endregion

                    ////news,videos,revews, user reviews
                    ctrlNews.TotalRecords = 3;
                    ctrlNews.ModelId = Convert.ToInt32(modelId);
                    ctrlNews.WidgetTitle = bikeName;

                    ctrlExpertReviews.TotalRecords = 2;
                    ctrlExpertReviews.ModelId = Convert.ToInt32(modelId);

                    ctrlVideos.TotalRecords = 3;
                    ctrlVideos.MakeMaskingName = modelPage.ModelDetails.MakeBase.MaskingName.Trim();
                    ctrlVideos.ModelMaskingName = modelPage.ModelDetails.MaskingName.Trim();
                    ctrlVideos.ModelId = Convert.ToInt32(modelId);

                    ctrlUserReviews.ReviewCount = 2;
                    ctrlUserReviews.PageNo = 1;
                    ctrlUserReviews.PageSize = 2;
                    ctrlUserReviews.ModelId = Convert.ToInt32(modelId);
                    ctrlUserReviews.Filter = Entities.UserReviews.FilterBy.MostRecent;

                    ctrlExpertReviews.MakeMaskingName = modelPage.ModelDetails.MakeBase.MaskingName.Trim();
                    ctrlExpertReviews.ModelMaskingName = modelPage.ModelDetails.MaskingName.Trim();
                    Trace.Warn("Trace 20 : Page Load ends");

                    if (modelPage.ModelVersions != null && modelPage.ModelVersions.Count > 0)
                    {
                        rptVarients.DataSource = modelPage.ModelVersions;
                        rptVarients.DataBind();
                    }
                    // Clear trailing query string -- added on 09-feb-2016 by Sangram
                    PropertyInfo isreadonly = typeof(System.Collections.Specialized.NameValueCollection).GetProperty("IsReadOnly", BindingFlags.Instance | BindingFlags.NonPublic);
                    if (isreadonly != null)
                    {
                        isreadonly.SetValue(this.Request.QueryString, false, null);
                        this.Request.QueryString.Clear();
                    }

                    if (!modelPage.ModelDetails.Futuristic || modelPage.ModelDetails.New)
                        ctrlTopCityPrices.ModelId = Convert.ToUInt32(modelId);
                    else ctrlTopCityPrices.ModelId = 0;

                    ctrlTopCityPrices.IsDiscontinued = isDiscontinued;
                    ctrlTopCityPrices.TopCount = 8;
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }

        }


        protected void btnVariant_Command(object sender, CommandEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.CommandName))
            {
                versionId = Convert.ToUInt32(e.CommandName);
                FetchVariantDetails(versionId);
                defaultVariant.Text = Convert.ToString(e.CommandArgument);
            }
        }

        protected void rptVarients_ItemDataBound2(object sender, RepeaterItemEventArgs e)
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
                        if (isOnRoadPrice)
                            lblExOn.Text = "On-road price";
                        if (pqOnRoad.IsDealerPriceAvailable && pqOnRoad.DPQOutput != null && pqOnRoad.DPQOutput.Varients != null)
                        {
                            var selecteVersionList = pqOnRoad.DPQOutput.Varients.Where(p => Convert.ToString(p.objVersion.VersionId) == hdn.Value);
                            if (selecteVersionList != null && selecteVersionList.Count() > 0)
                                currentTextBox.Text = Bikewale.Utility.Format.FormatPrice(Convert.ToString(selecteVersionList.First().OnRoadPrice - totalDiscountedPrice));
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
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, Request.ServerVariables["URL"] + MethodBase.GetCurrentMethod().Name);
                objErr.SendMail();
            }
        }

        #endregion

        #region methods

        #endregion

        static NewBikeModels()
        {
            _PageNotFoundPath = Bikewale.Common.CommonOpn.AppPath + "pageNotFound.aspx";
            _bwHostUrl = ConfigurationManager.AppSettings["bwHostUrl"];
        }


        //private void BindModelGallery()
        //{
        //    if (modelPage != null)
        //    {
        //        List<ModelImage> photos = modelPage.Photos;

        //        if (photos != null && photos.Count > 0)
        //        {
        //            photos.Insert(0, new ModelImage()
        //            {
        //                HostUrl = modelPage.ModelDetails.HostUrl,
        //                OriginalImgPath = modelPage.ModelDetails.OriginalImagePath,
        //                ImageCategory = bikeName,
        //            });
        //        }
        //    }
        //}

        private void BindAlternativeBikeControl()
        {
            ctrlAlternativeBikes.TopCount = 6;

            if (modelPage.ModelVersions != null && modelPage.ModelVersions.Count > 0)
            {
                ctrlAlternativeBikes.VersionId = modelPage.ModelVersions[0].VersionId;
                ctrlAlternativeBikes.PQSourceId = (int)PQSourceEnum.Mobile_ModelPage_Alternative;
                ctrlAlternativeBikes.WidgetTitle = bikeName;
            }
        }

        /// <summary>
        /// Function to bind the photos album
        /// </summary>
        private void BindRepeaters()
        {

            if (modelPage != null)
            {
                if (modelPage.Photos != null && modelPage.Photos.Count > 0)
                {
                    rptModelPhotos.DataSource = modelPage.Photos;
                    rptModelPhotos.DataBind();

                    ctrlModelGallery.bikeName = bikeName;
                    ctrlModelGallery.modelId = Convert.ToInt32(modelId);
                    ctrlModelGallery.Photos = modelPage.Photos;
                }

                if (modelPage.ModelVersions != null && modelPage.ModelVersions.Count > 0)
                {
                    rptVarients.DataSource = modelPage.ModelVersions;
                    rptVarients.DataBind();
                }

                if (modelPage.ModelColors != null && modelPage.ModelColors.Count() > 0)
                {
                    rptColors.DataSource = modelPage.ModelColors;
                    rptColors.DataBind();
                }
            }
        }


        /// <summary>
        /// Function to get the required parameters from the query string.
        /// Desc: It sets variantId and modelId
        /// </summary>
        private void ParseQueryString()
        {
            ModelMaskingResponse objResponse = null;
            try
            {
                if (!string.IsNullOrEmpty(Request.QueryString["model"]))
                {
                    string VersionIdStr = Request.QueryString["vid"];
                    if (!string.IsNullOrEmpty(VersionIdStr))
                    {
                        UInt32.TryParse(VersionIdStr, out versionId);
                    }

                    using (IUnityContainer container = new UnityContainer())
                    {
                        container.RegisterType<IBikeMaskingCacheRepository<BikeModelEntity, int>, BikeModelMaskingCache<BikeModelEntity, int>>()
                                 .RegisterType<ICacheManager, MemcacheManager>()
                                 .RegisterType<IBikeModelsRepository<BikeModelEntity, int>, BikeModelsRepository<BikeModelEntity, int>>();
                        var objCache = container.Resolve<IBikeMaskingCacheRepository<BikeModelEntity, int>>();

                        objResponse = objCache.GetModelMaskingResponse(Request.QueryString["model"]);
                    }
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, Request.ServerVariables["URL"] + " : FetchModelPageDetails");
                objErr.SendMail();

                Response.Redirect("/m/new/", true);
            }
            finally
            {
                if (!string.IsNullOrEmpty(Request.QueryString["model"]))
                {
                    if (objResponse != null)
                    {
                        if (objResponse.StatusCode == 200)
                        {
                            modelId = objResponse.ModelId;
                        }
                        else if (objResponse.StatusCode == 301)
                        {
                            //redirect permanent to new page 
                            CommonOpn.RedirectPermanent(Request.RawUrl.Replace(Request.QueryString["model"], objResponse.MaskingName));
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

        /// <summary>
        /// Summary     :  Set isCitySelected, isAreaSelected
        /// Modified by :   Sumit Kate on 04 Jan 2016
        /// Description :   Replaced the Convert.ToXXX with XXX.TryParse method
        /// 
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
                            if (objCityList != null && objCityList.Count() > 0)
                            {
                                // If Model doesn't have current City then don't show it, Show Ex-showroom Mumbai
                                if (objCityList.Any(p => p.CityId == cityId))
                                {
                                    cityName = locArray[1];
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
                        UInt32.TryParse(locArray[2], out areaId);
                        if (objAreaList != null)
                        {
                            if (objAreaList.Any(p => p.AreaId == areaId))
                            {
                                areaName = locArray[3] + ",";
                                isAreaSelected = true;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Author          :   Sangram Nandkhile
        /// Created Date    :   27 Nov 2015
        /// Description     :   To Load Specs for each variant
        /// </summary>
        private void LoadVariants()
        {
            try
            {
                if (modelPage != null)
                {
                    if (modelPage.ModelVersionSpecs != null && versionId <= 0)
                    {
                        versionId = (modelPage.ModelVersionSpecs.BikeVersionId);
                    }

                    if (modelPage.ModelVersions != null && !modelPage.ModelDetails.Futuristic)
                    {
                        if (modelPage.ModelVersions.Count > 1)
                        {
                            if (modelPage.ModelVersionSpecs != null && modelPage.ModelVersionSpecs.BikeVersionId != 0)
                            {
                                var firstVer = modelPage.ModelVersions.Where(p => p.VersionId == versionId).FirstOrDefault();
                                if (firstVer != null)
                                    defaultVariant.Text = firstVer.VersionName;

                                if (versionId == 0)
                                    hdnVariant.Value = Convert.ToString(modelPage.ModelVersionSpecs.BikeVersionId);
                                else
                                    hdnVariant.Value = Convert.ToString(versionId);
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

        /// <summary>
        /// Author          :   Sangram Nandkhile
        /// Created Date    :   27 Nov 2015
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
                        if (!modelPage.ModelDetails.Futuristic && modelPage.ModelVersionSpecs != null)
                        {
                            price = Convert.ToUInt32(modelPage.ModelDetails.MinPrice);
                            if (versionId == 0 && cityId == 0)
                            {
                                versionId = modelPage.ModelVersionSpecs.BikeVersionId;
                            }
                            // Check it versionId passed through url exists in current model's versions
                            else if (!modelPage.ModelVersions.Exists(p => p.VersionId == versionId))
                            {
                                versionId = modelPage.ModelVersionSpecs.BikeVersionId;
                            }
                        }
                        if (!modelPage.ModelDetails.New)
                            isDiscontinued = true;

                        if (modelPage.ModelDetails != null)
                        {
                            bikeModelName = modelPage.ModelDetails.ModelName;
                            if (modelPage.ModelDetails.MakeBase != null)
                            {
                                bikeMakeName = modelPage.ModelDetails.MakeBase.MakeName;
                            }
                            bikeName = string.Format("{0} {1}", bikeMakeName, bikeModelName);
                            modelImage = Utility.Image.GetPathToShowImages(modelPage.ModelDetails.OriginalImagePath, modelPage.ModelDetails.HostUrl, Bikewale.Utility.ImageSize._476x268);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, Request.ServerVariables["URL"] + MethodBase.GetCurrentMethod().Name);
                objErr.SendMail();
            }
        }
        /// <summary>
        /// Author          :   Sangram Nandkhile
        /// Created Date    :   27 Nov 2015
        /// Description     :   Fetch On road price depending on City, Area and DealerPQ and BWPQ
        /// Modified By     :   Sushil Kumar on 19th April 2016
        /// Description     :   Removed repeater binding for rptCategory and rptDiscount as view breakup popup removed 
        /// Modified by     :   Sumit Kate on 29 Apr 2016
        /// Description     :   Removed the Dealer Assistance code as it is not used.
        /// </summary>
        private void FetchOnRoadPrice()
        {
            try
            {
                if (cityId != 0)
                {
                    pqOnRoad = GetOnRoadPrice();
                    // Set Pricequote Cookie
                    if (pqOnRoad != null)
                    {
                        versionId = pqOnRoad.PriceQuote.VersionId;
                        if (pqOnRoad.PriceQuote != null)
                        {
                            dealerId = pqOnRoad.PriceQuote.DealerId;
                            pqId = Convert.ToString(pqOnRoad.PriceQuote.PQId);
                        }
                        //PriceQuoteCookie.SavePQCookie(cityId.ToString(), pqId, Convert.ToString(areaId), Convert.ToString(variantId), dealerId);

                        mpqQueryString = EncodingDecodingHelper.EncodeTo64(PriceQuoteQueryString.FormQueryString(cityId.ToString(), pqId, areaId.ToString(), versionId.ToString(), dealerId.ToString()));
                        if (pqOnRoad.IsDealerPriceAvailable && pqOnRoad.DPQOutput != null && pqOnRoad.DPQOutput.Varients != null && pqOnRoad.DPQOutput.Varients.Count() > 0)
                        {
                            #region when dealer Price is Available
                            // Select Variant for which details need to be shown
                            var selectedVariant = pqOnRoad.DPQOutput.Varients.Where(p => p.objVersion.VersionId == versionId).FirstOrDefault();
                            if (selectedVariant != null)
                            {
                                isDealerPQ = true;
                                onRoadPrice = selectedVariant.OnRoadPrice;
                                price = onRoadPrice;
                                if (pqOnRoad.DPQOutput.objOffers != null && pqOnRoad.DPQOutput.objOffers.Count > 0)
                                {
                                    rptOffers.DataSource = pqOnRoad.DPQOutput.objOffers;
                                    rptOffers.DataBind();
                                    isOfferAvailable = true;
                                }
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
                            #region BikeWale PQ
                            if (pqOnRoad.BPQOutput != null && pqOnRoad.BPQOutput.Varients != null)
                            {
                                if (hdnVariant.Value != "0")
                                {
                                    versionId = Convert.ToUInt32(hdnVariant.Value);
                                    if (versionId != 0)
                                    {

                                        objSelectedVariant = pqOnRoad.BPQOutput.Varients.Where(p => p.VersionId == versionId).FirstOrDefault();
                                        price = Convert.ToUInt32(objSelectedVariant.OnRoadPrice);
                                    }
                                }
                                else
                                {
                                    objSelectedVariant = pqOnRoad.BPQOutput.Varients.FirstOrDefault();
                                    price = Convert.ToUInt32(objSelectedVariant.OnRoadPrice);
                                }
                                isBikeWalePQ = true;

                                campaignId = pqOnRoad.BPQOutput.CampaignId;
                                manufacturerId = pqOnRoad.BPQOutput.ManufacturerId;
                            }
                            #endregion
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
                        if (versionId != 0)
                        {
                            var modelVersions = modelPage.ModelVersions.Where(p => p.VersionId == versionId).FirstOrDefault();
                            if (modelVersions != null)
                                price = Convert.ToUInt32(modelVersions.Price);
                        }
                        else
                        {
                            if (modelPage.ModelDetails != null)
                                price = Convert.ToUInt32(modelPage.ModelDetails.MinPrice);
                        }
                    }
                }
                else
                {
                    var modelVersions = modelPage.ModelVersions.Where(p => p.VersionId == versionId).FirstOrDefault();

                    if (modelVersions != null)
                        price = Convert.ToUInt32(modelVersions.Price);
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, Request.ServerVariables["URL"] + "-" + MethodBase.GetCurrentMethod().Name);
                objErr.SendMail();
            }
        }

        /// <summary>
        /// Author          :   Sangram Nandkhile
        /// Created Date    :   27 Nov 2015
        /// Description     :   These values need to be set to reflect UI HTML changes
        /// </summary>
        //private void ToggleOfferDiv()
        //{
        //    if (isOfferAvailable)
        //    {
        //        grid1_size = 5;
        //        grid2_size = 7;
        //        cssOffers = string.Empty;
        //        offerDivHide = string.Empty;
        //    }
        //    else
        //    {
        //        grid1_size = 9;
        //        grid2_size = 3;
        //        cssOffers = "noOffers";
        //        offerDivHide = "hide";
        //    }
        //}

        /// <summary>
        /// Author          :   Sangram Nandkhile
        /// Created Date    :   27 Nov 2015
        /// Description     :   Sends the notification to Customer and Dealer
        /// </summary>
        private void FetchVariantDetails(uint versionId)
        {
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IBikeModelsRepository<BikeModelEntity, int>, BikeModelsRepository<BikeModelEntity, int>>();
                    IBikeModelsRepository<BikeModelEntity, int> objVersion = container.Resolve<IBikeModelsRepository<BikeModelEntity, int>>();
                    modelPage.ModelVersionSpecs = objVersion.MVSpecsFeatures(Convert.ToInt32(versionId));
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, Request.ServerVariables["URL"] + MethodBase.GetCurrentMethod().Name);
                objErr.SendMail();
            }
        }
        /// <summary>
        /// Author          :   Sangram Nandkhile
        /// Created Date    :   27 Nov 2015
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
        /// Created Date    :   27 Nov 2015
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
                        if (areaList != null && areaList.Count() > 0)
                            isAreaAvailable = true;
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
        /// Author: Sangram Nandkhile
        /// Desc: Removed API Call for on road Price Quote
        /// Modified By : Lucky Rathore on 09 May 2016.
        /// Description : modelImage intialize.
        /// Modified By : Lucky Rathore on 27 June 2016
        /// Description : replace cookie __utmz with BWUtmz
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
                    objPQEntity.CityId = cityId;
                    objPQEntity.AreaId = areaId;
                    objPQEntity.ClientIP = clientIP;
                    objPQEntity.SourceId = 2;
                    objPQEntity.ModelId = modelId;
                    objPQEntity.VersionId = versionId;
                    objPQEntity.PQLeadId = Convert.ToUInt16(PQSourceEnum.Mobile_ModelPage);
                    objPQEntity.UTMA = Request.Cookies["__utma"] != null ? Request.Cookies["__utma"].Value : "";
                    objPQEntity.UTMZ = Request.Cookies["BWUtmz"] != null ? Request.Cookies["BWUtmz"].Value : "";
                    objPQEntity.DeviceId = Request.Cookies["BWC"] != null ? Request.Cookies["BWC"].Value : "";
                    PQOutputEntity objPQOutput = objDealer.ProcessPQ(objPQEntity);
                    if (versionId == 0)
                    {
                        if (objPQOutput != null && objPQOutput.VersionId != null)
                        {
                            versionId = objPQOutput.VersionId;
                        }
                    }
                    if (objPQOutput != null)
                    {
                        pqOnRoad = new PQOnRoadPrice();
                        pqOnRoad.PriceQuote = objPQOutput;
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
                                    string api = String.Format("/api/DealerPriceQuote/GetDealerPriceQuote/?cityid={0}&versionid={1}&dealerid={2}", cityId, versionId, objPQOutput.DealerId);
                                    using (Utility.BWHttpClient objDealerPqClient = new Utility.BWHttpClient())
                                    {
                                        //objPrice = objClient.GetApiResponseSync<PQ_QuotationEntity>(Utility.BWConfiguration.Instance.ABApiHostUrl, Utility.BWConfiguration.Instance.APIRequestTypeJSON, api, objPrice);
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

                                            if (pqOnRoad.DPQOutput.objOffers != null && pqOnRoad.DPQOutput.objOffers.Count > 0)
                                                pqOnRoad.DPQOutput.discountedPriceList = OfferHelper.ReturnDiscountPriceList(pqOnRoad.DPQOutput.objOffers, pqOnRoad.DPQOutput.PriceList);
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
                                    Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, Request.ServerVariables["URL"] + MethodBase.GetCurrentMethod().Name);
                                    objErr.SendMail();
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, Request.ServerVariables["URL"] + MethodBase.GetCurrentMethod().Name);
                objErr.SendMail();
            }

            return pqOnRoad;
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
            if ((!isCitySelected) || (isCitySelected && isAreaAvailable && !isAreaSelected))
            {
                toShowOnRoadPriceButton = true;
            }
        }

        /// <summary>
        /// Created By: Sangram Nandkhile on 17-Mar-2016
        /// Summary   : To create Viewmodel for Version Page View
        /// </summary>
        private void FillViewModel()
        {
            try
            {
                if (cityId > 0 && versionId > 0)
                {
                    viewModel = new ModelPageVM(cityId, versionId, dealerId);
                    if (viewModel.DealerCampaign.PrimaryDealer.OfferList != null && viewModel.DealerCampaign.PrimaryDealer.OfferList.Count() > 0)
                    {
                        rptOffers.DataSource = viewModel.Offers;
                        rptOffers.DataBind();
                        isOfferAvailable = true;
                    }
                    if (viewModel.DealerCampaign.SecondaryDealerCount > 0)
                    {
                        rptSecondaryDealers.DataSource = viewModel.DealerCampaign.SecondaryDealers;
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

    }
}
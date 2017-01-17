using Bikewale.BAL.BikeBooking;
using Bikewale.BAL.BikeData;
using Bikewale.BAL.Used.Search;
using Bikewale.BindViewModels.Webforms;
using Bikewale.Cache.BikeData;
using Bikewale.Cache.Core;
using Bikewale.Cache.Location;
using Bikewale.common;
using Bikewale.Common;
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
using Bikewale.Mobile.Controls;
using Bikewale.Utility;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
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
        protected PopularModelComparison ctrlCompareBikes;
        protected NewNewsWidget ctrlNews;
        protected NewExpertReviewsWidget ctrlExpertReviews;
        protected ServiceCenterCard ctrlServiceCenterCard;
        protected NewVideosWidget ctrlVideos;
        protected NewUserReviewList ctrlUserReviews;
        protected BikeModelPageEntity modelPage;
        protected VersionSpecifications bikeSpecs;
        protected LeadCaptureControl ctrlLeadCapture;
        protected PQOnRoadPrice pqOnRoad;
        protected UsedBikes ctrlRecentUsedBikes;
        protected Repeater rptNavigationPhoto, rptVarients, rptColors, rptOffers, rptNewOffers, rptSecondaryDealers;
        protected string cityName = string.Empty, mpqQueryString = string.Empty, areaName = string.Empty, variantText = string.Empty, pqId = string.Empty, bikeName = string.Empty, bikeModelName = string.Empty, bikeMakeName = string.Empty, modelImage = string.Empty, location = string.Empty, priceText = "Ex-showroom", detailedPriceLink = string.Empty, versionText = string.Empty, summaryDescription = string.Empty, clientIP = CommonOpn.GetClientIP();
        //Varible to Hide or show controlers
        protected bool isCitySelected, isAreaSelected, isBikeWalePQ, isDiscontinued, isOnRoadPrice, toShowOnRoadPriceButton, isUserReviewActive, isExpertReviewActive, isNewsActive, isVideoActive, isUserReviewZero = true, isExpertReviewZero = true, isNewsZero = true, isVideoZero = true, isAreaAvailable, isDealerPQ, isDealerAssitance, isBookingAvailable, isOfferAvailable;
        protected NewAlternativeBikes ctrlAlternativeBikes;
        protected DealersCard ctrlDealerCard;
        protected ushort reviewTabsCnt, moreOffersCount, versionCount = 1;
        public DropDownList ddlNewVersionList;
        //Variable to Assing ACTIVE class
        public uint totalUsedBikes;
        static readonly string _PageNotFoundPath;
        static readonly string _bwHostUrl;
        protected static bool isManufacturer;
        protected uint onRoadPrice, totalDiscountedPrice, price, bookingAmt, cityId, campaignId, manufacturerId, modelId, versionId, areaId, dealerId;
        protected IEnumerable<CityEntityBase> objCityList = null;
        protected IEnumerable<Bikewale.Entities.Location.AreaEntityBase> objAreaList = null;
        protected OtherVersionInfoEntity objSelectedVariant = null;
        protected Label defaultVariant;
        protected HiddenField hdnVariant;
        protected MPriceInTopCities ctrlTopCityPrices;
        protected string pq_leadsource = "33", pq_sourcepage = "59", hide = string.Empty, pgDescription = string.Empty;
        public Bikewale.Entities.Used.Search.SearchResult UsedBikes = null;
        protected ModelPageVM viewModel = null;
        protected ModelGallery ctrlModelGallery;
        protected int colorCount;
        GlobalCityAreaEntity currentCityArea = null;
        private StringBuilder colorStr = new StringBuilder();
        #region Events
        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }

        /// <summary>
        /// Modified By : Lucky Rathore on 04 July 2016.
        /// Description : function "SetBWUtmz" called.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //Set identification for m site
            // HttpContext.Current.Response.Cookies["IsMobileSite"].Value = "true";
            // Modified By :Ashish Kamble on 5 Feb 2016
            Form.Action = Request.RawUrl;
            // Do not change the sequence of the function calls
            Trace.Warn("Trace 3 : ParseQueryString Start");
            ParseQueryString();
            Trace.Warn("Trace 4 : ParseQueryString End");
            try
            {
                if (modelId > 0)
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

                    BindControls();

                    Trace.Warn("Trace 20 : Page Load ends");

                    if (modelPage.ModelVersions != null && modelPage.ModelVersions.Count > 0)
                    {
                        rptVarients.DataSource = modelPage.ModelVersions;
                        rptVarients.DataBind();
                        if (versionCount > 1)
                        {
                            ddlNewVersionList.DataSource = modelPage.ModelVersions;
                            ddlNewVersionList.DataTextField = "VersionName";
                            ddlNewVersionList.DataValueField = "VersionId";
                            ddlNewVersionList.DataBind();
                            ddlNewVersionList.SelectedValue = versionId.ToString();
                            versionText = ddlNewVersionList.SelectedItem.Text;
                        }
                    }
                    SetFlagsAtEnd();
                    BindColorString();
                    CreateMetas();
                    BindDescription();

                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, String.Format("PageLoad({0})", Request.QueryString["model"]));
                objErr.SendMail();
            }

        }
        /// <summary>
        /// Created By :-Subodh Jain 07 oct 2016
        /// Desc:- To bind Description on model page
        /// Modified by :   Sumit Kate on 20 Jan 2017
        /// Description :   Model Page SEO changes
        /// </summary>
        private void BindDescription()
        {
            try
            {
                string versionDescirption = versionCount > 1 ? string.Format(" It is available in {0} versions", versionCount) : string.Format(" It is available in {0} version", versionCount);
                string specsDescirption = string.Empty;
                string priceDescription = modelPage.ModelDetails.MinPrice > 0 ? string.Format("Price - Rs. {0} onwards (Ex-showroom, {1}).", Bikewale.Utility.Format.FormatPrice(Convert.ToString(modelPage.ModelDetails.MinPrice)), Bikewale.Utility.BWConfiguration.Instance.DefaultName) : string.Empty;

                if (modelPage != null && modelPage.ModelVersionSpecs != null && (modelPage.ModelVersionSpecs.TopSpeed > 0 || modelPage.ModelVersionSpecs.FuelEfficiencyOverall > 0))
                {
                    if ((modelPage.ModelVersionSpecs.TopSpeed > 0 && modelPage.ModelVersionSpecs.FuelEfficiencyOverall > 0))
                        specsDescirption = string.Format("{0} has a mileage of {1} kmpl and a top speed of {2} kmph. ", bikeModelName, modelPage.ModelVersionSpecs.FuelEfficiencyOverall, modelPage.ModelVersionSpecs.TopSpeed);
                    else if (modelPage.ModelVersionSpecs.TopSpeed == 0)
                    {
                        specsDescirption = string.Format("{0} has a mileage of {1} kmpl.", bikeModelName, modelPage.ModelVersionSpecs.FuelEfficiencyOverall);
                    }
                    else
                    {
                        specsDescirption = string.Format("{0} has a top speed of {1} kmph.", bikeModelName, modelPage.ModelVersionSpecs.TopSpeed);
                    }
                }
                summaryDescription = string.Format("{0} {1}{2}.{3}{4}", bikeName, priceDescription, versionDescirption, specsDescirption, colorStr);
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, "NewBikeModels.BindDescription");
                objErr.SendMail();
            }
        }

        /// <summary>
        /// Created By :-Subodh Jain 07 oct 2016
        /// Desc:- values to controls field
        /// Modified by :  Subodh Jain on 21 Dec 2016
        /// Description :  Added dealer card and service center card
        /// Modified By :Subodh Jain 06 Jan 2016
        /// Summary : Added Model gallery widget
        /// </summary>
        private void BindControls()
        {
            if (!isDiscontinued)
            {
                ctrlCompareBikes.versionId = versionId;
                ctrlCompareBikes.versionName = bikeModelName;
                ctrlCompareBikes.cityid = Convert.ToInt32(cityId);
                ctrlCompareBikes.TopCount = 6;
            }
            ////news,videos,revews, user reviews
            ctrlNews.TotalRecords = 3;
            ctrlNews.ModelId = Convert.ToInt32(modelId);
            ctrlNews.WidgetTitle = bikeName;
            ctrlNews.MakeMaskingName = modelPage.ModelDetails.MakeBase.MaskingName;
            ctrlNews.ModelMaskingName = modelPage.ModelDetails.MaskingName;
            ctrlNews.MakeName = modelPage.ModelDetails.MakeBase.MakeName;
            ctrlNews.ModelName = modelPage.ModelDetails.ModelName;

            ctrlExpertReviews.TotalRecords = 3;
            ctrlExpertReviews.ModelId = Convert.ToInt32(modelId);
            ctrlExpertReviews.MakeName = modelPage.ModelDetails.MakeBase.MakeName;
            ctrlExpertReviews.ModelName = modelPage.ModelDetails.ModelName;

            ctrlVideos.TotalRecords = 3;
            ctrlVideos.MakeMaskingName = modelPage.ModelDetails.MakeBase.MaskingName.Trim();
            ctrlVideos.ModelMaskingName = modelPage.ModelDetails.MaskingName.Trim();
            ctrlVideos.ModelId = Convert.ToInt32(modelId);
            ctrlVideos.MakeName = modelPage.ModelDetails.MakeBase.MakeName;
            ctrlVideos.ModelName = modelPage.ModelDetails.ModelName;

            ctrlUserReviews.ReviewCount = 3;
            ctrlUserReviews.PageNo = 1;
            ctrlUserReviews.PageSize = 3;
            ctrlUserReviews.ModelId = Convert.ToInt32(modelId);
            ctrlUserReviews.Filter = Entities.UserReviews.FilterBy.MostRecent;
            ctrlUserReviews.MakeName = modelPage.ModelDetails.MakeBase.MakeName;
            ctrlUserReviews.ModelName = modelPage.ModelDetails.ModelName;

            ctrlExpertReviews.MakeMaskingName = modelPage.ModelDetails.MakeBase.MaskingName.Trim();
            ctrlExpertReviews.ModelMaskingName = modelPage.ModelDetails.MaskingName.Trim();

            if (!modelPage.ModelDetails.Futuristic || modelPage.ModelDetails.New)
                ctrlTopCityPrices.ModelId = Convert.ToUInt32(modelId);
            else ctrlTopCityPrices.ModelId = 0;

            ctrlTopCityPrices.IsDiscontinued = isDiscontinued;
            ctrlTopCityPrices.TopCount = 4;

            ctrlDealerCard.CityId = cityId;
            ctrlDealerCard.MakeId = Convert.ToUInt32(modelPage.ModelDetails.MakeBase.MakeId);
            ctrlDealerCard.makeMaskingName = modelPage.ModelDetails.MakeBase.MaskingName;
            ctrlDealerCard.makeName = modelPage.ModelDetails.MakeBase.MakeName;
            ctrlDealerCard.cityName = cityName;
            ctrlDealerCard.PageName = "Model_Page";
            ctrlDealerCard.TopCount = 6;
            ctrlDealerCard.PQSourceId = (int)PQSourceEnum.Mobile_ModelPage;
            ctrlDealerCard.widgetHeading = string.Format("{0} showrooms in {1}", modelPage.ModelDetails.MakeBase.MakeName, cityName);


            ctrlServiceCenterCard.MakeId = Convert.ToUInt32(modelPage.ModelDetails.MakeBase.MakeId);
            ctrlServiceCenterCard.makeMaskingName = modelPage.ModelDetails.MakeBase.MaskingName;
            ctrlServiceCenterCard.makeName = modelPage.ModelDetails.MakeBase.MakeName;
            ctrlServiceCenterCard.CityId = cityId;
            ctrlServiceCenterCard.cityName = cityName;
            ctrlServiceCenterCard.TopCount = 9;
            ctrlServiceCenterCard.widgetHeading = string.Format("{0} service centers in {1}", modelPage.ModelDetails.MakeBase.MakeName, cityName);


            ctrlLeadCapture.CityId = cityId;
            ctrlLeadCapture.ModelId = modelId;
            ctrlLeadCapture.AreaId = areaId;
            ctrlRecentUsedBikes.MakeId = Convert.ToUInt32(modelPage.ModelDetails.MakeBase.MakeId);
            ctrlRecentUsedBikes.ModelId = Convert.ToUInt32(modelId);
            ctrlRecentUsedBikes.CityId = (int?)cityId;
            ctrlRecentUsedBikes.TopCount = 6;
            ctrlRecentUsedBikes.header = "Recently uploaded Used " + modelPage.ModelDetails.ModelName + " bikes " + (cityId > 0 ? String.Format("in {0}", cityName) : string.Empty);

            if (modelPage != null && modelPage.Photos != null)
            {
                ctrlModelGallery.Photos = modelPage.Photos;
                ctrlModelGallery.articleName = string.Format("{0} {1}", modelPage.ModelDetails.MakeBase.MakeName, modelPage.ModelDetails.ModelName);
                ctrlModelGallery.isModelPage = false;
                ctrlModelGallery.modelId = modelPage.ModelDetails.ModelId;
                ctrlModelGallery.modelName = modelPage.ModelDetails.ModelName;

            }

        }

        /// <summary>
        /// Created By :-Subodh Jain 07 oct 2016
        /// Desc:- Metas description according to discountinue,upcoming,continue bikes
        /// </summary>
        private void CreateMetas()
        {

            totalUsedBikes = TotalUsedBikes(modelId, cityId);
            if (modelPage.ModelDetails.Futuristic)
            {
                pgDescription = string.Format("{0} {1} Price in India is expected between Rs. {2} and Rs. {3}. Check out {0} {1}  specifications, reviews, mileage, versions, news & photos at BikeWale.com. Launch date of {1} is around {4}", modelPage.ModelDetails.MakeBase.MakeName, modelPage.ModelDetails.ModelName, Bikewale.Utility.Format.FormatNumeric(Convert.ToString(modelPage.UpcomingBike.EstimatedPriceMin)), Bikewale.Utility.Format.FormatNumeric(Convert.ToString(modelPage.UpcomingBike.EstimatedPriceMax)), modelPage.UpcomingBike.ExpectedLaunchDate);
            }
            else if (!modelPage.ModelDetails.New)
            {
                pgDescription = string.Format("{0} {1} Price in India - Rs. {2}. It has been discontinued in India. There are {3} used {1} bikes for sale. Check out {1} specifications, reviews, mileage, versions, news & photos at BikeWale.com", modelPage.ModelDetails.MakeBase.MakeName, modelPage.ModelDetails.ModelName, Bikewale.Utility.Format.FormatNumeric(price.ToString()), totalUsedBikes);
            }
            else
            {
                pgDescription = String.Format("{0} Price in India - Rs. {1}. Find {2} Reviews, Specs, Features, Mileage, On Road Price and Images at Bikewale. {3}", bikeName, Bikewale.Utility.Format.FormatNumeric(price.ToString()), bikeModelName, colorStr);

            }
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
                            {
                                if (isOnRoadPrice)
                                    currentTextBox.Text = Bikewale.Utility.Format.FormatPrice(Convert.ToString(selected.OnRoadPrice));
                                else currentTextBox.Text = Bikewale.Utility.Format.FormatPrice(Convert.ToString(selected.Price));

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
        }

        #endregion

        #region methods
        /// <summary>
        /// Summary     :  Set isCitySelected, isAreaSelected
        /// Modified by :   Sumit Kate on 04 Jan 2016
        /// Description :   Replaced the Convert.ToXXX with XXX.TryParse method
        /// Modified By : Sushil Kumar on 26th August 2016
        /// Description : Replaced location name from location cookie to selected location objects for city and area respectively.
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
                            if (_objArea != null)
                            {
                                areaName = _objArea.AreaName;
                                isAreaSelected = true;
                            }
                        }
                    }
                }
            }

            if (string.IsNullOrEmpty(cityName))
            {
                GlobalCityAreaEntity currentCityArea = GlobalCityArea.GetGlobalCityArea();
                cityName = currentCityArea.City;
            }
        }
        #endregion

        static NewBikeModels()
        {
            _PageNotFoundPath = Bikewale.Common.CommonOpn.AppPath + "pageNotFound.aspx";
            _bwHostUrl = ConfigurationManager.AppSettings["bwHostUrl"];
        }

        private void BindAlternativeBikeControl()
        {
            ctrlAlternativeBikes.TopCount = 9;

            if (modelPage.ModelVersions != null && modelPage.ModelVersions.Count > 0)
            {
                ctrlAlternativeBikes.VersionId = Convert.ToUInt32(modelPage.ModelVersions[0].VersionId);
                ctrlAlternativeBikes.PQSourceId = (int)PQSourceEnum.Mobile_ModelPage_Alternative;
                ctrlAlternativeBikes.WidgetTitle = bikeName;
                ctrlAlternativeBikes.CityId = cityId;
                ctrlAlternativeBikes.modelName = modelPage.ModelDetails.ModelName;
                ctrlAlternativeBikes.CustomHeading = "More info about similar bikes";
            }
        }

        /// <summary>
        /// Function to bind the photos album
        /// </summary>
        private void BindRepeaters()
        {
            if (modelPage != null)
            {
                if (modelPage.ModelVersions != null && modelPage.ModelVersions.Count > 0)
                {
                    versionCount = Convert.ToUInt16(modelPage.ModelVersions.Count);
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
                                    variantText = firstVer.VersionName;

                                if (versionId == 0)
                                    hdnVariant.Value = Convert.ToString(modelPage.ModelVersionSpecs.BikeVersionId);
                                else
                                    hdnVariant.Value = Convert.ToString(versionId);
                            }
                            else if (modelPage.ModelVersions.Count > 1)
                            {
                                var firstVer = modelPage.ModelVersions.FirstOrDefault();
                                if (firstVer != null)
                                {
                                    variantText = firstVer.VersionName;
                                }
                                variantText = firstVer.VersionName;
                            }
                        }
                        else if (modelPage.ModelVersions.Count == 1)
                        {
                            versionId = Convert.ToUInt32(modelPage.ModelVersions[0].VersionId);
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
                            .RegisterType<IBikeModelsRepository<BikeModelEntity, int>, BikeModelsRepository<BikeModelEntity, int>>()
                             .RegisterType<IBikeModels<BikeModelEntity, int>, BikeModels<BikeModelEntity, int>>()
                             .RegisterType<ICacheManager, MemcacheManager>();

                    var objCache = container.Resolve<IBikeModelsCacheRepository<int>>();
                    modelPage = objCache.GetModelPageDetails(Convert.ToInt16(modelId), (int)versionId);
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
                            else
                            {
                                if (versionId > 0)
                                {
                                    if (!modelPage.ModelVersions.Exists(p => p.VersionId == versionId))
                                    {
                                        versionId = modelPage.ModelVersionSpecs.BikeVersionId;
                                    }
                                }
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
                        if (pqOnRoad.BPQOutput != null)
                            pqOnRoad.BPQOutput.ManufacturerAd = Format.FormatManufacturerAd(pqOnRoad.BPQOutput.ManufacturerAd, pqOnRoad.BPQOutput.CampaignId, pqOnRoad.BPQOutput.ManufacturerName, pqOnRoad.BPQOutput.MaskingNumber, Convert.ToString(pqOnRoad.BPQOutput.ManufacturerId), pqOnRoad.BPQOutput.Area, pq_leadsource, pq_sourcepage, string.Empty, string.Empty, string.Empty, string.IsNullOrEmpty(pqOnRoad.BPQOutput.MaskingNumber) ? "hide" : string.Empty);

                        versionId = pqOnRoad.PriceQuote.VersionId;
                        if (pqOnRoad.PriceQuote != null)
                        {
                            dealerId = pqOnRoad.PriceQuote.DealerId;
                            pqId = Convert.ToString(pqOnRoad.PriceQuote.PQId);
                        }

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
                                    // New model page offers section
                                    IEnumerable<OfferEntity> distictOfferCategories = pqOnRoad.DPQOutput.objOffers.GroupBy(offer => offer.OfferCategoryId).Select(g => g.First());
                                    int distictOfferCount = distictOfferCategories.Count();
                                    if (distictOfferCount > 3)
                                    {
                                        rptNewOffers.DataSource = distictOfferCategories.Take(2);
                                        moreOffersCount = Convert.ToUInt16(pqOnRoad.DPQOutput.objOffers.Count() - 2);
                                    }
                                    else
                                    {
                                        rptNewOffers.DataSource = distictOfferCategories;
                                    }
                                    rptNewOffers.DataBind();
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
                                if (isOnRoadPrice)
                                {
                                    if (versionId > 0)
                                    {
                                        objSelectedVariant = pqOnRoad.BPQOutput.Varients.FirstOrDefault(p => p.VersionId == versionId);
                                        if (objSelectedVariant != null)
                                            price = Convert.ToUInt32(objSelectedVariant.OnRoadPrice);
                                    }
                                    else
                                    {
                                        objSelectedVariant = pqOnRoad.BPQOutput.Varients.FirstOrDefault();
                                        price = Convert.ToUInt32(objSelectedVariant.OnRoadPrice);
                                    }
                                }
                                else
                                {
                                    if (versionId > 0)
                                    {
                                        objSelectedVariant = pqOnRoad.BPQOutput.Varients.FirstOrDefault(p => p.VersionId == versionId);
                                        if (objSelectedVariant != null)
                                            price = Convert.ToUInt32(objSelectedVariant.Price);
                                    }
                                    else
                                    {
                                        objSelectedVariant = pqOnRoad.BPQOutput.Varients.FirstOrDefault();
                                        price = Convert.ToUInt32(objSelectedVariant.Price);
                                    }

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
                    objPQEntity.CityId = cityId;
                    objPQEntity.AreaId = areaId;
                    objPQEntity.ClientIP = clientIP;
                    objPQEntity.SourceId = 2;
                    objPQEntity.ModelId = modelId;
                    objPQEntity.VersionId = versionId;
                    objPQEntity.PQLeadId = Convert.ToUInt16(PQSourceEnum.Mobile_ModelPage);
                    objPQEntity.UTMA = Request.Cookies["__utma"] != null ? Request.Cookies["__utma"].Value : "";
                    objPQEntity.UTMZ = Request.Cookies["_bwutmz"] != null ? Request.Cookies["_bwutmz"].Value : "";
                    objPQEntity.DeviceId = Request.Cookies["BWC"] != null ? Request.Cookies["BWC"].Value : "";
                    PQOutputEntity objPQOutput = objDealer.ProcessPQ(objPQEntity);

                    if (versionId == 0)
                    {
                        if (objPQOutput != null && objPQOutput.VersionId > 0)
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
                            bpqOutput = objPq.GetPriceQuoteById(objPQOutput.PQId, LeadSourceEnum.Model_Mobile);
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
                                    oblDealerPQ = oblDealerPQ = dealerPq.GetDealePQEntity(cityId, objPQOutput.DealerId, versionId);
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
                                    //}
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

            if (!isDiscontinued)
            {
                if (isCitySelected)
                {
                    location += !string.IsNullOrEmpty(areaName) ? string.Format("{0}, {1}", areaName, cityName) : cityName;

                }
                else
                {
                    location = Bikewale.Utility.BWConfiguration.Instance.DefaultName;
                }
            }
        }
        /// <summary>
        /// Author: Sangram Nandkhile
        /// Created on: 19-aug-2016
        /// Desc: Set flags at the end of page load
        /// </summary>
        private void SetFlagsAtEnd()
        {
            if (isDiscontinued)
            {
                priceText = "Last known Ex-showroom";
            }
            else if (isCitySelected && isOnRoadPrice)
            {
                priceText = "On-road";
            }

            if (isOnRoadPrice && price > 0)
            {
                detailedPriceLink = PriceQuoteQueryString.FormBase64QueryString(cityId.ToString(), pqId, areaId.ToString(), versionId.ToString(), dealerId.ToString());
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
                if (cityId > 0 && areaId > 0 && versionId > 0)
                {
                    viewModel = new ModelPageVM(cityId, versionId, dealerId, areaId);
                    if (viewModel.DealerCampaignV2.PrimaryDealer.OfferList != null && viewModel.DealerCampaignV2.PrimaryDealer.OfferList.Count() > 0)
                    {
                        rptOffers.DataSource = viewModel.Offers;
                        rptOffers.DataBind();
                        isOfferAvailable = true;
                    }
                    if (viewModel.DealerCampaignV2.SecondaryDealerCount > 0)
                    {
                        rptSecondaryDealers.DataSource = viewModel.SecondaryDealersV2;
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

        private void BindColorString()
        {
            try
            {
                if (modelPage != null && modelPage.ModelColors != null && modelPage.ModelColors.Count() > 0)
                {
                    colorCount = modelPage.ModelColors.Count();
                    string lastColor = modelPage.ModelColors.Last().ColorName;
                    if (colorCount > 1)
                    {
                        colorStr.AppendFormat("{0} is available in {1} different colors : ", bikeName, colorCount);
                        var colorArr = modelPage.ModelColors.Select(x => x.ColorName).Take(colorCount - 1);
                        // Comma separated colors (except last one)
                        colorStr.Append(string.Join(",", colorArr));
                        // Append last color with And
                        colorStr.AppendFormat(" and {0}.", lastColor);
                    }
                    else if (colorCount == 1)
                    {
                        colorStr.AppendFormat("{0} is available in {1} color.", bikeName, lastColor);
                    }
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, Request.ServerVariables["URL"] + "BindColorString");
                objErr.SendMail();
            }
        }

        /// <summary>
        /// Created By :-Subodh Jain 07 oct 2016
        /// Desc:- To get total number of used bikes
        /// </summary>
        public uint TotalUsedBikes(uint modelId, uint cityId)
        {
            SearchResult UsedBikes = null;
            uint totalUsedBikes = 0;
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
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, string.Format("BikeModels.aspx --> TotalUsedBikes() --> modelId: {0}, cityId: {1}", modelId, cityId));
                objErr.SendMail();
            }
            return totalUsedBikes;
        }
    }
}
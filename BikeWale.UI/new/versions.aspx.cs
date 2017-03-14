using Bikewale.BAL.BikeBooking;
using Bikewale.BAL.BikeData;
using Bikewale.BAL.Pager;
using Bikewale.BAL.Used.Search;
using Bikewale.BindViewModels.Controls;
using Bikewale.BindViewModels.Webforms;
using Bikewale.Cache.BikeData;
using Bikewale.Cache.Core;
using Bikewale.common;
using Bikewale.Common;
using Bikewale.Controls;
using Bikewale.DAL.BikeData;
using Bikewale.DAL.Used.Search;
using Bikewale.DTO.Version;
using Bikewale.Entities.BikeBooking;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.GenericBikes;
using Bikewale.Entities.Location;
using Bikewale.Entities.PriceQuote;
using Bikewale.Entities.Used.Search;
using Bikewale.Interfaces.BikeBooking;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.Pager;
using Bikewale.Interfaces.PriceQuote;
using Bikewale.Interfaces.Used.Search;
using Bikewale.Utility;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;

namespace Bikewale.New
{
    /// <summary>
    /// Modified By : Sangram Nandkhile on 07 Dec 2016.
    /// Description : Removed unncessary functions,
    /// </summary>
    public class BikeModel : PageBase //inherited page base class to move viewstate from top of the html page to the end
    {
        #region Global Variables

        protected News_Widget ctrlNews;
        protected NewExpertReviews ctrlExpertReviews;
        protected NewVideosControl ctrlVideos;
        protected NewUserReviewsList ctrlUserReviews;
        protected PriceInTopCities ctrlTopCityPrices;
        protected BikeModelPageEntity modelPageEntity;
        protected PopularModelCompare ctrlPopularCompare;
        protected DealerCard ctrlDealerCard;
        protected VersionSpecifications bikeSpecs;
        protected PQOnRoadPrice pqOnRoad;
        protected ServiceCenterCard ctrlServiceCenterCard;
        protected int grid1_size = 9, grid2_size = 3, colorCount;
        protected Repeater rptModelPhotos, rptNavigationPhoto, rptVarients, rptOffers, rptSecondaryDealers;
        protected string location = string.Empty, modelQuerystring = string.Empty, cityName = string.Empty, mpqQueryString = string.Empty, areaName = string.Empty, variantText = string.Empty, pqId = string.Empty, bikeName = string.Empty, bikeModelName = string.Empty, bikeMakeName = string.Empty, modelImage = string.Empty, summaryDescription = string.Empty, clientIP = CommonOpn.GetClientIP();
        protected bool isCityAvailable, isCitySelected, isAreaSelected, isBikeWalePQ, isDiscontinued, isOnRoadPrice, toShowOnRoadPriceButton;

        //Varible to Hide or show controlers
        protected bool isUserReviewActive, isExpertReviewActive, isNewsActive, isVideoActive, isUserReviewZero = true, isExpertReviewZero = true, isNewsZero = true, isVideoZero = true, isAreaAvailable, isDealerAssitance, isBookingAvailable, isOfferAvailable;
        protected NewAlternativeBikes ctrlAlternativeBikes;
        protected LeadCaptureControl ctrlLeadCapture;
        protected short reviewTabsCnt;
        protected static bool isManufacturer;
        protected uint totalUsedBikes, onRoadPrice, totalDiscountedPrice, price, bookingAmt, cityId, campaignId, manufacturerId, modelId, variantId, defaultVersionId, areaId, dealerId;
        protected IEnumerable<CityEntityBase> objCityList = null;
        protected IEnumerable<Bikewale.Entities.Location.AreaEntityBase> objAreaList = null;
        protected OtherVersionInfoEntity objSelectedVariant = null;
        protected HiddenField hdnVariant;
        protected string pq_leadsource = "32", pq_sourcepage = "57", cssOffers = "noOffers", offerDivHide = "hide", pgDescription = string.Empty, detailedPriceLink = string.Empty;
        protected UsedBikes ctrlRecentUsedBikes;
        public Bikewale.Entities.Used.Search.SearchResult UsedBikes = null;
        private StringBuilder colorStr = new StringBuilder();
        protected ModelPageVM viewModel = null;
        public DropDownList ddlVersion;
        protected BikeRankingEntity bikeRankObj;
        protected string styleName = string.Empty, rankText = string.Empty, bikeType = string.Empty;

        #endregion Global Variables

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
            StringBuilder sb = new StringBuilder();
            System.Diagnostics.Stopwatch watch = System.Diagnostics.Stopwatch.StartNew();
            DetectDevice();
            watch.Stop();
            long elapsedMs = watch.ElapsedMilliseconds;
            sb.AppendFormat("DetectDevice\t{0} ms", elapsedMs).AppendLine();

            watch = System.Diagnostics.Stopwatch.StartNew();
            ParseQueryString();
            watch.Stop();
            elapsedMs = watch.ElapsedMilliseconds;
            sb.AppendFormat("ParseQueryString\t{0} ms", elapsedMs).AppendLine();

            try
            {
                if (modelId > 0)
                {
                    #region Do Not change the sequence
                    watch = System.Diagnostics.Stopwatch.StartNew();
                    CheckCityCookie();
                    watch.Stop();
                    elapsedMs = watch.ElapsedMilliseconds;
                    sb.AppendFormat("CheckCityCookie\t{0} ms", elapsedMs).AppendLine();

                    watch = System.Diagnostics.Stopwatch.StartNew();
                    SetFlags();
                    watch.Stop();
                    elapsedMs = watch.ElapsedMilliseconds;
                    sb.AppendFormat("SetFlags\t{0} ms", elapsedMs).AppendLine();

                    if (hdnVariant != null && hdnVariant.Value != "0")
                        variantId = Convert.ToUInt32(hdnVariant.Value);

                    watch = System.Diagnostics.Stopwatch.StartNew();
                    modelPageEntity = FetchModelPageDetails(modelId);
                    watch.Stop();
                    elapsedMs = watch.ElapsedMilliseconds;
                    sb.AppendFormat("FetchModelPageDetails\t{0} ms", elapsedMs).AppendLine();

                    if (modelPageEntity != null && modelPageEntity.ModelDetails != null && modelPageEntity.ModelDetails.New)
                    {
                        watch = System.Diagnostics.Stopwatch.StartNew();
                        FetchOnRoadPrice(modelPageEntity);
                        watch.Stop();
                        elapsedMs = watch.ElapsedMilliseconds;
                        sb.AppendFormat("FetchOnRoadPrice\t{0} ms", elapsedMs).AppendLine();

                        watch = System.Diagnostics.Stopwatch.StartNew();
                        FillViewModel();
                        watch.Stop();
                        elapsedMs = watch.ElapsedMilliseconds;
                        sb.AppendFormat("FillViewModel\t{0} ms", elapsedMs).AppendLine();
                    }
                    watch = System.Diagnostics.Stopwatch.StartNew();
                    BindPhotoRepeater(modelPageEntity);
                    watch.Stop();
                    elapsedMs = watch.ElapsedMilliseconds;
                    sb.AppendFormat("BindPhotoRepeater\t{0} ms", elapsedMs).AppendLine();

                    watch = System.Diagnostics.Stopwatch.StartNew();
                    LoadVariants(modelPageEntity);
                    watch.Stop();
                    elapsedMs = watch.ElapsedMilliseconds;
                    sb.AppendFormat("LoadVariants\t{0} ms", elapsedMs).AppendLine();

                    watch = System.Diagnostics.Stopwatch.StartNew();
                    BindControls(modelPageEntity);
                    watch.Stop();
                    elapsedMs = watch.ElapsedMilliseconds;
                    sb.AppendFormat("BindControls\t{0} ms", elapsedMs).AppendLine();

                    watch = System.Diagnostics.Stopwatch.StartNew();
                    ToggleOfferDiv();
                    watch.Stop();
                    elapsedMs = watch.ElapsedMilliseconds;
                    sb.AppendFormat("ToggleOfferDiv\t{0} ms", elapsedMs).AppendLine();

                    watch = System.Diagnostics.Stopwatch.StartNew();
                    BindColorString();
                    watch.Stop();
                    elapsedMs = watch.ElapsedMilliseconds;
                    sb.AppendFormat("BindColorString\t{0} ms", elapsedMs).AppendLine();

                    watch = System.Diagnostics.Stopwatch.StartNew();
                    CreateMetas();
                    watch.Stop();
                    elapsedMs = watch.ElapsedMilliseconds;
                    sb.AppendFormat("CreateMetas\t{0} ms", elapsedMs).AppendLine();
                    #endregion Do Not change the sequence
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, String.Format("Page_load({0})", modelQuerystring));

            }
            finally
            {
                //Notifications.ErrorClass obj = new Notifications.ErrorClass(new Exception("Mobile Model Page - Performance in ms"), sb.ToString());
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
                            {
                                if (isOnRoadPrice)
                                    currentTextBox.Text = Bikewale.Utility.Format.FormatPrice(Convert.ToString(selected.OnRoadPrice));
                                else
                                    currentTextBox.Text = Bikewale.Utility.Format.FormatPrice(Convert.ToString(selected.Price));
                            }
                        }
                        else if (cityId > 0) // no dealer or bikewale price found
                        {
                            currentTextBox.Text = "N/A";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, "versions.aspx -> rptVarients_ItemDataBound()");
            }
        }

        #endregion Events

        #region Methods

        /// <summary>
        /// Created By:Sangram Nandkhile on 07 Dec 2016
        /// Summary: Detect device and redirect
        /// </summary>
        private void DetectDevice()
        {
            Form.Action = Request.RawUrl;
            string originalUrl = Request.ServerVariables["HTTP_X_ORIGINAL_URL"];
            if (String.IsNullOrEmpty(originalUrl))
                originalUrl = Request.ServerVariables["URL"];
            DeviceDetection dd = new DeviceDetection(originalUrl);
            dd.DetectDevice();
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
                string versionDescirption = modelPageEntity.ModelVersions.Count > 1 ? string.Format(" It is available in {0} versions", modelPageEntity.ModelVersions.Count) : string.Format(" It is available in {0} version", modelPageEntity.ModelVersions.Count);
                string specsDescirption = string.Empty;
                string priceDescription = modelPageEntity.ModelDetails.MinPrice > 0 ? string.Format("Price - Rs. {0} onwards (Ex-showroom, {1}).", Bikewale.Utility.Format.FormatPrice(Convert.ToString(modelPageEntity.ModelDetails.MinPrice)), Bikewale.Utility.BWConfiguration.Instance.DefaultName) : string.Empty;
                if (modelPageEntity != null && modelPageEntity.ModelVersionSpecs != null && (modelPageEntity.ModelVersionSpecs.TopSpeed > 0 || modelPageEntity.ModelVersionSpecs.FuelEfficiencyOverall > 0))
                {
                    if ((modelPageEntity.ModelVersionSpecs.TopSpeed > 0 && modelPageEntity.ModelVersionSpecs.FuelEfficiencyOverall > 0))
                        specsDescirption = string.Format("{0} has a mileage of {1} kmpl and a top speed of {2} kmph.", bikeModelName, modelPageEntity.ModelVersionSpecs.FuelEfficiencyOverall, modelPageEntity.ModelVersionSpecs.TopSpeed);
                    else if (modelPageEntity.ModelVersionSpecs.TopSpeed == 0)
                    {
                        specsDescirption = string.Format("{0} has a mileage of {1} kmpl.", bikeModelName, modelPageEntity.ModelVersionSpecs.FuelEfficiencyOverall);
                    }
                    else
                    {
                        specsDescirption = string.Format("{0} has a top speed of {1} kmph.", bikeModelName, modelPageEntity.ModelVersionSpecs.TopSpeed);
                    }
                }
                summaryDescription = string.Format("{0} {1}{2}.{3}{4}", bikeName, priceDescription, versionDescirption, specsDescirption, colorStr);
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, "versions.aspx --> BindDescription()");

            }
        }

        /// <summary>
        /// Created By :-Subodh Jain 07 oct 2016
        /// Desc:- values to controls field
        /// Modified by :  Subodh Jain on 21 Dec 2016
        /// Description :  Added dealer card and service center card
        /// Modified by :   Sumit Kate on 02 Jan 2017
        /// Description :   Set makename,modelname,make and model masking name to news widget
        /// </summary>
        private void BindControls(BikeModelPageEntity modelPage)
        {
            try
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

                ctrlDealerCard.MakeId = Convert.ToUInt32(modelPage.ModelDetails.MakeBase.MakeId);
                ctrlDealerCard.makeName = modelPage.ModelDetails.MakeBase.MakeName;
                ctrlDealerCard.makeMaskingName = modelPage.ModelDetails.MakeBase.MaskingName;
                ctrlDealerCard.CityId = cityId;
                ctrlDealerCard.TopCount = Convert.ToUInt16(cityId > 0 ? 3 : 6);
                ctrlDealerCard.pageName = "Model_Page";
                ctrlDealerCard.widgetHeading = string.Format("{0} showrooms in {1}", modelPage.ModelDetails.MakeBase.MakeName, cityName);

                ctrlServiceCenterCard.MakeId = Convert.ToUInt32(modelPage.ModelDetails.MakeBase.MakeId);
                ctrlServiceCenterCard.CityId = cityId;
                ctrlServiceCenterCard.makeName = modelPage.ModelDetails.MakeBase.MakeName;
                ctrlServiceCenterCard.cityName = cityName;
                ctrlServiceCenterCard.makeMaskingName = modelPage.ModelDetails.MakeBase.MaskingName;
                ctrlServiceCenterCard.TopCount = 3;
                ctrlServiceCenterCard.widgetHeading = string.Format("{0} service centers in {1}", modelPage.ModelDetails.MakeBase.MakeName, cityName);

                if (!isDiscontinued)
                    ctrlPopularCompare.versionId = Convert.ToString(variantId);

                if (modelPage != null && modelPage.ModelDetails != null)
                {
                    int _modelId = Convert.ToInt32(modelId);
                    ctrlNews.TotalRecords = 3;
                    ctrlNews.ModelId = _modelId;
                    ctrlNews.WidgetTitle = bikeName;
                    ctrlNews.MakeMaskingName = modelPage.ModelDetails.MakeBase.MaskingName;
                    ctrlNews.ModelMaskingName = modelPage.ModelDetails.MaskingName;
                    ctrlNews.MakeName = modelPage.ModelDetails.MakeBase.MakeName;
                    ctrlNews.ModelName = modelPage.ModelDetails.ModelName;

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

                    ctrlAlternativeBikes.TopCount = 9;
                    ctrlAlternativeBikes.PQSourceId = (int)PQSourceEnum.Desktop_ModelPage_Alternative;
                    ctrlAlternativeBikes.WidgetTitle = bikeName;
                    ctrlAlternativeBikes.model = modelPage.ModelDetails.ModelName;
                    ctrlAlternativeBikes.cityId = cityId;
                    ctrlAlternativeBikes.customHeading = "More info about similar bikes";
                    if (modelPage != null)
                    {
                        var modelVersions = modelPage.ModelVersions;
                        if (modelVersions != null && modelVersions.Count > 0)
                        {
                            ctrlAlternativeBikes.VersionId = Convert.ToUInt32(modelVersions[0].VersionId);
                        }
                    }
                    GetBikeRankingCategory((uint)_modelId);
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, string.Format("versions.aspx --> CreateMetas() ModelId: {0}, MaskingName: {1}", modelId, modelQuerystring));

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
        /// Created By :-Subodh Jain 07 oct 2016
        /// Desc:- Metas description according to discountinue,upcoming,continue bikes
        /// </summary>
        private void CreateMetas()
        {
            try
            {
                totalUsedBikes = TotalUsedBikes(modelId, cityId);
                if (modelPageEntity.ModelDetails.Futuristic)
                {
                    pgDescription = string.Format("{0} {1} Price in India is expected between Rs. {2} and Rs. {3}. Check out {0} {1}  specifications, reviews, mileage, versions, news & images at BikeWale.com. Launch date of {1} is around {4}", modelPageEntity.ModelDetails.MakeBase.MakeName, modelPageEntity.ModelDetails.ModelName, Bikewale.Utility.Format.FormatNumeric(Convert.ToString(modelPageEntity.UpcomingBike.EstimatedPriceMin)), Bikewale.Utility.Format.FormatNumeric(Convert.ToString(modelPageEntity.UpcomingBike.EstimatedPriceMax)), modelPageEntity.UpcomingBike.ExpectedLaunchDate);
                }
                else if (!modelPageEntity.ModelDetails.New)
                {
                    pgDescription = string.Format("{0} {1} Price in India - Rs. {2}. It has been discontinued in India. There are {3} used {1} bikes for sale. Check out {1} specifications, reviews, mileage, versions, news & images at BikeWale.com", modelPageEntity.ModelDetails.MakeBase.MakeName, modelPageEntity.ModelDetails.ModelName, Bikewale.Utility.Format.FormatNumeric(price.ToString()), totalUsedBikes);
                }
                else
                {
                    pgDescription = String.Format("{0} Price in India - Rs. {1}. Find {2} Reviews, Specs, Features, Mileage, On Road Price and Images at Bikewale. {3}", bikeName, Bikewale.Utility.Format.FormatNumeric(price.ToString()), bikeModelName, colorStr);
                }
                BindDescription();
                SetFlagsAtEnd();
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, string.Format("versions.aspx --> CreateMetas() ModelId: {0}, MaskingName: {1}", modelId, modelQuerystring));
            }
        }

        /// <summary>
        /// Author          :   Sangram Nandkhile
        /// Created Date    :   20 Nov 2015
        /// Description     :   To Load version dropdown at Specs for each variant
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
                        if (modelPg.ModelVersions.Count == 1)
                        {
                            var firstVer = modelPg.ModelVersions.FirstOrDefault();
                            if (firstVer != null)
                                variantText = firstVer.VersionName;
                        }
                        else if (modelPg.ModelVersions.Count > 1)
                        {
                            // bind dropdwon version List
                            ddlVersion.DataSource = modelPg.ModelVersions;
                            ddlVersion.DataValueField = "VersionId";
                            ddlVersion.DataTextField = "VersionName";
                            if (variantId > 0)
                                ddlVersion.SelectedValue = variantId.ToString();
                            ddlVersion.DataBind();
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
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, string.Format("versions.aspx --> LoadVariants() ModelId: {0}, MaskingName: {1}", modelId, modelQuerystring));
            }
        }

        /// <summary>
        /// Modified By : Sajal Gupta on 01-03-2017
        /// Description : Removed repeater rptcolor.
        /// </summary>
        /// <param name="modelPage"></param>
        private void BindPhotoRepeater(BikeModelPageEntity modelPage)
        {
            try
            {
                if (modelPage != null)
                {
                    var photos = modelPage.AllPhotos;
                    if (photos != null && photos.Count() > 0)
                    {
                        rptModelPhotos.DataSource = photos;
                        rptModelPhotos.DataBind();
                        rptNavigationPhoto.DataSource = photos;
                        rptNavigationPhoto.DataBind();
                    }
                    if (!String.IsNullOrEmpty(modelPage.ModelDetails.OriginalImagePath))
                    {
                        modelImage = Utility.Image.GetPathToShowImages(modelPage.ModelDetails.OriginalImagePath, modelPage.ModelDetails.HostUrl, Bikewale.Utility.ImageSize._476x268);
                    }
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, "BindPhotoRepeater");
            }
        }

        private void ParseQueryString()
        {
            modelQuerystring = Request.QueryString["model"];
            ModelMaskingResponse objResponse = null;
            try
            {
                string VersionIdStr = Request.QueryString["vid"];
                if (!string.IsNullOrEmpty(VersionIdStr))
                {
                    UInt32.TryParse(VersionIdStr, out variantId);
                }
                if (!string.IsNullOrEmpty(modelQuerystring))
                {
                    objResponse = new ModelHelper().GetModelDataByMasking(modelQuerystring);
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, Request.ServerVariables["URL"] + "ParseQueryString");


                Response.Redirect("/new-bikes-in-india/", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
                this.Page.Visible = false;
            }
            finally
            {
                if (!string.IsNullOrEmpty(modelQuerystring))
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
                            CommonOpn.RedirectPermanent(Request.RawUrl.Replace(modelQuerystring, objResponse.MaskingName));
                        }
                        else
                        {
                            Response.Redirect(CommonOpn.AppPath + "pageNotFound.aspx", false);
                            HttpContext.Current.ApplicationInstance.CompleteRequest();
                            this.Page.Visible = false;
                        }
                    }
                    else
                    {
                        Response.Redirect(CommonOpn.AppPath + "pageNotFound.aspx", false);
                        HttpContext.Current.ApplicationInstance.CompleteRequest();
                        this.Page.Visible = false;
                    }
                }
                else
                {
                    Response.Redirect(CommonOpn.AppPath + "pageNotFound.aspx", false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                    this.Page.Visible = false;
                }
            }
        }
        /// <summary>
        /// Author          :   Sangram Nandkhile
        /// Created Date    :   18 Nov 2015
        /// Modified by : Sajal Gupta on 28-02-2017
        /// Description : Get model page data from calling BAL layer instead of calling cache layer.
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
                        container.RegisterType<IPager, Pager>()
                            .RegisterType<IBikeModelsCacheRepository<int>, BikeModelsCacheRepository<BikeModelEntity, int>>()
                                 .RegisterType<IBikeModels<BikeModelEntity, int>, BikeModels<BikeModelEntity, int>>()
                                 .RegisterType<IBikeModelsRepository<BikeModelEntity, int>, BikeModelsRepository<BikeModelEntity, int>>()
                                 .RegisterType<ICacheManager, MemcacheManager>();
                        var objBikeModels = container.Resolve<IBikeModels<BikeModelEntity, int>>();
                        modelPg = objBikeModels.GetModelPageDetails(Convert.ToInt16(modelID), (int)variantId);
                        if (modelPg != null)
                        {
                            if (!modelPg.ModelDetails.Futuristic && modelPg.ModelVersionSpecs != null)
                            {
                                if (cityId == 0)
                                    price = Convert.ToUInt32(modelPg.ModelDetails.MinPrice);

                                if (variantId == 0 && cityId == 0)
                                {
                                    variantId = modelPg.ModelVersionSpecs.BikeVersionId;
                                }
                                // Check it versionId passed through url exists in current model's versions
                                else if (variantId > 0 && !modelPg.ModelVersions.Exists(p => p.VersionId == variantId))
                                {
                                    variantId = modelPg.ModelVersionSpecs.BikeVersionId;
                                }
                            }

                            if (modelPg.ModelDetails != null)
                            {
                                if (modelPg.ModelDetails.ModelName != null)
                                    bikeModelName = modelPg.ModelDetails.ModelName;
                                if (modelPg.ModelDetails.MakeBase != null)
                                    bikeMakeName = modelPg.ModelDetails.MakeBase.MakeName;
                                bikeName = string.Format("{0} {1}", bikeMakeName, bikeModelName);
                            }

                            // Discontinued bikes
                            if (!modelPg.ModelDetails.New && modelPg.ModelVersions != null)
                            {
                                isDiscontinued = true;
                                if (modelPg.ModelVersions.Count == 1)
                                {
                                    price = Convert.ToUInt32(modelPg.ModelDetails.MinPrice);
                                }
                                else
                                {
                                    // When version is not selected
                                    if (variantId == 0)
                                    {
                                        List<BikeVersionMinSpecs> nonZeroValues = modelPg.ModelVersions.Where(x => x.Price > 0).ToList();
                                        if (nonZeroValues != null && nonZeroValues.Count > 0)
                                        {
                                            ulong minVal = nonZeroValues.Min(x => x.Price);
                                            var lowestVersion = modelPg.ModelVersions.First(x => x.Price == minVal);
                                            if (lowestVersion != null)
                                            {
                                                variantId = Convert.ToUInt16(lowestVersion.VersionId);
                                                price = Convert.ToUInt32(lowestVersion.Price);
                                            }
                                        }
                                    }
                                    else //When version is selected
                                    {
                                        BikeVersionMinSpecs selectedVersion = modelPg.ModelVersions.FirstOrDefault(x => x.VersionId == variantId);
                                        if (selectedVersion != null)
                                        {
                                            price = Convert.ToUInt32(selectedVersion.Price);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("versions.aspx -> FetchmodelPgDetails(): Modelid ==> {0}", modelId));
            }
            return modelPg;
        }

        /// <summary>
        /// Author          :   Sangram Nandkhile
        /// Created Date    :   20 Nov 2015
        /// Description     :   Fetch On road price depending on City, Area and DealerPQ and BWPQ
        /// Modified By     :   Sushil Kumar on 19th April 2016
        /// Description     :   Removed repeater binding for rptCategory and rptDiscount as view breakup popup removed
        /// Modified by     :   Sajal Gupta on 13-01-2017
        /// Description     :   Changed flag isOnRoadPrice if onroad price not available
        /// Modifide By :- Subodh jain on 02 March 2017
        /// Summary:- added manufacturer campaign leadpopup changes
        /// </summary>
        private void FetchOnRoadPrice(BikeModelPageEntity modelPage)
        {
            var errorParams = string.Empty;
            try
            {
                if (cityId > 0)
                {
                    pqOnRoad = GetOnRoadPrice();
                    // Set Pricequote Cookie
                    if (pqOnRoad != null)
                    {
                        if (pqOnRoad.BPQOutput != null && !String.IsNullOrEmpty(pqOnRoad.BPQOutput.ManufacturerAd))
                            pqOnRoad.BPQOutput.ManufacturerAd = Format.FormatManufacturerAd(pqOnRoad.BPQOutput.ManufacturerAd, pqOnRoad.BPQOutput.CampaignId, pqOnRoad.BPQOutput.ManufacturerName, pqOnRoad.BPQOutput.MaskingNumber, Convert.ToString(pqOnRoad.BPQOutput.ManufacturerId), pqOnRoad.BPQOutput.Area, pq_leadsource, pq_sourcepage, string.Empty, string.Empty, string.Empty, string.IsNullOrEmpty(pqOnRoad.BPQOutput.MaskingNumber) ? "hide" : string.Empty, pqOnRoad.BPQOutput.LeadCapturePopupHeading, pqOnRoad.BPQOutput.LeadCapturePopupDescription, pqOnRoad.BPQOutput.LeadCapturePopupMessage, pqOnRoad.BPQOutput.PinCodeRequired);
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
                            // Commented on 20 Feb 2017
                            // if (pqOnRoad.PriceQuote.PQId == 0)
                            //    isOnRoadPrice = false;

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
                            else // Show dealer properties and Bikewale priceQuote when dealer has pricing for any of the bike
                            // Added on 13 Feb 2017 Pivotal Id:138698777
                            {
                                SetBikeWalePQ(pqOnRoad);
                            }
                            #endregion when dealer Price is Available
                        }
                        else
                        {
                            SetBikeWalePQ(pqOnRoad);
                        }
                    }
                    else // On road PriceQuote is Null so get price from the modelpage variants
                    {
                        if (variantId != 0)
                        {
                            var modelVersions = modelPage.ModelVersions.FirstOrDefault(p => p.VersionId == variantId);
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
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, "versions.aspx -> FetchOnRoadPrice() " + " ===== parameters ========= " + errorParams);
            }
        }
        /// <summary>
        /// Created by : Sangram Nandkhile on 14 Feb 2017
        /// Summary: To set price variable with bikewale pricequote
        /// </summary>
        /// <param name="pqOnRoad"></param>
        private void SetBikeWalePQ(PQOnRoadPrice pqOnRoad)
        {
            if (pqOnRoad.BPQOutput != null && pqOnRoad.BPQOutput.Varients != null && variantId > 0)
            {
                objSelectedVariant = pqOnRoad.BPQOutput.Varients.Where(p => p.VersionId == variantId).FirstOrDefault();
                if (objSelectedVariant != null)
                    price = isOnRoadPrice ? Convert.ToUInt32(objSelectedVariant.OnRoadPrice) : Convert.ToUInt32(objSelectedVariant.Price);

                campaignId = pqOnRoad.BPQOutput.CampaignId;
                manufacturerId = pqOnRoad.BPQOutput.ManufacturerId;
                isBikeWalePQ = true;
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
                    PQOutputEntity objPQOutput = objDealer.ProcessPQV2(objPQEntity);

                    if (objPQOutput != null)
                    {
                        if (variantId == 0)
                            variantId = objPQOutput.VersionId;
                        defaultVersionId = objPQOutput.DefaultVersionId;
                        pqOnRoad = new PQOnRoadPrice();
                        pqOnRoad.PriceQuote = objPQOutput;
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
                                }
                                catch (Exception ex)
                                {
                                    Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, string.Format("versions.aspx --> GetOnRoadPrice() ModelId: {0}, MaskingName: {1}", modelId, modelQuerystring));
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, string.Format("versions.aspx --> GetOnRoadPrice() ModelId: {0}, MaskingName: {1}", modelId, modelQuerystring));
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
                        isOnRoadPrice = true;
                }
                else
                {
                    isOnRoadPrice = true;
                }
            }
            else if (cityId > 0 && (!isAreaAvailable))
            {
                isOnRoadPrice = true;
            }
            // if city and area is not selected OR if city is selected & area is available but not selected
            //if (isCityAvailable && ((!isCitySelected) || (isCitySelected && isAreaAvailable && !isAreaSelected)))
            {
                //toShowOnRoadPriceButton = true;
                toShowOnRoadPriceButton = !isOnRoadPrice;
            }
            if (!isDiscontinued)
            {
                if (cityId > 0)
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
            if (modelId > 0)
            {
                string location = String.Empty;
                var cookies = this.Context.Request.Cookies;
                // Check if Model has price in any of the cities, This will return list of cities in which model has prices
                objCityList = new ModelHelper().GetCitiesByModelId(modelId);
                isCityAvailable = (objCityList != null && objCityList.Count() > 0);
                if (cookies.AllKeys.Contains("location"))
                {
                    location = cookies["location"].Value;
                    if (!String.IsNullOrEmpty(location) && location.IndexOf('_') != -1)
                    {
                        string[] locArray = location.Split('_');
                        if (locArray.Length > 0)
                        {
                            UInt32.TryParse(locArray[0], out cityId);
                            if (isCityAvailable)
                            {
                                // If Model doesn't have current City then don't show it, Show Ex-showroom Mumbai
                                var firstCity = objCityList.FirstOrDefault(p => p.CityId == cityId);
                                if (firstCity != null)
                                {
                                    cityName = firstCity.CityName;
                                    isCitySelected = true;

                                    // This function will check if Areas are available for city and Model
                                    objAreaList = new ModelHelper().GetAreaForModelAndCity(modelId, cityId);
                                    isAreaAvailable = (objAreaList != null && objAreaList.Count() > 0);

                                    // locArray.Length = 4 Means City and area exists
                                    if (isAreaAvailable && locArray.Length > 3 && cityId > 0)
                                    {
                                        if (UInt32.TryParse(locArray[2], out areaId))
                                        {
                                            var firstArea = objAreaList.FirstOrDefault(p => p.AreaId == areaId);
                                            if (firstArea != null && isAreaAvailable)
                                            {
                                                areaName = firstArea.AreaName;
                                                isAreaSelected = true;
                                            }
                                        }
                                    }
                                }
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
        /// <summary>
        /// Author: Sangram Nandkhile
        /// Created on: 09 Dec 2016
        /// Desc: Set flags for aspx mark up to show and hide buttons, insurance links
        /// </summary>
        private void SetFlagsAtEnd()
        {
            if (isOnRoadPrice && price > 0)
            {
                detailedPriceLink = PriceQuoteQueryString.FormBase64QueryString(cityId.ToString(), pqId, areaId.ToString(), variantId.ToString(), dealerId.ToString());
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
                    viewModel = new ModelPageVM(cityId, defaultVersionId, dealerId, areaId);
                    if (viewModel.Offers != null && viewModel.Offers.Count() > 0)
                    {
                        rptOffers.DataSource = viewModel.Offers;
                        rptOffers.DataBind();
                        isOfferAvailable = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, string.Format("versions.aspx --> FillViewModel() ModelId: {0}, MaskingName: {1}", modelId, modelQuerystring));
            }
        }

        private void BindColorString()
        {
            try
            {
                if (modelPageEntity != null && modelPageEntity.ModelColors != null && modelPageEntity.ModelColors.Count() > 0)
                {
                    colorCount = modelPageEntity.ModelColors.Count();
                    string lastColor = modelPageEntity.ModelColors.Last().ColorName;
                    if (colorCount > 1)
                    {
                        colorStr.AppendFormat("{0} is available in {1} different colours : ", bikeName, colorCount);
                        var colorArr = modelPageEntity.ModelColors.Select(x => x.ColorName).Take(colorCount - 1);
                        // Comma separated colors (except last one)
                        colorStr.Append(string.Join(",", colorArr));
                        // Append last color with And
                        colorStr.AppendFormat(" and {0}.", lastColor);
                    }
                    else if (colorCount == 1)
                    {
                        colorStr.AppendFormat("{0} is available in {1} colour.", bikeName, lastColor);
                    }
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, "versions.aspx -->" + "BindColorString()");
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
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, string.Format("versions.aspx --> TotalUsedBikes() --> modelId: {0}, cityId: {1}", modelId, cityId));
            }
            return totalUsedBikes;
        }

        #endregion Methods

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

        #endregion enums
    }
}
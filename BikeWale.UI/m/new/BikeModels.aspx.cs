using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.UI.WebControls;
using Bikewale.Cache.BikeData;
using Bikewale.Cache.Core;
using Bikewale.Common;
using Bikewale.DAL.BikeData;
using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Mobile.Controls;
using Microsoft.Practices.Unity;
using Bikewale.m.controls;
using Bikewale.DTO.Version;
using System.Reflection;
using Bikewale.Mobile.PriceQuote;
using Bikewale.BAL.BikeBooking;
using Bikewale.Interfaces.BikeBooking;
using Bikewale.Entities.Location;
using Bikewale.Interfaces.Location;
using Bikewale.DAL.Location;
using Bikewale.BAL.BikeData;
using Bikewale.Entities.CMS.Photos;
using Bikewale.Interfaces.PriceQuote;
using Bikewale.Entities.PriceQuote;
using Bikewale.Entities.BikeBooking;

namespace Bikewale.Mobile.New
{
    /// <summary>
    /// Created By : Ashish G. Kamble on 9 Sept 2015    
    /// </summary>
    public class NewBikeModels : System.Web.UI.Page
    {
        // Register controls
        protected AlternativeBikes ctrlAlternateBikes;
        protected NewsWidget ctrlNews;
        protected ExpertReviewsWidget ctrlExpertReviews;
        protected VideosWidget ctrlVideos;
        protected UserReviewList ctrlUserReviews;
        protected ModelGallery ctrlModelGallery;
        // Register global variables
        protected BikeModelPageEntity modelPage;
        protected string modelId = string.Empty;
        protected Repeater rptModelPhotos, rptVarients, rptColors;
        protected String bikeName = String.Empty;
        protected String clientIP = string.Empty;
        protected int cityId = 0;
        protected short reviewTabsCnt = 0;
        //Variable to Assing ACTIVE class
        protected bool isUserReviewActive = false, isExpertReviewActive = false, isNewsActive = false, isVideoActive = false;
        //Varible to Hide or show controlers

        protected bool isCitySelected, isAreaSelected, isOnRoadPrice, toShowOnRoadPriceButton;
        protected bool isUserReviewZero = true, isExpertReviewZero = true, isNewsZero = true, isVideoZero = true;

        protected static bool isManufacturer = false;

        // New Model Revamp
        protected bool isBookingAvailable, isOfferAvailable, isBikeWalePQ, isDiscontinued, isAreaAvailable;
        protected Repeater rptOffers, rptMoreOffers, rptCategory, rptVariants;
        static readonly string _bwHostUrl, _PageNotFoundPath;
        protected VersionSpecifications bikeSpecs;
        protected int variantId = 0;
        protected int versionId = 0;
        protected PQOnRoadPrice pqOnRoad;
        protected int areaId = 0;
        protected string cityName = string.Empty;
        protected string areaName = string.Empty;
        protected DropDownList ddlVariant;
        protected string variantText = string.Empty;
        protected uint bookingAmt = 0;
        protected int grid1_size = 9;
        protected int grid2_size = 3;
        protected int btMoreDtlsSize = 12;
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

        #region Events
        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
            //ddlVariant.SelectedIndexChanged += new EventHandler(ddlVariant_SelectedIndexChanged);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            #region Do not change the sequence
            ParseQueryString();
            CheckCityCookie();
            if (hdnVariant.Value != "0")
                variantId = Convert.ToInt32(hdnVariant.Value);

            #endregion
            FetchModelPageDetails();
            if (!IsPostBack)
            {
                #region Do not change the sequence of these functions

                BindRepeaters();
                //BindModelGallery();
                BindAlternativeBikeControl();
                clientIP = CommonOpn.GetClientIP();
                LoadVariants();
                #endregion

                ////news,videos,revews, user reviews
                ctrlNews.TotalRecords = 3;
                ctrlNews.ModelId = Convert.ToInt32(modelId);

                ctrlExpertReviews.TotalRecords = 3;
                ctrlExpertReviews.ModelId = Convert.ToInt32(modelId);

                ctrlVideos.TotalRecords = 3;
                ctrlVideos.ModelId = Convert.ToInt32(modelId);

                ctrlUserReviews.ReviewCount = 4;
                ctrlUserReviews.PageNo = 1;
                ctrlUserReviews.PageSize = 4;
                ctrlUserReviews.ModelId = Convert.ToInt32(modelId);

                ctrlExpertReviews.MakeMaskingName = modelPage.ModelDetails.MakeBase.MaskingName.Trim();
                ctrlExpertReviews.ModelMaskingName = modelPage.ModelDetails.MaskingName.Trim();
            }
            else
            {
                //modelPage = (ModelPage)Session["modelPage"];
                //if (ViewState["modelPage"] != null)
                //{
                //    string json = (string)ViewState["modelPage"];
                //    modelPage = JsonConvert.DeserializeObject<ModelPage>(json);
                //}

                if (modelPage.ModelVersions != null && modelPage.ModelVersions.Count > 0)
                {
                    rptVarients.DataSource = modelPage.ModelVersions;
                    rptVarients.DataBind();
                }
            }
            if (modelPage.ModelDetails != null)
                bikeName = modelPage.ModelDetails.MakeBase.MakeName + ' ' + modelPage.ModelDetails.ModelName;
            if (modelPage.ModelDetails.New)
            {
                FetchOnRoadPrice();
            }
            ToggleOfferDiv();
            SetFlags();
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

        protected void btnVariant_Command(object sender, CommandEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.CommandName))
            {
                variantId = Convert.ToInt32(e.CommandName);
                FetchVariantDetails(variantId);
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
                        if ((isCitySelected && !isAreaAvailable))
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

        #endregion

        #region methods

        #endregion

        static NewBikeModels()
        {
            isManufacturer = (ConfigurationManager.AppSettings["TVSManufacturerId"] != "0") ? true : false;
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
            ctrlAlternateBikes.TopCount = 6;

            if (modelPage.ModelVersions != null && modelPage.ModelVersions.Count > 0)
            {
                ctrlAlternateBikes.VersionId = modelPage.ModelVersions[0].VersionId;
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
                    //if (modelPage.Photos.Count > 2)
                    //{
                    //    rptModelPhotos.DataSource = modelPage.Photos.Take(3);
                    //}
                    //else
                    //{
                    //    rptModelPhotos.DataSource = modelPage.Photos;
                    //}
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
        /// </summary>
        private void ParseQueryString()
        {
            ModelMaskingResponse objResponse = null;
            try
            {
                if (!string.IsNullOrEmpty(Request.QueryString["model"]))
                {
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
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"] + " : FetchModelPageDetails");
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
                            modelId = objResponse.ModelId.ToString();
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

        private void CheckCityCookie()
        {
            // Read current cookie values
            // Check if there are areas for current model and City
            // If No then drop area cookie
            string location = String.Empty;
            var cookies = this.Context.Request.Cookies;
            objAreaList = GetAreaForCityAndModel();
            if (cookies.AllKeys.Contains("location"))
            {
                location = cookies["location"].Value;
                if (!String.IsNullOrEmpty(location) && location.IndexOf('_') != -1)
                {
                    string[] locArray = location.Split('_');
                    if (locArray.Length > 0)
                    {
                        cityId = Convert.ToInt16(locArray[0]); //location.Substring(0, location.IndexOf('_'));
                        objCityList = FetchCityByModelId(modelId);

                        // If Model doesn't have current City then don't show it, Show Ex-showroom Mumbai
                        if (objCityList != null && !objCityList.Any(p => p.CityId == cityId))
                        {
                            cityId = 0;
                        }
                        else
                        {
                            cityName = locArray[1];
                            isCitySelected = true;
                        }
                        if (GetAreaForCityAndModel() != null)
                        {
                           // isAreaAvailable = true;
                        }
                    }
                    if (locArray.Length > 3 && cityId != 0)
                    {
                        areaId = Convert.ToInt16(locArray[2]);
                        objAreaList = GetAreaForCityAndModel();
                        if (objAreaList != null)
                        {
                            isAreaAvailable = true;
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

        /// <summary>
        /// Author          :   Sangram Nandkhile
        /// Created Date    :   27 Nov 2015
        /// Description     :   To Load Specs for each variant
        /// </summary>
        private void LoadVariants()
        {
            if (modelPage.ModelVersions != null && !modelPage.ModelDetails.Futuristic)
            {
                //ddlVariant.Items.Clear();
                if (modelPage.ModelVersions != null && modelPage.ModelVersions.Count > 1)
                {
                    foreach (var version in modelPage.ModelVersions)
                    {
                        // ddlVariant.Items.Insert(0, new ListItem(version.VersionName, version.VersionId.ToString()));
                    }
                    if (modelPage.ModelVersionSpecs != null && modelPage.ModelVersionSpecs.BikeVersionId != 0)
                    {
                        //ddlVariant.SelectedValue = Convert.ToString(modelPage.ModelVersionSpecs.BikeVersionId);
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

                //int curVersion = versionId == 0? Convert.ToInt32(modelPage.ModelVersionSpecs.BikeVersionId): versionId;
            }
        }

        static readonly string apiURL = "/api/model/details/?modelId={0}&variantId={1}";
        static readonly string onRoadApi = "/api/OnRoadPrice/?cityId={0}&modelId={1}&clientIP={2}&sourceType={3}&areaId={4}";
        static readonly string _requestType = "application/json";
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
        /// Created Date    :   27 Nov 2015
        /// Description     :   Fetch On road price depending on City, Area and DealerPQ and BWPQ
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
                        if (pqOnRoad.PriceQuote != null)
                        {
                            dealerId = Convert.ToString(pqOnRoad.PriceQuote.DealerId);
                            pqId = Convert.ToString(pqOnRoad.PriceQuote.PQId);
                        }
                        PriceQuoteCookie.SavePQCookie(cityId.ToString(), pqId, Convert.ToString(areaId), Convert.ToString(variantId), dealerId);
                        if (pqOnRoad.IsDealerPriceAvailable && pqOnRoad.DPQOutput != null && pqOnRoad.DPQOutput.Varients!=null && pqOnRoad.DPQOutput.Varients.Count() > 0)
                        {
                            #region when dealer Price is Available
                            // Select Variant for which details need to be shown
                            var selectedVariant = pqOnRoad.DPQOutput.Varients.Where(p => p.objVersion.VersionId == variantId).FirstOrDefault();
                            if (selectedVariant != null)
                            {
                                onRoadPrice = selectedVariant.OnRoadPrice;
                                price = onRoadPrice.ToString();
                                if (pqOnRoad.DPQOutput.objOffers!=null && pqOnRoad.DPQOutput.objOffers.Count > 0)
                                {
                                    IEnumerable<OfferEntity> moreOffers = null;
                                    rptOffers.DataSource = pqOnRoad.DPQOutput.objOffers.Take<OfferEntity>(2);
                                    //ONROAD.DPQOutput.objOffers.Take<DPQOfferBase>(2);
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
                            #region BikeWale PQ
                            if (pqOnRoad.BPQOutput != null && pqOnRoad.BPQOutput.Varients!= null)
                            {
                                if (hdnVariant.Value != "0")
                                {
                                    variantId = Convert.ToInt32(hdnVariant.Value);
                                    if (variantId != 0)
                                    {

                                        objSelectedVariant = pqOnRoad.BPQOutput.Varients.Where(p => p.VersionId == variantId).FirstOrDefault();
                                        price = Convert.ToString(objSelectedVariant.OnRoadPrice);
                                    }
                                }
                                else
                                {
                                    objSelectedVariant = pqOnRoad.BPQOutput.Varients.FirstOrDefault();
                                    price = Convert.ToString(objSelectedVariant.OnRoadPrice);
                                }
                                isBikeWalePQ = true;
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
                        if (variantId != 0)
                        {
                            price = Convert.ToString(modelPage.ModelVersions.Where(p => p.VersionId == variantId).FirstOrDefault().Price);
                        }
                        else
                        {
                            if (modelPage.ModelDetails!=null)
                              price = Convert.ToString(modelPage.ModelDetails.MinPrice);
                        }
                    }
                }
                else
                {
                    price = Convert.ToString(modelPage.ModelVersions.Where(p => p.VersionId == variantId).FirstOrDefault().Price);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"] + "-"+ MethodBase.GetCurrentMethod().Name);
                objErr.SendMail();
            }
        }

        /// <summary>
        /// Author          :   Sangram Nandkhile
        /// Created Date    :   27 Nov 2015
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
            if (bookingAmt > 0)
                btMoreDtlsSize = 7;
        }

        /// <summary>
        /// Author          :   Sangram Nandkhile
        /// Created Date    :   27 Nov 2015
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
        /// Created Date    :   27 Nov 2015
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
        /// Created Date    :   27 Nov 2015
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
                    objPQEntity.SourceId = 0;
                    objPQEntity.ModelId = Convert.ToUInt32(modelId);
                    objPQEntity.VersionId = Convert.ToUInt32(variantId);
                    PQOutputEntity objPQOutput = objDealer.ProcessPQ(objPQEntity);
                    if (objPQOutput != null)
                    {
                        pqOnRoad = new PQOnRoadPrice();
                        pqOnRoad.PriceQuote = objPQOutput;
                        BikeModelEntity bikemodelEnt = objClient.GetById(Convert.ToInt32(modelId));
                        pqOnRoad.BikeDetails = bikemodelEnt;

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
                                    string api = String.Format("/api/DealerPriceQuote/GetDealerPriceQuote/?cityid={0}&versionid={1}&dealerid={2}", cityId, variantId, objPQOutput.DealerId);
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
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {

                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

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
                    //else // Is area available and Not selected
                    //{

                    //}
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
    }
}
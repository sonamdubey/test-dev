using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Bikewale.Cache.BikeData;
using Bikewale.Cache.Core;
using Bikewale.Common;
using Bikewale.DAL.BikeData;
using Bikewale.DTO.Model;
using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Mobile.Controls;
using Microsoft.Practices.Unity;
using Bikewale.m.controls;
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
        protected ModelPage modelPage;
        protected string modelId = string.Empty;
        protected Repeater rptModelPhotos, rptVarients, rptColors;
        protected String bikeName = String.Empty;
        protected String clientIP = string.Empty;
        protected String cityId = "0";
        protected short reviewTabsCnt = 0;
        //Variable to Assing ACTIVE class
        protected bool isUserReviewActive = false, isExpertReviewActive = false, isNewsActive = false, isVideoActive = false;
        //Varible to Hide or show controlers

        protected bool isCityAreaSelected = false;
        protected bool isUserReviewZero = true, isExpertReviewZero = true, isNewsZero = true, isVideoZero = true;

        protected static bool isManufacturer = false;

        // New Model Revamp
        protected bool isBookingAvailable, isOfferAvailable, isBikeWalePQ, isDiscontinued, isAreaAvailable;
        protected Repeater rptOffers, rptMoreOffers, rptCategory, rptVariants;
        static readonly string _bwHostUrl, _PageNotFoundPath;
        protected VersionSpecifications bikeSpecs;
        protected int variantId = 0;
        protected int versionId = 0;
        protected PQOnRoad pqOnRoad;
        protected string areaId = "0";
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
        protected PQCityList objCityList = null;
        protected PQAreaList objAreaList = null;
        protected OtherVersionInfoDTO objSelectedVariant = null;
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

            if (!IsPostBack)
            {
                #region Do not change the sequence of these functions
                FetchModelPageDetails();
                BindRepeaters();
                BindModelGallery();
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
                modelPage = (ModelPage)Session["modelPage"];
                if (ViewState["modelPage"] != null)
                {
                    string json = (string)ViewState["modelPage"];
                    modelPage = JsonConvert.DeserializeObject<ModelPage>(json);
                }

                if (modelPage.ModelVersions != null && modelPage.ModelVersions.Count > 0)
                {
                    rptVarients.DataSource = modelPage.ModelVersions;
                    rptVarients.DataBind();
                }
            }
            if (modelPage.ModelDetails != null)
                bikeName = modelPage.ModelDetails.MakeBase.MakeName + ' ' + modelPage.ModelDetails.ModelName;
            FetchOnRoadPrice();
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
                        lblExOn.Text = "On-road price";
                        if (pqOnRoad.IsDealerPriceAvailable)
                        {
                            var selecteVersionList = pqOnRoad.DPQOutput.Varients.Where(p => Convert.ToString(p.objVersion.VersionId) == hdn.Value);
                            if (selecteVersionList != null && selecteVersionList.Count() > 0)
                                currentTextBox.Text = Bikewale.Utility.Format.FormatPrice(Convert.ToString(selecteVersionList.First().OnRoadPrice));
                        }
                        else
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

        
        private void BindModelGallery()
        {
            List<Bikewale.DTO.CMS.Photos.CMSModelImageBase> photos = null;
            if (modelPage != null && modelPage.Photos != null && modelPage.Photos.Count > 0)
            {
                photos = modelPage.Photos;
                photos.Insert(0, new DTO.CMS.Photos.CMSModelImageBase()
                {
                    HostUrl = modelPage.ModelDetails.HostUrl,
                    OriginalImgPath = modelPage.ModelDetails.OriginalImagePath,
                    ImageCategory = bikeName,
                });
                ctrlModelGallery.bikeName = bikeName;
                ctrlModelGallery.modelId = Convert.ToInt32(modelId);
                //ctrlModelGallery.Photos = photos;
            }
        }

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
                        cityId = locArray[0]; //location.Substring(0, location.IndexOf('_'));
                        objCityList = FetchCityByModelId(modelId);

                        // If Model doesn't have current City then don't show it, Show Ex-showroom Mumbai
                        if (objCityList != null && !objCityList.Cities.Any(p => p.CityId.ToString() == cityId))
                        {
                            cityId = "0";
                        }
                        else
                        {
                            cityName = locArray[1];
                            isCityAreaSelected = true;
                        }
                        if (GetAreaForCityAndModel() != null)
                        {
                            isAreaAvailable = true;
                        }
                    }
                    if (locArray.Length > 3 && cityId != "0")
                    {
                        areaId = locArray[2];
                        objAreaList = GetAreaForCityAndModel();
                        if (objAreaList != null)
                        {
                            isAreaAvailable = true;
                            if (!objAreaList.Areas.Any(p => p.AreaId.ToString() == areaId))
                            {
                                areaId = "0";
                            }
                            else
                            {
                                areaName = locArray[3] + ",";
                            }
                        }
                        else
                        {
                            areaId = "0";
                        }
                        objAreaList = GetAreaForCityAndModel();
                    }
                }
            }
        }

        //private void FetchModelPageDetails()
        //{
        //    try
        //    {
        //        string _bwHostUrl = ConfigurationManager.AppSettings["bwHostUrl"];
        //        string _requestType = "application/json";
        //        string _apiUrl = String.Format("/api/model/details/?modelId={0}", modelId);

        //        modelPage = BWHttpClient.GetApiResponseSync<ModelPage>(_bwHostUrl, _requestType, _apiUrl, modelPage);

        //        if (modelPage != null)
        //        {
        //            bikeName = modelPage.ModelDetails.MakeBase.MakeName + ' ' + modelPage.ModelDetails.ModelName;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"] + " : FetchModelPageDetails");
        //        objErr.SendMail();
        //    }
        //}

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
                if (!string.IsNullOrEmpty(modelId))
                {
                    string _apiUrl = String.Format(apiURL, modelId, variantId);
                    using (BWHttpClient objClient = new BWHttpClient())
                    {
                        modelPage = objClient.GetApiResponseSync<ModelPage>(_bwHostUrl, _requestType, _apiUrl, modelPage);
                    }
                    if (modelPage != null)
                    {
                        if (!modelPage.ModelDetails.Futuristic && modelPage.ModelVersionSpecs != null)
                        {
                            price = Convert.ToString(modelPage.ModelDetails.MinPrice);
                            variantId = Convert.ToInt32(modelPage.ModelVersionSpecs.BikeVersionId);
                        }
                        if (!modelPage.ModelDetails.New)
                            isDiscontinued = true;

                        string jsonModel = JsonConvert.SerializeObject(modelPage);
                        ViewState["modelPage"] = jsonModel;
                        Session["modelPage"] = modelPage;
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
                if (!string.IsNullOrEmpty(cityId) && cityId != "0")
                {
                    string _apiUrl = String.Format(onRoadApi, cityId, modelId, null, 0, areaId);
                    using (BWHttpClient objClient = new BWHttpClient())
                    {
                        pqOnRoad = objClient.GetApiResponseSync<PQOnRoad>(_bwHostUrl, _requestType, _apiUrl, pqOnRoad);
                    }

                    if (pqOnRoad != null)
                    {
                        dealerId = Convert.ToString(pqOnRoad.PriceQuote.DealerId);
                        pqId = Convert.ToString(pqOnRoad.PriceQuote.PQId);
                        PriceQuoteCookie.SavePQCookie(cityId, pqId, Convert.ToString(areaId), Convert.ToString(variantId), dealerId);

                        if (pqOnRoad.IsDealerPriceAvailable)
                        {
                            #region when dealer Price is Available
                            // Select Variant for which details need to be shown
                            var selectedVariant = pqOnRoad.DPQOutput.Varients.Where(p => p.objVersion.VersionId == variantId).FirstOrDefault();
                            if (selectedVariant != null)
                            {
                                onRoadPrice = selectedVariant.OnRoadPrice;
                                price = onRoadPrice.ToString();
                                if (pqOnRoad.DPQOutput.objOffers.Count > 0)
                                {
                                    IEnumerable<DPQOfferBase> moreOffers = null;
                                    rptOffers.DataSource = pqOnRoad.DPQOutput.objOffers.Take<DPQOfferBase>(2);
                                    rptOffers.DataBind();
                                    if (pqOnRoad.DPQOutput.objOffers.Count > 2)
                                    {
                                        moreOffers = pqOnRoad.DPQOutput.objOffers.Skip(2).Take<DPQOfferBase>(pqOnRoad.DPQOutput.objOffers.Count - 2);
                                        rptMoreOffers.DataSource = moreOffers;
                                        rptMoreOffers.DataBind();
                                    }
                                    isOfferAvailable = true;
                                }
                                rptCategory.DataSource = selectedVariant.PriceList;
                                rptCategory.DataBind();
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
                                bookingAmt = selectedVariant.BookingAmount;
                                if (bookingAmt > 0)
                                    isBookingAvailable = true;

                                if (pqOnRoad.IsInsuranceFree && pqOnRoad.InsuranceAmount > 0)
                                {
                                    price = Convert.ToString(onRoadPrice - pqOnRoad.InsuranceAmount);
                                }
                            }
                            else
                            {
                                objSelectedVariant = pqOnRoad.BPQOutput.Varients.Where(p => p.VersionId == variantId).FirstOrDefault();
                                price = Convert.ToString(objSelectedVariant.OnRoadPrice);
                                isBikeWalePQ = true;
                            }
                            #endregion
                        }
                        else
                        {
                            #region BikeWale PQ
                            if (hdnVariant.Value != "0")
                            {
                                variantId = Convert.ToInt32(hdnVariant.Value);
                                //Convert.ToInt32(ViewState["variantVal"].ToString());
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
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"] + MethodBase.GetCurrentMethod().Name);
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
                string apiVarUrl = "/api/version/specs/?versionId={0}";
                string _apiVarUrl = String.Format(apiVarUrl, versionId);
                using (BWHttpClient objClient = new BWHttpClient())
                {
                    bikeSpecs = objClient.GetApiResponseSync<VersionSpecifications>(_bwHostUrl, _requestType, _apiVarUrl, bikeSpecs);
                }
                if (bikeSpecs != null)
                {
                    modelPage.ModelVersionSpecs = bikeSpecs;
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
        private PQCityList FetchCityByModelId(string modelId)
        {
            string apiVarUrl = "/api/PQCityList/?modelId={0}";
            string _apiVarUrl = String.Format(apiVarUrl, modelId);
            using (BWHttpClient objClient = new BWHttpClient())
            {
                objCityList = objClient.GetApiResponseSync<PQCityList>(_bwHostUrl, _requestType, _apiVarUrl, objCityList);
            }
            return objCityList;
        }

        /// <summary>
        /// Author          :   Sangram Nandkhile
        /// Created Date    :   27 Nov 2015
        /// Description     :   Get List of Area depending on City and Model Id
        /// </summary>
        private PQAreaList GetAreaForCityAndModel()
        {
            string apiVarUrl = "/api/PQAreaList/?modelId={0}&cityId={1}";
            string _apiVarUrl = String.Format(apiVarUrl, modelId, cityId);
            using (BWHttpClient objClient = new BWHttpClient())
            {
                objAreaList = objClient.GetApiResponseSync<PQAreaList>(_bwHostUrl, _requestType, _apiVarUrl, objAreaList);
            }
            return objAreaList;
        }

    }
}
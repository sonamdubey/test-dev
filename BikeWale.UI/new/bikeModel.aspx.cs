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
        protected ModelPage modelPage;
        protected VersionSpecifications bikeSpecs;
        protected PQOnRoad pqOnRoad;
        protected string modelId = string.Empty;
        protected int variantId = 0 ;
        protected int versionId = 0;
        protected Repeater rptModelPhotos, rptNavigationPhoto, rptVarients, rptColor, rptOffers, rptMoreOffers, rptCategory, rptVariants;
        protected String bikeName = String.Empty;
        protected String clientIP = string.Empty;
        protected String cityId = "0";
        protected string areaId = "0";
        protected string cityName = string.Empty;
        protected string areaName = string.Empty;
        protected bool isCityAreaSelected = false;
        protected bool isBikeWalePQ = false;
        protected AlternativeBikes ctrlAlternativeBikes;
        protected short reviewTabsCnt = 0;
        //Variable to Assing ACTIVE class
        protected bool isUserReviewActive = false, isExpertReviewActive = false, isNewsActive = false, isVideoActive = false;
        //Varible to Hide or show controlers
        protected bool isUserReviewZero = true, isExpertReviewZero = true, isNewsZero = true, isVideoZero = true;
        protected bool isBookingAvailable, isOfferAvailable = false;
        protected bool isDiscontinued = false;
        static readonly string _PageNotFoundPath;
        static readonly string _bwHostUrl;
        protected static bool isManufacturer = false;
        protected DropDownList ddlVariant;
        protected string variantText = string.Empty;
        protected uint bookingAmt = 0;
        protected int grid1_size = 9;
        protected int grid2_size = 3;
        protected string cssOffers= "noOffers";
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
        #endregion

        #region Events

        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
            //ddlVariant.SelectedIndexChanged += new EventHandler(ddlVariant_SelectedIndexChanged);
            //rptVarients.OnItemDataBound += new EventHandler(rptVarients_ItemDataBound);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //device detection
            DeviceDetection dd = new DeviceDetection(Request.ServerVariables["HTTP_X_REWRITE_URL"]);
            dd.DetectDevice();
            #region Do Not change the sequence
            ParseQueryString();
            GetAreaForCityAndModel();
            CheckCityCookie();

            //if (!string.IsNullOrEmpty(ddlVariant.SelectedValue) && ddlVariant.SelectedValue != "0")
            if (hdnVariant.Value != "0")
                variantId = Convert.ToInt32(hdnVariant.Value);
            #endregion
            if (!IsPostBack)
            {
                #region Do not change the sequence
                FetchModelPageDetails();
                BindPhotoRepeater();
                BindModelGallery();
                BindAlternativeBikeControl();
                clientIP = CommonOpn.GetClientIP();
                LoadVariants();
                #endregion
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
            // Set BikeName
            if (modelPage.ModelDetails!=null)
                bikeName = modelPage.ModelDetails.MakeBase.MakeName + ' ' + modelPage.ModelDetails.ModelName;
            FetchOnRoadPrice();
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
            catch(Exception ex)
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
            if (modelPage.ModelVersions != null && !modelPage.ModelDetails.Futuristic)
            {
                //ddlVariant.Items.Clear();
                if (modelPage.ModelVersions != null && modelPage.ModelVersions.Count > 1)
                {
                    //foreach (var version in modelPage.ModelVersions)
                    //{
                    //    //ddlVariant.Items.Insert(0, new ListItem(version.VersionName, version.VersionId.ToString()));
                    //}
                    if (modelPage.ModelVersionSpecs != null && modelPage.ModelVersionSpecs.BikeVersionId != 0)
                    {
                        //ddlVariant.SelectedValue = Convert.ToString(modelPage.ModelVersionSpecs.BikeVersionId);
                        defaultVariant.Text = modelPage.ModelVersions.Where(p=>p.VersionId==(int)modelPage.ModelVersionSpecs.BikeVersionId).First().VersionName;
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

        private void BindModelGallery()
        {

            if (modelPage != null)
            {
                List<Bikewale.DTO.CMS.Photos.CMSModelImageBase> photos = modelPage.Photos;

                if (photos != null && photos.Count > 0)
                {
                    photos.Insert(0, new DTO.CMS.Photos.CMSModelImageBase()
                    {
                        HostUrl = modelPage.ModelDetails.HostUrl,
                        OriginalImgPath = modelPage.ModelDetails.OriginalImagePath,
                        ImageCategory = bikeName,
                    });
                    ctrlModelGallery.bikeName = bikeName;
                    ctrlModelGallery.modelId = Convert.ToInt32(modelId);
                    ctrlModelGallery.Photos = photos;
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
                    //if (modelPage.Photos.Count > 2)
                    //{
                    //    rptModelPhotos.DataSource = modelPage.Photos.Take(3);
                    //}
                    //else
                    //{
                    //    rptModelPhotos.DataSource = modelPage.Photos;
                    //}
                    //rptModelPhotos.DataBind();

                    //if (modelPage.Photos.Count > 2)
                    //{
                    //    rptNavigationPhoto.DataSource = modelPage.Photos.Take(3);
                    //}
                    //else
                    //{
                    //    rptNavigationPhoto.DataSource = modelPage.Photos;
                    //}

                    rptModelPhotos.DataSource = photos;
                    rptModelPhotos.DataBind();

                    rptNavigationPhoto.DataSource = photos;
                    rptNavigationPhoto.DataBind();
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
            catch(Exception ex)
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
                    }
                    if(locArray.Length > 3 && cityId != "0")
                    {
                       areaId = locArray[2];
                       objAreaList = GetAreaForCityAndModel();
                       if (objAreaList != null)
                       {
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
                if (!string.IsNullOrEmpty(modelId))
                {
                    string _apiUrl = String.Format(apiURL, modelId, variantId);
                    modelPage = Bikewale.Utility.BWHttpClient.GetApiResponseSync<ModelPage>(_bwHostUrl, _requestType, _apiUrl, modelPage);
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
            catch(Exception ex)
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
            try
            {
                if (!string.IsNullOrEmpty(cityId) && cityId != "0")
                {
                    string _apiUrl = String.Format(onRoadApi, cityId, modelId, null, 0, areaId);
                    pqOnRoad = Bikewale.Utility.BWHttpClient.GetApiResponseSync<PQOnRoad>(_bwHostUrl, _requestType, _apiUrl, pqOnRoad);
                    if (pqOnRoad != null)
                    {
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
                            #endregion
                        }
                        else
                        {
                            #region BikeWale PQ
                            //if (!string.IsNullOrEmpty(ddlVariant.SelectedValue))
                            //if (ViewState["variantVal"] != null)
                            if (hdnVariant.Value != "0")
                            {
                                int variantId = Convert.ToInt32(hdnVariant.Value);
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
            catch(Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"] + MethodBase.GetCurrentMethod().Name);
                objErr.SendMail();
            }
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
                string apiVarUrl = "/api/version/specs/?versionId={0}";
                string _apiVarUrl = String.Format(apiVarUrl, versionId);
                bikeSpecs = Bikewale.Utility.BWHttpClient.GetApiResponseSync<VersionSpecifications>(_bwHostUrl, _requestType, _apiVarUrl, bikeSpecs);
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
        /// Created Date    :   24 Nov 2015
        /// Description     :   Gets City Details by ModelId
        /// </summary>
        /// <param name="modelId">Model Id</param>
        private PQCityList FetchCityByModelId(string modelId)
        {
            string apiVarUrl = "/api/PQCityList/?modelId={0}";
            string _apiVarUrl = String.Format(apiVarUrl, modelId);
            objCityList = Bikewale.Utility.BWHttpClient.GetApiResponseSync<PQCityList>(_bwHostUrl, _requestType, _apiVarUrl, objCityList);
            return objCityList;
        }

        /// <summary>
        /// Author          :   Sangram Nandkhile
        /// Created Date    :   24 Nov 2015
        /// Description     :   Get List of Area depending on City and Model Id
        /// </summary>
        private PQAreaList GetAreaForCityAndModel()
        {
            string apiVarUrl = "/api/PQAreaList/?modelId={0}&cityId={1}";
            string _apiVarUrl = String.Format(apiVarUrl, modelId, cityId);
            objAreaList = Bikewale.Utility.BWHttpClient.GetApiResponseSync<PQAreaList>(_bwHostUrl, _requestType, _apiVarUrl, objAreaList);
            return objAreaList;
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
        #endregion

        
    }
}
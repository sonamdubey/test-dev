using Bikewale.Cache.BikeData;
using Bikewale.Cache.Core;
using Bikewale.Common;
using Bikewale.DAL.BikeData;
using Bikewale.DTO.Model;
using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Utility;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Bikewale.Controls;
using System.Web.Services;

namespace Bikewale.New
{
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

    public class versions : System.Web.UI.Page
    {
        protected News_new ctrlNews;
        protected ExpertReviews ctrlExpertReviews;
        protected VideosControl ctrlVideos;
        protected UserReviewsList ctrlUserReviews;
        protected ModelGallery ctrlModelGallery;
        protected ModelPage modelPage;
        protected string modelId = string.Empty;
        protected Repeater rptModelPhotos, rptNavigationPhoto, rptVarients, rptColor;
        protected String bikeName = String.Empty;
        protected String clientIP = string.Empty;
        protected String cityId = String.Empty;
        protected AlternativeBikes ctrlAlternativeBikes;
        protected short reviewTabsCnt = 0;
        //Variable to Assing ACTIVE class
        protected bool isUserReviewActive = false, isExpertReviewActive = false, isNewsActive = false, isVideoActive = false;
        //Varible to Hide or show controlers
        protected bool isUserReviewZero = true, isExpertReviewZero = true, isNewsZero = true, isVideoZero = true;

        static readonly string _PageNotFoundPath;
        static readonly string _bwHostUrl;
        protected static bool isManufacturer = false;

        static versions()
        {
            _PageNotFoundPath = Bikewale.Common.CommonOpn.AppPath + "pageNotFound.aspx";
            _bwHostUrl = ConfigurationManager.AppSettings["bwHostUrl"];
            isManufacturer = (ConfigurationManager.AppSettings["TVSManufacturerId"] != "0") ? true : false;
        }

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
            FetchModelPageDetails();
            #endregion
            if (!IsPostBack)
            {
                #region Do not change the sequence
                BindPhotoRepeater();
                BindModelGallery();
                BindAlternativeBikeControl();
                clientIP = CommonOpn.GetClientIP();
                #endregion
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
            string modelQuerystring = Request.QueryString["model"];
            if (!string.IsNullOrEmpty(modelQuerystring))
            {
                ModelMaskingResponse objResponse = null;

                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IBikeMaskingCacheRepository<BikeModelEntity, int>, BikeModelMaskingCache<BikeModelEntity, int>>()
                             .RegisterType<ICacheManager, MemcacheManager>()
                             .RegisterType<IBikeModelsRepository<BikeModelEntity, int>, BikeModelsRepository<BikeModelEntity, int>>()
                            ;
                    var objCache = container.Resolve<IBikeMaskingCacheRepository<BikeModelEntity, int>>();

                    objResponse = objCache.GetModelMaskingResponse(modelQuerystring);

                    if (objResponse != null && objResponse.StatusCode == 200)
                    {
                        modelId = objResponse.ModelId.ToString();
                    }
                    else
                    {
                        if (objResponse.StatusCode == 301)
                        {
                            //redirect permanent to new page                             
                            Bikewale.Common.CommonOpn.RedirectPermanent(Request.RawUrl.Replace(modelQuerystring, objResponse.MaskingName));

                        }
                        else
                        {
                            Response.Redirect(_PageNotFoundPath, false);
                            HttpContext.Current.ApplicationInstance.CompleteRequest();
                            this.Page.Visible = false;
                            //isSuccess = false;
                        }
                    }
                }
            }
            else
            {

            }
        }

        private void CheckCityCookie()
        {
            string location = String.Empty;
            var cookies = this.Context.Request.Cookies;
            if (cookies.AllKeys.Contains("location"))
            {
                location = cookies["location"].Value;
                if(!String.IsNullOrEmpty(location))
                    cityId = location.Substring(0, location.IndexOf('_'));//location.Split('_')[0];
            }
            else
            {
                cityId = "0";
            }
        }

        static readonly string apiURL = "/api/model/details/?modelId={0}";
        static readonly string _requestType = "application/json";

        private void FetchModelPageDetails()
        {
            if (!string.IsNullOrEmpty(modelId))
            {
                string _apiUrl = String.Format(apiURL, modelId);
                modelPage = Bikewale.Utility.BWHttpClient.GetApiResponseSync<ModelPage>(_bwHostUrl, _requestType, _apiUrl, modelPage);

                if (modelPage != null)
                {
                    bikeName = modelPage.ModelDetails.MakeBase.MakeName + ' ' + modelPage.ModelDetails.ModelName;
                }
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
    }

}
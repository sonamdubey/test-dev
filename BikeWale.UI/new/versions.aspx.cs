﻿using Bikewale.Cache.BikeData;
using Bikewale.Cache.Core;
using Bikewale.Common;
using Bikewale.controls;
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
        protected ModelPage modelPage;
        protected string modelId = string.Empty;
        protected Repeater rptModelPhotos, rptNavigationPhoto, rptVarients, rptColor;
        protected String bikeName = String.Empty;
        protected String clientIP = string.Empty;
        protected AlternativeBikes ctrlAlternativeBikes;
        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            //device detection
            DeviceDetection dd = new DeviceDetection(Request.ServerVariables["HTTP_X_REWRITE_URL"].ToString());
            dd.DetectDevice();

            if (!IsPostBack)
            {
                ParseQueryString();
                FetchModelPageDetails();
                BindPhotoRepeater();
                BindAlternativeBikeControl();
                clientIP = this.Context.Request.ServerVariables["REMOTE_ADDR"];
            }
        }

        private void BindAlternativeBikeControl()
        {
            ctrlAlternativeBikes.TopCount = 6;
            ctrlAlternativeBikes.VersionId = modelPage.ModelVersions[0].VersionId;            
        }

        private void BindPhotoRepeater()
        {
            rptModelPhotos.DataSource = modelPage.Photos;
            rptModelPhotos.DataBind();

            rptNavigationPhoto.DataSource = modelPage.Photos;
            rptNavigationPhoto.DataBind();

            rptVarients.DataSource = modelPage.ModelVersions;
            rptVarients.DataBind();
            if (modelPage.ModelColors != null && modelPage.ModelColors.Count() > 0)
            {
                rptColor.DataSource = modelPage.ModelColors;
                rptColor.DataBind();
            }
        }

        private void ParseQueryString()
        {
            if (!string.IsNullOrEmpty(Request.QueryString["model"]))
            {
                ModelMaskingResponse objResponse = null;

                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IBikeMaskingCacheRepository<BikeModelEntity, int>, BikeModelMaskingCache<BikeModelEntity, int>>()
                             .RegisterType<ICacheManager, MemcacheManager>()
                             .RegisterType<IBikeModelsRepository<BikeModelEntity, int>, BikeModelsRepository<BikeModelEntity, int>>()
                            ;
                    var objCache = container.Resolve<IBikeMaskingCacheRepository<BikeModelEntity, int>>();

                    objResponse = objCache.GetModelMaskingResponse(Request.QueryString["model"]);

                    if (objResponse != null && objResponse.StatusCode == 200)
                    {
                        modelId = objResponse.ModelId.ToString();
                    }
                    else
                    {
                        if (objResponse.StatusCode == 301)
                        {
                            //redirect permanent to new page 
                            //CommonOpn.RedirectPermanent(Request.RawUrl.Replace(Request.QueryString["model"], objResponse.MaskingName));

                        }
                        else
                        {
                            //Response.Redirect(CommonOpn.AppPath + "pageNotFound.aspx", true);
                            //isSuccess = false;
                        }
                    }
                }
            }
            else
            {

            }
        }

        private void FetchModelPageDetails()
        {
            string _bwHostUrl = ConfigurationManager.AppSettings["bwHostUrl"];
            string _requestType = "application/json";
            string _apiUrl = String.Format("api/Model?modelId={0}&isNew=true&specs=1&features=1", modelId);
            modelPage = Bikewale.Utility.BWHttpClient.GetApiResponseSync<ModelPage>(_bwHostUrl, _requestType, _apiUrl, modelPage);
            bikeName = modelPage.ModelDetails.MakeBase.MakeName + ' ' + modelPage.ModelDetails.ModelName;
        }

        protected string FormatShowReview(string makeName, string modelName)
        {
            return string.Format("/{0}-bikes/{1}/user-reviews/",makeName,modelName);
        }

        protected string FormatWriteReviewLink()
        {
            return String.Format("/content/userreviews/writereviews.aspx?bikem={0}",modelId);
        }

        protected string FormatOverview(object spec,Overviews overview)
        {
            String strSpec = "<span class=\"font26 text-bold text-black\">{0}</span><span class=\"font24 text-light-grey margin-left5\">{1}</span>";
            if (spec != null && !string.IsNullOrEmpty(spec.ToString()))
            {
                switch (overview)
                {
                    case Overviews.Capacity:
                        return String.Format(strSpec, spec.ToString(), "cc");
                    case Overviews.Mileage:
                        return String.Format(strSpec, spec.ToString(), "kmpl");
                    case Overviews.MaxPower:
                        return String.Format(strSpec, spec.ToString(), "bhp");
                    case Overviews.Weight:
                        return String.Format(strSpec, spec.ToString(), "kgs");
                    default:
                        return String.Format(strSpec, "-", "");
                }
            }
            else
            {
                return String.Format(strSpec,"-","");
            }            
        }

        protected string FormatMaxPower(object bhp,object rpm)
        {
            string format = "<div class=\"text-bold\">{0} bhp @ {1} rpm</div>";
            if (bhp != null && !String.IsNullOrEmpty(bhp.ToString()) && rpm != null && !String.IsNullOrEmpty(rpm.ToString()))
            {
                return String.Format(format,bhp.ToString(),rpm.ToString());
            }
            return "<div class=\"text-bold\">-</div>";
        }

        protected string FormatMaxTorque(object nm,object rpm)
        {
            string format = "<div class=\"text-bold\">{0} Nm @ {1} rpm</div>";
            if (nm != null && !String.IsNullOrEmpty(nm.ToString()) && rpm != null && !String.IsNullOrEmpty(rpm.ToString()))
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
                return String.Format(format,val.ToString());
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

        protected string FormatDimension(ushort val,string dim)
        {
            string format = "<div class=\"text-bold\">{0} {1}</div>";
            if (val > 0)
            {
                return String.Format(format, val.ToString(),dim);
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

        protected string FormatVarientMinSpec(bool alloyWheel,bool elecStart, bool abs,string breakType)
        {
            string format = "";
            if (alloyWheel)
            {
                format = String.Concat(format.Trim()," Alloy Wheels");
            }
            else{
                format = String.Concat(format.Trim(), " Spoke Wheels");
            }

            if (elecStart)
            {
                format = String.Concat(format.Trim(), " Electric Start");
            }
            else
            {
                format = String.Concat(format.Trim(), " Kick Start");
            }

            if (abs)
            {
                format = String.Concat(format.Trim(), " ABS");
            }
            
            if (!String.IsNullOrEmpty(breakType))
            {
                format = String.Concat(format.Trim(),breakType," Break");
            }

            if (String.IsNullOrEmpty(format.Trim()))
            {
                return "No specifications.";
            }
            return format;
        }
    }

}
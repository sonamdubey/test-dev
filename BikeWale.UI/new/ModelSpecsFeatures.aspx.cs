using Bikewale.BAL.BikeBooking;
using Bikewale.BAL.BikeData;
using Bikewale.BAL.PriceQuote;
using Bikewale.Cache.BikeData;
using Bikewale.Cache.Core;
using Bikewale.Cache.Location;
using Bikewale.Common;
using Bikewale.controls;
using Bikewale.DAL.BikeData;
using Bikewale.DAL.Location;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Dealer;
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
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Bikewale.New
{
    /// <summary>
    /// Created By : Lucky Rathore on 03 June 2016
    /// Description : class handle Binding of specification and Feature and other logics.
    /// </summary>
    public class ModelSpecsFeatures : PageBase
    {
        protected uint cityId, areaId, modelId, versionId, dealerId, price = 0;
        protected string cityName, areaName, makeName, modelName, modelImage, bikeName, versionName, makeMaskingName, modelMaskingName, clientIP = CommonOpn.GetClientIP();
        protected IEnumerable<CityEntityBase> objCityList = null;
        protected IEnumerable<Bikewale.Entities.Location.AreaEntityBase> objAreaList = null;
        protected bool isCitySelected, isAreaSelected, isBikeWalePQ, isOnRoadPrice, isAreaAvailable, showOnRoadPriceButton, isDiscontinued, IsDealerPriceQuote, IsExShowroomPrice;
        protected BikeSpecificationEntity specs;
        protected BikeModelPageEntity modelDetail;
        protected DetailedDealerQuotationEntity dealerDetail;
        protected BikeModelPageEntity modelPg;
        protected LeadCaptureControl ctrlLeadPopUp;

        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }

        /// <summary>
        /// Created By : Lucky Rathore on 03 June 2016
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            string originalUrl = Request.ServerVariables["HTTP_X_ORIGINAL_URL"];
            if (String.IsNullOrEmpty(originalUrl))
                originalUrl = Request.ServerVariables["URL"];
            DeviceDetection dd = new DeviceDetection(originalUrl);
            dd.DetectDevice();
            ProcessQueryString();
            modelDetail = FetchModelPageDetails(modelId, versionId);
            if (modelDetail != null)
            {
                CheckCityCookie();
                //for Lead Pop up Controle
                ctrlLeadPopUp.ModelId = modelId;
                ctrlLeadPopUp.CityId = cityId;
                ctrlLeadPopUp.AreaId = areaId;
                PQByCityArea pqOnRoad = null;
                PQOnRoadPrice pqEntity = null;
                if (cityId > 0 && versionId > 0)
                {
                    string PQLeadId = (PQSourceEnum.Desktop_SpecsAndFeature_PQOnroad).ToString();
                    string UTMA = Request.Cookies["__utma"] != null ? Request.Cookies["__utma"].Value : "";
                    string UTMZ = Request.Cookies["__utmz"] != null ? Request.Cookies["__utmz"].Value : "";
                    string DeviceId = Request.Cookies["BWC"] != null ? Request.Cookies["BWC"].Value : "";

                    pqOnRoad = new PQByCityArea();
                    pqEntity = pqOnRoad.GetOnRoadPrice((int)modelId, (int)cityId, (int)areaId, (int)versionId, 1, UTMA, UTMZ, DeviceId, clientIP, PQLeadId);
                    //dealerDetail =  GetDetailedDealer();
                }
                if (pqEntity != null)
                {
                    SetPrice(pqEntity, modelDetail);
                    // price = Convert.ToUInt32(dealerDetail.PrimaryDealer.PriceList.Sum(p => p.Price));
                }
            }
            if (versionId > 0)
            {
                specs = FetchVariantDetails(versionId);
            }
            else
            {
                specs = modelPg.ModelVersionSpecs;
            }
        }
        /// <summary>
        /// created by Sangram Nandkhile on 07 Jun 2016
        /// Summary: Set price according to version id
        /// </summary>
        private void SetPrice(PQOnRoadPrice pqOnRoad, BikeModelPageEntity modelDetail)
        {

            PQByCityAreaEntity pqEntity = new PQByCityAreaEntity();
            if (pqOnRoad != null)
            {
                pqEntity.PqId = pqOnRoad.PriceQuote.PQId;
                pqEntity.DealerId = pqOnRoad.PriceQuote.DealerId;
                //IsExShowroomPrice = pqOnRoad.DPQOutput == null && pqOnRoad.BPQOutput == null;

                // When City has areas and area is not selected then show ex-showrrom price so user can select it
                bool isAreaExistAndSelected = pqEntity.IsAreaExists && pqEntity.IsAreaSelected;
                // when DPQ OR Only city level pricing exists
                if (isAreaExistAndSelected || (!pqEntity.IsAreaExists))
                {
                    #region  Iterate over version to fetch Dealer PQ or BikeWalePQ

                    foreach (var version in modelDetail.ModelVersions)
                    {
                        if (pqOnRoad.DPQOutput != null)
                        {
                            var selected = pqOnRoad.DPQOutput.Varients.Where(p => p.objVersion.VersionId == versionId).FirstOrDefault();
                            if (selected != null)
                            {
                                price = selected.OnRoadPrice;
                                IsDealerPriceQuote = true;
                                break;
                            }
                            else if (pqOnRoad.BPQOutput != null && pqOnRoad.BPQOutput.Varients != null)
                            {
                                var selectedBPQ = pqOnRoad.BPQOutput.Varients.Where(p => p.VersionId == versionId).FirstOrDefault();
                                if (selectedBPQ != null)
                                {
                                    price = Convert.ToUInt32(selectedBPQ.OnRoadPrice);
                                    IsDealerPriceQuote = false;
                                    break;
                                }
                            }
                        }
                        else if (pqOnRoad.BPQOutput != null && pqOnRoad.BPQOutput.Varients != null)
                        {
                            var selectedBPQ = pqOnRoad.BPQOutput.Varients.Where(p => p.VersionId == versionId).FirstOrDefault();
                            if (selectedBPQ != null)
                            {
                                price = Convert.ToUInt32(selectedBPQ.OnRoadPrice);
                                IsDealerPriceQuote = false;
                                break;
                            }
                        }
                    }
                    #endregion
                }
                else
                {
                    IsExShowroomPrice = true;
                }
            }

        }
        /// <summary>
        /// Author          :   Sangram Nandkhile
        /// Created Date    :   18 Nov 2015
        /// </summary>
        private BikeModelPageEntity FetchModelPageDetails(uint modelID, uint versionId)
        {
            modelPg = new BikeModelPageEntity();
            try
            {
                if (modelID > 0)
                {
                    using (IUnityContainer container = new UnityContainer())
                    {
                        container.RegisterType<IBikeModelsCacheRepository<int>, BikeModelsCacheRepository<BikeModelEntity, int>>()
                                 .RegisterType<IBikeModels<BikeModelEntity, int>, BikeModels<BikeModelEntity, int>>()
                                 .RegisterType<ICacheManager, MemcacheManager>();
                        var objCache = container.Resolve<IBikeModelsCacheRepository<int>>();
                        modelPg = objCache.GetModelPageDetails(Convert.ToInt16(modelID));
                        if (modelPg != null && modelPg.ModelDetails != null)
                        {
                            if (modelPg.ModelDetails.ModelName != null)
                            {
                                modelName = modelPg.ModelDetails.ModelName;
                            }
                            if (modelPg.ModelDetails.MakeBase != null)
                            {
                                makeName = modelPg.ModelDetails.MakeBase.MakeName;
                                makeMaskingName = modelPg.ModelDetails.MakeBase.MaskingName;
                            }
                            bikeName = string.Format("{0} {1}", makeName, modelName);
                            if (!modelPg.ModelDetails.Futuristic && modelPg.ModelVersionSpecs != null)
                            {
                                modelImage = Bikewale.Utility.Image.GetPathToShowImages(modelPg.ModelDetails.OriginalImagePath, modelPg.ModelDetails.HostUrl, Bikewale.Utility.ImageSize._272x153);
                                var selectedVersion = modelPg.ModelVersions.First(p => p.VersionId == versionId);
                                if (selectedVersion != null)
                                {
                                    price = Convert.ToUInt32(selectedVersion.Price);
                                    versionName = selectedVersion.VersionName;
                                }

                                //Check it versionId passed through url exists in current model's versions
                                //if (!modelPg.ModelVersions.Exists(p => p.VersionId == versionId))
                                //{
                                //    versionId = modelPg.ModelVersionSpecs.BikeVersionId;
                                //}
                                //versionName = modelPg.ModelVersions.Find(item => item.VersionId == versionId).VersionName;
                            }
                            if (!modelPg.ModelDetails.New)
                                isDiscontinued = true;

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, Request.ServerVariables["URL"] + "FetchModelPageDetails");
                objErr.SendMail();
            }
            return modelPg;
        }

        /// <summary>
        /// Created by: Sangram Nandkhile on 16 mar 2016
        /// Summary     : API to fetch detailed dealer entity
        /// </summary>
        private DetailedDealerQuotationEntity GetDetailedDealer()
        {
            DetailedDealerQuotationEntity detailedDealer = null;
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IDealerPriceQuoteDetail, DealerPriceQuoteDetail>().RegisterType<IDealerPriceQuote, DealerPriceQuote>();
                    IDealerPriceQuoteDetail objIPQ = container.Resolve<IDealerPriceQuoteDetail>();
                    IDealerPriceQuote dealerPQ = container.Resolve<IDealerPriceQuote>();
                    DealerInfo dealerInfo = dealerPQ.IsDealerExists(versionId, areaId);
                    if (dealerInfo != null && dealerInfo.DealerId > 0)
                    {
                        detailedDealer = objIPQ.GetDealerQuotation(cityId, versionId, dealerInfo.DealerId);
                    }

                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.New.ModelSpecsFeatures.GetDetailedDealer");
                objErr.SendMail();
            }
            return detailedDealer;
        }

        /// <summary>
        /// Author          :   Sangram Nandkhile
        /// Created Date    :   18 Nov 2015
        /// Description     :   Sends the notification to Customer and Dealer
        /// </summary>
        private BikeSpecificationEntity FetchVariantDetails(uint versionId)
        {
            BikeSpecificationEntity specsFeature = null;
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IBikeModelsRepository<BikeModelEntity, int>, BikeModelsRepository<BikeModelEntity, int>>();
                    IBikeModelsRepository<BikeModelEntity, int> objVersion = container.Resolve<IBikeModelsRepository<BikeModelEntity, int>>();
                    specsFeature = objVersion.MVSpecsFeatures((int)versionId);
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, Request.ServerVariables["URL"] + "FetchVariantDetails");
                objErr.SendMail();
            }
            return specsFeature;
        }

        /// <summary>
        /// Created By : Lucky Rathore on 03 June 2016
        /// Description : Private Method to proceess mpq queryString and set the values 
        /// for queried parameters versionId, ModelMaskingName and set value of modelId from Model Masking Name. 
        /// </summary>
        private void ProcessQueryString()
        {
            try
            {
                if (HttpContext.Current.Request.QueryString != null && HttpContext.Current.Request.QueryString.HasKeys())
                {
                    UInt32.TryParse(Request.QueryString["vid"], out versionId);
                    modelMaskingName = Request.QueryString["model"];

                    if (!string.IsNullOrEmpty(modelMaskingName) && versionId > 0)
                    {
                        using (IUnityContainer container = new UnityContainer())
                        {
                            container.RegisterType<IBikeMaskingCacheRepository<BikeModelEntity, int>, BikeModelMaskingCache<BikeModelEntity, int>>()
                                     .RegisterType<ICacheManager, MemcacheManager>()
                                     .RegisterType<IBikeModelsRepository<BikeModelEntity, int>, BikeModelsRepository<BikeModelEntity, int>>()
                                    ;
                            var objCache = container.Resolve<IBikeMaskingCacheRepository<BikeModelEntity, int>>();
                            ModelMaskingResponse objResponse = objCache.GetModelMaskingResponse(modelMaskingName);
                            if (objResponse != null && objResponse.StatusCode == 200)
                            {
                                modelId = objResponse.ModelId;
                            }
                            else if (objResponse != null && objResponse.StatusCode == 301)
                            {
                                //redirect permanent to new page 
                                CommonOpn.RedirectPermanent(Request.RawUrl.Replace(modelMaskingName, objResponse.MaskingName));
                            }
                            else
                            {
                                Response.Redirect(CommonOpn.AppPath + "pageNotFound.aspx", false);
                                HttpContext.Current.ApplicationInstance.CompleteRequest();
                                this.Page.Visible = false;
                            }
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
            catch (Exception ex)
            {

                Trace.Warn("GetLocationCookie Ex: ", ex.Message);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"] + "ProcessQueryString");
                objErr.SendMail();
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
                        if (modelId > 0)
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
                    // This function will check if Areas are available for city and Model
                    objAreaList = GetAreaForCityAndModel();
                    // locArray.Length = 4 Means City and area exists
                    if (locArray.Length > 3 && cityId != 0)
                    {
                        UInt32.TryParse(locArray[2], out areaId);
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

        /// <summary>
        /// Author          :   Sangram Nandkhile
        /// Created Date    :   24 Nov 2015
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
        /// Created Date    :   24 Nov 2015
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

    }
}
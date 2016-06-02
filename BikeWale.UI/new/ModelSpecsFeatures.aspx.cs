using Bikewale.BAL.BikeBooking;
using Bikewale.BAL.BikeData;
using Bikewale.BAL.PriceQuote;
using Bikewale.Cache.BikeData;
using Bikewale.Cache.Core;
using Bikewale.Cache.Location;
using Bikewale.Common;
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
    public class ModelSpecsFeatures : PageBase
	{
        protected uint cityId, areaId, modelId, versionId, price = 0;
        protected string cityName, areaName, makeName, modelName, modelImage, bikeName, versionName;
        protected IEnumerable<CityEntityBase> objCityList = null;
        protected IEnumerable<Bikewale.Entities.Location.AreaEntityBase> objAreaList = null;
        protected bool isCitySelected, isAreaSelected, isBikeWalePQ, isOnRoadPrice, isAreaAvailable, showOnRoadPriceButton;
        protected BikeSpecificationEntity specs;
        protected BikeModelPageEntity modelDetail;
        protected DetailedDealerQuotationEntity dealerDetail;
        protected BikeModelPageEntity modelPg;

        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }

		protected void Page_Load(object sender, EventArgs e)
		{
            string originalUrl = Request.ServerVariables["HTTP_X_ORIGINAL_URL"];
            if (String.IsNullOrEmpty(originalUrl))
                originalUrl = Request.ServerVariables["URL"];
            DeviceDetection dd = new DeviceDetection(originalUrl);
            dd.DetectDevice();

            ParseQueryString();

            modelDetail = FetchModelPageDetails(modelId);//TODO: Cross checkt parameter to be passed.
            if (modelDetail != null)
            {
                CheckCityCookie();
                specs = modelDetail.ModelVersionSpecs;
                if (cityId > 0 && versionId > 0)
                {
                   dealerDetail =  GetDetailedDealer();
                }
                if (dealerDetail != null)
                {
                    setPrice();
                }
            }
            
		}

        /// <summary>
        /// Author          :   Sangram Nandkhile
        /// Created Date    :   18 Nov 2015
        /// </summary>
        private BikeModelPageEntity FetchModelPageDetails(uint modelID)
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
                        if (modelPg != null)
                        {
                            if (modelPg != null)
                            {   
                                //if (!modelPg.ModelDetails.New)
                                //    isDiscontinued = true;
                                if (modelPg.ModelDetails != null)
                                {
                                    if (modelPg.ModelDetails.ModelName != null)
                                        modelName = modelPg.ModelDetails.ModelName;
                                    if (modelPg.ModelDetails.MakeBase != null)
                                        makeName = modelPg.ModelDetails.MakeBase.MakeName;
                                    bikeName = string.Format("{0} {1}", makeName, modelName);
                                    if (!modelPg.ModelDetails.Futuristic && modelPg.ModelVersionSpecs != null)
                                    {
                                        modelImage = string.Format("{0} {1}", modelPg.ModelDetails.HostUrl, modelPg.ModelDetails.OriginalImagePath);
                                        price = Convert.ToUInt32(modelPg.ModelDetails.MinPrice);
                                        versionId = modelPg.ModelVersionSpecs.BikeVersionId;
                                         //Check it versionId passed through url exists in current model's versions
                                        if (!modelPg.ModelVersions.Exists(p => p.VersionId == versionId))
                                        {
                                            versionId = modelPg.ModelVersionSpecs.BikeVersionId;
                                        }
                                        versionName = modelPg.ModelVersions.Find(item => item.VersionId == versionId).VersionName;//ToDo: Cross Check It.
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, Request.ServerVariables["URL"] + "FetchmodelPgDetails");
                objErr.SendMail();
            }
            return modelPg;
        }

        private void setPrice()
        {
            foreach (var priceList in dealerDetail.PrimaryDealer.PriceList)
            {
                price += priceList.Price;
            }
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
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.BindViewModels.Webforms.GetDetailedDealer");
                objErr.SendMail();
            }
            return detailedDealer;
        }

        /// <summary>
        /// Author          :   Sangram Nandkhile
        /// Created Date    :   18 Nov 2015
        /// 
        /// </summary>

        ///// <summary>
        ///// Author          :   Sangram Nandkhile
        ///// Created Date    :   18 Nov 2015
        ///// Description     :   Sends the notification to Customer and Dealer
        ///// </summary>
        //private BikeSpecificationEntity FetchVariantDetails(uint versionId)
        //{
        //    BikeSpecificationEntity specsFeature = null;
        //    try
        //    {
        //        using (IUnityContainer container = new UnityContainer())
        //        {
        //            container.RegisterType<IBikeModelsRepository<BikeModelEntity, int>, BikeModelsRepository<BikeModelEntity, int>>();
        //            IBikeModelsRepository<BikeModelEntity, int> objVersion = container.Resolve<IBikeModelsRepository<BikeModelEntity, int>>();
        //            specsFeature = objVersion.MVSpecsFeatures((int)versionId);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, Request.ServerVariables["URL"] + "FetchVariantDetails");
        //        objErr.SendMail();
        //    }
        //    return specsFeature;
        //}

        private void ParseQueryString()
        {
            ModelMaskingResponse objResponse = null;
            string modelMaskingName = Request.QueryString["model"];
            try
            {
                if (!string.IsNullOrEmpty(modelMaskingName))
                {
                    using (IUnityContainer container = new UnityContainer())
                    {
                        container.RegisterType<IBikeMaskingCacheRepository<BikeModelEntity, int>, BikeModelMaskingCache<BikeModelEntity, int>>()
                                 .RegisterType<ICacheManager, MemcacheManager>()
                                 .RegisterType<IBikeModelsRepository<BikeModelEntity, int>, BikeModelsRepository<BikeModelEntity, int>>()
                                ;
                        var objCache = container.Resolve<IBikeMaskingCacheRepository<BikeModelEntity, int>>();
                        objResponse = objCache.GetModelMaskingResponse(modelMaskingName);
                    }
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, Request.ServerVariables["URL"] + "ParseQueryString");
                objErr.SendMail();
                Response.Redirect("/new/", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
                this.Page.Visible = false;
            }
            finally
            {
                if (!string.IsNullOrEmpty(modelMaskingName))
                {
                    if (objResponse != null)
                    {
                        //Trace.Warn(" objResponse.MaskingName : ", objResponse.MaskingName.ToString());
                        if (objResponse.StatusCode == 200)
                        {
                            modelId = objResponse.ModelId;
                        }
                        else if (objResponse.StatusCode == 301)
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
            if ((!isCitySelected) || (isCitySelected && isAreaAvailable && !isAreaSelected))
            {
                showOnRoadPriceButton = true;
            }
        }
    }
}
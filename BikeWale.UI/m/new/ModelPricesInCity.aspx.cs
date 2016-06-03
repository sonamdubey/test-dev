using Bikewale.Cache.BikeData;
using Bikewale.Cache.Core;
using Bikewale.Common;
using Bikewale.DAL.BikeData;
using Bikewale.DAL.Location;
using Bikewale.DAL.PriceQuote;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Location;
using Bikewale.Entities.PriceQuote;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.Location;
using Bikewale.Interfaces.PriceQuote;
using Bikewale.Memcache;
using Bikewale.Mobile.Controls;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Bikewale.Mobile.New
{
    public class ModelPricesInCity : System.Web.UI.Page
    {
        protected ModelPriceInNearestCities ctrlTopCityPrices;
        protected DealersCard ctrlDealers;
        public BikeQuotationEntity firstVersion;
        protected NewAlternativeBikes ctrlAlternateBikes;
        protected LeadCaptureControl ctrlLeadCapture;
        public Repeater rprVersionPrices, rpVersioNames;
        protected uint modelId = 0, cityId = 0, versionId, makeId;
        public int versionCount;
        public string makeName = string.Empty, makeMaskingName = string.Empty, modelName = string.Empty, modelMaskingName = string.Empty, bikeName = string.Empty, modelImage = string.Empty, cityName = string.Empty, cityMaskingName = string.Empty;
        string redirectUrl = string.Empty;
        private bool redirectToPageNotFound = false, redirectPermanent = false;
        protected bool isAreaAvailable;
        protected String clientIP = CommonOpn.GetClientIP();


        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ParseQueryString();
            if (redirectToPageNotFound || redirectPermanent)
            {
                DoPageNotFounRedirection();
            }
            else
            {
                FetchVersionPrices();
                ctrlTopCityPrices.ModelId = modelId;
                ctrlTopCityPrices.CityId = cityId;
                ctrlTopCityPrices.TopCount = 8;

                ctrlDealers.MakeId = makeId;
                ctrlDealers.CityId = cityId;
                ctrlDealers.TopCount = 3;
                ctrlDealers.PQSourceId = (int)PQSourceEnum.Desktop_PriceInCity_DealerCard_GetOffers;

                ctrlLeadCapture.CityId = cityId;
                ctrlLeadCapture.ModelId = modelId;
                ctrlLeadCapture.AreaId = 0;

                BindAlternativeBikeControl();

            }
        }
        /// <summary>
        /// Author : Created by Sangram Nandkhile on 25 May 2016
        /// Summary: Fetch version Prices according to model and city
        /// </summary>
        private void FetchVersionPrices()
        {
            try
            {
                IPriceQuote objPQ = null;
                bool hasArea;
                using (IUnityContainer objPQCont = new UnityContainer())
                {
                    objPQCont.RegisterType<IPriceQuote, PriceQuoteRepository>();
                    objPQ = objPQCont.Resolve<IPriceQuote>();
                    IEnumerable<BikeQuotationEntity> bikePrices = objPQ.GetVersionPricesByModelId(modelId, cityId, out hasArea);
                    isAreaAvailable = hasArea;
                    if (bikePrices != null && bikePrices.Count() != 0)
                    {
                        SetModelDetails(bikePrices);

                        rprVersionPrices.DataSource = bikePrices;
                        rprVersionPrices.DataBind();
                        rpVersioNames.DataSource = bikePrices;
                        rpVersioNames.DataBind();
                    }
                    else
                    {
                        redirectToPageNotFound = true;
                        DoPageNotFounRedirection();
                    }
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, Request.ServerVariables["URL"] + "-FetchVersionPrices");
                objErr.SendMail();
            }

        }

        /// <summary>
        /// Sets model details
        /// </summary>
        /// <param name="bikePrices"></param>
        private void SetModelDetails(IEnumerable<BikeQuotationEntity> bikePrices)
        {
            try
            {
                versionCount = bikePrices.Count();
                if (versionCount > 0)
                {
                    firstVersion = bikePrices.FirstOrDefault();
                    if (firstVersion != null)
                    {
                        makeName = firstVersion.MakeName;
                        makeMaskingName = firstVersion.MakeMaskingName;
                        modelName = firstVersion.ModelName;
                        modelMaskingName = firstVersion.ModelMaskingName;
                        cityMaskingName = firstVersion.CityMaskingName;
                        bikeName = String.Format("{0} {1}", makeName, modelName);
                        modelImage = Utility.Image.GetPathToShowImages(firstVersion.OriginalImage, firstVersion.HostUrl, Bikewale.Utility.ImageSize._310x174);
                        cityName = firstVersion.City;
                        versionId = firstVersion.VersionId;
                    }
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, Request.ServerVariables["URL"] + "-SetModelDetails");
                objErr.SendMail();
            }
        }

        /// <summary>
        /// Function to do the redirection on different pages.
        /// </summary>
        private void DoPageNotFounRedirection()
        {
            // Redirection
            if (redirectToPageNotFound)
            {
                Response.Redirect(CommonOpn.AppPath + "pageNotFound.aspx", false);
            }
            else if (redirectPermanent)
                CommonOpn.RedirectPermanent(redirectUrl);
        }

        /// <summary>
        /// Function to get parameters from the query string.
        /// Modified By : Sushil Kumar on 3rd June 2016
        /// Description :  Added fetch operation for makeid from query string
        /// </summary>
        private void ParseQueryString()
        {
            ModelMaskingResponse objResponse = null;
            string model = string.Empty, city = string.Empty, _make = string.Empty;
            try
            {
                model = Request.QueryString["model"];
                city = Request.QueryString["city"];
                _make = Request.QueryString["make"];

                if (!string.IsNullOrEmpty(city))
                {
                    cityId = GetCityMaskingName(city);
                }

                if (!string.IsNullOrEmpty(_make))
                {
                    string _makeId = MakeMapping.GetMakeId(_make);

                    if (CommonOpn.CheckId(_makeId))
                    {
                        makeId = Convert.ToUInt32(_makeId);
                    }
                }


                if (!string.IsNullOrEmpty(model))
                {
                    if (model.Contains("/"))
                    {
                        model = model.Split('/')[0];
                    }
                    using (IUnityContainer container = new UnityContainer())
                    {
                        container.RegisterType<IBikeMaskingCacheRepository<BikeModelEntity, int>, BikeModelMaskingCache<BikeModelEntity, int>>()
                                 .RegisterType<ICacheManager, MemcacheManager>()
                                 .RegisterType<IBikeModelsRepository<BikeModelEntity, int>, BikeModelsRepository<BikeModelEntity, int>>()
                                ;
                        var objCache = container.Resolve<IBikeMaskingCacheRepository<BikeModelEntity, int>>();
                        objResponse = objCache.GetModelMaskingResponse(model);
                    }
                }



            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, Request.ServerVariables["URL"] + "ParseQueryString");
                objErr.SendMail();

                Response.Redirect("/customerror.aspx", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
                this.Page.Visible = false;
            }
            finally
            {
                // Get ModelId
                // Code to check whether masking name is changed or not. If changed redirect to appropriate url
                if (makeId > 0 && objResponse != null && cityId > 0)
                {
                    if (objResponse.StatusCode == 200)
                    {
                        modelId = objResponse.ModelId;
                    }
                    else if (objResponse.StatusCode == 301)
                    {
                        //redirect permanent to new page 
                        //CommonOpn.RedirectPermanent(Request.RawUrl.Replace(model, objResponse.MaskingName));
                        redirectUrl = Request.RawUrl.Replace(model, objResponse.MaskingName);
                        redirectPermanent = true;
                    }
                    else
                    {
                        redirectToPageNotFound = true;
                    }
                }
                else
                {
                    redirectToPageNotFound = true;
                }

            }
        }

        /// <summary>
        /// Returns City Masking Name from City Id
        /// </summary>
        /// <param name="cityId">city id</param>
        /// <returns></returns>
        public uint GetCityMaskingName(string maskingName)
        {
            ICity _city = new CityRepository();
            List<CityEntityBase> objCityList = null;
            uint _cityId = 0;
            try
            {
                objCityList = _city.GetAllCities(EnumBikeType.All);
                _cityId = objCityList.Find(c => c.CityMaskingName == maskingName).CityId;
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : GetCityMaskingName - ModelPricesInCity");
                objErr.SendMail();
            }
            return _cityId;
        }

        private void BindAlternativeBikeControl()
        {
            ctrlAlternateBikes.TopCount = 6;
            ctrlAlternateBikes.PQSourceId = (int)PQSourceEnum.Desktop_PriceInCity_Alternative;
            ctrlAlternateBikes.WidgetTitle = bikeName;
            if (firstVersion != null)
                ctrlAlternateBikes.VersionId = (int)firstVersion.VersionId;
        }
    }   // class
}
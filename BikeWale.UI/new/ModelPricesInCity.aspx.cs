using Bikewale.Cache.BikeData;
using Bikewale.Cache.Core;
using Bikewale.Common;
using Bikewale.Controls;
using Bikewale.DAL.BikeData;
using Bikewale.DAL.PriceQuote;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.PriceQuote;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.PriceQuote;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace Bikewale.New
{
    /// <summary>
    /// Created By : Ashish G. Kamble on 23 May 2016
    /// </summary>
    public class ModelPricesInCity : System.Web.UI.Page
    {
        protected ModelPriceInNearestCities ctrlTopCityPrices;
        public BikeQuotationEntity firstVersion;
        public Repeater rprVersionPrices, rpVersioNames;
        public uint modelId, cityId;
        public int versionCount;
        public string makeName = string.Empty, makeMaskingName = string.Empty, modelName = string.Empty, modelMaskingName = string.Empty, bikeName = string.Empty, modelImage = string.Empty, cityName = string.Empty, cityMaskingName = string.Empty;
        string redirectUrl = string.Empty;
        private bool redirectToPageNotFound = false, redirectPermanent = false;
        public bool isAreaAvailable;
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
                IPriceQuote objPQ; bool hasArea;
                using (IUnityContainer objPQCont = new UnityContainer())
                {
                    objPQCont.RegisterType<IPriceQuote, PriceQuoteRepository>();
                    objPQ = objPQCont.Resolve<IPriceQuote>();
                    IEnumerable<BikeQuotationEntity> bikePrices = objPQ.GetVersionPricesByModelId(modelId, cityId, out hasArea);
                    isAreaAvailable = hasArea;
                    if (bikePrices != null)
                    {
                        SetModelDetails(bikePrices);

                        rprVersionPrices.DataSource = bikePrices;
                        rprVersionPrices.DataBind();
                        rpVersioNames.DataSource = bikePrices;
                        rpVersioNames.DataBind();
                    }
                    else
                    {
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
                Response.Redirect(CommonOpn.AppPath + "pageNotFound.aspx", true);
            }
            else if (redirectPermanent)
                CommonOpn.RedirectPermanent(redirectUrl);
        }

        /// <summary>
        /// Function to get parameters from the query string.
        /// </summary>
        private void ParseQueryString()
        {
            ModelMaskingResponse objResponse = null;
            string model = string.Empty, city = string.Empty;
            try
            {
                model = Request.QueryString["model"];
                city = Request.QueryString["city"];
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

                        //modelId = objResponse.ModelId;
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
                if (objResponse != null)
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

                // Get CityId
                cityId = Convert.ToUInt32(Request.QueryString["cityid"]);

            }
        }


    }   // class
}   // namespace
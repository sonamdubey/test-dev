using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Bikewale.Cache.BikeData;
using Bikewale.Cache.Core;
using Bikewale.Common;
using Bikewale.Controls;
using Bikewale.DAL.BikeData;
using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Cache.Core;
using Microsoft.Practices.Unity;

namespace Bikewale.New
{
    /// <summary>
    /// Created By : Ashish G. Kamble on 23 May 2016
    /// </summary>
	public class ModelPricesInCity : System.Web.UI.Page
	{
        protected ModelPriceInNearestCities ctrlTopCityPrices;
        
        private uint modelId = 0, cityId = 0;
        string redirectUrl = string.Empty;                
        private bool redirectToPageNotFound = false, redirectPermanent = false;

        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }

		protected void Page_Load(object sender, EventArgs e)
		{
            ParseQueryString();

            if (redirectToPageNotFound || redirectPermanent)
            {
                DoRedirection();                
            }
            else
            {
                ctrlTopCityPrices.ModelId = modelId;
                ctrlTopCityPrices.CityId = cityId;
                ctrlTopCityPrices.TopCount = 8;
            }
		}

        /// <summary>
        /// Function to do the redirection on different pages.
        /// </summary>
        private void DoRedirection()
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
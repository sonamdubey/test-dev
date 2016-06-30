﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Bikewale.Cache.Core;
using Bikewale.Cache.PriceQuote;
using Bikewale.Entities.PriceQuote;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.PriceQuote;
using Bikewale.Notifications;
using Microsoft.Practices.Unity;
using Bikewale.Common;

namespace Bikewale.Controls
{
    /// <summary>
    /// Created By : Ashish G. Kamble on 23 May 2016
    /// Summary : Control to show the pricing of a given model for the nearest cities of a given city.
    /// </summary>
    public class ModelPriceInNearestCities : System.Web.UI.UserControl
    {
        protected Repeater rptTopCityPrices;

        public uint ModelId { get; set; }
        public uint CityId { get; set; }
        public ushort TopCount { get; set; }
        protected bool showWidget = false;
        protected string make = string.Empty;
        protected string model = string.Empty;
        public bool IsDiscontinued { get; set; }


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

            if(isValidData())
                BindNearestCityPrices();            
        }

        /// <summary>
        /// Function to validate the data passed to the widget
        /// </summary>
        /// <returns></returns>
        private bool isValidData()
        {
            bool isValid = true;

            if (ModelId <= 0 || CityId <= 0)
            {
                isValid = false;
            }

            return isValid;
        }

        /// <summary>
        /// Function binds data with repeater.
        /// </summary>
        private void BindNearestCityPrices()
        {
            try
            {
                if (TopCount <= 0) { TopCount = 8; }

                IEnumerable<PriceQuoteOfTopCities> prices = null;

                using (IUnityContainer container = new UnityContainer())
                {

                    container.RegisterType<IPriceQuote, Bikewale.BAL.PriceQuote.PriceQuote>()                        
                        .RegisterType<ICacheManager, MemcacheManager>()
                        .RegisterType<IPriceQuoteCache, PriceQuoteCache>();
                    
                    IPriceQuoteCache objCache = container.Resolve<IPriceQuoteCache>();

                    prices = objCache.GetModelPriceInNearestCities(ModelId, CityId, TopCount);

                    if (prices != null && prices.Count() > 0)
                    {
                        make = prices.First().Make;
                        model = prices.First().Model;

                        rptTopCityPrices.DataSource = prices;
                        rptTopCityPrices.DataBind();

                        showWidget = true;
                    }
                }
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }
    }
}
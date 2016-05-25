﻿using Bikewale.Cache.Core;
using Bikewale.Cache.PriceQuote;
using Bikewale.Common;
using Bikewale.Entities.PriceQuote;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.PriceQuote;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;

namespace Bikewale.Controls
{
    /// <summary>
    /// Created By : Vivek Gupta on 20-05-2016
    /// Description : Class to show top city with price           
    /// </summary>
    public class PriceInTopCities : UserControl
    {
        protected Repeater rptTopCityPrices;

        public uint ModelId { get; set; }
        public uint TopCount { get; set; }

        protected bool showWidget = false;

        protected override void OnInit(EventArgs e)
        {
            InitializeComponents();
        }

        void InitializeComponents()
        {
            this.Load += new EventHandler(Page_Load);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (isValidData())
                BindTopCityPrice();
        }

        /// <summary>
        /// Function to validate the data passed to the widget
        /// </summary>
        /// <returns></returns>
        private bool isValidData()
        {
            bool isValid = true;

            if (ModelId <= 0)
            {
                isValid = false;
            }

            return isValid;
        }

        /// <summary>
        /// Created By : Vivek Gupta on 20-05-2016
        /// Description : Function to bind top city with price           
        /// </summary>
        protected void BindTopCityPrice()
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

                    prices = objCache.FetchPriceQuoteOfTopCitiesCache(ModelId, TopCount);

                    if (prices != null && prices.Count() > 0)
                    {
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
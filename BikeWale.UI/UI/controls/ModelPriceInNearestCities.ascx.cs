using Bikewale.Cache.Core;
using Bikewale.Cache.Helper.PriceQuote;
using Bikewale.Cache.PriceQuote;
using Bikewale.Entities.PriceQuote;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.PriceQuote;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

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
        public bool showWidget = false;
        public string make = string.Empty;
        public string model = string.Empty;
        public bool IsDiscontinued { get; set; }
        public string cityName { get; set; }

        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (isValidData())
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
                        .RegisterType<IPriceQuoteCache, PriceQuoteCache>()
                        .RegisterType<IPriceQuoteCacheHelper, PriceQuoteCacheHelper>();

                    IPriceQuoteCache objCache = container.Resolve<IPriceQuoteCache>();

                    prices = objCache.GetModelPriceInNearestCities(ModelId, CityId, TopCount);

                    if (prices != null && prices.Any())
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
                Bikewale.Notifications.ErrorClass.LogError(err, Request.ServerVariables["URL"]);
                
            }
        }
    }
}
using Bikewale.Cache.Core;
using Bikewale.Cache.PriceQuote;
using Bikewale.Entities.PriceQuote;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.PriceQuote;
using Bikewale.Notifications;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Bikewale.Mobile.Controls
{
    /// <summary>
    /// Created By  : Sushil Kumar on 3rd June 2016
    /// Description : Mobile Control to show the pricing of a given model for the nearest cities of a given city
    /// </summary>
    public class ModelPriceInNearestCities : UserControl
    {
        protected Repeater rptTopCityPrices;
        public string ModelName { get; set; }
        public string CityName { get; set; }
        public uint ModelId { get; set; }
        public uint CityId { get; set; }
        public ushort TopCount { get; set; }
        public bool IsDiscontinued { get; set; }
        protected bool showWidget = false;
        protected string make = string.Empty;
        protected string model = string.Empty;
        public string MakeName { get; set; }


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
                        .RegisterType<IPriceQuoteCache, PriceQuoteCache>();

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
                ErrorClass.LogError(err, Request.ServerVariables["URL"]);
                
            }
        }
    }
}
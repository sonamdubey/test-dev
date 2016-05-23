using System;
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


        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            BindNearestCityPrices();            
        }

        /// <summary>
        /// Function binds data with repeater.
        /// </summary>
        private void BindNearestCityPrices()
        {
            try
            {
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
                        rptTopCityPrices.DataSource = prices;
                        rptTopCityPrices.DataBind();
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
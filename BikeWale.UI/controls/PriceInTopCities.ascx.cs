using Bikewale.Cache.Core;
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
        public string MakeMaskingName { get; set; }
        public string ModelMaskingName { get; set; }

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
            BindTopCityPrice();
        }

        /// <summary>
        /// Created By : Vivek Gupta on 20-05-2016
        /// Description : Function to bind top city with price           
        /// </summary>
        protected void BindTopCityPrice()
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

                    prices = objCache.FetchPriceQuoteOfTopCitiesCache(ModelId, TopCount);



                    if (prices != null)
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
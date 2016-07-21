using Bikewale.Cache.Core;
using Bikewale.Cache.DealersLocator;
using Bikewale.DAL.Dealer;
using Bikewale.Entities.Dealer;
using Bikewale.Entities.DealerLocator;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.Dealer;
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
    /// Created By : Sushil Kumar on 3rd June 2016
    /// Description : Class to show dealers 
    /// Modified by :   Sumit Kate on 17 Jun 2016
    /// Description :   Added Model Id
    /// Modified by :   Sumit Kate on 22 Jun 2016
    /// Description :   Added Repeater to bind the Popular City Dealers when city is not selected
    /// </summary>
    public class DealersCard : UserControl
    {
        protected Repeater rptDealers, rptPopularCityDealers;
        public uint MakeId { get; set; }
        public uint ModelId { get; set; }
        public ushort TopCount { get; set; }
        public uint CityId { get; set; }
        public string makeName = string.Empty, cityName = string.Empty, cityMaskingName = string.Empty, makeMaskingName = string.Empty;
        public int LeadSourceId = 27; // DealersCard GetOfferButton
        public int PQSourceId { get; set; }
        public bool IsDiscontinued { get; set; }

        public bool showWidget = false;
        protected bool isCitySelected { get { return CityId > 0; } }
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
                BindDealers();
        }

        /// <summary>
        /// Function to validate the data passed to the widget
        /// </summary>
        /// <returns></returns>
        private bool isValidData()
        {
            bool isValid = true;

            if (MakeId <= 0)
            {
                isValid = false;
            }

            return isValid;
        }

        /// <summary>
        /// Created By : Vivek Gupta on 20-05-2016
        /// Description : Function to bind dealers    
        /// Modified by :   Sumit Kate on 17 Jun 2016
        /// Description :   Pass ModelId to get the dealers for Price in city page
        /// Modified by :   Sumit Kate on 22 Jun 2016
        /// Description :   If City Id is not passed Get the popular city dealer count
        /// </summary>
        protected void BindDealers()
        {
            try
            {
                if (TopCount <= 0) { TopCount = 2; }
                DealersEntity _dealers = null;

                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IDealerCacheRepository, DealerCacheRepository>()
                             .RegisterType<ICacheManager, MemcacheManager>()
                             .RegisterType<IDealer, DealersRepository>()
                            ;
                    var objCache = container.Resolve<IDealerCacheRepository>();
                    if (isCitySelected)
                    {
                        _dealers = objCache.GetDealerByMakeCity(CityId, MakeId, ModelId);

                        if (_dealers != null && _dealers.Dealers.Count() > 0)
                        {
                            makeName = _dealers.MakeName;
                            cityName = _dealers.CityName;
                            cityMaskingName = _dealers.CityMaskingName;
                            makeMaskingName = _dealers.MakeMaskingName;

                            rptDealers.DataSource = _dealers.Dealers.Take(TopCount);
                            rptDealers.DataBind();

                            showWidget = true;
                        }
                    }
                    else
                    {
                        IEnumerable<PopularCityDealerEntity> cityDealers = objCache.GetPopularCityDealer(MakeId, TopCount);
                        if (cityDealers != null && cityDealers.Count() > 0)
                        {
                            rptPopularCityDealers.DataSource = cityDealers;
                            rptPopularCityDealers.DataBind();
                            showWidget = true;
                        }
                    }
                }
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }

        /// <summary>
        /// Created By : Sushil Kumar on 3rd June 2016 
        /// Description : link URL changed.
        /// </summary>
        /// <param name="dealerType"></param>
        /// <param name="dealerId"></param>
        /// <param name="campId"></param>
        /// <param name="dealerName"></param>
        /// <returns></returns>
        public string GetDealerDetailLink(string dealerType, string dealerId, string campId, string dealerName)
        {
            string retString = string.Empty;
            if (dealerType == "2" || dealerType == "3")
            {
                string link = "/m/new/newbikedealers/dealerdetails.aspx/?query=" + Bikewale.Utility.EncodingDecodingHelper.EncodeTo64(String.Format("dealerId={0}&campId={1}&cityId={2}", dealerId, campId, CityId));
                retString = String.Format("<a class=\"target-link margin-bottom5 text-truncate font16\" href=\"{0}\">{1}</a>", link, dealerName);
            }
            else
            {
                retString = String.Format("<a class=\"target-link margin-bottom5 text-truncate font16\" href=\"/m/{0}-bikes/dealers-in-{1}/\">{2}</a>", makeMaskingName, cityMaskingName, dealerName);
            }

            return retString;
        }

    }
}
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
        protected Repeater rptPopularCityDealers;
        public uint MakeId { get; set; }
        public uint ModelId { get; set; }
        public ushort TopCount { get; set; }
        public uint CityId { get; set; }
        public string makeName = string.Empty, cityName = string.Empty, cityMaskingName = string.Empty, makeMaskingName = string.Empty;
        public int LeadSourceId = 27; // DealersCard GetOfferButton
        public int PQSourceId { get; set; }
        public bool IsDiscontinued { get; set; }
        public string PageName { get; set; }
        public int DealerId { get; set; }
        public bool isHeadingNeeded = true;
        public IEnumerable<DealersList> dealerList { get; set; }
        public bool showWidget, showServiceCenter;
        public string dealerUrl = string.Empty;
        protected bool isCitySelected { get { return CityId > 0; } }
        public string widgetHeading { get; set; }
        public PopularDealerServiceCenter cityDealers;
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
        /// Modified By : Sajal Gupta on 27-09-2016
        /// Description : Skipped particular dealer if dealer id present.
        /// Modified by :  Subodh Jain on 21 Dec 2016
        /// Description :  merge dealer card and service center card
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
                             .RegisterType<IDealerRepository, DealersRepository>()
                            ;
                    var objCache = container.Resolve<IDealerCacheRepository>();
                    if (isCitySelected)
                    {
                        _dealers = objCache.GetDealerByMakeCity(CityId, MakeId, ModelId);

                        if (_dealers != null && _dealers.Dealers != null && _dealers.Dealers.Any())
                        {
                            makeName = _dealers.MakeName;
                            cityName = _dealers.CityName;
                            cityMaskingName = _dealers.CityMaskingName;
                            makeMaskingName = _dealers.MakeMaskingName;

                            if (DealerId > 0)
                            {
                                _dealers.Dealers = _dealers.Dealers.Where(d => d.DealerId != DealerId);
                            }
                            if (_dealers.Dealers.Any())
                            {
                                showWidget = true;

                            }
                            dealerList = _dealers.Dealers.Take(TopCount);
                        }
                    }
                    else
                    {
                        cityDealers = objCache.GetPopularCityDealer(MakeId, TopCount);
                        if (cityDealers != null)
                        {
                            if (cityDealers.TotalDealerCount > 0 || cityDealers.TotalServiceCenterCount > 0)
                                showWidget = true;
                            if (cityDealers.TotalServiceCenterCount > 0)
                                showServiceCenter = true;
                        }
                    }
                }
            }
            catch (Exception err)
            {
                ErrorClass.LogError(err, "DealersCard.BindDealers");
                
            }
        }

        /// <summary>
        /// Created By : Sushil Kumar on 3rd June 2016 
        /// Description : link URL changed.
        /// Modified by : Sajal Gupta on 28-09-2016
        /// Description : Deletede dealer type condition.
        /// </summary>
        /// <param name="dealerType"></param>
        /// <param name="dealerId"></param>
        /// <param name="campId"></param>
        /// <param name="dealerName"></param>
        /// <returns></returns>
        public string GetDealerDetailLink(string dealerType, string dealerId, string campId, string dealerName)
        {
            string retString = string.Empty;
            retString = String.Format("<div class=\"target-link margin-bottom5 text-truncate font14\" >{0}</div>", dealerName);
            return retString;
        }

    }
}
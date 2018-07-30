using Bikewale.Cache.Core;
using Bikewale.Cache.DealersLocator;
using Bikewale.Common;
using Bikewale.DAL.Dealer;
using Bikewale.Entities.Dealer;
using Bikewale.Entities.DealerLocator;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.Dealer;
using Microsoft.Practices.Unity;
using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Bikewale.Controls
{
    /// <summary>
    /// Created By : Vivek Gupta on 20-05-2016
    /// Description : Class to show dealers  
    /// Modified By : Sushil Kumar on 2nd June 2016
    /// Description : Added LeadsourceId and PQSourceId for lead and pq sources
    /// Modified By :   Sumit Kate on 17 Jun 2016
    /// Description :   Added Model ID
    /// Modified by :   Sumit Kate on 22 Jun 2016
    /// Description :   Added Repeater to bind the Popular City Dealers when city is not selected
    /// Modified by Sajal Gupta on 17-11-2016
    /// Description : Added isHeading variable for making heading of repeater optional.
    /// Modified By :-Subodh Jain on 16 Dec 2016
    /// Summary :- Added heading to dealer widget
    /// </summary>
    public class DealerCard : UserControl
    {
        protected Repeater rptDealers, rptPopularCityDealers;

        public uint MakeId { get; set; }
        public uint ModelId { get; set; }
        public ushort TopCount { get; set; }
        public uint CityId { get; set; }
        public string makeName = string.Empty, cityName = string.Empty, cityMaskingName = string.Empty, makeMaskingName = string.Empty;
        public int LeadSourceId = 25; // DealersCard GetOfferButton
        public int PQSourceId { get; set; }
        public bool IsDiscontinued { get; set; }
        protected bool isCitySelected { get { return CityId > 0; } }
        public string pageName { get; set; }
        public bool showWidget, showServiceCenter;
        public uint DealerId { get; set; }
        public bool isHeading = true;
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
            {
                BindDealers();
            }
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
        /// Modified by :  Subodh Jain on 21 Dec 2016
        /// Description :   Merge Dealer and service center for make and model page
        /// Modified by : Sushil Kumar on 11th Jan 2016
        /// Description :   Modified show widget values
        /// </summary>
        protected void BindDealers()
        {
            try
            {
                if (TopCount <= 0) { TopCount = 3; }
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

                        if (_dealers != null && _dealers.Dealers.Any())
                        {
                            makeName = _dealers.MakeName;
                            cityName = _dealers.CityName;
                            cityMaskingName = _dealers.CityMaskingName;
                            makeMaskingName = _dealers.MakeMaskingName;
                            if (DealerId > 0)
                            {
                                _dealers.Dealers = _dealers.Dealers.Where(m => m.DealerId != DealerId);
                            }

                            showWidget = _dealers.Dealers.Any();

                            if (rptDealers != null)
                            {
                                rptDealers.DataSource = _dealers.Dealers.Take(TopCount);
                                rptDealers.DataBind();
                            }

                        }
                    }
                    else
                    {
                        cityDealers = objCache.GetPopularCityDealer(MakeId, TopCount);
                        if (cityDealers != null)
                        {
                            showWidget = (cityDealers.TotalDealerCount > 0 || cityDealers.TotalServiceCenterCount > 0);
                            showServiceCenter = (cityDealers.TotalServiceCenterCount > 0);
                        }
                    }
                }

            }
            catch (Exception err)
            {
                ErrorClass.LogError(err, "Bikewale.Controls.DealerCard.BindDealers");
            }
        }
    }
}

using Bikewale.Cache.Core;
using Bikewale.Cache.DealersLocator;
using Bikewale.Common;
using Bikewale.DAL.Dealer;
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
    /// </summary>
    public class DealerCard : UserControl
    {
        protected Repeater rptDealers;

        public uint MakeId { get; set; }
        public ushort TopCount { get; set; }
        public uint CityId { get; set; }
        public string makeName = string.Empty, cityName = string.Empty, cityMaskingName = string.Empty, makeMaskingName = string.Empty;
        public int LeadSourceId = 25; // DealersCard GetOfferButton
        public int PQSourceId { get; set; }
        public bool IsDiscontinued { get; set; }

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
                BindDealers();
        }

        /// <summary>
        /// Function to validate the data passed to the widget
        /// </summary>
        /// <returns></returns>
        private bool isValidData()
        {
            bool isValid = true;

            if (MakeId <= 0 || CityId <= 0)
            {
                isValid = false;
            }

            return isValid;
        }

        /// <summary>
        /// Created By : Vivek Gupta on 20-05-2016
        /// Description : Function to bind dealers    
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
                             .RegisterType<IDealer, DealersRepository>()
                            ;
                    var objCache = container.Resolve<IDealerCacheRepository>();
                    _dealers = objCache.GetDealerByMakeCity(CityId, MakeId);

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
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }
    }
}
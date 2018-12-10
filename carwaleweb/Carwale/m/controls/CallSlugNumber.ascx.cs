using Carwale.BL.Dealers;
using Carwale.BL.PriceQuote;
using AEPLCore.Cache;
using Carwale.Cache.Dealers;
using Carwale.DAL.Dealers;
using Carwale.Entity.Dealers;
using Carwale.Interfaces;
using Carwale.Interfaces.Dealers;
using Carwale.Interfaces.PriceQuote;
using Carwale.Utility;
using Microsoft.Practices.Unity;
using MobileWeb.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MobileWeb.Controls
{
    public class CallSlugNumber : System.Web.UI.UserControl
    {
        protected string callSlugNumber = string.Empty;
        protected int dealerId = 0;
        protected bool OEMDealerFound;
        protected string dealerName = string.Empty;
        protected int makeId = 0;
        
        protected int modelId = 0;
        public int ModelId
        {
            get { return modelId; }
            set { modelId = value; }
        }

        protected int versionId = 0;
        public int VersionId
        {
            get { return versionId; }
            set { versionId = value; }
        }

        protected int cityId = 0;
        public int CityId
        {
            get { return cityId; }
            set { cityId = value; }
        }
        protected string zoneId = "";
        protected string pagename=string.Empty;
        public string Pagename
        {
            get { return pagename; }
            set { pagename = value; }
        }

        public string trackingLabel = string.Empty;

        public string Make { get; set; }
        public string Model { get; set; }
        public string Version { get; set; }
        protected bool toDisplay = true;
        public bool ToDisplay {
            get { return toDisplay; }
            set { toDisplay = value; }
        }
        public SponsoredDealer dealerDetails = new SponsoredDealer();

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
        }

        void InitializeComponent()
        {
            base.Load += new EventHandler(Page_Load);
        }

        void Page_Load(object Sender, EventArgs e)
        {
            try
            {
                callSlugNumber = dealerDetails.DealerMobile;
                OEMDealerFound = Carwale.UI.Common.DealerSponsoredCommon.OEMDealerIds(dealerDetails.ActualDealerId);
                dealerId = dealerDetails.DealerId;
                dealerName = dealerDetails.DealerName;

                GetTrackingLabel();

            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }
        public void GetTrackingLabel()
        {
            if (string.IsNullOrEmpty(Version))
                trackingLabel = (Make + "_" + Model + "_" + dealerName);
            else
                trackingLabel = (Make + "_" + Model + "_" + Version + "_" + dealerName);
        }

       
    }
}

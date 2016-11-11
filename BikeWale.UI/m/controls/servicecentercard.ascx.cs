using Bikewale.BindViewModels.Controls;
using Bikewale.Common;
using Bikewale.Entities.ServiceCenters;
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Bikewale.Mobile.Controls
{
    /// <summary>
    /// Created By :  Sangram Nandkhile on 09-Nov-2016
    /// Description : Service centers in a city card
    /// </summary>
    public class ServiceCenterCard : UserControl
    {
        protected Repeater rptDealers, rptPopularCityDealers;

        public uint MakeId { get; set; }
        public uint ModelId { get; set; }
        public ushort TopCount { get; set; }
        public uint CityId { get; set; }
        public string makeName = string.Empty, cityName = string.Empty, cityMaskingName = string.Empty, makeMaskingName = string.Empty, widgetTitle = string.Empty;
        public int LeadSourceId = 25;
        public int PQSourceId { get; set; }
        public bool IsDiscontinued { get; set; }
        protected bool isCitySelected { get { return CityId > 0; } }
        public string pageName { get; set; }
        public bool showWidget = false;
        public uint DealerId { get; set; }

        public ServiceCenterData centerData;
        public IEnumerable<ServiceCenterDetails> ServiceCenteList;

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
        /// Created By : Sangram Nandkhile on 09-Nov-2016
        /// Description : Function to bind service centers
        /// </summary>
        protected void BindDealers()
        {
            try
            {
                BindServiceCenter serviceViewModel = new BindServiceCenter();
                centerData = serviceViewModel.GetServiceCenterList((int)MakeId, CityId);
                if (centerData != null && centerData.ServiceCenters != null)
                {
                    ServiceCenteList = centerData.ServiceCenters;
                    showWidget = true;
                    widgetTitle = string.Format("Other {0} service centers in {1}", makeName, cityName);
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
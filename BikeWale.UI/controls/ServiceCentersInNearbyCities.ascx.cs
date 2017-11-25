using Bikewale.BindViewModels.Controls;
using Bikewale.Entities.ServiceCenters;
using Bikewale.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Bikewale.Controls
{
    /// <summary>
    /// Created By : Aditi Srivastava on 19 Dec 2016
    /// Summary    : Widget to show number of service centers in nearby cities
    /// </summary>
    public class ServiceCentersInNearbyCities : System.Web.UI.UserControl
    {
        protected IEnumerable<CityBrandServiceCenters> ServiceCentersNearbyCities;
        public int makeId {get;set;}
        public int cityId {get;set;}
        public int topCount { get; set; }
        public string makeName{get;set;}
        public string makeMaskingName {get;set;}
        public string cityName { get; set; }
        public int FetchedRecordsCount { get; set; }
        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            GetServiceCentersNearbyCities();
        }
        /// <summary>
        /// Created By : Aditi Srivastava on 19 Dec 2016
        /// Summary    : Function to get number of service centers in nearby cities
        /// </summary>
        private void GetServiceCentersNearbyCities()
        {
            try
            {
                BindNearbyCitiesServiceCenters servicecentViewModel = new BindNearbyCitiesServiceCenters();
                ServiceCentersNearbyCities = servicecentViewModel.GetServiceCentersNearbyCitiesByMake(cityId, makeId, topCount);
                if (ServiceCentersNearbyCities != null)
                {
                    FetchedRecordsCount = ServiceCentersNearbyCities.Count();
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "ServiceCentersInNearbyCities.GetServiceCentersNearbyCities()");
                
            }
        }

    }
}
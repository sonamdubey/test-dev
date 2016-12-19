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
        protected string WidgetTitle;
        public int makeId,cityId,topCount;
        public string makeName, makeMaskingName,cityName;
        public int FetchedRecordsCount;
        public string ClientIP { get { return Bikewale.Common.CommonOpn.GetClientIP(); } }
        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                BindNearbyCitiesServiceCenters servicecentViewModel = new BindNearbyCitiesServiceCenters();
                ServiceCentersNearbyCities = servicecentViewModel.GetServiceCentersNearbyCitiesByMake(cityId,makeId,topCount);
                if (ServiceCentersNearbyCities != null)
                {
                    FetchedRecordsCount = ServiceCentersNearbyCities.Count();
                }
                WidgetTitle = String.Format("Explore {0} Service centers in cities near {1}",makeName,cityName);
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "ServiceCentersInNearbyCities.Page_Load()");
                objErr.SendMail();
            }
        }



    }
}
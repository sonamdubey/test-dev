using Bikewale.Common;
using Bikewale.Entities.Location;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Bikewale.BikeBooking
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DeviceDetection dd = new DeviceDetection(Request.ServerVariables["HTTP_X_REWRITE_URL"].ToString());
                dd.DetectDevice();

                GetDealerCities();
            }
        }

        private void GetDealerCities()
        {
            string _apiUrl = "/api/DealerPriceQuote/getBikeBookingCities/";
            List<CityEntityBase> lstCity = null;
            try
            {
                using (Utility.BWHttpClient objClient = new Utility.BWHttpClient())
                {
                    //lstCity = objClient.GetApiResponseSync<List<BBCityBase>>(Utility.BWConfiguration.Instance.ABApiHostUrl, Utility.BWConfiguration.Instance.APIRequestTypeJSON, _apiUrl, lstCity);
                    lstCity = objClient.GetApiResponseSync<List<CityEntityBase>>(Utility.APIHost.AB, Utility.BWConfiguration.Instance.APIRequestTypeJSON, _apiUrl, lstCity);
                }

            }
            catch (Exception ex)
            {
                Bikewale.Common.ErrorClass objErr = new Bikewale.Common.ErrorClass(ex, "Bikewale.BikeBooking.GetDealerCities");
                objErr.SendMail();
            }
        } 
        
    }
}
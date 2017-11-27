using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Bikewale.UI.Entities.Insurance;
using Bikewale.Utility;
using Bikewale.Notifications;
using Bikewale.Common;

namespace Bikewale.Insurance
{
    /// <summary>
    /// Created By : Lucky Rathore
    /// Creted On : 19 Nov. 2015
    /// </summary>
    public class Default : System.Web.UI.Page
    {        
        string _applicationid = ConfigurationManager.AppSettings["applicationId"];        
        IDictionary<string, string> _headerParameters;
        public IEnumerable<CityDetail> cityList = null;
        public IEnumerable<MakeDetail> makeList = null;
                
        protected override void OnInit(EventArgs e)
        {            
            this.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
             //Adding parameter in http header for insurance API
            _headerParameters = new Dictionary<string, string>(); 
            _headerParameters.Add("clientid", "5");
            _headerParameters.Add("platformid", "2");

            // Modified By :Lucky Rathore on 12 July 2016.
            Form.Action = Request.RawUrl;
            if (!IsPostBack)
            {
                // Modified By :Ashish Kamble on 5 Feb 2016
                string originalUrl = Request.ServerVariables["HTTP_X_ORIGINAL_URL"];
                if (String.IsNullOrEmpty(originalUrl))
                    originalUrl = Request.ServerVariables["URL"];

                DeviceDetection dd = new DeviceDetection(originalUrl);
                dd.DetectDevice();

                GetCities();
                GetMakes();
            }            
        }

        /// <summary>
        /// Created By : Lucky Rathore
        /// Creted On : 19 Nov. 2015
        /// Description : Call api to get Cities list from client.
        /// </summary>
        private void GetCities()//?
        {
            try
            {   
                string apiUrl = "/api/insurance/cities/";

                using (Bikewale.Utility.BWHttpClient objClient = new Utility.BWHttpClient())
                {
                    //cityList = objClient.GetApiResponseSync<IEnumerable<CityDetail>>(Utility.BWConfiguration.Instance.CwApiHostUrl, Utility.BWConfiguration.Instance.APIRequestTypeJSON, apiUrl, cityList, _headerParameters);
                    cityList = objClient.GetApiResponseSync<IEnumerable<CityDetail>>(Utility.APIHost.CW, Utility.BWConfiguration.Instance.APIRequestTypeJSON, apiUrl, cityList, _headerParameters);
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass.LogError(ex, "Exception : Bikewale.Insurance.Default.GetCities");
                                
            }
        }

        /// <summary>
        /// Created By : Lucky Rathore
        /// Creted On : 19 Nov. 2015
        /// Description : Call api to get make list from client.
        /// </summary>
        private void GetMakes()//?
        {
            try
            {  
                string apiUrl = "/api/insurance/makes/";

                using (Bikewale.Utility.BWHttpClient objClient = new Utility.BWHttpClient())
                {
                    //makeList = objClient.GetApiResponseSync<IEnumerable<MakeDetail>>(Utility.BWConfiguration.Instance.CwApiHostUrl, Utility.BWConfiguration.Instance.APIRequestTypeJSON, apiUrl, makeList, _headerParameters);
                    makeList = objClient.GetApiResponseSync<IEnumerable<MakeDetail>>(Utility.APIHost.CW, Utility.BWConfiguration.Instance.APIRequestTypeJSON, apiUrl, makeList, _headerParameters);
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass.LogError(ex, "Exception : Bikewale.Insurance.Default.GetMakes");
                
            }
        }
               
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Bikewale.UI.Entities.Insurance;
using Bikewale.Utility;
using Bikewale.Notifications;
using System.Configuration;

namespace Bikewale.Mobile.Insurance
{
    /// <summary>
    /// Created By : Lucky Rathore
    /// Creted On : 28 Nov. 2015
    /// </summary>
    public class Default : System.Web.UI.Page
    {        
        string _applicationid = Utility.BWConfiguration.Instance.ApplicationId;
        
        IDictionary<string, string> _headerParameters;
        protected IEnumerable<CityDetail> cityList = null;
        protected IEnumerable<MakeDetail> makeList = null;
        
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

            if (!IsPostBack)
            {
                GetCities();
                GetMakes();
            }
        }

        /// <summary>
        /// Created By : Lucky Rathore
        /// Creted On : 28 Nov. 2015
        /// Description : Call api to get Cities list from client.
        /// </summary>
        private void GetCities()
        {
            try
            {
                string apiUrl = "/api/insurance/cities/";

                using (Utility.BWHttpClient objClient = new BWHttpClient())
                {
                    //cityList = objClient.GetApiResponseSync<IEnumerable<CityDetail>>(Utility.BWConfiguration.Instance.CwApiHostUrl, Utility.BWConfiguration.Instance.APIRequestTypeJSON, apiUrl, cityList, _headerParameters);
                    cityList = objClient.GetApiResponseSync<IEnumerable<CityDetail>>(Utility.APIHost.CW, Utility.BWConfiguration.Instance.APIRequestTypeJSON, apiUrl, cityList, _headerParameters);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Exception : Bikewale.Insurance.Default.GetCities");
                
            }
        }

        /// <summary>
        /// Created By : Lucky Rathore
        /// Creted On : 28 Nov. 2015
        /// Description : Call api to get make list from client.
        /// </summary>
        protected void GetMakes()
        {
            try
            {
                string apiUrl = "/api/insurance/makes/";

                using(Utility.BWHttpClient objClient = new BWHttpClient())
                {
                    //makeList = objClient.GetApiResponseSync<IEnumerable<MakeDetail>>(Utility.BWConfiguration.Instance.CwApiHostUrl, Utility.BWConfiguration.Instance.APIRequestTypeJSON, apiUrl, makeList, _headerParameters);
                    makeList = objClient.GetApiResponseSync<IEnumerable<MakeDetail>>(Utility.APIHost.CW, Utility.BWConfiguration.Instance.APIRequestTypeJSON, apiUrl, makeList, _headerParameters);
                }
                
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Exception : Bikewale.Insurance.Default.GetMakes");
                
            }
        }       

    }
}
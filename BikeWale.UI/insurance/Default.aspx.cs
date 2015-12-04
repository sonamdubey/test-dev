﻿using System;
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
        string _cwHostUrl = ConfigurationManager.AppSettings["cwApiHostUrl"];
        string _applicationid = ConfigurationManager.AppSettings["applicationId"];
        string _requestType = "application/json";
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

            if (!IsPostBack)
            {
                DeviceDetection dd = new DeviceDetection(Request.ServerVariables["HTTP_X_REWRITE_URL"].ToString());
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
                cityList = Bikewale.Utility.BWHttpClient.GetApiResponseSync<IEnumerable<CityDetail>>(_cwHostUrl, _requestType, apiUrl, cityList, _headerParameters);                
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, "Exception : Bikewale.Insurance.Default.GetCities");
                objErr.SendMail();                
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
                makeList = Bikewale.Utility.BWHttpClient.GetApiResponseSync<IEnumerable<MakeDetail>>(_cwHostUrl, _requestType, apiUrl, makeList, _headerParameters);                
            }
            catch (Exception ex)
            {
                Bikewale.Common.ErrorClass objErr = new Bikewale.Common.ErrorClass(ex, "Exception : Bikewale.Insurance.Default.GetMakes");
                objErr.SendMail();
            }
        }
               
    }
}
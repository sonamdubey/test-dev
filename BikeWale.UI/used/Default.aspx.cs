

using Bikewale.BindViewModels.Webforms.Used;
using Bikewale.Common;
using Bikewale.Controls;
using Bikewale.Entities.Location;
using Bikewale.Utility;
using System;
namespace Bikewale.Used
{
    /// <summary>
    /// Created by: Subodh Jain on 20 oct 2016
    /// Summary: Landing page for Used desktop site
    /// </summary>
    public class Default : System.Web.UI.Page
    {
        protected UsedBikeLandingPage viewModel;
        protected UsedBikeInCities ctrlusedBikeInCities;
        protected UsedBikeModel ctrlusedBikeModel;
        protected int topCount = 10;
        protected string currentUser = string.Empty;
        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
        }

        protected void InitializeComponent()
        {
            base.Load += new EventHandler(Page_Load);
        }

        /// <summary>
        /// Modified by : Sajal Gupta on 21/11/2016
        /// Desc : Added device detection
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {
            string originalUrl = Request.ServerVariables["HTTP_X_ORIGINAL_URL"];
            if (String.IsNullOrEmpty(originalUrl))
                originalUrl = Request.ServerVariables["URL"];

            Bikewale.Common.DeviceDetection dd = new Bikewale.Common.DeviceDetection(originalUrl);
            dd.DetectDevice();

            viewModel = new UsedBikeLandingPage(topCount);//topcount=number of icons to be displayed on page
            currentUser = Bikewale.Common.CurrentUser.Id;
            if (viewModel == null)
            {
                RedirectToPageNotFound();
            }
            RenderUserControls();
        }

        private void RedirectToPageNotFound()
        {
            UrlRewrite.Return404();
        }
        /// Modified By :-Subodh Jain on 15 March 2017
        /// Summary :-Made count for other used bike 9
        private void RenderUserControls()
        {
            GlobalCityAreaEntity currentCityArea = GlobalCityArea.GetGlobalCityArea();
            string _cityName = currentCityArea.City;
            if (ctrlusedBikeModel != null)
            {

                CityEntityBase cityDetails = null;

                if (currentCityArea.CityId > 0)
                {
                    cityDetails = new CityHelper().GetCityById(currentCityArea.CityId);
                    ctrlusedBikeModel.CityId = currentCityArea.CityId;
                }

                ctrlusedBikeModel.WidgetTitle = string.Format("Second Hand Bikes in {0}", currentCityArea.CityId > 0 ? _cityName : "India");
                ctrlusedBikeModel.WidgetHref = string.Format("/used/bikes-in-{0}/", cityDetails != null ? cityDetails.CityMaskingName : "india");
                ctrlusedBikeModel.TopCount = 9;
                ctrlusedBikeModel.IsLandingPage = true;
            }
            if (ctrlusedBikeInCities != null)
            {
                ctrlusedBikeInCities.WidgetHref = "/used/browse-bikes-by-cities/";
                ctrlusedBikeInCities.WidgetTitle = "Second Hand Bikes in India";

            }
        }

    }
}
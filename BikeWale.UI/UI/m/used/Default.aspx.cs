using Bikewale.BindViewModels.Webforms.Used;
using Bikewale.Common;
using Bikewale.Entities.Location;
using Bikewale.Mobile.Controls;
using Bikewale.Utility;
using System;
using System.Web;

namespace Bikewale.Mobile.Used
{
    /// <summary>
    /// Created by: Sangram Nandkhile on 06 oct 2016
    /// Summary: Landing page for Used msite
    /// </summary>
    public class Default : System.Web.UI.Page
    {
        protected UsedBikeLandingPage viewModel;
        protected UsedBikeInCities ctrlusedBikeInCities;
        protected UsedBikeModel ctrlusedBikeModel;
        protected int topCount = 6;

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
        }

        protected void InitializeComponent()
        {
            base.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            viewModel = new UsedBikeLandingPage(topCount);//topcount=number of icons to be displayed on page
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
        /// <summary>
        /// Modified by :- Subodh Jain 17 March 2017
        /// Summary :- Bind widget
        /// </summary>
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
                ctrlusedBikeModel.WidgetHref = string.Format("/m/used/bikes-in-{0}/", cityDetails != null ? cityDetails.CityMaskingName : "india");
                ctrlusedBikeModel.TopCount = 9;
                ctrlusedBikeModel.IsLandingPage = true;
            }
            if (ctrlusedBikeInCities != null)
            {
                ctrlusedBikeInCities.WidgetHref = "/m/used/browse-bikes-by-cities/";
                ctrlusedBikeInCities.WidgetTitle = "Second Hand Bikes in India";

            }
        }

    }
}
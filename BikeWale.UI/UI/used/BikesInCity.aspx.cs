using Bikewale.BindViewModels.Controls;
using Bikewale.Common;
using Bikewale.Entities.Used;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
namespace Bikewale.Used
{
    /// <summary>
    /// Created By : Ashwini Todkar on 3 April 2014
    /// </summary>

    public class BikesInCity : System.Web.UI.Page
    {
        protected Repeater rptCity;
        protected IEnumerable<UsedBikeCities> objBikeCityCountTop = null;
        protected IEnumerable<UsedBikeCities> objBikeCityCount = null;
        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
        }
        protected void InitializeComponent()
        {
            base.Load += new EventHandler(Page_Load);
        }

        /// <summary>
        /// Modified by : Sajal Gupta on 06-10-2016
        /// Desc : Added device detection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //Device Detection
            string originalUrl = Request.ServerVariables["HTTP_X_ORIGINAL_URL"];
            if (String.IsNullOrEmpty(originalUrl))
                originalUrl = Request.ServerVariables["URL"];
            DeviceDetection dd = new DeviceDetection(originalUrl);
            dd.DetectDevice();
            BindCities();
        }
        /// <summary>
        /// Created By Subodh Jain on 20 oct 2016
        /// Desc : Bind cities on City page
        /// </summary>
        private void BindCities()
        {
            try
            {
                BindUsedBikesCityWithCount objBikeCity = new BindUsedBikesCityWithCount();
                objBikeCityCount = objBikeCity.GetUsedBikeByCityWithCount();
                objBikeCityCountTop = objBikeCityCount.Where(x => x.Priority > 0); ;
                objBikeCityCount = objBikeCityCount.OrderBy(c => c.CityName);
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "BikesInCity.BindCities");
                
            }
        }
    }
}
using Bikewale.BindViewModels.Controls;
using Bikewale.Entities.Used;
using Bikewale.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
namespace Bikewale.Mobile.Controls
{
    /// <summary>
    /// Created By :- Subodh Jain 16 March 2017
    /// Summary :- usedBikes in cities widget
    /// </summary>
    public class UsedBikeInCities : System.Web.UI.UserControl
    {
        public IEnumerable<UsedBikeCities> objCitiesWithCount;
        public string WidgetTitle { get; set; }
        public string WidgetHref { get; set; }
        public bool IsLandingPage { get; set; }
        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            BindCityWidgetWithCount();

        }
        /// <summary>
        /// Created By: Subodh Jain 16 March 2017
        /// Description: Get top cities with used bike count
        /// </summary>
        /// <param name="objCitiesCache"></param>
        private void BindCityWidgetWithCount()
        {
            try
            {
                BindUsedBikesCityWithCount objBikeCity = new BindUsedBikesCityWithCount();
                objCitiesWithCount = objBikeCity.GetUsedBikeByCityWithCount();
                objCitiesWithCount = objCitiesWithCount.Where(x => x.Priority > 0);
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "usedBikeInCities.BindCityWidgetWithCount");

            }
        }
    }
}
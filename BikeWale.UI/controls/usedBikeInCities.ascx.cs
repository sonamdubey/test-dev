﻿using Bikewale.BindViewModels.Controls;
using Bikewale.Entities.Used;
using Bikewale.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
namespace Bikewale.Controls
{
    /// <summary>
    /// Created By :- Subodh Jain 16 March 2017
    /// Summary :- usedBikes in cities widget
    /// </summary>
    public class usedBikeInCities : System.Web.UI.UserControl
    {
        public IEnumerable<UsedBikeCities> objCitiesWithCount;
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
                ErrorClass objErr = new ErrorClass(ex, "usedBikeInCities.BindCityWidgetWithCount");

            }
        }
    }
}
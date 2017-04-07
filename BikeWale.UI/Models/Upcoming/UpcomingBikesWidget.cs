using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.BikeData.UpComing;
using Bikewale.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bikewale.Models
{
    /// <summary>
    /// Created by : Aditi Srivastava on 28 Mar 2017
    /// Summary    : Model for upcoming bikes widget
    /// </summary>
    public class UpcomingBikesWidget
    {
        #region Private variables
        private IUpcoming _upcoming = null;
        private EnumUpcomingBikesFilter filter = EnumUpcomingBikesFilter.Default;
        #endregion

        #region Constructor
        public UpcomingBikesWidget(IUpcoming upcoming)
        {
            _upcoming = upcoming;
        }
        #endregion

        #region Public members
        public UpcomingBikesListInputEntity Filters { get; set; }
        public EnumUpcomingBikesFilter SortBy { get; set; }
        #endregion

        #region Functions
        /// <summary>
        ///  Created by : Aditi Srivastava on 28 Mar 2017
        /// Summary     : Get list of upcoming bikes
        /// </summary>
        public UpcomingBikesWidgetVM GetData()
        {
            UpcomingBikesWidgetVM objUpcoming = new UpcomingBikesWidgetVM();
            try
            {
                objUpcoming.UpcomingBikes = _upcoming.GetModels(Filters, SortBy);
            }
            catch (Exception ex)
            {

                ErrorClass objErr = new ErrorClass(ex, "Bikewale.Models.UpcomingBikesWidget.GetData");
            }
            return objUpcoming;
        }
        #endregion
    }
}
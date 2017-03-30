using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.BikeData;
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
        private IBikeModelsCacheRepository<int> _modelsCache = null;
        private int TotalWidgetItems = 9, curPageNo = 1;
        private EnumUpcomingBikesFilter filter = EnumUpcomingBikesFilter.Default;
        #endregion

        #region Constructor
        public UpcomingBikesWidget(IBikeModelsCacheRepository<int> modelsCache)
        {
            _modelsCache = modelsCache;
        }
        #endregion

        #region Public members
        public uint? MakeId { get; set; }
        public uint? ModelId { get; set; }
        public int TopCount { get; set; }
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
                if (!MakeId.HasValue)
                    MakeId = 0;
                if (!ModelId.HasValue)
                    ModelId = 0;
                objUpcoming.UpcomingBikes = _modelsCache.GetUpcomingBikesList(filter, TotalWidgetItems, (int)MakeId,(int)ModelId,curPageNo);
                if (objUpcoming.UpcomingBikes != null && objUpcoming.UpcomingBikes.Count()>0)
                objUpcoming.UpcomingBikes = objUpcoming.UpcomingBikes.Take(TopCount);
            }
            catch (Exception ex)
            {
                ErrorClass err = new ErrorClass(ex, "Bikewale.Models.Upcoming.UpcomingBikesWidget.GetData");
            }
            return objUpcoming;
        }
        #endregion
    }
}
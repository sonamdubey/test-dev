using Bikewale.Entities.GenericBikes;
using Bikewale.Notifications;
using System;
using System.Globalization;

namespace Bikewale.Models
{
    /// <summary>
    /// Created by: Sangram Nandkhile on 25-Mar-2017
    /// summary: Widget to display best bike widget
    /// </summary>
    public class BestBikeWidgetModel
    {
        public EnumBikeBodyStyles? _currentPage { get; set; }

        public BestBikeWidgetModel(EnumBikeBodyStyles? currentPage)
        {
            _currentPage = currentPage;
        }
        /// <summary>
        /// Gets the data.
        /// </summary>
        /// <returns>
        /// Created by : Sangram Nandkhile on 24-Mar-2017 
        /// </returns>
        public BestBikeWidgetVM GetData()
        {
            BestBikeWidgetVM objData = null;
            try
            {
                objData = new BestBikeWidgetVM();
                DateTime prevMonth = DateTime.Now.AddMonths(-1);
                objData.Title = string.Format("Best bikes of {0}", string.Format("{0} {1}", prevMonth.ToString("MMMM", CultureInfo.InvariantCulture), prevMonth.Year));
            }
            catch (Exception ex)
            {
                ErrorClass er = new ErrorClass(ex, "Bikewale.Models.GetData()");
            }
            return objData;
        }
    }
}
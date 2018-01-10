using Bikewale.Entities.GenericBikes;
using Bikewale.Interfaces.BikeData;
using Bikewale.Notifications;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Bikewale.Models
{
    /// <summary>
    /// Created by: Sangram Nandkhile on 25-Mar-2017
    /// summary: Widget to display best bike widget
    /// </summary>
    public class BestBikeWidgetModel
    {
        private readonly IBikeModelsCacheRepository<int> _objBestBikes = null;
        public EnumBikeBodyStyles? _currentPage { get; set; }
        public EnumBikeBodyStyles BodyStyleType;
        public BestBikeWidgetModel(EnumBikeBodyStyles? currentPage, IBikeModelsCacheRepository<int> objBestBikes)
        {
            _currentPage = currentPage;
            _objBestBikes = objBestBikes;
        }
        /// <summary>
        /// Gets the data.
        /// </summary>
        /// <returns>
        /// Created by : Sangram Nandkhile on 24-Mar-2017 
        /// Modified By: Snehal Dange on 29th Nov 2017
        /// Description: Changed previous month in title to current month
        /// </returns>
        public BestBikeWidgetVM GetData()
        {
            BestBikeWidgetVM objData = null;
            try
            {
                objData = new BestBikeWidgetVM();
                DateTime currMonth = DateTime.Now;
                objData.objBestScootersList = FetchBestBikesList(EnumBikeBodyStyles.Scooter);
                objData.objBestBikesList = FetchBestBikesList(EnumBikeBodyStyles.AllBikes);
                objData.objBestSportsBikeList = FetchBestBikesList(EnumBikeBodyStyles.Sports);
                objData.objBestCruiserBikesList = FetchBestBikesList(EnumBikeBodyStyles.Cruiser);
                objData.objBestMileageBikesList = FetchBestBikesList(EnumBikeBodyStyles.Mileage);
                objData.Title = string.Format("Best bikes of {0}", string.Format("{0} {1}", currMonth.ToString("MMMM", CultureInfo.InvariantCulture), currMonth.Year));
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.Models.GetData()");
            }
            return objData;
        }
        /// <summary>
        /// Created By :- Subodh Jain 18 May 2017
        /// Summary :- Generic Bike Model FetchBestBikesList;
        /// </summary>
        private string FetchBestBikesList(EnumBikeBodyStyles BodyStyleType)
        {
            string BikeList = string.Empty;
            try
            {

                int topCount = 5;
                IEnumerable<BestBikeEntityBase> objBikesList = _objBestBikes.GetBestBikesByCategory(BodyStyleType);
                if (objBikesList != null)
                {
                    objBikesList = objBikesList.Reverse();
                    objBikesList = objBikesList.Take(topCount);
                    BikeList = String.Join(", ", objBikesList.Select(x => x.Model.ModelName));
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("FetchBestBikesList BodyStyle:{0}", BodyStyleType));
            }
            return BikeList;
        }
    }
}
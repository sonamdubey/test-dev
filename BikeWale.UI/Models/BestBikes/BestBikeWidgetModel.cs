﻿using Bikewale.Entities.GenericBikes;
using Bikewale.Interfaces.BikeData;
using Bikewale.Notifications;
using System.Linq;
using System;
using System.Globalization;
using System.Collections.Generic;

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
        /// </returns>
        public BestBikeWidgetVM GetData()
        {
            BestBikeWidgetVM objData = null;
            try
            {
                objData = new BestBikeWidgetVM();
                DateTime prevMonth = DateTime.Now.AddMonths(-1);
                objData.objBestScootersList=FetchBestBikesList(EnumBikeBodyStyles.Scooter);
                objData.objBestBikesList = FetchBestBikesList(EnumBikeBodyStyles.AllBikes);
                objData.objBestSportsBikeList = FetchBestBikesList(EnumBikeBodyStyles.Sports);
                objData.objBestCruiserBikesList = FetchBestBikesList(EnumBikeBodyStyles.Cruiser);
                objData.objBestMileageBikesList = FetchBestBikesList(EnumBikeBodyStyles.Mileage);
                objData.Title = string.Format("Best bikes of {0}", string.Format("{0} {1}", prevMonth.ToString("MMMM", CultureInfo.InvariantCulture), prevMonth.Year));
            }
            catch (Exception ex)
            {
                ErrorClass er = new ErrorClass(ex, "Bikewale.Models.GetData()");
            }
            return objData;
        }
        /// <summary>
        /// Created By :- Subodh Jain 18 May 2017
        /// Summary :- Generic Bike Model FetchBestBikesList;
        /// </summary>
        private string FetchBestBikesList( EnumBikeBodyStyles BodyStyleType)
        {
            string BikeList = string.Empty;
            try
            {
               
                int topCount = 3;
                IEnumerable<BestBikeEntityBase> objBikesList = _objBestBikes.GetBestBikesByCategory(BodyStyleType);
                if (objBikesList != null)
                {
                    objBikesList = objBikesList.Reverse();
                    objBikesList = objBikesList.Take(topCount);
                    BikeList = String.Join(", ", objBikesList.Select(x=>x.Model.ModelName));
                }
            }
            catch (Exception ex)
            {
             ErrorClass objErr = new ErrorClass(ex, string.Format("FetchBestBikesList BodyStyle:{0}", BodyStyleType));
            }
            return BikeList;
        }
    }
}
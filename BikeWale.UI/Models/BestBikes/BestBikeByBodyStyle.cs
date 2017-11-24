using Bikewale.Entities.GenericBikes;
using Bikewale.Interfaces.BikeData;
using Bikewale.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bikewale.Models
{
    public class BestBikeByBodyStyle
    {
        private readonly IBikeModelsCacheRepository<int> _objBestBikes = null;

        public EnumBikeBodyStyles BodyStyleType;

        public uint topCount { get; set; }
        public BestBikeByBodyStyle(IBikeModelsCacheRepository<int> objBestBikes)
        {

            _objBestBikes = objBestBikes;
        }

        /// <summary>
        /// Modified By : Snehal Dange on 24th Nov 2017
        /// Description: Added EnumBikeBodyStyles  for each category
        /// </summary>
        /// <returns></returns>
        public BestBikeByCategoryVM GetData()
        {
            BestBikeByCategoryVM objData = null;
            try
            {
                objData = new BestBikeByCategoryVM();

                objData.objBestScootersList = FetchBestBikesList(EnumBikeBodyStyles.Scooter);
                objData.objBestSportsBikeList = FetchBestBikesList(EnumBikeBodyStyles.Sports);
                objData.objBestCruiserBikesList = FetchBestBikesList(EnumBikeBodyStyles.Cruiser);
                objData.objBestMileageBikesList = FetchBestBikesList(EnumBikeBodyStyles.Mileage);

                if (objData.objBestScootersList != null && objData.objBestScootersList.Any())
                {
                    objData.objBestScootersList.First().CurrentPage = EnumBikeBodyStyles.Scooter;
                }
                if (objData.objBestSportsBikeList != null && objData.objBestSportsBikeList.Any())
                {
                    objData.objBestSportsBikeList.First().CurrentPage = EnumBikeBodyStyles.Sports;
                }
                if (objData.objBestCruiserBikesList != null && objData.objBestCruiserBikesList.Any())
                {
                    objData.objBestCruiserBikesList.First().CurrentPage = EnumBikeBodyStyles.Cruiser;
                }
                if (objData.objBestMileageBikesList != null && objData.objBestMileageBikesList.Any())
                {
                    objData.objBestMileageBikesList.First().CurrentPage = EnumBikeBodyStyles.Mileage;
                }
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
        private IEnumerable<BestBikeEntityBase> FetchBestBikesList(EnumBikeBodyStyles BodyStyleType)
        {
            IEnumerable<BestBikeEntityBase> objBikesList = null;
            try
            {


                objBikesList = _objBestBikes.GetBestBikesByCategory(BodyStyleType);

                if (objBikesList != null)
                {
                    objBikesList = objBikesList.Reverse();
                    objBikesList = objBikesList.Take((int)topCount);
                }


            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("FetchBestBikesList BodyStyle:{0}", BodyStyleType));
            }
            return objBikesList;
        }
    }
}
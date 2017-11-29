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
                    if (objBikesList.Any())
                    {
                        objBikesList.First().CurrentPage = BodyStyleType;
                    }
                }


            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("FetchBestBikesList BodyStyle:{0}", BodyStyleType));
            }
            return objBikesList;
        }
    }
}
using Bikewale.Entities.GenericBikes;
using Bikewale.Interfaces.BikeData;
using Bikewale.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bikewale.Models
{
    public class BestBikeByBodyStyle
    {
        private readonly IBikeModelsCacheRepository<int> _objBestBikes = null;
        
        public EnumBikeBodyStyles BodyStyleType;

        public uint topCount { get; set; }
        public BestBikeByBodyStyle( IBikeModelsCacheRepository<int> objBestBikes)
        {
           
            _objBestBikes = objBestBikes;
        }

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
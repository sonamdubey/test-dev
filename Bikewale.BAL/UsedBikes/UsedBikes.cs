using Bikewale.DAL.UsedBikes;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.UsedBikes;
using Bikewale.Interfaces.UsedBikes;
using Bikewale.Notifications;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;

namespace Bikewale.BAL.UsedBikes
{
    /// <summary>
    /// Created By : Sajal Gupta on 14/09/2016
    /// Description : Business logic to get used bikes for model/make page.
    /// </summary>
    public class UsedBikes : IUsedBikes
    {
        /// <summary>
        /// Created By : Sajal Gupta on 14/09/2016
        /// Description : Logic to get used bikes for model/make page.
        /// </summary>
        public IEnumerable<MostRecentBikes> GetPopularUsedBikes(uint makeId, uint modelId, uint cityId, uint totalCount)
        {
            IUsedBikesRepository usedBikesRepository = null;

            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IUsedBikesRepository, UsedBikesRepository>();
                    usedBikesRepository = container.Resolve<IUsedBikesRepository>();
                }

                if (modelId != 0)
                {
                    if (cityId != 0)
                        return usedBikesRepository.GetUsedBikesbyModelCity(modelId, cityId, totalCount);
                    else
                        return usedBikesRepository.GetUsedBikesbyModel(modelId, totalCount);
                }
                else
                {
                    if (cityId != 0)
                        return usedBikesRepository.GetUsedBikesbyMakeCity(makeId, cityId, totalCount);
                    else
                        return usedBikesRepository.GetUsedBikesbyMake(makeId, totalCount);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, String.Format("Exception in Bikewale.BAL.UsedBikes.UsedBikes.GetPopularUsedBikes parametres makeId : {0}, modelId : {1}, cityId : {2}, totalCount : {3}", makeId, modelId, cityId, totalCount));
                objErr.SendMail();
                return null;
            }
        }

        /// <summary>
        /// Created by: Sangram Nandkhile on 06th oct 2016
        /// Summary: Fetch used bikes make from DAL
        /// </summary>
        /// <returns></returns>
        public IEnumerable<UsedBikeMakeEntity> GetUsedBikeMakesWithCount()
        {
            IUsedBikesRepository usedBikesRepository = null;
            IEnumerable<UsedBikeMakeEntity> usedBikeMakes = null;
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IUsedBikesRepository, UsedBikesRepository>();
                    usedBikesRepository = container.Resolve<IUsedBikesRepository>();
                }
                usedBikeMakes = usedBikesRepository.GetUsedBikeMakesWithCount();
                return usedBikeMakes;
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception in Bikewale.BAL.UsedBikes.UsedBikes.GetUsedBikeMakesWithCount");
                objErr.SendMail();
                return null;
            }
        }
    }
}



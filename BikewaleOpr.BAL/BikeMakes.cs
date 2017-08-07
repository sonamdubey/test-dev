using BikewaleOpr.Entities.BikeData;
using Bikewale.Notifications;
using BikewaleOpr.Interface.BikeData;
using System;
using System.Collections.Generic;

namespace BikewaleOpr.BAL
{
    /// <summary>
    /// Created by : Vivek Singh Tomar on 1st Aug 2017
    /// Description : BAL Layer for Bike Makes opr functions.
    /// </summary>
    public class BikeMakes: IBikeMakes
    {
        private readonly IBikeMakesRepository _bikeMakesRepository;

        public BikeMakes(IBikeMakesRepository bikeMakesRepository)
        {
            _bikeMakesRepository = bikeMakesRepository;
        }

        /// <summary>
        /// Created by : Vivek Singh Tomar on 1st Aug 2017
        /// Description : To fetch bike models for given model id from DAL
        /// </summary>
        /// <param name="makeId"></param>
        /// <returns></returns>
        public IEnumerable<BikeModelEntityBase> GetModelsByMake(uint makeId)
        {
            IEnumerable<BikeModelEntityBase> objBikeModelEntityBaseList = null;
            try
            {
                if(makeId > 0)
                {
                    objBikeModelEntityBaseList = _bikeMakesRepository.GetModelsByMake(makeId);
                }
            }
            catch (Exception ex)
            {
               ErrorClass objErr = new ErrorClass(ex, "BikewaleOpr.BAL.BikeMakes.GetModelsByMake");
            }
            return objBikeModelEntityBaseList;
        }
    }
}
